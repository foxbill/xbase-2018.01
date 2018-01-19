using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.math
{
    public interface IOperator
    {
        string LeftOperand { get; set; }
        string RightOperand { get; set; }
        object Eval();
    }
}
