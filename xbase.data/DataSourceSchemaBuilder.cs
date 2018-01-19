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
    public static class DataSourceSchemaBuilder
    {
        private const int DEF_PAGE_SIZE = 50;

        private static DataSourceSchema CreateSqlTextSchema(string tableName, string connectionName, string selectSql)
        {

            DataSourceSchema schema = new DataSourceSchema();

            schema.Id = tableName;
            schema.TableName = tableName;
            schema.SelectCommand.CommandText = selectSql;
            schema.SelectCommand.CommandType = CommandType.Text;
            schema.ConnectionName = connectionName;
            schema.PageSize = DEF_PAGE_SIZE;

            BuildFields(schema);

            BuildSchemaCommand(schema);

            return schema;
        }


        private static void BuildFields(DataSourceSchema schema)
        {

            DsAdapterCustomer dsa = new DsAdapterCustomer(schema);
            DataSet ds = dsa.getDataSet();
            DataTable netTable = ds.Tables[0];


            //  schema.Fields.Clear();
            SchemaList<FieldSchema> tmpFieldSchema = new SchemaList<FieldSchema>();

            //da.FillSchema(netTable, SchemaType.Source);
            //     schema.PrimaryKeys = new List<string>();
            //     foreach (DataColumn col in netTable.PrimaryKey)
            //     {
            //         schema.PrimaryKeys.Add(col.ColumnName);
            //     }
            foreach (DataColumn col in netTable.Columns)
            {
                FieldSchema field = new FieldSchema();
                field.Id = col.ColumnName;
                field.Title = col.Caption;
                if (col.ExtendedProperties.ContainsKey(DataSourceConst.ExProDescription))
                    if (col.ExtendedProperties[DataSourceConst.ExProDescription] != null)
                        field.Description = col.ExtendedProperties[DataSourceConst.ExProDescription].ToString();
                field.ReadOnly = col.ReadOnly;
                field.ReadOnly = col.AutoIncrement;

                if (netTable.PrimaryKey.Contains(col))
                    field.IsKey = true;

                if (col.ExtendedProperties.ContainsKey(DataSourceConst.ExProDbType))
                    field.DataType = (DbType)col.ExtendedProperties[DataSourceConst.ExProDbType];
                else
                    field.DataType = DatabaseAdmin.getInstance().getDbType(col.DataType);

                //                    schema.Fields.Add(field);
                if (schema.Fields.FindItem(field.Id) == null)
                    schema.Fields.Add(field);

                if (tmpFieldSchema.FindItem(field.Id) == null)
                    tmpFieldSchema.Add(field);

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

        public static DataSourceSchema BuildTableSchema(string tableFullName)
        {
            string connName = null;
            string tableName = tableFullName;
            //            string[] paths = tableFullName.Split(ContainerConst.NamePathChar);
            string[] paths = tableFullName.Split('.');

            if (paths.Length > 1)
            {
                connName = paths[0];
                tableName = paths[1];
            }
            return BuildTableSchema(connName, tableName);
        }

        public static DataSourceSchema BuildTableSchema(string connName, string tableName)
        {
            string defCon = ConnectionAdmin.getDefaultConnName();
            if (!string.IsNullOrEmpty(connName) && connName.Equals(defCon, StringComparison.OrdinalIgnoreCase))
                connName = null;
            DatabaseAdmin dba = DatabaseAdmin.getInstance(connName);

            DataSourceSchema ret = new DataSourceSchema();
            ret.ConnectionName = connName;
            ret.TableName = tableName;
            ret.SelectCommand = new CommandSchema();
            ret.SelectCommand.CommandText = tableName;
            ret.SelectCommand.CommandType = CommandType.TableDirect;
            BuildFields(ret);
            ret.PrimaryKeys = dba.GetPrimaryKeys(tableName);
            BuildSchemaCommand(ret);
            //string dsId = tableName;
            //if (!string.IsNullOrEmpty(connName))
            //    dsId = connName + "." + dsId;
            //  DataSourceSchemaContainer.Instance().AddItem(dsId, ret);
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
