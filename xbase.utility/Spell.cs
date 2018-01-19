using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Text.RegularExpressions;

namespace xbase.utility
{
    public static class Spell
    {
        private static Dictionary<char, string> wordlist;
        private static List<string> hebinglist = new List<string>();

        //初始化，读文本文件
        static Spell()
        {
            wordlist = new Dictionary<char, string>();
            //string path = ConfigurationManager.AppSettings["PinYinPath"];
            string path = AppDomain.CurrentDomain.BaseDirectory + "bin\\pinyin.ini";
            using (System.IO.StreamReader st = new System.IO.StreamReader(path, Encoding.Default))
            {
                string line;
                int i = 0;
                while (!string.IsNullOrEmpty(line = st.ReadLine()))
                {
                    line = Regex.Unescape(line);
                    if (!line.Contains('='))
                        continue;

                    string[] keyValue = line.Split('=');
                    string key = keyValue[0].Trim();
                    string value = keyValue[1].Trim();
                    if (key.Length != 1)
                        throw new Exception("In pinyin.ini line (" + i + ") format error .");
                    char c = key[0];
                    if (!wordlist.ContainsKey(c))
                        wordlist.Add(c, value);

                }
                st.Close();
            }
        }


        /// <summary>
        /// 扩展string方法，获取字符穿首拼
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToShouPin(this string s)
        {
            return GetShouPin(s);
        }

        public static string ToQuanPin(this string s)
        {
            return GetQuanPin(s);
        }

        private static string findDict(char c)
        {
            bool isAllowChar = (c >= 'A' && c <= 'Z')
                            || (c >= 'a' && c <= 'z')
                            || (c >= '0' && c <= '9')
                            || c == '_';

            string ret = "";
            if (isAllowChar)
                ret = c.ToString();
            else if (wordlist.ContainsKey(c))
            {
                ret = wordlist[c];
            }

            //if (string.IsNullOrEmpty(ret))
            //    throw new Exception("Can not find char '" + c + "' (ascii:" + (int)c + ") in pinyin.ini");

            return ret;
        }

        /// <summary>
        /// 检测并处理首字母为数字的字符串
        /// </summary>
        /// <param name="sb"></param>
        private static void procAlif(StringBuilder sb)
        {
            if (sb[0] >= '0' && sb[0] <= '9')
            {
                sb.Insert(1, '_');
                sb.Insert(0, "No");
            }

        }
        //获取全拼
        public static string GetQuanPin(string chineseWord)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in chineseWord)
            {
                string py = findDict(c);
                if (py.Length > 0)
                    py = py.ToUpper()[0] + py.Remove(0, 1);
                sb.Append(py);
            }
            procAlif(sb);
            return sb.ToString();
        }

        //获取首字母
        public static string GetShouPin(string s)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in s)
            {
                string py = findDict(c);
                char sp = py.ToUpper()[0];
                sb.Append(sp);
            }
            procAlif(sb);
            return sb.ToString();
        }

    }
}
