using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForView
{
    public class Weights
    {
        public List<String> Date { get; set; }

        public List<Double?> Weight { get; set; }

        public List<Double> UpperThreshold { get; set; }

        public List<Double> LowerThreshold { get; set; }
    }
}
