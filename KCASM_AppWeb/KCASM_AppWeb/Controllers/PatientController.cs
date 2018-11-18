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

            Patient patient = id.getPatient().GetPatient(id.GetPatientInitial(), id.getLogin(true), id.getPatientMedics());

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View(patient);
        }

        [HttpPost]
        public IActionResult Update(string name, string surname, int age, string phone, string home_address, string hospital_address, bool email_notify, bool sms_notify)
        {
            var id = HttpContext.Session.GetString("Id");
            string body = $"{{ \"name\": \"{name}\", \"surname\": \"{surname}\", \"age\": {age}, \"phone\": \"{phone}\", \"address_home\": \"{home_address}\", \"address_hospital\": \"{hospital_address}\", \"email_notify\": {email_notify}, \"sms_notify\": {sms_notify} }}";

            try
            {
                WebClient client = new WebClient();
                client.Headers.Add("Content-Type", "application/json");
                client.UploadString($"{Constant.API_ADDRESS}patients/{id}", "PUT", body);
                ViewData["Message"] = "Successo";
            }
            catch (WebException e)
            {
                Console.WriteLine(e.StackTrace);
                ViewData["Message"] = "Errore durante la modifica. Ritenta più tardi";
            }

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return RedirectToAction("Patient", "Patient");
        }

        [HttpPost]
        public IActionResult UpdatePassword(string email, string old_password, string new_password, string new_password2)
        {
            var id = HttpContext.Session.GetString("Id");
            if(id.getLogin(true).Password.Equals(old_password))
            {
                if (new_password == null && new_password2 == null)
                {
                    try
                    {
                        string body = $"{{ \"email\": \"{email}\", \"password\": \"{old_password}\" }}";
                        WebClient client = new WebClient();
                        client.Headers.Add("Content-Type", "application/json");
                        client.UploadString($"{Constant.API_ADDRESS}patients/{id}/login_data", "PUT", body);
                    }
                    catch (WebException e)
                    {
                        Console.WriteLine(e);
                        ViewData["Message"] = "Errore durante la modifica. Ritenta più tardi";
                    }
                }
                else
                {
                    if (new_password != null && new_password2 != null)
                    {
                        if (new_password.Equals(new_password2))
                        {
                            try
                            {
                                string body = $"{{ \"email\": \"{email}\", \"password\": \"{new_password}\" }}";
                                WebClient client = new WebClient();
                                client.Headers.Add("Content-Type", "application/json");
                                client.UploadString($"{Constant.API_ADDRESS}patients/{id}", "PUT", body);
                            }
                            catch (WebException e)
                            {
                                Console.WriteLine(e);
                                ViewData["Message"] = "Errore durante la modifica. Ritenta più tardi";
                            }
                        }
                        else
                            ViewData["Message"] = "Password non coincidono";
                    }
                    else
                        ViewData["Message"] = "Password non coincidono";
                }
            }
            else
                ViewData["Message"] = "Password non corretta";

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return RedirectToAction("Patient", "Patient");
        }

        [HttpPost]
        public IActionResult UpdateInitial(bool twin)
        {
            var id = HttpContext.Session.GetString("PatientId");
            string body = $"{{ \"twin\": {twin} }}";

            try
            {
                WebClient client = new WebClient();
                client.Headers.Add("Content-Type", "application/json");
                client.UploadString($"{Constant.API_ADDRESS}patients/{id}/initial_data", "PUT", body);
                ViewData["Message"] = "Successo";
            }
            catch (WebException e)
            {
                Console.WriteLine(e.StackTrace);
                ViewData["Message"] = "Errore durante la modifica. Ritenta più tardi";
            }

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return RedirectToAction("Patient", "Patient");
        }

    }
}