using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForApi
{
    public class CategoryTask
    {
        public List<String> General { get; set; } = new List<string>();

        public List<String> Activities { get; set; } = new List<string>();

        public List<String> Diets { get; set; } = new List<string>();
    }
}
