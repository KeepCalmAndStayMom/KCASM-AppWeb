using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForApi
{
    public class Fitbit
    {
        public Int16 Patient_id { get; set; }

        public String Timedate { get; set; }

        public Int16 Avg_heartbeats { get; set; }

        public Int16 Calories { get; set; }

        public Double Elevation { get; set; }

        public Int16 Floors { get; set; }

        public Int16 Steps { get; set; }

        public Double Distance { get; set; }

        public Int16 Minutes_asleep { get; set; }

        public Int16 Minutes_awake { get; set; }

    }
}
