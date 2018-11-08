using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForApi
{
    public class Patient
    {
        public Int16 Id { get; set; }

        public String Name { get; set; }

        public String Surnamne { get; set; }

        public Int16 Age { get; set; }

        public String Phone { get; set; }

        public String Address_home { get; set; }

        public String Address_hospital { get; set; }
    }
}
