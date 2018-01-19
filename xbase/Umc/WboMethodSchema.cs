using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.security;

namespace xbase.umc
{
    public class WboMethodSchema : Schema
    {
        public PermissionTypes PermissionTypes = PermissionTypes.Read;
        public SchemaList<WboMethodParamSchema> Params = new SchemaList<WboMethodParamSchema>();
        public string ReturnType;
    }
}
