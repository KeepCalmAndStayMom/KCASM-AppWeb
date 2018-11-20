using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForView
{
    public class Weights
    {
        public List<String> Date { get; set; } = new List<string>();

        public List<Double?> Weight { get; set; } = new List<double?>();

        public List<Double> UpperThreshold { get; set; } = new List<double>();

        public List<Double> LowerThreshold { get; set; } = new List<double>();

        public Double Min { get; set; }

        public Double Max { get; set; }
    }
}
