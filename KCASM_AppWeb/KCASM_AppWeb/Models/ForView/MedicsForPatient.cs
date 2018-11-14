using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForView
{
    public class MedicsForPatient
    {
        public MedicsForPatient(Int16 id, String name, String surname, String specialization)
        {
            this.Id = id;
            this.Name = name;
            this.Surname = surname;
            this.Specialization = specialization;
        }

        public Int16 Id { get; set; }

        public String Name { get; set; }

        public String Surname { get; set; }

        public String Specialization { get; set; }
    }
}
