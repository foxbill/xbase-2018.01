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
using xbase.Exceptions;
using xbase.local;

namespace xbase.data.db
{
    public abstract class DatabaseAdmin
    {

        protected Database database;

        /// <summary>
        ///构造函数 基类必须继承 
        /// </summary>
        /// <param name="database"></param>
        protected DatabaseAdmin(Database database)
        {
            this.database = database;
        }

        private DatabaseAdmin()
        {
        }

        /// <summary>
        /// 返回Dababase对象
        /// </summary>
        public Database Database
        {
            get
            {
                return database;
            }
        }



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
                FileConfigurationSource fileSource = GetFileConfigurationSource();

                if (fileSource != null)
                {
                    DatabaseProviderFactory dbFactory = new DatabaseProviderFactory(fileSource);
                    if (string.IsNullOrEmpty(connName))
                    {
                        db = dbFactory.CreateDefault();
                    }
                    else
                    {
                        db = dbFactory.Create(connName);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(connName))
                    {
                        db = DatabaseFactory.CreateDatabase();
                    }
                    else
                    {
                        db = DatabaseFactory.CreateDatabase(connName);
                    }
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
                throw ex;
            }


        }




        /// <summary>
        /// 返回默认数据库实例
        /// </summary>
        /// <returns></returns>
        public static DatabaseAdmin getInstance()
        {
            Database db = null;
            try
            {
                FileConfigurationSource fileSource = GetFileConfigurationSource();

                if (fileSource != null)
                {
                    DatabaseProviderFactory dbFactory = new DatabaseProviderFactory(fileSource);
                    db = dbFactory.CreateDefault();
                }
                else
                {
                    db = DatabaseFactory.CreateDatabase();
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
                throw ex;
            }
        }


        /// <summary>
        /// 取webconfig中的配置
        /// </summary>
        /// <param name="SourceName"></param>
        /// <returns></returns>
        private static FileConfigurationSource GetFileConfigurationSource()
        {

            FileConfigurationSource fileSource;
            string configPath = new ConfigurationOperator().getRedirectConfigPath();
            //获取重定向配置文件资源
            if (string.IsNullOrEmpty(configPath))
            {
                fileSource = null;
            }
            else
            {
                fileSource = new FileConfigurationSource(HttpContext.Current.Server.MapPath(configPath));
            }

            return fileSource;
        }


        /// <summary>
        /// 执行Sql语句
        /// </summary>
        /// <param name="strSql"></param>     
        /// <returns></returns>
        public bool execNonQuery(string strSql)
        {
            bool bRet = false;

            try
            {
                DbCommand cmd = null;
                cmd = database.GetSqlStringCommand(strSql);
                database.ExecuteNonQuery(cmd);
                bRet = true;
            }
            catch (Exception ex)
            {
                throw ex;
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
                    case 0: cmd = database.GetSqlStringCommand(strSql);
                        break;
                    case 1: cmd = database.GetStoredProcCommand(strSql);
                        if (paras != null)
                        {
                            for (int i = 0; i < paras.Length; i++)
                            {
                                if (paras[i].Direction == ParameterDirection.Input)
                                {
                                    database.AddInParameter(cmd, paras[i].ParameterName, paras[i].DbType, paras[i].Value);
                                }
                                else
                                {
                                    database.AddInParameter(cmd, paras[i].ParameterName, paras[i].DbType, paras[i].Value);
                                }
                            }
                        }
                        break;
                }

                return database.ExecuteDataSet(cmd).Tables[0];
            }
            catch (System.Exception ex)
            {
                throw ex;
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
                    cmd[i] = database.GetSqlStringCommand(strSql[i]);
                }
            }

            using (DbConnection conn = database.CreateConnection())
            {
                conn.Open();
                DbTransaction trans = conn.BeginTransaction();

                try
                {
                    if (cmd != null && count > 0)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            database.ExecuteNonQuery(cmd[i], trans);
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
                finally
                {
                    conn.Close();
                }

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
            DbCommand cmd = database.GetStoredProcCommand(spNmae);

            for (int i = 0; i < paras.Length; i++)
            {
                if (paras[i].Direction == ParameterDirection.Input)
                {
                    database.AddInParameter(cmd, paras[i].ParameterName, paras[i].DbType, paras[i].Value);
                }
                else
                {
                    database.AddInParameter(cmd, paras[i].ParameterName, paras[i].DbType, paras[i].Value);
                }
            }
            return database.ExecuteNonQuery(cmd);
        }


        /// <summary>
        /// 返回首行首列,返回""表示没有值
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns>没数据或异常时为""</returns>
        protected string execScalar(string strSql)
        {
            string strRet = "";
            DbCommand cmd = database.GetSqlStringCommand(strSql);
            try
            {
                object val = database.ExecuteScalar(cmd);
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
            catch (System.Exception ex)
            {
                strRet = "";

            }
            return strRet;
        }

        /// <summary>
        /// 执行存储过程并返回DataSet对象
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="sqlParamValues"></param>
        /// <returns></returns>
        public DataSet executeDateSet(string spName, params object[] sqlParamValues)
        {
            return database.ExecuteDataSet(spName, sqlParamValues);
        }

        public DataSet executeDateSet(string spName, List<IDataParameter> dataParams)
        {
            return database.ExecuteDataSet(spName, dataParams);
        }

        internal DataSet executeDateSet(DbCommand cmd)
        {
            return database.ExecuteDataSet(cmd);
        }


        /// <summary>
        /// 获取存储过程参数表
        /// </summary>
        /// <param name="spName"></param>
        /// <returns></returns>
        public List<IDataParameter> getSpParams(string spName)
        {
            string sql = @" select * from syscolumns where ID in    
                    (SELECT id FROM sysobjects as a  
                     WHERE OBJECTPROPERTY(id, N'IsProcedure') = 1    
                        and id = object_id('" + spName + "'))";
            DbCommand cmd = database.GetSqlStringCommand(sql);
            database.ExecuteDataSet(cmd);
            DataSet ds = executeDateSet(sql);
            return null;
        }

        /// <summary>
        /// 删除字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        public void deleteField(string tableName, string fieldName)
        {
            string sqlText = "alter Table " + tableName + " drop column " + fieldName;
            execNonQuery(sqlText);
        }

        /// <summary>
        /// 删除约束
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="ctrName"></param>
        public void deleteConstraint(string tableName, string ctrName)
        {
            string sqlText = "ALTER TABLE " + tableName + " DROP CONSTRAINT " + ctrName;
            execNonQuery(sqlText);
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
        public TableDef getTableDef(string tableName)
        {
            TableDef ret = new TableDef();
            ret.Name = tableName;
            ret.OldName = tableName;
            ret.Description = getTableDescription(tableName);
            ret.Title = getTableTitle(tableName);
            DataTable tb = getFieldDefsToTable(tableName);
            if (tb.Rows == null || tb.Rows.Count == 0)
                return null;

            if (tb == null || tb.Rows.Count < 1)
            {
                ret.FieldDefs = new List<FieldDef>() { new FieldDef() };
                return ret;
            }

            foreach (DataRow row in tb.Rows)
            {

                FieldDef fldDef = new FieldDef();

                fldDef.Name = row["colName"].ToString();
                fldDef.Type = row["colType"].ToString();
                fldDef.OldName = fldDef.Name;
                if (!string.IsNullOrEmpty(row["colLength"].ToString()))
                {
                    fldDef.Length = int.Parse(row["colLength"].ToString());
                }
                else
                {
                    fldDef.Length = 0;
                }

                if (!string.IsNullOrEmpty(row["growMark"].ToString()))
                {
                    fldDef.IsIdentity = bool.Parse(row["growMark"].ToString());
                }
                else
                {
                    fldDef.IsIdentity = false;
                }

                if (!string.IsNullOrEmpty(row["pointCount"].ToString()))
                {
                    fldDef.Procesion = int.Parse(row["pointCount"].ToString());
                }



                if (!string.IsNullOrEmpty(row["colVal"].ToString()))
                {
                    fldDef.DefaultValue = row["colVal"].ToString();
                }


                if (!string.IsNullOrEmpty(row["colNote"].ToString()))
                {
                    fldDef.Description = row["colNote"].ToString();
                }

                if (!string.IsNullOrEmpty(row["title"].ToString()))
                {
                    fldDef.Title = row["title"].ToString();
                }

                if (!string.IsNullOrEmpty(row["alias"].ToString()))
                {
                    fldDef.Alias = row["alias"].ToString();
                }


                if (!string.IsNullOrEmpty(row["colIsUnique"].ToString()))
                {
                    fldDef.IsUnique = bool.Parse(row["colIsUnique"].ToString());
                }
                else
                {
                    fldDef.IsUnique = false;
                }

                if (!string.IsNullOrEmpty(row["colIsNull"].ToString()))
                {
                    fldDef.NotNull = !bool.Parse(row["colIsNull"].ToString());
                }
                else
                {
                    fldDef.NotNull = false;
                }

                if (!string.IsNullOrEmpty(row["pkName"].ToString()))
                    fldDef.IsPriKey = bool.Parse(row["pkName"].ToString());

                if (fldDef.IsPriKey)
                    ret.MainKeys.Add(fldDef);

                ret.FieldDefs.Add(fldDef);


                //if (!string.IsNullOrEmpty(row["pkName"].ToString()) && row["pkName"].ToString() == "True")
                //{
                //    ret.MainKeys.Add(fldDef);
                //    fldDef.IsPriKey = true;
                //}
                //else
                //{
                //    fldDef.IsPriKey = false;
                //}
            }
            return ret;
        }


        /// <summary>
        /// 返回指定表名的字段信息到一个DataTable
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns>返回一个DataTable对象，里面填充了指定表名的字段信息</returns>
        public abstract DataTable getFieldDefsToTable(string tableName);

        /// <summary>
        /// 获取表标题
        /// </summary>
        /// <param name="tableName">表明</param>
        /// <returns></returns>
        public abstract string getTableTitle(string tableName);

        /// <summary>
        /// 获取表说明
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public abstract string getTableDescription(string tableName);

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
        public abstract bool createProc(string procName, string procText);

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
        public abstract string getTriggerText(string tableName, string triggerName);

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

        /// <summary>
        /// 获取视图脚本
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <returns></returns>
        public abstract string getViewScript(string viewName);

        /// <summary>
        /// 修改视图脚本
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <param name="script">视图脚本</param>
        public abstract void modifyViewScript(string viewName, string script);


        public abstract bool deleteTable(string tbName);


        public abstract bool deleteView(string viewName);
        public abstract bool deleteProcedure(string spName);
        public abstract bool deleteTrigger(string tgName);
        public abstract List<string> GetPrimaryKeys(string tableName);
        public abstract bool isIdentityField(string tableName, string fieldName);
        public abstract bool isRowGuidField(string tableName, string fieldName);


        public bool containsTableName(string name)
        {
            List<string> names = getTableNames();
            foreach (string tbName in names)
            {
                if (tbName.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public abstract bool ExistsSp(string spName);


        public int execNonQuery(DbCommand cmd)
        {
            return database.ExecuteNonQuery(cmd);
        }

        public DbCommand getSqlStringCommand(string text)
        {
            return database.GetSqlStringCommand(text);
        }

        public void addInParameter(DbCommand cmd, string paramName, DbType dbType, object value)
        {
            database.AddInParameter(cmd, paramName, dbType, value);
        }

        public abstract DbType getDbType(string typeString);


        public DbCommand GetStoredProcCommand(string spName)
        {
            return database.GetStoredProcCommand(spName);
        }

        internal void AddOutParameter(DbCommand cmd, string paramName, DbType dbType, int size)
        {
            database.AddOutParameter(cmd, paramName, dbType, size);
        }

        internal DbCommand GetStoredProcCommandWithSourceColumns(string spName, params string[] fields)
        {
            return database.GetStoredProcCommandWithSourceColumns(spName, fields);
        }


        internal DbCommand getSqlCommand(string sql, CommandType commandType)
        {
            switch (commandType)
            {
                case CommandType.StoredProcedure:
                    return database.GetStoredProcCommand(sql);
                case CommandType.Text:
                    return database.GetSqlStringCommand(sql);
                default:
                    throw new XException(Lang.NotSupportsTableDirectCommand);
            }
        }

        public abstract void renameTable(string tableName, string newTableName);
        public abstract void renameFieldName(string tableName, string fieldName, string newName);


        public abstract string typeOfDotNetType(Type type);

        public abstract string typeOfDbType(DbType dbType);


        public void compareUpdate(string _tableName, ListDataRow row)
        {
            DbCommand cmd = database.GetSqlStringCommand("update " + _tableName + " set ");
            StringBuilder sbWhere = new StringBuilder();
            StringBuilder sbValues = new StringBuilder();
            foreach (string field in row.Keys)
            {
                if (field.StartsWith(XSqlBuilder.OLD_VERSION_PIX))
                {
                    string rName = field.Remove(0, XSqlBuilder.OLD_VERSION_PIX.Length);
                    sbWhere.Append(" and ");
                    sbWhere.Append(rName);
                    sbWhere.Append("=");
                    sbWhere.Append("@" + field);
                    database.AddInParameter(cmd, "@" + field, DbType.String, row[field]);
                }
                else
                {
                    sbValues.Append(",");
                    sbValues.Append(field);
                    sbValues.Append("=");
                    sbValues.Append("@" + field);
                    database.AddInParameter(cmd, "@" + field, DbType.String, row[field]);
                }
            }

            if (sbWhere.Length == 0)
                throw new Exception(Lang.RowNoOldVer);

            sbWhere.Remove(0, 5);
            sbValues.Remove(0, 1);
            cmd.CommandText += sbValues.ToString() + " where " + sbWhere.ToString();
            int c = database.ExecuteNonQuery(cmd);
            if (c < 1)
                insertTableRow(_tableName, row);
        }

        public void insertTableRow(string tableName, ListDataRow row)
        {
            DbCommand cmd = database.GetSqlStringCommand("insert into  " + tableName);
            StringBuilder sbFields = new StringBuilder();
            StringBuilder sbValues = new StringBuilder();
            foreach (string field in row.Keys)
            {
                if (!field.StartsWith(XSqlBuilder.OLD_VERSION_PIX))
                {
                    sbFields.Append(",");
                    sbFields.Append("[");
                    sbFields.Append(field);
                    sbFields.Append("]");

                    sbValues.Append(",");
                    sbValues.Append("@");
                    sbValues.Append(field);
                    database.AddInParameter(cmd, "@" + field, DbType.String, row[field]);
                }
            }
            sbFields.Remove(0, 1);
            sbValues.Remove(0, 1);
            cmd.CommandText += "(" + sbFields.ToString() + ") values (" + sbValues.ToString() + ")";
            database.ExecuteNonQuery(cmd);
        }

        public DataTable executeTable(string sql)
        {
            DbCommand cmd = getSqlStringCommand(sql);
            DataSet ds = executeDateSet(cmd);
            return ds.Tables[0];
        }
        public DataTable executeTable(DbCommand cmd)
        {
            DataSet ds = executeDateSet(cmd);
            return ds.Tables[0];
        }

        public DbType getDbType(Type type)
        {
            Dictionary<Type, DbType> map = new Dictionary<Type, DbType>()
            {
                { typeof(string),DbType.String},
                {typeof(DateTime),DbType.DateTime},
//                {typeof(int),DbType.Int64},
                {typeof(float),DbType.Double},
                {typeof(byte[]),DbType.Binary},
                {typeof(bool),DbType.Boolean},
                {typeof(Int16),DbType.Int16},
                {typeof(Int32),DbType.Int32},
                {typeof(Int64),DbType.Int64},
                {typeof(double),DbType.Double},
                {typeof(byte),DbType.Byte},
                {typeof(sbyte),DbType.SByte},
              //  {typeof(Single),DbType.Single},
                {typeof(char),DbType.AnsiString},
                {typeof(decimal),DbType.Decimal}
            };
            return map[type];
        }

        public object executeScalar(DbCommand cmd)
        {
            return Database.ExecuteScalar(cmd);
        }

        public IDataReader executeReader(DbCommand cmd)
        {
            return database.ExecuteReader(cmd);
        }
    }
}
