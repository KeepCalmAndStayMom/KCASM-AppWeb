using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForApi
{
    public class MeasuresTotal
    {
        public Fitbit Fitbit_total { get; set; }

        public HueTotal Hue_total { get; set; }

        public Sensor Sensor_total { get; set; }
    }
}
