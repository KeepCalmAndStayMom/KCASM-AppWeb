using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForView
{
    public class Message
    {
        public List<MessageMedicPatient> Message_sent { get; set; } = null;

        public List<MessageMedicPatient> Message_received { get; set; } = null;
    }
}
