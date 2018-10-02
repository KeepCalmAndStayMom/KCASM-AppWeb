using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using KCASM_AppWeb.Configuration;
using KCASM_AppWeb.Models;
using Newtonsoft.Json;

namespace KCASM_AppWeb.ExtensionMethods
{
    /*Extension Methods relativi ai sensori*/
    public static class ExtensionSensor
    {
        /*Creo tutte le liste visionando una sola volta il dizionario*/
        public static SensorList GetAllHueList(this Dictionary<string, Sensor> dictionary)
        {
            SensorList sensorList = new SensorList
            {
                Temperature = new List<int?>(),
                Luminescence = new List<int?>(),
                Humidity = new List<int?>()
            };

            foreach (Sensor element in dictionary.Values)
            {
                if (element == null)
                {
                    sensorList.Temperature.Add(null);
                    sensorList.Luminescence.Add(null);
                    sensorList.Humidity.Add(null);
                }
                else
                {
                    sensorList.Temperature.Add(element.Temperature);
                    sensorList.Luminescence.Add(element.Luminescence);
                    sensorList.Humidity.Add(element.Humidity);
                }
            }
            return sensorList;
        }

        /*Creo una sola lista da un dizionario a seconda del parametro inner_key*/
        public static List<int?> GetSensorList(this Dictionary<string, Sensor> dictionary, string inner_key)
        {
            List<int?> list = new List<int?>();
            foreach (Sensor element in dictionary.Values)
            {
                if (element == null)
                    list.Add(null);
                else
                    switch (inner_key)
                    {
                        case "temperature": list.Add(element.Temperature); break;
                        case "luminescence": list.Add(element.Luminescence); break;
                        case "humidity": list.Add(element.Humidity); break;
                    }
            }
            return list;
        }

        /*Restituisco un utente con i dati dei sensori medi a partire da una data fino a un range*/
        public static User GetSensorAvg(this User user, DateTime start_date, int range)
        {
            if (user != null)
            {
                Dictionary<string, Sensor> sensors = new Dictionary<string, Sensor>();
                for (int i = 0; i < range; i++)
                {
                    var date_formatted = DateTime.ParseExact(start_date.ToString(), Constant.DATETIME_FORMAT, CultureInfo.InvariantCulture).ToString(Constant.DATE_API_FORMAT);

                    try
                    {
                        var content = new WebClient().DownloadString($"{Constant.API_ADDRESS}zway_avg/{user.Homestation_id}/{date_formatted}");
                        sensors.Add(date_formatted, JsonConvert.DeserializeObject<Sensor>(content));
                    }
                    catch (WebException e)
                    {
                        Console.WriteLine(e.StackTrace);
                        sensors.Add(date_formatted, null);
                    }

                    start_date = start_date.AddDays(1);
                }

                user.DateSensor = sensors.GetKeyList<Sensor>();
                user.SensorList = sensors.GetAllHueList();
            }
            return user;
        }

        /*Restituisco un utente con i dati dei sensori progressivi all'interno di due date*/
        public static User GetSensorProgressive(this User user, DateTime start_date, DateTime end_date)
        {
            if (user != null)
            {
                Dictionary<string, Sensor> sensors = new Dictionary<string, Sensor>();
                var date_start_formatted = DateTime.ParseExact(start_date.ToString(), Constant.DATETIME_FORMAT, CultureInfo.InvariantCulture).ToString(Constant.DATETIME_API_FORMAT);
                var date_end_formatted = DateTime.ParseExact(end_date.ToString(), Constant.DATETIME_FORMAT, CultureInfo.InvariantCulture).ToString(Constant.DATETIME_API_FORMAT);
                try
                {
                    var content = new WebClient().DownloadString($"{Constant.API_ADDRESS}zway/{user.Homestation_id}/{date_start_formatted}/{date_end_formatted}");
                    sensors = JsonConvert.DeserializeObject<Dictionary<string, Sensor>>(content);
                }
                catch (WebException e)
                {
                    Console.WriteLine(e.StackTrace);
                    sensors = null;
                }

                if (sensors != null)
                {
                    user.DateSensor = sensors.GetKeyList<Sensor>();
                    user.SensorList = sensors.GetAllHueList();
                }
                else
                {
                    user.SensorList = null;
                }
            }
            return user;
        }
    }
}
