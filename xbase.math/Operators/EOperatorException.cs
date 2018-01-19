using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.math.Operators
{
    public class EOperatorException:xbase.Exceptions.XUserException
    {
        public EOperatorException(string msg)
            : base(msg)
        {
        }
    }
}
