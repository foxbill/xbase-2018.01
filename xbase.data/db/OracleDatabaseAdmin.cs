using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
namespace xbase.data.db
{
    public class OracleDatabaseAdmin : DatabaseAdmin
    {

        public OracleDatabaseAdmin(Database db)
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
                    if (isExistObject(tableDef.Name, "Table"))
                    {
                        bRet = modifyTable(tableDef);
                    }
                    else
                    {
                        StringBuilder strSql = new StringBuilder();
                        StringBuilder strConstrains = new StringBuilder();//约束
                        strSql.Append("CREATE  TABLE " + tableDef.Name);
                        strSql.Append("(  ");
                        foreach (FieldDef field in tableDef.FieldDefs)
                        {
                            tmpStr = "";
                            attachStr = "";
                            if (field.Name != "")
                            {
                                //字段说明
                                if (!string.IsNullOrEmpty(field.Alias))
                                {
                                    noteStr.Append("COMMENT ON COLUMN " + tableDef.Name + "." + field.Name + " IS '" + field.Alias + "';");
                                }

                                if (!string.IsNullOrEmpty(field.Constrains))
                                {
                                    strConstrains.Append(field.Constrains + ";");

                                }

                                if (!string.IsNullOrEmpty(field.DefaultValue))
                                {
                                    attachStr += "  default '" + field.DefaultValue + "'";
                                }
                                if (!field.NotNull)
                                {
                                    attachStr += "  not null ";
                                }
                                else if (field.IsPriKey)
                                {
                                    attachStr += "  not null ";
                                }

                                if (field.IsGuid)
                                {
                                    attachStr += "  unique ";
                                }

                                if (field.Type.ToLower().ToLower() == "number")
                                {
                                    tmpStr = field.Name + " " + field.Type.ToLower().ToLower() + "(" + field.Length + "," + field.Procesion + ")";
                                }
                                else if (field.Length > 0 && field.Type.ToLower().ToLower() != "int" && field.Type.ToLower().ToLower() != "blob" && field.Type.ToLower().ToLower() != "clob" && field.Type.ToLower().ToLower() != "nclob" && field.Type.ToLower().ToLower() != "date" && field.Type.ToLower().ToLower() != "timestamp")
                                {
                                    tmpStr = field.Name + " " + field.Type.ToLower().ToLower() + "(" + field.Length + ")";
                                }
                                else
                                {
                                    tmpStr = field.Name + " " + field.Type.ToLower().ToLower();
                                }
                                tmpStr += attachStr + ",";
                                strSql.Append(tmpStr);
                            }
                        }

                        if (tableDef.FieldDefs.Count > 0)
                        {
                            strSql.Remove(strSql.Length - 1, 1);
                            strSql.Append(")");
                        }


                        if (!string.IsNullOrEmpty(strSql.ToString()))
                        {
                            //创建表
                            cmd = database.GetSqlStringCommand(strSql.ToString().ToUpper());
                            database.ExecuteNonQuery(cmd);
                        }



                        if (!string.IsNullOrEmpty(noteStr.ToString()))
                        {
                            //字段说明
                            string[] strArr = noteStr.ToString().Split(';');
                            foreach (string s in strArr)
                            {
                                cmd = database.GetSqlStringCommand(s.ToUpper());
                                database.ExecuteNonQuery(cmd);
                            }
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
                            strKeySql = "ALTER TABLE " + tableDef.Name + " ADD CONSTRAINT PK_" + tableDef.Name + " PRIMARY KEY ( " + strKeyName + ")";
                            cmd = database.GetSqlStringCommand(strKeySql.ToUpper());
                            database.ExecuteNonQuery(cmd);
                        }

                        if (!string.IsNullOrEmpty(strConstrains.ToString()))
                        {
                            string[] strArr = strConstrains.ToString().Split(';');
                            foreach (string s in strArr)
                            {
                                cmd = database.GetSqlStringCommand(s.ToUpper());
                                database.ExecuteNonQuery(cmd);
                            }

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
                string strSql = "SELECT Table_Name FROM user_tables  ORDER BY Table_Name";

                DataTable table = getQueryTable(strSql, 0, null);
                if (table != null && table.Rows.Count > 0)
                {
                    tbNames = new List<string>();
                    foreach (DataRow row in table.Rows)
                    {
                        tbNames.Add(row["Table_Name"].ToString());
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
            string strSql = "SELECT USER_TAB_COLS.COLUMN_NAME as colName ,USER_TAB_COLS.DATA_TYPE as colType, USER_TAB_COLS.DATA_LENGTH as colLength,DATA_PRECISION as intLength,DATA_SCALE as pointCount, USER_TAB_COLS.NULLABLE as colIsNull,USER_TAB_COLS.DATA_DEFAULT as colVal, " +
            "case  when (SELECT col.column_name FROM user_constraints con, user_cons_columns col where con.constraint_name = col.constraint_name and con.constraint_type='P' and col.table_name = upper('" + tableName.ToUpper() + "') and col.column_name = USER_TAB_COLS.COLUMN_NAME) = USER_TAB_COLS.COLUMN_NAME THEN 'True'" +
                  "ELSE 'False'" +
            " END " +
             " as pkName, " +
             "case when (SELECT col.column_name FROM user_constraints con, user_cons_columns col where con.constraint_name = col.constraint_name and con.constraint_type='U' and col.table_name = upper('" + tableName.ToUpper() + "') and col.column_name = USER_TAB_COLS.COLUMN_NAME) = USER_TAB_COLS.COLUMN_NAME THEN 'True'" +
            "ELSE 'False'" +
             " END " +
              " as colIsUnique, " +
            "case when (select count(*) from user_constraints con, user_cons_columns col, (select t2.table_name,t2.column_name,t1.r_constraint_name from user_constraints t1,user_cons_columns t2 where t1.r_constraint_name=t2.constraint_name and t1.table_name=upper('" + tableName.ToUpper() + "')) r where con.constraint_name=col.constraint_name and con.r_constraint_name=r.r_constraint_name and con.table_name=upper('" + tableName.ToUpper() + "') and col.column_name = USER_TAB_COLS.COLUMN_NAME) > 0 then 'True'" +
            " else 'False'" +
           " END " +
            " as fkName, " +
            "user_col_comments.comments as colNote FROM USER_TAB_COLS inner join user_col_comments on user_col_comments.TABLE_NAME = USER_TAB_COLS.TABLE_NAME and user_col_comments.COLUMN_NAME=USER_TAB_COLS.COLUMN_NAME and USER_TAB_COLS.TABLE_NAME = upper('" + tableName.ToUpper() + "') ORDER BY USER_TAB_COLS.COLUMN_ID";

            DataTable tb = getQueryTable(strSql, 0, null);

            return tb;
        }

        /// <summary>
        /// 修改数据库表
        /// </summary>
        /// <param name="tableDef"></param>
        /// <returns></returns>
        public override bool modifyTable(TableDef tableDef)
        {

            bool bRet = false;
            TableDef existTbDef;
            StringBuilder strConstrains = new StringBuilder();//约束

            string attachStr = "";//约束信息
            //字段说明
            string noteStr;


            string tmpStr = "";


            DbCommand cmd;
            string strSql = "";
            try
            {
                if (tableDef != null)
                {
                    if (isExistObject(tableDef.Name, "Table"))
                    {
                        existTbDef = getTableDef(tableDef.Name);
                        for (int i = 0; i < tableDef.FieldDefs.Count; i++)
                        {
                            if (tableDef.FieldDefs[i].Name != "")
                            {

                                for (int j = 0; j < existTbDef.FieldDefs.Count; j++)
                                {
                                    //字段说明
                                    if (!string.IsNullOrEmpty(tableDef.FieldDefs[i].Alias))
                                    {
                                        noteStr = "COMMENT ON COLUMN " + tableDef.Name + "." + tableDef.FieldDefs[i].Name + " IS '" + tableDef.FieldDefs[i].Alias + "'";
                                        cmd = database.GetSqlStringCommand(noteStr.ToUpper());
                                        database.ExecuteNonQuery(cmd);
                                    }

                                    //修改列
                                    if (tableDef.FieldDefs[i].Name == existTbDef.FieldDefs[j].Name)
                                    {
                                        attachStr = "";
                                        if (tableDef.FieldDefs[i].Type.ToLower() == "number")
                                        {
                                            tmpStr = tableDef.FieldDefs[i].Name + " " + tableDef.FieldDefs[i].Type.ToLower() + "(" + tableDef.FieldDefs[i].Length + "," + tableDef.FieldDefs[i].Procesion + ")";
                                        }
                                        else if (tableDef.FieldDefs[i].Length > 0 && tableDef.FieldDefs[i].Type.ToLower() != "int" && tableDef.FieldDefs[i].Type.ToLower() != "blob" && tableDef.FieldDefs[i].Type.ToLower() != "clob" && tableDef.FieldDefs[i].Type.ToLower() != "nclob" && tableDef.FieldDefs[i].Type.ToLower() != "date" && tableDef.FieldDefs[i].Type.ToLower() != "timestamp")
                                        {
                                            tmpStr = tableDef.FieldDefs[i].Name + " " + tableDef.FieldDefs[i].Type.ToLower() + "(" + tableDef.FieldDefs[i].Length + ")";
                                        }
                                        else
                                        {
                                            tmpStr = tableDef.FieldDefs[i].Name + " " + tableDef.FieldDefs[i].Type.ToLower();
                                        }

                                        if (!string.IsNullOrEmpty(tableDef.FieldDefs[i].DefaultValue))
                                        {
                                            attachStr += "  default " + tableDef.FieldDefs[i].DefaultValue;
                                        }

                                        if (tableDef.FieldDefs[i].NotNull != existTbDef.FieldDefs[j].NotNull)
                                        {
                                            if (!tableDef.FieldDefs[i].NotNull)
                                            {

                                                attachStr += "  not null ";
                                            }
                                            else
                                            {
                                                attachStr += " null ";
                                            }
                                        }

                                        if (tableDef.FieldDefs[i].IsPriKey)
                                        {
                                            attachStr += "  not null ";
                                        }

                                        tmpStr += attachStr;
                                        try
                                        {
                                            strSql = "alter  table " + tableDef.Name + "  MODIFY   " + tmpStr + " ";
                                            cmd = database.GetSqlStringCommand(strSql.ToUpper());
                                            database.ExecuteNonQuery(cmd);
                                            if (!string.IsNullOrEmpty(tableDef.FieldDefs[i].Constrains))
                                            {
                                                strConstrains.Append(tableDef.FieldDefs[i].Constrains.ToUpper() + ";");
                                            }
                                        }
                                        catch (System.Exception ex)
                                        {
                                            throw ex;
                                        }


                                        existTbDef.FieldDefs.Remove(existTbDef.FieldDefs[j]);
                                        tableDef.FieldDefs.Remove(tableDef.FieldDefs[i]);

                                        j--;
                                        i--;

                                        break;
                                    }

                                }

                            }
                        }


                        //添加列
                        foreach (FieldDef fd in tableDef.FieldDefs)
                        {
                            tmpStr = "";
                            attachStr = "";


                            if (!string.IsNullOrEmpty(fd.Constrains))
                            {
                                strConstrains.Append(fd.Constrains + " ;");

                            }
                            if (!string.IsNullOrEmpty(fd.DefaultValue))
                            {
                                attachStr += "  default '" + fd.DefaultValue + "'";
                            }

                            if (fd.Type.ToLower().ToLower() == "number")
                            {
                                tmpStr = fd.Name + " " + fd.Type.ToLower().ToLower() + "(" + fd.Length + "," + fd.Procesion + ")";
                            }
                            else if (fd.Length > 0)
                            {
                                tmpStr = fd.Name + " " + fd.Type.ToLower().ToLower() + "(" + fd.Length + ")";
                            }
                            else
                            {
                                tmpStr = fd.Name + " " + fd.Type.ToLower().ToLower();
                            }

                            if (!fd.NotNull)
                            {
                                attachStr += "  not null ";
                            }
                            else if (fd.IsPriKey)
                            {
                                attachStr += "  not null ";
                            }

                            if (fd.IsGuid)
                            {
                                attachStr += "  unique ";
                            }


                            if (fd.Type.ToLower().ToLower() == "number")
                            {
                                tmpStr = fd.Name + " " + fd.Type.ToLower().ToLower() + "(" + fd.Length + "," + fd.Procesion + ")";
                            }
                            else if (fd.Length > 0 && fd.Type.ToLower().ToLower() != "int" && fd.Type.ToLower().ToLower() != "blob" && fd.Type.ToLower().ToLower() != "clob" && fd.Type.ToLower().ToLower() != "nclob" && fd.Type.ToLower().ToLower() != "date" && fd.Type.ToLower().ToLower() != "timestamp")
                            {
                                tmpStr = fd.Name + " " + fd.Type.ToLower().ToLower() + "(" + fd.Length + ")";
                            }
                            else
                            {
                                tmpStr = fd.Name + " " + fd.Type.ToLower().ToLower();
                            }
                            tmpStr += attachStr;

                            strSql = "alter table " + tableDef.Name + " add  " + tmpStr + "";
                            cmd = database.GetSqlStringCommand(strSql.ToUpper());
                            database.ExecuteNonQuery(cmd);


                            //字段说明
                            if (!string.IsNullOrEmpty(fd.Alias))
                            {
                                noteStr = "COMMENT ON COLUMN " + tableDef.Name + "." + fd.Name + " IS '" + fd.Alias + "'";
                                cmd = database.GetSqlStringCommand(noteStr.ToUpper());
                                database.ExecuteNonQuery(cmd);
                            }

                        }

                        //删除列
                        foreach (FieldDef fd1 in existTbDef.FieldDefs)
                        {
                            strSql = "alter  table " + tableDef.Name + " DROP COLUMN " + fd1.Name + "";
                            cmd = database.GetSqlStringCommand(strSql.ToUpper());
                            database.ExecuteNonQuery(cmd);
                        }



                        if (!string.IsNullOrEmpty(strConstrains.ToString()))
                        {
                            string[] strArr = strConstrains.ToString().Split(';');
                            foreach (string s in strArr)
                            {
                                cmd = database.GetSqlStringCommand(s.ToUpper());
                                database.ExecuteNonQuery(cmd);
                            }
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
                            try
                            {
                                strKeySql = "ALTER TABLE " + tableDef.Name + " DROP CONSTRAINT PK_" + tableDef.Name + "";
                                cmd = database.GetSqlStringCommand(strKeySql.ToUpper());
                                database.ExecuteNonQuery(cmd);
                            }
                            catch
                            {

                            }

                            strKeySql = "ALTER TABLE " + tableDef.Name + "  ADD CONSTRAINT PK_" + tableDef.Name + " PRIMARY KEY ( " + strKeyName + ")";
                            cmd = database.GetSqlStringCommand(strKeySql.ToUpper());
                            database.ExecuteNonQuery(cmd);
                        }

                        bRet = true;
                    }
                    else
                    {
                        bRet = createTable(tableDef);
                    }
                }
                else
                {
                    bRet = false;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return bRet;
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
                if (isExistObject(procName, "Procedure"))
                {
                    bRet = modifyProc(procName, procText);
                }
                else
                {
                    if (!string.IsNullOrEmpty(procName) && !string.IsNullOrEmpty(procText))
                    {

                        DbCommand cmd = database.GetSqlStringCommand(procText.ToUpper());
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
                strSql = "SELECT Distinct NAME  FROM user_source  where TYPE ='PROCEDURE' ";
                tb = getQueryTable(strSql.ToUpper(), 0, null);
                if (tb != null)
                {
                    foreach (DataRow row in tb.Rows)
                    {
                        listProc.Add(row["NAME"].ToString());
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
            strRet = getTPContent("PROCEDURE", procName.ToUpper());
            if (strRet == "")
            {
                strRet = "CREATE OR REPLACE PROCEDURE " + procName.ToUpper();
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
                if (isExistObject(procName, "Procedure"))
                {
                    if (!string.IsNullOrEmpty(procName) && !string.IsNullOrEmpty(procText))
                    {
                        DbCommand cmd = database.GetSqlStringCommand(procText.ToUpper());
                        database.ExecuteNonQuery(cmd);
                        bRet = true;
                    }
                }
                else
                {
                    bRet = createProc(procName.ToUpper(), procText.ToUpper());
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
                if (isExistObject(triggerName, "Trigger"))
                {
                    bRet = modifyTrigger(tableName.ToUpper(), triggerName.ToUpper(), triggerText.ToUpper());
                }
                else
                {
                    if (!string.IsNullOrEmpty(tableName) && !string.IsNullOrEmpty(triggerText))
                    {
                        DbCommand cmd = database.GetSqlStringCommand(triggerText.ToUpper());
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
                    strSql = "SELECT Distinct NAME  FROM user_source  where TYPE ='TRIGGER' ";
                }
                else
                {
                    strSql = "select TRIGGER_NAME from user_triggers where table_name = upper('" + tableName + "')";
                }

                tb = getQueryTable(strSql.ToUpper(), 0, null);
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
                if (isExistObject(triggerName, "Trigger"))
                {

                    if (!string.IsNullOrEmpty(tableName) && !string.IsNullOrEmpty(triggerText))
                    {
                        DbCommand cmd = database.GetSqlStringCommand(triggerText.ToUpper());
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
            strRet = getTPContent("TRIGGER", triggerName);
            if (strRet == "")
            {
                strRet = "CREATE OR REPLACE TRIGGER " + triggerName.ToUpper();
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
            StringBuilder strRet = new StringBuilder();
            string strSql = "SELECT name,text  from user_source where type = '" + xType.ToUpper() + "' and name='" + tpName.ToUpper() + "'";
            try
            {
                DataTable tb = getQueryTable(strSql.ToUpper(), 0, null);
                if (tb != null && tb.Rows.Count > 0)
                {
                    strRet.Append(" CREATE OR REPLACE ");
                    foreach (DataRow row in tb.Rows)
                    {
                        strRet.Append(row["TEXT"].ToString());
                    }

                }

            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return strRet.ToString();

        }


        /// <summary>
        /// 判断表是否存在
        /// </summary>
        /// <param name="tbName"></param>
        /// <returns></returns>
        private bool isExistObject(string tbName, string para)
        {
            bool bRet = false;
            string strSql = "";
            if (para == "Procedure")
            {
                strSql = "select count(distinct(NAME)) from user_source where Type='" + para.ToUpper() + "' and Name='" + tbName.ToUpper() + "'";
            }
            else if (para == "Trigger")
            {
                strSql = "select count(distinct(NAME)) from user_source where Type='" + para.ToUpper() + "' and Name='" + tbName.ToUpper() + "'";
            }
            else if (para == "Table")
            {
                strSql = "select count(*) from user_tables where table_name = upper('" + tbName.ToUpper() + "')";
            }

            string strExsit = execScalar(strSql.ToString().ToUpper());
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
                string strSql = "SELECT constraint_name as Name, constraint_Type as Type FROM all_constraints WHERE table_name = upper('" + tableName + "') ";
                tb = getQueryTable(strSql.ToUpper(), 0, null);
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
            strRet = execScalar("select SEARCH_CONDITION as content from user_constraints where table_name=upper('" + tbName + "') and  constraint_name = '" + contraintName + "'");
            return strRet;
        }

        /// <summary>
        /// 返回当前数据库字段数据类型
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, string> getFieldTypeList()
        {
            Dictionary<string, string> dicFieldTypes = new Dictionary<string, string>();
            dicFieldTypes.Add("INT", "INT");
            dicFieldTypes.Add("NUMBER", "NUMBER");
            dicFieldTypes.Add("CHAR", "CHAR");
            dicFieldTypes.Add("NCHAR", "NCHAR");
            dicFieldTypes.Add("VARCHAR2", "VARCHAR2");
            dicFieldTypes.Add("NVARCHAR2", "NVARCHAR2");
            dicFieldTypes.Add("DATE", "DATE");
            dicFieldTypes.Add("TIMESTAMP", "TIMESTAMP");
            dicFieldTypes.Add("BLOB", "BLOB");
            dicFieldTypes.Add("CLOB", "CLOB");
            dicFieldTypes.Add("NCLOB", "NCLOB");
            return dicFieldTypes;
        }

        public override List<string> getViewNames()
        {
            List<string> viewNames = null;
            string strSql = "SELECT view_name FROM user_views  ORDER BY view_name";

            DataTable table = getQueryTable(strSql, 0, null);
            if (table != null && table.Rows.Count > 0)
            {
                viewNames = new List<string>();
                foreach (DataRow row in table.Rows)
                {
                    viewNames.Add(row["view_name"].ToString());
                }
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
            string strSql = "select * from " + viewName + "  where rownum<10";
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
            string strSql = "drop Trigger " + tgName + "";
            return execNonQuery(strSql.ToUpper());
        }


        /// <summary>
        /// 返回数据表的主键集合
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public override List<string> GetPrimaryKeys(string tableName)
        {
            string strSql = "SELECT COLUMN_NAME FROM USER_CONSTRAINTS C,USER_CONS_COLUMNS COL WHERE C.CONSTRAINT_NAME=COL.CONSTRAINT_NAME AND C.CONSTRAINT_TYPE='P' AND C.TABLE_NAME='" + tableName + "'";
            List<string> listKeys = new List<string>();
            DataTable tb;
            try
            {
                tb = getQueryTable(strSql.ToUpper(), 0, null);
                if (tb != null)
                {
                    foreach (DataRow row in tb.Rows)
                    {

                        //                        listKeys.Add(row["COLUMN_NAME"].ToString());
                        string key = row["COLUMN_NAME"].ToString();
                        key = key.Trim('[', ']', '"');

                        listKeys.Add(key);

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

            return bRet; ;
        }


        public override string getViewScript(string viewName)
        {
            throw new NotImplementedException();
        }

        public override void modifyViewScript(string viewName, string script)
        {
            throw new NotImplementedException();
        }

        public override bool ExistsSp(string spName)
        {
            throw new NotImplementedException();
        }


        public override DbType getDbType(string typeString)
        {
            throw new NotImplementedException();
        }

        public override string getTableTitle(string tableName)
        {
            throw new NotImplementedException();
        }

        public override string getTableDescription(string tableName)
        {
            throw new NotImplementedException();
        }

        public override void renameTable(string tableName, string newTableName)
        {
            throw new NotImplementedException();
        }

        public override void renameFieldName(string tableName, string fieldName, string newName)
        {
            throw new NotImplementedException();
        }

        public override string typeOfDotNetType(Type type)
        {
            throw new NotImplementedException();
        }

        public override string typeOfDbType(DbType dbType)
        {
            throw new NotImplementedException();
        }
    }
}
