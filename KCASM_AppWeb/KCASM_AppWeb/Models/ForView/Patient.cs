using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForView
{
    public class Patient
    {
        public String Name { get; set; }

        public String Surname { get; set; }

        public String Phone { get; set; }

        public String Email { get; set; }

        public String Address_home { get; set; }

        public String Address_hospital { get; set; }

        public Int16 Age { get; set; }

        public Double Height { get; set; }

        public Boolean Email_notify { get; set; }

        public Boolean Sms_notify { get; set; }

        public Boolean Twin { get; set; } //modificabile solo dal medico

        public List<MedicsForPatient> Medics { get; set; }

        public String Pregnancy_start_date { get; set; }

        public Double Weight { get; set; }

        public String Bmi { get; set; }
    }
}
