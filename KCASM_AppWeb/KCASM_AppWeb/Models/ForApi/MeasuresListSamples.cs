using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForApi
{
    public class MeasuresListSamples
    {
        public List<Fitbit> Fitbit_samples { get; set; } = new List<Fitbit>();

        public List<Hue> Hue_samples { get; set; } = new List<Hue>();

        public List<Sensor> Sensor_samples { get; set; } = new List<Sensor>();

    }
}
