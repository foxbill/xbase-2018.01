using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.security
{
    /// <summary>
    /// 安全框架接口
    /// </summary>
    public interface ISecurity
    {
        /// <summary>
        /// 检查对象访问许可
        /// </summary>
        /// <param name="sessionid"></param>
        /// <param name="objectid"></param>
        /// <param name="permissionValue"></param>
        /// <returns></returns>
        bool CheckObjectPermission(string objectType, string objectId, PermissionTypes permissionTypes);
        /// <summary>
        /// 登陆验证
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="password"></param>
        /// <param name="objUserContext"></param>
        /// <returns></returns>
        bool login(string userid, string password);
        bool IsAdminRoleUser{get;}
        string UserRole { get; }


        IUserContext user { get; }

        string LoginPageUrl { get; }
        string CheckingUrl { set; get; }

        IDictionary<string, string> LoginFormDatas { get; set; }

        void Logout();

        bool IsLogin { get;}

        bool isFriend(string p);
    }
}
