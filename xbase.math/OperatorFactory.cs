using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using xbase.Exceptions;
using System.IO;
using xbase.math.Operators;

namespace xbase.math
{
    /// <summary>
    /// 操作符类工厂
    /// </summary>
    public static class OperatorFactory
    {
        private static OperatorRegList _operatorRegList;
        private static Dictionary<string, IOperator> operatorBuf = new Dictionary<string, IOperator>();


        public static void InitualFile(string regFile)
        {
            if (!File.Exists(regFile))
                RegistDefaultOperator(regFile);
            else
                _operatorRegList = SchemaFile.LoadSchema<OperatorRegList>(regFile);
        }

        private static void RegistOperator(string symbol, Type operatorClass, int level)
        {
            OperatorSchema oper = new OperatorSchema();
            oper.Id = symbol;
            oper.Level = level;
            oper.AssemblyName = operatorClass.Assembly.FullName;
            oper.ClassName = operatorClass.FullName;
            _operatorRegList.Operators.Add(oper);
        }

        public static void RegistDefaultOperator(string regFile)
        {
            _operatorRegList = new OperatorRegList();

            RegistOperator("||", typeof(Or), 1);
            RegistOperator("&&", typeof(And), 1);

            RegistOperator(">", typeof(GreaterThan), 2);
            RegistOperator("<", typeof(LessThan), 2);
            RegistOperator("<=", typeof(GE), 2);
            RegistOperator(">=", typeof(LE), 2);
            RegistOperator("!=", typeof(NotEqualTo), 2);
            RegistOperator("==", typeof(EqualTo), 2);
            RegistOperator("=", typeof(EqualTo), 2);

            RegistOperator("+", typeof(Addition), 3);
            RegistOperator("-", typeof(Subtraction), 3);

            RegistOperator("*", typeof(Multiplication), 4);
            RegistOperator("/", typeof(Division), 4);

            SchemaFile.SaveSchema<OperatorRegList>(_operatorRegList, regFile);
        }

        public static IOperator GetOperator(string symbol)
        {
            IOperator iop;
            if (operatorBuf.ContainsKey(symbol))
                iop = operatorBuf[symbol];
            else
            {
                iop = CreateOperator(symbol);
                operatorBuf.Add(symbol, iop);
            }

            return iop;
        }

        private static IOperator CreateOperator(string symbol)
        {
            OperatorSchema opReg = _operatorRegList.Operators.GetItem(symbol);
            Assembly assembly = Assembly.Load(opReg.AssemblyName);
            object obj = obj = assembly.CreateInstance(opReg.ClassName, true);
            if (obj is IOperator)
                return obj as IOperator;
            else
                throw new XException("操作符:" + symbol + "注册的操作符类：" + opReg.ClassName + "不是IOperator接口");
        }

        /// <summary>
        /// 检查表达式首个字符开始的位置是否是运算符，是则返回这个运算符，否则返回空
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static ExpNode CheckOperatorSing(string exp)
        {
            ExpNode expNode = null;

            foreach (OperatorSchema opReg in _operatorRegList.Operators)
            {
                string sign = opReg.Id;
                if (exp.StartsWith(sign))
                {
                    if (expNode == null)
                    {
                        expNode = new ExpNode(sign, ExpNodeType.Operator, opReg.Level);
                    }
                    else if (sign.Length > expNode.Text.Length)
                        expNode = new ExpNode(sign, ExpNodeType.Operator, opReg.Level);
                }
            }
            return expNode;
        }



    }
}
