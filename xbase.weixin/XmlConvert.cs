using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace xbase.weixin
{
    public static class XmlConvert
    {
        public static string getXml(object obj)
        {
            XmlSerializer ser = new XmlSerializer(obj.GetType());
            // Encoding utf8EncodingWithNoByteOrderMark = new UTF8Encoding(false);
            using (MemoryStream stream = new MemoryStream())
            {
                using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8))
                {
                    XmlDocument doc = new XmlDocument();
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add(string.Empty, string.Empty);
                    ser.Serialize(writer, obj, ns);
                    stream.Seek(0, SeekOrigin.Begin);
                    doc.Load(stream);
                    return doc.FirstChild.NextSibling.OuterXml;
                }

            }
        }

        public static T getObj<T>(string xml)
        {
            using (MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml)))
            {
                return getObj<T>(stream);
            }
        }

        public static T getObj<T>(Stream stream)
        {
            XmlSerializer xmlSer = new XmlSerializer(typeof(T));
            StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
            return (T)xmlSer.Deserialize(reader);
        }

    }
}
