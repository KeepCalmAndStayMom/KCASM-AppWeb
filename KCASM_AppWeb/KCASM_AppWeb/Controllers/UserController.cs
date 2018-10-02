using System;
using System.Net;
using KCASM_AppWeb.Configuration;
using KCASM_AppWeb.ExtensionMethods;
using KCASM_AppWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KCASM_AppWeb.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            /*Controllo se è loggato un utente o un medico abilitato*/
            if (HttpContext.Session.GetString("Type").SessionNotEquals("user") && HttpContext.Session.GetString("Type").SessionNotEquals("medicUser"))
                return RedirectToAction("Index", "Home");

            /*Creo il modello dell'utente con tutti i suoi dati di base*/
            User user = new User();
            user = user.GetUserIndex(SetHomestationID(), HttpContext);

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            ViewBag.Title = "Home";
            return View(user);
        }

        public IActionResult Details()
        {
            /*Controllo se è loggato un utente o un medico abilitato*/
            if (HttpContext.Session.GetString("Type").SessionNotEquals("user") && HttpContext.Session.GetString("Type").SessionNotEquals("medicUser"))
                return RedirectToAction("Index", "Home");

            /*Creo il modello dell'utente*/
            User user = new User();
            user = user.GetUser(SetHomestationID(), HttpContext);

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            ViewBag.Title = "Dettagli";
            return View(user);
        }

        [HttpPost]
        public IActionResult Details(string mode, string start_date, string end_date)
        {
            /*Prendo le date*/
            DateTime date_start = DateTime.Parse(start_date);
            DateTime date_end = DateTime.Parse(end_date);
            DateTime date_end_fitbit = date_end;
            int range;

            /*Scambio le date se invertite nel form*/
            if (date_start.CompareTo(date_end) > 0)
            {
                DateTime tmp = date_start;
                date_start = date_end;
                date_end = tmp;
                date_end_fitbit = tmp;
            }

            /*imposto i valori massimi del range a seconda dei dati richiesti*/
            if (mode == "total")
            {
                range = date_end.Subtract(date_start).Days + 1;
                if (range > Constant.RANGE_DATE_INDEX)
                    range = Constant.RANGE_DATE_INDEX;
            }
            else
            {
                range = (int)date_end.Subtract(date_start).TotalHours + 1;
                if (range > Constant.MAX_RANGE_PROGRESSIVE_FITBIT)
                    date_end_fitbit = date_start.AddHours(Constant.MAX_RANGE_PROGRESSIVE_FITBIT);

                if (range > Constant.MAX_RANGE_PROGRESSIVE)
                    date_end = date_start.AddHours(Constant.MAX_RANGE_PROGRESSIVE);
            }

            /*Creo il modello dell'utente*/
            User user = new User();
            user = user.GetUser(SetHomestationID(), HttpContext);

            /*Creo le liste di dati a seconda dei dati richiesti*/
            if (mode == "total")
            {
                user = user.GetFitbitTotal(date_start, range);
                user = user.GetHueTotal(date_start, range);
                user = user.GetSensorAvg(date_start, range);
            }
            else
            {
                user = user.GetFitbitProgressive(date_start, date_end_fitbit);
                user = user.GetHueProgressive(date_start, date_end);
                user = user.GetSensorProgressive(date_start, date_end);
            }

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View(user);
        }

        public IActionResult Tasks()
        {
            /*Controllo se è loggato un utente o un medico abilitato*/
            if (HttpContext.Session.GetString("Type").SessionNotEquals("user") && HttpContext.Session.GetString("Type").SessionNotEquals("medicUser"))
                return RedirectToAction("Index", "Home");

            /*Creo il modello dell'utente con i suoi task*/
            User user = new User();
            user = user.GetUser(SetHomestationID(), HttpContext);
            user = user.GetTask();           

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            ViewBag.Title = "Attività";
            return View(user);
        }

        [HttpPost]
        public IActionResult TaskCheck(string status)
        {
            if (HttpContext.Session.GetString("Type").SessionNotEquals("user"))
                return RedirectToAction("Tasks", "User");
            /*Imposto i valori per la put*/
            var id = Request.Form["Task_Id"].ToString();
            string executed;

            if (status.Equals("Attività completata"))
                executed = "true";
            else
                executed = "false";

            string body = $"{{ \"executed\": \"{executed}\" }}";

            /*Aggiorno lo stato del task*/
            try
            {
                new WebClient().UploadString($"{Constant.API_ADDRESS}task/{id}", "PUT", body);
            }
            catch (WebException e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return RedirectToAction("Tasks","User");
        }

        public IActionResult Settings()
        {
            if (HttpContext.Session.GetString("Type").SessionNotEquals("user"))
                return RedirectToAction("Index", "Home");

            /*Creo il modello dell'utente*/
            User user = new User();
            user = user.GetUser(HttpContext.Session.GetString("Id"), HttpContext);

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            ViewBag.Title = "Impostazioni";
            return View(user);
        }

        [HttpPost]
        public IActionResult Settings(string username, string email, string name, int age, int height, int weight)
        {
            /*Preparo i dati per il json*/
            var old_username = HttpContext.Session.GetString("Username");
            var id = HttpContext.Session.GetString("Id");
            string body = $"{{ \"username\": \"{username}\", \"email\": \"{email}\", \"name\": \"{name}\", \"age\": {age}, \"height\": {height}, \"weight\": {weight} }}";

            /*Aggiorno i dati dell'utente nel database e nella sessione*/
            try
            {
                new WebClient().UploadString($"{Constant.API_ADDRESS}login/{old_username}/{id}", "PUT", body);
                HttpContext.Session.SetString("Username", username);
                HttpContext.Session.SetString("Email", email);
                ViewData["Message"] = "Successo";
            }
            catch (WebException e)
            {
                Console.WriteLine(e.StackTrace);
                if (((HttpWebResponse)e.Response).StatusCode == HttpStatusCode.Conflict)
                    ViewData["Message"] = "Username o Email già esistenti";
                else
                    ViewData["Message"] = "Errore durante la modifica. Ritenta più tardi";
            }

            /*Creo il modello dell'utente*/
            User user = new User();
            user = user.GetUser(HttpContext.Session.GetString("Id"), HttpContext);

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View(user);
        }

        [HttpPost]
        public IActionResult SettingsPassword(string old_password, string new_password, string new_password2)
        {
            /*Controllo che le due password siano uguali*/
            if (!new_password.Equals(new_password2))
                ViewData["Password"] = "Scrivi la nuova password uguale in entrambi i campi";
            else
            {
                /*Aggiorno la password nel database*/
                var username = HttpContext.Session.GetString("Username");
                try
                {
                    new WebClient().DownloadString($"{Constant.API_ADDRESS}login?name={username}&password={old_password}");

                    string body = $"{{ \"username\": \"{username}\", \"password\": \"{new_password}\" }}";

                    new WebClient().UploadString($"{Constant.API_ADDRESS}password", "PUT", body);
                    ViewData["Password"] = "Password aggiornata!";
                }
                catch (WebException e)
                {
                    Console.WriteLine(e.StackTrace);
                    if (((HttpWebResponse)e.Response).StatusCode == HttpStatusCode.NotFound)
                        ViewData["Password"] = "Password non corretta. Reinserisci la tua vecchia password";
                    else
                        ViewData["Password"] = "Errore durante la modifica. Ritenta più tardi";
                }
            }

            /*Creo il modello dell'utente*/
            User user = new User();
            user = user.GetUser(HttpContext.Session.GetString("Id"), HttpContext);

            ViewData["Session"] = HttpContext.Session.GetString("Type");
            return View("Settings", user);
        }

        /*Imposto l'homestationid a seconda se si tratta dell'utente o del medico*/
        private string SetHomestationID()
        {
            if (HttpContext.Session.GetString("Type").SessionEquals("user"))
                return HttpContext.Session.GetString("Id");
            else
                return HttpContext.Session.GetString("HomestationId");
        }
    }
}