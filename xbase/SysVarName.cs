using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.security;

namespace xbase
{
    public static class SysVar
    {
        public const string Today = "Date.Today";
        public const string Now = "Date.Now";
        public const string Time = "Date.Time";
        public const string YYYY = "Date.YYYY";
        public const string mm = "Date.mm";
        public const string DD = "Date.DD";
        public const string HH = "Date.HH";
        public const string MI = "Date.MI";
        public const string Week = "Date.W";
        public const string Q = "Date.Q";
        public const string UserName = "User.Name";
        public const string UserId = "User.Id";
        public const string UserGroup = "User.Group";


        public static bool IsVar(string varName)
        {
            if (string.IsNullOrEmpty(varName))
                return false;

            if (varName.Equals("Date", StringComparison.OrdinalIgnoreCase))
                return true;

            return varName.StartsWith("Date.", StringComparison.OrdinalIgnoreCase) || varName.StartsWith("User.", StringComparison.OrdinalIgnoreCase);
        }

        public static string GetValue(string name, IUserContext userConext)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            string ret = null;

            if (name.Equals("Date", StringComparison.OrdinalIgnoreCase))
                ret = DateTime.Today.ToString();

            else if (name.Equals(DD, StringComparison.OrdinalIgnoreCase))
                ret = DateTime.Today.ToString("YY");

            else if (name.Equals(HH, StringComparison.OrdinalIgnoreCase))
                ret = DateTime.Today.ToString("HH");

            else if (name.Equals(MI, StringComparison.OrdinalIgnoreCase))
                ret = DateTime.Today.ToString("MM");

            else if (name.Equals(mm, StringComparison.OrdinalIgnoreCase))
                ret = DateTime.Today.ToString("mm");

            else if (name.Equals(Time, StringComparison.OrdinalIgnoreCase))
                ret = DateTime.Now.ToShortTimeString();

            else if (name.Equals(Now, StringComparison.OrdinalIgnoreCase))
                ret = DateTime.Now.ToString();

            else if (name.Equals(Q, StringComparison.OrdinalIgnoreCase))
                ret = (((int)DateTime.Today.Month / 4) + 1).ToString();

            else if (name.Equals(Today, StringComparison.OrdinalIgnoreCase))
                ret = DateTime.Today.ToShortDateString();

            else if (name.Equals(Week, StringComparison.OrdinalIgnoreCase))
                ret = DateTime.Today.DayOfWeek.ToString();

            else if (name.Equals(YYYY, StringComparison.OrdinalIgnoreCase))
                ret = DateTime.Today.ToString("YYYY");

            else if (name.Equals(UserId, StringComparison.OrdinalIgnoreCase))
                ret = userConext.Id;

            else if (name.Equals(UserName, StringComparison.OrdinalIgnoreCase))
                ret = userConext.Name;

            else if(name.Equals(UserGroup,StringComparison.OrdinalIgnoreCase))
                ret=userConext.GroupId;

            return ret;
        }

    }
}
