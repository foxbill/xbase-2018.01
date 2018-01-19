using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.IO;
using xbase.host;
using Newtonsoft.Json;

public static class RegMachine
{
    private const string FileName = "system.data";
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

    public static string GetMac()
    {
        try
        {
            ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection queryCollection = query.Get();
            foreach (ManagementObject mo in queryCollection)
            {
                if (mo["IPEnabled"].ToString() == "True")
                    return mo["MacAddress"].ToString();
            }
            return "";
        }
        catch
        {
            return "";
        }
    }

    //生成机器码
    public static string getMNum()
    {
        string strNum = getCpu() + GetDiskVolumeSerialNumber();//获得24位Cpu和硬盘序列号
        string strMNum = strNum.Substring(0, 24);//从生成的字符串中取出前24个字符做为机器码
        return strMNum;
    }

    /// <summary>
    /// 转换Key
    /// </summary>
    private static void setIntCode()//给数组赋值小于10的数
    {
        for (int i = 1; i < intCode.Length; i++)
        {
            intCode[i] = i % 9;
        }
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

    //public static bool CheckRegNum(string regNum)
    //{
    //    if (String.IsNullOrEmpty(regNum)) return false;
    //    return regNum.Equals(getRNum(getMNum()));
    //}

    //internal static bool CheckRegFile(string fileName)
    //{
    //    string regNum = File.ReadAllText(fileName);
    //    if (String.IsNullOrEmpty(regNum)) return false;
    //    return regNum.Equals(getRNum(getMNum()));
    //}

    //public static bool Regist(string regNum, string fileName)
    //{
    //    if (!CheckRegNum(regNum)) return false;
    //    File.WriteAllText(fileName, regNum);
    //    return true;
    //}

    /// <summary>
    /// 获取注册码，用户用这个码获取授权码
    /// </summary>
    /// <returns></returns>
    internal static string getRegistorCode()
    {
        UserInfo ui = new UserInfo();
        ui.macCode = getCpu();
        ui.installTime = DateTime.Now;
        string s = JsonConvert.SerializeObject(ui);
        return Code.Encode(s);
    }



    /// <summary>
    /// 获取加密授权码
    /// </summary>
    /// <returns></returns>
    internal static string getAccCode()
    {
        try
        {
            string fileName = AppDomain.CurrentDomain.BaseDirectory + FileName;
            string regNum = File.ReadAllText(fileName);
            string sui = Code.Decode(regNum);
            UserInfo ui = JsonConvert.DeserializeObject<UserInfo>(sui);

            if (getCpu().Equals(ui.macCode, StringComparison.OrdinalIgnoreCase))
                return Code.Encode(regNum);
            else
                return "error:授权文件不匹配";
        }
        catch (Exception e)
        {
            return "error:" + e.Message;
        }

    }

    internal static UserInfo writeAccredit(string accredit)
    {
        try
        {
            string s = Code.Decode(accredit);
            UserInfo ui = JsonConvert.DeserializeObject<UserInfo>(s);

            if (ui.accreditLevel < 1 || string.IsNullOrEmpty(ui.userName))
                return null;

            string fileName = AppDomain.CurrentDomain.BaseDirectory + FileName;
            File.WriteAllText(fileName, accredit);
            return ui;
        }
        catch
        {
            return null;
        }

    }

}



