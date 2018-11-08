using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForApi
{
    public class Login
    {
        public String Email { get; set; }

        public String Password { get; set; }

        public Int16 Patient_id { get; set; }

        public Int16 Medic_id { get; set; }

        public Boolean Email_notify { get; set; }

        public Boolean Sms_notify { get; set; }

    }
}
