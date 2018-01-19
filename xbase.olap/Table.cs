using System;
using System.Collections.Generic;
using System.Text;

namespace xbase.olap
{
    public class Table
    {
        /// <summary>
        /// 名称，用于访问Table实例
        /// </summary>
        public string name { get; set; }
        
        /// <summary>
        /// 别名，表述数据库中对应的表明，如果没有指定则默认为name的值
        /// </summary>
        public string alias { get; set; }

        /// <summary>
        /// 数据库链接名，如果没有指定，则采用默认连接
        /// </summary>
        public string connection { get; set; }

        /// <summary>
        /// 过滤条件，Sql语句的where 部分
        /// </summary>
        public string SQL { get; set; }
    }
}
