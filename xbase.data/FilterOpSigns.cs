using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.data
{
    public static class FilterOpSigns
    {
        static Dictionary<FilterOps, string> _signs = new Dictionary<FilterOps, string>()
        {
            {FilterOps.contains," Like "},
            {FilterOps.equal," = "},
            {FilterOps.notequal," <> "},
            {FilterOps.beginwith," like "},
            {FilterOps.endwith," like "},
            {FilterOps.greater," > "},
            {FilterOps.greaterorequal," >= "},
            {FilterOps.less," < "},
            {FilterOps.lessorequal," <= "},
            {FilterOps.isnull," is null "},
            {FilterOps.notnull," is not null "}
        };

        public static string getSign(FilterOps op)
        {
            return _signs[op];
        }

        public static FilterOps parse(string op)
        {
            return (FilterOps)Enum.Parse(typeof(FilterOps), op, true);
        }

        public static string getSign(string op)
        {
            FilterOps fop = parse(op);
            return getSign(fop);
        }
    }
}
