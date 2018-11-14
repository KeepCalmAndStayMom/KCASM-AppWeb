
using KCASM_AppWeb.Models.ForView;
using System;
using System.Collections.Generic;
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

            medic.Email = apiLogin.Email;
            medic.Email_notify = apiLogin.Email_notify;
            medic.Sms_notify = apiLogin.Sms_notify;

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

            foreach (Models.ForApi.Task t in general.Activities)
                tasks.TaskList.Add(new Models.ForView.Task(t, "Activities"));

            foreach (Models.ForApi.Task t in general.Diets)
                tasks.TaskList.Add(new Models.ForView.Task(t, "Diets"));

            return tasks;
        }

        public static Weights GetWeights(this Models.ForApi.WeightsList weights, Models.ForApi.PatientInitial patientInitial)
        {

            return null;
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

            patient.Pregnancy_start_date = patientInitial.Pregnancy_start_date;
            patient.Weight = patientInitial.Weight;
            patient.Height = patientInitial.Height;
            patient.Bmi = patientInitial.Bmi;
            patient.Twin = patientInitial.Twin;

            patient.Email = login.Email;
            patient.Email_notify = login.Email_notify;
            patient.Sms_notify = login.Sms_notify;

            patient.Medics = new List<MedicsForPatient>();

            foreach (Models.ForApi.Medic medic in medics)
                patient.Medics.Add(new MedicsForPatient(medic.Id, medic.Name, medic.Surname, medic.Specialization));
            return null;
        }

        public static Measures GetMeasuresTotal(this Int16 id, String date, String endDate)
        {

            return null;
        }

        public static Measures GetMeasuresSamples(this Int16 id, String date, String endDate)
        {

            return null;
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
