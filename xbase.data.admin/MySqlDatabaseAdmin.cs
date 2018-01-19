using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
namespace xbase.data.admin
{
    public class MySqlDatabaseAdmin : DatabaseAdmin
    {

        public MySqlDatabaseAdmin(Database db)
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
            string noteStr = "";
            DbCommand cmd;

            try
            {
                if (tableDef != null)
                {
                    if (isExistObject(tableDef.Name, "Tb"))
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
                            noteStr = "";
                            if (field.Name != "")
                            {
                                //字段说明
                                if (!string.IsNullOrEmpty(field.Alias))
                                {
                                    noteStr=" comment  '" + field.Alias + "' ";
                                }

                                if (!string.IsNullOrEmpty(field.Constrains))
                                {
                                    strConstrains.Append(field.Constrains + ";");
                                }

                                if (!string.IsNullOrEmpty(field.DefaultValue))
                                {
                                    attachStr += "  default  " + field.DefaultValue;
                                }
                                if (!field.IsNull)
                                {
                                    attachStr += "  not null ";
                                }

                                if (field.IsIdentity)
                                {
                                    attachStr += "  auto_increment ";
                                    field.IsGuid = true;
                                }

                                if (field.IsGuid)
                                {
                                    attachStr += "  unique ";
                                }

                                if (field.IsIdentity)
                                {
                                    attachStr += "  auto_increment ";
                                }

                                if (field.Type.ToLower() == "decimal" || field.Type.ToLower() == "float")
                                {
                                    tmpStr = field.Name + " " + field.Type.ToLower() + "(" + field.Length + "," + field.Procesion + ")";
                                }
                                else if (field.Length > 0 && field.Type.ToLower() != "int" && field.Type.ToLower() != "blob" && field.Type.ToLower() != "enum" && field.Type.ToLower() != "datetime" && field.Type.ToLower() != "date" && field.Type.ToLower() != "time" && field.Type.ToLower() != "text")
                                {
                                    tmpStr = field.Name + " " + field.Type.ToLower() + "(" + field.Length + ")";
                                }
                                else
                                {
                                    tmpStr = field.Name + " " + field.Type.ToLower();
                                }
                                tmpStr +=noteStr.ToString();
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
                            cmd = Database.GetSqlStringCommand(strSql.ToString());
                            Database.ExecuteNonQuery(cmd);
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
                            strKeySql = "ALTER TABLE " + tableDef.Name + "  WITH NOCHECK ADD CONSTRAINT PK_" + tableDef.Name + " PRIMARY KEY  NONCLUSTERED ( " + strKeyName + ")";
                            cmd = Database.GetSqlStringCommand(strKeySql);
                            Database.ExecuteNonQuery(cmd);
                        }

                        if (!string.IsNullOrEmpty(strConstrains.ToString()))
                        {
                            cmd = Database.GetSqlStringCommand(strConstrains.ToString());
                            Database.ExecuteNonQuery(cmd);
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
                string strSql = "show tables";

                DataTable table = getQueryTable(strSql, 0, null);
                if (table != null && table.Rows.Count > 0)
                {
                    tbNames = new List<string>();
                    foreach (DataRow row in table.Rows)
                    {
                        tbNames.Add(row[0].ToString());
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
                string strSql = "SELECT COLUMN_NAME as colName, DATA_TYPE as colType,IFNULL(character_maximum_length,numeric_precision) as colLength,numeric_scale as pointCount,IS_NULLABLE colIsNull,COLUMN_DEFAULT as colVal,COLUMN_KEY as pkName,EXTRA as growMark, COLUMN_COMMENT as colNote FROM information_schema.columns as cols WHERE table_name ='"+tableName+"'";
                DataTable tb = getQueryTable(strSql, 0, null);
                if (tb != null)
                {
                    tbDef.Name = tableName;
                    foreach (DataRow row in tb.Rows)
                    {
                        fdDef = new FieldDef();
                        fdDef.Name = row["colName"].ToString();
                        fdDef.Type = row["colType"].ToString();
                        if (!string.IsNullOrEmpty(row["colLength"].ToString()))
                        {
                            fdDef.Length = int.Parse(row["colLength"].ToString());
                        }
                        else
                        {
                            fdDef.Length = 0;
                        }

                        if (!string.IsNullOrEmpty(row["growMark"].ToString()) && row["growMark"].ToString() == "auto_increment")
                        {
                            fdDef.IsIdentity = true;
                        }
                        else
                        {
                            fdDef.IsIdentity = false;
                        }

                        if (!string.IsNullOrEmpty(row["pointCount"].ToString()))
                        {
                            fdDef.Procesion = int.Parse(row["pointCount"].ToString());
                        }



                        if (!string.IsNullOrEmpty(row["colVal"].ToString()))
                        {
                            fdDef.DefaultValue = row["colVal"].ToString();
                        }


                        if (!string.IsNullOrEmpty(row["colNote"].ToString()))
                        {
                            fdDef.Alias = row["colNote"].ToString();
                        }


                        if (!string.IsNullOrEmpty(row["pkName"].ToString()))
                        {
                            if (row["pkName"].ToString() == "PRI")
                            {
                                fdDef.IsPriKey = true;
                            }
                            else
                            {
                                fdDef.IsPriKey = false;
                            }

                            if (row["pkName"].ToString() == "UNI")
                            {
                                fdDef.IsGuid = true;
                            }
                            else
                            {
                                fdDef.IsGuid = false;
                            }
                        }


                        if (!string.IsNullOrEmpty(row["colIsNull"].ToString()) && row["colIsNull"].ToString() == "YES")
                        {
                            fdDef.IsNull = true;
                        }
                        else
                        {
                            fdDef.IsNull = false;
                        }

                        fdDef.Constrains = "";
                        tbDef.FieldDefs.Add(fdDef);

                        if (fdDef.IsPriKey)
                        {
                            tbDef.MainKeys.Add(fdDef);

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
            string noteStr = "";

            string tmpStr = "";


            DbCommand cmd;
            string strSql = "";
            try
            {
                if (tableDef != null)
                {
                    if (isExistObject(tableDef.Name, "Tb"))
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

                                       noteStr = " comment  '" + tableDef.FieldDefs[i].Alias + "' "; 
                                                                            
                                    }

                                    //修改列
                                    if (tableDef.FieldDefs[i].Name == existTbDef.FieldDefs[j].Name)
                                    {
                                        attachStr = "";
                                        if (tableDef.FieldDefs[i].Type == "decimal")
                                        {
                                            tmpStr = tableDef.FieldDefs[i].Name + " " + tableDef.FieldDefs[i].Type + "(" + tableDef.FieldDefs[i].Length + "," + tableDef.FieldDefs[i].Procesion + ")";
                                        }
                                        else if (tableDef.FieldDefs[i].Length > 0 && tableDef.FieldDefs[i].Type.ToLower() != "int" && tableDef.FieldDefs[i].Type.ToLower() != "blob" && tableDef.FieldDefs[i].Type.ToLower() != "enum" && tableDef.FieldDefs[i].Type.ToLower() != "datetime" && tableDef.FieldDefs[i].Type.ToLower() != "date" && tableDef.FieldDefs[i].Type.ToLower() != "time" && tableDef.FieldDefs[i].Type.ToLower() != "text")
                                        {
                                            tmpStr = tableDef.FieldDefs[i].Name + " " + tableDef.FieldDefs[i].Type + "(" + tableDef.FieldDefs[i].Length + ")";
                                        }
                                        else
                                        {
                                            tmpStr = tableDef.FieldDefs[i].Name + " " + tableDef.FieldDefs[i].Type;
                                        }

                                        if (tableDef.FieldDefs[i].IsNull != existTbDef.FieldDefs[j].IsNull)
                                        {
                                            if (!tableDef.FieldDefs[i].IsNull)
                                            {

                                                attachStr += "  not null ";
                                            }
                                            else
                                            {
                                                attachStr += "  null ";
                                            }
                                        }
                                       
                                        if (tableDef.FieldDefs[i].IsPriKey)
                                        {
                                            attachStr += "  not null ";
                                        }
                                      
                                        

                                        if (!string.IsNullOrEmpty(tableDef.FieldDefs[i].DefaultValue))
                                        {
                                            if (tableDef.FieldDefs[i].DefaultValue != existTbDef.FieldDefs[j].DefaultValue)
                                            {
                                                attachStr += "  set default " + tableDef.FieldDefs[i].DefaultValue + "  ";
                                            }
                                        }

                                        if (tableDef.FieldDefs[i].IsIdentity)
                                        {
                                            attachStr += "  auto_increment ";
                                            tableDef.FieldDefs[i].IsGuid = true;
                                        }

                                        if (tableDef.FieldDefs[i].IsGuid)
                                        {
                                            attachStr += "  unique ";
                                        }
                                                                               

                                        tmpStr += attachStr;
                                        tmpStr += " "+ noteStr +" ";

                                        try
                                        {
                                            strSql = "alter  table " + tableDef.Name + "  modify column " + tmpStr + " ";
                                            cmd = Database.GetSqlStringCommand(strSql);
                                            Database.ExecuteNonQuery(cmd);
                                            if (!string.IsNullOrEmpty(tableDef.FieldDefs[i].Constrains))
                                            {
                                                strConstrains.Append(tableDef.FieldDefs[i].Constrains);
                                                strConstrains.Append(";");
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
                            noteStr = "";

                            //字段说明
                            if (!string.IsNullOrEmpty(fd.Alias))
                            {                               
                                noteStr = " comment  '" + fd.Alias + "' "; 
                            }

                            if (!string.IsNullOrEmpty(fd.Constrains))
                            {
                                strConstrains.Append(fd.Constrains);
                                strConstrains.Append(";");
                            }
                            if (!string.IsNullOrEmpty(fd.DefaultValue))
                            {
                                attachStr += "  default "+ fd.DefaultValue +" ";
                            }

                            if (fd.Type.ToLower() == "decimal")
                            {
                                tmpStr = fd.Name + " " + fd.Type.ToLower() + "(" + fd.Length + "," + fd.Procesion + ")";
                            }
                            else if (fd.Length > 0)
                            {
                                tmpStr = fd.Name + " " + fd.Type.ToLower() + "(" + fd.Length + ")";
                            }
                            else
                            {
                                tmpStr = fd.Name + " " + fd.Type.ToLower();
                            }

                            if (!fd.IsNull)
                            {
                                attachStr += "  not null ";
                            }
                            else if(fd.IsPriKey)
                            {
                                attachStr += "  not null ";
                            }

                            if (fd.IsIdentity)
                            {
                                attachStr += "  auto_increment ";
                                fd.IsGuid = true;
                            }

                            if (fd.IsGuid)
                            {
                                attachStr += "  unique ";
                            }


                            if (fd.Type.ToLower() == "decimal" || fd.Type.ToLower() == "float")
                            {
                                tmpStr = fd.Name + " " + fd.Type.ToLower() + "(" + fd.Length + "," + fd.Procesion + ")";
                            }
                            else if (fd.Length > 0 && fd.Type.ToLower() != "int" && fd.Type.ToLower() != "blob" && fd.Type.ToLower() != "enum" && fd.Type.ToLower() != "datetime" && fd.Type.ToLower() != "date" && fd.Type.ToLower() != "time" && fd.Type.ToLower() != "text")
                            {
                                tmpStr = fd.Name + " " + fd.Type.ToLower() + "(" + fd.Length + ")";
                            }
                            else
                            {
                                tmpStr = fd.Name + " " + fd.Type.ToLower();
                            }
                            tmpStr += attachStr;
                            tmpStr += noteStr;

                            strSql = "alter table " + tableDef.Name + " add  " + tmpStr + "";
                            cmd = Database.GetSqlStringCommand(strSql);
                            Database.ExecuteNonQuery(cmd);
                        }

                        //删除列
                        foreach (FieldDef fd1 in existTbDef.FieldDefs)
                        {
                            strSql = "alter  table " + tableDef.Name + " DROP COLUMN " + fd1.Name + "";
                            cmd = Database.GetSqlStringCommand(strSql);
                            Database.ExecuteNonQuery(cmd);
                        }

                      
                        if (!string.IsNullOrEmpty(strConstrains.ToString()))
                        {
                            cmd = Database.GetSqlStringCommand(strConstrains.ToString());
                            Database.ExecuteNonQuery(cmd);
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
                                strKeySql = "ALTER TABLE " + tableDef.Name + " drop primary key";
                                cmd = Database.GetSqlStringCommand(strKeySql.ToUpper());
                                Database.ExecuteNonQuery(cmd);
                            }
                            catch
                            {

                            }

                            strKeySql = "ALTER TABLE " + tableDef.Name + " ADD CONSTRAINT PK_" + tableDef.Name + " PRIMARY KEY  ( " + strKeyName + ")";
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
                if (isExistObject(procName, "SP"))
                {
                    bRet = modifyProc(procName, procText);
                }
                else
                {
                    if (!string.IsNullOrEmpty(procName) && !string.IsNullOrEmpty(procText))
                    {                        
                        DbCommand cmd = Database.GetSqlStringCommand(procText);
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
                strSql = "select specific_name as Name from mysql.proc ";

                tb = getQueryTable(strSql, 0, null);
                if (tb != null)
                {
                    foreach (DataRow row in tb.Rows)
                    {
                        listProc.Add(row["Name"].ToString());
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
            strRet = getTPContent("SP", procName);
            if (strRet == "")
            {
                strRet = "Create Procedure " + procName + " () begin   end ";
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
                if (isExistObject(procName, "SP"))
                {
                    if (!string.IsNullOrEmpty(procName) && !string.IsNullOrEmpty(procText))
                    {                       
                        DbCommand cmd = Database.GetSqlStringCommand(procText);
                        Database.ExecuteNonQuery(cmd);
                        bRet = true;
                    }
                }
                else
                {
                    bRet = createProc(procName, procText);
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
                if (isExistObject(triggerName, "Tg"))
                {
                    bRet = modifyTrigger(tableName, triggerName, triggerText);
                }
                else
                {
                    if (!string.IsNullOrEmpty(tableName) && !string.IsNullOrEmpty(triggerText))
                    {                        
                        DbCommand cmd = Database.GetSqlStringCommand(triggerText);
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
                    strSql = "SELECT trigger_Name as Name FROM  information_schema.TRIGGERS";
                }
                else
                {
                    strSql = "SELECT trigger_Name as Name FROM  information_schema.TRIGGERS where Event_Object_Table='" + tableName + "' ";
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
                if (isExistObject(triggerName, "Tg"))
                {

                    if (!string.IsNullOrEmpty(tableName) && !string.IsNullOrEmpty(triggerText))
                    {                       
                        DbCommand cmd = Database.GetSqlStringCommand(triggerText);
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
            strRet = getTPContent("Tg", triggerName);
            if (strRet == "")
            {
                strRet = "Create TRIGGER " + triggerName + " before|after insert|update|delete  ON  " + tableName + "  for each row   Begin   End ";
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
            string strSql ="";
            if (xType=="SP")
            {
                strSql = "select specific_name as Name,param_list as paras,body as content from mysql.proc where specific_name = '" + tpName + "'";
            }
            else if (xType=="Tg")
            {
                strSql = "select trigger_name as Name, event_object_table as tb,Event_manipulation as way,action_timing as time,action_statement as content  from information_schema.TRIGGERS where trigger_name='" + tpName + "'";
            }


            try
            {
                DataTable tb = getQueryTable(strSql, 0, null);
                if (tb != null && tb.Rows.Count > 0)
                {
                    if (xType == "SP")
                    {
                        strRet = "DROP PROCEDURE IF EXISTS " + tpName + "; Create Procedure " + tpName + " (" + System.Text.Encoding.Default.GetString((System.Byte[])tb.Rows[0]["paras"]) + ") " + System.Text.Encoding.Default.GetString((System.Byte[])tb.Rows[0]["content"]) + " ";
                    }
                    else if (xType == "Tg")
                    {
                        strRet = "DROP TRIGGER  IF EXISTS " + tpName + "; Create TRIGGER " + tpName + " " + tb.Rows[0]["time"].ToString() + " " + tb.Rows[0]["way"].ToString() + " on " + tb.Rows[0]["tb"].ToString() + " for each row " + tb.Rows[0]["content"].ToString() + "";
                    }
                    
                }
                else
                {
                    strRet = "";
                }
            }
            catch
            {
                strRet = "";
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
            string strSql = "";
            if (para=="Tb")
            {
                strSql = "SELECT count(*) FROM information_schema.TABLES WHERE table_name ='"+tbName+"'";
            }
            else if (para=="SP")
            {
                strSql = "select count(*) from mysql.proc where specific_name = '" + tbName + "'";
            }
            else if (para=="Tg")
            {
                strSql = "SELECT count(*) FROM  information_schema.TRIGGERS where trigger_Name='" + tbName + "'";
            }


            string strExsit = execScalar(strSql);
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
                string strSql = "SELECT CONSTRAINT_NAME as Name,CONSTRAINT_TYPE as Type FROM information_schema.TABLE_CONSTRAINTS WHERE TABLE_NAME='"+tableName+"'  ";
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
            return strRet;
        }

        /// <summary>
        /// 返回当前数据库字段数据类型
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, string> getFieldTypeList()
        {
            Dictionary<string, string> dicFieldTypes = new Dictionary<string, string>();
            dicFieldTypes.Add("int", "int");
            dicFieldTypes.Add("bigint","bigint");
            dicFieldTypes.Add("decimal","decimal");
            dicFieldTypes.Add("char","char");
            dicFieldTypes.Add("varchar","varchar");
            dicFieldTypes.Add("enum","enum");
            dicFieldTypes.Add("text","text");
            dicFieldTypes.Add("blob","blob");
            dicFieldTypes.Add("date","date");
            dicFieldTypes.Add("time","time");
            dicFieldTypes.Add("datetime","datetime");
            dicFieldTypes.Add("timestamp","timestamp");            
            return dicFieldTypes;
        }


        public override List<string> getViewNames()
        {
            return null;
        }

        /// <summary>
        /// 视图结构
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public override DataTable getViewData(string viewName)
        {
            string strSql = "select * from " + viewName + "  limit 0,10";
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
            return execNonQuery(strSql.ToUpper(),out errMsg);
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
            return execNonQuery(strSql.ToUpper(), out errMsg);
        }
    }
}
