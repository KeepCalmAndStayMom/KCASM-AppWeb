using System;
using System.Collections.Generic;
using System.Net;
using KCASM_AppWeb.Configuration;
using KCASM_AppWeb.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace KCASM_AppWeb.ExtensionMethods
{
    /*Extension Methods relativi a utenti e medici*/
    public static class ExtensionModel
    {
        /*Restituisco una lista di date a partire dalle chiavi di un dizionario*/
        public static List<string> GetKeyList<T>(this Dictionary<string, T> dictionary)
        {
            if (dictionary.Keys == null)
                return null;

            List<string> keys = new List<string>();

            foreach (string key in dictionary.Keys)
                keys.Add(key);

            return keys;
        }

        /*Restituisco un medico a partire da un id e dalle variabili di sessione*/
        public static Medic GetMedic(this Medic medic, string id, HttpContext session)
        {
            try
            {
                var content = new WebClient().DownloadString($"{Constant.API_ADDRESS}medics/{id}");
                medic = JsonConvert.DeserializeObject<Medic>(content);
                medic.Id = id;
                medic.Username = session.Session.GetString("Username");
                medic.Email = session.Session.GetString("Email");
            }
            catch (WebException e)
            {
                Console.WriteLine(e.StackTrace);
                medic = null;
            }

            return medic;
        }

        /*Restituisco un medico con il dizionario di tutti i suoi pazienti con dati base*/
        public static Medic GetPatients(this Medic medic, HttpContext session)
        {
            if (medic != null)
            {
                try
                {
                    var content = new WebClient().DownloadString($"{Constant.API_ADDRESS}medics/{medic.Id}/users");
                    medic.Patients = new Dictionary<string, User>();
                    foreach (string homestation_id in JsonConvert.DeserializeObject<Dictionary<string, User>>(content).Keys)
                    {
                        User user = new User();
                        user = user.GetUser(homestation_id, session);
                        medic.Patients.Add(homestation_id, user);
                    }
                }
                catch (WebException e)
                {
                    Console.WriteLine(e.StackTrace);
                    medic.Patients = null;
                }
            }
            return medic;
        }

        /*restituisco un utente con tutti i dati*/
        public static User GetUserIndex(this User user, string id, HttpContext session)
        {
            user = user.GetUser(id, session);
            if (user != null)
            {
                user = user.GetTask();

                var range = Constant.RANGE_DATE_INDEX;
                DateTime date_start = DateTime.Today.AddDays(-range);

                user = user.GetFitbitTotal(date_start, range);
                user = user.GetHueTotal(date_start, range);
                user = user.GetSensorAvg(date_start, range);
            }
            return user;
        }

        /*Restituisco un utente con dati base tramite l'id e la sessione*/
        public static User GetUser(this User user, string id, HttpContext session)
        {
            try
            {
                var content = new WebClient().DownloadString($"{Constant.API_ADDRESS}users/{id}");
                user = JsonConvert.DeserializeObject<User>(content);
                user.Homestation_id = id;
                if (session.Session.GetString("Type") == "user")
                {
                    user.Username = session.Session.GetString("Username");
                    user.Email = session.Session.GetString("Email");
                }

                return user;
            }
            catch (WebException e)
            {
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }

        /*Restituisco un utente con i task*/
        public static User GetTask(this User user)
        {
            if (user != null)
            {
                try
                {
                    var content = new WebClient().DownloadString($"{Constant.API_ADDRESS}tasks/{user.Homestation_id}");
                    user.Tasks = JsonConvert.DeserializeObject<Dictionary<string, Models.Task>>(content);
                }
                catch (WebException e)
                {
                    Console.WriteLine(e.StackTrace);
                    user.Tasks = null;
                }
            }
            return user;
        }
    }
}
