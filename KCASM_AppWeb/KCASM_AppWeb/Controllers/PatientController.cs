using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public IActionResult Update(string name, string surname, int age, string phone, string home_address, string hospital_address)
        {
            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return RedirectToAction("Patient", "Patient");
        }

        [HttpPost]
        public IActionResult UpdateLogin(string email, bool email_notify, bool sms_notify)
        {
            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return RedirectToAction("Patient", "Patient");
        }

        [HttpPost]
        public IActionResult UpdatePassword(string old_password, string new_password, string new_password2)
        {
            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return RedirectToAction("Patient", "Patient");
        }

        [HttpPost]
        public IActionResult UpdateInitial(bool twin)
        {
            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return RedirectToAction("Patient", "Patient");
        }

    }
}