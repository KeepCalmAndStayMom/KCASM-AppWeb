using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
                client.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.GetEncoding("UTF-8").GetBytes(email+":"+password)));
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
        public IActionResult PasswordForgot(string email)
        {
            try
            {
                var content = new WebClient().DownloadString($"{Constant.API_ADDRESS}password_reset?email={email}");
                Dictionary<string, string> pass_rec = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);

                /*Preparo la mail da inviare*/
                MailMessage mailMessagePlainText = new MailMessage();
                mailMessagePlainText.From = new MailAddress("kandstaymom@gmail.com", "KeepCalmAndStayMom");
                mailMessagePlainText.To.Add(new MailAddress(email));
                mailMessagePlainText.Subject = "Recupero Password";
                mailMessagePlainText.Body = $"Gentile Utente, la password associata al tuo account è: \"{pass_rec["password"]}\"";

                /*Creo un Smtp tramite quello di gmail*/
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.Credentials = new NetworkCredential("kandstaymom@gmail.com", "KCASM_96");
                smtpClient.EnableSsl = true;

                /*Invio della mail*/
                try
                {
                    smtpClient.Send(mailMessagePlainText);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
            catch (WebException e)
            {
                Console.WriteLine(e.StackTrace);
            }

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}