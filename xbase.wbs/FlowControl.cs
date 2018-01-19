using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.wbs
{
    public interface IFlowControl
    {
        FlowStatus CheckDo();
        bool IsIterate { get; }
    }



}
