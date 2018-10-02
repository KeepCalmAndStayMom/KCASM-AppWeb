using System.Collections.Generic;

namespace KCASM_AppWeb.Models
{
    public class User
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Homestation_id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public int Height { get; set; }

        public int Weight { get; set; }

        public int Medic_id { get; set; }

        public Dictionary<string, Task> Tasks { get; set; }

        public List<string> DateFitbit { get; set; }

        public List<string> DateHue { get; set; }

        public List<string> DateSensor { get; set; }

        public FitbitList FitbitList { get; set; }

        public HueList HueList { get; set; }

        public SensorList SensorList { get; set; }
    }
}
