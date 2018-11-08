using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KCASM_AppWeb.ExtensionMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KCASM_AppWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Type") == null)
                HttpContext.Session.SetString("Type", "NotLogged");

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View();
        }

        [HttpPost]
        public IActionResult Login()
        {

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View();
        }

        [HttpPost]
        public IActionResult PasswordForgot()
        {
            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View();
        }
    }
}