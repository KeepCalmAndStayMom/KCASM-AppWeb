﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForApi
{
    public class TaskList
    {
        public List<Task> General { get; set; }

        public List<Task> Activities { get; set; }

        public List<Task> Diets { get; set; }
    }
}
