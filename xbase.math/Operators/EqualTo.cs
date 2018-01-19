using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.math.Operators
{
    /// <summary>
    /// 等于操作符
    /// </summary>
    public class EqualTo : IOperator
    {
        private string leftOperand;
        private string rightOperand;

        public EqualTo()
        {
        }

        public EqualTo(string leftOperand, string rightOperand)
        {
            this.leftOperand = leftOperand;
            this.rightOperand = rightOperand;
        }

        #region IOperator 成员

        public string LeftOperand
        {
            get
            {
                return leftOperand;
            }
            set
            {
                leftOperand = value;
            }
        }

        public string RightOperand
        {
            get
            {
                return rightOperand;
            }
            set
            {
                 this.rightOperand=value;
            }
        }

        public object Eval()
        {
            try
            {
                return (Convert.ToDecimal(leftOperand) == Convert.ToDecimal(rightOperand));
            }
            catch
            {
                throw new EOperatorException("加法运算器错误,做操作数:" + leftOperand + "  右操作数:" + rightOperand);
            }

        }

        #endregion
    }

}
