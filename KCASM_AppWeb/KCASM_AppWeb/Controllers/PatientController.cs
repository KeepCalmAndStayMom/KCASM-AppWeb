using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KCASM_AppWeb.ExtensionMethods;
using KCASM_AppWeb.Models.ForApi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KCASM_AppWeb.Controllers
{
    public class PatientController : Controller
    {
        public IActionResult Patient()
        {
            if (!"Patient".checkSession(HttpContext.Session.GetString("Type")))
                return RedirectToAction("Index", "Home");

            Patient patient = HttpContext.Session.GetString("Id").getPatient();

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View(patient);
        }

        [HttpPost]
        public IActionResult Update()
        {
            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View();
        }

        public IActionResult RedirectMedic()
        {
            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View();
        }
    }
}