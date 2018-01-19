using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.math
{
    /// <summary>
    /// 中缀表达式生成器
    /// </summary>
    public class InfixExpress
    {
        private enum Expect { Operand, Operator }

        private string strExp;

        public InfixExpress(string strExp)
        {
            this.strExp = strExp;
        }


        private int ReadOperator(string readStr, List<ExpNode> infixList, string operand)
        {
            ExpNode node = OperatorFactory.CheckOperatorSing(readStr);
            if (node != null)
            {
                operand = operand.Trim();
                if (!string.IsNullOrEmpty(operand))
                    infixList.Add(new ExpNode(operand, ExpNodeType.Operand, 0));

                infixList.Add(node);
                return node.Text.Length;
            }
            return 0;
        }

        private int ReadFunction(string readStr, List<ExpNode> infixList, string operand)
        {
            int bracketCount = 0;
            for (int i = 0; i < readStr.Length; i++)
            {
                char c = readStr[i];
                if (c == '(')
                    bracketCount++;
                if (c == ')')
                    bracketCount--;
                operand += c;
                if (bracketCount == 0)
                {
                    infixList.Add(new ExpNode(operand, ExpNodeType.Method, 0));
                    return i + 1;
                }

            }
            throw new EExpressException("表达式函数格式错误，缺少货号')'," + operand);
        }

        /// <summary>
        /// 从字符串创建中缀表达式
        /// </summary>
        /// <param name="strExp"></param>
        /// <returns></returns>
        public List<ExpNode> Build()
        {
            Expect expect = Expect.Operand;
            List<ExpNode> ret = new List<ExpNode>();
            string operand = "";
            int i = 0;

            while (i < this.strExp.Length)
            {
                char c = strExp[i];

                if (c == '(')
                {
                    if (!operand.Trim().Equals(""))
                    {
                        if (!char.IsNumber(operand[0]))
                        {
                            i += ReadFunction(strExp.Substring(i), ret, operand);
                            operand = "";
                            continue;
                        }
                        else
                            throw new EExpressException("表达式中含有非法的函数名" + operand + c);

                    }
                    else
                    {
                        ret.Add(new ExpNode(c + "", ExpNodeType.LeftBracket, 0));
                        i++;
                        continue;
                    }
                }

                if (c == ')')
                {
                    operand = operand.Trim();
                    if (!string.IsNullOrEmpty(operand))
                    {
                        ret.Add(new ExpNode(operand, ExpNodeType.Operand, 0));
                        operand = "";
                    }
                    ret.Add(new ExpNode(c + "", ExpNodeType.RightBracket, 0));
                    i++;
                    continue;
                }

                switch (expect)
                {
                    case Expect.Operand:
                        {
                            operand += c;
                            expect = Expect.Operator;
                            i++;
                            break;
                        }
                    case Expect.Operator:
                        {
                            int len = ReadOperator(strExp.Substring(i), ret, operand);
                            if (len > 0)
                            {
                                operand = "";
                                expect = Expect.Operand;
                                i += len;
                            }
                            else
                            {
                                operand += c;
                                i++;
                            }
                            break;
                        }
                }

            }
            if (!string.IsNullOrEmpty(operand))
                ret.Add(new ExpNode(operand, ExpNodeType.Operand, 0));
            return ret;
        }

    }
}
