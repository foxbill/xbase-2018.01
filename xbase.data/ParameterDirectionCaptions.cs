using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace xbase.data
{
    public static class ParameterDirectionCaptions
    {
        private static Dictionary<int, string> _captions = new Dictionary<int, string>()
        {
            // 摘要:
            //     参数是输入参数。

            {(int)ParameterDirection.Input,"输入"},
            //
            // 摘要:
            //     参数是输出参数。
            {(int)ParameterDirection.Output,"输出"},
            //
            // 摘要:
            //     参数既能输入，也能输出。
            {(int)ParameterDirection.InputOutput,"输入输出"},
            //
            // 摘要:
            //     参数表示诸如存储过程、内置函数或用户定义函数之类的操作的返回值。
            {(int)ParameterDirection.ReturnValue,"返回值"}

        };

        public static Dictionary<int, string> captions
        {
            get { return ParameterDirectionCaptions._captions; }
        }

        public static string caption(ParameterDirection direction)
        {
            return _captions[(int)direction];
        }

        public static List<ValueTextPair<int>> valueTextPairs()
        {
            List<ValueTextPair<int>> ret = new List<ValueTextPair<int>>();
            foreach (int dir in _captions.Keys)
            {
                ValueTextPair<int> vtp = new ValueTextPair<int>();
                vtp.value = dir;
                vtp.text = _captions[dir];
                ret.Add(vtp);
            }
            return ret;
        }
    }
}
