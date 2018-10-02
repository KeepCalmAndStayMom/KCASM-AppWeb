using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using KCASM_AppWeb.Configuration;
using KCASM_AppWeb.ExtensionMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace KCASM_AppWeb.Controllers
{
    /*Controller per le azioni di utenti non loggati e per il logout*/
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            /*Controllo se l'utente è già loggato e lo ridireziono alla sua pagina principale*/
            if (HttpContext.Session.GetString("Type").SessionEquals("user"))
                return RedirectToAction("Index", "User");

            if (HttpContext.Session.GetString("Type").SessionEquals("medic") || HttpContext.Session.GetString("Type").SessionEquals("medicUser"))
                return RedirectToAction("Index", "Medic");

            /*Controllo preliminare se il server è raggiungibile tramite api*/
            try
            {
                new WebClient().DownloadString($"{Constant.API_ADDRESS}");
                ViewData["Server"] = "Success";
            }
            catch (WebException e)
            {
                Console.WriteLine(e.StackTrace);
                ViewData["Server"] = "500: Server Error. Al momento il server non è disponibile. Riprova più tardi.";
            }

            ViewData["Session"] = String.Empty;
            ViewBag.Title = "Home";
            return View();
        }
        
        [HttpPost]
        public IActionResult Index(string name, string password)
        {
            var client = new WebClient();
            try
            {
                /*Controllo se i dati di login sono corretti*/
                var content = client.DownloadString($"{Constant.API_ADDRESS}login?name={name}&password={password}");
                Dictionary<string, string> login = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);

                /*Imposto le variabili di sessione*/
                HttpContext.Session.SetString("Username", login["username"]);
                HttpContext.Session.SetString("Email", login["email"]);
                HttpContext.Session.SetString("Type", login["type"]);
                HttpContext.Session.SetString("Id", login["id"]);

                /*Ridireziono a seconda del tipo di utente loggato*/
                switch (login["type"])
                {
                    case "user": return RedirectToAction("Index", "User");
                    case "medic": return RedirectToAction("Index", "Medic");
                }
            }
            catch (WebException e)
            {
                Console.WriteLine(e);
                ViewData["result"] = "Utente non trovato";
            }

            ViewData["Server"] = "Success";
            ViewData["Session"] = String.Empty;
            return View();
        }

        [HttpPost]
        public IActionResult PasswordForgot(string name)
        {
            try
            {
                /*controllo se ho inserito uno username o email corretta presente nel database*/
                var content = new WebClient().DownloadString($"{Constant.API_ADDRESS}login_recovery?name={name}");
                Dictionary<string, string> pass_rec = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);

                /*Preparo la mail da inviare*/
                MailMessage mailMessagePlainText = new MailMessage();
                mailMessagePlainText.From = new MailAddress("kandstaymom@gmail.com", "KeepCalmAndStayMom");
                mailMessagePlainText.To.Add(new MailAddress(pass_rec["email"], pass_rec["username"]));
                mailMessagePlainText.Subject = "Recupero Password";
                mailMessagePlainText.Body = $"Gentile {pass_rec["username"]}, la password associata al tuo account è: \"{pass_rec["password"]}\"";

                /*Creo un Smtp tramite quello di gmail*/
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.Credentials = new System.Net.NetworkCredential("kandstaymom@gmail.com", "KCASM_96");
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
           
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            /*Pulisco la sessione e ridireziono alla pagina iniziale*/
            HttpContext.Session.Clear();
            ViewData["Session"] = String.Empty;
            return RedirectToAction("Index", "Home");
        }
    }
}