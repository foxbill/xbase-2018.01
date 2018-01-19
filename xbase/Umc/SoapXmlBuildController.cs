using System.Collections.Generic;

namespace xbase.umc
{
    public class SoapXmlBuildController
    {
        protected SoapXmlBuilder builder;
        public SoapXmlBuildController(SoapXmlBuilder builder)
        {
            this.builder = builder;
        }
        public void BuildXml(string funcname, Dictionary<string, object> args, string targetNamespace)
        {
            if (builder != null)
            {
                builder.BeginDoc();
                builder.BeginHeader();
                builder.BuildHeaderContent();
                builder.EndHeader();
                builder.BeginBody();
                builder.BuildBodyContent(funcname, args, targetNamespace);
                builder.EndHeader();
                builder.EndBody();
                builder.EndDoc();
            }
        }
        public string GetXml()
        {
            if (builder != null)
            {
                return builder.Xml;
            }
            return "";
        }

    }
}
