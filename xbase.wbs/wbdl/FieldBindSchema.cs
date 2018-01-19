using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase;

namespace xbase.wbs.wbdl
{
    /// <summary>
    /// 元素数据实体
    /// </summary>
    public class FieldBindSchema : Schema
    {
        private string tableId;
        private string displayName;
        private string fieldId;
        private string formula;
        /// <summary>
        /// 表名称
        /// </summary>
        public string TableId
        {
            get { return tableId; }
            set { tableId = value; }
        }
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldId
        {
            get { return fieldId; }
            set { fieldId = value; }
        }
        /// <summary>
        /// 显示名
        /// </summary>
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }
        /// <summary>
        /// 公式
        /// </summary>
        public string Formula
        {
            get { return formula; }
            set { formula = value; }
        }
    }
}
