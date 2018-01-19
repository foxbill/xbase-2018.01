using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace xbase
{
    public static class TypeUtility
    {
        public static MethodInfo GetMatchMethod(Type t, string methodName, int paramCount)
        {
            methodName = methodName.Trim();
            MethodInfo[] ms = t.GetMethods();
            for (int i = 0; i < ms.Length; i++)
            {
                MethodInfo m = ms[i];
                ParameterInfo[] pis = m.GetParameters();
                if (m.Name.Equals(methodName, StringComparison.OrdinalIgnoreCase)
                  && pis.Length == paramCount)
                {
                    return m;
                }
            }
            return null;
        }



        public static MethodInfo GetMatchMethod(Type t, string methodName, string[] stringParamValues, out object[] outParamValues)
        {
            methodName = methodName.Trim();
            outParamValues = null;
            if (stringParamValues == null || stringParamValues.Length == 0)
            {
                return t.GetMethod(methodName);
            }


            MethodInfo[] ms = t.GetMethods();

            for (int i = 0; i < ms.Length; i++)
            {
                MethodInfo m = ms[i];
                ParameterInfo[] pis = m.GetParameters();
                outParamValues = new object[pis.Length];
                if (m.Name.Equals(methodName, StringComparison.OrdinalIgnoreCase)
                  && pis.Length == stringParamValues.Length)
                {
                    bool match = true;

                    for (int j = 0; j < pis.Length; j++)
                    {
                        try
                        {
                            outParamValues[j] = Convert.ChangeType(stringParamValues[j], pis[j].ParameterType);
                        }
                        catch
                        {
                            match = false;
                            break;
                        }
                    }
                    if (match)
                        return m;
                }
            }
            return null;
        }


    }
}
