using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.data.admin
{
    public class ConnectionInfo
    {
        private string name;        
        private string connectionString;
        private string dBType;

        public string DBType
        {
            get { return dBType; }
            set { dBType = value; }
        }

        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
