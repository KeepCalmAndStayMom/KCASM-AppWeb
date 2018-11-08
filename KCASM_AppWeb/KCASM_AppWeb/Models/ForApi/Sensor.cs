using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForApi
{
    public class Sensor
    {
        public Int16 Patient_id { get; set; }

        public String Timedate { get; set; }

        public Double Temperature { get; set; }

        public Double Luminescence { get; set; }

        public Double Humidity { get; set; }

    }
}
