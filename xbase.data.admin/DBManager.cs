using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Configuration;
using System.Xml;
//using XLogging;
using xbase.data;
using xbase;
using xbase.data.db;
using xbase.Exceptions;

namespace xbase.data.admin
{


    public class DBManager
    {
        XDatabaseFactory db = XDatabaseFactory.Instance;

        ConnectionInfo ci = new ConnectionInfo();

        public DbCategory GetDbExplorer()
        {
            return GetDbExplorer("");
        }

        public XTableSchema GetTableSchema(string tableFullName)
        {
            XTableSchema tableSchema = null;
            if (XTableSchemaContainer.Instance().Contains(tableFullName))
                tableSchema = XTableSchemaContainer.Instance().GetItem(tableFullName);
            else if (TableSchemaTools.isSrcTable(tableFullName))
            {
                tableSchema = TableSchemaTools.BuildSrcTableSchema(tableFullName);
            }
            else
                throw new XException("不能发现表(" + tableFullName + ")");

            return tableSchema;
        }

        public XTableSchema RefreshTableSchemaFields(string tableId)
        {
            XTableSchema tableSchema = null;

            if (XTableSchemaContainer.Instance().Contains(tableId))
                tableSchema = XTableSchemaContainer.Instance().GetItem(tableId);
            else
                throw new XException("不能发现表(" + tableId + ")");

            TableSchemaTools.RefreshSchemaFields(tableId, tableSchema);

            return tableSchema;

        }

        public void UpdateTableSchema(string tableFullName, XTableSchema schema)
        {
            XTableSchemaContainer.Instance().UpdateItem(tableFullName, schema);
        }

        public bool CheckTableExists(string tableFullName)
        {
            return XTableSchemaContainer.Instance().Contains(tableFullName);
        }

        public DbCategory GetDbExplorer(string connName)
        {
            DbCategory dbCategory = new DbCategory();
            ConnectionSchema connSchema = db.GetSchema().Connections.GetItem(connName);
            return GetDbExplorer(connSchema);
        }

        public DbCategory GetDbExplorer(ConnectionSchema conSchema)
        {

            DbCategory dbCategory = new DbCategory();
            dbCategory.DbName = conSchema.Id;
            dbCategory.DbTitle = conSchema.Title;

            if (string.IsNullOrEmpty(dbCategory.DbTitle))
                dbCategory.DbTitle = dbCategory.DbName;

            DbConnection conn = null;
            try
            {
                conn = db.GetConnection(conSchema.Id);
            }
            catch (Exception e)
            {
                dbCategory.Err = e.Message;
                return dbCategory;
            }

            DataTable dt = conn.GetSchema("Tables");
            DataView dv = dt.DefaultView;
            dv.Sort = "TABLE_NAME";

            foreach (DataRowView dr in dv)
            {
                ObjectDocket od = new ObjectDocket();

                od.Name = dr["TABLE_NAME"].ToString();
                od.Title = dr["TABLE_NAME"].ToString();

                string tbType = dr["TABLE_TYPE"].ToString();


                if (tbType.Equals("VIEW"))
                {
                    dbCategory.Views.Add(od);
                }
                // if (tbType.Equals("TABLE"))
                else
                {
                    if (!od.Name.Equals("sysdiagrams", StringComparison.OrdinalIgnoreCase))
                        if (!od.Name.Equals("dtproperties", StringComparison.OrdinalIgnoreCase))
                            dbCategory.Tables.Add(od);
                }
            }
            return dbCategory;
        }


        public List<DbCategory> GetDataCategorys()
        {
            DatabaseSchema dbSchema = db.GetSchema();
            List<DbCategory> ret = new List<DbCategory>();
            foreach (ConnectionSchema conSchema in dbSchema.Connections)
            {
                ret.Add(GetDbExplorer(conSchema));
            }
            return ret;
        }

        public List<DataSummary> GetDbSummarisek()
        {
            List<DataSummary> summarise = new List<DataSummary>();

            DbConnection conn = db.GetConnection();

            DataTable dt = conn.GetSchema("Tables");
            DataView dv = dt.DefaultView;
            dv.Sort = "TABLE_NAME";

            conn.Close();
            SchemaContainer<XTableSchema> xtc = XTableSchemaContainer.Instance();

            foreach (DataRow dr in dt.Rows)
            {
                DataSummary tbSummary = new DataSummary();
                tbSummary.Name = dr["TABLE_NAME"].ToString();
                tbSummary.Description = dr["TABLE_NAME"].ToString();
                tbSummary.Caption = dr["TABLE_NAME"].ToString();
                tbSummary.Type = dr["TABLE_TYPE"].ToString();

                XTableSchema ts = xtc.GetItem(tbSummary.Name);
                if (xtc.Contains(tbSummary.Name))
                {
                    tbSummary.Caption = ts.Title;
                    tbSummary.Description = ts.Description;
                }
                summarise.Add(tbSummary);
            }
            return summarise;
        }

        public List<DataSummary> GetDbSummarise()
        {
            List<DataSummary> summarise = new List<DataSummary>();

            DbConnection conn = db.GetConnection();

            DataTable dt = conn.GetSchema("Tables");
            DataView dv = dt.DefaultView;
            dv.Sort = "TABLE_NAME";

            conn.Close();
            SchemaContainer<XTableSchema> xtc = XTableSchemaContainer.Instance();

            foreach (DataRowView dr in dv)
            {
                DataSummary tbSummary = new DataSummary();
                tbSummary.Name = dr["TABLE_NAME"].ToString();
                tbSummary.Description = dr["TABLE_NAME"].ToString();
                tbSummary.Caption = dr["TABLE_NAME"].ToString();
                tbSummary.Type = dr["TABLE_TYPE"].ToString();

                XTableSchema ts = xtc.GetItem(tbSummary.Name);
                if (xtc.Contains(tbSummary.Name))
                {
                    tbSummary.Caption = ts.Title;
                    tbSummary.Description = ts.Description;
                }
                summarise.Add(tbSummary);
            }
            return summarise;
        }

        public List<ObjectDocket> GetTableSummarise(string folder)
        {
            List<ObjectDocket> ret = new List<ObjectDocket>();
            string[] tables = XTableSchemaContainer.Instance().GetIDByFolder(folder);

            for (int i = 0; i < tables.Length; i++)
            {
                ObjectDocket tabSum = new ObjectDocket();
                tabSum.Name = tables[i];
                XTableSchema tabsch = XTableSchemaContainer.Instance().GetItem(tables[i]);
                tabSum.Title = tabsch.Title;
                ret.Add(tabSum);
            }
            return ret;

        }

        public XTableSchema BuildTableCommand(XTableSchema schema)
        {
            TableSchemaTools.BuildSchemaCommand(schema);
            return schema;
        }

        public List<string> GetTableNames_delete()
        {
            //            DbConnection conn = db.GetConnection();
            DbConnection conn = db.GetConnection();

            //try
            //{
            //                conn.Open();
            DataTable dt = conn.GetSchema("Tables");
            //                conn.Close();

            List<string> tableNames = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                var tableName = dr["TABLE_NAME"].ToString();
                if (tableName.Equals("dtproperties")) continue;
                if (tableName.StartsWith("sys")) continue;
                tableNames.Add(tableName);
            }

            return tableNames;
            //}
            //catch (Exception ex)
            //{
            //conn.Close();

            //XLog xl = new XLog("Get Table Names", ex.Message);
            //xl.WriteLog();

            //    throw ex;
            //}
        }

        public TableInfo GetTableInfo(string tableName)
        {
            DbConnection conn = db.GetConnection();

            //try
            //{
            //                conn.Open();
            DataTable dt = conn.GetSchema("Columns");
            //                conn.Close();

            TableInfo ti = new TableInfo();
            ti.Name = tableName;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["TABLE_NAME"].ToString() == tableName)
                {
                    FieldInfo fi = new FieldInfo();

                    fi.Name = dr["COLUMN_NAME"].ToString();
                    fi.IsNull = dr["IS_NULLABLE"].ToString();
                    fi.DataType = dr["DATA_TYPE"].ToString();
                    fi.MaxLength = dr["CHARACTER_MAXIMUM_LENGTH"].ToString();

                    ti.FieldInfo.Add(fi);
                }
            }

            return ti;
            //}
            //catch (Exception ex)
            //{
            //    conn.Close();

            //    XLog xl = new XLog("Get Single Table Infomation", "Table Name:" + tableName + " Message:" + ex.Message);
            //    xl.WriteLog();

            //    throw ex;
            //}
        }

        public TableInfo SearchTableInfo(string sql)
        {
            sql = sql.ToUpper();
            if (!sql.Contains("SELECT"))
            {
                throw new Exception("必须为SELECT语句！");
            }

            if (sql.Contains("WHERE"))
            {
                int index = sql.IndexOf("WHERE");
                sql = sql.Remove(index);
            }

            sql = sql + " WHERE  1<>1";

            DbConnection con = db.GetConnection();
            DbCommand cmd = con.CreateCommand();
            cmd.CommandText = sql;

            DbDataAdapter da = db.GetAdapter();
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();

            TableInfo ti = new TableInfo();
            ti.Name = ds.DataSetName;

            foreach (DataColumn dc in ds.Tables[0].Columns)
            {
                FieldInfo fi = new FieldInfo();

                fi.Name = dc.ColumnName;
                fi.IsNull = dc.AllowDBNull.ToString();
                fi.DataType = dc.DataType.ToString();
                fi.MaxLength = dc.MaxLength.ToString();
                ti.FieldInfo.Add(fi);
            }

            return ti;
        }

        public ConnectionInfo GetConnectionInfo()
        {
            try
            {
                return ci;
            }
            catch (Exception ex)
            {
                // XLog xl = new XLog("Get Connection Info", ex.Message);
                // xl.WriteLog();

                throw ex;
            }
        }

        public List<string> GetStoredProcedures()
        {
            DbConnection conn = db.GetConnection();

            //try
            //{
            //    conn.Open();
            DataTable dt = conn.GetSchema("Procedures");
            //conn.Close();

            List<string> storedProcedures = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                storedProcedures.Add(dr["SPECIFIC_NAME"].ToString());
            }

            return storedProcedures;
            //}
            //catch (Exception ex)
            //{
            //    conn.Close();

            //    XLog xl = new XLog("Get Stored Procedures", ex.Message);
            //    xl.WriteLog();

            //    throw ex;
            //}
        }

        public List<string> GetViews()
        {
            DbConnection conn = db.GetConnection();

            //try
            //{
            //    conn.Open();
            DataTable dt = conn.GetSchema("Views");
            //conn.Close();

            List<string> views = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                views.Add(dr["TABLE_NAME"].ToString());
            }

            return views;
            //}
            //catch (Exception ex)
            //{
            //    conn.Close();

            //    XLog xl = new XLog("Get Views", ex.Message);
            //    xl.WriteLog();

            //    throw ex;
            //}
        }

        public DataTable OpenTable(string tableName)
        {
            DbDataAdapter da = db.GetAdapter();
            DbConnection con = db.GetConnection();
            DbCommand cmd = con.CreateCommand();

            cmd.CommandText = "Select * From [" + tableName + "]";
            DataTable table = new DataTable();
            da.SelectCommand = cmd;
            da.Fill(table);
            con.Close();
            return table;
        }
    }
}
