using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KCASM_AppWeb.ExtensionMethods;
using KCASM_AppWeb.Models.ForApi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KCASM_AppWeb.Controllers
{
    public class PatientController : Controller
    {
        public IActionResult Patient()
        {
            if (!"Patient".checkSession(HttpContext.Session.GetString("Type")))
                return RedirectToAction("Index", "Home");

            /*test di tutti i modelli in questa pagina*/
            //Patient patient = HttpContext.Session.GetString("Id").getPatient();

            //TaskList taskListPatientGeneral = HttpContext.Session.GetString("Id").getTasks(true, "general", null);

            //TaskList taskListPatientActivities = HttpContext.Session.GetString("Id").getTasks(true, "activities", null);

            //TaskList taskListPatientDiet = HttpContext.Session.GetString("Id").getTasks(true, "diets", null);

            //WeightsList weightsList = HttpContext.Session.GetString("Id").GetWeights(null);

            //PatientInitial patientInitial = HttpContext.Session.GetString("Id").GetPatientInitial();

            //List<Medic> medics = HttpContext.Session.GetString("Id").getPatientMedics();

            //MessageList messageListReceived = HttpContext.Session.GetString("Id").GetMessage(true, "received", null);

            //MessageList messageListSent = HttpContext.Session.GetString("Id").GetMessage(true, "sent", null);

            //MeasuresList measuresListSamplesFitbit = HttpContext.Session.GetString("Id").GetMeasures("samples", "fitbit", null, null);

            //MeasuresList measuresListSamplesHue = HttpContext.Session.GetString("Id").GetMeasures("samples", "hue", null, null);

            //MeasuresList measuresListSamplesSensor = HttpContext.Session.GetString("Id").GetMeasures("samples", "sensor", null, null);

            //MeasuresList measuresListTotalFitbit = HttpContext.Session.GetString("Id").GetMeasures("total", "fitbit", "2018-11-08", null);

            //MeasuresList measuresListTotalHue = HttpContext.Session.GetString("Id").GetMeasures("total", "hue", "2018-11-08", null);

            //MeasuresList measuresListTotalSensor = HttpContext.Session.GetString("Id").GetMeasures("total", "sensor", "2018-11-08", null);

            //Login login = HttpContext.Session.GetString("Id").getLogin(true);


            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View();
        }

        [HttpPost]
        public IActionResult Update()
        {
            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View();
        }

        public IActionResult RedirectMedic()
        {
            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View();
        }
    }
}