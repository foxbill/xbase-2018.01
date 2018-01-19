using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.Exceptions
{
    public class ESchemaFileException : Exception
    {
        private string fileId;
        private string objType;

        public ESchemaFileException(string msg, Schema schema)
            : base("配置文件错误：" + schema.Title + "(" + schema.Id + ")," + msg)
        {
        }
        public ESchemaFileException(string msg)
            : base(msg)
        {

        }
    }
}
