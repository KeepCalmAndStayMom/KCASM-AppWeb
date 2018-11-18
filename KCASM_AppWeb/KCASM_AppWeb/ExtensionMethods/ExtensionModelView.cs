
using KCASM_AppWeb.Configuration;
using KCASM_AppWeb.Models.ForView;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.ExtensionMethods
{
    public static class ExtensionModelView
    {

        public static Medic GetMedic(this Models.ForApi.Medic apiMedic, Models.ForApi.Login apiLogin, List<Models.ForApi.Patient> patients)
        {
            Medic medic = new Medic();

            medic.Name = apiMedic.Name;
            medic.Surname = apiMedic.Surname;
            medic.Age = apiMedic.Age;
            medic.Phone = apiMedic.Phone;
            medic.Address = apiMedic.Address;
            medic.Specialization = apiMedic.Specialization;
            medic.Email_notify = apiMedic.Email_notify;
            medic.Sms_notify = apiMedic.Sms_notify;

            medic.Email = apiLogin.Email;

            medic.Patients = new List<PatientsForMedic>();

            foreach (Models.ForApi.Patient p in patients)
                medic.Patients.Add(new PatientsForMedic(p.Id, p.Name, p.Surname));

            return medic;
        }

        public static Message GetMessage(this Models.ForApi.MessageList received, Models.ForApi.MessageList sent)
        {
            Message message = new Message();
            message.Message_received = received.Message_received;
            message.Message_sent = received.Message_sent;
            return message;
        }

        public static Tasks GetTask(this Models.ForApi.TaskList general, Models.ForApi.TaskList activity, Models.ForApi.TaskList diet)
        {
            Tasks tasks = new Tasks();
            tasks.TaskList = new List<Models.ForView.Task>();

            foreach (Models.ForApi.Task t in general.General)
                tasks.TaskList.Add(new Models.ForView.Task(t, "General"));

            foreach (Models.ForApi.Task t in activity.Activities)
                tasks.TaskList.Add(new Models.ForView.Task(t, "Activities"));

            foreach (Models.ForApi.Task t in diet.Diets)
                tasks.TaskList.Add(new Models.ForView.Task(t, "Diets"));

            return tasks;
        }

        public static Weights GetWeights(this Models.ForApi.WeightsList weightsList, Models.ForApi.PatientInitial patientInitial, Models.ForApi.Threshold threshold)
        {
            Weights weights = new Weights();
            DateTime startDate = DateTime.Parse(patientInitial.Pregnancy_start_date);
            weights.Date[0] = DateTime.ParseExact(startDate.ToString(), Constant.DATETIME_FORMAT, CultureInfo.InvariantCulture).ToString(Constant.DATE_API_FORMAT);
            weights.Weight[0] = patientInitial.Weight;
            startDate = startDate.AddDays(1);

            double avg;
            double count;
            for (int i = 1; i <= Constant.WEIGHT_LIMIT; i++)
            {
                weights.Date[i] =  DateTime.ParseExact(startDate.ToString(), Constant.DATETIME_FORMAT, CultureInfo.InvariantCulture).ToString(Constant.DATE_API_FORMAT);

                avg = 0d;
                count = 0d;
                for (int j = 0; j < 7; j++)
                {
                    foreach (Models.ForApi.Weights w in weightsList.Weights)
                    {
                        if (DateTime.Parse(w.Date).Equals(startDate))
                        {
                            avg += w.Weight;
                            count += 1;
                            break;
                        }
                    }
                    startDate = startDate.AddDays(1);
                }

                if (count != 0)
                    weights.Weight[i] = avg / count;
                else
                    weights.Weight[i] = null;
            }

            weights.LowerThreshold = threshold.Min;
            weights.UpperThreshold = threshold.Max;

            return weights;
        }

        public static Patient GetPatient(this Models.ForApi.Patient apiPatient, Models.ForApi.PatientInitial patientInitial, Models.ForApi.Login login, List<Models.ForApi.Medic> medics)
        {
            Patient patient = new Patient();

            patient.Name = apiPatient.Name;
            patient.Surname = apiPatient.Surname;
            patient.Age = apiPatient.Age;
            patient.Phone = apiPatient.Phone;
            patient.Address_home = apiPatient.Address_home;
            patient.Address_hospital = apiPatient.Address_hospital;
            patient.Email_notify = apiPatient.Email_notify;
            patient.Sms_notify = apiPatient.Sms_notify;

            patient.Pregnancy_start_date = patientInitial.Pregnancy_start_date;
            patient.Weight = patientInitial.Weight;
            patient.Height = patientInitial.Height;
            patient.Bmi = patientInitial.Bmi;
            patient.Twin = patientInitial.Twin;

            patient.Email = login.Email;

            patient.Medics = new List<MedicsForPatient>();

            foreach (Models.ForApi.Medic medic in medics)
                patient.Medics.Add(new MedicsForPatient(medic.Id, medic.Name, medic.Surname, medic.Specialization));

            return patient;
        }

        public static Measures GetMeasuresTotal(this string id, DateTime date, DateTime endDate)
        {
            Measures measures = new Measures();
            measures.fitbitArray = new FitbitArray();
            measures.hueArray = new HueArray();
            measures.sensorArray = new SensorArray();

            Models.ForApi.MeasuresTotal measuresTotal;
            int range;

            if (endDate != null)
            {
                if (date.CompareTo(endDate) > 0)
                {
                    DateTime tmp = date;
                    date = endDate;
                    endDate = tmp;
                }

                range = endDate.Subtract(date).Days + 1;
                if (range > Constant.DATE_LIMIT_TOTAL)
                    range = Constant.DATE_LIMIT_TOTAL;

            }
            else
                range = Constant.DATE_LIMIT_TOTAL;

            for (int i = 0; i < range; i++)
            {
                var dateFormatted = DateTime.ParseExact(date.ToString(), Constant.DATETIME_FORMAT, CultureInfo.InvariantCulture).ToString(Constant.DATE_API_FORMAT);

                measuresTotal = id.GetMeasuresTotalApi(null, dateFormatted);

                if (measuresTotal != null)
                {
                    if (measuresTotal.fitbit_total != null)
                    {
                        measures.fitbitArray.Date[i] = dateFormatted;
                        measures.fitbitArray.Avg_heartbeats[i] = measuresTotal.fitbit_total.Avg_heartbeats;
                        measures.fitbitArray.Calories[i] = measuresTotal.fitbit_total.Calories;
                        measures.fitbitArray.Elevation[i] = measuresTotal.fitbit_total.Elevation;
                        measures.fitbitArray.Floors[i] = measuresTotal.fitbit_total.Floors;
                        measures.fitbitArray.Steps[i] = measuresTotal.fitbit_total.Steps;
                        measures.fitbitArray.Distance[i] = measuresTotal.fitbit_total.Distance;
                        measures.fitbitArray.Minutes_asleep[i] = measuresTotal.fitbit_total.Minutes_asleep;
                        measures.fitbitArray.Minutes_awake[i] = measuresTotal.fitbit_total.Minutes_awake;
                    }
                    else
                    {
                        measures.fitbitArray.Date[i] = dateFormatted;
                        measures.fitbitArray.Avg_heartbeats[i] = null;
                        measures.fitbitArray.Calories[i] = null;
                        measures.fitbitArray.Elevation[i] = null;
                        measures.fitbitArray.Floors[i] = null;
                        measures.fitbitArray.Steps[i] = null;
                        measures.fitbitArray.Distance[i] = null;
                        measures.fitbitArray.Minutes_asleep[i] = null;
                        measures.fitbitArray.Minutes_awake[i] = null;
                    }

                    if (measuresTotal.hue_total != null)
                    {
                        measures.hueArray.Date[i] = dateFormatted;
                        measures.hueArray.Hard[i] = measuresTotal.hue_total.Hard;
                        measures.hueArray.Soft[i] = measuresTotal.hue_total.Soft;
                    }
                    else
                    {
                        measures.hueArray.Date[i] = dateFormatted;
                        measures.hueArray.Hard[i] = 0;
                        measures.hueArray.Soft[i] = 0;
                    }

                    if (measuresTotal.sensor_total != null)
                    {
                        measures.sensorArray.Date[i] = dateFormatted;
                        measures.sensorArray.Temperature[i] = measuresTotal.sensor_total.Temperature;
                        measures.sensorArray.Luminescence[i] = measuresTotal.sensor_total.Luminescence;
                        measures.sensorArray.Humidity[i] = measuresTotal.sensor_total.Humidity;
                    }
                    else
                    {
                        measures.sensorArray.Date[i] = dateFormatted;
                        measures.sensorArray.Temperature[i] = null;
                        measures.sensorArray.Luminescence[i] = null;
                        measures.sensorArray.Humidity[i] = null;
                    }
                }

                date = date.AddDays(1);
            }


            return measures;
        }

        public static Measures GetMeasuresSamples(this string id, DateTime date, DateTime endDate)
        {
            Measures measures = new Measures();
            measures.fitbitArray = new FitbitArray();
            measures.hueArray = new HueArray();
            measures.sensorArray = new SensorArray();
            Models.ForApi.MeasuresListSamples measuresSamples;

            if (date != null)
            {
                var dateFormatted = DateTime.ParseExact(date.ToString(), Constant.DATETIME_FORMAT, CultureInfo.InvariantCulture).ToString(Constant.DATE_API_FORMAT);
                if (endDate != null)
                {
                    var endDateFormatted = DateTime.ParseExact(endDate.ToString(), Constant.DATETIME_FORMAT, CultureInfo.InvariantCulture).ToString(Constant.DATE_API_FORMAT);
                    if (date.CompareTo(endDate) > 0)
                    {
                        DateTime tmp = date;
                        date = endDate;
                        endDate = tmp;
                    }
                    measuresSamples = id.GetMeasuresSamplesApi(null, dateFormatted, endDateFormatted);
                }
                else
                    measuresSamples = id.GetMeasuresSamplesApi(null, dateFormatted);
            }
            else
                measuresSamples = id.GetMeasuresSamplesApi(null);

            int i = 0;
   
            foreach (Models.ForApi.Fitbit fitbit in measuresSamples.fitbit_samples)
            {
                if (i <= Constant.LIMIT_SAMPLES)
                {
                    measures.fitbitArray.Date[i] = fitbit.Timedate;
                    measures.fitbitArray.Avg_heartbeats[i] = fitbit.Avg_heartbeats;
                    measures.fitbitArray.Calories[i] = fitbit.Calories;
                    measures.fitbitArray.Elevation[i] = fitbit.Elevation;
                    measures.fitbitArray.Floors[i] = fitbit.Floors;
                    measures.fitbitArray.Steps[i] = fitbit.Steps;
                    measures.fitbitArray.Distance[i] = fitbit.Distance;
                    measures.fitbitArray.Minutes_asleep[i] = fitbit.Minutes_asleep;
                    measures.fitbitArray.Minutes_awake[i] = fitbit.Minutes_awake;
                }
                else
                    break;
                i++;
            }

            i = 0;
            foreach (Models.ForApi.Hue hue in measuresSamples.hue_samples)
            {
                if (i <= Constant.LIMIT_SAMPLES)
                {
                    measures.hueArray.Date[i] = hue.Timedate;
                    if (hue.Chromotherapy.Equals("Hard"))
                    {
                        measures.hueArray.Hard[i] = 1;
                        measures.hueArray.Soft[i] = 0;
                    }
                    else
                    {
                        measures.hueArray.Hard[i] = 0;
                        measures.hueArray.Soft[i] = 1;
                    }
                }
                else
                    break;
                i++;
            }

            i = 0;
            foreach (Models.ForApi.Sensor sensor in measuresSamples.sensor_samples)
            {
                if (i <= Constant.LIMIT_SAMPLES)
                {
                    measures.sensorArray.Date[i] = sensor.Timedate;
                    measures.sensorArray.Temperature[i] = sensor.Temperature;
                    measures.sensorArray.Luminescence[i] = sensor.Luminescence;
                    measures.sensorArray.Humidity[i] = sensor.Humidity;

                }
                else
                    break;
                i++;
            }

            return measures;
        }
    }
}



/* 
 
     Home -> niente

    Measures -> apertura base -> ultimi N giorni per 3 grafici fitbit, hue, sensor -> 
                richiesta form di total tra due date -> 
                    api total -> asse date è lo stesso per tutti +
                        fitbit -> array per heartbeats, calories, elevation, floors, steps, distance, minutes_asleep, minutes_awake
                        hue -> array per hard, soft
                        sensor -> array per temperature, luminescence, humidity
     
     
            -> richiesta form di samples -> come grafico settiamo un limite di dati a N
            -> per ogni grafico c'è un array di timedate diverso a seconda dei valori contenuti.
               con i relativi array per dispositivi
     
     
    Medic -> dati medico, dati login, lista di pazienti

    Messaggi -> lista di messaggi ricevuti, lista di messaggi inviati

    Patient -> dati paziente, dati login, dati patient_initial, lista dei medici

    Task -> apertura scheduler con tutti i task divisi nelle 3 categorie  
        creare un unica lista con in più il campo type (general, activity, diet) partendo dalle 3 api task
        
    Weight -> apertura -> grafico con asse date ultimi N giorni -> array date
                -> array pesi
                -> dato Patient_Initial creo due array per i pesi di soglia

            -> form richiesta fino a N dati massimi
                -> array date, array pesi, array pesi soglia



     
     */
