using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.security.schema;

namespace xbase.security
{
    public class SecuritySettingsFile
    {
        static private SecuritySettingsFile _instance;
        static private SecuritySettingSchema _schema;

        private SecuritySettingsFile()
        {
            if (_schema == null)
            {
                _schema = new SecuritySettingSchema();
                _schema.ConnectionName = "";
            }

        }

        public static void Open(string fullFileName)
        {
            _schema = xbase.SchemaFile.LoadSchema<SecuritySettingSchema>(fullFileName);
            _instance = new SecuritySettingsFile();
        }

        public static SecuritySettingsFile Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new Exception("SecuritySetting not open");
                }
                return _instance;
            }
        }

        public string ConnectionString
        {
            get
            {
                return _schema.ConnectionName;
            }
        }

    }
}
