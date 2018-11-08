using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCASM_AppWeb.ExtensionMethods
{
    public static class ExtensionLogin
    {
        private static Dictionary<string, LinkedList<string>> sessionKey = CreateDictionarySession();

        private static Dictionary<string, LinkedList<string>> CreateDictionarySession() {
            sessionKey = new Dictionary<string, LinkedList<string>>();

            LinkedList<string> list = new LinkedList<string>();
            list.AddFirst("Patient");
            list.AddFirst("MedicPatient");
            list.AddFirst("Admin");
            sessionKey.Add("Measures", list);

            list = new LinkedList<string>();
            list.AddFirst("Medic");
            list.AddFirst("MedicPatient");
            list.AddFirst("Admin");
            sessionKey.Add("Medic", list);

            list = new LinkedList<string>();
            list.AddFirst("Patient");
            list.AddFirst("Medic");
            list.AddFirst("Admin");
            sessionKey.Add("Message", list);

            list = new LinkedList<string>();
            list.AddFirst("Patient");
            list.AddFirst("MedicPatient");
            list.AddFirst("Admin");
            sessionKey.Add("Patient", list);

            list = new LinkedList<string>();
            list.AddFirst("Patient");
            list.AddFirst("Medic");
            list.AddFirst("MedicPatient");
            list.AddFirst("Admin");
            sessionKey.Add("Tasks", list);

            list = new LinkedList<string>();
            list.AddFirst("Patient");
            list.AddFirst("MedicPatient");
            list.AddFirst("Admin");
            sessionKey.Add("Weight", list);

            return sessionKey;
        }

        public static bool checkSession(this string controller, string type)
        {
            if (type != null)
            {
                LinkedList<string> list = sessionKey.GetValueOrDefault(controller);
                if (list.Contains(type))
                    return true;
            }
            return false;
        }
    }
}
