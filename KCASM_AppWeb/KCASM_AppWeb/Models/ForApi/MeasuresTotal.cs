using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForApi
{
    public class MeasuresTotal : Measures
    {
        public Fitbit fitbit_total { get; set; }

        public HueTotal hue_total { get; set; }

        public Sensor sensor_total { get; set; }
    }
}
