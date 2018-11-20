using KCASM_AppWeb.Models.ForApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForView
{
    public class Message
    {
        public List<MessageMedicPatient> Messages_sent { get; set; } = new List<MessageMedicPatient>();

        public List<MessageMedicPatient> Messages_received { get; set; } = new List<MessageMedicPatient>();

        public List<MedicsForPatient> Medics { get; set; } = new List<MedicsForPatient>();

        public List<PatientsForMedic> Patients { get; set; } = new List<PatientsForMedic>();
    }
}
