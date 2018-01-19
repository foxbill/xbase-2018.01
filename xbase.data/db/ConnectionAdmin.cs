using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;


namespace xbase.data.db
{

    public static class ConnectionAdmin
    {
        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static string configFilePath;

        /// <summary>
        /// 构造函数
        /// </summary>
        static ConnectionAdmin()
        {
            ConfigurationOperator cfgOper = new ConfigurationOperator(configFilePath);
            string filePath = cfgOper.getRedirectConfigPath();
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    configFilePath = "~";
                }
                else
                {
                    configFilePath = @"~/" + filePath.Substring(0, filePath.LastIndexOf(@"/"));
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// 配置文件路径
        /// </summary>
        public static string ConfigFilePath
        {
            get { return configFilePath; }
            set { configFilePath = value; }
        }

        /// <summary>
        /// 添加数据库连接字符串
        /// </summary>
        /// <param name="connName"></param>
        /// <param name="connStr"></param>
        /// <param name="xType"></param>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static bool addConnection(string connName, string connStr, string prividerName)
        {
            bool bRet = false;
            try
            {
                ConfigurationOperator cfgOper = new ConfigurationOperator(configFilePath);
                cfgOper.setConnectionString(connName, connStr, prividerName);
                cfgOper.Dispose();
                bRet = true;

            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return bRet;
        }

        public static ConnDef getConnDef(string connName)
        {
            ConfigurationOperator cfgOper = new ConfigurationOperator(configFilePath);
            return cfgOper.getConnDef(connName);

        }

        /// <summary>
        /// 删除数据库连接字符串
        /// </summary>
        /// <param name="connName"></param>
        /// <returns></returns>
        public static bool deleteConnection(string connName)
        {

            ConfigurationOperator cfgOper = new ConfigurationOperator(configFilePath);
            return cfgOper.deleteConnectionNode(connName);
        }


        /// <summary>
        /// 添加数据库连接字符串
        /// </summary>
        /// <param name="connName"></param>
        /// <param name="connStr"></param>
        /// <param name="xType">数据库类型</param>
        /// <returns></returns>
        public static bool modifyConnection(string connName, string connStr, string prividerName)
        {
            bool bRet = addConnection(connName, connStr, prividerName);
            return bRet;
        }

        /// <summary>
        /// 获取数据库连接字符串内容
        /// </summary>
        /// <param name="connName"></param>
        /// <returns></returns>
        public static string getConnStr(string connName)
        {
            ConfigurationOperator cfgOper = new ConfigurationOperator(configFilePath);
            return cfgOper.getConnStr(connName);
        }


        /// <summary>
        /// 获取所有数据库连接内容
        /// </summary>
        /// <param name="connName"></param>
        /// <returns>DataTable 列包括：name、val、xtype、xdefault</returns>
        public static DataTable getAllConnInfoTable()
        {
            ConfigurationOperator cfgOper = new ConfigurationOperator(configFilePath);
            return cfgOper.allConnectionStrings();
        }


        /// <summary>
        /// 返回数据库连接名列表
        /// </summary>
        /// <returns>ListData<string></returns>
        public static List<string> getConnNameList()
        {
            ConfigurationOperator cfgOper = new ConfigurationOperator(configFilePath);
            return cfgOper.getConnNameList();
        }




        /// <summary>
        /// 设置默认数据库连接
        /// </summary>
        /// <returns></returns>
        public static bool setDefaultConn(string connName)
        {
            ConfigurationOperator cfgOper = new ConfigurationOperator(configFilePath);
            return cfgOper.setDefaultConnStr(connName);
        }

        /// <summary>
        /// 取默认连接串
        /// </summary>
        /// <returns></returns>
        public static string getDefaultConnName()
        {
            ConfigurationOperator cfgOper = new ConfigurationOperator(configFilePath);
            return cfgOper.getDefaultConnName();
        }

    }
}
