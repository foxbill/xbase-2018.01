using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.Validation;
using xbase.data.db;


namespace xbase.data.Validation
{
    public class UniqueValidator:BaseValidator
    {
        private string connectString;
        private string dbProvider;
        private string selectSql;

        public string ConnectString
        {
            get { return connectString; }
            set { connectString = value; }
        }

        public string SelectSql
        {
            get { return selectSql; }
            set { selectSql = value; }
        }

        public override bool Check(string value)
        {
            //XDatabaseFactory dbfact = XDatabaseFactory.Instance;
            DatabaseAdmin dba = DatabaseAdmin.getInstance(connectString);
            object o= dba.Database.ExecuteScalar("");
            return o != null;
//            db.ExecuteTable()
        }


    }
}
