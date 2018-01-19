using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.olap
{
    public class Level
    {
        public string name { get; set; }

        /// <summary>
        /// 级别Key字段名，与KeyExpression有相同的意义
        /// </summary>
        public string column { get; set; }

        /// <summary>
        /// 显示成员名称列，如果没有指定则使用column指定的主键列
        /// xxxDate
        /// </summary>
        public string nameColumn { get; set; }

        /// <summary>
        /// SQL expression used to compute the name of a member, in lieu of Level.nameColumn.
        /// year(xxxxxdate)
        /// </summary>
        public string nameExpression { get; set; }

        /// <summary>
        /// 成员的数据类型
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 父子层次中的父级列字段。
        /// </summary>
        public string parentColumn { get; set; }

        /// <summary>
        /// 在父子层次中的顶级父级字段值，缺省为空
        /// </summary>
        public string nullParentValue { get; set; }

        public string ordinalColumn { get; set; }

        public string uniqueMembers { get; set; }

        public List<Property> properties { get; set; }
    }
}
