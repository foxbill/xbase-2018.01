using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Services.Description;
using System.Xml;
using System.Xml.Serialization;
using xbase.Exceptions;

namespace xbase.umc
{
    public static class XAssemblyBuilder
    {
        public const string wboDllRelaPath = "App_Data\\bin\\";
        public const string errFileExt = ".errs";
        public static string wboDllPath { get { return System.AppDomain.CurrentDomain.BaseDirectory + wboDllRelaPath; } }

        private static string saveWSDL(string url)
        {

            if (!Directory.Exists(wboDllPath))
                Directory.CreateDirectory(wboDllPath);

            Uri uri = new Uri(url);

            StringBuilder sb = new StringBuilder();
            sb.Append(wboDllPath);
            sb.Append(Path.GetFileNameWithoutExtension(uri.AbsolutePath));
            sb.Append(".wsdl");
            string wsdl = sb.ToString();

            WebClient web = new WebClient();
            Stream stream = web.OpenRead(url);
            if (File.Exists(wsdl))
                File.Delete(wsdl);
            using (Stream localFile = new FileStream(wsdl, FileMode.OpenOrCreate))
            {
                byte[] b = new byte[5000];
                int readSize = stream.Read(b, 0, b.Length);
                while (readSize > 0)
                {
                    localFile.Write(b, 0, readSize);
                    readSize = stream.Read(b, 0, b.Length);
                }
            }
            return wsdl;
        }

        public static CompilerErrorCollection getCodeErrs(string sourceFilePath)
        {
            string fileName = sourceFilePath + errFileExt;

            XmlReader xmlReader = XmlReader.Create(fileName);

            try
            {
                if (xmlReader == null) throw (new XException("保存编译错误文件失败", fileName));
                XmlSerializer xmls = new XmlSerializer(typeof(CompilerErrorCollection));
                return (CompilerErrorCollection)xmls.Deserialize(xmlReader);
            }
            finally
            {
                xmlReader.Close();
            }

        }

        private static string saveErr(string sourceFilePath, CompilerErrorCollection errs)
        {
            string fileName = sourceFilePath + ".errs";
            XmlWriter writer = XmlWriter.Create(fileName);
            try
            {
                XmlSerializer xmls = new XmlSerializer(typeof(CompilerErrorCollection));
                xmls.Serialize(writer, errs);
            }
            finally
            {
                writer.Close();
            }
            return fileName;
        }

        public static string CompileFile(String sourceFile, string lang = "CSharp")
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider(lang);

            CompilerParameters cp = new CompilerParameters();

            string dllFile = wboDllPath + Path.GetFileName(sourceFile) + ".dll";
            // Generate an executable instead of 
            // a class library.
            cp.GenerateExecutable = false;

            // Set the assembly file name to generate.
            cp.OutputAssembly = dllFile;

            // Generate debug information.
            cp.IncludeDebugInformation = true;

            // Add an assembly reference.
            cp.ReferencedAssemblies.Add("System.dll");

            // Save the assembly as a physical file.
            cp.GenerateInMemory = false;

            // Set the level at which the compiler 
            // should start displaying warnings.
            cp.WarningLevel = 3;

            // Set whether to treat all warnings as errors.
            cp.TreatWarningsAsErrors = false;

            // Set compiler argument to optimize output.
            cp.CompilerOptions = "/optimize";

            // Set a temporary files collection.
            // The TempFileCollection stores the temporary files
            // generated during a build in the current directory,
            // and does not delete them after compilation.
            //            cp.TempFiles = new TempFileCollection(".", true);

            //if (provider.Supports(GeneratorSupport.EntryPointMethod))
            //{
            //    // Specify the class that contains 
            //    // the main method of the executable.
            //    cp.MainClass = "Samples.Class1";
            //}

            //if (provider.Supports(GeneratorSupport.Resources))
            //{
            //    // Set the embedded resource file of the assembly.
            //    // This is useful for culture-neutral resources, 
            //    // or default (fallback) resources.
            //    cp.EmbeddedResources.Add("Default.resources");

            //    // Set the linked resource reference files of the assembly.
            //    // These resources are included in separate assembly files,
            //    // typically localized for a specific language and culture.
            //    cp.LinkedResources.Add("nb-no.resources");
            //}

            // Invoke compilation.
            CompilerResults cr = provider.CompileAssemblyFromFile(cp, sourceFile);

            if (cr.Errors.Count > 0)
                return saveErr(sourceFile, cr.Errors);
            else
                return dllFile;
            // Return the results of compilation.
        }



        public static string buildWSDL(string url)
        {

            string wbdlPath = saveWSDL(url);
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
            string assemblyFile = wbdlPath + ".dll";
            parameter.OutputAssembly = assemblyFile;
            parameter.ReferencedAssemblies.Add("System.dll");
            parameter.ReferencedAssemblies.Add("System.XML.dll");
            parameter.ReferencedAssemblies.Add("System.Web.Services.dll");
            parameter.ReferencedAssemblies.Add("System.Data.dll");

            CompilerResults result = provider.CompileAssemblyFromDom(parameter, unit);
            //this.assembly = result.CompiledAssembly;
            return assemblyFile;
        }
    }
}
