using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace xbase.data.db
{
    public class DataTypeMap
    {
        public string Caption { get; set; }
        public Type CType { get; set; }
        public DbType DbType { get; set; }
    }
}
