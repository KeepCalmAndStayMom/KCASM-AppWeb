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
    public class MessageController : Controller
    {
        public IActionResult Message()
        {
            if (!"Message".CheckSession(HttpContext.Session.GetString("Type")))
                return RedirectToAction("Index", "Home");

            if (HttpContext.Session.GetString("Type").Equals("MedicPatient"))
                HttpContext.Session.SetString("Type", "Medic");

            string id = HttpContext.Session.GetString("Id");
            Message message = id.GetMessage(false, "received", null).GetMessage(id.GetMessage(false, "sent", null));

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View(message);
        }

        public IActionResult ReadMessage(int id)
        {
            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return RedirectToAction("Message", "Message");
        }

        [HttpPost]
        public IActionResult NewMessage(int id_receiver, string subject, string message)
        {
            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return RedirectToAction("Message", "Message");
        }
    }
}