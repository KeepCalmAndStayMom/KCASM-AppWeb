using System;
using System.Collections.Generic;
using System.Globalization;
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
            Message message;

            if (HttpContext.Session.GetString("Type").Equals("Medic"))
                message = id.GetMessage(false, "received", null).GetMessage(id.GetMedicPatients(), null);
            else
                message = id.GetMessage(true, "received", null).GetMessage(null, id.GetPatientMedics());

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View(message);
        }

        public IActionResult Chat(string filterId, string name, string surname)
        {
            if (!"Message".CheckSession(HttpContext.Session.GetString("Type")))
                return RedirectToAction("Index", "Home");

            if (HttpContext.Session.GetString("Type").Equals("MedicPatient"))
                HttpContext.Session.SetString("Type", "Medic");

            string id = HttpContext.Session.GetString("Id");
            Chat chat;

            if (HttpContext.Session.GetString("Type").Equals("Patient"))
                chat = id.GetChat(filterId, name, surname, true);
            else
                chat = id.GetChat(filterId, name, surname, false);

            ViewData["Session"] = HttpContext.Session.GetString("Type");  
            return View(chat);
        }

        public IActionResult ReadMessage(int senderId, string timedate, string name, string surname)
        {
            var id = HttpContext.Session.GetString("Id");

            string url;
            timedate = timedate.Replace(" ", "T");

            if (HttpContext.Session.GetString("Type").Equals("Medic"))
                url = $"{Constant.API_ADDRESS}medics/{id}/messages/received?timedate={timedate}&patient_id={senderId}";
            else
                url = $"{Constant.API_ADDRESS}patients/{id}/messages/received?timedate={timedate}&medic_id={senderId}";

            url.ExecuteWebUpload("PUT", "{ }");

            return RedirectToAction("Chat", "Message", new { senderId, name, surname });
        }

        [HttpPost]
        public IActionResult NewMessage(int id_receiver, string subject, string message, string name, string surname)
        {
            var id = HttpContext.Session.GetString("Id");
            var timedate = DateTime.ParseExact(DateTime.Now.ToString(), Constant.DATETIME_FORMAT, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            string body = $"{{ \"subject\": \"{subject}\", \"message\": \"{message}\", \"timedate\": \"{timedate}\", ";
            if (HttpContext.Session.GetString("Type").Equals("Medic"))
                body += $"\"patient_id\": {id_receiver} }}";
            else
                body += $"\"medic_id\": {id_receiver} }}";

            string url;
            if (HttpContext.Session.GetString("Type").Equals("Medic"))
                url = $"{Constant.API_ADDRESS}medics/{id}/messages";
            else
                url = $"{Constant.API_ADDRESS}patients/{id}/messages";

            url.ExecuteWebUpload("POST", body);

            return RedirectToAction("Chat", "Message", new { id_receiver, name, surname });
        }
    }
}