using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using xbase.local;

namespace xbase.data.db
{
    public class SqlDatabaseAdmin : DatabaseAdmin
    {
        private static Dictionary<string, DataTypeMap> DataTypeDict = new Dictionary<string, DataTypeMap>()
        {
                {"nvarchar", new DataTypeMap(){Caption="字符",CType=typeof(string),DbType=DbType.String}},
                {"varchar",new DataTypeMap(){Caption="英文字符",CType=typeof(string),DbType= DbType.AnsiString}},
                {"char",new DataTypeMap(){Caption="定长英文字符",CType=typeof(string),DbType=DbType.AnsiStringFixedLength}},
                {"nchar", new DataTypeMap(){Caption="定长字符",CType=typeof(string),DbType=DbType.StringFixedLength}},
                {"ntext", new DataTypeMap(){Caption="文本",CType=typeof(string),DbType=DbType.String}},
                {"text", new DataTypeMap(){Caption="英文文本",CType=typeof(string),DbType=DbType.AnsiString}},
                {"int",new DataTypeMap(){Caption="整数",CType=typeof(int),DbType= DbType.Int32}},
                {"bigint",new DataTypeMap(){Caption="大整数",CType=typeof(Int64),DbType=DbType.Int64}},
                {"smallint", new DataTypeMap(){Caption="小整数",CType=typeof(Int16),DbType=DbType.Int16}},
                {"tinyint", new DataTypeMap(){Caption="微整数",CType=typeof(Int16),DbType=DbType.SByte}},
                {"decimal",new DataTypeMap(){Caption="数字",CType=typeof(double),DbType= DbType.Decimal}},
                {"numeric",new DataTypeMap(){Caption="数字",CType=typeof(double),DbType= DbType.Decimal}},
                {"real",new DataTypeMap(){Caption="实数",CType=typeof(double),DbType= DbType.Double}},
                {"float",new DataTypeMap(){Caption="单精近似数",CType=typeof(float),DbType= DbType.Double}},
                {"bit",new DataTypeMap(){Caption="是否值",CType=typeof(bool),DbType=DbType.Boolean}},
                {"datetime",new DataTypeMap(){Caption="时间",CType=typeof(DateTime),DbType=DbType.DateTime}},
                {"smalldatetime", new DataTypeMap(){Caption="短时间",CType=typeof(DateTime),DbType=DbType.DateTime}},
                {"money", new DataTypeMap(){Caption="货币",CType=typeof(double),DbType=DbType.Currency}},
                {"image",new DataTypeMap(){Caption="二进制印象",CType=typeof(byte[]),DbType= DbType.Binary}},
                {"timestamp", new DataTypeMap(){Caption="时间戳",CType=typeof(string),DbType=DbType.String}},
                {"binary",new DataTypeMap(){Caption="二进制数据",CType=typeof(byte[]),DbType= DbType.Binary }},
         };



        public SqlDatabaseAdmin(Database db)
            : base(db)
        {
        }

        /// <summary>
        /// 创建数据库表
        /// </summary>
        /// <param name="tableDef"></param>
        /// <returns>成功返回true</returns>
        public override bool createTable(TableDef tableDef)
        {
            bool bRet = false;
            string tmpStr = "";
            string attachStr = "";//约束信息

            //字段说明
            StringBuilder noteStr = new StringBuilder();
            DbCommand cmd;

            try
            {
                if (tableDef != null)
                {
                    if (isExistObject(tableDef.Name, "IsUserTable"))
                    {
                        bRet = modifyTable(tableDef);
                    }
                    else
                    {
                        tableDef.MainKeys = new List<FieldDef>();
                        StringBuilder strSql = new StringBuilder();
                        StringBuilder strConstrains = new StringBuilder();//约束
                        strSql.Append("CREATE  TABLE " + tableDef.Name);
                        strSql.Append("(  ");
                        foreach (FieldDef field in tableDef.FieldDefs)
                        {
                            tmpStr = "";
                            attachStr = "";
                            if (string.IsNullOrEmpty(field.Name))
                                continue;

                            if (field.IsPriKey)
                                tableDef.MainKeys.Add(field);

                            //字段说明
                            if (!string.IsNullOrEmpty(field.Description))
                            {
                                noteStr.Append("EXECUTE sp_addextendedproperty N'MS_Description','" + field.Description + "',N'user',N'dbo',N'table',N'" + tableDef.Name + "',N'column',N'" + field.Name + "';");
                            }

                            if (!string.IsNullOrEmpty(field.Title))
                            {
                                noteStr.Append("EXECUTE sp_addextendedproperty N'Title','" + field.Title + "',N'user',N'dbo',N'table',N'" + tableDef.Name + "',N'column',N'" + field.Name + "';");
                            }

                            if (!string.IsNullOrEmpty(field.Alias))
                            {
                                noteStr.Append("EXECUTE sp_addextendedproperty N'Alias','" + field.Alias + "',N'user',N'dbo',N'table',N'" + tableDef.Name + "',N'column',N'" + field.Name + "';");
                            }

                            if (!string.IsNullOrEmpty(field.Constrains))
                            {
                                strConstrains.Append(field.Constrains + ";");
                            }

                            if (!string.IsNullOrEmpty(field.DefaultValue))
                            {
                                attachStr += "  default  " + field.DefaultValue;
                            }

                            if (field.NotNull || field.IsPriKey)
                            {
                                attachStr += "  not null ";
                            }

                            if (field.IsUnique)
                            {
                                attachStr += "  unique ";
                            }

                            if (field.IsIdentity)
                            {
                                attachStr += "  identity(1,1) ";
                            }

                            if (string.IsNullOrEmpty(field.Type))
                                throw new Exception(string.Format(Lang.FiledTypeMustFill, field.Name));

                            if (field.Type.ToLower() == "decimal")
                            {
                                tmpStr = field.Name + " " + field.Type.ToLower() + "(" + field.Length + "," + field.Procesion + ")";
                            }
                            else if (field.Length > 0 && field.Type.ToLower() != "int" && field.Type.ToLower() != "bigint" && field.Type.ToLower() != "tinyint" && field.Type.ToLower() != "money" && field.Type.ToLower() != "bit" && field.Type.ToLower() != "datetime" && field.Type.ToLower() != "text" && field.Type.ToLower() != "ntext" && field.Type.ToLower() != "timestamp" && field.Type.ToLower() != "smalldatetime" && field.Type.ToLower() != "image")
                            {
                                tmpStr = field.Name + " " + field.Type.ToLower() + "(" + field.Length + ")";
                            }
                            else
                            {
                                tmpStr = field.Name + " " + field.Type.ToLower();
                            }
                            tmpStr += attachStr + ",";
                            strSql.Append(tmpStr);

                        }

                        if (tableDef.FieldDefs.Count > 0)
                        {
                            strSql.Remove(strSql.Length - 1, 1);
                            strSql.Append(")");
                        }


                        if (!string.IsNullOrEmpty(strSql.ToString()))
                        {
                            //创建表
                            cmd = database.GetSqlStringCommand(strSql.ToString());
                            database.ExecuteNonQuery(cmd);
                        }


                        if (!string.IsNullOrEmpty(noteStr.ToString()))
                        {
                            //字段说明
                            cmd = database.GetSqlStringCommand(noteStr.ToString());
                            database.ExecuteNonQuery(cmd);
                        }

                        if (!string.IsNullOrEmpty(tableDef.Title))
                        {
                            string sql = "EXEC sys.sp_addextendedproperty @name=N'Title', @value=N'" + tableDef.Title + "' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + tableDef.Name + "'";
                            cmd = database.GetSqlStringCommand(sql);
                            database.ExecuteNonQuery(cmd);
                        }
                        if (!string.IsNullOrEmpty(tableDef.Description))
                        {
                            string sql = "EXEC sys.sp_addextendedproperty @name=N'Description', @value=N'" + tableDef.Description + "' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + tableDef.Name + "'";
                            cmd = database.GetSqlStringCommand(sql);
                            database.ExecuteNonQuery(cmd);
                        }



                        //表主键
                        string strKeyName = "";
                        string strKeySql = "";
                        foreach (FieldDef field in tableDef.MainKeys)
                        {
                            if (field.Name != "")
                            {
                                strKeyName += field.Name + ",";
                            }
                        }
                        strKeyName = strKeyName.TrimEnd(',');
                        if (strKeyName != "")
                        {
                            strKeySql = "ALTER TABLE " + tableDef.Name + "  WITH NOCHECK ADD CONSTRAINT PK_" + tableDef.Name + " PRIMARY KEY  NONCLUSTERED (" + strKeyName + ")";
                            cmd = database.GetSqlStringCommand(strKeySql);
                            database.ExecuteNonQuery(cmd);
                        }

                        if (!string.IsNullOrEmpty(strConstrains.ToString()))
                        {
                            cmd = database.GetSqlStringCommand(strConstrains.ToString());
                            database.ExecuteNonQuery(cmd);
                        }
                        bRet = true;
                    }


                }
                else
                {
                    bRet = false;
                }
            }
            catch (System.Exception ex)
            {
                bRet = false;
                throw ex;
            }
            return bRet;

        }

        /// <summary>
        /// 返回数据库表集合
        /// </summary>
        /// <returns>失败返回null</returns>
        public override List<string> getTableNames()
        {
            List<string> tbNames = null;
            try
            {
                string strSql = "SELECT Name FROM SysObjects Where XType='U' ORDER BY Name";

                DataTable table = getQueryTable(strSql, 0, null);
                if (table != null && table.Rows.Count > 0)
                {
                    tbNames = new List<string>();
                    foreach (DataRow row in table.Rows)
                    {
                        tbNames.Add(row["Name"].ToString());
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }


            return tbNames;
        }


        /// <summary>
        /// 返回指定表名的字段信息到一个DataTable
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns>返回一个DataTable对象，里面填充了指定表名的字段信息</returns>
        public override DataTable getFieldDefsToTable(string tableName)
        {
            if (!ExistsSp(MsSqlCmdText.GetFieldDefsSpName))
            {
                execNonQuery(MsSqlCmdText.GetFieldDefsSp);
            }
            DbCommand cmd = this.GetStoredProcCommand(MsSqlCmdText.GetFieldDefsSpName);
            addInParameter(cmd, "TableName", DbType.String, tableName);
            DataTable tb = executeDateSet(cmd).Tables[0];
            return tb;
        }

        private void clearDefaultValue(string tableName)
        {
            string sql = @"select name from sysobjects t where id  in (select cdefault from syscolumns where id = object_id('" + tableName + "') and cdefault<>0)";
            DbCommand cmd = getSqlStringCommand(sql);
            DataSet ds = executeDateSet(cmd);
            if (ds == null) return;
            if (ds.Tables.Count < 1) return;
            DataTable tb = ds.Tables[0];
            foreach (DataRow row in tb.Rows)
            {
                string name = row["name"].ToString();
                execNonQuery("alter table " + tableName + " drop constraint " + name + "");
            }
        }

        /// <summary>
        ///设置默认值
        /// </summary>
        /// <param name="tbName"></param>
        /// <param name="filedName"></param>
        /// <param name="defautValue"></param>
        private void setDefaultContraint(string tbName, string filedName, string defautValue)
        {
            string strSql = "select [name] from sysobjects t where id = (select cdefault from syscolumns where id = object_id('" + tbName + "') and name='" + filedName + "')";

            try
            {
                string ctName = execScalar(strSql);
                string strDrop = "alter table " + tbName + " drop constraint " + ctName + "";
                execNonQuery(strDrop);
            }
            catch
            {

            }

            try
            {
                string strSet = "alter table " + tbName + " add default(" + defautValue + ") for " + filedName;
                execNonQuery(strSet);
            }
            catch (System.Exception ex)
            {

            }

        }

        private void alterFields(string tableName, List<FieldDef> fieldDefs)
        {

        }

        /// <summary>
        /// 通过FieldDef返回sql语句字段定义
        /// </summary>
        /// <param name="fieldDef"></param>
        /// <returns></returns>
        private string getSqlFieldDefine(FieldDef fieldDef, bool isAdd = true)
        {
            string[] noLenFields = new string[] { "int", "bigint", "tinyint", "smallint", "money", "bit", "float", "datetime", "text", "ntext", "timestamp", "smalldatetime", "image" };

            StringBuilder sb = new StringBuilder();
            sb.Append(" ");
            sb.Append(fieldDef.Name);
            sb.Append(" ");
            sb.Append(fieldDef.Type);

            if (!noLenFields.Contains(fieldDef.Type.ToLower().Trim()))
            {
                sb.Append("(");
                sb.Append(fieldDef.Length);
                if (fieldDef.Type == "decimal")
                {
                    sb.Append(",");
                    sb.Append(fieldDef.Procesion);
                }
                sb.Append(")");
            }

            if (fieldDef.NotNull || fieldDef.IsPriKey)
                sb.Append("  not null ");

            if (isAdd)
            {
                if (!string.IsNullOrEmpty(fieldDef.DefaultValue))
                {
                    sb.Append("  default  ");
                    sb.Append(fieldDef.DefaultValue);
                }

                if (fieldDef.IsUnique)
                    sb.Append("  unique ");

                if (fieldDef.IsIdentity)
                    sb.Append("  identity(1,1) ");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 修改数据库表
        /// </summary>
        /// <param name="tableDef"></param>
        /// <returns></returns>
        public override bool modifyTable(TableDef tableDef)
        {

            if (string.IsNullOrEmpty(tableDef.OldName))
                return createTable(tableDef);

            if (!tableDef.OldName.Equals(tableDef.Name, StringComparison.OrdinalIgnoreCase))
                renameTable(tableDef.OldName, tableDef.Name);


            //修改字段名
            foreach (FieldDef fldDef in tableDef.FieldDefs)
            {
                if (!string.IsNullOrEmpty(fldDef.OldName)
                        && !string.IsNullOrEmpty(fldDef.Name)
                        && !fldDef.OldName.Equals(fldDef.Name, StringComparison.OrdinalIgnoreCase)
                   )
                    renameFieldName(tableDef.Name, fldDef.OldName, fldDef.Name);
            }

            StringBuilder noteStr = new StringBuilder();//字段说明

            string strSql = "";

            TableDef oldTabDef = getTableDef(tableDef.Name);

            clearDefaultValue(tableDef.Name);
            tableDef.MainKeys = new List<FieldDef>();
            for (int i = 0; i < tableDef.FieldDefs.Count; i++)
            {

                if (string.IsNullOrEmpty(tableDef.FieldDefs[i].Name))
                    continue;



                FieldDef fieldDef = tableDef.FieldDefs[i];

                if (fieldDef.IsPriKey)
                    tableDef.MainKeys.Add(fieldDef);



                if (string.IsNullOrEmpty(fieldDef.OldName))
                    strSql = "alter  table " + tableDef.Name + "  add " + getSqlFieldDefine(fieldDef) + " ;";
                else
                    strSql = "alter  table " + tableDef.Name + "  alter column " + getSqlFieldDefine(fieldDef, false) + " ;";

                execNonQuery(strSql);

                if (!String.IsNullOrEmpty(tableDef.FieldDefs[i].DefaultValue))
                {
                    setDefaultContraint(tableDef.Name, tableDef.FieldDefs[i].Name, tableDef.FieldDefs[i].DefaultValue);
                }

                noteStr.Append("EXECUTE sp_dropextendedproperty  N'MS_Description',N'user',N'dbo',N'table',N'" + tableDef.Name + "',N'column',N'" + tableDef.FieldDefs[i].Name + "';");
                noteStr.Append("EXECUTE sp_addextendedproperty  N'MS_Description','" + tableDef.FieldDefs[i].Description + "',N'user',N'dbo',N'table',N'" + tableDef.Name + "',N'column',N'" + tableDef.FieldDefs[i].Name + "';");

                noteStr.Append("EXECUTE sp_dropextendedproperty  N'Title',N'user',N'dbo',N'table',N'" + tableDef.Name + "',N'column',N'" + tableDef.FieldDefs[i].Name + "';");
                noteStr.Append("EXECUTE sp_addextendedproperty  N'Title','" + tableDef.FieldDefs[i].Title + "',N'user',N'dbo',N'table',N'" + tableDef.Name + "',N'column',N'" + tableDef.FieldDefs[i].Name + "';");

                noteStr.Append("EXECUTE sp_dropextendedproperty  N'Alias',N'user',N'dbo',N'table',N'" + tableDef.Name + "',N'column',N'" + tableDef.FieldDefs[i].Name + "';");
                noteStr.Append("EXECUTE sp_addextendedproperty  N'Alias','" + tableDef.FieldDefs[i].Alias + "',N'user',N'dbo',N'table',N'" + tableDef.Name + "',N'column',N'" + tableDef.FieldDefs[i].Name + "';");

            }


            noteStr.Append("EXEC sys.sp_dropextendedproperty @name=N'Title', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + tableDef.Name + "'");
            noteStr.Append("EXEC sys.sp_addextendedproperty @name=N'Title', @value=N'" + tableDef.Title + "' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + tableDef.Name + "'");

            noteStr.Append("EXEC sys.sp_dropextendedproperty @name=N'Description', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + tableDef.Name + "'");
            noteStr.Append("EXEC sys.sp_addextendedproperty @name=N'Description', @value=N'" + tableDef.Description + "' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + tableDef.Name + "'");

            if (!string.IsNullOrEmpty(noteStr.ToString()))
            {
                try
                {
                    execNonQuery(noteStr.ToString());
                }
                catch { }
            }

            //if (!string.IsNullOrEmpty(strConstrains.ToString()))
            //{
            //    cmd = database.getSqlStringCommand(strConstrains.ToString());
            //    database.ExecuteNonQuery(cmd);
            //}

            //表主键
            string strKeyName = "";
            string strKeySql = "";
            foreach (FieldDef field in tableDef.MainKeys)
            {
                if (field.Name != "")
                {
                    strKeyName += field.Name + ",";
                }

            }
            strKeyName = strKeyName.TrimEnd(',');
            if (strKeyName != "")
            {
                try
                {
                    strKeySql = "ALTER TABLE " + tableDef.Name + " DROP CONSTRAINT PK_" + tableDef.Name + "";
                    // cmd = database.getSqlStringCommand(strKeySql.ToUpper());
                    database.ExecuteNonQuery(CommandType.Text, strKeySql);
                }
                catch
                {
                    string defKey = "select name from sysindexes where  id=object_id( '" + tableDef.Name + "')  and status= 2066";
                    string keyName = execScalar(defKey);
                    if (!string.IsNullOrEmpty(keyName))
                    {
                        strKeySql = "ALTER TABLE " + tableDef.Name + " DROP CONSTRAINT " + execScalar(defKey) + "";
                        database.ExecuteNonQuery(CommandType.Text, strKeySql);
                    }

                }

                strKeySql = "ALTER TABLE " + tableDef.Name + "  WITH NOCHECK ADD CONSTRAINT PK_" + tableDef.Name + " PRIMARY KEY  NONCLUSTERED ( " + strKeyName + ")";
                database.ExecuteNonQuery(CommandType.Text, strKeySql);
            }
            return true;
        }

        /// <summary>
        /// 创建存储过程
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="procText"></param>
        /// <returns></returns>
        public override bool createProc(string procName, string procText)
        {
            bool bRet = false;
            try
            {
                if (isExistObject(procName, "IsProcedure"))
                {
                    bRet = modifyProc(procName, procText);
                }
                else
                {
                    if (!string.IsNullOrEmpty(procName) && !string.IsNullOrEmpty(procText))
                    {
                        procText = procText.ToUpper().Replace("ALTER", "CREATE");
                        DbCommand cmd = database.GetSqlStringCommand(procText);
                        database.ExecuteNonQuery(cmd);
                        bRet = true;
                    }
                }

            }
            catch (System.Exception ex)
            {
                bRet = false;
                throw ex;
            }

            return bRet;

        }

        /// <summary>
        /// 返回存储过程集合
        /// </summary>
        /// <returns></returns>
        public override List<string> getProcNames()
        {
            string strSql = "";
            List<string> listProc = new List<string>();

            DataTable tb;
            try
            {
                strSql = "SELECT name,type_desc  FROM sys.objects  where [type] ='P' ";

                tb = getQueryTable(strSql, 0, null);
                if (tb != null)
                {
                    foreach (DataRow row in tb.Rows)
                    {
                        listProc.Add(row["Name"].ToString());
                    }

                }


            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return listProc;
        }

        /// <summary>
        /// 返回存储过程内容
        /// </summary>
        /// <param name="procName"></param>
        /// <returns></returns>
        public override string getProcText(string procName)
        {
            string strRet = "";
            strRet = getTPContent("P", procName);
            if (strRet == "")
            {
                strRet = "Create Proc " + procName + " () AS ";
            }
            else
            {
                strRet = strRet.ToUpper().Replace("CREATE", "ALTER");
            }
            return strRet;
        }

        /// <summary>
        /// 修改存储过程
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="procText"></param>
        /// <returns></returns>
        public override bool modifyProc(string procName, string procText)
        {
            bool bRet = false;
            try
            {
                if (isExistObject(procName, "IsProcedure"))
                {
                    if (!string.IsNullOrEmpty(procName) && !string.IsNullOrEmpty(procText))
                    {
                        procText = procText.ToUpper().Replace("CREATE", "ALTER");
                        DbCommand cmd = database.GetSqlStringCommand(procText);
                        database.ExecuteNonQuery(cmd);
                        bRet = true;
                    }
                }
                else
                {
                    bRet = createProc(procName, procText);
                }


            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return bRet;
        }

        /// <summary>
        /// 创建触发器
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="triggerText"></param>
        /// <returns></returns>
        public override bool createTrigger(string tableName, string triggerName, string triggerText)
        {
            bool bRet = false;
            try
            {
                if (isExistObject(triggerName, "IsTrigger"))
                {
                    bRet = modifyTrigger(tableName, triggerName, triggerText);
                }
                else
                {
                    if (!string.IsNullOrEmpty(tableName) && !string.IsNullOrEmpty(triggerText))
                    {
                        triggerText = triggerText.ToUpper().Replace("ALTER", "CREATE");
                        DbCommand cmd = database.GetSqlStringCommand(triggerText);
                        database.ExecuteNonQuery(cmd);
                        bRet = true;
                    }
                }


            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return bRet;
        }

        /// <summary>
        /// 返回触发器集合
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public override List<string> getTriggerNames(string tableName)
        {
            string strSql = "";
            List<string> listTrig = new List<string>();

            DataTable tb;
            try
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    strSql = "SELECT name,type_desc FROM sys.objects  where [type] ='TR'";
                }
                else
                {
                    strSql = "SELECT name,type_desc FROM sys.objects  where [type] ='TR' and parent_object_id=object_id('" + tableName + "') ";
                }

                tb = getQueryTable(strSql, 0, null);
                if (tb != null)
                {
                    foreach (DataRow row in tb.Rows)
                    {
                        listTrig.Add(row["Name"].ToString());
                    }

                }

            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return listTrig;
        }

        /// <summary>
        /// 修改触发器
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="triggerText"></param>
        /// <returns></returns>
        public override bool modifyTrigger(string tableName, string triggerName, string triggerText)
        {
            bool bRet = false;
            try
            {
                if (isExistObject(triggerName, "IsTrigger"))
                {

                    if (!string.IsNullOrEmpty(tableName) && !string.IsNullOrEmpty(triggerText))
                    {
                        triggerText = triggerText.ToUpper().Replace("CREATE", "ALTER");
                        DbCommand cmd = database.GetSqlStringCommand(triggerText);
                        database.ExecuteNonQuery(cmd);
                        bRet = true;
                    }
                }
                else
                {
                    bRet = createTrigger(tableName, triggerName, triggerText);
                }


            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return bRet;
        }

        /// <summary>
        /// 返回当前触发器内容
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="triggerName"></param>
        /// <returns></returns>
        public override string getTriggerText(string tableName, string triggerName)
        {
            string strRet = "";
            strRet = getTPContent("TR", triggerName);
            if (strRet == "")
            {
                strRet = "Create TRIGGER " + triggerName + " ON  " + tableName + "  FOR INSERT,UPDATE,DELETE  AS ";
            }
            else
            {
                strRet = strRet.ToUpper().Replace("CREATE", "ALTER");
            }
            return strRet;
        }


        /// <summary>
        /// 根据名称得到存储过程、触发器内容
        /// </summary>
        /// <param name="xType">类型</param>
        /// <param name="tpName">名称</param>
        /// <returns></returns>
        private string getTPContent(string xType, string tpName)
        {
            string strRet = "";
            string strSql = "SELECT a.[name],b.[text]  from sysobjects a, syscomments b  where a.id = b.id and  a.xtype = '" + xType + "' and name='" + tpName + "'";
            DataTable tb = getQueryTable(strSql, 0, null);
            for (int i = 0; i < tb.Rows.Count; i++)
            {
                strRet += tb.Rows[i]["text"].ToString() + "\r\n";
            }
            return strRet;

        }


        /// <summary>
        /// 判断表是否存在
        /// </summary>
        /// <param name="tbName"></param>
        /// <returns></returns>
        private bool isExistObject(string tbName, string para)
        {
            bool bRet = false;
            StringBuilder strBSql = new StringBuilder();
            strBSql.Append("if exists (select * from sysobjects where id = object_id(N'" + tbName + "') and OBJECTPROPERTY(id,N'" + para + "') = 1) ");
            strBSql.Append(" select '1' as col ");
            strBSql.Append(" else ");
            strBSql.Append(" select '0' as col");

            string strExsit = execScalar(strBSql.ToString());
            if (strExsit == "1")
            {
                bRet = true;
            }
            else
            {
                bRet = false;
            }

            return bRet;
        }


        /// <summary>
        /// 返回当前表结束集合
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public override DataTable getConstraintTable(string tableName)
        {
            DataTable tb;
            if (!string.IsNullOrEmpty(tableName))
            {
                string strSql = "SELECT CONSTRAINT_NAME as Name,CONSTRAINT_TYPE as Type FROM information_schema.TABLE_CONSTRAINTS WHERE TABLE_NAME='" + tableName + "' ";
                tb = getQueryTable(strSql, 0, null);
            }
            else
            {
                tb = null;
            }
            return tb;
        }

        /// <summary>
        /// 返回约束内容
        /// </summary>
        /// <param name="contraintName"></param>
        /// <returns></returns>
        public override string getConstraintText(string contraintName, string tbName)
        {
            string strRet = "";
            strRet = execScalar("select text as content from sysconstraints a, syscomments b where a.id = object_id('" + tbName + "') and a.constid=b.id and object_name(constid)='" + contraintName + "'");
            return strRet;
        }

        /// <summary>
        /// 返回当前数据库字段数据类型
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, string> getFieldTypeList()
        {
            Dictionary<string, string> dicFieldTypes = new Dictionary<string, string>();
            foreach (string typeName in DataTypeDict.Keys)
            {
                DataTypeMap dtm = DataTypeDict[typeName];
                dicFieldTypes.Add(typeName, DataTypeDict[typeName].Caption);
            }
            return dicFieldTypes;
        }

        /// <summary>
        /// 返回数据库视图集合
        /// </summary>
        /// <returns></returns>
        public override List<string> getViewNames()
        {
            List<string> viewNames = null;
            try
            {
                string strSql = "SELECT Name FROM SysObjects Where XType='V' ORDER BY Name";

                DataTable table = getQueryTable(strSql, 0, null);
                if (table != null && table.Rows.Count > 0)
                {
                    viewNames = new List<string>();
                    foreach (DataRow row in table.Rows)
                    {
                        viewNames.Add(row["Name"].ToString());
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }


            return viewNames;
        }

        /// <summary>
        /// 视图结构
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public override DataTable getViewData(string viewName)
        {
            string strSql = "select top 10 * from " + viewName + "";
            DataTable tb = getQueryTable(strSql, 0, null);
            return tb;

        }

        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="tbName"></param>
        /// <returns></returns>
        public override bool deleteTable(string tbName)
        {
            string strSql = "drop table " + tbName + "";
            return execNonQuery(strSql.ToUpper());
        }

        /// <summary>
        /// 删除视图
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public override bool deleteView(string viewName)
        {
            string strSql = "drop view " + viewName + "";
            return execNonQuery(strSql.ToUpper());
        }

        /// <summary>
        /// 删除过程
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public override bool deleteProcedure(string spName)
        {
            string strSql = "drop PROCEDURE " + spName + "";
            return execNonQuery(strSql.ToUpper());
        }

        /// <summary>
        /// 删除触发器
        /// </summary>
        /// <param name="tgName"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public override bool deleteTrigger(string tgName)
        {
            string strSql = @"IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'" + tgName
                            + "')) DROP TRIGGER " + tgName + "";
            return execNonQuery(strSql.ToUpper());
        }


        /// <summary>
        /// 返回数据表的主键集合
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public override List<string> GetPrimaryKeys(string tableName)
        {

            //string strSql = "SELECT NAME FROM SYSCOLUMNS WHERE ID=OBJECT_ID('" + tableName + "') AND COLID IN (SELECT KEYNO FROM SYSINDEXKEYS WHERE ID=OBJECT_ID('" + tableName + "'))";
            string strSql = "Execute   sp_pkeys " + tableName;
            List<string> listKeys = new List<string>();
            DataTable tb;
            try
            {

                tb = getQueryTable(strSql, 0, null);
                if (tb != null)
                {
                    foreach (DataRow row in tb.Rows)
                    {
                        string key = row["COLUMN_NAME"].ToString();
                        key = key.Trim('[', ']', '"');

                        listKeys.Add(key);
                        //listKeys.Add(row["NAME"].ToString());
                    }
                }

            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return listKeys;
        }

        /// <summary>
        /// 判断字段是否为自增字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public override bool isIdentityField(string tableName, string fieldName)
        {
            bool bRet = false;
            string strSql = "SELECT COLUMNPROPERTY(OBJECT_ID('" + tableName + "'),'" + fieldName + "','IsIdentity')";
            if (execScalar(strSql.ToUpper()) == "1")
            {
                bRet = true;
            }
            else
            {
                bRet = false;
            }

            return bRet;
        }

        /// <summary>
        /// 判断字段是否为GUID字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public override bool isRowGuidField(string tableName, string fieldName)
        {
            bool bRet = false;
            string strSql = "SELECT COLUMNPROPERTY(OBJECT_ID('" + tableName + "'),'" + fieldName + "','IsRowGuidCol')";
            if (execScalar(strSql.ToUpper()) == "1")
            {
                bRet = true;
            }
            else
            {
                bRet = false;
            }
            return bRet; ;
        }


        public override string getViewScript(string viewName)
        {
            string ret = getTPContent("V", viewName);
            if (string.IsNullOrEmpty(ret))
                ret = "Create View " + viewName + " as ";
            else
                ret = ret.ToUpper().Replace("CREATE", "ALTER");

            return ret;
        }

        public override void modifyViewScript(string viewName, string script)
        {
            if (!string.IsNullOrEmpty(viewName) && isExistObject(viewName, "IsView"))
            {
                if (!string.IsNullOrEmpty(script))
                {
                    script = script.ToUpper().Replace("CREATE", "ALTER");
                }
            }
            else
            {
                script = script.ToUpper().Replace("ALTER", "CREATE");
            }
            DbCommand cmd = database.GetSqlStringCommand(script);
            database.ExecuteNonQuery(cmd);
        }

        public override bool ExistsSp(string spName)
        {
            int ret = (int)database.ExecuteScalar(CommandType.Text, @"select  case when exists(select * from dbo.sysobjects 
                           where id=object_id('" + spName + @"') and objectproperty(id,'isprocedure')=1)
                           then -1  else 0 end");
            return ret != 0;
        }

        /// <summary>
        /// 获取Sql Server字段类型的DbType
        /// </summary>
        /// <param name="typeString">Sql Server 字段类型</param>
        /// <returns></returns>
        public override DbType getDbType(string typeString)
        {
            typeString = typeString.ToLower();
            return DataTypeDict[typeString].DbType;
        }

        /// <summary>
        /// 获取表标题
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public override string getTableTitle(string tableName)
        {
            if (!ExistsSp(MsSqlCmdText.GetTableExtendPropSpName))
                execNonQuery(MsSqlCmdText.GetTableExtendProp);

            DbCommand cmd = GetStoredProcCommand(MsSqlCmdText.GetTableExtendPropSpName);
            addInParameter(cmd, "@TableName", DbType.String, tableName);
            addInParameter(cmd, "@PropName", DbType.String, "Title");

            object obj = database.ExecuteScalar(cmd);
            if (obj == null)
                return "";
            return obj.ToString();
        }

        /// <summary>
        /// 获取表说明
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public override string getTableDescription(string tableName)
        {
            if (!ExistsSp(MsSqlCmdText.GetTableExtendPropSpName))
                execNonQuery(MsSqlCmdText.GetTableExtendProp);
            DbCommand cmd = GetStoredProcCommand(MsSqlCmdText.GetTableExtendPropSpName);
            addInParameter(cmd, "@TableName", DbType.String, tableName);
            addInParameter(cmd, "@PropName", DbType.String, "Description");

            object obj = database.ExecuteScalar(cmd);
            if (obj == null)
                return "";
            return obj.ToString();
        }

        public override void renameTable(string tableName, string newTableName)
        {
            execNonQuery("EXEC sp_rename '" + tableName + "', '" + newTableName + "'");
        }

        public override void renameFieldName(string tableName, string fieldName, string newName)
        {
            execNonQuery("EXEC sp_rename '" + tableName + ".[" + fieldName + "]', '" + newName + "', 'COLUMN'");
        }

        public override string typeOfDotNetType(Type type)
        {
            foreach (string typeName in DataTypeDict.Keys)
            {
                DataTypeMap dtm = DataTypeDict[typeName];
                if (type.Equals(dtm.CType))
                    return typeName;
            }
            throw new Exception(string.Format(Lang.DatabaseNotSuportsDataType, type.ToString()));
        }

        public override string typeOfDbType(DbType dbType)
        {
            foreach (string typeName in DataTypeDict.Keys)
            {
                DataTypeMap dtm = DataTypeDict[typeName];
                if (dbType.Equals(dtm.DbType))
                    return typeName;
            }
            throw new Exception(string.Format(Lang.DatabaseNotSuportsDataType, dbType.ToString()));
        }
    }
}

