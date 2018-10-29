using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace KCASM_AppWeb.Controllers
{
    public class MedicController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}