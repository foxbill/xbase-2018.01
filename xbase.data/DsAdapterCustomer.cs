using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using xbase.local;
using xbase.data.db;
using xbase.Exceptions;
using System.Transactions;

namespace xbase.data
{
    delegate object ValueRequest(string valName);
    internal class DsAdapterCustomer : DsAdapter
    {
        private DataSourceSchema _schema;
        private TransactionScope _ts;
        private DatabaseAdmin dbAdmin;
        private ValueRequest _onRequestValue;

        internal ValueRequest onRequestVar
        {
            get { return _onRequestValue; }
            set { _onRequestValue = value; }
        }

        public DsAdapterCustomer(DataSourceSchema schema)
        {
            this._schema = schema;
            dbAdmin = DatabaseAdmin.getInstance(schema.ConnectionName);
        }




        public DataSourceSchema schema
        {
            get { return _schema; }
        }

        private static string listToString(List<string> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in list)
            {
                sb.Append(s);
                sb.Append(',');
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        internal List<DataListColumn> getTableColumns(string tableName)
        {
            List<DataListColumn> ret = new List<DataListColumn>();
            TableDef tableDef = dbAdmin.getTableDef(tableName);
            int i = 0;
            foreach (FieldDef field in tableDef.FieldDefs)
            {
                i++;
                if (i > DataSourceConst.MaxCol) break;
                DataListColumn col = new DataListColumn();
                col.field = field.Name;

                col.title = field.Alias;
                if (string.IsNullOrEmpty(col.title))
                    col.title = field.Name;

                col.resizable = true;
                col.sortable = true;
                ret.Add(col);
            }
            return ret;
        }



        internal static List<FilterRule> getTableFilters(string name)
        {
            string[] ary = name.Split(',');
            string conn = "";
            string tbName = name;
            if (ary.Length > 1)
            {
                conn = ary[0];
                tbName = ary[1];
            }

            DatabaseAdmin dba = DatabaseAdmin.getInstance(conn);
            TableDef tbDef = dba.getTableDef(tbName);
            List<FilterRule> ret = new List<FilterRule>();

            foreach (FieldDef fldDef in tbDef.FieldDefs)
            {
                FilterRule fr = new FilterRule();
                fr.field = fldDef.Name;
                ret.Add(fr);
            }

            return ret;

        }


        internal DataSet getSourceDataSet(string where, string orderBy, PaginationInfo pi)
        {

            string connName = _schema.ConnectionName;
            string tableName = _schema.TableName;

            Database db = dbAdmin.Database;
            List<string> pks = dbAdmin.GetPrimaryKeys(tableName);
            DbCommand cmd = null;

            if (!dbAdmin.ExistsSp(DataSourceConst.PaginationSpName))
                dbAdmin.modifyProc(DataSourceConst.PaginationSpName, DataSourceConst.PaginationSpText);

            if (pks.Count() == 1)
            {
                pi.isStoreProcessPagination = true;
                cmd = db.GetStoredProcCommand(DataSourceConst.PaginationSpName);
                cmd.CommandType = CommandType.StoredProcedure;
                db.AddInParameter(cmd, "@TableName", DbType.String, tableName);
                db.AddInParameter(cmd, "@PrimaryKey", DbType.String, pks[0]);
                db.AddInParameter(cmd, "@PageNo", DbType.Int32, pi.page);
                db.AddInParameter(cmd, "@PageSize", DbType.Int32, pi.pageSize);
                db.AddInParameter(cmd, "@Where", DbType.String, where);
                db.AddOutParameter(cmd, "@PageCount", DbType.Int32, sizeof(Int32));
                db.AddOutParameter(cmd, "@Total", DbType.Int32, sizeof(Int32));
            }
            else
            {
                pi.isStoreProcessPagination = false;

                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("Select * From ");
                sbSql.Append(tableName);
                if (!string.IsNullOrEmpty(where))
                {
                    sbSql.Append(" Where ");
                    sbSql.Append(where);
                }
                if (!string.IsNullOrEmpty(orderBy))
                {
                    sbSql.Append(" Order By ");
                    sbSql.Append(orderBy);
                }
                cmd = db.GetSqlStringCommand(sbSql.ToString());
            }

            DataSet ds = db.ExecuteDataSet(cmd);
            TableDef tableDef = dbAdmin.getTableDef(tableName);
            DataTable table = ds.Tables[0];

            if (pks.Count > 0)
            {
                DataColumn[] primaryKey = new DataColumn[2];
                for (int i = 0; i < pks.Count; i++)
                {
                    primaryKey[i] = table.Columns[pks[i]];
                }
                table.PrimaryKey = primaryKey;
            }

            foreach (FieldDef fieldDef in tableDef.FieldDefs)
            {
                DataColumn col = table.Columns[fieldDef.Name];

                if (!string.IsNullOrEmpty(fieldDef.Alias))
                    col.Caption = fieldDef.Alias;

                if (fieldDef.IsIdentity)
                {
                    col.AutoIncrement = true;
                    col.ReadOnly = true;
                }

            }

            if (pi.isStoreProcessPagination)
            {
                pi.pageCount = (int)cmd.Parameters["@PageCount"].Value;
                pi.total = (int)cmd.Parameters["@Total"].Value;
            }
            else
            {
                pi.total = table.Rows.Count;
                pi.pageCount = pi.total / pi.pageSize;
                if (pi.total % pi.pageSize != 0)
                    pi.pageCount++;
            }
            return ds;
        }


        private void applyTableDef(DataSet dataSet)
        {

            foreach (DataTable table in dataSet.Tables)
            {
                string tableName = table.TableName;

                List<string> pks = dbAdmin.GetPrimaryKeys(tableName);


                var tbName = tableName.Trim('[', ']');
                TableDef tbDef = dbAdmin.getTableDef(tbName);
                if (tbDef == null)
                    break;
                List<FieldDef> fieldDefs = tbDef.FieldDefs;

                for (int i = 0; i < fieldDefs.Count; i++)
                {
                    FieldDef fieldDef = fieldDefs[i];
                    string colName = fieldDef.Name;
                    if (table.Columns.Contains(colName))
                    {
                        DataColumn col = table.Columns[colName];
                        col.Caption = colName;
                        if (!string.IsNullOrEmpty(fieldDef.Title))
                            col.Caption = fieldDef.Title;
                        //if (string.IsNullOrEmpty(fieldDef.DefaultValue))
                        //    col.DefaultValue = fieldDef.DefaultValue;
                        col.AutoIncrement = fieldDef.IsIdentity;
                        //if (!string.IsNullOrEmpty(fieldDef.Description))
                        col.ExtendedProperties[DataSourceConst.ExProDescription] = fieldDef.Description;
                        DbType dbType;
                        try
                        {
                            dbType = dbAdmin.getDbType(fieldDef.Type);
                        }
                        catch
                        {
                            throw new Exception(string.Format(Lang.unknowDbType, tableName, colName));
                        }

                        if (!col.ExtendedProperties.ContainsKey(DataSourceConst.ExProDbType))
                            col.ExtendedProperties.Add(DataSourceConst.ExProDbType, dbType);
                        else
                            col.ExtendedProperties[DataSourceConst.ExProDbType] = dbType;
                    }
                }

                if (pks != null && pks.Count > 0)
                {
                    DataColumn[] cols = new DataColumn[pks.Count];
                    for (int i = 0; i < pks.Count; i++)
                    {
                        string colName = pks[i];
                        if (table.Columns.Contains(colName))
                            cols[i] = table.Columns[colName];
                    }
                    table.PrimaryKey = cols;
                }

            }
        }


        private DbCommand getSelectCommand(int pageSize, int pageNo, string fields, Dictionary<string, string> queryParams, string where, string orderBy, string groupBy)
        {
            string connectionName = _schema.ConnectionName;
            string selectSql;
            //  tableNames = new List<string>();

            DbCommand cmd = null;

            switch (_schema.SelectCommand.CommandType)
            {
                case CommandType.TableDirect:

                    if (pageNo > 1 || pageSize > 1)
                        cmd = getPagingSpCommand(_schema.SelectCommand.CommandText, pageSize, pageNo, fields, where, orderBy, groupBy);

                    if (cmd == null)
                    {
                        selectSql = XSqlBuilder.BuildTableSql(_schema.SelectCommand.CommandText, pageSize, pageNo, fields, where, orderBy, groupBy);
                        cmd = dbAdmin.getSqlStringCommand(selectSql);
                    }
                    else
                    {
                        _schema.IsPagingByParams = true;
                    }
                    break;
                case CommandType.Text:
                    selectSql = _schema.SelectCommand.CommandText;
                    SqlParse xSql = new SqlParse(selectSql);
                    cmd = dbAdmin.getSqlStringCommand(selectSql);
                    //                    paramNames = xSql.GetParamNames();
                    break;
                case CommandType.StoredProcedure:
                    selectSql = _schema.SelectCommand.CommandText;
                    cmd = dbAdmin.GetStoredProcCommandWithSourceColumns(selectSql, fields.Split(','));
                    break;
            }


            if (_schema.SelectCommand != null && _schema.SelectCommand.QueryParams != null)
            {
                foreach (ParameterSchema ps in _schema.SelectCommand.QueryParams)
                {
                    if (string.IsNullOrEmpty(ps.Id))
                        continue;
                    string name = ps.Id;

                    object value = ps.DefaultValue;

                    if (queryParams != null && queryParams.ContainsKey(name))
                        value = queryParams[name];

                    //if (name.Equals(DataSourceConst.PagingPageNo, StringComparison.OrdinalIgnoreCase))
                    //    value = pageNo;
                    //if (name.Equals(DataSourceConst.PagingPageSize, StringComparison.OrdinalIgnoreCase))
                    //    value = pageSize;

                    if (cmd.Parameters.Contains(name))
                        dbAdmin.Database.SetParameterValue(cmd, name, value);
                    else if (ps.Direction == ParameterDirection.Input)
                        dbAdmin.addInParameter(cmd, name, ps.DataType, value);
                    else
                        dbAdmin.AddOutParameter(cmd, name, ps.DataType, ps.DataSize);

                }
            }
            return cmd;
        }

        /// <summary>
        /// 获取源表的分页存储过程查询命令，如果获取失败则返回Null
        /// </summary>
        /// <param name="dba"></param>
        /// <param name="tableName"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="fields"></param>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="groupBy"></param>
        /// <returns></returns>
        private DbCommand getPagingSpCommand(string tableName, int pageSize, int pageNo, string fields, string where, string orderBy, string groupBy)
        {
            List<string> pks = dbAdmin.GetPrimaryKeys(tableName);
            if (pks == null || pks.Count != 1)
                return null;

            DbCommand cmd = null;

            if (!dbAdmin.ExistsSp(DataSourceConst.PaginationSpName))
                dbAdmin.modifyProc(DataSourceConst.PaginationSpName, DataSourceConst.PaginationSpText);

            cmd = dbAdmin.GetStoredProcCommand(DataSourceConst.PaginationSpName);
            cmd.CommandType = CommandType.StoredProcedure;
            dbAdmin.addInParameter(cmd, "@TableName", DbType.String, tableName);
            dbAdmin.addInParameter(cmd, "@PrimaryKey", DbType.String, pks[0]);
            dbAdmin.addInParameter(cmd, "@PageNo", DbType.Int32, pageNo);
            dbAdmin.addInParameter(cmd, "@PageSize", DbType.Int32, pageSize);
            dbAdmin.addInParameter(cmd, "@Where", DbType.String, where);
            dbAdmin.AddOutParameter(cmd, "@PageCount", DbType.Int32, sizeof(int));
            dbAdmin.AddOutParameter(cmd, "@Total", DbType.Int32, sizeof(int));
            return cmd;
        }

        public static string getFields(DataSourceSchema schema)
        {
            if (schema.Fields == null || schema.Fields.Count < 1)
                return "*";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < schema.Fields.Count; i++)
            {
                FieldSchema fs = schema.Fields[i];
                if (i > 0) sb.Append(',');
                sb.Append(fs.Id);
            }
            return sb.ToString();
        }



        public DataSet getDataSet(Dictionary<string, string> _queryParams, string where, string orderBy, string groupBy, PaginationInfo pi)
        {
            //            DatabaseAdmin dbAdmin = DatabaseAdmin.getInstance(_schema.ConnectionName);
            string fields = getFields(_schema);

            DbCommand cmd = getSelectCommand(pi.pageSize, pi.page, fields, _queryParams, where, orderBy, groupBy);

            DataSet ret = dbAdmin.Database.ExecuteDataSet(cmd);

            if (cmd.Parameters.Contains("@Total") && cmd.Parameters.Contains("@PageCount"))
            {
                pi.pageCount = (int)cmd.Parameters["@PageCount"].Value;
                pi.total = (int)cmd.Parameters["@Total"].Value;
                pi.isStoreProcessPagination = true;
            }
            else
            {
                pi.isStoreProcessPagination = false;
                pi.total = ret.Tables[0].Rows.Count;
                pi.pageCount = 1;
                if (pi.pageSize > 0 && pi.total > 0)
                {
                    pi.pageCount = pi.total / pi.pageSize;
                    if (pi.total % pi.pageSize != 0)
                        pi.pageCount++;
                }
            }
            return ret;
        }

        public DataSet getDataSet()
        {
            //            DatabaseAdmin dbAdmin = DatabaseAdmin.getInstance(_schema.ConnectionName);
            string fields = getFields(_schema);
            DbCommand cmd = getSelectCommand(1, 1, null, null, null, null, null);
            DataSet ds = dbAdmin.Database.ExecuteDataSet(cmd);
            //  applyTableDef(ds);
            applySchema(ds);
            return ds;
        }

        private void applySchema(DataSet ds)
        {
            DataTable dt = ds.Tables[0];
            for (int i = 0; i < _schema.Fields.Count; i++)
            {
                FieldSchema fld = _schema.Fields[i];
                string fieldName = fld.Id;
                DataColumn col = dt.Columns[fieldName];
                if (col != null)
                {
                    col.SetOrdinal(i);
                }
                else if (!string.IsNullOrEmpty(fld.Expression))
                {
                    col = dt.Columns.Add(fieldName);
                    col.SetOrdinal(i);
                }
                else
                {
                    throw new Exception(string.Format(Lang.SchemaColNotFieldAndNotExpression, fieldName));
                }

            }
        }

        private bool isNewRow(ListDataRow row)
        {
            if (schema.PrimaryKeys == null || schema.PrimaryKeys.Count <= 0)
                throw new XException(Lang.NoMainKey);
            foreach (string pk in _schema.PrimaryKeys)
            {
                string kf = XSqlBuilder.OLD_VERSION_PIX + pk;
                if (!row.ContainsKey(kf) || string.IsNullOrEmpty(row[kf]))
                    return true;
            }
            return false;
        }



        private void setCommandParamValue(DbCommand cmd, Dictionary<string, string> paramValues, ListDataRow row)
        {

            foreach (DbParameter dbp in cmd.Parameters)
            {
                string pName = dbp.ParameterName;
                string fName = pName.Remove(0, 1);
                object value = null;

                if (row != null && row.ContainsKey(fName))
                    if (!string.IsNullOrEmpty(row[fName]))
                        value = row[fName];

                if (paramValues != null && paramValues.ContainsKey(pName))
                    if (!string.IsNullOrEmpty(paramValues[pName]))
                        value = paramValues[pName];

                dbAdmin.Database.SetParameterValue(cmd, pName, value);

            }

        }

        /// <summary>
        /// 根据CommandSchema创建命令，并将Shema中的参数设置到命令
        /// </summary>
        /// <param name="cmdSchema">命令配置</param>
        /// <returns></returns>
        public DbCommand getCommand(CommandSchema cmdSchema)
        {
            if (cmdSchema == null || string.IsNullOrEmpty(cmdSchema.CommandText))
                throw new XException(Lang.NoUpLoadCommand);

            DbCommand cmd = dbAdmin.getSqlCommand(cmdSchema.CommandText, cmdSchema.CommandType);

            SqlParse sqlp = new SqlParse(cmdSchema.CommandText);
            string[] parameters = sqlp.GetParamNames();

            foreach (string pName in parameters)
            {
                string fName = pName.Remove(0, 1);
                string val = null;
                DbType pType = DbType.String;
                ParameterDirection pDir = ParameterDirection.Input;

                ParameterSchema ps = cmdSchema.QueryParams == null ? null : cmdSchema.QueryParams.FindItem(pName);
                FieldSchema flds = schema.Fields == null ? null : schema.Fields.FindItem(fName);

                if (pName.StartsWith(XSqlBuilder.OLD_VERSION_PIX))
                    flds = schema.Fields.FindItem(fName.Remove(0, XSqlBuilder.OLD_VERSION_PIX.Length));

                //获取字典定义中的参数类型，参数方向默认为input
                if (flds != null)
                    pType = flds.DataType;

                //获取参数定义中的参数类型,及参数方向;
                if (ps != null)
                {
                    pType = ps.DataType;
                    val = ps.DefaultValue;
                    pDir = ps.Direction;
                }
                //if (row.ContainsKey(fName))
                //    val = row[fName];

                //if (realParams.ContainsKey(pName))
                //    val = realParams[pName];


                switch (pDir)
                {
                    case ParameterDirection.Input:
                        dbAdmin.addInParameter(cmd, pName, pType, val);
                        break;
                    case ParameterDirection.InputOutput:
                        break;
                    case ParameterDirection.Output:
                        dbAdmin.AddOutParameter(cmd, pName, pType, int.MaxValue);
                        break;
                }

            }

            return cmd;
        }

        private void beginTrans()
        {
            _ts = new TransactionScope();
        }

        private void refreshRow(DataSet ds, ListDataRow row)
        {
            if (ds.Tables.Count < 1) return;
            DataTable tb = ds.Tables[0];
            if (tb.Rows.Count < 1) return;

            foreach (DataColumn col in tb.Columns)
            {

                string field = col.ColumnName;
                string fName = XSqlBuilder.OLD_VERSION_PIX + field;

                if (row.ContainsKey(field))
                    row[field] = tb.Rows[0][field].ToString();

                if (row.ContainsKey(fName))
                    row[fName] = tb.Rows[0][field].ToString();
            }

        }

        public void executeCommandSchema(CommandSchema commandSchema, ListDataRow row, Dictionary<string, string> realParams, bool refresh = false)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                DbCommand cmd = getCommand(commandSchema);
                setCommandParamValue(cmd, realParams, row);
                if (refresh)
                {
                    DataSet ds = dbAdmin.executeDateSet(cmd);
                    refreshRow(ds, row);
                }
                else
                    dbAdmin.execNonQuery(cmd);
                ts.Complete();
            }

        }

        public void executeCommandSchema(CommandSchema commandSchema, List<ListDataRow> rows, Dictionary<string, string> realParams, bool refresh = false)
        {
            using (TransactionScope ts = new TransactionScope())
            {

                DbCommand cmd = getCommand(commandSchema);
                foreach (ListDataRow row in rows)
                {
                    setCommandParamValue(cmd, realParams, row);
                    if (refresh)
                    {
                        DataSet ds = dbAdmin.executeDateSet(cmd);
                        refreshRow(ds, row);
                    }
                    else
                        dbAdmin.execNonQuery(cmd);
                }
                ts.Complete();
            }
        }


        public void update(ListDataRow row, Dictionary<string, string> realParams)
        {
            executeCommandSchema(schema.UpdateCommand, row, realParams, true);
        }

        public void update(List<ListDataRow> rows, Dictionary<string, string> realParams)
        {
            executeCommandSchema(schema.UpdateCommand, rows, realParams);
        }

        public void delete(ListDataRow row, Dictionary<string, string> realParams)
        {
            executeCommandSchema(schema.DeleteCommand, row, realParams);
        }

        public void delete(Dictionary<string, string> realParams)
        {
            executeCommandSchema(schema.DeleteCommand, new ListDataRow(), realParams);
        }

        public void delete(List<ListDataRow> rows, Dictionary<string, string> realParams)
        {
            executeCommandSchema(schema.DeleteCommand, rows, realParams);
        }

        public void insert(ListDataRow row, Dictionary<string, string> realParams)
        {
            executeCommandSchema(schema.InsertCommand, row, realParams, true);
        }

        public void insert(List<ListDataRow> rows, Dictionary<string, string> realParams)
        {
            executeCommandSchema(schema.InsertCommand, rows, realParams, true);
        }

        public void update(List<ListDataRow> insertRows, List<ListDataRow> updateRows, List<ListDataRow> deleteRows, Dictionary<string, string> realParams)
        {
            if (insertRows != null)
                executeCommandSchema(schema.InsertCommand, insertRows, realParams);
            if (updateRows != null)
                executeCommandSchema(schema.UpdateCommand, updateRows, realParams);
            if (deleteRows != null)
                executeCommandSchema(schema.DeleteCommand, deleteRows, realParams);
        }
    }
}
