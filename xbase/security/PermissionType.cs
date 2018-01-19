using System;
using System.Collections.Generic;

namespace xbase.security
{
    [Flags]
    public enum PermissionTypes
    {
        None = 0,
        Read = 1,
        Write = 2,
        Delete = 4,
        Execute = 8,
        DoAll = 16
    }

    public static class PermissionTypesHelper
    {
        public static Dictionary<PermissionTypes, string> Captions =
           new Dictionary<PermissionTypes, string>() 
        {
           {PermissionTypes.None,"拒绝访问"},
           {PermissionTypes.Read,"读取"},
           {PermissionTypes.Write,"写入"},
           {PermissionTypes.Delete,"删除"},
           {PermissionTypes.Execute,"执行"},
           {PermissionTypes.DoAll,"完全控制"}
        };
    }

}
