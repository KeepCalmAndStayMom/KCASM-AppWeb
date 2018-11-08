using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KCASM_AppWeb.ExtensionMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KCASM_AppWeb.Controllers
{
    public class MessageController : Controller
    {
        public IActionResult Message()
        {
            if (!"Message".checkSession(HttpContext.Session.GetString("Type")))
                return RedirectToAction("Index", "Home");

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View();
        }

        public IActionResult OpenMessage()
        {
            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View();
        }

        [HttpPost]
        public IActionResult NewMessage()
        {
            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View();
        }
    }
}