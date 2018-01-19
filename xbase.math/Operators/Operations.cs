using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace xbase.math
{

    public delegate Object operation(string left, string right);

    /// <summary>
    /// 运算符计算方法
    /// </summary>
    public static class Operations
    {
        private const decimal DETA = 0.0000001m;
        /// <summary>
        /// 加法
        /// </summary>
        /// <param name="x">左边参数</param>
        /// <param name="y">右边参数</param>
        /// <returns>结果</returns>
        public static string Addition(string x, string y)
        {
            try
            {
                return (Convert.ToDecimal(x) + Convert.ToDecimal(y)).ToString();
            }
            catch (Exception ex)
            {
                //   XLog log = new XLog("运算方法", ex.Message);
                //   log.WriteLog();

                throw ex;
            }
        }

        /// <summary>
        /// 减法
        /// </summary>
        /// <param name="x">左边参数</param>
        /// <param name="y">右边参数</param>
        /// <returns>结果</returns>
        public static string Subtraction(string x, string y)
        {
            try
            {
                return (Convert.ToDecimal(x) - Convert.ToDecimal(y)).ToString();
            }
            catch (Exception ex)
            {
                // XLog log = new XLog("运算方法", ex.Message);
                // log.WriteLog();

                throw ex;
            }
        }

        /// <summary>
        /// 乘法
        /// </summary>
        /// <param name="x">左边参数</param>
        /// <param name="y">右边参数</param>
        /// <returns>结果</returns>
        public static string Multiplication(string x, string y)
        {
            try
            {
                return (Convert.ToDecimal(x) * Convert.ToDecimal(y)).ToString();
            }
            catch (Exception ex)
            {
                //XLog log = new XLog("运算方法", ex.Message);
                //  log.WriteLog();

                throw ex;
            }
        }

        /// <summary>
        /// 除法
        /// </summary>
        /// <param name="x">左边参数</param>
        /// <param name="y">右边参数</param>
        /// <returns>结果</returns>
        public static string Devision(string x, string y)
        {
            try
            {
                if (y == "0")
                {
                    y = "1";
                }
                return (Convert.ToDecimal(x) / Convert.ToDecimal(y)).ToString();
            }
            catch (Exception ex)
            {
                //XLog log = new XLog("运算方法", ex.Message);
                //  log.WriteLog();

                throw ex;
            }
        }

        private static bool ParseBoolValue(string x, out bool resultX)
        {
            bool b = bool.TryParse(x, out resultX);
            if (!b) throw new Exception("Calculatoe::Input variable type dismatched");
            return b;
        }

        public static string OR(string x, string y)
        {
            bool resultX;
            bool resultY;
            bool b;
            b = ParseBoolValue(x, out resultX);
            b = ParseBoolValue(x, out resultY);
            return (resultX | resultY).ToString();
        }

        public static string AND(string x, string y)
        {
            bool resultX;
            bool resultY;
            bool b;
            b = ParseBoolValue(x, out resultX);
            b = ParseBoolValue(x, out resultY);
            return (resultX & resultY).ToString();
        }

        public static string NOT(string x, string nouse)
        {
            bool resultX;
            bool b = ParseBoolValue(x, out resultX);
            return (!resultX).ToString();
        }

        public static string Great(string x, string y)
        {
            try
            {
                return ((Convert.ToDecimal(x) - Convert.ToDecimal(y)) > 0).ToString();
            }
            catch (Exception ex)
            {
                //XLog log = new XLog("运算方法", ex.Message);
                //  log.WriteLog();

                throw ex;
            }
        }

        public static string Less(string x, string y)
        {
            try
            {
                return ((Convert.ToDecimal(x) - Convert.ToDecimal(y)) < 0).ToString();
            }
            catch (Exception ex)
            {
                //  XLog log = new XLog("运算方法", ex.Message);
                //   log.WriteLog();

                throw ex;
            }
        }

        public static string Equal(string x, string y)
        {
            try
            {
                //return Object.Equals(x, y);
                return (Math.Abs(Convert.ToDecimal(x) - Convert.ToDecimal(y)) <= DETA).ToString();
            }
            catch (Exception ex)
            {
                //  XLog log = new XLog("运算方法", ex.Message);
                //   log.WriteLog();

                throw ex;
            }
        }


    }

    /// <summary>
    /// 运算符计算方法注册器
    /// </summary>
    public static class OperationRegister
    {
        private static Dictionary<string, operation> operations = new Dictionary<string, operation>();

        /// <summary>
        /// 注册所有运算符的计算方法
        /// </summary>
        public static void RegistOperations()
        {
            try
            {
                operation addition = new operation(Operations.Addition);
                operations.Add("addition", addition);

                operation subtraction = new operation(Operations.Subtraction);
                operations.Add("subtraction", subtraction);

                operation multiplication = new operation(Operations.Multiplication);
                operations.Add("multiplication", multiplication);

                operation devision = new operation(Operations.Devision);
                operations.Add("devision", devision);

                operation or = new operation(Operations.OR);
                operations.Add("or", or);

                operation and = new operation(Operations.AND);
                operations.Add("and", and);

                operation not = new operation(Operations.NOT);
                operations.Add("not", not);

                operation gt = new operation(Operations.Great);
                operations.Add("gt", gt);

                operation les = new operation(Operations.Less);
                operations.Add("les", les);

                operation equ = new operation(Operations.Equal);
                operations.Add("equ", equ);

            }
            catch (Exception ex)
            {
                //XLog log = new XLog("运算方法", ex.Message);
                ///   log.WriteLog();

                throw ex;
            }
        }

        /// <summary>
        /// 获取运算符的计算方法
        /// </summary>
        /// <param name="operationId">运算符Id</param>
        /// <returns>该算符的计算方法</returns>
        public static operation GetOperation(string operationId)
        {
            try
            {
                return operations[operationId];
            }
            catch (Exception ex)
            {
                //XLog log = new XLog("运算方法", ex.Message);
                //  log.WriteLog();

                throw ex;
            }
        }

    }
}
