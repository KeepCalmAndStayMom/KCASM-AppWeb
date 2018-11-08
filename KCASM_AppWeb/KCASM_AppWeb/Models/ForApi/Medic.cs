using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForApi
{
    public class Medic
    {
        public Int16 Id { get; set; }

        public String Name { get; set; }

        public String Surname { get; set; }

        public Int16 Age { get; set; }

        public String Phone { get; set; }

        public String Address { get; set; }

        public String Specialization { get; set; }

    }
}
