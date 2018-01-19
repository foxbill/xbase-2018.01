using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.local;

namespace xbase.olap
{
    public class OlapLevelOverException : Exception
    {
        public OlapLevelOverException()
            : base(Lang.OlapLevelOver)
        {

        }
    }
}
