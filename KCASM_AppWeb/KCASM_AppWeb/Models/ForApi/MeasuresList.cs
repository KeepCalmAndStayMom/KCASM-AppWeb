using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForApi
{
    public class MeasuresList
    {
        public List<Fitbit> fitbit_samples { get; set; }

        public List<Hue> hue_samples { get; set; }

        public List<Sensor> sensor_samples { get; set; }

        public List<Fitbit> fitbit_total { get; set; }

        public List<Hue> hue_total { get; set; }

        public List<Sensor> sensor_total { get; set; }
    }
}
