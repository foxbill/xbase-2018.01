using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.data
{
    public class ListDataRow : Dictionary<string, string>
    {
        //public Dictionary<string, string> Pk = new Dictionary<string, string>();

        public bool isBlank()
        {
            bool ret = true;
            foreach (string field in this.Keys)
            {
                if (!string.IsNullOrEmpty(this[field]))
                    return false;
            }
            return ret;
        }
    }

    public class SubTable
    {
        public string Name { get; set; }
        public List<ListDataRow> Rows { get; set; }
    }

}
