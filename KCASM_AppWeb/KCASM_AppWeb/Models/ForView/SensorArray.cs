using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForView
{
    public class SensorArray
    {
        public String[] Date { get; set; }

        public Double[] Temperature { get; set; }

        public Double[] Luminescence { get; set; }

        public Double[] Humidity { get; set; }
    }
}
