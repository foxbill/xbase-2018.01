using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace xbase.data
{
    public class CommandSchema
    {
        public string CommandText;
        public CommandType CommandType = CommandType.Text;
        public SchemaList<ParameterSchema> QueryParams;
    }
}
