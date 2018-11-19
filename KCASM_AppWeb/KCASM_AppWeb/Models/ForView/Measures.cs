using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForView
{
    public class Measures
    {

        public FitbitList FitbitArray { get; set; } = new FitbitList();

        public HueList HueArray { get; set; } = new HueList();

        public SensorList SensorArray { get; set; } = new SensorList();

    }
}
