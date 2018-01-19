using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using xbase.local;
using xbase.data.db;

namespace xbase.data.easyui
{
    /// <summary>
    /// 
    /// </summary>
    public static class EUGridUtils
    {
        private static string editorForType(DbType dbType)
        {
            //text,textarea,checkbox,numberbox,validatebox,datebox,combobox,combotree.

            switch (dbType)
            {
                case DbType.Binary:
                    return null;
                case DbType.DateTime:
                case DbType.DateTime2:
                case DbType.Date:
                    return "datetimebox";
                case DbType.Boolean:
                    return "checkbox";
                case DbType.Currency:
                case DbType.Decimal:
                case DbType.Double:
                case DbType.Int16:
                case DbType.UInt32:
                case DbType.UInt64:
                case DbType.VarNumeric:
                    return "numberbox";
                default:
                    return "text";
            }
        }

        public static List<DataListColumn> getColumns(DataTable tb)
        {
            List<DataListColumn> ret = new List<DataListColumn>();

            foreach (DataColumn col in tb.Columns)
            {
                if (col.DataType.IsArray) continue;
                DataListColumn listCol = new DataListColumn();
                string fldName = col.ColumnName;
                if (col.ExtendedProperties.ContainsKey("Alias"))
                    fldName = col.ExtendedProperties["Alias"].ToString();
                listCol.field = fldName;
                listCol.title = col.Caption;
                listCol.resizable = true;
                listCol.hidden = false;
                listCol.editor = "text";
                if (col.ReadOnly)
                {
                    listCol.editor = null;
                }

                ret.Add(listCol);
            }

            foreach (DataColumn col in tb.PrimaryKey)
            {
                DataListColumn listCol = new DataListColumn();
                string fldName = col.ColumnName;
                //                if (col.ExtendedProperties.ContainsKey("Alias"))
                //                    fldName = col.ExtendedProperties["Alias"].ToString();
                listCol.field = XSqlBuilder.OLD_VERSION_PIX + fldName;
                listCol.title = col.Caption;
                listCol.hidden = true;
                ret.Add(listCol);
                // row.Pk.Add(fldName, value);
            }
            return ret;
        }


        public static List<DataListColumn> getColumns(List<FieldSchema> fieldDefs)
        {

            List<DataListColumn> ret = new List<DataListColumn>();
            DataListColumn ckCol = new DataListColumn();
            ckCol.checkbox = true;
            ckCol.field = "ck";
            ret.Add(ckCol);
            foreach (FieldSchema fldSchema in fieldDefs)
            {
                if (fldSchema.DataType == DbType.Binary) continue;
                DataListColumn listCol = new DataListColumn();
                string fldName = fldSchema.Id;
                if (!string.IsNullOrEmpty(fldSchema.Alias))
                {
                    fldName = fldSchema.Alias;
                }
                listCol.field = fldName;
                listCol.title = fldSchema.Title;// string.IsNullOrEmpty(fldSchema.Title) ? fldName : fldSchema.Title;
                if (string.IsNullOrEmpty(listCol.title))
                    listCol.title = fldName;

                listCol.width = fldSchema.DisplayWidth < 50 ? 50 : fldSchema.DisplayWidth;

                listCol.resizable = true;
                listCol.hidden = !fldSchema.Visable;

                if (!string.IsNullOrEmpty(fldSchema.Editor))
                    listCol.editor = fldSchema.Editor;
                else
                    listCol.editor = editorForType(fldSchema.DataType);

                if (fldSchema.ReadOnly || fldSchema.IsAutoInc)
                {
                    listCol.editor = null;
                }

                ret.Add(listCol);

                if (fldSchema.IsKey)
                {
                    listCol = new DataListColumn();
                    listCol.field = XSqlBuilder.OLD_VERSION_PIX + fldName;
                    listCol.title = listCol.field;
                    listCol.hidden = true;
                    ret.Add(listCol);
                }
            }


            return ret;

        }

        public static List<FilterInput> getFilterInputs(List<FieldSchema> fieldDefs)
        {
            List<FilterInput> ret = new List<FilterInput>();

            foreach (FieldSchema col in fieldDefs)
            {
                if (col.DataType == DbType.Binary) continue;

                //if (col.DataType.Equals(typeof(System.String))) continue;

                FilterInput fi = new FilterInput();
                fi.field = col.Id;
                fi.options = new FilterOption();
                fi.op = FilterInput.BigTextOP;
                ret.Add(fi);

            }
            return ret;
        }


        public static EasyUiGridData getGrid(string title, List<FieldSchema> fieldDefs)
        {
            EasyUiGridData ret = new EasyUiGridData();
            ret.columns.Add(getColumns(fieldDefs));
            //            ret.filterRules = getFilterRules();
            ret.filterInputs = getFilterInputs(fieldDefs);
            ret.pagination = true;
            ret.rownumbers = true;
            ret.singleSelect = true;
            ret.checkOnSelect = false;
            ret.selectOnCheck = false;
            ret.pagePosition = "bottom";
            ret.pageSize = DataSourceConst.DefPageSize;
            //    ret.pageNumber = _pagination.pageCount;
            ret.pageList = DataSourceConst.PageSizeList;
            ret.showHeader = true;
            ret.showFooter = true;
            ret.title = title;
            ret.striped = true;
            ret.loadMsg = Lang.Loading;
            ret.multiSort = true;
            //     ret.data = data(_pagination.page, _pagination.pageSize);
            return ret;
        }

        public static EasyUiGridData getGrid(string connName, string title, List<FieldDef> fields)
        {
            EasyUiGridData ret = new EasyUiGridData();
            ret.columns.Add(getColumns(fields));
            //            ret.filterRules = getFilterRules();
            ret.filterInputs = getFilterInputs(connName, fields);
            ret.pagination = true;
            ret.rownumbers = true;
            ret.singleSelect = true;
            ret.checkOnSelect = true;
            ret.selectOnCheck = true;
            ret.pagePosition = "bottom";
            ret.pageSize = DataSourceConst.DefPageSize;
            //    ret.pageNumber = _pagination.pageCount;
            ret.pageList = DataSourceConst.PageSizeList;
            ret.showHeader = true;
            ret.showFooter = true;
            ret.title = title;
            ret.striped = true;
            ret.loadMsg = Lang.Loading;
            ret.multiSort = true;
            //     ret.data = data(_pagination.page, _pagination.pageSize);
            return ret;
        }

        private static List<FilterInput> getFilterInputs(string connName, List<FieldDef> fields)
        {
            List<FilterInput> ret = new List<FilterInput>();

            foreach (FieldDef col in fields)
            {
                if (DatabaseAdmin.getInstance(connName).getDbType(col.Type) == DbType.Binary) continue;


                //if (col.DataType.Equals(typeof(System.String))) continue;

                FilterInput fi = new FilterInput();
                fi.field = col.Name;
                fi.options = new FilterOption();
                fi.op = FilterInput.BigTextOP;
                ret.Add(fi);

            }
            return ret;
        }



        private static List<DataListColumn> getColumns(List<FieldDef> fields)
        {

            List<DataListColumn> ret = new List<DataListColumn>();

            foreach (FieldDef col in fields)
            {
                DbType type = DatabaseAdmin.getInstance().getDbType(col.Type);
                if (type.Equals(DbType.Binary)) continue;
                DataListColumn listCol = new DataListColumn();
                string fldName = col.Name;
                listCol.field = fldName;
                listCol.title = col.Title;
                listCol.resizable = true;
                listCol.hidden = false;
                listCol.editor = "text";
                ret.Add(listCol);

            }
            return ret;
        }
    }
}
