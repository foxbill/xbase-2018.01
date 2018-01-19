using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.data
{
    public class FilterRule
    {
        public string field;
        //public string op =FilterOps.contains.ToString();
        public string op = FilterOps.nofilter.ToString();
        public string value;
    }
}
