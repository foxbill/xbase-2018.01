using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Web;
using xbase.Exceptions;
using xbase.local;

namespace xbase.umc
{
    public static class WboRegService
    {


        /// <summary>
        /// GetAssemblyVer
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Version getAssemblyVer(string filePath)
        {
            Version v = null;
            try
            {
                byte[] filedata = File.ReadAllBytes(filePath);
                Assembly asb = Assembly.Load(filedata);
                if (asb == null)
                    return v;
                v = asb.GetName().Version;

                return v;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// 注册系统内置组件，组件Id默认为组件类名
        /// </summary>
        /// <param name="type"></param>
        public static void RegisterClass(Type type)
        {
            WboSchema os = WboSchemaRegisterUtils.BuildObjectSchema<WboSchema>(type);
            os.AssemblyCategory = AssemblyCategory.DotNet;
            if (!WboSchemaContainer.Instance().Contains(os.Id))
            {
                WboSchemaContainer.Instance().AddItem(os.Id, os);
            }
        }


        /// <summary>
        /// 通过组件Id，注册组件
        /// </summary>
        /// <param name="objType">要注册的类的类型</param>
        /// <param name="comId">注册类的别名</param>
        public static void RegisterClass(Type objType, string comId)
        {
            WboSchema os = WboSchemaRegisterUtils.BuildObjectSchema<WboSchema>(objType, comId);
            os.AssemblyCategory = AssemblyCategory.DotNet;

            if (!WboSchemaContainer.Instance().Contains(os.Id))
            {
                WboSchemaContainer.Instance().AddItem(os.Id, os);
            }
        }

    }


}
