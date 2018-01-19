using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.Exceptions;

namespace xbase.wbs
{
    public class E_NotFindDataObject : XException
    {
        public E_NotFindDataObject(string msg) :
            base("数据源对象无法找到.数据源Id" + msg)
        {
        }
    }

    public class E_CurrentVesionNoSuportsNonIDatasourceObject:XException{
        public E_CurrentVesionNoSuportsNonIDatasourceObject():
            base("目前版本不支持非IDataSource对象")
        {

        }
    }
}
