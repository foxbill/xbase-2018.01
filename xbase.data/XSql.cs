using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase;
using System.Text.RegularExpressions;
using xbase.Exceptions;
using xbase.data.db;
using Newtonsoft.Json;
using System.Data;


namespace xbase.data
{
    public class E_ParamGetterNotAssigned : XException { public E_ParamGetterNotAssigned(string msg) : base(msg) { } }
    public class E_XSQL_NotFindFrom : XException
    {
        public E_XSQL_NotFindFrom()
            : base("在语句中不能发现Form部分")
        {
        }
    }

    public delegate string ParamGetter(string paramName);
    /// <summary>
    /// 获取sql语句中的参数
    /// </summary>
    public class SqlParse
    {
        private const string REG_FROM = @"(from)(\s*.[,\s]*)+((\s+where\s+)|(\s+on\s+)|(\s+having\s+)|(\s+group\s+by\s+)|(\s+order\s+by\s+)|$)";
        private string sql;
        private ParamGetter paramGetter;

        public SqlParse(string sql)
        {
            SetSql(sql);
        }

        public SqlParse(string sql, ParamGetter paramGetter)
        {
            SetSql(sql);
            this.paramGetter = paramGetter;
        }

        public string[] GetTableNames()
        {
            string from = GetFrom();
            if (string.IsNullOrEmpty(from))
                return null;
            return from.Split(' ', ',');
        }

        private void SetSql(string sql)
        {
            this.sql = sql;
        }

        public ParamGetter ParamGetter
        {
            get { return paramGetter; }
            set { paramGetter = value; }
        }


        /// <summary>
        /// 获取sql语句中的参数
        /// </summary>
        /// <returns>参数数组</returns>
        public string[] GetParamNames()
        {


            List<string> sqlParams = new List<string>();
            Regex regex = new Regex(@"@[@a-zA-Z0-9_.]+");

            MatchCollection ms = regex.Matches(sql);

            foreach (Match match in ms)
            {
                foreach (Capture c in match.Captures)
                {
                    string varName = c.Value;
                    if (sqlParams.IndexOf(varName) < 0)
                        sqlParams.Add(varName);
                }
            }

            return sqlParams.ToArray();
        }


        public string RelaceParamValue()
        {
            string newSql = sql;

            string[] paraNames = GetParamNames();

            if (paramGetter == null)
                throw new E_ParamGetterNotAssigned("not paramGetter when GetSql");

            foreach (string pName in paraNames)
            {
                string value = paramGetter(pName);
                if (value != null)
                    newSql = newSql.Replace("@" + pName, value);
            }

            return newSql;
        }

        public string GetFrom()
        {
            this.sql = this.sql.Replace('\n', ' ');
            this.sql = this.sql.Replace('\r', ' ');
            this.sql = this.sql.Replace('\t', ' ');

            string start = " FROM ";
            string[] keywords = { " from ", " where ", " group by ", "order by " };

            int b = sql.IndexOf(start, StringComparison.OrdinalIgnoreCase);
            if (b < 0)
                return null;

            string s = sql.Substring(b + start.Length);
            string[] strs = s.Split(keywords, StringSplitOptions.RemoveEmptyEntries);
            if (strs.Length > 0)
                return strs[0].Trim();

            return null;
        }
    }

    public class E_SQLBuilder_NoSelectCommand : XException
    {
        public E_SQLBuilder_NoSelectCommand() : base("必须先指定Select 命令") { }
    }

    public class E_SQLBuilder_NoPrimaryKey : XException
    {
        public E_SQLBuilder_NoPrimaryKey() : base("Table配置文件，必须先指定唯一主键") { }
    }

    public static class StringExt
    {
        public static string GetMidStr(string srcString, string start, string end)
        {
            int b = srcString.IndexOf(start, StringComparison.OrdinalIgnoreCase) + start.Length;
            int e = srcString.IndexOf(end, StringComparison.OrdinalIgnoreCase);
            return srcString.Substring(b, e - b);
        }
    }

    public class XSqlBuilder
    {
        public const string OLD_VERSION_PIX = XDataConst.Old_Value_Field;
        public const string FIELD_SPLITOR = ";";
        DataSourceSchema schema;


        public XSqlBuilder(DataSourceSchema schema)
        {
            this.schema = schema;
        }

        private string GetFrom()
        {
            if (schema.SelectCommand.CommandType == CommandType.TableDirect)
                return schema.SelectCommand.CommandText;
            else if (schema.SelectCommand.CommandType == CommandType.StoredProcedure)
                throw new E_SQLBuilder_NoSelectCommand();
            else if (string.IsNullOrEmpty(schema.SelectCommand.CommandText))
                throw new E_SQLBuilder_NoSelectCommand();

            SqlParse sql = new SqlParse(schema.SelectCommand.CommandText);
            return sql.GetFrom();
        }

        public string GetUpdateCommand()
        {
            DatabaseAdmin db = DatabaseAdmin.getInstance(schema.ConnectionName);

            List<string> pks = schema.PrimaryKeys;

            if (pks == null || pks.Count < 1)
                throw new E_SQLBuilder_NoPrimaryKey();

            string tableName = GetFrom();
            string update = "UPDATE " + tableName + " SET ";


            foreach (FieldSchema fldsch in schema.Fields)
            {
                if (!fldsch.ReadOnly && !db.isIdentityField(tableName, fldsch.Id) && !db.isRowGuidField(tableName, fldsch.Id))
                    update += " [" + fldsch.Id + "]=@" + fldsch.Id + ",";
            }

            update = update.Remove(update.Length - 1);

            update += " WHERE ";

            foreach (string pk in pks)
            {
                update += " [" + pk + "]=@" + OLD_VERSION_PIX + pk + " And ";
            }

            update = update.Remove(update.LastIndexOf(" And "));

            return update;

        }


        public string GetInsertCommand()
        {
            string tableName = GetFrom();
            string insertCmd = "INSERT INTO " + tableName + " (";

            string values = "";

            DatabaseAdmin db = DatabaseAdmin.getInstance(schema.ConnectionName);

            foreach (FieldSchema fldsch in schema.Fields)
            {
                if (!fldsch.ReadOnly && !db.isIdentityField(tableName, fldsch.Id) && !db.isRowGuidField(tableName, fldsch.Id))
                {
                    insertCmd += " [" + fldsch.Id + "]" + XTableSchemaConst.FieldSplitor;
                    values += "@" + fldsch.Id + XTableSchemaConst.FieldSplitor;
                }
            }


            values = values.Remove(values.LastIndexOf(XTableSchemaConst.FieldSplitor));

            insertCmd = insertCmd.Remove(insertCmd.Length - 1);
            insertCmd += ")";
            insertCmd += " VALUES (" + values + ")";

            if (schema.PrimaryKeys != null && schema.PrimaryKeys.Count > 0)
            {
                List<string> pks = schema.PrimaryKeys;
                string id = "";
                foreach (string fieldName in pks)
                {
                    if (db.isIdentityField(tableName, fieldName))
                    {
                        id = fieldName;
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(id))
                {
                    insertCmd += ";SELECT * FROM " + tableName + " WHERE [" + id + "]=SCOPE_IDENTITY()";
                }
            }
            // db.Close();
            return insertCmd;
        }

        public string GetDeleteCommand()
        {

            string deleteCmd = "DELETE FROM " + GetFrom() + " WHERE ";

            List<string> pks = schema.PrimaryKeys;

            if (pks == null || pks.Count < 1)
                throw new E_SQLBuilder_NoPrimaryKey();

            foreach (string pk in pks)
            {
                deleteCmd += " [" + pk + "]=@" + OLD_VERSION_PIX + pk + " And ";
            }

            deleteCmd = deleteCmd.Remove(deleteCmd.LastIndexOf(" And "));

            return deleteCmd;
        }


        public static string BuildTableSql(string tableName, int pageSize, int pageNo, string fields, string where, string orderBy, string groupBy)
        {
            if (string.IsNullOrEmpty(fields))
                fields = "*";
            StringBuilder sb = new StringBuilder("Select ");
            //if (pageSize > 0)
            //{
            //    sb.Append(" top ");
            //    sb.Append(pageSize);
            //    sb.Append(" ");  
            //}

            sb.Append(fields);
            sb.Append(" From ");
            sb.Append(tableName);
            if (!string.IsNullOrEmpty(where))
            {
                sb.Append(" Where ");
                sb.Append(where);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                sb.Append(" Order By ");
                sb.Append(orderBy);
            }
            if (!string.IsNullOrEmpty(groupBy))
            {
                sb.Append(" Group By ");
                sb.Append(groupBy);
            }
            string ret = sb.ToString();
            return ret;
        }
    }



}
