using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.data.admin
{
    public class FieldInfo
    {
        private string name;
        private string isNull;
        private string dataType;
        private string maxLength;

        public string MaxLength
        {
            get { return maxLength; }
            set { maxLength = value; }
        }

        public string DataType
        {
            get { return dataType; }
            set { dataType = value; }
        }

        public string IsNull
        {
            get { return isNull; }
            set { isNull = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
