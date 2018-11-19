
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
            weights.Date.Add(DateTime.ParseExact(startDate.ToString(), Constant.DATETIME_FORMAT, CultureInfo.InvariantCulture).ToString(Constant.DATE_API_FORMAT));
            weights.Weight.Add(patientInitial.Weight);
            startDate = startDate.AddDays(1);

            double avg;
            double count;
            for (int i = 1; i <= Constant.WEIGHT_LIMIT; i++)
            {
                weights.Date.Add(DateTime.ParseExact(startDate.ToString(), Constant.DATETIME_FORMAT, CultureInfo.InvariantCulture).ToString(Constant.DATE_API_FORMAT));

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
                    weights.Weight.Add(avg / count);
                else
                    weights.Weight.Add(null);
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

            foreach (Models.ForApi.Medic medic in medics)
                patient.Medics.Add(new MedicsForPatient(medic.Id, medic.Name, medic.Surname, medic.Specialization));

            return patient;
        }

        public static Measures GetMeasuresTotal(this string id, DateTime date, DateTime endDate)
        {
            Measures measures = new Measures();

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
                    measures.FitbitArray.Date.Add(dateFormatted);
                    measures.HueArray.Date.Add(dateFormatted);
                    measures.SensorArray.Date.Add(dateFormatted);

                    if (measuresTotal.Fitbit_total != null)
                    {
                        measures.FitbitArray.Avg_heartbeats.Add(measuresTotal.Fitbit_total.Avg_heartbeats);
                        measures.FitbitArray.Calories.Add(measuresTotal.Fitbit_total.Calories);
                        measures.FitbitArray.Elevation.Add(measuresTotal.Fitbit_total.Elevation);
                        measures.FitbitArray.Floors.Add(measuresTotal.Fitbit_total.Floors);
                        measures.FitbitArray.Steps.Add(measuresTotal.Fitbit_total.Steps);
                        measures.FitbitArray.Distance.Add(measuresTotal.Fitbit_total.Distance);
                        measures.FitbitArray.Minutes_asleep.Add(measuresTotal.Fitbit_total.Minutes_asleep);
                        measures.FitbitArray.Minutes_awake.Add(measuresTotal.Fitbit_total.Minutes_awake);
                    }
                    else
                    {
                        measures.FitbitArray.Avg_heartbeats.Add(null);
                        measures.FitbitArray.Calories.Add(null);
                        measures.FitbitArray.Elevation.Add(null);
                        measures.FitbitArray.Floors.Add(null);
                        measures.FitbitArray.Steps.Add(null);
                        measures.FitbitArray.Distance.Add(null);
                        measures.FitbitArray.Minutes_asleep.Add(null);
                        measures.FitbitArray.Minutes_awake.Add(null);
                    }

                    if (measuresTotal.Hue_total != null)
                    {
                        measures.HueArray.Hard.Add(measuresTotal.Hue_total.Hard);
                        measures.HueArray.Soft.Add(measuresTotal.Hue_total.Soft);
                    }
                    else
                    {
                        measures.HueArray.Hard.Add(0);
                        measures.HueArray.Soft.Add(0);
                    }

                    if (measuresTotal.Sensor_total != null)
                    {
                        measures.SensorArray.Temperature.Add(measuresTotal.Sensor_total.Temperature);
                        measures.SensorArray.Luminescence.Add(measuresTotal.Sensor_total.Luminescence);
                        measures.SensorArray.Humidity.Add(measuresTotal.Sensor_total.Humidity);
                    }
                    else
                    {
                        measures.SensorArray.Temperature.Add(null);
                        measures.SensorArray.Luminescence.Add(null);
                        measures.SensorArray.Humidity.Add(null);
                    }
                }

                date = date.AddDays(1);
            }

            return measures;
        }

        public static Measures GetMeasuresSamples(this string id, DateTime date, DateTime endDate)
        {
            Measures measures = new Measures();
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
   
            foreach (Models.ForApi.Fitbit fitbit in measuresSamples.Fitbit_samples)
            {
                if (i <= Constant.LIMIT_SAMPLES)
                {
                    measures.FitbitArray.Date.Add(fitbit.Timedate);
                    measures.FitbitArray.Avg_heartbeats.Add(fitbit.Avg_heartbeats);
                    measures.FitbitArray.Calories.Add(fitbit.Calories);
                    measures.FitbitArray.Elevation.Add(fitbit.Elevation);
                    measures.FitbitArray.Floors.Add(fitbit.Floors);
                    measures.FitbitArray.Steps.Add(fitbit.Steps);
                    measures.FitbitArray.Distance.Add(fitbit.Distance);
                    measures.FitbitArray.Minutes_asleep.Add(fitbit.Minutes_asleep);
                    measures.FitbitArray.Minutes_awake.Add(fitbit.Minutes_awake);
                }
                else
                    break;
                i++;
            }

            i = 0;
            foreach (Models.ForApi.Hue hue in measuresSamples.Hue_samples)
            {
                if (i <= Constant.LIMIT_SAMPLES)
                {
                    measures.HueArray.Date.Add(hue.Timedate);
                    if (hue.Chromotherapy.Equals("Hard"))
                    {
                        measures.HueArray.Hard[i] = 1;
                        measures.HueArray.Soft[i] = 0;
                    }
                    else
                    {
                        measures.HueArray.Hard[i] = 0;
                        measures.HueArray.Soft[i] = 1;
                    }
                }
                else
                    break;
                i++;
            }

            i = 0;
            foreach (Models.ForApi.Sensor sensor in measuresSamples.Sensor_samples)
            {
                if (i <= Constant.LIMIT_SAMPLES)
                {
                    measures.SensorArray.Date.Add(sensor.Timedate);
                    measures.SensorArray.Temperature.Add(sensor.Temperature);
                    measures.SensorArray.Luminescence.Add(sensor.Luminescence);
                    measures.SensorArray.Humidity.Add(sensor.Humidity);

                }
                else
                    break;
                i++;
            }

            return measures;
        }
    }
}
