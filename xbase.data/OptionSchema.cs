using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.data
{
    public class OptionSchema
    {
        public string DataSource { get; set; }
        public string ValueField { get; set; }
        public string TextField { get; set; }
        public List<ValueTextPair<string>> QueryParams { get; set; }
        public List<ValueTextPair<string>> Options { get; set; }
    }
}
