using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using xbase;
using System.Data;
using xbase.data.db;
using System.Data.SqlClient;

namespace xbase.data
{
    public static class TableSchemaTools
    {


        private static XTableSchema CreateTableSchema(string tableId, string connectionName, string selectSql)
        {

            XTableSchema schema = new XTableSchema();

            schema.Id = tableId;
            schema.SelectCommand = selectSql;
            schema.ConnectionName = connectionName;

            BuildFields(schema);

            BuildSchemaCommand(schema);

            return schema;
        }



        private static void BuildFields1(XTableSchema schema)
        {
            string connectionName = schema.ConnectionName;
            string selectSql = schema.SelectCommand;

          //  XDataBase xdb = XDatabaseFactory.Instance.GetDataBase(connectionName);
            DatabaseAdmin

            XSql xSql = new XSql(selectSql);

            string tableName = xSql.GetFrom();
            //  schema.Fields.Clear();

            SchemaList<FieldSchema> tmpfs = new SchemaList<FieldSchema>();

            if (xSql.IsFromSourceTable(tableName))
            {
                string keyfield = xdb.GetPrimaryKey(tableName);
                if (!string.IsNullOrEmpty(keyfield))
                    schema.KeyField = keyfield;

                List<FieldDef> fieldDefs = xdb.GetFieldInfos(tableName);

                for (int i = 0; i < fieldDefs.Count; i++)
                {
                    FieldSchema field = new FieldSchema();
                    FieldDef fieldDef = fieldDefs[i];
                    field.Id = fieldDef.Name;
                    field.DisplayName = fieldDef.Alias;//未用
                    if (string.IsNullOrEmpty(field.Title))
                        field.Title = fieldDef.Alias;
                    if (schema.Fields.FindItem(field.Id) == null)
                        schema.Fields.Add(field);
                    if (tmpfs.FindItem(field.Id) == null)
                        tmpfs.Add(field);
                }
            }
            else
            {
                DbConnection con = xdb.GetConnection();
                DataTable netTable = new DataTable();

                DbDataAdapter da = xdb.GetAdapter();

                DbCommand cmd = con.CreateCommand();

                xSql.ParamGetter = delegate(string name)
                {
                    if (!name.StartsWith("@"))
                        name = "@" + name;

                    if (schema.QueryParams.FindItem(name) != null)
                    {
                        XQueryParameterSchema paramSch = schema.QueryParams.GetItem(name);
                        return paramSch.DefaultValue;
                    }
                    else
                    {
                        return null;
                    }

                };




                cmd.CommandText = xSql.RelaceParamValue();

                da.SelectCommand = cmd;

                da.Fill(netTable);


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

            }

            for (int i = schema.Fields.Count - 1; i >= 0; i--)
            {
                FieldSchema fld = schema.Fields[i];
                FieldSchema newFld = tmpfs.FindItem(fld.Id);
                if (newFld == null)
                    schema.Fields.Remove(fld);
            }

            xdb.Close();

        }

        private static void BuildFields(XTableSchema schema)
        {
            string connectionName = schema.ConnectionName;
            string selectSql = schema.SelectCommand;

            XDataBase xdb = XDatabaseFactory.Instance.GetDataBase(connectionName);


            XSql xSql = new XSql(selectSql);

            string tableName = xSql.GetFrom();
            //  schema.Fields.Clear();

            SchemaList<FieldSchema> tmpfs = new SchemaList<FieldSchema>();

            DbConnection con = xdb.GetConnection();
            DataTable netTable = new DataTable();

            DbDataAdapter da = xdb.GetAdapter();

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
                string keyfield = xdb.GetPrimaryKey(tableName);
                if (!string.IsNullOrEmpty(keyfield))
                    schema.KeyField = keyfield;

                List<FieldDef> fieldDefs = xdb.GetFieldInfos(tableName);

                for (int i = 0; i < fieldDefs.Count; i++)
                {
                    FieldDef fieldDef = fieldDefs[i];
                    FieldSchema field = schema.Fields.FindItem(fieldDef.Name);
                    
                    //                    field.Id =;
                    if (field != null)
                    {
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

            xdb.Close();

        }

        public static void BuildSchemaCommand(XTableSchema schema)
        {

            XSqlBuilder cmdBuild = new XSqlBuilder(schema);
            try
            {
                //                schema.UpdateCommand = cmdBuild.GetUpdateCommand(true).CommandText;
                schema.UpdateCommand = cmdBuild.GetUpdateCommand();
            }
            catch
            {
                schema.UpdateCommand = "";
            }

            try
            {
                //schema.InsertCommand = cmdBuild.GetInsertCommand(true).CommandText;
                schema.InsertCommand = cmdBuild.GetInsertCommand();
            }
            catch (Exception)
            {

                schema.InsertCommand = "";
            }

            try
            {
                //schema.DeleteCommand = cmdBuild.GetDeleteCommand(true).CommandText;
                schema.DeleteCommand = cmdBuild.GetDeleteCommand();
            }
            catch (Exception)
            {
                schema.DeleteCommand = "";
            }
        }

        public static XTableSchema BuildSrcTableSchema(string fullTableName)
        {
            string[] paths = fullTableName.Split(ContainerConst.NamePathChar);
            string connName = paths[0];
            string tbName = paths[1];
            string sql = "Select * From [" + tbName + "]";
            XTableSchema ret = CreateTableSchema(tbName, connName, sql);
            XTableSchemaContainer.Instance().AddItem(fullTableName, ret);
            return ret;
        }

        public static bool isSrcTable(string tableId)
        {
            //  throw new NotImplementedException();

            XDatabaseFactory dbf = XDatabaseFactory.Instance;

            string[] paths = tableId.Split(ContainerConst.NamePathChar);
            if (paths.Length != 2) return false;
            string connName = paths[0];
            string tbName = paths[1];
            if (!dbf.GetConnectionNames().Contains(connName)) return false;
            if (!dbf.GetTableNames(connName).Contains(tbName)) return false;
            return true;
        }


        /// <summary>
        /// //用物理数据库中的字段及表信息，重新更新XTableSchema信息
        /// </summary>
        /// <param name="tableId"></param>
        /// <param name="schema"></param>
        public static void RefreshSchemaFields(string tableId, XTableSchema schema)
        {
            BuildFields(schema);
            XTableSchemaContainer.Instance().UpdateItem(tableId, schema);
        }
    }
}
