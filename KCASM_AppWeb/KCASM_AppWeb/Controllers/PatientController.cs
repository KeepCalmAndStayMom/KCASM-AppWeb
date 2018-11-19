using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using KCASM_AppWeb.Configuration;
using KCASM_AppWeb.ExtensionMethods;
using KCASM_AppWeb.Models.ForView;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KCASM_AppWeb.Controllers
{
    public class PatientController : Controller
    {
        public IActionResult Patient()
        {
            if (!"Patient".CheckSession(HttpContext.Session.GetString("Type")))
                return RedirectToAction("Index", "Home");

            string id;
            if (HttpContext.Session.GetString("Type").Equals("MedicPatient"))
                id = HttpContext.Session.GetString("PatientId");
            else
                id = HttpContext.Session.GetString("Id");

            Patient patient = id.GetPatient().GetPatient(id.GetPatientInitial(), id.GetLogin(true), id.GetPatientMedics());

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View(patient);
        }

        [HttpPost]
        public IActionResult Update(string name, string surname, int age, string phone, string home_address, string hospital_address, bool email_notify, bool sms_notify)
        {
            var id = HttpContext.Session.GetString("Id");
            string body = $"{{ \"name\": \"{name}\", \"surname\": \"{surname}\", \"age\": {age}, \"phone\": \"{phone}\", \"address_home\": \"{home_address}\", \"address_hospital\": \"{hospital_address}\", \"email_notify\": {email_notify}, \"sms_notify\": {sms_notify} }}";
            string url = $"{Constant.API_ADDRESS}patients/{id}";

            url.ExecuteWebUpload("PUT", body);

            return RedirectToAction("Patient", "Patient");
        }

        [HttpPost]
        public IActionResult UpdatePassword(string email, string old_password, string new_password, string new_password2)
        {
            var id = HttpContext.Session.GetString("Id");
            if(id.GetLogin(true).Password.Equals(old_password))
            {
                if (new_password == null && new_password2 == null)
                {
                    string body = $"{{ \"email\": \"{email}\", \"password\": \"{old_password}\" }}";
                    string url = $"{Constant.API_ADDRESS}patients/{id}/login_data";
                    url.ExecuteWebUpload("PUT", body);
                }
                else
                    if (new_password != null && new_password2 != null)
                        if (new_password.Equals(new_password2))
                        {
                            string body = $"{{ \"email\": \"{email}\", \"password\": \"{new_password}\" }}";
                            string url = $"{Constant.API_ADDRESS}patients/{id}/login_data";
                            url.ExecuteWebUpload("PUT", body);
                        }
            }

            return RedirectToAction("Patient", "Patient");
        }

        [HttpPost]
        public IActionResult UpdateInitial(bool twin)
        {
            var id = HttpContext.Session.GetString("PatientId");
            string body = $"{{ \"twin\": {twin} }}";
            string url = $"{Constant.API_ADDRESS}patients/{id}/initial_data";
            url.ExecuteWebUpload("PUT", body);

            return RedirectToAction("Patient", "Patient");
        }

    }
}