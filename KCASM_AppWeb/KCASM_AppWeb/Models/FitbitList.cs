using System.Collections.Generic;

namespace KCASM_AppWeb.Models
{
    public class FitbitList
    {
        public List<int?> Heartbeats { get; set; }

        public List<float?> Calories { get; set; }

        public List<float?> Elevation { get; set; }

        public List<float?> Floors { get; set; }

        public List<float?> Steps { get; set; }

        public List<float?> Distance { get; set; }

        public List<float?> MinutesAsleep { get; set; }

        public List<float?> MinutesAwake { get; set; }
    }
}
