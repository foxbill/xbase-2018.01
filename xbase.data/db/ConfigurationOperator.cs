using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace xbase.data.db
{
    public class ConfigurationOperator : IDisposable
    {
        private Configuration config;

        public ConfigurationOperator() : this(null) { }
        public ConfigurationOperator(string path)
        {
            if (string.IsNullOrEmpty(path))
                path = "~";
            config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(path);
        }

        /// <summary>
        /// 返回webconfig中redirectSection的filepath
        /// </summary>
        /// <returns></returns>
        public string getRedirectConfigPath()
        {
            string strRet = "";
            //获取资源节点集合
            ConfigurationSourceSection section = (ConfigurationSourceSection)config.GetSection(ConfigurationSourceSection.SectionName);
            RedirectedSectionElement elem_r = null;
            FileConfigurationSourceElement elem = null;
            if (section != null && section.RedirectedSections.Count > 0)
            {
                elem_r = section.RedirectedSections.Get(0);
                elem = (FileConfigurationSourceElement)section.Sources.Get(elem_r.SourceName);
            }

            if (elem == null)
                strRet = "";
            else
                strRet = elem.FilePath;

            return strRet;
        }


        /// <summary>
        /// 设置应用程序配置节点,如果已经存在此节点,则会修改该节点的值,否则添加此节点
        /// </summary>
        /// <param name="key">节点名称</param>
        /// <param name="value">节点值</param>
        public void setAppSetting(string key, string value)
        {
            AppSettingsSection appSetting = (AppSettingsSection)config.GetSection("appSettings");
            if (appSetting.Settings[key] == null)//如果不存在此节点,则添加
            {
                appSetting.Settings.Add(key, value);
            }
            else//如果存在此节点,则修改
            {
                appSetting.Settings[key].Value = value;
            }
            this.save();
        }

        /// <summary>
        /// 定义删除当前或者其他应用程序配置文件中的appSettings节点
        /// </summary>
        /// <param name="key">keyName</param>
        /// <returns>删除成功返回true，删除失败返回false</returns>
        public bool removeAppSettingsSection(string key)
        {
            AppSettingsSection appSettings = (AppSettingsSection)config.GetSection("appSettings");
            if (appSettings.Settings[key] != null)
            {
                appSettings.Settings.Remove(key);
                this.save();
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 定义获取当前或其他应用程序appSettings的所有keyName方法
        /// </summary>
        /// <returns>返回appSettings的所有keyName</returns>
        public string[] activeAllAppSettingsSection()
        {
            AppSettingsSection appSettings = (AppSettingsSection)config.GetSection("appSettings");
            string[] appKeys = appSettings.Settings.AllKeys;
            return appKeys;
        }


        /// <summary>
        /// 获取当前或其他应用程序配置文件中connectionStrings节点的所有ConnectionString
        /// </summary>
        /// <returns>返回connectionStrings节点的所有ConnectionString</returns>
        public DataTable allConnectionStrings()
        {

            ConnectionStringsSection conSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            ConnectionStringSettingsCollection conCollection = conSection.ConnectionStrings;

            DataTable dt = new DataTable();
            dt.Columns.Add("name", typeof(string));
            dt.Columns.Add("val", typeof(string));
            dt.Columns.Add("xtype", typeof(string));
            dt.Columns.Add("xdefault", typeof(string));
            foreach (ConnectionStringSettings conSetting in conCollection)
            {
                if (conSetting.Name != "LocalSqlServer")
                {
                    if (conSetting.Name == getDefaultConnName())
                    {
                        dt.Rows.Add(new object[] { conSetting.Name, conSetting.ConnectionString, conSetting.ProviderName, "是" });
                    }
                    else
                    {
                        dt.Rows.Add(new object[] { conSetting.Name, conSetting.ConnectionString, conSetting.ProviderName, "否" });
                    }

                }

            }
            return dt;
        }

        /// <summary>
        /// 返回数据库连接名列表
        /// </summary>
        /// <returns>ListData<string></returns>
        public List<string> getConnNameList()
        {
            List<string> connNameList = new List<string>();
            try
            {
                ConnectionStringsSection conSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
                ConnectionStringSettingsCollection conCollection = conSection.ConnectionStrings;
                foreach (ConnectionStringSettings conSetting in conCollection)
                {
                    if (conSetting.Name.Equals("LocalSqlServer", StringComparison.OrdinalIgnoreCase))
                        continue;
                    connNameList.Add(conSetting.Name);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return connNameList;
        }


        /// <summary>
        /// 设置数据库连接字符串节点,如果不存在此节点,则会添加此节点及对应的值,存在则修改
        /// </summary>
        /// <param name="key">节点名称</param>
        /// <param name="connectionString">节点值</param>
        public void setConnectionString(string key, string connectionString, string prividerName)
        {
            ConnectionStringsSection connectionSetting;
            if (!exsitConnectionNode(key, out connectionSetting))
            {
                ConnectionStringSettings connectionStringSettings = new ConnectionStringSettings(key, connectionString, prividerName);
                connectionSetting.ConnectionStrings.Add(connectionStringSettings);
            }
            else//如果存在此节点,则修改
            {
                connectionSetting.ConnectionStrings[key].ConnectionString = connectionString;
                connectionSetting.ConnectionStrings[key].ProviderName = prividerName;
            }
            this.save();
        }


        /// <summary>
        /// 获取数据库连接串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string getConnStr(string key)
        {
            string strRet = "";
            ConnectionStringsSection connectionSetting;
            if (exsitConnectionNode(key, out connectionSetting))
            {
                strRet = connectionSetting.ConnectionStrings[key].ConnectionString;
            }

            return strRet;
        }


        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool deleteConnectionNode(string key)
        {
            bool bRet = false;
            ConnectionStringsSection connectionSetting;
            if (exsitConnectionNode(key, out connectionSetting))
            {
                connectionSetting.ConnectionStrings.Remove(key);
                this.save();
            }
            else
            {
                bRet = false;
            }

            return bRet;
        }


        /// <summary>
        /// 设置默认连接名称
        /// </summary>
        /// <param name="connKey"></param>
        /// <returns></returns>
        public bool setDefaultConnStr(string connKey)
        {
            bool bRet = false;
            try
            {
                Microsoft.Practices.EnterpriseLibrary.Data.Configuration.DatabaseSettings defultSec = (Microsoft.Practices.EnterpriseLibrary.Data.Configuration.DatabaseSettings)config.GetSection("dataConfiguration");
                defultSec.DefaultDatabase = connKey;
                save();
                bRet = true;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return bRet;
        }


        /// <summary>
        /// 获取默认连接名称
        /// </summary>
        /// <returns></returns>
        public string getDefaultConnName()
        {

            try
            {
                Microsoft.Practices.EnterpriseLibrary.Data.Configuration.DatabaseSettings defultSec = (Microsoft.Practices.EnterpriseLibrary.Data.Configuration.DatabaseSettings)config.GetSection("dataConfiguration");

                return defultSec.DefaultDatabase;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }


        }


        /// <summary>
        /// 配置文件中是否存在此节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool exsitConnectionNode(string key, out ConnectionStringsSection connectionSetting)
        {
            bool bRet = false;
            connectionSetting = (ConnectionStringsSection)config.GetSection("connectionStrings");
            if (connectionSetting.ConnectionStrings[key] == null)//如果不存在此节点,则添加
            {
                bRet = false;
            }
            else
            {
                bRet = true;
            }
            return bRet;
        }


        /// <summary>
        ///  保存所作的修改
        /// </summary> 
        private void save()
        {
            config.Save();
            config = null;
        }

        public void Dispose()
        {
            if (config != null)
            {
                config.Save();
            }
        }


        internal ConnDef getConnDef(string connName)
        {
            ConnDef ret = new ConnDef();
            ret.Name = connName;
            ConnectionStringsSection connectionSetting;
            if (exsitConnectionNode(connName, out connectionSetting))
            {
                ret.ConnStr = connectionSetting.ConnectionStrings[connName].ConnectionString;
                ret.Provider = connectionSetting.ConnectionStrings[connName].ProviderName;
                ret.Name = connName;
            }

            return ret;
        }
    }
}
