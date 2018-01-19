using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase;
using xbase.Exceptions;

namespace xbase.data
{
    public class SearchParamNoFind : XException
    {
        public SearchParamNoFind(string param)
            : base("查询参数不存在："+param)
        {
        }
    }
}
