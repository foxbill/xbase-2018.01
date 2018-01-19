using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using EntLibContrib.Data.MySql;
using System.Data.Common;
using System.Data;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Web;

namespace xbase.data.admin
{
    public abstract class DatabaseAdmin
    {
        protected Database database;
        /// <summary>
        /// 返回数据库实例
        /// </summary>
        /// <param name="connName"></param>
        /// <returns></returns>
        public static DatabaseAdmin getInstance(string connName)
        {
            Database db = null;
            try
            {
                DatabaseProviderFactory dbFactory = new DatabaseProviderFactory(GetFileConfigurationSource("DBSource"));
                if (connName.Trim() == "")
                {
                    db = dbFactory.CreateDefault();
                }
                else
                {
                    db = dbFactory.Create(connName);
                }

                if (db is SqlDatabase)
                {
                    return new SqlDatabaseAdmin(db);
                }
                else if (db is OracleDatabase)
                {
                    return new OracleDatabaseAdmin(db);
                }
                else if (db is MySqlDatabase)
                {
                    return new MySqlDatabaseAdmin(db);
                }
                else if (db.DbProviderFactory.ToString() == "System.Data.OleDb.OleDbFactory")
                {
                    return new OleDbDatabaseAdmin(db);
                }
                else
                {
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                return null;
            }
           
           
        }

        /// <summary>
        /// 取webconfig中的配置
        /// </summary>
        /// <param name="SourceName"></param>
        /// <returns></returns>
        private static  FileConfigurationSource GetFileConfigurationSource(string SourceName)
        {
            //获取App.config
            //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");

            //获取资源节点集合
            ConfigurationSourceSection section =
            (ConfigurationSourceSection)config.GetSection(ConfigurationSourceSection.SectionName);

            //获取重定向配置文件资源配置节点
            FileConfigurationSourceElement elem =
            (FileConfigurationSourceElement)section.Sources.Get(SourceName);

            //获取重定向配置文件资源
            FileConfigurationSource fileSource = new FileConfigurationSource(HttpContext.Current.Server.MapPath(elem.FilePath));
            return fileSource;
        }


      /// <summary>
      /// 执行Sql语句
      /// </summary>
      /// <param name="strSql"></param>
      /// <param name="errInfo">执行描述</param>
      /// <returns></returns>
        public bool execNonQuery(string strSql, out string errInfo)
        {
            bool bRet=false;
        
            try
            {
                DbCommand cmd = null;
                cmd = Database.GetSqlStringCommand(strSql.ToUpper());
                Database.ExecuteNonQuery(cmd);
                errInfo = "成功";
                bRet=true;
            }
            catch(Exception ex)
            {
                errInfo = ex.Message;
            	bRet=false;
            }

            return bRet; 
        }


        /// <summary>
        /// 返回查询结果表
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="flag">0为stringcommand ;1为storeproccommand</param>
        /// <param name="paras">存储过程参数集合</param>
        /// <returns></returns>
        protected DataTable getQueryTable(string strSql, int flag, IDataParameter[] paras)
        {
            DbCommand cmd = null;
            try
            {
                switch (flag)
                {
                    case 0: cmd = Database.GetSqlStringCommand(strSql);
                        break;
                    case 1: cmd = Database.GetStoredProcCommand(strSql);
                        if (paras != null)
                        {
                            for (int i = 0; i < paras.Length; i++)
                            {
                                if (paras[i].Direction == ParameterDirection.Input)
                                {
                                    Database.AddInParameter(cmd, paras[i].ParameterName, paras[i].DbType, paras[i].Value);
                                }
                                else
                                {
                                    Database.AddInParameter(cmd, paras[i].ParameterName, paras[i].DbType, paras[i].Value);
                                }
                            }
                        }
                        break;
                }

                return Database.ExecuteDataSet(cmd).Tables[0];
            }
            catch (System.Exception ex)
            {
                return null;
            }
           
        }


        /// <summary>
        /// 执行事务
        /// </summary>
        protected void execSqlTransaction(string[] strSql)
        {
            int count = 0;
            DbCommand[] cmd = null;

            if (strSql.Length > 0)
            {
                count = strSql.Length;
                cmd = new DbCommand[strSql.Length];
                for (int i = 0; i < strSql.Length; i++)
                {
                    cmd[i] = Database.GetSqlStringCommand(strSql[i]);
                }
            }

            using (DbConnection conn = Database.CreateConnection())
            {
                conn.Open();
                DbTransaction trans = conn.BeginTransaction();

                try
                {
                    if (cmd != null && count > 0)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            Database.ExecuteNonQuery(cmd[i], trans);
                        }
                    }
                    //提交事务.
                    trans.Commit();
                }
                catch
                {
                    //回滚
                    trans.Rollback();
                }
                conn.Close();
            }
        }


        /// <summary>
        ///  执行存储过程 无返回
        /// </summary>
        /// <param name="spNmae"></param>
        /// <param name="[]paras"></param>
        /// <returns></returns>
        protected int execProcNonQuery(string spNmae, IDataParameter[] paras)
        {
            DbCommand cmd = Database.GetStoredProcCommand(spNmae);

            for (int i = 0; i < paras.Length; i++)
            {
                if (paras[i].Direction == ParameterDirection.Input)
                {
                    Database.AddInParameter(cmd, paras[i].ParameterName, paras[i].DbType, paras[i].Value);
                }
                else
                {
                    Database.AddInParameter(cmd, paras[i].ParameterName, paras[i].DbType, paras[i].Value);
                }
            }
            return Database.ExecuteNonQuery(cmd);
        }

       /// <summary>
       /// 返回首行首列
       /// </summary>
       /// <param name="strSql"></param>
       /// <returns>没数据或异常时为""</returns>
        protected string execScalar(string strSql)
        {
            string strRet = "";
            DbCommand cmd = Database.GetSqlStringCommand(strSql);
            try
            {                
                object val = Database.ExecuteScalar(cmd);
                if (val == System.DBNull.Value) 
                { 
                    val = null;
                    strRet = "";
                }
                else
                {
                    strRet = val.ToString();
                }
            }
            catch
            {
                strRet = "";
            }
            return strRet;
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="database"></param>
        protected DatabaseAdmin(Database database)
        {
            this.Database = database;            
        }
    
        /// <summary>
        /// 创建数据库表
        /// </summary>
        /// <param name="tableDef"></param>
        /// <returns></returns>
        public abstract bool createTable(TableDef tableDef);
        /// <summary>
        /// 返回数据库表集合
        /// </summary>
        /// <returns></returns>
        public abstract List<string> getTableNames();

        /// <summary>
        /// 返回当前表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public abstract TableDef getTableDef(string tableName);

        /// <summary>
        /// 修改数据库表
        /// </summary>
        /// <param name="tableDef"></param>
        /// <returns></returns>
        public abstract bool modifyTable(TableDef tableDef);

        /// <summary>
        ///  创建存储过程
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="procText"></param>
        /// <returns></returns>
        public abstract bool createProc(string procName,string procText);

        /// <summary>
        /// 返回存储过程集合
        /// </summary>
        /// <returns></returns>
        public abstract List<string> getProcNames();

        /// <summary>
        /// 返回存储过程内容
        /// </summary>
        /// <param name="procName"></param>
        /// <returns></returns>
        public abstract string getProcText(string procName);

        /// <summary>
        /// 修改存储过程
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="procText"></param>
        /// <returns></returns>
        public abstract bool modifyProc(string procName, string procText);

        /// <summary>
        /// 创建触发器
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="triggerText"></param>
        /// <returns></returns>
        public abstract bool createTrigger(string tableName, string triggerName, string triggerText);

        /// <summary>
        /// 返回触发器集合
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public abstract List<string> getTriggerNames(string tableName);

        /// <summary>
        /// 修改触发器
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="triggerText"></param>
        /// <returns></returns>
        public abstract bool modifyTrigger(string tableName, string triggerName, string triggerText);

        /// <summary>
        /// 返回当前触发器内容
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="triggerName"></param>
        /// <returns></returns>
        public abstract string getTriggerText(string tableName,string triggerName);

            
        /// <summary>
        /// 返回当前表结束集合
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public abstract DataTable getConstraintTable(string tableName);

        /// <summary>
        /// 返回约束内容
        /// </summary>
        /// <param name="contraintName"></param>
        /// <returns></returns>
        public abstract string getConstraintText(string contraintName, string tbName);


        /// <summary>
        /// 返回约束内容
        /// </summary>
        /// <param name="contraintName"></param>
        /// <returns></returns>
        public abstract Dictionary<string, string> getFieldTypeList();


        /// <summary>
        /// 返回数据库视图集合
        /// </summary>
        /// <returns></returns>
        public abstract List<string> getViewNames();


        /// <summary>
        /// 视图结构
        /// </summary>
        /// <returns></returns>
        public abstract DataTable getViewData(string viewName);


        public abstract bool deleteTable(string tbName,out string errMsg);
        public abstract bool deleteView(string viewName, out string errMsg);
        public abstract bool deleteProcedure(string spName, out string errMsg);
        public abstract bool deleteTrigger(string tgName, out string errMsg);
       
    }
}
