using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using xbase.data.easyui;
using xbase.Exceptions;
using xbase.local;

namespace xbase.data
{
    public static class DataSourceComm
    {
        public static SubTableSchema getSubTableSchema(string subDsName, DataSourceSchema dsSchema)
        {
            foreach (SubTableSchema ss in dsSchema.SubTables)
            {
                if (ss.Name.Equals(subDsName))
                    return ss;

            }
            throw new XException(string.Format( Lang.SubTableNoDefine,subDsName,dsSchema.Id));
        }

        public static ListDataRow readRow(DataTable tb, DataSourceSchema dsSchema, DataRow dRow)
        {

            List<DataListColumn> fColumns = EUGridUtils.getColumns(dsSchema.Fields);

            ListDataRow row = new ListDataRow();

            foreach (DataListColumn dcol in fColumns)

            //                foreach (FieldSchema fldSchema in _schema.Fields)
            //foreach (DataColumn col in tb.Columns)
            {
                string fName = dcol.field;
                if (fName.StartsWith(XSqlBuilder.OLD_VERSION_PIX))
                    fName = fName.Remove(0, XSqlBuilder.OLD_VERSION_PIX.Length);

                if (!dsSchema.Fields.ContainsId(fName))
                {
                    row.Add("ck", "false");
                    continue;
                }
                FieldSchema fldSchema = dsSchema.Fields.GetItem(fName);

                if (fldSchema.DataType == DbType.Binary) continue;

                DataColumn col = tb.Columns[fName];


                //   if (!string.IsNullOrEmpty(fldSchema.Alias))
                //       fName = fldSchema.Alias;
                //if (col == null)
                //    throw new Exception(string.Format(Lang.FieldNotFind, fName));
                string value = "";
                if (col != null)
                    value = GetCellString(fldSchema, col, dRow);
                row.Add(dcol.field, value);

                //      if (tb.PrimaryKey.Contains(col))
                //      {
                // row.Add(dcol, value);
                //row.Pk.Add(fName, value);
                //     }

            }
            return row;
        }

        private static string GetCellString(FieldSchema fldSchema, DataColumn col, DataRow row)
        {
            object v = row[col];
            if (v is DBNull)
                return null;

            if (v is DateTime)
            {
                if (string.IsNullOrEmpty(fldSchema.EditFormat))
                    return ((DateTime)v).ToString(Lang.DateTimeFormat);
                else
                    return ((DateTime)v).ToString(fldSchema.EditFormat);
            } if (v is string)
                return TextType.getTextConent(v as string);

            return v.ToString();
        }

        public static bool isNewRow(ListDataRow row)
        {
            foreach (string fld in row.Keys)
            {
                if (fld.StartsWith(XSqlBuilder.OLD_VERSION_PIX))
                    if (!string.IsNullOrEmpty(row[fld]))
                        return false;
            }
            return true;
        }


    }
}
