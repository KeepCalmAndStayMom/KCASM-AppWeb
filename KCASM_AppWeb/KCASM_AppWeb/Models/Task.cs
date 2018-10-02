using System;

namespace KCASM_AppWeb.Models
{
    public class Task
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Programmed_date { get; set; }

        public Boolean Executed { get; set; }
    }
}