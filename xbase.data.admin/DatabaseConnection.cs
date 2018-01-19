using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.data.admin
{
    public class DatabaseConnection
    {
        private List<SingleDatabaseConnection> connections = new List<SingleDatabaseConnection>();

        public List<SingleDatabaseConnection> Connections
        {
            get { return connections; }
            set { connections = value; }
        }
    }

    public class SingleDatabaseConnection
    {
        private string name;        
        private string connectionName;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string ConnectionName
        {
            get { return connectionName; }
            set { connectionName = value; }
        }
    }
}
