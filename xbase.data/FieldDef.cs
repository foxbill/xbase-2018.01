using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.data
{
    public class FieldDef
    {
        public string OldName { get; set; }

        private string name;
        /// <summary>
        /// 字段名
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string type;

        /// <summary>
        /// 数据类型
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; }
        }


        private int length;
        /// <summary>
        /// 数据长度
        /// </summary>
        public int Length
        {
            get { return length; }
            set { length = value; }
        }

        private int procesion;
        /// <summary>
        /// 数据精度
        /// </summary>
        public int Procesion
        {
            get { return procesion; }
            set { procesion = value; }
        }

        private string alias;
        /// <summary>
        /// 字段别名
        /// </summary>
        public string Alias
        {
            get { return alias; }
            set { alias = value; }
        }


        private string constrains;
        /// <summary>
        /// 约束
        /// </summary>
        public string Constrains
        {
            get { return constrains; }
            set { constrains = value; }
        }

        private bool isIdentity;
        /// <summary>
        /// 是否为标识
        /// </summary>
        public bool IsIdentity
        {
            get { return isIdentity; }
            set { isIdentity = value; }
        }

        private bool isPriKey;
        /// <summary>
        /// 是否主键
        /// </summary>
        public bool IsPriKey
        {
            get { return isPriKey; }
            set { isPriKey = value; }
        }

        private bool isGuid;
        /// <summary>
        /// 是否为唯一
        /// </summary>
        public bool IsGuid
        {
            get { return isGuid; }
            set { isGuid = value; }
        }

        private string defaultValue;
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue
        {
            get { return defaultValue; }
            set { defaultValue = value; }
        }


        private bool isIndex;
        /// <summary>
        /// 是否为索引
        /// </summary>
        public bool IsIndex
        {
            get { return isIndex; }
            set { isIndex = value; }
        }

        public string Title { get; set; }

        public bool IsUnique { get; set; }

        public string Description { get; set; }

        public bool NotNull { get; set; }
    }
}
