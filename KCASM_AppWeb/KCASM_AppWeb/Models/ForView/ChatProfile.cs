using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForView
{
    public class ChatProfile
    {
        public ChatProfile(Int16 id, String name, String surname, Int16 toRead)
        {
            Id = id;
            Name = name;
            Surname = surname;
            ToRead = toRead;
        }

        public Int16 Id { get; set; }

        public String Name { get; set; }

        public String Surname { get; set; }

        public Int16 ToRead { get; set; }
    }
}
