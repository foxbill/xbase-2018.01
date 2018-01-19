using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase;
using System.Xml.Serialization;

namespace xbase.wbs.wbdl
{
    public class NameValue : Schema
    {
        private string value;

        public string Name
        {
            get { return this.Id; }
            set { this.Id = value; }
        }

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }
}
