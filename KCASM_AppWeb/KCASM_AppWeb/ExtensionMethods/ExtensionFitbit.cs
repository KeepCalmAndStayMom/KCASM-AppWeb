using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using KCASM_AppWeb.Configuration;
using KCASM_AppWeb.Models;
using Newtonsoft.Json;

namespace KCASM_AppWeb.ExtensionMethods
{
    /*Extension Methods relativi ai Fitbit*/
    public static class ExtensionFitbit
    {
        /*Creo tutte le liste visionando una sola volta il dizionario*/
        public static FitbitList GetAllFitbitList(this Dictionary<string, Fitbit> dictionary)
        {
            FitbitList fitbitList = new FitbitList
            {
                Calories = new List<float?>(),
                Elevation = new List<float?>(),
                Floors = new List<float?>(),
                Steps = new List<float?>(),
                Distance = new List<float?>(),
                MinutesAsleep = new List<float?>(),
                MinutesAwake = new List<float?>(),
                Heartbeats = new List<int?>()
            };

            foreach (Fitbit element in dictionary.Values)
            {
                /*Se l'elemento è nullo aggiungo comunque il campo alla lista per la visualizzazione nel grafico*/
                if (element == null)
                {
                    fitbitList.Calories.Add(null);
                    fitbitList.Elevation.Add(null);
                    fitbitList.Floors.Add(null);
                    fitbitList.Steps.Add(null);
                    fitbitList.Distance.Add(null);
                    fitbitList.MinutesAsleep.Add(null);
                    fitbitList.MinutesAwake.Add(null);
                    fitbitList.Heartbeats.Add(null);
                }
                else
                {
                    fitbitList.Calories.Add(element.Calories);
                    fitbitList.Elevation.Add(element.Elevation);
                    fitbitList.Floors.Add(element.Floors);
                    fitbitList.Steps.Add(element.Steps);
                    fitbitList.Distance.Add(element.Distance);
                    fitbitList.MinutesAsleep.Add(element.MinutesAsleep);
                    fitbitList.MinutesAwake.Add(element.MinutesAwake);
                    fitbitList.Heartbeats.Add(element.Avg_heartbeats);
                }
            }
            return fitbitList;
        }

        /*Creo una sola lista da un dizionario a seconda del parametro inner_key*/
        public static List<float?> GetFitbitFloatList(this Dictionary<string, Fitbit> dictionary, string inner_key)
        {
            List<float?> list = new List<float?>();
            foreach (Fitbit element in dictionary.Values)
            {
                if (element == null)
                    list.Add(null);
                else
                    switch (inner_key)
                    {
                        case "calories": list.Add(element.Calories); break;
                        case "elevation": list.Add(element.Elevation); break;
                        case "floors": list.Add(element.Floors); break;
                        case "steps": list.Add(element.Steps); break;
                        case "distance": list.Add(element.Distance); break;
                        case "minutesAsleep": list.Add(element.MinutesAsleep); break;
                        case "minutesAwake": list.Add(element.MinutesAwake); break;
                    }
            }
            return list;
        }

        /*Creo la singola lista degli HeartBeats a partire dal dizionario*/
        public static List<int?> GetFitbitHeartbeatsList(this Dictionary<string, Fitbit> dictionary)
        {
            List<int?> list = new List<int?>();
            foreach (Fitbit element in dictionary.Values)
            {
                if (element == null)
                    list.Add(null);
                else
                    list.Add(element.Avg_heartbeats);
            }
            return list;
        }

        /*Restituisco un utente con i dati del fitbit totali a partire da una data fino a un range*/
        public static User GetFitbitTotal(this User user, DateTime start_date, int range)
        {
            if (user != null)
            {
                Dictionary<string, Fitbit> fitbits = new Dictionary<string, Fitbit>();

                for (int i = 0; i < range; i++)
                {
                    var date_formatted = DateTime.ParseExact(start_date.ToString(), Constant.DATETIME_FORMAT, CultureInfo.InvariantCulture).ToString(Constant.DATE_API_FORMAT);

                    try
                    {
                        var content = new WebClient().DownloadString($"{Constant.API_ADDRESS}fitbit_total/{user.Homestation_id}/{date_formatted}");
                        fitbits.Add(date_formatted, JsonConvert.DeserializeObject<Fitbit>(content));
                    }
                    catch (WebException e)
                    {
                        Console.WriteLine(e.StackTrace);
                        fitbits.Add(date_formatted, null);
                    }

                    start_date = start_date.AddDays(1);
                }
            
                user.DateFitbit = fitbits.GetKeyList<Fitbit>();
                user.FitbitList = fitbits.GetAllFitbitList();
            }
            return user;
        }

        /*Restituisco un utente con i dati del fitbit progressivi all'interno di due date*/
        public static User GetFitbitProgressive(this User user, DateTime start_date, DateTime end_date)
        {
            if (user != null)
            {
                Dictionary<string, Fitbit> fitbits = new Dictionary<string, Fitbit>();
                var date_start_formatted = DateTime.ParseExact(start_date.ToString(), Constant.DATETIME_FORMAT, CultureInfo.InvariantCulture).ToString(Constant.DATETIME_API_FORMAT);
                var date_end_formatted = DateTime.ParseExact(end_date.ToString(), Constant.DATETIME_FORMAT, CultureInfo.InvariantCulture).ToString(Constant.DATETIME_API_FORMAT);
                try
                {
                    var content = new WebClient().DownloadString($"{Constant.API_ADDRESS}fitbit/{user.Homestation_id}/{date_start_formatted}/{date_end_formatted}");
                    fitbits = JsonConvert.DeserializeObject<Dictionary<string, Fitbit>>(content);
                }
                catch (WebException e)
                {
                    Console.WriteLine(e.StackTrace);
                    fitbits = null;
                }

                if (fitbits != null)
                {
                    user.DateFitbit = fitbits.GetKeyList<Fitbit>();
                    user.FitbitList = fitbits.GetAllFitbitList();
                }
                else
                    user.FitbitList = null;
            }
            return user;
        }
    }
}
