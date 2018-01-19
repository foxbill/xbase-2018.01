using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;


namespace xbase.data.admin
{
    
    public static class ConnectionAdmin
    {
        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static string configFilePath = @"~/config";
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
                bRet = false;
            }

            return bRet;
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
        /// 获取数据库连接字符串
        /// </summary>
        /// <param name="connName"></param>
        /// <returns></returns>
        public static string getConnStr(string connName)
        {
            ConfigurationOperator cfgOper = new ConfigurationOperator(configFilePath);                      
            return cfgOper.getConnStr(connName);
        }


        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <param name="connName"></param>
        /// <returns></returns>
        public static DataTable getConnNameList()
        {
            ConfigurationOperator cfgOper = new ConfigurationOperator(configFilePath);
            return cfgOper.allConnectionStrings();
        }

        /// <summary>
        /// 设置默认数据库连接
        /// </summary>
        /// <returns></returns>
        public static bool setDefaultDbStr(string keyName)
        {
            ConfigurationOperator cfgOper = new ConfigurationOperator(configFilePath);
            return cfgOper.setDefaultConnStr(keyName);
        }

        /// <summary>
        /// 取默认连接串
        /// </summary>
        /// <returns></returns>
        public static string getDefaultDbStr()
        {
            ConfigurationOperator cfgOper = new ConfigurationOperator(configFilePath);
            return cfgOper.getDefaultConnStr();
        }

    }
}
