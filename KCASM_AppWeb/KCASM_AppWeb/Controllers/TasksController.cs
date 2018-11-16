using System;
using System.Collections.Generic;
using System.Linq;
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
            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return RedirectToAction("Tasks","Tasks");
        }

        [HttpPost]
        public IActionResult NewTask(string type, string date, string category, string description, bool starting_program)
        {
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