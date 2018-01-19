using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.tools
{
    public static class StringTools
    {
        /// <summary>
        /// 将unicode字符串转成Ansi字符串
        /// </summary>
        /// <returns>Ansi字符串</returns>
        public static string UnicodeStringToAnsi(string unicodeString)
        {

            string asciiString = "";
            if (!string.IsNullOrEmpty(unicodeString))
            {
                unicodeString = unicodeString.Replace("/", "");
                unicodeString = unicodeString.Replace("\\", "");
                string[] strlist = unicodeString.Split('u');
                if (strlist.Length > 1)
                {
                    try
                    {
                        for (int i = 1; i < strlist.Length; i++)
                        {
                            //將unicode轉為10進制整數，然後轉為char中文
                            asciiString += (char)int.Parse(strlist[i], System.Globalization.NumberStyles.HexNumber);
                        }
                    }
                    catch (FormatException ex)
                    {
                        asciiString = ex.Message;
                    }
                }
                else
                {
                    Encoding ascii = Encoding.ASCII;
                    Encoding unicode = Encoding.Unicode;            
                    asciiString = ConvertEncoding(unicodeString, unicode, ascii);
                }
                          
            } 
            return asciiString;
        }

        /// <summary>
        /// 取中文文本的拼音首字母
        /// </summary>
        /// <param name="text">编码为UTF8的文本</param>
        /// <returns>返回中文对应的拼音首字母</returns>
        public static string CnToPinyinHead(string text)
        {
            text = text.Trim();
            StringBuilder chars = new StringBuilder();
            for (var i = 0; i < text.Length; ++i)
            {
                string py = GetPinyin(text[i]);
                if (py != "") chars.Append(py[0]);
            }

            return chars.ToString().ToUpper();
        }


        /// <summary>
        /// 取中文文本的拼音首字母
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="encoding">源文本的编码</param>
        /// <returns>返回encoding编码类型中文对应的拼音首字母</returns>
        public static string CnToPinyinHead(string text, Encoding encoding)
        {
            string temp = ConvertEncoding(text, encoding, Encoding.UTF8);
            return ConvertEncoding(CnToPinyinHead(temp), Encoding.UTF8, encoding);
        }



        /// <summary>
        /// 取中文文本的拼音
        /// </summary>
        /// <param name="text">编码为UTF8的文本</param>
        /// <returns>返回中文文本的拼音</returns>
        public static string CnToPinyin(string text)
        {
            StringBuilder sbPinyin = new StringBuilder();
            for (var i = 0; i < text.Length; ++i)
            {
                string py = GetPinyin(text[i]);
                if (py != "") sbPinyin.Append(py);
                sbPinyin.Append(" ");
            }

            return sbPinyin.ToString().Trim();
        }

        /// <summary>
        /// 取中文文本的拼音
        /// </summary>
        /// <param name="text">编码为UTF8的文本</param>
        /// <param name="encoding">源文本的编码</param>
        /// <returns>返回encoding编码类型的中文文本的拼音</returns>
        public static string CnToPinyin(string text, Encoding encoding)
        {
            string temp = ConvertEncoding(text.Trim(), encoding, Encoding.UTF8);
            return ConvertEncoding(CnToPinyin(temp), Encoding.UTF8, encoding);
        }

        /// <summary>
        /// 取和拼音相同的汉字列表
        /// </summary>
        /// <param name="Pinyin">编码为UTF8的拼音</param>
        /// <returns>取拼音相同的汉字列表，如拼音“ai”将会返回“唉爱……”等</returns>
        public static string PyToCn(string pinyin)
        {
            string key = pinyin.Trim().ToLower();

            foreach (string str in PyCode.codes)
            {
                if (str.StartsWith(key + " ") || str.StartsWith(key + ":"))
                    return str.Substring(7);
            }

            return "";
        }


        /// <summary>
        /// 取和拼音相同的汉字列表，编码同参数encoding
        /// </summary>
        /// <param name="Pinyin">编码为encoding的拼音</param>
        /// <param name="encoding">编码</param>
        /// <returns>返回编码为encoding的拼音为pinyin的汉字列表，如拼音“ai”将会返回“唉爱……”等</returns>
        public static string PyToCn(string pinyin, Encoding encoding)
        {
            string text = ConvertEncoding(pinyin, encoding, Encoding.UTF8);
            return ConvertEncoding(PyToCn(text), Encoding.UTF8, encoding);
        }



        /// <summary>
        /// 返回单个字符的汉字拼音
        /// </summary>
        /// <param name="ch">编码为UTF8的中文字符</param>
        /// <returns>ch对应的拼音</returns>
        private static string GetPinyin(char ch)
        {
            short hash = GetHashIndex(ch);
            for (var i = 0; i < PyHash.hashes[hash].Length; ++i)
            {
                short index = PyHash.hashes[hash][i];
                var pos = PyCode.codes[index].IndexOf(ch, 7);
                if (pos != -1)
                    return PyCode.codes[index].Substring(0, 6).Trim();
            }
            return ch.ToString();
        }

        /// <summary>
        /// 返回单个字符的汉字拼音
        /// </summary>
        /// <param name="ch">编码为encoding的中文字符</param>
        /// <returns>编码为encoding的ch对应的拼音</returns>
        private static string GetPinyin(char ch, Encoding encoding)
        {
            ch = ConvertEncoding(ch.ToString(), encoding, Encoding.UTF8)[0];
            return ConvertEncoding(GetPinyin(ch), Encoding.UTF8, encoding);
        }

        /// <summary>
        /// 转换编码 
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="srcEncoding">源编码</param>
        /// <param name="dstEncoding">目标编码</param>
        /// <returns>目标编码文本</returns>
        private static string ConvertEncoding(string text, Encoding srcEncoding, Encoding dstEncoding)
        {
            byte[] srcBytes = srcEncoding.GetBytes(text);
            byte[] dstBytes = Encoding.Convert(srcEncoding, dstEncoding, srcBytes);
            return dstEncoding.GetString(dstBytes);
        }

        /// <summary>
        /// 取文本索引值
        /// </summary>
        /// <param name="ch">字符</param>
        /// <returns>文本索引值</returns>
        private static short GetHashIndex(char ch)
        {
            return (short)((uint)ch % PyCode.codes.Length);
        }
    }
}
