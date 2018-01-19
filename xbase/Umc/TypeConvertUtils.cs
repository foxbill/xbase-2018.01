#define DEBUG
#undef DEBUG
using System;
using System.Reflection;
using Newtonsoft.Json;
using System.Collections.Generic;
namespace xbase.umc
{
    /// <summary>
    /// TypeConvertUtils
    /// </summary>
    public static class TypeConvertUtils
    {
        private static bool isSystemType(Type t)
        {
            if (t == null)
                return false;
            TypeCode tc = Type.GetTypeCode(t);
            if (tc >= System.TypeCode.DBNull && tc <= System.TypeCode.String)
                return true;
            return false;
        }

        private static Type getListType(Assembly assembly, string typeStr)
        {
            int index = typeStr.LastIndexOf('[');
            string genericType = typeStr.Substring(index + 1, typeStr.Length - index - 1);
            Type t = assembly.GetType(genericType);
            if (t == null)
                return null;
            Type ltype = typeof(List<>);
            Type listOfAType = ltype.MakeGenericType(new[] { t });
            return listOfAType;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="desType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object Convert(string desType, string value)
        {

            // object resultObj;
            string[] strs = desType.Split(',');

            string typeStr = strs[0];
            string assemblyStr = strs[1];

            Type t = Type.GetType(desType);
            return Convert(t, value);

            //try
            //{
            //    Assembly assembly = AssemblyPool.getAssembly(assemblyStr);
            //    if (typeStr.IndexOf("List") >= 0)
            //    {
            //        t = getListType(assembly, typeStr);
            //    }
            //    else
            //    {

            //        t = assembly.GetType(typeStr);
            //    }

            //    return Convert(t, value);

            //}
            //catch
            //{
            //    throw new Exception("type convert failed");
            //}
        }

        public static object Convert(Type t, string value)
        {
            if (t == null)
                return null;
            if (isSystemType(t))
                return System.Convert.ChangeType(value, t);
            else
                return JsonConvert.DeserializeObject(value, t);

        }
    }
}
