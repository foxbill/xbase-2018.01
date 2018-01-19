using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;


namespace xbase.umc
{
    public class SoapVer11XmlBuilder : SoapXmlBuilder
    {
        protected MemoryStream _mstream;
        protected XmlTextWriter _xtw;

        public SoapVer11XmlBuilder()
        {
            _mstream = new MemoryStream();
            _xtw = new XmlTextWriter(_mstream, Encoding.UTF8);
        }

        public override void BeginDoc()
        {

            string soap = "http://schemas.xmlsoap.org/soap/envelope/";
            string xsi = "http://www.w3.org/2001/XMLSchema-instance";
            string xsd = "http://www.w3.org/2001/XMLSchema";
            _xtw.Formatting = Formatting.None;
            _xtw.WriteStartDocument();
            _xtw.WriteStartElement("soap", "Envelope", soap);
            _xtw.WriteAttributeString("xmlns", "xsi", null, xsi);
            _xtw.WriteAttributeString("xmlns", "xsd", null, xsd);
            _xtw.WriteAttributeString("xmlns", "soap", null, soap);
        }

        public override void BeginHeader()
        {
            //
        }

        public override void BuildHeaderContent()
        {
            //
        }

        public override void EndHeader()
        {
            //
        }

        public override void BeginBody()
        {
            _xtw.WriteStartElement("soap", "Body", null);
        }

        public override void BuildBodyContent(string funcname, Dictionary<string, object> args, string targetNamespace)
        {
            _xtw.WriteStartElement(funcname);
            _xtw.WriteAttributeString(null, "xmlns", null, targetNamespace);

            if (args != null)
            {
                foreach (KeyValuePair<string, object> pair in args)
                {
                    _xtw.WriteElementString(pair.Key, pair.Value.ToString());
                }
            }
            _xtw.WriteEndElement();
        }

        public override void EndBody()
        {
            _xtw.WriteEndElement();

        }

        public override void EndDoc()
        {
            _xtw.WriteEndDocument();
            _xtw.Flush();
            this.xml = MemStreamToString(_mstream);
        }

        private string MemStreamToString(MemoryStream stream)
        {
            byte[] buffer = stream.GetBuffer();

            Decoder d = Encoding.UTF8.GetDecoder();

            char[] chars = new char[buffer.Length];

            d.GetChars(buffer, 3, (int)buffer.Length - 3, chars, 0);
            string result = new string(chars);
            return result;
        }
    }
}
