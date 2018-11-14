using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForView
{
    public class Task
    {
        public Task(Models.ForApi.Task task, String type)
        {
            this.Id = task.Id;
            this.Type = type;
            this.Patient_id = task.Patient_id;
            this.Medic_id = task.Medic_id;
            this.Date = task.Date;
            this.Category = task.Category;
            this.Description = task.Description;
            this.Starting_program = task.Starting_program;
            this.Executed = task.Executed;
        }

        public Int16 Id { get; set; }

        public String Type { get; set; }

        public Int16 Patient_id { get; set; }

        public Int16 Medic_id { get; set; }

        public String Date { get; set; }

        public String Category { get; set; }

        public String Description { get; set; }

        public Boolean Starting_program { get; set; }

        public Boolean Executed { get; set; }
    }
}
