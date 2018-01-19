using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using xbase.Exceptions;

namespace xbase
{
    public static class SchemaFile
    {
        public static T LoadSchema<T>(string fileName)
           where T : Schema
        {
            T ret = null;

            XmlReader xmlReader = XmlReader.Create(fileName);

            try
            {
                if (xmlReader == null) throw (new EContainerCanNotOpenSchameFile());
                XmlSerializer xmls = new XmlSerializer(typeof(T));
                ret = (T)xmls.Deserialize(xmlReader);
            }
            finally
            {
                xmlReader.Close();
            }
            return ret;


        }

        public static void SaveSchema<T>(T schema, string fileName)
           where T : Schema
        {
            XmlWriter writer = XmlWriter.Create(fileName);
            try
            {
                XmlSerializer xmls = new XmlSerializer(typeof(T));
                xmls.Serialize(writer, schema);
            }
            finally
            {
                writer.Close();
            }
        }
    }


}
