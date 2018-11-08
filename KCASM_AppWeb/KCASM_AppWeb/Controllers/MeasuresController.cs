using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KCASM_AppWeb.ExtensionMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KCASM_AppWeb.Controllers
{
    public class MeasuresController : Controller
    {
        public IActionResult Measures()
        {
            if (!"Measures".checkSession(HttpContext.Session.GetString("Type")))
                RedirectToAction("Index", "Home");

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View();
        }

        /*possibile form / filtro per le misure immaginando tutte le misure nella stessa pagina*/
        [HttpPost]
        public IActionResult Details()
        {
            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View();
        }
    }
}