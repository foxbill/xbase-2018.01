using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.data
{
    public class DataSummary
    {
        private string name = "";
        private string caption = "";
        private string description = "";
        private string type = "";

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Caption
        {
            get { return caption; }
            set { caption = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

    }
}
