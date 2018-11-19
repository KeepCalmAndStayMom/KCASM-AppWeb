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

            if(HttpContext.Session.GetString("Type").Equals("Medic"))
                message = id.GetMessage(false, "received", null).GetMessage(id.GetMessage(false, "sent", null), id.GetMedicPatients(), null);
            else
                message = id.GetMessage(true, "received", null).GetMessage(id.GetMessage(true, "sent", null), null, id.GetPatientMedics());

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View(message);
        }

        public IActionResult ReadMessage(int senderId, string timedate)
        {
            var id = HttpContext.Session.GetString("Id");

            string url;
            timedate = timedate.Replace(" ", "T");

            if (HttpContext.Session.GetString("Type").Equals("Medic"))
                url = $"{Constant.API_ADDRESS}medics/{id}/messages/received?timedate={timedate}&patient_id={senderId}";
            else
                url = $"{Constant.API_ADDRESS}patients/{id}/messages/received?timedate={timedate}&medic_id={senderId}";

            url.ExecuteWebUpload("PUT", null);

            return RedirectToAction("Message", "Message");
        }

        [HttpPost]
        public IActionResult NewMessage(int id_receiver, string subject, string message)
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

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return RedirectToAction("Message", "Message");
        }
    }
}