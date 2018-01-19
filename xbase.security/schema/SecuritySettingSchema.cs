using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.security.schema
{
    public class SecuritySettingSchema : xbase.Schema
    {
        private string connectionName;
        private string loginPageUrl;

        public string LoginPageUrl
        {
            get { return loginPageUrl; }
            set { loginPageUrl = value; }
        }

        public string ConnectionName
        {
            get { return connectionName; }
            set { connectionName = value; }
        }

        public List<string> AdministratorObjects ;

    }
}
