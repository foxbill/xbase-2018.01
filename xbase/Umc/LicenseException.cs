using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.local;

namespace xbase.umc
{
    public class LicenseException : Exception
    {
        public LicenseException()
            : base(Lang.LicenseErr)
        {
        }
    }
}
