using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.data
{
    public class TableDef
    {
        // private string name;
        /// <summary>
        /// 数据表名
        /// </summary>
        public string Name { get; set; }

        public string OldName { get; set; }

        private List<FieldDef> fieldDefs;
        /// <summary>
        /// 表字段集合
        /// </summary>
        public List<FieldDef> FieldDefs
        {
            get { return fieldDefs; }
            set { fieldDefs = value; }
        }



        private List<FieldDef> mainKeys;
        /// <summary>
        /// 表主健集合
        /// </summary>
        public List<FieldDef> MainKeys
        {
            get { return mainKeys; }
            set { mainKeys = value; }
        }

        public TableDef()
        {
            FieldDefs = new List<FieldDef>();
            MainKeys = new List<FieldDef>();
        }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
