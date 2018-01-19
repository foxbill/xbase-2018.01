using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace xbase.wbs
{
    public class VarChecker
    {
        private IDictionary<string, string> nameValues;
        private Dictionary<string, string[]> nameAryValues;
        private string varMark;

        public VarChecker(IDictionary<string, string> nameValues)
        {
            this.nameValues = nameValues;
        }

        public VarChecker(IDictionary<string, string> nameValues,string varMark)
        {
            this.varMark = varMark;
            this.nameValues = nameValues;
        }

        public VarChecker(Dictionary<string, string[]> nameAryValues, string varMark)
        {
            this.nameAryValues = nameAryValues;
            this.varMark = varMark;
        }

        public string CapText(Match m)
        {
            string mText = m.ToString();
            if (!string.IsNullOrEmpty(varMark))
                mText = mText.Remove(0, varMark.Length);

            if (nameValues != null && nameValues.ContainsKey(mText))
                return nameValues[mText];

            if (nameAryValues != null && nameAryValues.ContainsKey(mText))
            {
                string[] values = nameAryValues[mText];
                if (values != null && values.Length > 0)
                    return values[0];
                else
                    return null;
            }

            throw new xbase.Exceptions.XUserException("不能发现表达式中的变量" + mText);

        }


    }
}
