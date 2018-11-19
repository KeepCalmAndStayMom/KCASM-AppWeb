using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForView
{
    public class SensorList
    {
        public List<String> Date { get; set; } = new List<string>();

        public List<Double?> Temperature { get; set; } = new List<double?>();

        public List<Double?> Luminescence { get; set; } = new List<double?>();

        public List<Double?> Humidity { get; set; } = new List<double?>();
    }
}
