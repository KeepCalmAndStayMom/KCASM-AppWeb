using System.Collections.Generic;

namespace KCASM_AppWeb.Models
{
    public class SensorList
    {
        public List<int?> Temperature { get; set; }

        public List<int?> Luminescence { get; set; }

        public List<int?> Humidity { get; set; }
    }
}
