using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.umc;
using xbase.umc.attributes;

namespace xbase.Validation
{
    /// <summary>
    /// 校验器抽象类
    /// </summary>
    public abstract class BaseValidator : Validator
    {
        private string errHint = "输入校验不合法";

        #region Validator 成员

        /// <summary>
        ///错误提示信息
        /// </summary>
        [WboPropertyAttr(Title = "错误提示", Description = "如果输入不合法的，提示内容")]
        public string ErrHint
        {
            get
            {
                return errHint;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    errHint = value;
            }
        }

        /// <summary>
        /// 检查值正确性，抽象方法 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract bool Check(string value);
        #endregion
    }
}
