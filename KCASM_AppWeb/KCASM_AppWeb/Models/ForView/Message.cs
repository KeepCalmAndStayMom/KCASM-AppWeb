using KCASM_AppWeb.Models.ForApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForView
{
    public class Message
    {
        public List<ChatProfile> chatProfiles { get; set; } = new List<ChatProfile>();
    }
}
