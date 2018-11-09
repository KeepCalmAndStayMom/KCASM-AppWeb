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

        public static TaskList getTasks(this string firstId, Boolean patient, String type, Int16 secondId, Boolean executed, String date, String endDate, Boolean startingProgram)
        {
            TaskList t = null;
            string url = Constant.API_ADDRESS;
            if (patient)
                url += "patients/";
            else
                url += "medics/";

            url += firstId + "/tastks/" + type;
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

        public static MessageList GetMessage(this string id, Boolean patient, String type, String date, String endDate, String timedate, Int16 medicId)
        {
            MessageList m = null;
            string url = Constant.API_ADDRESS;
            if (patient)
                url += "patients/";
            else
                url += "medics/";

            url += id + "/message/" + type;
            // al momento senza filtri ....

            var content = executeGet(url);
            if (content != null)
                m = JsonConvert.DeserializeObject<MessageList>(content);

            return m;
        }

        public static MeasuresList GetMeasures(this string id, String type, String device, String date, String endDate)
        {
            MeasuresList m = null;
            string url = Constant.API_ADDRESS + "patients/" + id + "/tasks/" + type + "/" + device;
            if (type.Equals("total"))
                url += "?date=" + date;

            // al momento senza filtri ....

            var content = executeGet(url);
            if (content != null)
                m = JsonConvert.DeserializeObject<MeasuresList>(content);

            return m;
        }

        public static Login getLogin(this string id, Boolean patient)
        {
            Login l = null;
            string url = Constant.API_ADDRESS;
            if (patient)
                url += "patients/" + id;
            else
                url += "medics/" + id;

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
