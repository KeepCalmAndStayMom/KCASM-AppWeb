using System;
using System.Net;
using KCASM_AppWeb.Configuration;
using KCASM_AppWeb.ExtensionMethods;
using KCASM_AppWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KCASM_AppWeb.Controllers
{
    /*Controller per le azioni principali del medico*/
    public class MedicController : Controller
    {
        public IActionResult Index()
        {
            /*Reimposto la sessione del medico se torno da una delle pagine dell'utente*/
            if (HttpContext.Session.GetString("Type").SessionEquals("medicUser"))
            {
                HttpContext.Session.Remove("HomestationId");
                HttpContext.Session.SetString("Type", "medic");
            }

            if (HttpContext.Session.GetString("Type").SessionNotEquals("medic"))
                return RedirectToAction("Index", "Home");

            /*Creo il modello del medico con la sua lista di pazienti*/
            Medic medic = new Medic();
            medic = medic.GetMedic(HttpContext.Session.GetString("Id"), HttpContext);
            medic = medic.GetPatients(HttpContext);
            
            ViewData["Session"] = HttpContext.Session.GetString("Type");
            ViewBag.Title = "Home";
            return View(medic);
        }

        [HttpPost]
        public IActionResult Index(string id)
        {
            /*Imposto la sessione per poter accedere come medico alle pagine del paziente scelto*/
            HttpContext.Session.SetString("HomestationId", Request.Form["Homestation_Id"].ToString());
            HttpContext.Session.SetString("Type", "medicUser");
            return RedirectToAction("Index", "User");
        }

        public IActionResult Settings()
        {
            /*Reimposto la sessione del medico se torno da una delle pagine dell'utente*/
            if (HttpContext.Session.GetString("Type").SessionEquals("medicUser"))
            {
                HttpContext.Session.Remove("HomestationId");
                HttpContext.Session.SetString("Type", "medic");
            }

            if (HttpContext.Session.GetString("Type").SessionNotEquals("medic"))
                return RedirectToAction("Index", "Home");

            /*Creo il modello del medico*/
            Medic medic = new Medic();
            medic = medic.GetMedic(HttpContext.Session.GetString("Id"), HttpContext);

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            ViewBag.Title = "Impostazioni";
            return View(medic);
        }

        [HttpPost]
        public IActionResult Settings(string username, string email, string name)
        {
            /*preparo il json da inviare alla put*/
            var old_username = HttpContext.Session.GetString("Username");
            var id = HttpContext.Session.GetString("Id");
            string body = $"{{ \"username\": \"{username}\", \"email\": \"{email}\", \"name\": \"{name}\" }}";

            /*Aggiorno i dati del medico nel database e nella sessione*/
            try
            {
                new WebClient().UploadString($"{Constant.API_ADDRESS}login_medic/{old_username}/{id}", "PUT", body);
                HttpContext.Session.SetString("Username", username);
                HttpContext.Session.SetString("Email", email);
                ViewData["Message"] = "Successo";
            }
            catch (WebException e)
            {
                /*Distinguo il tipo di errore a seconda dell'eccezione ritornata*/
                Console.WriteLine(e.StackTrace);
                if (((HttpWebResponse)e.Response).StatusCode == HttpStatusCode.Conflict)
                    ViewData["Message"] = "Username o Email già esistenti";
                else
                    ViewData["Message"] = "Errore durante la modifica. Ritenta più tardi";
            }

            /*Creo il modello del medico*/
            Medic medic = new Medic();
            medic = medic.GetMedic(HttpContext.Session.GetString("Id"), HttpContext);

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View(medic);
        }

        [HttpPost]
        public IActionResult SettingsPassword(string old_password, string new_password, string new_password2)
        {
            /*Controllo che i campi delle nuove password siano uguali*/
            if (!new_password.Equals(new_password2))
                ViewData["Password"] = "Scrivi la nuova password uguale in entrambi i campi";
            else
            {
                var username = HttpContext.Session.GetString("Username");
                try
                {
                    /*Controllo che la vecchia password sia corretta*/
                    new WebClient().DownloadString($"{Constant.API_ADDRESS}login?name={username}&password={old_password}");

                    /*Aggiorno la password nel database*/
                    string body = $"{{ \"username\": \"{username}\", \"password\": \"{new_password}\" }}";
                    new WebClient().UploadString($"{Constant.API_ADDRESS}password", "PUT", body);
                    ViewData["Password"] = "Password aggiornata!";
                }
                catch (WebException e)
                {
                    /*Distinguo il tipo di errore a seconda dell'eccezione ritornata*/
                    Console.WriteLine(e.StackTrace);
                    if (((HttpWebResponse)e.Response).StatusCode == HttpStatusCode.NotFound)
                        ViewData["Password"] = "Password non corretta. Reinserisci la tua vecchia password";
                    else
                        ViewData["Password"] = "Errore durante la modifica. Ritenta più tardi";
                }
            }

            /*Creo il modello del medico*/
            Medic medic = new Medic();
            medic = medic.GetMedic(HttpContext.Session.GetString("Id"), HttpContext);

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View("Settings", medic);
        }

        [HttpPost]
        public IActionResult TaskRemove()
        {
            if (HttpContext.Session.GetString("Type").SessionNotEquals("medic"))
                return RedirectToAction("Tasks", "User");

            /*Prendo l'id del task selezionato*/
            var id = Request.Form["Task_Id"].ToString();

            try
            {
                /*Elimino il task nel database*/
                new WebClient().UploadString($"{Constant.API_ADDRESS}task/{id}", "DELETE", "");
            }
            catch (WebException e)
            {
                Console.WriteLine(e.StackTrace);
            }

            /*Ricarico la pagina dei task*/
            return RedirectToAction("Tasks", "User");
        }

        [HttpPost]
        public IActionResult TaskAdd(string title, string description, string date)
        {
            if (HttpContext.Session.GetString("Type").SessionNotEquals("medic"))
                return RedirectToAction("Tasks", "User");

            /*Aggiungo un nuovo task nel database per l'utente che si stava visionando*/
            var homestation_id = HttpContext.Session.GetString("HomestationId");
            string body = $"{{ \"homestation_id\": {homestation_id}, \"title\": \"{title}\", \"description\": \"{description}\", \"programmed_date\": \"{date}\" }}";
            try
            {
                new WebClient().UploadString($"{Constant.API_ADDRESS}tasks", "POST", body);
            }
            catch (WebException e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return RedirectToAction("Tasks", "User");
        }

    }
}