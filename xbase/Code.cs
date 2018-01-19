using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

class Code
{
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

    private static string _KEY = "HQDCKEY1";  //密钥  
    private static string _IV = "HQDCKEY2";   //向量  


    internal static byte[] getKey()
    {
        List<byte> keys = new List<byte>();

        for (int i = 10; i < 18; i++)
        {
            int v = ((intCode[i] * 30) % 9) + 48 + intCode[i];
            keys.Add((byte)v);
        }

        return keys.ToArray();
    }

    internal static byte[] getIV()
    {
        List<byte> keys = new List<byte>();

        for (int i = 30; i < 38; i++)
        {
            int v = ((intCode[i] * 10) % 8) + 97 + intCode[i];
            keys.Add((byte)v);
        }

        return keys.ToArray();
    }

    /// <summary>  
    /// 加密  
    /// </summary>  
    /// <param name="data"></param>  
    /// <returns></returns>  
    public static string Encode(string data)
    {

        byte[] byKey = getKey();  //System.Text.ASCIIEncoding.ASCII.GetBytes(_KEY);
        byte[] byIV = getIV();// System.Text.ASCIIEncoding.ASCII.GetBytes(_IV);

        DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
        int i = cryptoProvider.KeySize;
        MemoryStream ms = new MemoryStream();
        CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);

        StreamWriter sw = new StreamWriter(cst);
        sw.Write(data);
        sw.Flush();
        cst.FlushFinalBlock();
        sw.Flush();

        string strRet = Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
        return strRet;
    }

    /// <summary>  
    /// 解密  
    /// </summary>  
    /// <param name="data"></param>  
    /// <returns></returns>  
    public static string Decode(string data)
    {

        byte[] byKey = getKey();// System.Text.ASCIIEncoding.ASCII.GetBytes(_KEY);
        byte[] byIV = getIV();// System.Text.ASCIIEncoding.ASCII.GetBytes(_IV);

        byte[] byEnc;

        try
        {
            data.Replace("_%_", "/");
            data.Replace("-%-", "#");
            byEnc = Convert.FromBase64String(data);

        }
        catch
        {
            return null;
        }

        DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
        MemoryStream ms = new MemoryStream(byEnc);
        CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
        StreamReader sr = new StreamReader(cst);
        return sr.ReadToEnd();
    }
}

