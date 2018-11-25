using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KCASM_AppWeb.Configuration;
using KCASM_AppWeb.ExtensionMethods;
using KCASM_AppWeb.Models.ForView;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KCASM_AppWeb.Controllers
{
    public class MeasuresController : Controller
    {
        public IActionResult Measures()
        {
            if (!"Measures".CheckSession(HttpContext.Session.GetString("Type")))
                return RedirectToAction("Index", "Home");

            string id;
            if (HttpContext.Session.GetString("Type").Equals("MedicPatient"))
                id = HttpContext.Session.GetString("PatientId");
            else
                id = HttpContext.Session.GetString("Id");
            
            DateTime endDate = DateTime.Today;
            //DateTime startDate = endDate.AddDays(-Constant.DATE_LIMIT_TOTAL);

            DateTime startDate = endDate.AddDays(2);

            Measures measures = id.GetMeasuresTotal(startDate, endDate);

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View(measures);
        }

        /*possibile form / filtro per le misure immaginando tutte le misure nella stessa pagina*/
        [HttpPost]
        public IActionResult Measures(string type, string startDate, string endDate)
        {
            string id;
            if (HttpContext.Session.GetString("Type").Equals("MedicPatient"))
                id = HttpContext.Session.GetString("PatientId");
            else
                id = HttpContext.Session.GetString("Id");

            DateTime start = DateTime.Parse(startDate);
            DateTime end = DateTime.Parse(endDate);
            Measures measures = null;

            if (type.Equals("Total"))
                measures = id.GetMeasuresTotal(start, end);
            else
                measures = id.GetMeasuresSamples(start, end); 

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View(measures);
        }
    }
}