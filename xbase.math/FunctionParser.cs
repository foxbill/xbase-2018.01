using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace xbase.math
{
    public class FunctionParser
    {
        public string FunctionString;
        private string methodName;
        private string objectName;

        private List<string> parameters = new List<string>();


        public string Name
        {
            get { return methodName; }
        }
        public List<string> Parameters
        {
            get { return parameters; }
        }

        public FunctionParser()
        {

        }

        public FunctionParser(string functionString)
        {
            this.FunctionString = functionString;
            this.Parse();
        }

        private void ParseParams(string funcBody)
        {
            string param = "";
            int bc = 0;

            foreach (char c in funcBody)
            {
                switch (c)
                {
                    case '(':
                        bc++;
                        param += c;
                        break;
                    case ')':
                        bc--;
                        param += c;
                        break;
                    case ',':
                        if (bc > 0)
                        {
                            param += c;
                        }
                        else
                        {
                            parameters.Add(param);
                            param = "";
                        }
                        break;
                    default:
                        param += c;
                        break;
                }
            }

            if (param.Trim() != "")
            {
                parameters.Add(param);
            }
        }

        private void Parse()
        {
            string function = FunctionString.Trim();
            this.methodName = function.Substring(0, function.IndexOf('('));
            string funcBody = function.Substring(function.IndexOf('('));
            funcBody = funcBody.Trim('(', ')');
            ParseParams(funcBody);
        }
        /// <summary>
        /// 执行函数
        /// </summary>
        /// <returns></returns>
        public object Invoke()
        {

            object[] pValues = null;

            MethodInfo mi = TypeUtility.GetMatchMethod(typeof(Math), this.methodName, this.parameters.ToArray(), out pValues);
            if (mi == null)
                throw new EExpressException("函数表达式，不能被计算:" + this.FunctionString);

            return mi.Invoke(null, pValues);

        }


    }
}
