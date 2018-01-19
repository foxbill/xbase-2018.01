using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase;
using System.Xml.Serialization;

namespace xbase.wbs.wbdl
{
    public class WbdlDataSchema:Schema
    {
        private string dataType;
        private string name;
        private List<DataPropertySchema> props;

        public string DataType
        {
            get { return dataType; }
            set { dataType = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [XmlArrayItem("Prop")]
        public List<DataPropertySchema> Props
        {
            get { return props; }
            set { props = value; }

        }
    }

    public class DataPropertySchema
    {
        private string name;
        private string type;
        private string value;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public string  Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }
}
