using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.Exceptions
{
    public class EAppNotStart : XException
    {
        public EAppNotStart(string msg) : base(msg) { }
    }
}
