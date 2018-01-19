using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.data.transfer
{
    public static class XlsTools
    {
        /// <summary>
        /// 满Z向前进位
        /// </summary>
        /// <param name="st">组成XLS列名的字符栈</param>
        private static void carryStatic(Stack<char> st)
        {
            Stack<char> tst = new Stack<char>();
            if (st.Count < 1)
            {
                st.Push('A');
                return;
            }
            while (st.Count > 0)
            {
                char p = st.Pop();
                if ((int)p < (int)'Z')
                {
                    p = Convert.ToChar((int)p + 1);
                    st.Push(p);
                    break;
                }
                if (st.Count < 1)
                {
                    st.Push('A');
                    break;
                }
                tst.Push(p);
            }
            while (tst.Count > 0)
            {
                char p = tst.Pop();
                st.Push(p);
            }
        }

        public static string ToXlsColName(this int value)
        {
            Stack<char> st = new Stack<char>();
            while (value > -1)
            {
                char c = Convert.ToChar(((int)'A') + value);
                if (((int)'A' + value) > (int)'Z')
                {
                    carryStatic(st);
                    value -= ((int)'Z' - (int)'A' + 1);
                }
                else
                {
                    st.Push(c);
                    value -= ((int)c - (int)'A' + 1);
                }
            }

            StringBuilder sb1 = new StringBuilder();
            sb1.Append(st.Reverse().ToArray());
            return sb1.ToString();

        }
    }
}
