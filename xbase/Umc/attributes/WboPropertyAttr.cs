using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.umc.attributes
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class WboPropertyAttr : System.Attribute
    {
        public string Title = "";
        public string Description = "";
        public WboPropertyAttr()
        {
        }
    }
}
