using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace KCASM_AppWeb.Controllers
{
    public class WeightController : Controller
    {
        public IActionResult Weight()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewWeight()
        {
            return View();
        }
    }
}