using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace KCASM_AppWeb.Controllers
{
    public class TasksController : Controller
    {
        public IActionResult Task()
        {
            return View();
        }


        /*Vedere se possibile tramite comandi diretti nello scheduler o form apposite a parte*/
        [HttpPost]
        public IActionResult Update()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewTask()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Delete()
        {
            return View();
        }
    }
}