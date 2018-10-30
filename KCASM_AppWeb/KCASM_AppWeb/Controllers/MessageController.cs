using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace KCASM_AppWeb.Controllers
{
    public class MessageController : Controller
    {
        public IActionResult Message()
        {
            return View();
        }

        public IActionResult OpenMessage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewMessage()
        {
            return View();
        }
    }
}