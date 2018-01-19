using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.Exceptions
{
    public class NoConfigException:XException
    {
        public NoConfigException(string msg)
            : base(msg)
        {
        }
    }
}
