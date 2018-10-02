using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using KCASM_AppWeb.Configuration;
using KCASM_AppWeb.Models;
using Newtonsoft.Json;

namespace KCASM_AppWeb.ExtensionMethods
{
    /*Extension Methods relativi alle Hue*/
    public static class ExtensionHue
    {
        /*Creo tutte le liste visionando una sola volta il dizionario*/
        public static HueList GetAllHueList(this Dictionary<string, Hue> dictionary)
        {
            HueList hueList = new HueList
            {
                CromoSoft = new List<int?>(),
                CromoHard = new List<int?>()
            };
            foreach (Hue element in dictionary.Values)
            {
                if (element == null)
                {
                    hueList.CromoSoft.Add(null);
                    hueList.CromoHard.Add(null);
                }
                else
                {
                    hueList.CromoSoft.Add(element.CromoSoft);
                    hueList.CromoHard.Add(element.CromoHard);
                }
            }
            return hueList;
        }

        /*Creo una sola lista da un dizionario a seconda del parametro inner_key*/
        public static List<int?> GetHueList(this Dictionary<string, Hue> dictionary, string inner_key)
        {
            List<int?> list = new List<int?>();
            foreach (Hue element in dictionary.Values)
            {
                if (element == null)
                    list.Add(null);
                else
                    switch (inner_key)
                    {
                        case "cromosoft": list.Add(element.CromoSoft); break;
                        case "cromohard": list.Add(element.CromoHard); break;
                    }
            }
            return list;
        }

        /*Restituisco un utente con i dati delle hue totali a partire da una data fino a un range*/
        public static User GetHueTotal(this User user, DateTime start_date, int range)
        {
            if (user != null)
            {
                Dictionary<string, Hue> hues = new Dictionary<string, Hue>();
                for (int i = 0; i < range; i++)
                {
                    var date_formatted = DateTime.ParseExact(start_date.ToString(), Constant.DATETIME_FORMAT, CultureInfo.InvariantCulture).ToString(Constant.DATE_API_FORMAT);

                    try
                    {
                        var content = new WebClient().DownloadString($"{Constant.API_ADDRESS}hue_total/{user.Homestation_id}/{date_formatted}");
                        hues.Add(date_formatted, JsonConvert.DeserializeObject<Hue>(content));
                    }
                    catch (WebException e)
                    {
                        Console.WriteLine(e.StackTrace);
                        hues.Add(date_formatted, null);
                    }

                    start_date = start_date.AddDays(1);
                }

                user.DateHue = hues.GetKeyList<Hue>();
                user.HueList = hues.GetAllHueList();
            }
            return user;
        }

        /*Restituisco un utente con i dati delle hue progressivi all'interno di due date*/
        public static User GetHueProgressive(this User user, DateTime start_date, DateTime end_date)
        {
            if (user != null)
            {
                Dictionary<string, Hue> hues = new Dictionary<string, Hue>();
                var date_start_formatted = DateTime.ParseExact(start_date.ToString(), Constant.DATETIME_FORMAT, CultureInfo.InvariantCulture).ToString(Constant.DATETIME_API_FORMAT);
                var date_end_formatted = DateTime.ParseExact(end_date.ToString(), Constant.DATETIME_FORMAT, CultureInfo.InvariantCulture).ToString(Constant.DATETIME_API_FORMAT);
                try
                {
                    var content = new WebClient().DownloadString($"{Constant.API_ADDRESS}hue/{user.Homestation_id}/{date_start_formatted}/{date_end_formatted}");
                    hues = JsonConvert.DeserializeObject<Dictionary<string, Hue>>(content);
                }
                catch (WebException e)
                {
                    Console.WriteLine(e.StackTrace);
                    hues = null;
                }

                if (hues != null)
                {
                    user.DateHue = hues.GetKeyList<Hue>();
                    user.HueList = hues.GetAllHueList();
                }
                else
                {
                    user.HueList = null;
                }
            }
            return user;
        }
    }
}
