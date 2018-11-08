using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KCASM_AppWeb.ExtensionMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KCASM_AppWeb.Controllers
{
    public class TasksController : Controller
    {
        public IActionResult Tasks()
        {
            if (!"Tasks".checkSession(HttpContext.Session.GetString("Type")))
                return RedirectToAction("Index", "Home");

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View();
        }


        /*Vedere se possibile tramite comandi diretti nello scheduler o form apposite a parte*/
        [HttpPost]
        public IActionResult Update()
        {
            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View();
        }

        [HttpPost]
        public IActionResult NewTask()
        {
            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View();
        }

        [HttpPost]
        public IActionResult Delete()
        {
            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View();
        }
    }
}