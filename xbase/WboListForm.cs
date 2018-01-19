using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase
{
    public class WboListForm
    {
        public Dictionary<string, WboFieldDef> FieldInfos = new Dictionary<string, WboFieldDef>();
        public List<object> WboList = new List<object>();
    }
}
