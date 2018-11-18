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
    public class WeightController : Controller
    {
        public IActionResult Weight()
        {
            if (!"Weight".CheckSession(HttpContext.Session.GetString("Type")))
                return RedirectToAction("Index", "Home");

            string id;
            if (HttpContext.Session.GetString("Type").Equals("MedicPatient"))
                id = HttpContext.Session.GetString("PatientId");
            else
                id = HttpContext.Session.GetString("Id");

            Weights weights = id.GetWeights(null).GetWeights(id.GetPatientInitial(), id.GetThreshold());

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View(weights);
        }

        [HttpPost]
        public IActionResult NewWeight(double weight)
        {
            var id = HttpContext.Session.GetString("Id");
            var date = DateTime.Today;
            string body = $"{{ \"weight\": \"{weight}\", \"date\": \"{date}\" }}";

            try
            {
                WebClient client = new WebClient();
                client.Headers.Add("Content-Type", "application/json");
                client.UploadString($"{Constant.API_ADDRESS}patients/{id}/weights", "POST", body);
                ViewData["Message"] = "Successo";
            }
            catch (WebException e)
            {
                Console.WriteLine(e.StackTrace);
                ViewData["Message"] = "Errore durante la modifica. Ritenta più tardi";
            }

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return RedirectToAction("Weight","Weight");
        }
    }
}