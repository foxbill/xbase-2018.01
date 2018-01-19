using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.Exceptions;

namespace xbase.umc
{
    public class WboProxyFactory
    {
        public static WboProxy getWboProxy(WboSchema wboSchema)
        {
            switch (wboSchema.AssemblyCategory)
            {
                case AssemblyCategory.DotNet:
                    return new DotNetWboProxy(wboSchema);
                case AssemblyCategory.WebService:
                    return new DotNetWboProxy(wboSchema);
                default:
                    throw new XException("系统目前不支持，这种程序类型");
            }
        }

    }
}
