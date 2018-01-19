using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Serialization;

namespace xbase.data.admin
{
    class test
    {
        public static void Main()
        {
            //string connstr = "Data Source=172.16.0.41;Initial Database=Test;User Id=sa;Password=tj@2009;";

            ConnectionInfo ci = new ConnectionInfo();
            //ci.ConnectionString = connstr;
            ci.DBType = "SQL";
            ci.Name = "172.16.0.41";

            DBManager dm = new DBManager();

            string SQL = "select * from mz_ghmx";
            TableInfo ti = dm.SearchTableInfo(SQL);
        }
    }
}
