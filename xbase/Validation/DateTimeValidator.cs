using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.umc;
using xbase.umc.attributes;

namespace xbase.Validation
{
    /// <summary>
    /// 日期校验
    /// </summary>
    [WboAttr(Title = "日期格式校验", Description = "校验合法的日期格式")]
    public class DateTimeValidator : BaseValidator
    {
        #region Validation 成员

        /// <summary>
        /// 确认一个值是非空或非空字符
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool Check(string value)
        {
            DateTime v;
            return DateTime.TryParse(value, out v);
        }

        #endregion
    }
}
