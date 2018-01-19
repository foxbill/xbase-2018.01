using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.Exceptions;

namespace xbase.math
{
    /// <summary>
    /// 函数构造工厂
    ///1 检查表达式中是否存在函数
    ///2 根据函数名称装载实例并计算出函数的值
    ///3  
    /// </summary>
    public static class FunctionFactory
    {
        /// <summary>
        /// 检查表达式是否以函数开头
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static string CheckFunction(string exp)
        {
            string retFun = "";
            int leftBreckets = 0;
            byte b = (byte)exp[0];

            if (!((b >= 65 && b <= 90) || (b >= 97 && b <= 122) || (b == 95)))
                return null;

            for (int i = 0; i < exp.Length; i++)
            {
                retFun += exp[i];

                b = (byte)exp[i];

                if (OperatorFactory.CheckOperatorSing(exp.Substring(i)) != null && leftBreckets == 0)
                {
                    return null;
                }

                if (exp[i] == '(')
                    leftBreckets++;

                if (exp[i] == ')')
                {
                    if (leftBreckets > 0)
                    {
                        leftBreckets--;
                        if (leftBreckets == 0)
                            return retFun;
                    }
                    else
                    {
                        throw new XException("函数检查错误，遇到不该出现的右括号')'");
                    }
                }


            }

            return null;
        }

        internal static object Invoke(string function)
        {
            FunctionParser parser = new FunctionParser(function);
            return  parser.Invoke();
        }
    }
}
