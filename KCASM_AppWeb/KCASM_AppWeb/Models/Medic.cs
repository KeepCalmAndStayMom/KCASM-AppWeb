using System.Collections.Generic;

namespace KCASM_AppWeb.Models
{
    public class Medic
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public Dictionary<string, User> Patients { get; set; }
    }
}
