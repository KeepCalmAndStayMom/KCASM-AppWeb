using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.ExtensionMethods
{
    public class ExtensionModelView
    {
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
