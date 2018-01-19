using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.security;

namespace xbase.umc.attributes
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class WboMethodAttr : System.Attribute
    {
        public string Title = "";
        public string Description = "";
        public PermissionTypes PermissionTypes = PermissionTypes.Read;
        public WboMethodAttr()
        {
        }
    }

}
