using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.Exceptions
{
    public class XUserException : XException
    {
        public XUserException(string msg)
            : base(msg)
        {

        }
    }
}
