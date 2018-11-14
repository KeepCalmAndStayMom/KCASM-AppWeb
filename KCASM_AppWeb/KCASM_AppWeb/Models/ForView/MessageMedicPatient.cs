using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForView
{
    public class MessageMedicPatient
    {
        public Int16 Medic_id { get; set; }

        public Int16 Patient_id { get; set; }

        public String Timedate { get; set; }

        public String Subject { get; set; }

        public String Message { get; set; }

        public Boolean Medic_sender { get; set; }

        public Boolean Read { get; set; }
    }
}
