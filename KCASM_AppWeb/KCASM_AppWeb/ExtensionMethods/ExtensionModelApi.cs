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
        private static string executeGet(string url)
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

        public static Patient getPatient(this string id)
        {
            Patient p = null;

            var content = executeGet(Constant.API_ADDRESS + "patients/" + id);
            if (content != null)
                p = JsonConvert.DeserializeObject<Patient>(content);

            return p;
        }

        public static TaskList getTasks(this string firstId, Boolean patient, String type, Dictionary<String, Object> filter)
        {
            TaskList t = null;
            string url = Constant.API_ADDRESS;
            if (patient)
                url += "patients/";
            else
                url += "medics/";

            url += firstId + "/tasks/" + type;
            // al momento senza filtri ....

            var content = executeGet(url);
            if (content != null)
                t = JsonConvert.DeserializeObject<TaskList>(content);

            return t;
        }

        public static WeightsList GetWeights(this string id, String date)
        {
            WeightsList w = null;
            string url = Constant.API_ADDRESS + "patients/" + id;
            if (date != null)
                url += "?date=" + date;

            var content = executeGet(url);
            if (content != null)
                w = JsonConvert.DeserializeObject<WeightsList>(content);

            return w;
        }

        public static Threshold GetThreshold(this string id)
        {
            Threshold t = null;
            string url = Constant.API_ADDRESS + "patients/" + id + "threshold";
            var content = executeGet(url);
            if (content != null)
                t = JsonConvert.DeserializeObject<Threshold>(content);
            return t;
        }

        public static PatientInitial GetPatientInitial(this string id)
        {
            PatientInitial p = null;

            var content = executeGet(Constant.API_ADDRESS + "patients/" + id + "/initial_data");
            if (content != null)
                p = JsonConvert.DeserializeObject<PatientInitial>(content);

            return p;
        }

        public static List<Medic> getPatientMedics(this string id)
        {
            List<Medic> m = null;

            var content = executeGet(Constant.API_ADDRESS + "patients/" + id + "/medics");
            if (content != null)
                m = JsonConvert.DeserializeObject<List<Medic>>(content);

            return m;
        }

        public static MessageList GetMessage(this string id, Boolean patient, String type, Dictionary<String, Object> filter)
        {
            MessageList m = null;
            string url = Constant.API_ADDRESS;
            if (patient)
                url += "patients/";
            else
                url += "medics/";

            url += id + "/messages/" + type;
            // al momento senza filtri ....

            var content = executeGet(url);
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
                case 2: filter += "?stardate=" + date[0] + "&enddate=" + date[1]; break;
            }

            if (device != null)
            {
                url += device;
                url += filter;
                var content = executeGet(url);
                if (content != null)
                {
                    m = JsonConvert.DeserializeObject<MeasuresListSamples>(content);
                }
            }
            else
            {
                MeasuresListSamples mSingle;
                string content;
                url += "fitbit";
                url += filter;
                content = executeGet(url);
                if (content != null)
                {
                    mSingle = JsonConvert.DeserializeObject<MeasuresListSamples>(content);
                    m.fitbit_samples = mSingle.fitbit_samples;
                }

                url = Constant.API_ADDRESS + "patients/" + id + "/measures/samples/hue";
                url += filter;
                content = executeGet(url);
                if (content != null)
                {
                    mSingle = JsonConvert.DeserializeObject<MeasuresListSamples>(content);
                    m.hue_samples = mSingle.hue_samples;
                }

                url = Constant.API_ADDRESS + "patients/" + id + "/measures/samples/sensor";
                url += filter;
                content = executeGet(url);
                if (content != null)
                {
                    mSingle = JsonConvert.DeserializeObject<MeasuresListSamples>(content);
                    m.sensor_samples = mSingle.sensor_samples;
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

                var content = executeGet(url);
                if (content != null)
                {
                    m = new MeasuresTotal();
                    switch (device)
                    {
                        case "fitbit": m.fitbit_total = JsonConvert.DeserializeObject<Fitbit>(content); break;
                        case "hue": m.hue_total = JsonConvert.DeserializeObject<HueTotal>(content); break;
                        case "sensor": m.sensor_total = JsonConvert.DeserializeObject<Sensor>(content); break;
                    }
                }
            }
            else
            {
                m = new MeasuresTotal();
                string content;
                url = Constant.API_ADDRESS + "patients/" + id + "/measures/total/fitbit?date=" + date;
                content = executeGet(url);
                if (content != null)
                    m.fitbit_total = JsonConvert.DeserializeObject<Fitbit>(content);

                url = Constant.API_ADDRESS + "patients/" + id + "/measures/total/hue?date=" + date;
                content = executeGet(url);
                if (content != null)
                    m.hue_total = JsonConvert.DeserializeObject<HueTotal>(content);

                url = Constant.API_ADDRESS + "patients/" + id + "/measures/total/sensor?date=" + date;
                content = executeGet(url);
                if (content != null)
                    m.sensor_total = JsonConvert.DeserializeObject<Sensor>(content);


            }
            return m;
        }

        public static Login getLogin(this string id, Boolean patient)
        {
            Login l = null;
            string url = Constant.API_ADDRESS;
            if (patient)
                url += "patients/" + id + "/login_data";
            else
                url += "medics/" + id + "/login_data";

            var content = executeGet(url);
            if (content != null)
                l = JsonConvert.DeserializeObject<Login>(content);

            return l;
        }


        public static Medic GetMedic(this string id)
        {
            Medic m = null;

            var content = executeGet(Constant.API_ADDRESS + "medics/" + id);
            if (content != null)
                m = JsonConvert.DeserializeObject<Medic>(content);

            return m;
        }

        public static List<Patient> getMedicPatients(this string id)
        {
            List<Patient> p = null;

            var content = executeGet(Constant.API_ADDRESS + "medics/" + id + "/patients");
            if (content != null)
                p = JsonConvert.DeserializeObject<List<Patient>>(content);

            return p;
        }

        /*
        paziente -> patients/id -> mappa

        task -> general/activities/diets -> patients/id/tasks/general -> lista su "general"
            filtri ?medic_id , ?executed=0/1 , ?date , ?startdate= &enddate= , ?starting_program=0/1 
         
        weight -> patients/id/weights -> lista su "weights"
            filtri ?date

        patientInitial -> patients/id/initial_data -> mappa diretta

        medici collegati -> patients/id/medics -> lista diretta

        messaggi paziente -> ricevuti/inviati -> patients/id/messages/received|sent -> lista su "message_sent" / "message_received"
            filtri ?date, ?startdate= &enddate= , ?timedate , ?medic_id

        measures -> samples/total -> fitbit/hue/sensor -> patients/id/measures/samples/fitbit|hue|sensor -> lista su "sensor_samples"
            filtri samples ?date , ?startdate= &enddate= 
            filtri total ?date (obbligatorio)

        login -> patients/id/login_data -> mappa diretta


        medico -> medics/id -> mappa diretta

        pazienti collegati -> medics/id/patients -> lista diretta

        task|message|login sono uguali ai patient

         */
    }
}
