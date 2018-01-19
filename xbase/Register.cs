using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using xbase.local;
using Newtonsoft.Json;

namespace xbase
{
    public static class Register
    {
        private const string HostName = "xbase.host.exe";
        private static UserInfo userInfo;
        private static string callHost(string cmd)
        {
            string proceRet = "";
            //    try
            //    {
            Process proce = new Process();
            proce.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory + "bin\\";
            proce.StartInfo.FileName = proce.StartInfo.WorkingDirectory + HostName;
            proce.StartInfo.UseShellExecute = false;
            if (!string.IsNullOrEmpty(cmd))
            {
                proce.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                proce.StartInfo.Arguments = cmd;
            }
            proce.StartInfo.RedirectStandardInput = true;
            proce.StartInfo.RedirectStandardOutput = true;
            proce.Start();
            proceRet = proce.StandardOutput.ReadToEnd();
            // proce.WaitForExit();
            if (!string.IsNullOrEmpty(cmd))
                proce.Close();
            return proceRet;
            //   }
            //   catch
            //   {
            //       throw new Exception(Lang.AccreditSystemErr);
            //   }
        }
        public static string getRegisterString()
        {
            return callHost("2");
        }

        public static UserInfo setAccreditString(string accredit)
        {
            string s = callHost("3 " + accredit);
            userInfo = JsonConvert.DeserializeObject<UserInfo>(s);
            return userInfo;
        }

        public static UserInfo getUserInfo()
        {
            if (userInfo == null)
            {
                string s = callHost("1");
                if (string.IsNullOrEmpty(s))
                    throw new Exception(Lang.AccreditFileErr);
                if (s.StartsWith("error:"))
                    throw new Exception(Lang.AccreditFileErr);

                s = Code.Decode(s);
                s = Code.Decode(s);

                if (string.IsNullOrEmpty(s))
                    throw new Exception(Lang.AccreditFileErr);

                userInfo = JsonConvert.DeserializeObject<UserInfo>(s);
            }
            return userInfo;
        }

        private static bool checkUser()
        {
            getUserInfo();
            switch (userInfo.accreditLevel)
            {
                case 1:
                    TimeSpan ts = DateTime.Today - userInfo.accreditTime;
                    return (ts.Days < 20) && (ts.Days > -1);
                case 100:
                    return true;
                default:
                    return false;
            }
        }

        public static void testHost()
        {
            callHost(null);
        }


        public static bool checkActive()
        {

            bool ret = checkUser();
            if (!ret)
            {
                //删除旧授权重检一次
                userInfo = null;
                ret = checkUser();
            }
            return ret;
        }

    }
}
