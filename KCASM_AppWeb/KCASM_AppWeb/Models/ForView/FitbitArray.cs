using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForView
{
    public class FitbitArray
    {
        public String[] Date { get; set; }

        public Int16?[] Avg_heartbeats { get; set; }

        public Int16?[] Calories { get; set; } 

        public Double?[] Elevation { get; set; }

        public Int16?[] Floors { get; set; }

        public Int16?[] Steps { get; set; }

        public Double?[] Distance { get; set; }

        public Int16?[] Minutes_asleep { get; set; }

        public Int16?[] Minutes_awake { get; set; }
    }
}