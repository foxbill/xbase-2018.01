using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.Exceptions;

namespace xbase.Validation
{
    public class EValidateException : XException
    {
        public EValidateException(string msg) : base(msg) { }
    }
}
