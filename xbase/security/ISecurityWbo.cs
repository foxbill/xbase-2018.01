using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.security
{
    public interface ISecurityWbo
    {
        IUserContext UserContext{get;set;}
    }
}
