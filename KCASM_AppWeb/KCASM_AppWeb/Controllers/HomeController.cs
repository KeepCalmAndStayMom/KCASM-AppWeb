using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using KCASM_AppWeb.Configuration;
using KCASM_AppWeb.ExtensionMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace KCASM_AppWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Type") == null)
                HttpContext.Session.SetString("Type", "NotLogged");

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var client = new WebClient();
            string type = "NotLogged", id = null;

            try
            {

                client.Headers.Add("Authorization", "Basic " + System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("UTF-8").GetBytes(email+":"+password)));
                var content = client.UploadString($"{Constant.API_ADDRESS}login_data", "POST");
                Dictionary<string, String> login = JsonConvert.DeserializeObject<Dictionary<string, String>>(content);

                if (login.ContainsKey("patient_id"))
                {
                    type = "Patient";
                    id = login["patient_id"];
                }
                else if (login.ContainsKey("medic_id"))
                {
                    type = "Medic";
                    id = login["medic_id"];
                }

                HttpContext.Session.SetString("Type", type);
                HttpContext.Session.SetString("Id", id);

                return RedirectToAction(type, type);
            }
            catch (WebException e)
            {
                Console.WriteLine(e);
                ViewData["result"] = "Utente non trovato";
            }

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View("Index");
        }

        [HttpPost]
        public IActionResult PasswordForgot()
        {
            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}