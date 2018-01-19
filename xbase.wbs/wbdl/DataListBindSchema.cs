using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using xbase;

namespace xbase.wbs.wbdl
{
    /// <summary>
    /// 多行元素数据实体
    /// </summary>
    public class DataListBindSchema : Schema
    {
        private SchemaList<FieldBindSchema> columns = new SchemaList<FieldBindSchema>();
        private string tableId = null;
        private string dataRow = "";

        public string DataRow
        {
            get { return dataRow; }
            set { dataRow = value; }
        }

        public string TableId
        {
            get { return GetTableId(); }
            set { tableId = value; }
        }

        private string GetTableId()
        {
            if (tableId != null && tableId.Equals(""))
                return tableId;
            if (columns.Count > 0)
                return columns[0].TableId;
            return null;
        }
        /// <summary>
        /// 由多个元素数据组成的集合
        /// </summary>

        [XmlArrayItem("DataListColumn")]
        public SchemaList<FieldBindSchema> Columns
        {
            get { return columns; }
            set { columns = value; }
        }
    }
}
