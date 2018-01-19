using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.umc
{
    public class PropertySchema : Schema
    {
        private Dictionary<string, object> options;

        public Dictionary<string, object> Options
        {
            get { return options; }
            set { options = value; }
        }
    }
}
