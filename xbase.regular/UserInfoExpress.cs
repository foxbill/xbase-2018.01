using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace xbase.regular
{
    public sealed  class UserInfoExpress
    {
        public const string EMAIL_EXPRESS = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
        public const string MOBILE_EXPRESS = @"^((13[0-9])|(15[^4,\D])|(18[0,5-9]))\d{8}$";

        public static bool isEmail(string text)
        {
            Regex ret = new Regex(EMAIL_EXPRESS);
            return ret.IsMatch(text);
        }
        public static bool isMobile(string text)
        {
            Regex ret = new Regex(MOBILE_EXPRESS);
            return ret.IsMatch(text);
        }


    }
}
