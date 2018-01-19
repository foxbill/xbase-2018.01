using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.data
{
    public enum FilterOps
    {
        //不过滤
        nofilter,
        //包含
        contains,
        //等于
        equal,
        //不等于
        notequal,
        //用xx开始
        beginwith,
        //用xx结束
        endwith,
        //小于
        less,
        //小于等于
        lessorequal,
        //大于
        greater,
        //大于等于
        greaterorequal,
        //为空
        isnull,
        //不为空
        notnull
    }
}
