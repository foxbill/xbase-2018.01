using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.security;
using xbase.umc;
using System.Web;

namespace xbase
{
    public interface IHttpWbo
    {
        void setContext(Umc umc);
    }
}
