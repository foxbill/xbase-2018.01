using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;
using System.Data;
using System.Data.Common;
using xbase;
using System.Globalization;
using System.Data.SqlClient;
using System.IO;
using xbase.Interface;
using System.Data.OleDb;
//using xbase.data.db;
using xbase.umc;
using xbase.data.Exceptions;
using xbase.security;
using xbase.Exceptions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using xbase.data.db;
using xbase.umc.attributes;
using Newtonsoft.Json;
using xbase.local;
using System.Web;
using xbase.data.easyui;
using xbase.math;
using System.Transactions;

namespace xbase.data
{

    /// <summary>
    /// 数据列表对象，用于
    /// </summary>
    [WboAttr(Id = "ds", Title = "数据源", Version = 1.1, LifeCycle = LifeCycle.Session, ContainerType = typeof(DataSourceSchemaContainer)
    , IsPublish = true)]
    public class DataSource : HttpWbo, INamedWbo
    {
        private DataSourceSchema _schema;
        private string _name;
        private bool _isSourceTable = false;
        private DataSet _ds;
        private Dictionary<string, string> _queryParams;
        private PaginationInfo _pagination = new PaginationInfo();
        private string[] _sorts;
        private List<FilterRule> _filterRules;
        private string updateText;
        private string insertText;
        private List<string> readOnlyFields = new List<string>();
        private List<FieldDef> _fieldDefs;
        private List<DataListColumn> _fieldColumns = null;

        private static string getFileUrl(string fileName)
        {
            return XSite.DataFileVirPath + fileName.Remove(0, XSite.DataFilePath.Length).Replace("\\", "/");
        }


        private void schemaChanged(object sender)
        {
            loadSchema();
        }

        private void loadSchema()
        {
            _ds = null;
            _fieldColumns = null;
            _pagination = new PaginationInfo();
            if (DataSourceSchemaContainer.Instance().Contains(name))
            {
                _schema = DataSourceSchemaContainer.Instance().GetItem(name);
                _schema.ChangedEvent += schemaChanged;
                if (_schema.SelectCommand == null)
                    throw new XException(Lang.DataSourceNotSelectCommand);
                _isSourceTable = false; //_schema.SelectCommand.CommandType == CommandType.TableDirect;
            }
            else
            {
                _isSourceTable = true;
                _schema = DataSourceSchemaBuilder.BuildTableSchema(_name);
            }
        }

        private void setQueryParameters(CommandSchema cmdSchema, DbCommand cmd)
        {
            DatabaseAdmin dbAdmin = DatabaseAdmin.getInstance(_schema.ConnectionName);

            foreach (ParameterSchema p in cmdSchema.QueryParams)
            {
                switch (p.Direction)
                {
                    case ParameterDirection.Input:
                    case ParameterDirection.InputOutput:
                        string value = p.DefaultValue;
                        if (Request.QueryString.AllKeys.Contains(p.Id))
                            value = Request[p.Id];
                        if (!string.IsNullOrEmpty(value) && value.StartsWith("(") && value.EndsWith(")"))
                        {
                            value = null;
                            object obj = Umc.invoke(value, null).ToString();
                            if (obj != null)
                                value = obj.ToString();
                        }

                        break;
                }

            }
        }

        private string getExpressValue(string express)
        {
            if (string.IsNullOrEmpty(express))
                return express;
            if (!(express.StartsWith("[") && express.EndsWith("]")))
                return express;
            express = express.Trim('[', ']');
            return Umc.invoke(express, null).ToString();
        }

        private Dictionary<string, string> getQueryParams(CommandSchema cmdSchema)
        {
            if (cmdSchema == null || cmdSchema.QueryParams == null) return null;
            Dictionary<string, string> ret = new Dictionary<string, string>();
            foreach (ParameterSchema paramSchema in cmdSchema.QueryParams)
            {

                string name = paramSchema.Id;
                if (string.IsNullOrEmpty(name)) continue;
                if (!name.StartsWith("@")) name = "@" + name;
                string value = paramSchema.DefaultValue;
                string qryName = name.TrimStart('@');
                if (!string.IsNullOrEmpty(value) && value.StartsWith("[") && value.EndsWith("]"))
                    value = getExpressValue(value);
                else if (Request.QueryString.AllKeys.Contains(name))
                    value = Request.QueryString[qryName];
                else if (Request.QueryString.AllKeys.Contains(qryName))
                    value = Request.QueryString[qryName];

                ret.Add(name, value);
            }
            return ret;
        }
        private void applySchema(DataTable table)
        {
            foreach (FieldSchema fldsch in _schema.Fields)
            {
                if (!fldsch.Visable) continue;
                //  DataColumn col = mastTable.Columns[fldsch.Id];

                DataListColumn listCol = new DataListColumn();

                string fldName = fldsch.Id;
                if (!string.IsNullOrEmpty(fldsch.Alias))
                    fldName = fldsch.Alias;
                //if (col.ExtendedProperties.ContainsKey("Alias"))
                //    fldName = col.ExtendedProperties["Alias"].ToString();

                listCol.field = fldName;
                listCol.title = fldName;
                if (string.IsNullOrEmpty(fldsch.Title))
                    listCol.title = fldsch.Title;
                listCol.editor = "text";
                //listCol.width = 0;
                //listCol.rowspan = 1;
                //listCol.colspan = 1;
                listCol.align = null;
                listCol.halign = null;
                listCol.sortable = true;
                //    listCol.order = "asc";
                listCol.resizable = true;
            }
        }

        private void clearBuffer()
        {
            _ds = null;
        }

        private string getOrderBy()
        {
            if (_sorts == null || _sorts.Length < 1)
                return null;

            StringBuilder sbSort = new StringBuilder();

            foreach (string s in _sorts)
            {
                sbSort.Append(",");
                sbSort.Append(s);
            }
            sbSort.Remove(0, 1);
            return sbSort.ToString();
        }

        private string getWhere()
        {
            StringBuilder sb = new StringBuilder();

            //if (_queryParams != null && _queryParams.Count > 0 && _isSourceTable)
            //{
            //    foreach (string key in _queryParams.Keys)
            //    {
            //        sb.Append(" and ");
            //        sb.Append(key);
            //        sb.Append("=");
            //        sb.Append(_queryParams[key]);
            //    }
            //}

            if (_filterRules != null && _filterRules.Count > 0)
            {
                foreach (FilterRule fr in _filterRules)
                {
                    if (string.IsNullOrEmpty(fr.op))
                        continue;

                    FilterOps op = FilterOpSigns.parse(fr.op);
                    if (op == FilterOps.nofilter) continue;

                    sb.Append(" and ");
                    sb.Append(fr.field);
                    sb.Append(FilterOpSigns.getSign(op));

                    switch (op)
                    {
                        case FilterOps.isnull:
                        case FilterOps.notnull:
                            continue;
                    }


                    sb.Append(" '");
                    switch (op)
                    {
                        case FilterOps.contains:
                        case FilterOps.endwith:
                            sb.Append("%");
                            break;
                    }
                    sb.Append(fr.value);
                    switch (op)
                    {
                        case FilterOps.contains:
                        case FilterOps.beginwith:
                            sb.Append("%");
                            break;
                    }
                    sb.Append("' ");
                }
            }

            if (sb.Length > 0)
                sb.Remove(0, " and ".Length);
            return sb.ToString();

        }

        private List<FilterRule> getFilterRules()
        {
            DataSet ds = getDataSet();
            DataTable tb = ds.Tables[0];

            List<FilterRule> ret = new List<FilterRule>();
            foreach (DataColumn col in tb.Columns)
            {
                if (col.DataType.IsArray) continue;
                FilterRule fr = new FilterRule();
                fr.field = col.ColumnName;
                ret.Add(fr);
            }

            return ret;
        }
        private string getRecordFolder(string fieldName, ListDataRow row)
        {

            StringBuilder sb = new StringBuilder();
            List<string> mkeys = _schema.PrimaryKeys;
            for (int i = 0; i < mkeys.Count; i++)
            {
                string keyName = mkeys[i];
                string keyVal = row[keyName];
                sb.Append('_');
                sb.Append(keyVal);
            }

            string fFolder = getFieldFolder(fieldName);
            fFolder = fFolder + sb.ToString() + "\\";
            if (!Directory.Exists(fFolder))
                Directory.CreateDirectory(fFolder);
            return fFolder;
        }

        public DataSource()
        {

        }

        public DataSource(string name)
        {
            setName(name);
        }
        /// <summary>
        /// 
        /// </summary>
        public string name
        {
            get { return _name; }
            set { setName(value); }
        }

        private void setName(string name)
        {
            if (string.IsNullOrEmpty(name) || name.Equals(_name, StringComparison.OrdinalIgnoreCase))
                return;

            this._name = name;
            loadSchema();
        }

        /// <summary>
        /// 获取数据集
        /// </summary>
        /// <returns></returns>
        [WboMethodAttr(Title = "获取数据", PermissionTypes = PermissionTypes.Read)]
        public DataSet getDataSet()
        {
            if (_ds != null) return _ds;
            string where = getWhere();
            string orderBy = getOrderBy();
            string groupBy = null;
            DsAdapterCustomer dsa = new DsAdapterCustomer(_schema);
            return dsa.getDataSet(getQueryParams(_schema.SelectCommand), where, orderBy, groupBy, _pagination);
        }


        public DataTable getMainTable()
        {
            return getDataSet().Tables[0];
        }
        /// <summary>
        /// 用实际查询结果刷新字段配置
        /// </summary>
        [WboMethodAttr(Title = "刷新字段配置", PermissionTypes = PermissionTypes.Write)]
        public void refreshFields()
        {
            DatabaseAdmin dba = DatabaseAdmin.getInstance(_schema.ConnectionName);
            DataSet ds = getDataSet();
            if (ds == null || ds.Tables.Count < 1)
                return;
            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                FieldSchema fs = _schema.Fields.FindItem(col.ColumnName);
                if (fs == null)
                {
                    fs = new FieldSchema();
                    fs.Id = col.ColumnName;
                    fs.DataType = dba.getDbType(col.DataType);
                    _schema.Fields.Add(fs);
                }
            }
            if (DataSourceSchemaContainer.Instance().Contains(_name))
                DataSourceSchemaContainer.Instance().UpdateItem(_name, _schema);
        }

        /// <summary>
        /// 获取字段标题返回格式{fieldName:fieldCaption , fieldName:FieldCaption}
        /// </summary>
        public Dictionary<string, string> FieldCaptions
        {
            get
            {
                Dictionary<string, string> ret = new Dictionary<string, string>();
                foreach (FieldSchema fld in _schema.Fields)
                {
                    if (fld.Visable)
                    {
                        ret.Add(fld.Id, string.IsNullOrEmpty(fld.Title) ? fld.Id : fld.Title);
                    }
                }
                return ret;
            }
        }

        /// <summary>
        /// 返回easy ui规范的表格
        /// </summary>
        /// <returns></returns>
        public EasyUiGridData grid()
        {
            _filterRules = null;
            _sorts = null;
            _ds = null;

            string title = _name;
            if (_schema != null && !string.IsNullOrEmpty(_schema.Title))
                title = _schema.Title;
            return EUGridUtils.getGrid(title, _schema.Fields);
        }

        /// <summary>
        /// 设置查询规则
        /// </summary>
        /// <param name="filterRules">
        /// </param>
        [WboMethodAttr(PermissionTypes = PermissionTypes.Read)]
        public void setfilterRules(List<FilterRule> filterRules)
        {
            if (this._filterRules == filterRules)
                return;

            if (filterRules != null && _filterRules != null && filterRules.Count == _filterRules.Count)
            {
                foreach (FilterRule fr in filterRules)
                {
                    bool isEquals = false;
                    foreach (FilterRule fr1 in _filterRules)
                    {
                        if (String.Compare(fr.field, fr1.field, true) == 0 && String.Compare(fr.op, fr1.op, true) == 0 &&
                            String.Compare(fr.value, fr1.value, true) == 0)
                        {
                            isEquals = true;
                            break;
                        }
                    }
                    if (!isEquals)
                    {
                        clearBuffer();
                        this._filterRules = filterRules;
                        return;
                    }
                }
                return;
            }
            clearBuffer();
            this._filterRules = filterRules;
        }

        /// <summary>
        /// 返回所有数据的ListData对象,格式为{total:记录总数,rows:[row1,row2]}
        /// row的可是为{field1:value,field2:value}
        /// </summary>
        /// <returns></returns>
        [WboMethodAttr(Title = "获取数据", PermissionTypes = PermissionTypes.Read)]
        public ListData data(int page, int rows, List<FilterRule> filterRules)
        {
            setfilterRules(filterRules);
            return data(page, rows);
        }

        /// <summary>
        /// 返回所有数据的ListData对象,格式为{total:记录总数,rows:[row1,row2]}
        /// row的可是为{field1:value,field2:value}
        /// </summary>
        /// <returns></returns>
        [WboMethodAttr(Title = "获取数据", PermissionTypes = PermissionTypes.Read)]
        public ListData data()
        {
            return data(0, 0);
        }

        [WboMethodAttr(Title = "获取数据", PermissionTypes = PermissionTypes.Read)]
        public ListData data(int page, int rows)
        {
            _pagination.page = page;
            _pagination.pageSize = rows;
            ListData ret = new ListData();

            ret.rows = this.rows();
            ret.total = _pagination.total;

            return ret;
        }

        [WboMethodAttr(Title = "获取数据", PermissionTypes = PermissionTypes.Read)]
        public ListData data(int page, int rows, string sort, string order, List<FilterRule> filterRules)
        {
            setfilterRules(filterRules);
            return data(page, rows, sort, order);
        }


        [WboMethodAttr(Title = "获取数据", PermissionTypes = PermissionTypes.Read)]
        public ListData data(int page, int rows, string sort, string order)
        {
            if (!(sort != null && order != null && sort.Equals(sort, StringComparison.OrdinalIgnoreCase) && order.Equals(order)))
            {
                _ds = null;
                if (order == null)
                    order = "";
                if (!string.IsNullOrEmpty(sort))
                {
                    _sorts = sort.Split(',');
                    string[] orders = order.Split(',');
                    for (int i = 0; i < _sorts.Length; i++)
                    {
                        _sorts[i] = i < orders.Length ? _sorts[i] + " " + orders[i].Trim().ToUpper() : _sorts[i];
                    }
                }
            }

            ListData ret = data(page, rows);

            return ret;
        }

        public List<DataListColumn> cols
        {
            get { return columns(); }
        }

        [WboMethodAttr(Title = "字段文件夹", PermissionTypes = PermissionTypes.Read)]
        public string getFieldFolder(string fieldName)
        {
            string path = _schema.TableName;

            if (string.IsNullOrEmpty(path))
            {
                if (_schema.SelectCommand == null || _schema.SelectCommand.CommandType != CommandType.TableDirect)
                    throw new Exception(Lang.DataSourceNotTableNameCannotUploadFile);
                path = _schema.SelectCommand.CommandText;
            }
            path = XSite.DataFilePath + path + "\\" + fieldName + "\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }

        [WboMethodAttr(Title = "获取配置信息", PermissionTypes = PermissionTypes.Read)]
        public DataSourceSchema getSchema()
        {
            return this._schema;
        }

        [WboMethodAttr(Title = "获取列配置信息", PermissionTypes = PermissionTypes.Read)]
        public List<DataListColumn> columns()
        {
            //  if (_fieldColumns == null || _fieldColumns.Count < 1)
            _fieldColumns = EUGridUtils.getColumns(_schema.Fields);
            return _fieldColumns;
        }

        [WboMethodAttr(Title = "设置列宽度", PermissionTypes = PermissionTypes.Write)]
        public void setFieldWidth(string field, int width)
        {
            if (_isSourceTable) return;
            _fieldColumns = null;
            FieldSchema fieldSchema = _schema.Fields.FindItem(field);
            if (fieldSchema == null) return;
            fieldSchema.DisplayWidth = width;
            if (DataSourceSchemaContainer.Instance().Contains(_name))
                DataSourceSchemaContainer.Instance().UpdateItem(_name, _schema);
        }

        /// <summary>
        /// 用户修改列属性
        /// </summary>
        /// <param name="cols"></param>
        [WboMethodAttr(Title = "设置列配置", PermissionTypes = PermissionTypes.Write)]
        public void setColumns(List<DataListColumn> cols)
        {
            foreach (DataListColumn col in cols)
            {
                FieldSchema fldSch = _schema.Fields.FindItem(col.field);
                if (fldSch == null) continue;
                fldSch.DisplayWidth = col.width;
                fldSch.Title = col.title;
            }

            if (DataSourceSchemaContainer.Instance().Contains(_name))
                DataSourceSchemaContainer.Instance().UpdateItem(name, _schema);

            _fieldColumns = null;
        }

        [WboMethodAttr(Title = "设置隐藏列", PermissionTypes = PermissionTypes.Write)]
        public void setFieldVisable(string field, bool visable)
        {
            if (_isSourceTable) return;
            _fieldColumns = null;
            FieldSchema fieldSchema = _schema.Fields.FindItem(field);
            if (fieldSchema == null) return;
            fieldSchema.Visable = visable;
            if (DataSourceSchemaContainer.Instance().Contains(_name))
                DataSourceSchemaContainer.Instance().UpdateItem(_name, _schema);
        }

        [WboMethodAttr(Title = "设置列标题", PermissionTypes = PermissionTypes.Write)]
        public void setFieldTitle(string field, string title)
        {
            if (_isSourceTable) return;
            _fieldColumns = null;
            FieldSchema fieldSchema = _schema.Fields.FindItem(field);
            if (fieldSchema == null) return;
            fieldSchema.Title = title;
            if (DataSourceSchemaContainer.Instance().Contains(_name))
                DataSourceSchemaContainer.Instance().UpdateItem(_name, _schema);
        }

        /// <summary>
        /// 当前页号
        /// </summary>
        public int pageNo
        {
            get { return _pagination.page; }
            set { _pagination.page = value; }
        }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int pageSize
        {
            get { return _pagination.pageSize; }
            set { _pagination.pageSize = value; }

        }

        /// <summary>
        /// 设置查询参数
        /// </summary>

        [WboMethodAttr(Title = "获取记录列表", PermissionTypes = PermissionTypes.Read)]
        public List<ListDataRow> rows(int page, int rows)
        {
            if (page < 1) page = 1;
            if (rows < 1) rows = DataSourceConst.DefPageSize;
            _pagination.page = page;
            _pagination.pageSize = rows;
            return this.rows();
        }

        [WboMethodAttr(Title = "获取记录列表", PermissionTypes = PermissionTypes.Read)]
        public List<ListDataRow> rows(int page, int rows, List<FilterRule> filterRules)
        {
            setfilterRules(filterRules);
            return this.rows(page, rows);
        }

        [WboMethodAttr(Title = "获取记录列表", PermissionTypes = PermissionTypes.Read)]
        public List<ListDataRow> rows()
        {

            List<ListDataRow> ret = new List<ListDataRow>();
            DataSet ds = getDataSet();
            DataTable tb = ds.Tables[0];

            if (tb == null) return ret;

            int start = 0;
            int count = tb.Rows.Count;


            if (!_pagination.isStoreProcessPagination)
                if (_pagination.page != 0 && _pagination.pageSize != 0)
                {
                    start = (_pagination.page - 1) * _pagination.pageSize;
                    count = (_pagination.page) * _pagination.pageSize;
                }

            if (start < 0) start = 0;

            for (int r = start; r < count; r++)
            {
                if (r >= tb.Rows.Count) break;
                DataRow dRow = tb.Rows[r];

                ListDataRow row = DataSourceComm.readRow(tb, _schema, dRow);
                ret.Add(row);
            }

            if (_pagination.isStoreProcessPagination)
                this._ds = null;

            return ret;
        }


        /// <summary>
        /// 获取过滤表单的输入组件定义
        /// </summary>
        /// <returns></returns> 
        private DataSet updateSourceTableRow(ListDataRow row)
        {
            DatabaseAdmin dba = DatabaseAdmin.getInstance(_schema.ConnectionName);
            //            Database db = dba.Database;
            DbCommand cmd = null;

            cmd = dba.getSqlStringCommand(" update " + _schema.TableName);
            StringBuilder sb = new StringBuilder();
            StringBuilder sbWhere = new StringBuilder();

            sb.Append(" update [");
            sb.Append(_schema.TableName);
            sb.Append("] set ");

            bool first = true;
            foreach (string field in row.Keys)
            {
                string paramName = "@" + field.Replace(' ', '_');


                FieldSchema fldSchema = null;
                if (field.StartsWith(XSqlBuilder.OLD_VERSION_PIX))
                {
                    string keyField = field.Replace(XSqlBuilder.OLD_VERSION_PIX, "");
                    fldSchema = _schema.Fields.GetItem(keyField);
                    sbWhere.Append(" and [");
                    sbWhere.Append(keyField);
                    sbWhere.Append("]=");
                    sbWhere.Append(paramName);
                }
                else
                {

                    fldSchema = _schema.Fields.GetItem(field);

                    if (readOnlyFields.Contains(field))
                        continue;

                    if (first)
                        first = false;
                    else
                        sb.Append(",");

                    sb.Append("[");
                    sb.Append(field);
                    sb.Append("]");
                    sb.Append("=");
                    sb.Append(paramName);
                }

                dba.addInParameter(cmd, paramName, fldSchema.DataType, string.IsNullOrEmpty(row[field]) ? null : row[field]);

            }

            sbWhere.Remove(0, 5);

            if (sbWhere.Length < 2)
                throw new XException(Lang.UpdateNoKey);

            sb.Append(" where ");
            sb.Append(sbWhere.ToString());
            cmd.CommandText = sb.ToString();
            return dba.executeDateSet(cmd);
        }

        /// <summary>
        /// 更新一行,新版本中采用update
        /// </summary>
        /// <param name="row"></param>
        [WboMethodAttr(Title = "更新行（已废弃）新版用update代替", PermissionTypes = PermissionTypes.Write)]
        public void updateRow(ListDataRow row)
        {
            update(row);
        }

        /// <summary>
        /// 更新行，更新时系统查找Row中查找是否有前缀为"pk_"的字段，有则查找其值对应的记录并更新，否则插入行
        /// </summary>
        /// <param name="row"></param>
        [WboMethodAttr(Title = "更新行", PermissionTypes = PermissionTypes.Write)]
        public void update(ListDataRow row)
        {
            //if (_isSourceTable)
            //    updateSourceTableRow(row);
            DsAdapterCustomer dbsa = new DsAdapterCustomer(_schema);
            if (DataSourceComm.isNewRow(row))
                dbsa.insert(row, this.getQueryParams(_schema.InsertCommand));
            else
                dbsa.update(row, this.getQueryParams(_schema.UpdateCommand));

        }

        /// <summary>
        /// 更新行，更新时系统查找Row中查找是否有前缀为"pk_"的字段，有则查找其值对应的记录并更新，否则插入行
        /// </summary>
        /// <param name="row"></param>
        [WboMethodAttr(Title = "更新行", PermissionTypes = PermissionTypes.Write)]
        public void update(ListDataRow row,List<SubTable> subTables)
        {
            update(row);
            updateSubTables(row,subTables);
        }



        private void updateSubTables(ListDataRow row, List<SubTable> subTables)
        {
            if (subTables == null || subTables.Count < 1) return;
            for (int c = 0; c < subTables.Count; c++)
            {
                string subTable = subTables[c].Name;
                DataSourceSchema ds = DataSourceSchemaContainer.Instance().GetItem(subTable);
                if (ds.SelectCommand.CommandType != CommandType.TableDirect)
                    throw new XException(string.Format(Lang.SubTableSelCommandTypeOnlyIsTable, subTable));

                SubTableSchema sds = DataSourceComm.getSubTableSchema(subTable, _schema);
                Dictionary<string, string> parametes = new Dictionary<string, string>();


                for (int i = 0; i < sds.Fks.Count; i++)
                {
                    string fk = sds.Fks[i];
                    parametes.Add("@" + fk, row[_schema.PrimaryKeys[i]]);
                }


                DsAdapter dsa = new DsAdapterCustomer(ds);

                List<ListDataRow> subRows = subTables[c].Rows;
                for (int i = 0; i < subRows.Count; i++)
                {
                    ListDataRow subRow = subRows[i];
                    if (DataSourceComm.isNewRow(subRow))
                        dsa.insert(subRow, parametes);
                    else
                        dsa.update(subRow, parametes);
                }

            }
        }

        [WboMethodAttr(Title = "批量新行", PermissionTypes = PermissionTypes.Write)]
        public void update(List<ListDataRow> insertRows, List<ListDataRow> updateRows, List<ListDataRow> deleteRows)
        {
            DsAdapterCustomer dsa = new DsAdapterCustomer(_schema);
            dsa.update(updateRows, getQueryParams(_schema.UpdateCommand));
            dsa.insert(insertRows, getQueryParams(_schema.InsertCommand));
            dsa.delete(deleteRows, getQueryParams(_schema.DeleteCommand));
        }

        [WboMethodAttr(Title = "批量新行", PermissionTypes = PermissionTypes.Write)]
        public void update(List<ListDataRow> rows)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                foreach (ListDataRow row in rows)
                {
                    update(row);
                }
                ts.Complete();
            }
        }

        [WboMethodAttr(Title = "插入记录", PermissionTypes = PermissionTypes.Write)]
        public void insert(ListDataRow row)
        {
            DsAdapterCustomer dbsa = new DsAdapterCustomer(_schema);
            dbsa.insert(row, getQueryParams(_schema.InsertCommand));
        }


        /// <summary>
        /// 用于接收Post记录表单，如果Post表单记录中包含"pk_"前缀的字段则用pk_前缀的字段更新记录，否则添加一条新纪录。
        /// </summary>
        [WboMethodAttr(Title = "提交表单", PermissionTypes = PermissionTypes.Write)]
        public void form()
        {
            ListDataRow row = new ListDataRow();

            foreach (string fld in this.Request.Form.AllKeys)
            {
                row.Add(fld, Request.Form[fld]);
            }

            for (int i = 0; i < Request.Files.AllKeys.Length; i++)
            {
                string fld = Request.Files.AllKeys[i];
                HttpPostedFile file = Request.Files[i];
                List<string> updatedFields = new List<string>();


                if (file != null && file.ContentLength > 0)
                {

                    string recFolder = getRecordFolder(fld, row);
                    string fileName = Path.GetFileName(file.FileName);

                    fileName = recFolder + fileName;
                    file.SaveAs(fileName);

                    if (fld.StartsWith("file_"))
                        fld = fld.Remove(0, 5);

                    if (!updatedFields.Contains(fld))
                    {
                        row[fld] = TextType.img + getFileUrl(fileName);
                    }
                }
            }
            update(row);
        }

        /// <summary>
        /// 通过指定Key字段和值字段来获取获取一个键值对字典，用于下拉框、选项框等界面元素的数据
        /// </summary>
        /// <param name="keyField"></param>
        /// <param name="valueField"></param>
        /// <returns></returns>
        [WboMethodAttr(PermissionTypes = PermissionTypes.Read)]
        public Dictionary<string, object> dict(string keyField, string valueField)
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            DataTable tb = getDataSet().Tables[0];
            foreach (DataRow row in tb.Rows)
            {
                ret.Add(row[keyField].ToString(), row[valueField]);
            }
            return ret;
        }
        /// <summary>
        /// 获取数据源字段名数组
        /// </summary>
        /// <param name="field"></param>
        /// <param name="rowId"></param>
        /// <returns></returns>
        [WboMethodAttr(PermissionTypes = PermissionTypes.Read)]
        public List<string> getFieldFiles(string field, ListDataRow rowId)
        {
            List<string> ret = new List<string>();
            string folder = getRecordFolder(field, rowId);
            string[] files = Directory.GetFiles(folder);
            foreach (string file in files)
            {
                string fileName = getFieldFolder(field) + file;
                ret.Add(getFileUrl(fileName));
            }
            return ret;
        }

        /// <summary>
        /// 通过主键获取一条记录
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        [WboMethodAttr(PermissionTypes = PermissionTypes.Read)]
        public ListDataRow row(Dictionary<string, string> pk)
        {
            if (pk == null || pk.Count < 1)
            {
                throw new Exception(Lang.UpdateNoKey);
            }

            StringBuilder sb = new StringBuilder();

            foreach (string field in pk.Keys)
            {
                sb.Append(" and ");
                sb.Append(field);
                sb.Append("='");
                sb.Append(pk[field]);
                sb.Append("' ");
            }

            sb.Remove(0, 5);
            PaginationInfo pagin = new PaginationInfo();

            DsAdapterCustomer dsa = new DsAdapterCustomer(_schema);
            DataSet ds = dsa.getDataSet(getQueryParams(_schema.SelectCommand), sb.ToString(), null, null, pagin);
            if (ds.Tables.Count < 1)
                return null;
            DataTable tb = ds.Tables[0];
            DataRow[] rows = tb.Select(sb.ToString());
            if (tb.Rows.Count < 1)
                return null;
            ListDataRow ret = DataSourceComm.readRow(tb, _schema, rows[0]);
            return ret;
        }

        public ListDataRow row()
        {
            List<ListDataRow> rows = this.rows();
            if (rows.Count > 0)
                return rows[0];
            else
                return null;
        }

        public List<ListDataRow> sub(string table, Dictionary<string, string> pk)
        {
            SubTableSchema subSchema = DataSourceComm.getSubTableSchema(table, _schema);
            List<ListDataRow> rows = getSubTableRows(pk, subSchema);
            return rows;
        }

        private List<ListDataRow> getSubTableRows(Dictionary<string, string> pks, SubTableSchema subSchema)
        {
            DatabaseAdmin dba = DatabaseAdmin.getInstance();
            DataSource subDs = new DataSource(subSchema.Name);

            DataSourceSchema dss = subDs.getSchema();
            
            if (dss.SelectCommand.CommandType != CommandType.TableDirect)
                throw new XException(string.Format(Lang.SubTableSelCommandTypeOnlyIsTable, subSchema.Name));

            StringBuilder sb = new StringBuilder("select * from ");
            sb.Append(dss.SelectCommand.CommandText);
            sb.Append(" ");
            sb.Append(" where ");
            Hashtable ps = new Hashtable();
            for (int i = 0; i < subSchema.Fks.Count; i++)
            {
                string fk = subSchema.Fks[i];
                string pk = _schema.PrimaryKeys[i];
                sb.Append(fk);
                sb.Append("=@");
                sb.Append(pk);
                sb.Append(" and ");
                ps.Add("@" + pk, pks[pk].ToString());
            }
            sb.Remove(sb.Length - 5, 5);
            DbCommand cmd = dba.getSqlStringCommand(sb.ToString());
            foreach (string key in ps.Keys)
            {
                dba.addInParameter(cmd, key, DbType.String, ps[key]);
            }
            DataTable tb = dba.executeTable(cmd);

            List<ListDataRow> rows = new List<ListDataRow>();
            foreach (DataRow row in tb.Rows)
            {
                rows.Add(DataSourceComm.readRow(tb, dss, row));
            }
            return rows;
        }

        [WboMethodAttr(Title = "删除行", PermissionTypes = PermissionTypes.Write)]
        public void delete(ListDataRow row)
        {
            DsAdapterCustomer dsa = new DsAdapterCustomer(_schema);
            dsa.delete(row, getQueryParams(_schema.DeleteCommand));
        }

        /// <summary>
        /// 删除行
        /// </summary>
        [WboMethodAttr(Title = "删除行", PermissionTypes = PermissionTypes.Write)]
        public void delete()
        {
            DsAdapterCustomer dsa = new DsAdapterCustomer(_schema);
            dsa.delete(getQueryParams(_schema.DeleteCommand));
        }

        public void delete(List<ListDataRow> rows)
        {
            DsAdapterCustomer dsa = new DsAdapterCustomer(_schema);
            dsa.delete(rows, getQueryParams(_schema.DeleteCommand));
        }

        public string[] GetFieldData(string p)
        {
            throw new NotImplementedException();
        }
    }


}
