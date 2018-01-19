using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.umc
{
    public static class UmcTools
    {
        internal static string getObjName(string[] names, int start, int end)
        {
            if (names.Length - 1 < start)
                return null;
            StringBuilder sb = new StringBuilder();
            int l = names.Length;
            for (int i = start; i < l && i <= end; i++)
            {
                sb.Append(".");
                sb.Append(names[i]);
            }
            if (sb.Length > 0)
                sb.Remove(0, 1);
            return sb.ToString();
        }

        internal static string getObjName(string[] names, int start)
        {
            return getObjName(names, start, names.Length - 1);
        }
    }
}
