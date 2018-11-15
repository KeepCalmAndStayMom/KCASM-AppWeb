using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForView
{
    public class Weights
    {
        public String[] Date { get; set; }

        public Double?[] Weight { get; set; }

        public Double[] UpperThreshold { get; set; }

        public Double[] LowerThreshold { get; set; }
    }
}
