using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.IO;

namespace xbase
{
    public static class RegMachine
    {
        private const int TRY_DATS = 100;
        private const string DES_KEY = "foxbill1";
        private const string DES_IV = "28649347";
        private static int[] intCode = new int[127]
        //  0,1,2,3,4,5,6,7,8,9,
        {
            3,5,3,4,5,9,2,3,1,8,//1
            6,7,6,3,9,9,8,2,8,4,//2
            9,5,3,6,9,5,3,4,9,3,//3
            4,5,2,4,5,3,2,3,1,8,//4
            8,5,7,3,5,9,3,3,1,8,//5
            1,5,9,7,5,3,2,3,1,8,//6
            7,5,3,8,2,9,4,3,1,8,//7
            6,5,9,7,6,8,2,3,1,8,//8
            4,5,2,4,8,9,7,3,1,8,//9
            9,5,9,8,4,7,7,3,1,8,//10
            8,5,7,3,9,4,7,3,1,8,//11
            9,5,3,6,3,9,6,3,1,8,//12
            7,5,9,9,6,3,2       //13
        };//存储密钥
        private static int[] intNumber = new int[25];//存机器码的Ascii值
        private static char[] charcode = new char[25];//存储机器码字

        public static string GetDiskVolumeSerialNumber()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"d:\"");
            disk.Get();
            return disk.GetPropertyValue("VolumeSerialNumber").ToString();
        }

        //获得CPU的序列号
        public static string getCpu()
        {
            string strCpu = null;
            ManagementClass myCpu = new ManagementClass("win32_Processor");
            ManagementObjectCollection myCpuConnection = myCpu.GetInstances();
            foreach (ManagementObject myObject in myCpuConnection)
            {
                strCpu = myObject.Properties["Processorid"].Value.ToString();
                break;
            }
            return strCpu;
        }

        //生成机器码
        public static string getMNum()
        {
            string strNum = getCpu() + GetDiskVolumeSerialNumber();//获得24位Cpu和硬盘序列号
            string strMNum = strNum.Substring(0, 24);//从生成的字符串中取出前24个字符做为机器码
            return strMNum;
        }
        private static void setIntCode()//给数组赋值小于10的数
        {
            //   for (int i = 1; i < intCode.Length; i++)
            //   {
            //       intCode[i] = i % 9;
            //   }
        }

        //第二步。根据机器码 生成注册码 
        //生成注册码      

        internal static string getRNum(string macNum)
        {
            setIntCode();//初始化127位数组
            for (int i = 1; i < charcode.Length; i++)//把机器码存入数组中
            {
                charcode[i] = Convert.ToChar(macNum.Substring(i - 1, 1));
            }
            for (int j = 1; j < intNumber.Length; j++)//把字符的ASCII值存入一个整数组中。
            {
                intNumber[j] = intCode[Convert.ToInt32(charcode[j])] + Convert.ToInt32(charcode[j]);
            }
            string strAsciiName = "";//用于存储注册码
            for (int j = 1; j < intNumber.Length; j++)
            {
                if (intNumber[j] >= 48 && intNumber[j] <= 57)//判断字符ASCII值是否0－9之间
                {
                    strAsciiName += Convert.ToChar(intNumber[j]).ToString();
                }
                else if (intNumber[j] >= 65 && intNumber[j] <= 90)//判断字符ASCII值是否A－Z之间
                {
                    strAsciiName += Convert.ToChar(intNumber[j]).ToString();
                }
                else if (intNumber[j] >= 97 && intNumber[j] <= 122)//判断字符ASCII值是否a－z之间
                {
                    strAsciiName += Convert.ToChar(intNumber[j]).ToString();
                }
                else//判断字符ASCII值不在以上范围内
                {
                    if (intNumber[j] > 122)//判断字符ASCII值是否大于z
                    {
                        strAsciiName += Convert.ToChar(intNumber[j] - 10).ToString();
                    }
                    else
                    {
                        strAsciiName += Convert.ToChar(intNumber[j] - 9).ToString();
                    }
                }
            }
            return strAsciiName;
        }

        private static bool CheckRegNum(string regNum)
        {
            if (String.IsNullOrEmpty(regNum)) return false;
            return regNum.Equals(getRNum(getMNum()));
        }

        internal static bool CheckRegFile(string fileName)
        {
            string regNum = File.ReadAllText(fileName);
            if (String.IsNullOrEmpty(regNum)) return false;
            return regNum.Equals(getRNum(getMNum()));
        }

        public static bool Regist(string regNum, string fileName)
        {
            if (!CheckRegNum(regNum)) return false;
            File.WriteAllText(fileName, regNum);
            return true;
        }

        public static bool CheckRegFile()
        {
            string fileName = XSite.BinServerPath + "reg.data";
            if (!File.Exists(fileName))
                return false;
            string regNum = File.ReadAllText(fileName);
            if (String.IsNullOrEmpty(regNum)) return false;
            return regNum.Equals(getRNum(getMNum()));
        }

        public static bool Regist(string regNum)
        {
            string fileName = XSite.BinServerPath + "reg.data";
            if (!CheckRegNum(regNum)) return false;
            File.WriteAllText(fileName, regNum);
            return true;
        }

        private static void CreateStartTime()
        {
            string fileName = XSite.BinServerPath + "syatem.dat";
            DateTime startTime = DateTime.Now;
            File.WriteAllText(fileName, Crypto.DESEncrypt(startTime.ToString(), DES_KEY, DES_IV));
        }

        internal static bool CheckTimeOut()
        {
            string fileName = XSite.BinServerPath + "syatem.dat";
            DateTime starttime = DateTime.MinValue;

            if (!File.Exists(fileName))
                CreateStartTime();

            try
            {
                string s = File.ReadAllText(fileName);
                s = Crypto.DESDecrypt(s, DES_KEY, DES_IV);
                starttime = DateTime.Parse(s);
            }
            catch { }

            TimeSpan ts = DateTime.Now.Subtract(starttime);
            return ts.Days > 30;

        }


        //第三步。检查注册状况，若没有注册，可自定义试用       

        /// <summary>
        /// 检查注册
        /// </summary>
        //    private void CheckRegist()
        //   {

        //this.btn_reg.Enabled = true;

        //RegistryKey retkey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("software", true).CreateSubKey("wxk").CreateSubKey("wxk.INI");
        //foreach (string strRNum in retkey.GetSubKeyNames())//判断是否注册
        //{
        //    if (strRNum == clsTools.getRNum())
        //    {
        //        thControl(true);
        //        return;
        //    }
        //}
        //thControl(false);
        //Thread th2 = new Thread(new ThreadStart(thCheckRegist2));
        //th2.Start();

    }


    /// <summary>
    /// 验证试用次数
    /// </summary>
    ///     private static void thCheckRegist2()
    //     {
    //MessageBox.Show("您现在使用的是试用版，该软件可以免费试用3000000次！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
    //Int32 tLong;
    //try
    //{
    //    tLong = (Int32)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\angel", "UseTimes", 0);
    //    MessageBox.Show("感谢您已使用了" + tLong + "次", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
    //}
    //catch
    //{
    //    Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\angel", "UseTimes", 0, RegistryValueKind.DWord);
    //    MessageBox.Show("欢迎新用户使用本软件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
    //}
    //tLong = (Int32)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\angel", "UseTimes", 0);
    //if (tLong < 3000000)
    //{
    //    int Times = tLong + 1;
    //    Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\angel", "UseTimes", Times);
    //}
    //else
    //{
    //    MessageBox.Show("试用次数已到", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    //    Application.Exit();
    //}
    //   }


    // }
}
