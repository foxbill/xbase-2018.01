using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.IO;
using System.Xml.Serialization;
using System.Configuration;
using System.Reflection;
//using XLogging;


namespace xbase.data.admin
{
    public class ConnectionAdapter
    {
        string localPath = ConfigurationSettings.AppSettings["localPath"];

        public DatabaseType GetAllDBType()
        {
            try
            {
                FileStream fs = new FileStream(localPath + "Databases.xml", FileMode.Open);
                XmlSerializer xs = new XmlSerializer(typeof(DatabaseType));
                return (DatabaseType)xs.Deserialize(fs);
            }
            catch (Exception ex)
            {
                //XLog xl = new XLog("Get All Database Type", ex.Message);
               // xl.WriteLog();

                throw ex;
            }
        }

        //public DbConnection GetConnection(ConnectionInfo ci)
        //{
        //    try
        //    {
        //        Database db = DatabaseFactory.CreateDatabase(ci.Name);
        //        return db.CreateConnection();

        //        //FileStream fs = new FileStream(localPath + "DatabaseClient.xml", FileMode.Open);
        //        //XmlSerializer xs = new XmlSerializer(typeof(DatabaseConnection));
        //        //DatabaseConnection dc = (DatabaseConnection)xs.Deserialize(fs);

        //        //string name = ci.DBType;
        //        //string connection = "";
        //        //foreach (SingleDatabaseConnection sdc in dc.Connections)
        //        //{
        //        //    if (sdc.Name == name)
        //        //    {
        //        //        connection = sdc.ConnectionName;
        //        //        break;
        //        //    }
        //        //}
                
        //        //Assembly ass = Assembly.Load("System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
        //        //Type type = ass.GetType("System.Data." + connection);
        //        //ConstructorInfo cInfo = type.GetConstructor(new Type[] { typeof(string) });
        //        //Object obj = cInfo.Invoke(new string[] { ci.ConnectionString });

        //        //return (DbConnection)obj;
        //    }
        //    catch (Exception ex)
        //    {
        //        XLog xl = new XLog("Get Connection", "Database Type:" + ci.DBType + " Connection String:" + ci.ConnectionString + " Message:" + ex.Message);
        //        xl.WriteLog();

        //        throw ex;
        //    }
        //}
    }
}
