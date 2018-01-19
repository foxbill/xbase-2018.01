using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.security;

namespace xbase
{
    public static class VarGetter
    {
        public static string GetValue(string name, IUserContext userConext)
        {
            string ret = null;
            if (name.Equals(SysVar.DD, StringComparison.OrdinalIgnoreCase))
                ret = DateTime.Today.ToString("YY");

            else if (name.Equals(SysVar.HH, StringComparison.OrdinalIgnoreCase))
                ret = DateTime.Today.ToString("HH");

            else if (name.Equals(SysVar.MI, StringComparison.OrdinalIgnoreCase))
                ret = DateTime.Today.ToString("MM");

            else if (name.Equals(SysVar.mm, StringComparison.OrdinalIgnoreCase))
                ret = DateTime.Today.ToString("mm");

            else if (name.Equals(SysVar.Time, StringComparison.OrdinalIgnoreCase))
                ret = DateTime.Now.ToShortTimeString();

            else if (name.Equals(SysVar.Q, StringComparison.OrdinalIgnoreCase))
                ret = (((int)DateTime.Today.Month / 4) + 1).ToString();

            else if (name.Equals(SysVar.Today, StringComparison.OrdinalIgnoreCase))
                ret = DateTime.Today.ToString();

            else if (name.Equals(SysVar.Week, StringComparison.OrdinalIgnoreCase))
                ret = DateTime.Today.DayOfWeek.ToString();

            else if (name.Equals(SysVar.YYYY, StringComparison.OrdinalIgnoreCase))
                ret = DateTime.Today.ToString("YYYY");

            else if (name.Equals(SysVar.UserId, StringComparison.OrdinalIgnoreCase))
                ret = userConext.Id;

            else if (name.Equals(SysVar.UserName, StringComparison.OrdinalIgnoreCase))
                ret = userConext.Name;

            return ret;
        }
    }
}
