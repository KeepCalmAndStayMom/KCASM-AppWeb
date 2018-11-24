using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForApi
{
    public class MeasuresTotal
    {
        public List<Fitbit> Fitbit_total { get; set; } = new List<Fitbit>();

        public List<HueTotal> Hue_total { get; set; } = new List<HueTotal>();

        public List<Sensor> Sensor_total { get; set; } = new List<Sensor>();
    }
}
