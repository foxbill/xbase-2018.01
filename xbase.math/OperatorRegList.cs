using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase;

namespace xbase.math
{
    /// <summary>
    /// 操作符集合
    /// </summary>
    public class OperatorRegList : Schema
    {
        private SchemaList<OperatorSchema> operators = new SchemaList<OperatorSchema>();

        public SchemaList<OperatorSchema> Operators
        {
            get { return operators; }
        }
    }

    /// <summary>
    /// 操作符
    /// </summary>
    public class OperatorSchema : Schema
    {
        private string assemblyName;
        private string className;
        private string method;
        private int level;


        public string AssemblyName
        {
            get { return assemblyName; }
            set { assemblyName = value; }
        }
        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }
        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        public string Method
        {
            get { return method; }
            set { method = value; }
        }
    }
}
