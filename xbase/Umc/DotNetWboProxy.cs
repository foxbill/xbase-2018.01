using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using xbase.Exceptions;
using xbase.local;

namespace xbase.umc
{
    /// <summary>
    /// wbo对象执行代理
    /// </summary>
    public class DotNetWboProxy : BaseWboProxy, xbase.umc.WboProxy
    {
        //private WboSchema wboSchema;
        //private Type type;
        //private Assembly assembly;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="objectTypeName">对象注册名</param>
        public DotNetWboProxy(string objectTypeName) :
            base(objectTypeName)
        {
        }

        /// <summary>
        /// 通过Wbo对象构架（WboSchema） 加载代理
        /// </summary>
        /// <param name="wboSchema">Wbo对象构架</param>
        public DotNetWboProxy(WboSchema wboSchema) :
            base(wboSchema)
        {
        }

        protected override void loadAssembly()
        {
            //if (!RegMachine.CheckRegFile())
            //{
            //    if (RegMachine.CheckTimeOut())
            //        throw new Exception("software not regist! and try use time out");
            //}
            this.assembly = AssemblyPool.getAssembly(wboSchema);
            this.type = this.assembly.GetType(wboSchema.ClassName);
            if (this.type == null)
                throw new Exception(string.Format(Lang.TypeCannotLoad, assembly.FullName, wboSchema.ClassName));
        }

        public override object createObject(string objectName)
        {
            Type t = assembly.GetType(wboSchema.ClassName, true);
            ConstructorInfo c = t.GetConstructor(new Type[] { typeof(string) });
            if (c != null)
                return c.Invoke(new object[] { objectName });
            else
                return createObject(new string[] { objectName });
        }

        public override object createObject(string[] jsonParams)
        {
            ConstructorInfo[] constructors = this.type.GetConstructors();
            if (constructors.Count() == 0)
                return null;

            if (jsonParams == null || jsonParams.Length == 0)
                return assembly.CreateInstance(wboSchema.ClassName, true);

            //Type[] paramTypes = new Type[jsonParams.Count()];
            for (int i = 0; i < constructors.Length; i++)
            {
                ConstructorInfo c = constructors[i];
                object[] ps = convertParams(c, jsonParams);
                if (ps != null)
                    return c.Invoke(ps);
            }
            return assembly.CreateInstance(wboSchema.ClassName, true);
        }

        public override object createObject(Dictionary<string, string> jsonNamedParams)
        {
            ConstructorInfo[] constructors = this.type.GetConstructors();
            if (constructors.Length == 0)
                return null;

            if (jsonNamedParams == null || jsonNamedParams.Count() == 0)
                return assembly.CreateInstance(wboSchema.ClassName, true);
            //            Type[] paramTypes = new Type[jsonNamedParams.Count()];
            for (int i = 0; i < constructors.Length; i++)
            {
                ConstructorInfo c = constructors[i];
                object[] ps = convertParams(c, jsonNamedParams);
                if (ps != null)
                    return c.Invoke(ps);
            }
            return assembly.CreateInstance(wboSchema.ClassName, true);
        }

    }
}
