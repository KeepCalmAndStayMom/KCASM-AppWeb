using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KCASM_AppWeb.ExtensionMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KCASM_AppWeb.Controllers
{
    public class MedicController : Controller
    {
        public IActionResult Medic()
        {
            if (!"Medic".checkSession(HttpContext.Session.GetString("Type")))
                return RedirectToAction("Index", "Home");

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View();
        }

        [HttpPost]
        public IActionResult Update()
        {
            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View();
        }

        public IActionResult RedirectPatient()
        {
            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View();
        }
    }
}