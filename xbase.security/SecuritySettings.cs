using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.security.schema;
using xbase;
using System.IO;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using xbase.data.db;


namespace xbase.security
{
    internal static class SecuritySettings
    {
        private const string SETTING_FILE = "security.xml";
        private static SecuritySettingSchema _settings;

        static SecuritySettings()
        {
            if (File.Exists(SettingFileName))
                _settings = SchemaFile.LoadSchema<SecuritySettingSchema>(SettingFileName);

            if (_settings == null)
            {
                _settings = new SecuritySettingSchema();
                _settings.LoginPageUrl = "";
                _settings.ConnectionName = "";
                _settings.AdministratorObjects = new List<string>();
                _settings.AdministratorObjects.Add("/studio");
                _settings.AdministratorObjects.Add("DataExplore");
                SchemaFile.SaveSchema<SecuritySettingSchema>(_settings, SettingFileName);
            }

        }

        public static SecuritySettingSchema Settings
        {
            get { return SecuritySettings._settings; }
            set { SecuritySettings._settings = value; }
        }

        public static void ExecuteNonQuery(string sql)
        {
            Database db = GetDb();
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(sql);
        }

        public static DataTable Execute(string sql)
        {
            Database db = GetDb();
            DbCommand cmd = db.GetSqlStringCommand(sql);
            return db.ExecuteDataSet(cmd).Tables[0];
        }

        public static object ExecuteScalar(string sql)
        {
            Database db = GetDb();
            DbCommand cmd = db.GetSqlStringCommand(sql);
            return db.ExecuteScalar(cmd);
        }

        public static void CreateDefaultDb()
        {
            Database db = GetDb();
            DbCommand cmd = db.GetSqlStringCommand(SecurityDataScripts.CreateSystemTablesSQL);
            db.ExecuteNonQuery(cmd);
        }

        private static string SettingFileName
        {
            get { return XSite.AppSchemaPath + SETTING_FILE; }
        }

        public static DatabaseAdmin getDBA()
        {
            return DatabaseAdmin.getInstance(_settings.ConnectionName);
        }

        public static Database GetDb()
        {

            string connName = _settings.ConnectionName;

            if (string.IsNullOrEmpty(connName))
                connName = ConnectionAdmin.getDefaultConnName();

            Database db = DatabaseFactory.CreateDatabase(connName);
            return db;
        }
    }
}
