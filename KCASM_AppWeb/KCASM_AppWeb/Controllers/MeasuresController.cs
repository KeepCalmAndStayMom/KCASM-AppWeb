using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace KCASM_AppWeb.Controllers
{
    public class MeasuresController : Controller
    {
        public IActionResult Measures()
        {
            return View();
        }

        /*possibile form / filtro per le misure immaginando tutte le misure nella stessa pagina*/
        [HttpPost]
        public IActionResult Details()
        {
            return View();
        }
    }
}