using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.Exceptions
{
    public class E_UmcCheckParamError : XException { public E_UmcCheckParamError(string msg) : base(msg) { } }
    public class E_UmcNoMatchObjectType : XException { public E_UmcNoMatchObjectType(string msg) : base(msg) { } }
    public class E_UmcNotFindRegObjcect : XException { public E_UmcNotFindRegObjcect(string msg) : base(msg) { } }
    public class E_UmcGetSessionObjectError : XException { public E_UmcGetSessionObjectError(string msg) : base(msg) { } }
}
