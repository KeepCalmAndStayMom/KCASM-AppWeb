using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForApi
{
    public class PatientInitial
    {
        public Int16 Patient_id { get; set; }

        public String Pregnancy_start_date { get; set; }

        public Double Weight { get; set; }

        public Double Height { get; set; }

        public String Bmi { get; set; }

        public Boolean Twin { get; set; }

    }
}
