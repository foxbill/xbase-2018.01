using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase;
using xbase.Exceptions;

namespace xbase.data
{
    public  class XDataException:XException
    {

    }

    public class WebTableException : XException
    {

    }

    public class E_CannotUploadFileAtNewRecord : XException
    {
        public E_CannotUploadFileAtNewRecord() :
            base("不能在没有保存的记录上上传文件！")
        {
        }
    }
    public class E_NoKeyFieldColumn : XException
    {
        public E_NoKeyFieldColumn() :
            base("更新记录必须要包括一个主键字段！没有发现主键字段，不能更新记录！")
        {
        }
    }

}
