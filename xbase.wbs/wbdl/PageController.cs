using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xData;
using xBase;
using System.Reflection;
using wbs.wbap;
using System.Data;
using xBase.Umc;

namespace wbs
{
    public class E_WbdlPageNotHaveListElement : XException { public E_WbdlPageNotHaveListElement(string msg) : base(msg) { } }
    public class E_WbdlPageNotHaveFieldElement : XException { public E_WbdlPageNotHaveFieldElement(string msg) : base(msg) { } }
    public class E_WbdlPageEventActionNotDefine : XException { public E_WbdlPageEventActionNotDefine(string message) : base(message) { } };
    public class E_WbdlPageActionIdNotAssignedForEvent : XException { public E_WbdlPageActionIdNotAssignedForEvent(string message) : base(message) { } };
    public class E_WbdlPageCannotGetTableSchame : XException { public E_WbdlPageCannotGetTableSchame(string message) : base(message) { } };
    public class E_WbdlPageCannotGetFieldSchame : XException { public E_WbdlPageCannotGetFieldSchame(string message) : base(message) { } }
    public class E_WbdlPageFieldBindSchemaNotAssignedFieldId : XException { public E_WbdlPageFieldBindSchemaNotAssignedFieldId(string message) : base(message) { } };
    public class E_WbdlDataSourceNotFindTable : XException { public E_WbdlDataSourceNotFindTable(string message) : base(message) { } };
    public class E_WbdlDatasourceNotFindField : XException { public E_WbdlDatasourceNotFindField(string message) : base(message) { } };
    public class E_WbdlDataTableNotRowForEdit : XException { public E_WbdlDataTableNotRowForEdit(string message) : base(message) { } };
    public class E_WbdlListSchemaNotDefine : XException { public E_WbdlListSchemaNotDefine(string message) : base(message) { } };
    public class E_WbdlWbapListNoKeyColumn : XException { public E_WbdlWbapListNoKeyColumn(string message) : base(message) { } };
    public class E_WbdlPageControllerException : XException { public E_WbdlPageControllerException(string message) : base(message) { } };


    public delegate void DataElementSchemaHandle(FieldBindSchema dataElement);



    public class PageController : BizController<WbdlSchema, WbdlSchemaContainer, WbdlPage>
    {


        private Dictionary<string, XTableSchema> tableSchemas = new Dictionary<string, XTableSchema>();
        private DataSource dataSource;
        private string sessionId;

        public string SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }

        public WbdlPage Page { get { return BuzObject; } }


        public PageController(WbdlPage page, string sessionId)
            : base(page)
        {
            this.sessionId = sessionId;
            CreateDataSource();
        }

        public PageController(string pageId, string sessionId)
            : base(pageId)
        {
            this.sessionId = sessionId;
            CreateDataSource();
        }

        internal bool IsDataBindElement(FieldBindSchema fieldSchema)
        {
            return (fieldSchema != null
                     && !string.IsNullOrEmpty(fieldSchema.TableId)
                     && !string.IsNullOrEmpty(fieldSchema.FieldId)
                    );
        }

        internal bool IsDataBindElement(string elementId)
        {
            FieldBindSchema fieldSchema = Schame.FieldBinds.FindItem(elementId);
            if (fieldSchema == null) throw new E_WbdlPageCannotGetFieldSchame(elementId);
            return (fieldSchema != null
                     && !string.IsNullOrEmpty(fieldSchema.TableId)
                     && !string.IsNullOrEmpty(fieldSchema.FieldId)
                    );
        }

        private void CreateDataSource()
        {
          //  this.dataSource = new DataSource(Schame.DataSource, this.ParamGetter);
            //XData xData = Umc.GetObject<XData>(this.sessionId, "", SysObjects.xData);
            //xData.DataSource = this.dataSource;
            //xData.GetDataSet();
        }

        private string ParamGetter(string paramName)
        {
            return "";
        }

        protected override void SetObj(WbdlPage obj)
        {
            base.SetObj(obj);
            InitDataSourceSchema();
        }

        private void InitDataSourceSchema()
        {
            //if (Schame.DataSource.Tables == null || Schame.DataSource.Tables.Count < 1)
            //{
            //    string[] tables = GetTableNames();
            //    foreach (string table in tables)
            //    {
            //        Schame.DataSource.Tables.NewItem(table);
            //    }
            //}
        }

        public void ForEachDataElementSchema(DataElementSchemaHandle handle)
        {
            foreach (FieldBindSchema dataElement in Schame.FieldBinds)
            {
                handle(dataElement);
            }
            foreach (DataListBindSchema listSchema in Schame.DataListBinds)
            {
                foreach (FieldBindSchema dataElement in listSchema.Columns)
                {
                    handle(dataElement);
                }
            }
        }

        public string[] GetTableNames()
        {
            List<string> tables = new List<string>();
            ForEachDataElementSchema(delegate(FieldBindSchema dataElement)
            {
                if (!tables.Contains(dataElement.TableId))
                    tables.Add(dataElement.TableId);
            });
            return tables.ToArray();
        }


        public XTableSchema GetTableSchema(string tableName)
        {
            return null;
            //XTableSchema tableSchema = null;

            //if (tableSchemas.ContainsKey(tableName))
            //{
            //    tableSchema = tableSchemas[tableName];
            //    return tableSchema;
            //}

            //if (XTableSchemaContainer.Instance().Contains(tableName))
            //    tableSchema = XTableSchemaContainer.Instance().GetItem(tableName);

            //if (this.dataSource.DataSet != null)
            //{
            //    DataSet ds = dataSource.DataSet;

            //    if (ds.Tables.Contains(tableName))
            //    {
            //        DataTable dataTable = ds.Tables[tableName];
            //        XTableAdapter tableCtr = new XTableAdapter(dataTable);
            //        tableSchema = tableCtr.Schame;
            //    }

            //}

            //if (tableSchema != null)
            //    tableSchemas.Add(tableName, tableSchema);

            //return tableSchema;
        }


        private void InitEventBinds()
        {
            Page.Events.Clear();
            //            Wbap.Wbap wbap = new Wbap.Wbap(this);
            foreach (EventSchema eventBindSchema in Schame.EventBinds)
            {
                PageEvent elementEvent = Page.Events.NewObject(eventBindSchema.Id);
                elementEvent.EventName = eventBindSchema.EventName;
                ActionFlowSchema actionSchema = null;
                if (eventBindSchema.ActionFlow == null || eventBindSchema.ActionFlow == "")
                    throw (new E_WbdlPageActionIdNotAssignedForEvent(eventBindSchema.Id));
                try
                {
                    actionSchema = Schame.Actions.GetItem(eventBindSchema.ActionFlow);
                }
                catch (EPeresisListNoItemOfId)
                {
                    throw new E_WbdlPageEventActionNotDefine(eventBindSchema.ActionFlow);
                }

                //           elementEvent.RequestEnv = wbap.GetRequestEnv(actionSchema);

            }
        }

        private void InitFieldBinds()
        {
            Page.StringElements.Clear();

            foreach (FieldBindSchema fieldBindSchema in Schame.FieldBinds)
            {
                if (IsDataBindElement(fieldBindSchema))
                {
                    PageDataForm form = Page.PageForms.FindForm(fieldBindSchema.TableId);
                    if (form == null)
                    {
                        DataTable dataTable = DataSet.Tables[fieldBindSchema.TableId];
                        if (dataTable == null)
                            continue;
                        form = new PageDataForm(fieldBindSchema.TableId);
                        form.DataTable = dataTable;
                        Page.PageForms.Add(form);
                    }
                    PageDataField field = new PageDataField(fieldBindSchema);
                    field.DataColumn = form.DataTable.Columns[fieldBindSchema.FieldId];
                    form.Fields.Add(field);
                }
                else
                {
                    Page.StringElements.Add(fieldBindSchema.Id, "");
                }
            }
        }

        private PageDataList InitDataList(DataListBindSchema listSchema)
        {
            PageDataList pageList = new PageDataList(listSchema);
            Page.PageLists.Add(listSchema.Id, pageList);
            string tableName = "";
            foreach (FieldBindSchema fieldSchema in listSchema.Columns)
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = fieldSchema.TableId;
                    DataTable dataTable = DataSet.Tables[tableName];
                    pageList.DataTable = dataTable;
                }
                PageDataField listField = new PageDataField(fieldSchema);
                listField.DataColumn = pageList.DataTable.Columns[fieldSchema.FieldId];
                pageList.Fields.Add(listField);
            }
            return pageList;
        }

        private void InitDataLists()
        {
            Page.PageLists.Clear();
            for (int i = 0; i < Schame.DataListBinds.Count; i++)
            {
                DataListBindSchema listSchema = Schame.DataListBinds[i];
                InitDataList(listSchema);
                //CreatePageList(listSchema);
                //FillWbapList(pageList);
            }
        }


        public string GetListTableId(DataListBindSchema listSchema)
        {
            foreach (FieldBindSchema field in listSchema.Columns)
            {
                if (!String.IsNullOrEmpty(field.TableId))
                    return field.TableId;
            }
            return null;
        }

        public DataSet DataSet
        {
            get
            {
                return this.dataSource.DataSet;
            }
        }



        public WbdlPage Initialize()
        {
            InitFieldBinds();
            InitEventBinds();
            InitDataLists();
            return this.Page;
        }


        internal List<FieldBindSchema> GetDataBindSchemas(string tableName)
        {
            List<FieldBindSchema> fieldBinds = new List<FieldBindSchema>();

            for (int i = 0; i < Schame.FieldBinds.Count; i++)
            {
                FieldBindSchema fieldBind = Schame.FieldBinds[i];
                if (tableName.Equals(fieldBind.TableId, StringComparison.OrdinalIgnoreCase))
                    fieldBinds.Add(fieldBind);
            }

            return fieldBinds;
        }


        internal DataListBindSchema GetListBindSchame(string tableName)
        {
            foreach (DataListBindSchema listBind in Schame.DataListBinds)
            {
                foreach (FieldBindSchema fieldBind in listBind.Columns)
                {
                    if (tableName.Equals(fieldBind.TableId, StringComparison.OrdinalIgnoreCase))
                        return listBind;
                }
            }
            return null;
        }


        internal FieldBindSchema FindFieldSchema(string tableName, string fieldName)
        {
            for (int i = 0; i < Schame.FieldBinds.Count; i++)
            {
                FieldBindSchema fieldBind = Schame.FieldBinds[i];
                if (tableName.Equals(fieldBind.TableId, StringComparison.OrdinalIgnoreCase))
                    if (fieldName.Equals(fieldBind.FieldId, StringComparison.OrdinalIgnoreCase))
                        return fieldBind;
            }
            foreach (DataListBindSchema listBind in Schame.DataListBinds)
            {
                foreach (FieldBindSchema fieldBind in listBind.Columns)
                {
                    if (tableName.Equals(fieldBind.TableId, StringComparison.OrdinalIgnoreCase))
                        if (fieldName.Equals(fieldBind.FieldId, StringComparison.OrdinalIgnoreCase))
                            return fieldBind;
                }
            }
            return null;
        }

        /// <summary>
        /// 检查DataSource中是否，有tableName指定的tableName
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        internal bool ContainsDataTable(string tableName)
        {
            foreach (TableSource table in dataSource.Tables)
            {
                if (table.TableName.Equals(tableName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        private WbapList CreatePageList(DataListBindSchema listSchema)
        {
            WbapList webList = new WbapList();
            webList.Id = listSchema.Id;
            webList.DataRow = listSchema.DataRow;

            for (int j = 0; j < listSchema.Columns.Count; j++)
            {
                FieldBindSchema fieldSchema = listSchema.Columns[j];
                webList.Columns.Add(fieldSchema.Id);
            }

            DataTable table = this.DataSet.Tables[listSchema.TableId];
            if (table == null) throw new E_WbdlDataSourceNotFindTable(listSchema.TableId);

            DataColumn[] keys = table.PrimaryKey;

            foreach (DataColumn key in keys)
            {
                string colName = key.ColumnName + WbdlConst._Key.ToString();
                if (!webList.Columns.Contains(colName))
                    webList.Columns.Add(colName);
            }

            return webList;

        }

        private void FillWbapList(WbapList list, DataRow[] dataRows)
        {
            DataListBindSchema listSchema = Schame.DataListBinds.FindItem(list.Id);
            if (listSchema == null) return;

            if (dataRows == null)
            {
                string tableName = GetListTableId(listSchema);
                if (String.IsNullOrEmpty(tableName)) return;
                if (this.DataSet == null) return;
                DataTable table = this.DataSet.Tables[listSchema.TableId];
                if (table == null) throw new E_WbdlDataSourceNotFindTable(listSchema.TableId);
                dataRows = table.Select();
            }

            list.Data.Clear();

            if (dataRows.Length == 1 && (dataRows[0].RowState == DataRowState.Added || dataRows[0].RowState == DataRowState.Detached))
                list.IsAdd = true;

            foreach (DataRow dataRow in dataRows)
            {
                List<string> listRow = new List<string>();
                list.Data.Add(listRow);
                foreach (string colName in list.Columns)
                {
                    string fieldName = null;

                    if (colName.EndsWith(WbdlConst._Key.ToString()))
                        fieldName = colName.Substring(0, colName.Length - WbdlConst._Key.ToString().Length);
                    else
                        fieldName = listSchema.Columns.GetItem(colName).FieldId;

                    if (String.IsNullOrEmpty(fieldName))
                        listRow.Add("");
                    else
                        listRow.Add(dataRow[fieldName].ToString());

                }
            }

        }


        internal void SetWbapList(string elementId, WbapList wbapList)
        {
            string typeMarker = WbapDataType._List.ToString();

            if (elementId.EndsWith(typeMarker))
            {
                elementId = elementId.Substring(0, elementId.Length - typeMarker.Length);
            }

            if (!Page.PageLists.ContainsKey(elementId))
            {
                DataListBindSchema listSchema = this.Schame.DataListBinds.FindItem(elementId);
                if (listSchema == null)
                    throw new E_WbdlListSchemaNotDefine(elementId);
                InitDataList(listSchema);
            }

            if (!Page.PageLists.ContainsKey(elementId))
                throw new E_WbdlPageNotHaveListElement(elementId);

            PageDataList pageList = Page.PageLists[elementId];
            if (pageList == null)
                throw new E_WbdlPageNotHaveListElement(elementId);


            DataColumn[] keyCols = pageList.DataTable.PrimaryKey;
            Dictionary<string, int> keyIndexs = new Dictionary<string, int>();

            for (int i = 0; i < keyCols.Length; i++)
            {
                string keyName = keyCols[i].ColumnName + WbdlConst._Key.ToString();
                int keyIndex = wbapList.Columns.IndexOf(keyName);
                if (keyIndex < 0) throw new E_WbdlWbapListNoKeyColumn(keyName);
                keyIndexs.Add(keyCols[i].ColumnName, keyIndex);
            }

            for (int i = 0; i < wbapList.Data.Count; i++)
            {
                List<string> row = wbapList.Data[i];
                string filter = "";

                bool isNewRow = false;
                foreach (KeyValuePair<string, int> keyIndex in keyIndexs)
                {
                    if (string.IsNullOrEmpty(row[keyIndex.Value]))
                    {
                        isNewRow = true;
                        break;
                    }
                    filter += keyIndex.Key + "='" + row[keyIndex.Value] + "' and ";
                }

                DataRow dataRow = null;
                if (!isNewRow)
                {
                    if (filter.EndsWith(" and "))
                        filter = filter.Substring(0, filter.Length - " and ".Length);

                    if (string.IsNullOrEmpty(filter)) throw new E_WbdlPageControllerException("key err can not select record ");

                    DataRow[] dataRows = pageList.DataTable.Select(filter);
                    if (dataRows.Length > 0)
                        dataRow = dataRows[0];
                }

                if (dataRow == null) isNewRow = true;

                if (isNewRow)
                {
                    isNewRow = true;
                    dataRow = pageList.DataTable.NewRow();
                }

                if (!isNewRow) dataRow.BeginEdit();
                foreach (PageDataField col in pageList.Fields)
                {
                    int wbapColIndex = wbapList.Columns.IndexOf(col.ElementId);
                    if (wbapColIndex > -1)
                        dataRow[col.FieldId] = row[wbapColIndex];
                }
                if (!isNewRow) dataRow.EndEdit();
                if (isNewRow)
                    pageList.DataTable.Rows.Add(dataRow);

            }

        }

        internal void SetRequestData(WbapDataBody wbapElementBinds)
        {

            PageForms forms = Page.PageForms;

            foreach (KeyValuePair<string, object> keyValue in wbapElementBinds)
            {
                var elementId = keyValue.Key;
                var Value = keyValue.Value;

                if (Value is WbapList)
                {
                    SetWbapList(elementId, (WbapList)Value);
                }
                else if (Value is string)
                {
                    PageDataForm form;
                    FieldBindSchema schema = this.Schame.FieldBinds.FindItem(elementId);
                    if (IsDataBindElement(schema))
                    {
                        form = forms.FindForm(schema.TableId);
                        if (form == null) InitFieldBinds();
                        form = forms.FindForm(schema.TableId);
                        if (form == null) throw new E_WbdlPageControllerException("not Initialize form " + schema.TableId);
                        form.CacheValue(schema.FieldId, (string)Value);
                    }
                    else
                    {
                        if (elementId.EndsWith(WbdlConst._Key.ToString()))
                        {
                            form = Page.PageForms.FindFormByKeyElement(elementId);
                            if (form == null) InitFieldBinds();
                            form = Page.PageForms.FindFormByKeyElement(elementId);
                            if (form != null)
                                form.PutKeyValue(elementId, (string)Value);
                            else
                                Page.StringElements.Add(elementId, (string)Value);

                        }

                    }
                }
            }

            foreach (PageDataForm form in forms)
            {
                form.UpdateCache();
            }
        }

        internal void SetStringElement(string elementId, string elementValue)
        {
            if (!Page.StringElements.ContainsKey(elementId))
                throw new E_WbdlPageNotHaveFieldElement(elementId);

            Page.StringElements[elementId] = elementValue;
        }

        private void SetElementDataValue(FieldBindSchema fieldSchema, string value)
        {
            if (!DataSet.Tables.Contains(fieldSchema.TableId))
                throw new E_WbdlDataSourceNotFindTable(fieldSchema.TableId);

            DataTable dataTable = DataSet.Tables[fieldSchema.TableId];

            if (!dataTable.Columns.Contains(fieldSchema.FieldId))
                throw new E_WbdlDatasourceNotFindField(fieldSchema.TableId + "." + fieldSchema.FieldId);

            int RowIndex = dataSource.Tables[fieldSchema.TableId].RowIndex;
            if (RowIndex > -1)
            {
                DataRow dataRow = dataTable.Rows[RowIndex];
                dataRow[fieldSchema.FieldId] = value;
            }
            else
            {
                throw new E_WbdlDataTableNotRowForEdit(fieldSchema.TableId);
            }
        }

        internal WbapList GetWbapList(string elementId)
        {
            WbapList list = GetNullWbapList(elementId);
            PageDataList pageDataList = Page.PageLists[elementId];
            FillWbapList(list, pageDataList.DataTable.Select());
            return list;
        }

        internal WbapList GetNullWbapList(string elementId)
        {
            DataListBindSchema listSchema = Schame.DataListBinds.GetItem(elementId);
            return CreatePageList(listSchema);
        }

        internal string GetStringElement(string elementId)
        {
            FieldBindSchema fieldSchema = Schame.FieldBinds.FindItem(elementId);

            if (IsDataBindElement(fieldSchema))
            {
                return GetElementDataValue(fieldSchema);
            }

            if (Page.StringElements.ContainsKey(elementId))
                return Page.StringElements[elementId];
            throw new E_WbdlPageNotHaveFieldElement(elementId);
        }

        private string GetElementDataValue(FieldBindSchema fieldSchema)
        {
            if (!DataSet.Tables.Contains(fieldSchema.TableId))
                throw new E_WbdlDataSourceNotFindTable(fieldSchema.TableId);

            DataTable dataTable = DataSet.Tables[fieldSchema.TableId];

            if (!dataTable.Columns.Contains(fieldSchema.FieldId))
                throw new E_WbdlDatasourceNotFindField(fieldSchema.TableId + "." + fieldSchema.FieldId);

            int RowIndex = dataSource.Tables[fieldSchema.TableId].RowIndex;
            if (RowIndex > -1)
            {
                DataRow dataRow = dataTable.Rows[RowIndex];
                return dataRow[fieldSchema.FieldId].ToString();
            }
            return "";
        }

        internal void BuildRequestDataBodyWithTable(WbapDataBody wbapElementBinds, string tableName, DataRow[] dataRows)
        {
            PageDataForm form = Page.PageForms.FindForm(tableName);
            //if (form == null) InitFieldBinds();
            //form = Page.PageForms.FindForm(fieldName);
            if (form != null) BuildRequestDataBodyWithForm(wbapElementBinds, form, dataRows);
            PageDataList list = Page.PageLists.FindListByTableName(tableName);

            if (list != null)
            {
                WbapList wbapList = CreatePageList(list.ListSchema);
                if (dataRows != null)
                    FillWbapList(wbapList, dataRows);
                wbapElementBinds.Add(wbapList.Id + WbapList.TypeMarker, wbapList);

            }
        }

        private void BuildRequestDataBodyWithForm(WbapDataBody wbapElementBinds, PageDataForm form, DataRow[] dataRows)
        {
            DataColumn[] keyCols = form.DataTable.PrimaryKey;

            for (int i = 0; i < keyCols.Length; i++)
            {
                string value = "";
                if (dataRows != null && dataRows.Length > 0)
                    value = dataRows[dataRows.Length - 1][keyCols[i]].ToString();
                wbapElementBinds.Add(form.TableName + "_" + keyCols[i].ColumnName + WbdlConst._Key, value);
            }

            for (int i = 0; i < form.Fields.Count; i++)
            {
                PageDataField field = form.Fields[i];
                string value = "";
                if (dataRows != null && dataRows.Length > 0)
                    value = dataRows[dataRows.Length - 1][field.DataColumn].ToString();
                wbapElementBinds.Add(field.ElementId, value);
            }

        }

        internal void BuildRequestDataBodyWithDataSet(WbapDataBody wbapElementBinds)
        {
            foreach (TableSource table in this.dataSource.Tables)
            {
                BuildRequestDataBodyWithTable(wbapElementBinds, table.TableName, null);
            }
        }

        internal void FillDataBodyWithDataRows(WbapDataBody wbapDataBody, DataRow[] dataRows)
        {
            List<DataTable> dataTables = new List<DataTable>();
            foreach (DataRow row in dataRows)
            {
                if (!dataTables.Contains(row.Table))
                    dataTables.Add(row.Table);
            }

            foreach (DataTable table in dataTables)
            {
                BuildRequestDataBodyWithTable(wbapDataBody, table.TableName, dataRows);
            }

        }

        internal void FillDataBodyWithDataSet(WbapDataBody wbapDataBody)
        {

            foreach (DataTable table in DataSet.Tables)
            {
                BuildRequestDataBodyWithTable(wbapDataBody, table.TableName, table.Select());
            }

        }


        internal void FillDataBodyWithHashRows(WbapDataBody wbapDataBody, Dictionary<string, DataRow[]> hashRows)
        {
            foreach (KeyValuePair<string, DataRow[]> tableRows in hashRows)
            {
                FillDataBodyWithDataRows(wbapDataBody, tableRows.Value);
            }
        }
    }//PageController类结束
}
