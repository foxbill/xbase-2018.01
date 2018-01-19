using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web.Services.Description;
using System.Xml;
using System.Xml.Serialization;
namespace xbase.umc
{
    /// <summary>
    /// 
    /// </summary>
    public class WebServiceAssemblyProxy : BaseAssemblyProxy
    {
        private Assembly assembly;
        private string assemblyFile;



        private string saveWSDL(string url)
        {

            string wbdlPath = XSite.AppSchemaPath + "wsdl\\";
            if (!Directory.Exists(wbdlPath))
                Directory.CreateDirectory(wbdlPath);

            Uri uri = new Uri(url);
            StringBuilder sb = new StringBuilder();
            sb.Append(wbdlPath);
            sb.Append(uri.Host);
            sb.Append(".");
            sb.Append(uri.Port);
            sb.Append(Path.GetFileNameWithoutExtension(uri.AbsolutePath.Replace("/", ".")));
            sb.Append(".wsdl");
            wbdlPath = sb.ToString();
            //              wbdlPath =wbdlPath+ Path.GetFileNameWithoutExtension(wbdlPath) + ".wbdl";

            WebClient web = new WebClient();

            Stream stream = web.OpenRead(url);
            using (Stream localFile = new FileStream(wbdlPath, FileMode.OpenOrCreate))
            {
                byte[] b = new byte[5000];
                int readSize = stream.Read(b, 0, b.Length);
                while (readSize > 0)
                {
                    localFile.Write(b, 0, readSize);
                    readSize = stream.Read(b, 0, b.Length);
                }
            }
            return wbdlPath;
        }




        public WebServiceAssemblyProxy(string src) :
            base(src)
        {
            string wbdlPath = saveWSDL(src);
            ServiceDescription description = ServiceDescription.Read(wbdlPath);
            ServiceDescriptionImporter importer = new ServiceDescriptionImporter();

            importer.ProtocolName = "Soap";
            importer.Style = ServiceDescriptionImportStyle.Client;
            importer.CodeGenerationOptions = CodeGenerationOptions.GenerateProperties | CodeGenerationOptions.GenerateNewAsync;

            importer.AddServiceDescription(description, null, null);

            CodeNamespace nmspace = new CodeNamespace();
            CodeCompileUnit unit = new CodeCompileUnit();
            unit.Namespaces.Add(nmspace);

            ServiceDescriptionImportWarnings warning = importer.Import(nmspace, unit);
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

            CompilerParameters parameter = new CompilerParameters();
            parameter.GenerateExecutable = false;
            parameter.GenerateInMemory = false;
            assemblyFile = wbdlPath + ".dll";
            parameter.OutputAssembly = assemblyFile; // 可以指定你所需的任何文件名。
            parameter.ReferencedAssemblies.Add("System.dll");
            parameter.ReferencedAssemblies.Add("System.XML.dll");
            parameter.ReferencedAssemblies.Add("System.Web.Services.dll");
            parameter.ReferencedAssemblies.Add("System.Data.dll");

            CompilerResults result = provider.CompileAssemblyFromDom(parameter, unit);
            this.assembly = result.CompiledAssembly;
        }


        protected override AssemblyCategory getAssemblyCategory()
        {
            return umc.AssemblyCategory.DotNet;
        }

        protected override Assembly getAssembly()
        {
            return this.assembly;
        }

        public override void regAssembly()
        {
            Type[] types = this.assembly.GetTypes();
            if (types.Length < 1)
                throw new Exception("在程序集"+assembly.FullName+"中,没有发现任何类");

            Type type = types[0];

            WboSchema os = WboSchemaRegisterUtils.BuildObjectSchema<WboSchema>(type);
            os.Src = src;
            os.AssemblyCategory = AssemblyCategory.DotNet;
            os.AssemblyFile = assemblyFile;
            os.Id = Path.GetFileNameWithoutExtension(assemblyFile) + "." + type.FullName;
            if (!WboSchemaContainer.Instance().Contains(os.Id))
            {
                WboSchemaContainer.Instance().AddItem(os.Id, os);
            }
            else
            {
                WboSchemaContainer.Instance().UpdateItem(os.Id, os);
            }
        }
    }
}
