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

        public IActionResult ReadMessage(int senderId, string timedate)
        {
            var id = HttpContext.Session.GetString("Id");

            try
            {
                if (HttpContext.Session.GetString("Type").Equals("Medic"))
                    new WebClient().UploadString($"{Constant.API_ADDRESS}medics/{id}?timedate={timedate}&medic_id={id}&patient_id={senderId}", "PUT", null);
                else
                    new WebClient().UploadString($"{Constant.API_ADDRESS}patients/{id}?timedate={timedate}&patient_id={id}&medic_id={senderId}", "PUT", null);
                ViewData["Message"] = "Successo";
            }
            catch (WebException e)
            {
                Console.WriteLine(e.StackTrace);
                ViewData["Message"] = "Errore durante la modifica. Ritenta più tardi";
            }

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return RedirectToAction("Message", "Message");
        }

        [HttpPost]
        public IActionResult NewMessage(int id_receiver, string subject, string message)
        {

            var id = HttpContext.Session.GetString("Id");
            var timedate = DateTime.Today;
            string body = $"{{ \"subject\": \"{subject}\", \"messagge\": \"{message}\", \"timedate\": \"{timedate}\", ";
            if (HttpContext.Session.GetString("Type").Equals("Medic"))
                body += $"\"patient_id\": {id_receiver} }}";
            else
                body += $"\"medic_id\": {id_receiver} }}";

            try
            {
                if (HttpContext.Session.GetString("Type").Equals("Medic"))
                    new WebClient().UploadString($"{Constant.API_ADDRESS}medics/{id}/messages", "POST", body);
                else
                    new WebClient().UploadString($"{Constant.API_ADDRESS}patients/{id}/messages", "POST", body);
                ViewData["Message"] = "Successo";
            }
            catch (WebException e)
            {
                Console.WriteLine(e.StackTrace);
                ViewData["Message"] = "Errore durante la modifica. Ritenta più tardi";
            }

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return RedirectToAction("Message", "Message");
        }
    }
}