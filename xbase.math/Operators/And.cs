using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.math.Operators
{
    /// <summary>
    /// 布尔并操作符
    /// </summary>
    public class And : IOperator
    {
        private string leftOperand;
        private string rightOperand;

        public And()
        {
        }
        public And(string leftOperand, string rightOperand)
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
                return (Convert.ToBoolean(leftOperand) && Convert.ToBoolean(rightOperand));
            }
            catch
            {
                throw new EOperatorException("加法运算器错误,做操作数:" + leftOperand + "  右操作数:" + rightOperand);
            }

        }

        #endregion
    }
}
