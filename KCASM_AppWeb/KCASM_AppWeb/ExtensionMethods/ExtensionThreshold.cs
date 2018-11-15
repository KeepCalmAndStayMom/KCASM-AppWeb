using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.ExtensionMethods
{
    public static class ExtensionThreshold
    {

        public static Double[] GetUpperThreshold(this string bmi, Boolean twin, DateTime date, DateTime endDate)
        {
            List<Double> upperThreshold = new List<Double>();



            return upperThreshold.ToArray();
        }

        public static Double[] GetLowerThreshold(this string bmi, Boolean twin, DateTime date, DateTime endDate)
        {
            return null;
        }
    }
}
