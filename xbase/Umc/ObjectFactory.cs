using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;


namespace xbase.umc
{
    public static class ObjectFactory
    {
        /// <summary>
        /// 根据对象构架创建对象
        /// </summary>
        /// <param name="objectSchema"></param>
        /// <returns></returns>
        public static object CreateObject(WboSchema objectSchema)
        {
            Assembly assembly = Assembly.Load(objectSchema.AssemblyName);
            object obj = assembly.CreateInstance(objectSchema.ClassName, true);
            return obj;
        }


        public static object CreateObject(WboSchema objectSchema,string objectName)
        {
            Assembly assembly = Assembly.Load(objectSchema.AssemblyName);
            Type t = assembly.GetType(objectSchema.ClassName, true);
            ConstructorInfo c = t.GetConstructor(new Type[] { typeof(string) });
            object obj = c.Invoke(new object[] { objectName });
            return obj;
        }

    }
}
