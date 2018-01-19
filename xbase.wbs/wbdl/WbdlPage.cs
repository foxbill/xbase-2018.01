using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using xData;
using System.Reflection;
using xBase;
using wbs.wbap;
using System.Data;

namespace wbs
{
    public class E_wbdlPageTableNotDefine : XException { }
    public class E_WbdlPageNotFindField : XException { public E_WbdlPageNotFindField(string msg) : base(msg) { } }

    public enum WbdlConst
    {
        _Key
    }

    /// <summary>
    /// 元素事件实体模型
    /// </summary>
    public class PageEvent : BizObject
    {
        //private string script;
        private string eventName;
        private RequestEnv requestEnv;

        public RequestEnv RequestEnv
        {
            get { return requestEnv; }
            set { requestEnv = value; }
        }

        /// <summary>
        /// 事件名称
        /// </summary>
        public string EventName
        {
            get { return eventName; }
            set { eventName = value; }
        }

    }


    public class PageDataField
    {
        private FieldBindSchema fieldSchema;
        private DataColumn dataColumn;
        private string value;

        DataSet ds;

        private bool isKey = false;

        public PageDataField(FieldBindSchema fieldSchema)
        {
            this.fieldSchema = fieldSchema;
        }

        public string ElementId
        {
            get { return fieldSchema.Id; }
        }
        public string TableId
        {
            get { return fieldSchema.TableId; }
            
        }

        public string FieldId
        {
            get { return fieldSchema.FieldId; }
        }

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public bool IsKey
        {
            get { return isKey; }
            set { isKey = value; }
        }

        public DataColumn DataColumn
        {
            get { return dataColumn; }
            set { dataColumn = value; }
        }
    }

    /// <summary>
    ///数据元素表单和Table对应 
    /// </summary>
    public class PageDataForm
    {
        private List<PageDataField> fields = new List<PageDataField>();
     //   private string fieldName;
        private DataTable dataTable;
        private Dictionary<string, string> keyValues=new Dictionary<string,string>();

        public DataTable DataTable
        {
            get { return dataTable; }
            set { dataTable = value; }
        }

        public PageDataForm(string tableName)
        {
        }

        public List<PageDataField> Fields
        {
            get { return fields; }
            set { fields = value; }
        }

        public string TableName
        {
            get { return dataTable.TableName; }
        }

        public string this[string fieldName]
        {
            get
            {
                PageDataField field = FindField(fieldName);
                if (field == null)
                    throw new E_WbdlPageNotFindField(TableName + "." + fieldName);
                return field.Value;
            }
        }

        public string this[int index]
        {
            get
            {
                return fields[index].Value;
            }
        }

        public PageDataField FindField(string fieldName)
        {
            foreach (PageDataField field in fields)
            
                if (field.FieldId.Equals(fieldName,StringComparison.OrdinalIgnoreCase)) return field;
            
            return null;

        }

        public void CacheValue(string fieldName, string fieldValue)
        {
            PageDataField field = FindField(fieldName);
            if (field == null)
                throw new E_WbdlPageNotFindField(TableName + "." + fieldName);
            field.Value = fieldValue;
        }

        public void UpdateCache()
        {

            string filterStr = "";
            bool isNewRow = false;
            foreach (KeyValuePair<string, string> keyValue in keyValues)
            {
                if (string.IsNullOrEmpty(keyValue.Value))
                {
                    isNewRow = true;
                }
                filterStr += keyValue.Key + "='" + keyValue.Value + "' and ";
            }

            DataRow row=null;

            if (!isNewRow)
            {
                if (filterStr.EndsWith(" and "))
                    filterStr = filterStr.Substring(0, filterStr.Length - " and ".Length);

                DataRow[] rows = dataTable.Select(filterStr);

                if (rows.Length > 0)
                    row = rows[0];
                else
                {
                    isNewRow = true;
                }
            }
            if (isNewRow)
            {
                row = dataTable.NewRow();
                dataTable.Rows.Add(row);

            }
            if (!isNewRow) row.BeginEdit();
            foreach (PageDataField field in fields)
            {
                row[field.DataColumn.ColumnName] = field.Value;
            }
            if (!isNewRow) row.EndEdit();

        }

        internal void PutKeyValue(string elementId, string value)
        {
            string fieldName = elementId.Replace(TableName+"_", "");
            fieldName = fieldName.Replace(WbdlConst._Key.ToString(), "");
            keyValues.Add(fieldName, value);
        }
    }

    public class PageDataList
    {

        private string tableId;
        private List<PageDataField> fields = new List<PageDataField>();
        private DataTable dataTable;
        private DataListBindSchema listSchema;

        public DataListBindSchema ListSchema
        {
            get { return listSchema; }
            set { listSchema = value; }
        }

        public PageDataList(DataListBindSchema listSchema)
        {
            this.listSchema = listSchema;
            if (listSchema.Columns.Count > 0)
            {
                tableId = listSchema.Columns[0].TableId;
            }
        }
        public DataTable DataTable
        {
            get { return dataTable; }
            set { dataTable = value; }
        }
        public string ElementId
        {
            get { return listSchema.Id; }
        }

        public string TableId
        {
            get { return tableId; }
            set { tableId = value; }
        }

        public List<PageDataField> Fields
        {
            get { return fields; }
            set { fields = value; }
        }
    }

    public class PageForms : List<PageDataForm>
    {
        public PageDataForm FindForm(string tableName)
        {
            foreach (PageDataForm form in this)
            {
                if (form.TableName.Equals(tableName))
                {
                    return form;
                }
            }
            return null;
        }

        internal PageDataForm FindFormByKeyElement(string elementId)
        {
            string tableName = elementId.Substring(0, elementId.IndexOf('_'));
            return this.FindForm(tableName);
        }
    }

    public class PageLists : Dictionary<string, PageDataList>
    {
        public PageDataList FindListByTableName(string tableName)
        {
            foreach (PageDataList list in this.Values)
            {
                if (list.TableId.Equals(tableName, StringComparison.OrdinalIgnoreCase))
                    return list;
            }
            return null;
        }
    }
    /// <summary>
    /// WbdlPage实体模型
    /// </summary>    
    public class WbdlPage : BizObject
    {
        private Dictionary<string, string> stringElements = new Dictionary<string, string>();
        private PageLists pageLists = new PageLists();
        private PageForms pageForms = new PageForms();
        private BizObjectList<PageEvent> events = new BizObjectList<PageEvent>();
        private Dictionary<string, ActionBroker> actions = new Dictionary<string, ActionBroker>();

        public PageLists PageLists
        {
            get { return pageLists; }
            set { pageLists = value; }
        }

        public PageForms PageForms
        {
            get { return pageForms; }
        }

        /// <summary>
        /// ElementEvent集合
        /// </summary>
        public BizObjectList<PageEvent> Events
        {
            get { return events; }
            set { events = value; }
        }

        /// <summary>
        /// 活动集合
        /// </summary>
        public Dictionary<string, ActionBroker> Actions
        {
            get { return actions; }
            set { actions = value; }
        }

        public Dictionary<string, string> StringElements
        {
            get { return stringElements; }
            set { stringElements = value; }
        }

    }

}
