namespace KCASM_AppWeb.ExtensionMethods
{
    /*Extension Methods relativi ai sensori*/
    public static class ExtensionSession
    {
        /*Controllo l'uguaglianza tra due stringhe considerando anche i null*/
        public static bool SessionEquals(this string session, string type)
        {
            if (session != null && type!=null)
                if (session.Equals(type))
                    return true;

            if (session == null && type == null)
                return true;

            return false;
        }

        /*Controllo la disuguaglianza tra due stringhe considerando anche i null*/
        public static bool SessionNotEquals(this string session, string type)
        {
            if (session == null)
            {
                if (type == null)
                    return false;
                else
                    return true;
            }
            else
            {
                if (type == null)
                    return true;
                else
                {
                    if (!session.Equals(type))
                        return true;
                    return false;
                }
            }

        }
    }
}
