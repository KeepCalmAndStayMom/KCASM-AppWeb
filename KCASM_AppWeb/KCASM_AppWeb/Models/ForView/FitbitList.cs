using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForView
{
    public class FitbitList
    {
        public List<String> Date { get; set; } = new List<string>();

        public List<Int16?> Avg_heartbeats { get; set; } = new List<short?>();

        public List<Int16?> Calories { get; set; } = new List<short?>();

        public List<Double?> Elevation { get; set; } = new List<double?>();

        public List<Int16?> Floors { get; set; } = new List<short?>();

        public List<Int16?> Steps { get; set; } = new List<short?>();

        public List<Double?> Distance { get; set; } = new List<double?>();

        public List<Int16?> Minutes_asleep { get; set; } = new List<short?>();

        public List<Int16?> Minutes_awake { get; set; } = new List<short?>();
    }
}