/*
    文件的字符集在Windows下有两种，一种是ANSI，一种Unicode。

    对于Unicode，Windows支持了它的三种编码方式，一种是小尾编码（Unicode)，一种是大尾编码(BigEndianUnicode)，一种是UTF-8编码。

    我们可以从文件的头部来区分一个文件是属于哪种编码。当头部开始的两个字节为 FF FE时，是Unicode的小尾编码；当头部的两个字节为FE FF时，是Unicode的大尾编码；当头部两个字节为EF BB时，是Unicode的UTF-8编码；当它不为这些时，则是ANSI编码。

    按照如上所说，我们可以通过读取文件头的两个字节来判断文件的编码格式，代码如下(C#代码）：

    程序中System.Text.Encoding.Default是指操作系统的当前 ANSI 代码页的编码。

 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.sdk
{
    class FileUtils
    {

        public System.Text.Encoding GetFileEncodeType(string filename)
        {

            System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);

            Byte[] buffer = br.ReadBytes(2);

            if (buffer[0] >= 0xEF)
            {

                if (buffer[0] == 0xEF && buffer[1] == 0xBB)
                {

                    return System.Text.Encoding.UTF8;

                }

                else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
                {

                    return System.Text.Encoding.BigEndianUnicode;

                }

                else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
                {

                    return System.Text.Encoding.Unicode;

                }

                else
                {

                    return System.Text.Encoding.Default;

                }

            }

            else
            {

                return System.Text.Encoding.Default;

            }
        }
    }
}
