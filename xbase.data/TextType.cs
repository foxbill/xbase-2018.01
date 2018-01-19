using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.data
{
    /// <summary>
    /// 文本类型工具，判断文本是否是一个特定的内容；
    /// </summary>
    public sealed class TextType
    {
        public const string file = "file:";
        public const string img = "img:";
        public const string wbc = "wbc:";
        public const string chart = "chart:";
        public const string wbo = "wbo:";

        public static string getTextConent(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            else if (text.StartsWith(file))
                return text.Remove(0, file.Length);
            else if (text.StartsWith(img))
                return text.Remove(0, img.Length);
            else
                return text;
        }
    }
}
