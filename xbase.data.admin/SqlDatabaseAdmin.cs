using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
namespace xbase.data.admin
{
    public class SqlDatabaseAdmin : DatabaseAdmin
    {

        public SqlDatabaseAdmin(Database db)
            : base(db)
        {
        }

        public override string GetPrimaryKeys(string tableName)
        {
            return "";
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
                     bRet=  modifyTable(tableDef);
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
                                    noteStr.Append("EXECUTE sp_addextendedproperty N'MS_Description','" + field.Alias + "',N'user',N'dbo',N'table',N'" + tableDef.Name + "',N'column',N'" + field.Name + "';");
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
                                else if (field.IsPriKey)
                                {
                                    attachStr += "  not null ";
                                }
                             

                                if (field.IsGuid)
                                {
                                    attachStr += "  unique ";
                                }

                                if (field.IsIdentity)
                                {
                                    attachStr += "  identity(1,1)";
                                }

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


                        if (!string.IsNullOrEmpty(noteStr.ToString()))
                        {
                            //字段说明
                            cmd = Database.GetSqlStringCommand(noteStr.ToString());
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
                            strKeySql = "ALTER TABLE " + tableDef.Name + "  WITH NOCHECK ADD CONSTRAINT PK_" + tableDef.Name + " PRIMARY KEY  NONCLUSTERED (" + strKeyName + ")";
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
                tbNames = null;
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
                string strSql = "SELECT " +
    "tbName=case when a.colorder=1 then d.name else d.name end, " +
    "colName=a.name, " +
    "growMark=case when COLUMNPROPERTY(a.id,a.name,'IsIdentity')=1 then 'True' else 'False' end,  " +
    "pkName=case when exists(SELECT 1 FROM sysobjects where xtype= 'PK' and name in (  " +
    "SELECT name FROM sysindexes WHERE indid in(  " +
    "SELECT indid FROM sysindexkeys WHERE id = a.id AND colid=a.colid  " +
  "))) then 'True' else 'False' end,  " +
    "colIsUnique=case when exists(SELECT 1 FROM sysobjects where xtype= 'UQ' and name in (  " +
    "SELECT name FROM sysindexes WHERE indid in(  " +
    "SELECT indid FROM sysindexkeys WHERE id = a.id AND colid=a.colid  " +
  "))) then 'True' else 'False' end,  " +
   "colType=b.name,  " +
    "byteLength=a.length,  " +
    "colLength=COLUMNPROPERTY(a.id,a.name, 'PRECISION'),  " +
    "pointCount=isnull(COLUMNPROPERTY(a.id,a.name, 'Scale'),0),  " +
    "colIsNull=case when a.isnullable=1 then 'True'else 'False' end,  " +
    "colVal=isnull(e.text, ''),  " +
    "colNote=isnull(g.[value], '')  " +
    "FROM syscolumns a  " +
    "left join systypes b on a.xtype=b.xusertype  " +
    "inner join sysobjects d on a.id=d.id and d.xtype= 'U' and d.name <> 'dtproperties' and d.name = '" + tableName + "' " +
    "left join syscomments e on a.cdefault=e.id  " +
    "left join sys.extended_properties g on a.id=g.major_id and a.colid=g.minor_id and g.name='MS_Description'  " +
    "order by a.id,a.colorder ";
                DataTable tb = getQueryTable(strSql, 0, null);
                if (tb!=null)
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

                        if (!string.IsNullOrEmpty(row["growMark"].ToString()))
                        {
                            fdDef.IsIdentity = bool.Parse(row["growMark"].ToString());
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


                        if (!string.IsNullOrEmpty(row["colIsUnique"].ToString()))
                        {
                            fdDef.IsGuid = bool.Parse(row["colIsUnique"].ToString());
                        }
                        else
                        {
                            fdDef.IsGuid = false;
                        }

                        if (!string.IsNullOrEmpty(row["colIsNull"].ToString()))
                        {
                            fdDef.IsNull = bool.Parse(row["colIsNull"].ToString());
                        }
                        else
                        {
                            fdDef.IsNull = false;
                        }

                        fdDef.Constrains = "";
                        tbDef.FieldDefs.Add(fdDef);
                        if (!string.IsNullOrEmpty(row["pkName"].ToString()) && row["pkName"].ToString() == "True")
                        {
                            tbDef.MainKeys.Add(fdDef);
                            fdDef.IsPriKey = true;
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
            StringBuilder noteStr = new StringBuilder();

            string tmpStr = "";


            DbCommand cmd;
            string strSql = "";
            try
            {
                if (tableDef != null)
                {
                    if (isExistObject(tableDef.Name, "IsUserTable"))
                    {
                        existTbDef = getTableDef(tableDef.Name);
                        for (int i = 0; i < tableDef.FieldDefs.Count;i++ )                           
                            {
                                if (tableDef.FieldDefs[i].Name != "")
                                {

                                    for (int j = 0; j < existTbDef.FieldDefs.Count; j++) 
                                        {

                                            //字段说明
                                            if (!string.IsNullOrEmpty(tableDef.FieldDefs[i].Alias))
                                            {
                                                if (!string.IsNullOrEmpty(existTbDef.FieldDefs[j].Alias))
                                                {
                                                    noteStr.Append("EXECUTE sp_updateextendedproperty  N'MS_Description','" + tableDef.FieldDefs[i].Alias + "',N'user',N'dbo',N'table',N'" + tableDef.Name + "',N'column',N'" + tableDef.FieldDefs[i].Name + "';");
                                                }
                                                else
                                                {
                                                    noteStr.Append("EXECUTE sp_addextendedproperty  N'MS_Description','" + tableDef.FieldDefs[i].Alias + "',N'user',N'dbo',N'table',N'" + tableDef.Name + "',N'column',N'" + tableDef.FieldDefs[i].Name + "';");
                                                }
                                                
                                            }

                                            //修改列
                                            if (tableDef.FieldDefs[i].Name == existTbDef.FieldDefs[j].Name)
                                            {
                                                attachStr = "";
                                                if (tableDef.FieldDefs[i].Type == "decimal")
                                                {
                                                    tmpStr = tableDef.FieldDefs[i].Name + " " + tableDef.FieldDefs[i].Type + "(" + tableDef.FieldDefs[i].Length + "," + tableDef.FieldDefs[i].Procesion + ")";
                                                }
                                                else if (tableDef.FieldDefs[i].Length > 0 && tableDef.FieldDefs[i].Type.ToLower() != "int" && tableDef.FieldDefs[i].Type.ToLower() != "bigint" && tableDef.FieldDefs[i].Type.ToLower() != "tinyint" && tableDef.FieldDefs[i].Type.ToLower() != "money" && tableDef.FieldDefs[i].Type.ToLower() != "bit" && tableDef.FieldDefs[i].Type.ToLower() != "datetime" && tableDef.FieldDefs[i].Type.ToLower() != "text" && tableDef.FieldDefs[i].Type.ToLower() != "ntext" && tableDef.FieldDefs[i].Type.ToLower() != "timestamp" && tableDef.FieldDefs[i].Type.ToLower() != "smalldatetime" && tableDef.FieldDefs[i].Type.ToLower() != "image")
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
                                              
                                               

                                                tmpStr += attachStr;
                                                try
                                                {
                                                    strSql = "alter  table " + tableDef.Name + "  alter column " + tmpStr + " ";
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

                            //字段说明
                            if (!string.IsNullOrEmpty(fd.Alias))
                            {
                                noteStr.Append("EXECUTE sp_addextendedproperty N'MS_Description','" + fd.Alias + "',N'user',N'dbo',N'table',N'" + tableDef.Name + "',N'column',N'" + fd.Name + "';");
                            }

                            if (!string.IsNullOrEmpty(fd.Constrains))
                            {
                                strConstrains.Append(fd.Constrains);
                                strConstrains.Append(";");
                            }
                            if (!string.IsNullOrEmpty(fd.DefaultValue))
                            {
                                attachStr += "  default " + fd.DefaultValue;
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
                            else if (fd.IsPriKey)
                            {
                                attachStr += "  not null ";
                            }


                            if (fd.IsGuid)
                            {
                                attachStr += "  unique ";
                            }

                            if (fd.IsIdentity)
                            {
                                attachStr += "  identity(1,1)";
                            }

                            if (fd.Type.ToLower() == "decimal")
                            {
                                tmpStr = fd.Name + " " + fd.Type.ToLower() + "(" + fd.Length + "," + fd.Procesion + ")";
                            }
                            else if (fd.Length > 0 && fd.Type.ToLower() != "int" && fd.Type.ToLower() != "bigint" && fd.Type.ToLower() != "tinyint" && fd.Type.ToLower() != "money" && fd.Type.ToLower() != "bit" && fd.Type.ToLower() != "datetime" && fd.Type.ToLower() != "text" && fd.Type.ToLower() != "ntext" && fd.Type.ToLower() != "timestamp" && fd.Type.ToLower() != "smalldatetime" && fd.Type.ToLower() != "image")
                            {
                                tmpStr = fd.Name + " " + fd.Type.ToLower() + "(" + fd.Length + ")";
                            }
                            else
                            {
                                tmpStr = fd.Name + " " + fd.Type.ToLower();
                            }
                            tmpStr += attachStr;

                            strSql = "alter table " + tableDef.Name+ " add  " + tmpStr + "";
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

                        if (!string.IsNullOrEmpty(noteStr.ToString()))
                        {
                            cmd = Database.GetSqlStringCommand(noteStr.ToString());
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
                                strKeySql = "ALTER TABLE " + tableDef.Name + " DROP CONSTRAINT PK_" + tableDef.Name + "";
                                cmd = Database.GetSqlStringCommand(strKeySql.ToUpper());
                                Database.ExecuteNonQuery(cmd);
                            }
                            catch
                            {

                            }

                            strKeySql = "ALTER TABLE " + tableDef.Name + "  WITH NOCHECK ADD CONSTRAINT PK_" + tableDef.Name + " PRIMARY KEY  NONCLUSTERED ( " + strKeyName + ")";
                            cmd = Database.GetSqlStringCommand(strKeySql.ToUpper());
                            Database.ExecuteNonQuery(cmd);
                        }

                        bRet = true;
                    }
                    else
                    {
                      bRet= createTable(tableDef);
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
                if (isExistObject(procName, "IsProcedure"))
                {
                    bRet = modifyProc(procName, procText);
                }
                else
                {
                    if (!string.IsNullOrEmpty(procName) && !string.IsNullOrEmpty(procText))
                    {
                        procText = procText.ToUpper().Replace("ALTER", "CREATE");
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
            strRet = getTPContent("P", procName);
            if (strRet == "")
            {
                strRet = "Create Proc " +procName + " () AS ";
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
        public override bool createTrigger(string tableName,string triggerName,string triggerText)
        {
            bool bRet = false;
            try
            {
                if (isExistObject(triggerName, "IsTrigger"))
                {
                  bRet= modifyTrigger(tableName, triggerName, triggerText);
                }
                else
                {
                    if (!string.IsNullOrEmpty(tableName) && !string.IsNullOrEmpty(triggerText))
                    {
                        triggerText = triggerText.ToUpper().Replace("ALTER", "CREATE");
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
        public override bool modifyTrigger(string tableName,string triggerName,string triggerText)
        {
            bool bRet = false;
            try
            {
                if (isExistObject(triggerName, "IsTrigger"))
                {

                    if (!string.IsNullOrEmpty(tableName) &&!string.IsNullOrEmpty(triggerText))
                    {
                        triggerText = triggerText.ToUpper().Replace("CREATE", "ALTER");
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
            try
            {
                DataTable tb = getQueryTable(strSql, 0, null);
                if (tb != null && tb.Rows.Count > 0)
                {
                    strRet = tb.Rows[0]["text"].ToString();
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
        private bool isExistObject(string tbName,string para)
        {
            bool bRet = false;
            StringBuilder strBSql = new StringBuilder();
            strBSql.Append("if exists (select * from sysobjects where id = object_id(N'"+tbName+"') and OBJECTPROPERTY(id,N'"+para+"') = 1) ");
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
            dicFieldTypes.Add("tinyint","tinyint");
            dicFieldTypes.Add("int","int");
            dicFieldTypes.Add("bigint","bigint");
            dicFieldTypes.Add("bit","bit");
            dicFieldTypes.Add("decimal","decimal");
            dicFieldTypes.Add("char","char");
            dicFieldTypes.Add("nchar","nchar");
            dicFieldTypes.Add("varchar","varchar");
            dicFieldTypes.Add("nvarchar","nvarchar");
            dicFieldTypes.Add("text","text");
            dicFieldTypes.Add("ntext","ntext");
            dicFieldTypes.Add("timestamp","timestamp");
            dicFieldTypes.Add("smalldatetime","smalldatetime");
            dicFieldTypes.Add("datetime","datetime");
            dicFieldTypes.Add("money","money");
            dicFieldTypes.Add("image","image");
            dicFieldTypes.Add("binary","binary");
            return dicFieldTypes;
        }

        /// <summary>
        /// 返回数据库视图集合
        /// </summary>
        /// <returns></returns>
        public override List<string> getViewNames()
        {
            List<string> tbNames = null;
            try
            {
                string strSql = "SELECT Name FROM SysObjects Where XType='V' ORDER BY Name";

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
            string strSql = "select top 10 * from " + viewName + "";
            DataTable tb = getQueryTable(strSql, 0, null);
            return tb;

        }
      
        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="tbName"></param>
        /// <returns></returns>
        public override bool deleteTable(string tbName,out string errMsg)
        {
            string strSql = "drop table " + tbName + "";
            return execNonQuery(strSql.ToUpper(), out errMsg);
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

