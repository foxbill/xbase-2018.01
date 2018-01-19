using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace xbase.data
{
    public static class DbTypeCaptions
    {
        private static Dictionary<int, string> _captions = new Dictionary<int, string>()
        {

            //
            // 摘要:
            //     非 Unicode 字符的可变长度流，范围在 1 到 8,000 个字符之间。
            {(int)DbType.AnsiString,"英文字符"},

            //
            // 摘要:
            //     表示 Unicode 字符串的类型。
            {(int)DbType.String,"任何字符串"},

            //
            // 摘要:
            //     变长数值。
            {(int)DbType.VarNumeric,"任何数字"},

            //
            // 摘要:
            //     表示一个日期和时间值的类型。
            {(int)DbType.DateTime,"日期时间"},

            //
            // 摘要:
            //     常规类型，表示任何没有由其他 DbType 值显式表示的引用或值类型。
            {(int)DbType.Object,"未知"},

            //
            // 摘要:
            //     二进制数据的可变长度流，范围在 1 到 8,000 个字节之间。
            {(int)DbType.Binary,"二进制数据"},

            //
            // 摘要:
            //     一个 8 位无符号整数，范围在 0 到 255 之间。
            {(int)DbType.Byte,"短整数(0-255)"},
            //
            // 摘要:
            //     简单类型，表示 true 或 false 的布尔值。
            {(int)DbType.Boolean,"增加值"},
            //
            // 摘要:
            //     货币值，范围在 -2 63（即 -922,337,203,685,477.5808）到 2 63 -1（即 +922,337,203,685,477.5807）之间，精度为千分之十个货币单位。
            {(int)DbType.Currency,"货币"},
            //
            // 摘要:
            //     表示日期值的类型。
            {(int)DbType.Date,"日期"},

            //
            // 摘要:
            //     简单类型，表示从 1.0 x 10 -28 到大约 7.9 x 10 28 且有效位数为 28 到 29 位的值。
            {(int)DbType.Decimal,"数字"},
            //
            // 摘要:
            //     浮点型，表示从大约 5.0 x 10 -324 到 1.7 x 10 308 且精度为 15 到 16 位的值。
            {(int)DbType.Double,"小数"},
            //
            // 摘要:
            //     全局唯一标识符（或 GUID）。
            {(int)DbType.Guid,"全局唯一标识符（GUID）"},
            //
            // 摘要:
            //     整型，表示值介于 -32768 到 32767 之间的有符号 16 位整数。
            {(int)DbType.Int16,"16位整数(-32768 到 32767)"},
            //
            // 摘要:
            //     整型，表示值介于 -2147483648 到 2147483647 之间的有符号 32 位整数。
            {(int)DbType.Int32,"32位整数(-2147483648 到 2147483647)"},
            //
            // 摘要:
            //     整型，表示值介于 -9223372036854775808 到 9223372036854775807 之间的有符号 64 位整数。
            {(int)DbType.Int64,"32位整数(-9223372036854775808 到 9223372036854775807)"},
 
            //
            // 摘要:
            //     整型，表示值介于 -128 到 127 之间的有符号 8 位整数。
            {(int)DbType.SByte,"短整数(-128 到 127)"},
            //
            // 摘要:
            //     浮点型，表示从大约 1.5 x 10 -45 到 3.4 x 10 38 且精度为 7 位的值。
            {(int)DbType.Single,"单精小数"},
            //
            // 摘要:
            //     表示时间值的类型。
            {(int)DbType.Time,"时间"},
            //
            // 摘要:
            //     整型，表示值介于 0 到 65535 之间的无符号 16 位整数。
            {(int)DbType.UInt16,"16位无符号整数"},
            //
            // 摘要:
            //     整型，表示值介于 0 到 4294967295 之间的无符号 32 位整数。
            {(int)DbType.UInt32,"32位整数"},
            //
            // 摘要:
            //     整型，表示值介于 0 到 18446744073709551615 之间的无符号 64 位整数。
            {(int)DbType.UInt64,"整数"},
            //
            // 摘要:
            //     非 Unicode 字符的固定长度流。
            {(int)DbType.AnsiStringFixedLength,"定长英文字符"},
            //
            // 摘要:
            //     Unicode 字符的定长串。
            {(int)DbType.StringFixedLength,"定长字符串"},
            //
            // 摘要:
            //     XML 文档或片段的分析表示。
            {(int)DbType.Xml,"XML文本"},
            //
            // 摘要:
            //     日期和时间数据。日期值范围从公元 1 年 1 月 1 日到公元 9999 年 12 月 31 日。时间值范围从 00:00:00 到 23:59:59.9999999，精度为
            //     100 毫微秒。
            {(int)DbType.DateTime2,"精确日期时间"},
            //
            // 摘要:
            //     显示时区的日期和时间数据。日期值范围从公元 1 年 1 月 1 日到公元 9999 年 12 月 31 日。时间值范围从 00:00:00 到 23:59:59.9999999，精度为
            //     100 毫微秒。时区值范围从 -14:00 到 +14:00。
            {(int)DbType.DateTimeOffset,"时区日期时间"}
        };

        public static string caption(DbType type)
        {
            return _captions[(int)type];
        }

        public static Dictionary<int, string> captions
        {
            get
            {
                return _captions;
            }
        }

        public static List<ValueTextPair<int>> valueTextPairs()
        {
            List<ValueTextPair<int>> ret = new List<ValueTextPair<int>>();
            foreach (int type in _captions.Keys)
            {
                ValueTextPair<int> vtp = new ValueTextPair<int>();
                vtp.value = type;
                vtp.text = _captions[type];
                ret.Add(vtp);
            }
            return ret;
        }



    }
}
