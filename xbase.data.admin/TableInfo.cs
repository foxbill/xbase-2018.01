using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.data.admin
{
    public class TableInfo
    {
        private string name;        
        private List<FieldInfo> fieldInfo = new List<FieldInfo>();

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<FieldInfo> FieldInfo
        {
            get { return fieldInfo; }
            set { fieldInfo = value; }
        }
    }
}
