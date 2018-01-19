using System.Collections.Generic;

namespace xbase.umc
{
    public abstract class SoapXmlBuilder
    {
        protected string xml;
        public abstract void BeginDoc();
        public abstract void BeginHeader();
        public abstract void BuildHeaderContent();
        public abstract void EndHeader();
        public abstract void BeginBody();
        public abstract void BuildBodyContent(string funcname, Dictionary<string, object> args, string targetNamespace);
        public abstract void EndBody();
        public abstract void EndDoc();

        public string Xml
        {
            get { return xml; }
        }
    }
}
