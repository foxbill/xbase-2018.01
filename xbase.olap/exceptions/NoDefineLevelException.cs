using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.local;

namespace xbase.olap
{
    public class NoDefineLevelException : Exception
    {
        public NoDefineLevelException()
            : base(Lang.OlapNoDefineLevel)
        {
        }
    }
}
