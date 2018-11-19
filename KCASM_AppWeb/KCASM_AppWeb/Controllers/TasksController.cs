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

            Tasks tasks = id.GetTasks(true, "general", null).GetTask(id.GetTasks(true, "activities", null), id.GetTasks(true, "diets", null));

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
                WebClient client = new WebClient();
                client.Headers.Add("Content-Type", "application/json");
                switch (type)
                {
                    case "activities": client.UploadString($"{Constant.API_ADDRESS}patients/{patient_id}/tasks/activities/{id}", "PUT", body); break;
                    case "general": client.UploadString($"{Constant.API_ADDRESS}patients/{patient_id}/tasks/general/{id}", "PUT", body); break;
                    case "diets": client.UploadString($"{Constant.API_ADDRESS}patients/{patient_id}/tasks/diets/{id}", "PUT", body); break;
                }
                ViewData["Message"] = "Successo";
            }
            catch (WebException e)
            {
                Console.WriteLine(e.StackTrace);
                ViewData["Message"] = "Errore durante la modifica. Ritenta più tardi";
            }

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return RedirectToAction("Tasks","Tasks");
        }

        [HttpPost]
        public IActionResult UpdateAll(int id, string type, string date, string category, string description, bool starting_program)
        {
            var patient_id = HttpContext.Session.GetString("PatientId");
            var medic_id = HttpContext.Session.GetString("Id");
            string body = $"{{ \"category\": \"{category}\", \"date\": \"{date}\", \"description\": \"{description}\", \"starting_program\": {starting_program} }}";


            try
            {
                WebClient client = new WebClient();
                client.Headers.Add("Content-Type", "application/json");
                switch (type)
                {
                    case "activities": client.UploadString($"{Constant.API_ADDRESS}medics/{medic_id}/tasks/activities/{id}", "PUT", body); break;
                    case "general": client.UploadString($"{Constant.API_ADDRESS}medics/{medic_id}/tasks/general/{id}", "PUT", body); break;
                    case "diets": client.UploadString($"{Constant.API_ADDRESS}medics/{medic_id}/tasks/diets/{id}", "PUT", body); break;
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
        public IActionResult NewTask(string type, string date, string category, string description, bool starting_program)
        {
            var id = HttpContext.Session.GetString("Id");
            var patient_id = HttpContext.Session.GetString("PatientId");
            string body = $"{{ \"patient_id\": {patient_id}, \"category\": \"{category}\", \"date\": \"{date}\", \"description\": \"{description}\", \"starting_program\": {starting_program} }}";

            try
            {
                WebClient client = new WebClient();
                client.Headers.Add("Content-Type", "application/json");
                switch (type)
                {
                    case "activities": client.UploadString($"{Constant.API_ADDRESS}medics/{id}/tasks/activities", "POST", body); break;
                    case "general": client.UploadString($"{Constant.API_ADDRESS}medics/{id}/tasks/general", "POST", body); break;
                    case "diets": client.UploadString($"{Constant.API_ADDRESS}medics/{id}/tasks/diets", "POST", body); break;
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
            var medic_id = HttpContext.Session.GetString("Id");

            try
            {
                WebClient client = new WebClient();
                client.Headers.Add("Content-Type", "application/json");
                switch (type)
                {
                    case "activities": client.UploadString($"{Constant.API_ADDRESS}medics/{medic_id}/tasks/activities/{id}", "DELETE", null); break;
                    case "general": client.UploadString($"{Constant.API_ADDRESS}medics/{medic_id}/tasks/general/{id}", "DELETE", null); break;
                    case "diets": client.UploadString($"{Constant.API_ADDRESS}medics/{medic_id}/tasks/diets/{id}", "DELETE", null); break;
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
    }
}