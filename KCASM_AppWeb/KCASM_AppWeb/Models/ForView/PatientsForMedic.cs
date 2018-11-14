using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForView
{
    public class PatientsForMedic
    {

        public PatientsForMedic(Int16 id, String name, String surname)
        {
            this.Id = id;
            this.Name = name;
            this.Surname = surname;
        }

        public Int16 Id { get; set; }

        public String Name { get; set; }

        public String Surname { get; set; }
    }
}
