using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForView
{
    public class Medic
    {
        public String Name { get; set; }

        public String Surname { get; set; }

        public Int16 Age { get; set; }

        public String Phone { get; set; }

        public String Address { get; set; }

        public String Specialization { get; set; }

        public String Email { get; set; }

        public Boolean Email_notify { get; set; }

        public Boolean Sms_notify { get; set; }

        public List<PatientsForMedic> Patients { get; set; }
    }
}
