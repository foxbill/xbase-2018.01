using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using xbase.Exceptions;
using xbase.local;

namespace xbase.umc
{
    public class AssemblyProxy
    {
        private string src;
        private Assembly assembly;
        private string assemblyFile;//不带路径的文件名

        public AssemblyProxy(string dllFilePath)
        {
            this.src = dllFilePath;
            if (string.IsNullOrEmpty(dllFilePath))
                assembly = Assembly.GetExecutingAssembly();

            else if (dllFilePath.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
            {
                assemblyFile = Path.GetFileName(dllFilePath);

                string path = XAssemblyBuilder.wboDllPath + assemblyFile;

                if (!File.Exists(path))
                    throw new XException(Lang.WboDllNotUpload, Path.GetFileName(dllFilePath));
              
                assembly = Assembly.LoadFrom(path);
                //assembly = Assembly.LoadFile(src);//依赖项未被加载
            }
            else
                assembly = Assembly.Load(dllFilePath);

        }


        public static void regType(Type type, string src, AssemblyCategory srcType, string filePath)
        {
            WboSchema os = WboSchemaRegisterUtils.BuildObjectSchema<WboSchema>(type);
            os.Src = src;
            os.AssemblyCategory = AssemblyCategory.DotNet;
            os.AssemblyFile = filePath;

            if (!WboSchemaContainer.Instance().Contains(os.Id))
            {
                WboSchemaContainer.Instance().AddItem(os.Id, os);
            }
            else
            {
                WboSchemaContainer.Instance().UpdateItem(os.Id, os);
            }
        }

        public Type[] getTypes()
        {
            return assembly.GetTypes();
        }

        public Assembly Assembly
        {
            get { return this.assembly; }
        }


        public void regAssembly()
        {
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                regType(type, src, AssemblyCategory.DotNet, assemblyFile);
            }
        }
    }
}
