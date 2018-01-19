using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.data
{
    public class FilterInput
    {
        private string _type = "text";

        public string field { get; set; }
        public FilterOption options { get; set; }

        public static string[] TextOP = null;

        public static string[] BigTextOP = new string[] 
        {
            FilterOps.contains.ToString(),
            FilterOps.equal.ToString(),
            FilterOps.notequal.ToString(),
            FilterOps.beginwith.ToString(),
            FilterOps.endwith.ToString(),
            FilterOps.less.ToString(),
            FilterOps.lessorequal.ToString(),
            FilterOps.greater.ToString(),
            FilterOps.greaterorequal.ToString(),
            FilterOps.isnull.ToString(),
            FilterOps.notnull.ToString()
        };

        public static string[] NumberOP = new string[] 
        {
            FilterOps.equal.ToString(),
            FilterOps.less.ToString(),
            FilterOps.lessorequal.ToString(),
            FilterOps.greater.ToString(),
            FilterOps.greaterorequal.ToString(),
            FilterOps.isnull.ToString(),
            FilterOps.notnull.ToString()
        };



        public string type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string[] op = new string[] 
        {
            "contains",
            "equal",
            "notequal",
            "beginwith",
            "endwith",
            "less",
            "lessorequal",
            "greater",
            "greaterorequal"
        };

    }
}
