using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase
{
    public class WboFieldDef
    {
        public string Description;
        public string Title;
        public Dictionary<object, string> Options = new Dictionary<object, string>();
    }
}
