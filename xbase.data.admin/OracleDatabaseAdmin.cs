using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
namespace xbase.data.admin
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
                                if (!field.IsNull)
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
                                else if (field.Length > 0 && field.Type.ToLower().ToLower() != "int" && field.Type.ToLower().ToLower() != "blob" && field.Type.ToLower().ToLower() != "clob"&& field.Type.ToLower().ToLower() != "nclob" && field.Type.ToLower().ToLower() != "date" && field.Type.ToLower().ToLower() != "timestamp")
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
                            cmd = Database.GetSqlStringCommand(strSql.ToString().ToUpper());
                            Database.ExecuteNonQuery(cmd);
                        }



                        if (!string.IsNullOrEmpty(noteStr.ToString()))
                        {
                            //字段说明
                            string[] strArr = noteStr.ToString().Split(';');
                            foreach (string s in strArr)
                            {
                                cmd = Database.GetSqlStringCommand(s.ToUpper());
                                Database.ExecuteNonQuery(cmd);
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
                            cmd = Database.GetSqlStringCommand(strKeySql.ToUpper());
                            Database.ExecuteNonQuery(cmd);
                        }

                        if (!string.IsNullOrEmpty(strConstrains.ToString()))
                        {
                            string[] strArr = strConstrains.ToString().Split(';');
                            foreach (string s in strArr)
                            {
                                cmd = Database.GetSqlStringCommand(s.ToUpper());
                                Database.ExecuteNonQuery(cmd);
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
            catch
            {
                bRet = false;
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
            	
            }
           

            return tbNames;
        }

        /// <summary>
        /// 返回当前表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns>失败返回null</returns>
        public override TableDef getTableDef(string tableName)
        {
            TableDef tbDef = new TableDef();
            FieldDef fdDef;
            
            try
            {
                //返回当前表所有字段
                string strSql = "SELECT USER_TAB_COLS.COLUMN_NAME as colName ,USER_TAB_COLS.DATA_TYPE as colType, USER_TAB_COLS.DATA_LENGTH as colLength,DATA_PRECISION as intLength,DATA_SCALE as pointCount, USER_TAB_COLS.NULLABLE as colIsNull,USER_TAB_COLS.DATA_DEFAULT as colVal, "+
                "case  when (SELECT col.column_name FROM user_constraints con, user_cons_columns col where con.constraint_name = col.constraint_name and con.constraint_type='P' and col.table_name = upper('"+ tableName.ToUpper() +"') and col.column_name = USER_TAB_COLS.COLUMN_NAME) = USER_TAB_COLS.COLUMN_NAME THEN 'True'"+ 
                      "ELSE 'False'"+
                " END "+
                 " as pkName, "+
                 "case when (SELECT col.column_name FROM user_constraints con, user_cons_columns col where con.constraint_name = col.constraint_name and con.constraint_type='U' and col.table_name = upper('"+ tableName.ToUpper() +"') and col.column_name = USER_TAB_COLS.COLUMN_NAME) = USER_TAB_COLS.COLUMN_NAME THEN 'True'"+ 
                "ELSE 'False'"+
                 " END "+
                  " as colIsUnique, "+
                "case when (select count(*) from user_constraints con, user_cons_columns col, (select t2.table_name,t2.column_name,t1.r_constraint_name from user_constraints t1,user_cons_columns t2 where t1.r_constraint_name=t2.constraint_name and t1.table_name=upper('"+ tableName.ToUpper() +"')) r where con.constraint_name=col.constraint_name and con.r_constraint_name=r.r_constraint_name and con.table_name=upper('"+ tableName.ToUpper() +"') and col.column_name = USER_TAB_COLS.COLUMN_NAME) > 0 then 'True'"+
                " else 'False'"+
               " END "+
                " as fkName, "+ 
                "user_col_comments.comments as colNote FROM USER_TAB_COLS inner join user_col_comments on user_col_comments.TABLE_NAME = USER_TAB_COLS.TABLE_NAME and user_col_comments.COLUMN_NAME=USER_TAB_COLS.COLUMN_NAME and USER_TAB_COLS.TABLE_NAME = upper('"+ tableName.ToUpper() +"') ORDER BY USER_TAB_COLS.COLUMN_ID";

                DataTable tb = getQueryTable(strSql, 0, null);
                if (tb!=null)
                {
                    tbDef.Name = tableName;
                    foreach (DataRow row in tb.Rows)
                    {
                        fdDef = new FieldDef();
                        fdDef.Name = row["COLNAME"].ToString();
                        fdDef.Type = row["COLTYPE"].ToString();
                        if (!string.IsNullOrEmpty(row["COLLENGTH"].ToString()))
                        {
                            fdDef.Length = int.Parse(row["COLLENGTH"].ToString());
                        }
                        else
                        {
                            fdDef.Length = 0;
                        }

                        if (!string.IsNullOrEmpty(row["POINTCOUNT"].ToString()))
                        {
                            fdDef.Procesion = int.Parse(row["POINTCOUNT"].ToString());
                        }

                        if (!string.IsNullOrEmpty(row["COLVAL"].ToString()))
                        {
                            fdDef.DefaultValue = row["COLVAL"].ToString();
                        }

                        if (!string.IsNullOrEmpty(row["COLNOTE"].ToString()))
                        {
                            fdDef.Alias = row["COLNOTE"].ToString();
                        }


                        if (!string.IsNullOrEmpty(row["COLISUNIQUE"].ToString()))
                        {
                            fdDef.IsGuid = bool.Parse(row["COLISUNIQUE"].ToString());
                        }
                        else
                        {
                            fdDef.IsGuid = false;
                        }

                        if (row["COLISNULL"].ToString() == "Y")
                        {
                            fdDef.IsNull = true;
                        }
                        else
                        {
                            fdDef.IsNull = false;
                        }

                        fdDef.Constrains = "";
                        tbDef.FieldDefs.Add(fdDef);
                        if (!string.IsNullOrEmpty(row["PKNAME"].ToString()) && row["PKNAME"].ToString() == "True")
                        {
                            fdDef.IsPriKey = true;
                            tbDef.MainKeys.Add(fdDef);
                        }
                        else
                        {
                            fdDef.IsPriKey = false;
                        }

                    }
                }
                
            }
            catch
            {
                tbDef = null;
            }

            return tbDef;
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
            string  noteStr;
           

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
                                       cmd = Database.GetSqlStringCommand(noteStr.ToUpper());
                                       Database.ExecuteNonQuery(cmd);
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

                                        if (tableDef.FieldDefs[i].IsNull != existTbDef.FieldDefs[j].IsNull)
                                        {
                                            if (!tableDef.FieldDefs[i].IsNull)
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
                                            cmd = Database.GetSqlStringCommand(strSql.ToUpper());
                                            Database.ExecuteNonQuery(cmd);
                                            if (!string.IsNullOrEmpty(tableDef.FieldDefs[i].Constrains))
                                            {
                                                strConstrains.Append(tableDef.FieldDefs[i].Constrains.ToUpper() + ";");
                                            }
                                        }
                                        catch (System.Exception ex)
                                        {
                                        	
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
                                strConstrains.Append(fd.Constrains+" ;");                              
                                
                            }
                            if (!string.IsNullOrEmpty(fd.DefaultValue))
                            {
                                attachStr += "  default '"+fd.DefaultValue+"'";
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

                            if (!fd.IsNull)
                            {
                                attachStr += "  not null ";
                            }
                            else if(fd.IsPriKey)
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
                            cmd = Database.GetSqlStringCommand(strSql.ToUpper());
                            Database.ExecuteNonQuery(cmd);


                            //字段说明
                            if (!string.IsNullOrEmpty(fd.Alias))
                            {
                                noteStr = "COMMENT ON COLUMN " + tableDef.Name + "." + fd.Name + " IS '" + fd.Alias + "'";
                                cmd = Database.GetSqlStringCommand(noteStr.ToUpper());
                                Database.ExecuteNonQuery(cmd);
                            }

                        }

                        //删除列
                        foreach (FieldDef fd1 in existTbDef.FieldDefs)
                        {
                            strSql = "alter  table " + tableDef.Name + " DROP COLUMN " + fd1.Name + "";
                            cmd = Database.GetSqlStringCommand(strSql.ToUpper());
                            Database.ExecuteNonQuery(cmd);
                        }


                      
                        if (!string.IsNullOrEmpty(strConstrains.ToString()))
                        {
                            string[] strArr = strConstrains.ToString().Split(';');
                            foreach (string s in strArr)
                            {
                                cmd = Database.GetSqlStringCommand(s.ToUpper());
                                Database.ExecuteNonQuery(cmd);
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
                                cmd = Database.GetSqlStringCommand(strKeySql.ToUpper());
                                Database.ExecuteNonQuery(cmd);
                            }
                            catch
                            {
                            	
                            }                            

                            strKeySql = "ALTER TABLE " + tableDef.Name + "  ADD CONSTRAINT PK_" + tableDef.Name + " PRIMARY KEY ( " + strKeyName + ")";
                            cmd = Database.GetSqlStringCommand(strKeySql.ToUpper());
                            Database.ExecuteNonQuery(cmd);
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
            catch
            {
                bRet = false;
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
                        
                        DbCommand cmd = Database.GetSqlStringCommand(procText.ToUpper());
                        Database.ExecuteNonQuery(cmd);
                        bRet = true;
                    }
                }

            }
            catch
            {
                bRet = false;
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
            catch
            {

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
                        DbCommand cmd = Database.GetSqlStringCommand(procText.ToUpper());
                        Database.ExecuteNonQuery(cmd);
                        bRet = true;
                    }
                }
                else
                {
                    bRet = createProc(procName.ToUpper(), procText.ToUpper());
                }


            }
            catch
            {
                bRet = false;
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
                        DbCommand cmd = Database.GetSqlStringCommand(triggerText.ToUpper());
                        Database.ExecuteNonQuery(cmd);
                        bRet = true;
                    }
                }


            }
            catch
            {
                bRet = false;
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
            catch
            {

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
                        DbCommand cmd = Database.GetSqlStringCommand(triggerText.ToUpper());
                        Database.ExecuteNonQuery(cmd);
                        bRet = true;
                    }
                }
                else
                {
                    bRet = createTrigger(tableName, triggerName, triggerText);
                }


            }
            catch
            {
                bRet = false;
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
            StringBuilder strRet =new StringBuilder();
            string strSql = "SELECT name,text  from user_source where type = '" + xType.ToUpper() + "' and name='" + tpName.ToUpper() + "'";
            try
            {
                DataTable tb = getQueryTable(strSql.ToUpper(), 0, null);
                if (tb != null && tb.Rows.Count > 0)
                {
                    strRet.Append(" CREATE OR REPLACE ");
                    foreach(DataRow row in tb.Rows)
                    {
                        strRet.Append(row["TEXT"].ToString());
                    }
                   
                }
               
            }
            catch
            {
               
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
                string strSql = "SELECT constraint_name as Name, constraint_Type as Type FROM all_constraints WHERE table_name = upper('"+tableName+"') ";
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
            dicFieldTypes.Add("INT","INT");
            dicFieldTypes.Add("NUMBER","NUMBER");
            dicFieldTypes.Add("CHAR","CHAR");
            dicFieldTypes.Add("NCHAR","NCHAR");
            dicFieldTypes.Add("VARCHAR2","VARCHAR2");
            dicFieldTypes.Add("NVARCHAR2","NVARCHAR2");
            dicFieldTypes.Add("DATE", "DATE");
            dicFieldTypes.Add("TIMESTAMP","TIMESTAMP");
            dicFieldTypes.Add("BLOB","BLOB");
            dicFieldTypes.Add("CLOB","CLOB");
            dicFieldTypes.Add("NCLOB","NCLOB");            
            return dicFieldTypes;
        }

        public override List<string> getViewNames()
        {
            List<string> tbNames = null;
            string strSql = "SELECT view_name FROM user_views  ORDER BY view_name";         

            DataTable table = getQueryTable(strSql, 0, null);
            if (table!=null&&table.Rows.Count > 0)
            {
                tbNames = new List<string>();
                foreach (DataRow row in table.Rows)
                {
                    tbNames.Add(row["view_name"].ToString());
                }
            }

            return tbNames;
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
        public override bool deleteTable(string tbName, out string errMsg)
        {
            string strSql = "drop table " + tbName + "";
            return execNonQuery(strSql.ToUpper(),out errMsg);
        }

        /// <summary>
        /// 删除视图
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public override bool deleteView(string viewName, out string errMsg)
        {
            string strSql = "drop view " + viewName + "";
            return execNonQuery(strSql.ToUpper(), out errMsg);
        }

        /// <summary>
        /// 删除过程
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public override bool deleteProcedure(string spName, out string errMsg)
        {
            string strSql = "drop PROCEDURE " + spName + "";
            return execNonQuery(strSql.ToUpper(), out errMsg);
        }

        /// <summary>
        /// 删除触发器
        /// </summary>
        /// <param name="tgName"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public override bool deleteTrigger(string tgName, out string errMsg)
        {
            string strSql = "drop Trigger " + tgName + "";
            return execNonQuery(strSql.ToUpper(),out errMsg);
        }
    }
}
