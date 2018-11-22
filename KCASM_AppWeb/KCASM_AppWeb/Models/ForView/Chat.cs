using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForView
{
    public class Chat
    {
        public Int16 Id { get; set; }

        public String Name { get; set; }

        public String Surname { get; set; }

        public List<Models.ForApi.MessageMedicPatient> MessageList { get; set; } = new List<Models.ForApi.MessageMedicPatient>();
    }
}
