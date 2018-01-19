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

namespace xbase.data
{
    internal static class DataSourceTools
    {
        public static DbCommand getSchemaCommand(DatabaseAdmin dba, CommandSchema schemaCommand)
        {
            Database db = dba.Database;
            if (schemaCommand == null || string.IsNullOrEmpty(schemaCommand.CommandText))
                throw new Exception(Lang.NotAssignedSQL);

            CommandType cmdType = (CommandType)schemaCommand.CommandType;
            DbCommand cmd = null;
            switch (cmdType)
            {
                case CommandType.StoredProcedure:
                    {
                        cmd = db.GetStoredProcCommand(schemaCommand.CommandText);
                        break;
                    }
                default:
                    {
                        cmd = db.GetSqlStringCommand(schemaCommand.CommandText);
                        break;
                    }

            }
            return cmd;
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

        internal static List<DataListColumn> getTableColumns(DatabaseAdmin dba, string tableName)
        {
            List<DataListColumn> ret = new List<DataListColumn>();
            TableDef tableDef = dba.getTableDef(tableName);
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


        internal static DataSet getSourceDataSet(string dsName, string where, string orderBy, PaginationInfo pi)
        {
            if (string.IsNullOrEmpty(dsName))
                throw new XException(Lang.DataSourceNameIsNull);
            string connName = "";
            string tableName = dsName;
            string[] namePath = dsName.Split('.');
            if (namePath.Length > 1)
            {
                connName = namePath[0];
                tableName = namePath[1];
            }

            DatabaseAdmin dba = DatabaseAdmin.getInstance(connName);
            Database db = dba.Database;
            List<string> pks = dba.GetPrimaryKeys(tableName);
            DbCommand cmd = null;
            if (!dba.ExistsSp(DataSourceConst.PaginationSpName))
                dba.modifyProc(DataSourceConst.PaginationSpName, DataSourceConst.PaginationSpText);

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
            TableDef tableDef = dba.getTableDef(tableName);
            DataTable table = ds.Tables[0];

            foreach (FieldDef fieldDef in tableDef.FieldDefs)
            {
                if (string.IsNullOrEmpty(fieldDef.Alias))
                    continue;
                table.Columns[fieldDef.Name].Caption = fieldDef.Alias;
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

        internal static DataSet getSchemaDataSet(DatabaseAdmin dba, DataSourceSchema _schema, Dictionary<string, string> _queryParams, int p, int p_2)
        {
            throw new NotImplementedException();
        }
    }
}
