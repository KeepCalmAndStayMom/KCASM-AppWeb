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
    public class MedicController : Controller
    {
        public IActionResult Medic()
        {
            if (!"Medic".CheckSession(HttpContext.Session.GetString("Type")))
                return RedirectToAction("Index", "Home");

            if (HttpContext.Session.GetString("Type").Equals("MedicPatient"))
                HttpContext.Session.SetString("Type", "Medic");
            
            string id = HttpContext.Session.GetString("Id");
            Medic medic = id.GetMedic().GetMedic(id.getLogin(false), id.getMedicPatients());

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View(medic);
        }

        [HttpPost]
        public IActionResult Update(string name, string surname, int age, string phone, string home_address)
        {
            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return RedirectToAction("Medic","Medic");
        }

        [HttpPost]
        public IActionResult UpdateLogin(string email, bool email_notify, bool sms_notify)
        {
            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return RedirectToAction("Medic", "Medic");
        }

        [HttpPost]
        public IActionResult UpdatePassword(string old_password, string new_password, string new_password2)
        {
            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return RedirectToAction("Medic", "Medic");
        }

        public IActionResult RedirectPatient(string patientId)
        {
            HttpContext.Session.SetString("Type", "MedicPatient");
            HttpContext.Session.SetString("PatientId", patientId);

            return RedirectToAction("Patient", "Patient");
        }
    }
}