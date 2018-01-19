using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.umc;
using xbase.umc.attributes;

namespace xbase.Validation
{
    [WboAttr(Title = "值范围校验", Description = "校验值范围在开始值和结束值之间")]
    public class ScopeValidator : BaseValidator
    {
        private double start;

        [WboPropertyAttr(Title = "开始值", Description = "")]
        public double Start
        {
            get { return start; }
            set { start = value; }
        }
        private double end;

        [WboPropertyAttr(Title = "结束值", Description = "")]
        public double End
        {
            get { return end; }
            set { end = value; }
        }

        #region Validation 成员
        public override bool Check(string value)
        {
            return !string.IsNullOrEmpty(value);
        }
        #endregion
    }
}
