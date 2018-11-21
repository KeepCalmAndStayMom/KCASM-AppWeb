using KCASM_AppWeb.Models.ForApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForView
{
    public class Tasks
    {
        public List<Task> TaskList { get; set; } = new List<Task>();

        public CategoryTask CategoryTask { get; set; } = new CategoryTask();
    }
}
