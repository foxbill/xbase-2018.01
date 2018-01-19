using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.umc;
using xbase.umc.attributes;

namespace xbase.Validation
{
    [WboAttr(Title = "非空值校验", Description = "验证如果是空值，则输入不合法")]
    public class NullValidator : BaseValidator
    {
        #region Validation 成员
        public override bool Check(string value)
        {
            return !string.IsNullOrEmpty(value);
        }
        #endregion
    }

}
