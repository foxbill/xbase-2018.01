using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.Validation
{
    /// <summary>
    /// 校验接口类型
    /// </summary>
    public interface Validator
    {
        /// <summary>
        /// 错误提示信息
        /// </summary>
        string ErrHint { get; set; }
        /// <summary>
        /// 检查值是否合法
        /// </summary>
        /// <param name="value">要检查的值</param>
        /// <returns>如果合法返回true，否则返回false</returns>
        bool Check(string value);
    }
}
