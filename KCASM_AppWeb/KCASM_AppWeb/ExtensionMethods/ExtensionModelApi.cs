using KCASM_AppWeb.Configuration;
using KCASM_AppWeb.Models.ForApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace KCASM_AppWeb.ExtensionMethods
{
    public static class ExtensionModelApi
    {
        private static string ExecuteGet(string url)
        {
            try
            {
                return new WebClient().DownloadString(url);
            }
            catch (WebException e)
            {
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }

        public static string ExecuteWebUpload(this string url, string method, string body)
        {
            try
            {
                WebClient client = new WebClient();
                client.Headers.Add("Content-Type", "application/json");
                return client.UploadString(url, method, body);
            }
            catch (WebException e)
            {
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }

        public static Patient GetPatient(this string id)
        {
            Patient p = null;

            var content = ExecuteGet(Constant.API_ADDRESS + "patients/" + id);
            if (content != null)
                p = JsonConvert.DeserializeObject<Patient>(content);

            return p;
        }

        public static CategoryTask GetCategoryTask(string type)
        {
            CategoryTask t = new CategoryTask();
            List<String> single_category_task = new List<string>();
            string url = $"{Constant.API_ADDRESS}/task_categories/";

            if (type != null)
            {
                url += type;
                var content = ExecuteGet(url);
                if (content != null)
                    switch (type)
                    {
                        case "general": t.General = JsonConvert.DeserializeObject<List<String>>(content); break;
                        case "activities": t.Activities = JsonConvert.DeserializeObject<List<String>>(content); break;
                        case "diets": t.Diets = JsonConvert.DeserializeObject<List<String>>(content); break;
                    }
            }
            else
            {
                url += "general";
                var content = ExecuteGet(url);
                if (content != null)
                    t.General = JsonConvert.DeserializeObject<List<String>>(content);

                url = $"{Constant.API_ADDRESS}/task_categories/activities";
                content = ExecuteGet(url);
                if (content != null)
                    t.Activities = JsonConvert.DeserializeObject<List<String>>(content);

                url = $"{Constant.API_ADDRESS}/task_categories/diets";
                content = ExecuteGet(url);
                if (content != null)
                    t.Diets = JsonConvert.DeserializeObject<List<String>>(content);
            }

            return t;
        }

        public static TaskList GetTasks(this string firstId, Boolean patient, String type, Dictionary<String, Object> filter)
        {
            TaskList t = null;
            string url = Constant.API_ADDRESS;
            if (patient)
                url += "patients/";
            else
                url += "medics/";

            url += firstId + "/tasks/" + type;
            // al momento senza filtri ....

            var content = ExecuteGet(url);
            if (content != null)
                t = JsonConvert.DeserializeObject<TaskList>(content);

            return t;
        }

        public static WeightsList GetWeights(this string id, String date)
        {
            WeightsList w = null;
            string url = Constant.API_ADDRESS + "patients/" + id + "/weights";
            if (date != null)
                url += "?date=" + date;

            var content = ExecuteGet(url);
            if (content != null)
                w = JsonConvert.DeserializeObject<WeightsList>(content);

            return w;
        }

        public static Threshold GetThreshold(this string id)
        {
            Threshold t = null;
            string url = Constant.API_ADDRESS + "patients/" + id + "/thresholds";
            var content = ExecuteGet(url);
            if (content != null)
                t = JsonConvert.DeserializeObject<Threshold>(content);
            return t;
        }

        public static PatientInitial GetPatientInitial(this string id)
        {
            PatientInitial p = null;

            var content = ExecuteGet(Constant.API_ADDRESS + "patients/" + id + "/initial_data");
            if (content != null)
                p = JsonConvert.DeserializeObject<PatientInitial>(content);

            return p;
        }

        public static List<Medic> GetPatientMedics(this string id)
        {
            List<Medic> m = null;

            var content = ExecuteGet(Constant.API_ADDRESS + "patients/" + id + "/medics");
            if (content != null)
                m = JsonConvert.DeserializeObject<List<Medic>>(content);

            return m;
        }

        public static MessageList GetMessage(this string id, Boolean patient, String type, string filterId)
        {
            MessageList m = null;
            string url = Constant.API_ADDRESS;
            if (patient)
                url += "patients/";
            else
                url += "medics/";

            url += id + "/messages/" + type;

            if (filterId != null)
            {
                if (patient)
                    url += "?medic_id=" + filterId;
                else
                    url += "?patient_id=" + filterId;
            }

            var content = ExecuteGet(url);
            if (content != null)
                m = JsonConvert.DeserializeObject<MessageList>(content);

            return m;
        }

        public static MeasuresListSamples GetMeasuresSamplesApi(this string id, String device, params string[] date)
        {
            MeasuresListSamples m = null;
            string url = Constant.API_ADDRESS + "patients/" + id + "/measures/samples/";

            string filter = "";
            switch (date.Length)
            {
                case 1: filter += "?date=" + date[0]; break;
                case 2: filter += "?startdate=" + date[0] + "&enddate=" + date[1]; break;
            }

            if (device != null)
            {
                url += device + filter;
                var content = ExecuteGet(url);
                if (content != null)
                {
                    m = JsonConvert.DeserializeObject<MeasuresListSamples>(content);
                }
            }
            else
            {
                m = new MeasuresListSamples();
                MeasuresListSamples mSingle;
                string content;
                url += "fitbit" + filter;
                content = ExecuteGet(url);
                if (content != null)
                {
                    mSingle = JsonConvert.DeserializeObject<MeasuresListSamples>(content);
                    m.Fitbit_samples = mSingle.Fitbit_samples;
                }

                url = Constant.API_ADDRESS + "patients/" + id + "/measures/samples/hue" + filter;
                content = ExecuteGet(url);
                if (content != null)
                {
                    mSingle = JsonConvert.DeserializeObject<MeasuresListSamples>(content);
                    m.Hue_samples = mSingle.Hue_samples;
                }

                url = Constant.API_ADDRESS + "patients/" + id + "/measures/samples/sensor" + filter;
                content = ExecuteGet(url);
                if (content != null)
                {
                    mSingle = JsonConvert.DeserializeObject<MeasuresListSamples>(content);
                    m.Sensor_samples = mSingle.Sensor_samples;
                }

            }
            return m;
        }

        public static MeasuresTotal GetMeasuresTotalApi(this string id, String device, String date)
        {
            MeasuresTotal m = null;
            string url;

            if (device != null)
            {
                url = Constant.API_ADDRESS + "patients/" + id + "/measures/total/" + device + "?date=" + date;

                var content = ExecuteGet(url);
                if (content != null)
                {
                    m = new MeasuresTotal();
                    switch (device)
                    {
                        case "fitbit": m.Fitbit_total = JsonConvert.DeserializeObject<Fitbit>(content); break;
                        case "hue": m.Hue_total = JsonConvert.DeserializeObject<HueTotal>(content); break;
                        case "sensor": m.Sensor_total = JsonConvert.DeserializeObject<Sensor>(content); break;
                    }
                }
            }
            else
            {
                m = new MeasuresTotal();
                string content;
                url = Constant.API_ADDRESS + "patients/" + id + "/measures/total/fitbit?date=" + date;
                content = ExecuteGet(url);
                if (content != null)
                    m.Fitbit_total = JsonConvert.DeserializeObject<Fitbit>(content);

                url = Constant.API_ADDRESS + "patients/" + id + "/measures/total/hue?date=" + date;
                content = ExecuteGet(url);
                if (content != null)
                    m.Hue_total = JsonConvert.DeserializeObject<HueTotal>(content);

                url = Constant.API_ADDRESS + "patients/" + id + "/measures/total/sensor?date=" + date;
                content = ExecuteGet(url);
                if (content != null)
                    m.Sensor_total = JsonConvert.DeserializeObject<Sensor>(content);


            }
            return m;
        }

        public static Login GetLogin(this string id, Boolean patient)
        {
            Login l = null;
            string url = Constant.API_ADDRESS;
            if (patient)
                url += "patients/" + id + "/login_data";
            else
                url += "medics/" + id + "/login_data";

            var content = ExecuteGet(url);
            if (content != null)
                l = JsonConvert.DeserializeObject<Login>(content);

            return l;
        }


        public static Medic GetMedic(this string id)
        {
            Medic m = null;

            var content = ExecuteGet(Constant.API_ADDRESS + "medics/" + id);
            if (content != null)
                m = JsonConvert.DeserializeObject<Medic>(content);

            return m;
        }

        public static List<Patient> GetMedicPatients(this string id)
        {
            List<Patient> p = null;

            var content = ExecuteGet(Constant.API_ADDRESS + "medics/" + id + "/patients");
            if (content != null)
                p = JsonConvert.DeserializeObject<List<Patient>>(content);

            return p;
        }

    }
}
