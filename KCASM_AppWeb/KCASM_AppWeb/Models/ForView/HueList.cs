using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.Models.ForView
{
    public class HueList
    {
        public List<String> Date { get; set; } = new List<string>();

        public List<Int16> Hard { get; set; } = new List<short>();

        public List<Int16> Soft { get; set; } = new List<short>();
    }
}
