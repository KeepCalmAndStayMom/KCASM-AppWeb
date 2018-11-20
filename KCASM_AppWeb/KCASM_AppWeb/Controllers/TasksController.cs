﻿using System;
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
            Tasks tasks;
            ViewData["Session"] = HttpContext.Session.GetString("Type");

            if (HttpContext.Session.GetString("Type").Equals("MedicPatient"))
            {
                id = HttpContext.Session.GetString("PatientId");
                tasks = id.GetTasks(true, "general", null).GetTask(id.GetTasks(true, "activities", null), id.GetTasks(true, "diets", null));
                return View("TasksMedic", tasks);
            }
            else
            {
                id = HttpContext.Session.GetString("Id");
                tasks = id.GetTasks(true, "general", null).GetTask(id.GetTasks(true, "activities", null), id.GetTasks(true, "diets", null));
                return View("TasksPatient", tasks);
            }
            
        }

        public IActionResult UpdateExecuted(int id, string type, bool executed)
        {
            var patient_id = HttpContext.Session.GetString("Id");
            string body = $"{{ \"executed\": {executed} }}";
            string url = $"{Constant.API_ADDRESS}patients/{patient_id}/tasks/{type}/{id}";

            url.ExecuteWebUpload("PUT", body);

            return RedirectToAction("Tasks","Tasks");
        }

        [HttpPost]
        public IActionResult UpdateAll(int id, string type, string date, string category, string description, bool starting_program)
        {
            var patient_id = HttpContext.Session.GetString("PatientId");
            var medic_id = HttpContext.Session.GetString("Id");
            string body = $"{{ \"category\": \"{category}\", \"date\": \"{date}\", \"description\": \"{description}\", \"starting_program\": {starting_program} }}";
            string url = $"{Constant.API_ADDRESS}medics/{medic_id}/tasks/{type}/{id}";

            url.ExecuteWebUpload("PUT", body);

            return RedirectToAction("Tasks", "Tasks");
        }

        [HttpPost]
        public IActionResult NewTask(string type, string date, string category, string description, bool starting_program)
        {
            var id = HttpContext.Session.GetString("Id");
            var patient_id = HttpContext.Session.GetString("PatientId");
            string body = $"{{ \"patient_id\": {patient_id}, \"category\": \"{category}\", \"date\": \"{date}\", \"description\": \"{description}\", \"starting_program\": {starting_program} }}";
            string url = $"{Constant.API_ADDRESS}medics/{id}/tasks/{type}";

            url.ExecuteWebUpload("POST", body);

            return RedirectToAction("Tasks", "Tasks");
        }

        public IActionResult DeleteTask(int id, string type)
        {
            var medic_id = HttpContext.Session.GetString("Id");
            string url = $"{Constant.API_ADDRESS}patients/{medic_id}/tasks/{type}/{id}";

            url.ExecuteWebUpload("DELETE", "{ }");

            return RedirectToAction("Tasks", "Tasks");
        }
    }
}