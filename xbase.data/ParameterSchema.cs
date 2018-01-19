using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;

namespace xbase.data
{
    public class ParameterSchema : Schema
    {
        private string defaultValue;
        private DbType dataType;
        private ParameterDirection direction;
        public int DataSize = sizeof(int);

        public ParameterDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public DbType DataType
        {
            get { return dataType; }
            set { dataType = value; }
        }

        public string DefaultValue
        {
            get { return defaultValue; }
            set { defaultValue = value; }
        }
    }
}
