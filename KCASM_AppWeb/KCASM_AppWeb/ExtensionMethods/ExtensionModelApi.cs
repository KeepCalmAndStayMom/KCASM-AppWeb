using KCASM_AppWeb.Models.ForApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.ExtensionMethods
{
    public class ExtensionModelApi
    {
        public static Patient getPatient(Int16 id)
        {
            //prendo gli input
            //costruisco la stringa di URL
            //eseguo get
            //parsifico nel model relativo
            //ritorno il model
            return null;
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
            filtri total ?date

        login -> patients/id/login_data -> mappa diretta


        medico -> medics/id -> mappa diretta

        pazienti collegati -> medics/id/patients -> lista diretta

        task|message|login sono uguali ai patient


         */
    }
}
