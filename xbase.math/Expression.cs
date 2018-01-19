using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace xbase.math
{
    public class Expression
    {
        /// <summary>
        /// 字符串表达式
        /// </summary>
        public string Expresstion;

        public Expression()
        {
        }

        public Expression(string expresstion)
        {
            this.Expresstion = expresstion;
        }

        /// <summary>
        /// 将操作数节点处理为操作数
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private object OperandOperation(ExpNode node)
        {
            object result = null;
            switch (node.NodeType)
            {
                case ExpNodeType.Operand:
                    result = node.Text.ToString();
                    break;
                case ExpNodeType.Method:
                    result = FunctionOperation(node.Text);
                    break;
                default:
                    result = 0;
                    break;
            }
            return result;
        }


        public object FunctionOperation(string functionExpress)
        {
            return FunctionFactory.Invoke(functionExpress);
        }

        /// <summary>
        /// 二元计算
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        public object DyadicOperation(ExpNode left, ExpNode right, string symbol)
        {
            IOperator iOp = OperatorFactory.GetOperator(symbol);
            iOp.LeftOperand = OperandOperation(left).ToString();
            iOp.RightOperand = OperandOperation(right).ToString();
            return iOp.Eval();
            //            return iOp.Eval(;
        }


        /// <summary>
        ///求中缀表达式 
        /// </summary>
        /// <param name="exp">字符串计算表达式</param>
        /// <returns></returns>
        public List<ExpNode> GetInfixList(string exp)
        {
            InfixExpress infix = new InfixExpress(exp);
            return infix.Build();
        }


        private bool IsPop(ExpNode op, Stack<ExpNode> operators)
        {

            if (operators.Count == 0)
            {
                return false;
            }

            ExpNode stackTop = operators.Peek();

            if (stackTop.NodeType == ExpNodeType.LeftBracket)
            {
                return false;
            }

            if (stackTop.Level >= op.Level)
                return true;

            return false;

        }

        /// <summary>
        /// 中缀表达式转换到后缀表达式
        /// </summary>
        /// <param name="infixList"></param>
        /// <returns></returns>
        public List<ExpNode> GetPostfixList(List<ExpNode> infixList)
        {
            List<ExpNode> postfixList = new List<ExpNode>();
            Stack<ExpNode> operators = new Stack<ExpNode>();
            ExpNode op;
            //遍历ArrayList类型的中缀表达式
            foreach (ExpNode s in infixList)
            {
                switch (s.NodeType)
                {
                    //为操作数
                    case ExpNodeType.Method:
                    case ExpNodeType.Operand:
                        postfixList.Add(s);
                        break;
                    //为运算符
                    case ExpNodeType.Operator:
                        while (IsPop(s, operators))
                        {
                            postfixList.Add(operators.Pop());
                        }
                        operators.Push(s);
                        break;
                    //为开括号
                    case ExpNodeType.LeftBracket:
                        operators.Push(s);
                        break;
                    //为闭括号
                    case ExpNodeType.RightBracket:
                        while (operators.Count != 0)
                        {
                            op = operators.Pop();
                            if (op.NodeType == ExpNodeType.LeftBracket)
                            {
                                break;
                            }
                            else
                            {
                                postfixList.Add(op);
                            }
                        }
                        break;
                }
            }

            while (operators.Count != 0)
            {
                postfixList.Add(operators.Pop());
            }
            return postfixList;
        }

        /// <summary>
        /// 计算后缀表达式的值
        /// </summary>
        /// <param name="postfixList"></param>
        /// <returns></returns>
        public object Eval(List<ExpNode> postfixList)
        {
            ExpNode num1, num2;
            Stack<ExpNode> num = new Stack<ExpNode>();
            object result = null;
            foreach (ExpNode node in postfixList)
            {
                if (node.NodeType == ExpNodeType.Operator)
                {
                    num2 = num.Pop();
                    num1 = num.Pop();
                    try
                    {
                        result = DyadicOperation(num1, num2, node.Text);
                        ExpNode resultNode = new ExpNode(result.ToString(), ExpNodeType.Operand, 0);
                        num.Push(resultNode);
                    }
                    catch (Exception e)
                    {
                        throw new EExpressException("在计算" + num1.Text + node.Text + num2.Text + "时发生错误," + e.Message);
                    }
                }
                else
                {
                    num.Push(node);
                }
            }

            if (result == null)
            {
                ExpNode node = num.Pop();

                if (node == null)
                    return null;

                result = OperandOperation(node);
            }
            return result;
        }



        /// <summary>
        /// 求出表示式的值
        /// </summary>
        /// <returns></returns>
        public object Eval()
        {
            //ArrayList a2 = new ArrayList();
            List<ExpNode> a1 = GetInfixList(this.Expresstion);
            List<ExpNode> a2 = GetPostfixList(a1);
            return Eval(a2);
        }

    }
}
