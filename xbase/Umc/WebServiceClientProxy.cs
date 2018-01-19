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
    public static class WebServiceClientProxy
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Type GetTypeofWebService(string Url)
        {
            try
            {
                WebClient web = new WebClient();
                Stream stream = web.OpenRead(Url);
                ServiceDescription description = ServiceDescription.Read(stream);
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
                parameter.GenerateInMemory = true;
                // parameter.OutputAssembly = ".\\wsdl.dll"; // 可以指定你所需的任何文件名。
                parameter.ReferencedAssemblies.Add("System.dll");
                parameter.ReferencedAssemblies.Add("System.XML.dll");
                parameter.ReferencedAssemblies.Add("System.Web.Services.dll");
                parameter.ReferencedAssemblies.Add("System.Data.dll");

                CompilerResults result = provider.CompileAssemblyFromDom(parameter, unit);

                Type t = null;
                if (!result.Errors.HasErrors)
                {

                    Assembly asm = result.CompiledAssembly;
                    t = asm.GetType("Service");
                }
                return t;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private static HttpWebRequest getRequest(string funcName, Dictionary<string, object> args, string webNS, string Url)
        {
            if (webNS.LastIndexOf('/') != webNS.Length - 1)
                webNS += "/";
            SoapXmlBuildController con = new SoapXmlBuildController(new SoapVer11XmlBuilder());
            con.BuildXml(funcName, args, webNS);
            string soapXml = con.GetXml();

            byte[] bs = Encoding.UTF8.GetBytes(soapXml);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(Url));
            request.ContentType = "text/xml;charset=utf-8";
            request.Method = "POST";
            request.Headers.Add("SOAPAction", webNS + funcName);
            request.ContentLength = bs.Length;

            using (Stream reqStream = request.GetRequestStream())
            {

                reqStream.Write(bs, 0, bs.Length);

            }
            return request;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="funcName"></param>
        /// <param name="args"></param>
        /// <param name="webNS"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Invoke(string funcName, Dictionary<string, object> args, string webNS, string Url)
        {


            try
            {
                HttpWebRequest request = getRequest(funcName, args, webNS, Url);
                string responseString = "";
                using (HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse())
                {

                    StreamReader sr = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);

                    responseString = sr.ReadToEnd();
                }


                XmlDocument xd = new XmlDocument();
                xd.LoadXml(responseString);
                XmlNodeList xnl = xd.GetElementsByTagName(funcName + "Result");
                string resultXml = xnl.Item(0).InnerXml;
                return resultXml;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private static void onResponse(IAsyncResult result)
        {
            HttpWebRequest req = result.AsyncState as HttpWebRequest;
            using (HttpWebResponse resp = req.EndGetResponse(result) as HttpWebResponse)
            {
                StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
                string responseString = sr.ReadToEnd();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="funcName"></param>
        /// <param name="args"></param>
        /// <param name="webNS"></param>
        /// <param name="url"></param>
        public static void InvokeAsync(string funcName, Dictionary<string, object> args, string webNS, string Url)
        {
            try
            {
                HttpWebRequest request = getRequest(funcName, args, webNS, Url);
                IAsyncResult res = request.BeginGetResponse(new AsyncCallback(onResponse), request);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


    }
}
