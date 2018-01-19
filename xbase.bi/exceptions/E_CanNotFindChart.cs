using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.Exceptions;

namespace xbase.bi.exceptions
{
    public class E_CanNotFindChart : XException
    {
        public E_CanNotFindChart(string msg) : base("不能发现指定的Chart:"+msg) { }
    }
}
