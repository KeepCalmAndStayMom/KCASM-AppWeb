using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using KCASM_AppWeb.Configuration;
using KCASM_AppWeb.ExtensionMethods;
using KCASM_AppWeb.Models.ForView;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KCASM_AppWeb.Controllers
{
    public class TasksController : Controller
    {
        public IActionResult Tasks()
        {
            if (!"Tasks".CheckSession(HttpContext.Session.GetString("Type")))
                return RedirectToAction("Index", "Home");

            string id;
            if (HttpContext.Session.GetString("Type").Equals("MedicPatient"))
                id = HttpContext.Session.GetString("PatientId");
            else
                id = HttpContext.Session.GetString("Id");

            Tasks tasks = id.getTasks(true, "general", null).GetTask(id.getTasks(true, "activities", null), id.getTasks(true, "diets", null));

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View(tasks);
        }


        /*Vedere se possibile tramite comandi diretti nello scheduler o form apposite a parte*/
        [HttpPost]
        public IActionResult UpdateExecuted(int id, string type, bool executed)
        {
            var patient_id = HttpContext.Session.GetString("Id");
            string body = $"{{ \"executed\": {executed} }}";

            try
            {
                switch (type)
                {
                    case "fitbit": new WebClient().UploadString($"{Constant.API_ADDRESS}patients/{patient_id}/tasks/fitbit/{id}", "PUT", body); break;
                    case "hue": new WebClient().UploadString($"{Constant.API_ADDRESS}patients/{id}/tasks/hue/{id}", "PUT", body); break;
                    case "sensor": new WebClient().UploadString($"{Constant.API_ADDRESS}patients/{id}/tasks/sensor/{id}", "PUT", body); break;
                }
                ViewData["Message"] = "Successo";
            }
            catch (WebException e)
            {
                Console.WriteLine(e.StackTrace);
                ViewData["Message"] = "Errore durante la modifica. Ritenta più tardi";
            }

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return RedirectToAction("Patient", "Patient");

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return RedirectToAction("Tasks","Tasks");
        }

        [HttpPost]
        public IActionResult NewTask(string type, string date, string category, string description, bool starting_program)
        {
            var id = HttpContext.Session.GetString("Id");
            string body = $"{{ \"category\": \"{category}\", \"date\": \"{date}\", \"description\": \"{description}\", \"starting_program\": {starting_program} }}";

            try
            {
                switch(type)
                {
                    case "fitbit": new WebClient().UploadString($"{Constant.API_ADDRESS}patients/{id}/tasks/fitbit", "POST", body); break;
                    case "hue": new WebClient().UploadString($"{Constant.API_ADDRESS}patients/{id}/tasks/hue", "POST", body); break;
                    case "sensor": new WebClient().UploadString($"{Constant.API_ADDRESS}patients/{id}/tasks/sensor", "POST", body); break;
                }
                
                ViewData["Message"] = "Successo";
            }
            catch (WebException e)
            {
                Console.WriteLine(e.StackTrace);
                ViewData["Message"] = "Errore durante la modifica. Ritenta più tardi";
            }

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return RedirectToAction("Tasks", "Tasks");
        }

        [HttpPost]
        public IActionResult Delete(int id, string type)
        {
            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return RedirectToAction("Tasks", "Tasks");
        }
    }
}