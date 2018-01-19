using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using xbase.data.db;
using Newtonsoft.Json;

namespace xbase.data
{
    public static class TableSchemaTools
    {
        private const int DEF_PAGE_SIZE = 50;

        private static DataSourceSchema CreateTableSchema(string tableId, string connectionName, string selectSql)
        {

            DataSourceSchema schema = new DataSourceSchema();

            schema.Id = tableId;
            schema.SelectCommand.CommandText = selectSql;
            schema.SelectCommand.CommandType =(int)CommandType.Text;
            schema.ConnectionName = connectionName;
            schema.PageSize = DEF_PAGE_SIZE;

            BuildFields(schema);

            BuildSchemaCommand(schema);

            return schema;
        }


        private static void BuildFields(DataSourceSchema schema)
        {
            string connectionName = schema.ConnectionName;
            string selectSql = schema.SelectCommand.CommandText;

            DatabaseAdmin dbAdmin = DatabaseAdmin.getInstance(connectionName);


            XSql xSql = new XSql(selectSql);

            string tableName = xSql.GetFrom();
            //  schema.Fields.Clear();

            SchemaList<FieldSchema> tmpfs = new SchemaList<FieldSchema>();

            DbConnection con = dbAdmin.Database.CreateConnection();
            try
            {
                DataTable netTable = new DataTable();

                DbDataAdapter da = dbAdmin.Database.GetDataAdapter();

                DbCommand cmd = con.CreateCommand();
                cmd.CommandText = selectSql;

                string[] paramNames = xSql.GetParamNames();
                for (int i = 0; i < paramNames.Count(); i++)
                {
                    string pName = paramNames[i];
                    if (!string.IsNullOrEmpty(pName))
                        pName = pName.Trim();
                    SqlParameter p = new SqlParameter();
                    p.ParameterName = "@" + pName;
                    //p.SourceColumn = pName;
                    p.Value = "";
                    cmd.Parameters.Add(p);
                }
                da.SelectCommand = cmd;

                da.Fill(netTable);
                //da.FillSchema(netTable, SchemaType.Source);

                foreach (DataColumn col in netTable.Columns)
                {
                    FieldSchema field = new FieldSchema();
                    field.Id = col.ColumnName;
                    field.DisplayName = col.Caption;
                    //                    schema.Fields.Add(field);
                    if (schema.Fields.FindItem(field.Id) == null)
                        schema.Fields.Add(field);

                    if (tmpfs.FindItem(field.Id) == null)
                        tmpfs.Add(field);

                }


                //从原是表中获取，字段标题等信息；

                if (xSql.IsFromSourceTable(tableName))
                {
                    List<string> pks = dbAdmin.GetPrimaryKeys(tableName);
                    schema.KeyField = JsonConvert.SerializeObject(pks);
                    //if (pks.Count > 0)
                    //    keyfield = pks[0];
                    //if (!string.IsNullOrEmpty(keyfield))
                    //    schema.KeyField = keyfield;
                    var tbName = tableName.Trim('[', ']');
                    List<FieldDef> fieldDefs = dbAdmin.getTableDef(tbName).FieldDefs;

                    for (int i = 0; i < fieldDefs.Count; i++)
                    {
                        FieldDef fieldDef = fieldDefs[i];
                        FieldSchema field = schema.Fields.FindItem(fieldDef.Name);

                        //                    field.Id =;
                        if (field != null)
                        {
                            field.IsKey = pks.Contains(field.Id);
                            field.DisplayName = fieldDef.Alias;//未用
                            if (string.IsNullOrEmpty(field.Title))
                                field.Title = fieldDef.Alias;
                            if (fieldDef.IsIdentity)
                                field.IsAutoInc = true;
                        }
                        //                    if (field == null)
                        //                        schema.Fields.Add(field);
                        //    if (tmpfs.FindItem(field.Id) == null)
                        //        tmpfs.Add(field);
                    }
                }

                //删除不存在的字段
                for (int i = schema.Fields.Count - 1; i >= 0; i--)
                {
                    FieldSchema fld = schema.Fields[i];
                    FieldSchema newFld = tmpfs.FindItem(fld.Id);
                    if (newFld == null)
                        schema.Fields.Remove(fld);
                }
            }
            finally
            {
                con.Close();
            }
        }

        /// <summary>
        /// 根据数据源的Select命令和列配置等，创建数据源配置的其他SQL命令
        /// </summary>
        /// <param name="schema"></param>
        public static void BuildSchemaCommand(DataSourceSchema schema)
        {

            XSqlBuilder cmdBuild = new XSqlBuilder(schema);
            try
            {
                //                schema.UpdateCommand = cmdBuild.GetUpdateCommand(true).CommandText;
                schema.UpdateCommand.CommandText = cmdBuild.GetUpdateCommand();
            }
            catch
            {
                schema.UpdateCommand.CommandText = "";
            }

            try
            {
                //schema.InsertCommand = cmdBuild.GetInsertCommand(true).CommandText;
                schema.InsertCommand.CommandText = cmdBuild.GetInsertCommand();
            }
            catch (Exception)
            {

                schema.InsertCommand.CommandText = "";
            }

            try
            {
                //schema.DeleteCommand = cmdBuild.GetDeleteCommand(true).CommandText;
                schema.DeleteCommand.CommandText = cmdBuild.GetDeleteCommand();
            }
            catch (Exception)
            {
                schema.DeleteCommand.CommandText = "";
            }
        }

        public static DataSourceSchema BuildTableSchema(string fullTableName)
        {
            string[] paths = fullTableName.Split(ContainerConst.NamePathChar);
            string connName = paths[0];
            string tbName = paths[1];
            return BuildTableSchema(connName, tbName);
        }

        public static DataSourceSchema BuildTableSchema(string connName, string tableName)
        {
            string defCon = ConnectionAdmin.getDefaultConnName();

            if (connName.Equals(defCon, StringComparison.OrdinalIgnoreCase))
                connName = null;

            string sql = "Select top "+DEF_PAGE_SIZE.ToString()+" * From [" + tableName + "]";
            DataSourceSchema ret = CreateTableSchema(tableName, connName, sql);
            string dsId = tableName;
            if (!string.IsNullOrEmpty(connName))
                dsId = connName + "." + dsId;
            DataSourceSchemaContainer.Instance().AddItem(dsId, ret);
            return ret;
        }

        /// <summary>
        /// //用物理数据库中的字段及表信息，重新更新XTableSchema信息
        /// </summary>
        /// <param name="tableId"></param>
        /// <param name="schema"></param>
        public static void RefreshSchemaFields(string tableId, DataSourceSchema schema)
        {
            BuildFields(schema);
            DataSourceSchemaContainer.Instance().UpdateItem(tableId, schema);
        }
    }
}
