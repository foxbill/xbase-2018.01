using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.data.Exceptions
{
    public class ENoColumnException:Exception
    {
        public ENoColumnException(string msg)
            : base(msg)
        {
        }
    }
}
