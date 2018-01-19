#define DEBUG
#undef DEBUG
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Data;
using xbase;
using xbase.security;
using xbase.security.schema;
using xbase.umc;
using xbase.umc.attributes;
using xbase.Exceptions;
using xbase.security.wechat;

namespace xbase.security
{
    [WboAttr(LifeCycle = LifeCycle.Session, Title = "安全组件")]
    public class Security : HttpWbo, ISecurity
    {
        private const string SETTING_FILE = "security.xml";
        private const string DEF_DB_FILE = "security.mdb";
        private const string RegisterPageUrl = "register.aspx";

        private bool isLogin = false;

        public bool IsLogin
        {
            get { return isLogin; }
        }

        public static string[] ADMIG_Objects = 
        { 
          "DbManager","ChartAdmin"
        };

        private IUserContext userContext = new LoginUser(BuiltinUsers.anonym);
        private string checkingUrl;
        private IDictionary<string, string> loginFormDatas = new Dictionary<string, string>();
        private ISession session;

        public IDictionary<string, string> LoginFormDatas
        {
            get { return loginFormDatas; }
            set { loginFormDatas = value; }
        }
        //  private string loginPageUrl;



        public Security(string sessionId)
        {
            userContext.SessionId = sessionId;
            SecuritySettings.CreateDefaultDb();
        }


        public void checkPage()
        {
            if (Request.UrlReferrer == null)
                return;
            if (string.IsNullOrEmpty(Request.UrlReferrer.AbsoluteUri))
                return;


            if (Request.UrlReferrer.AbsolutePath.EndsWith(LoginPageUrl, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(LoginPageUrl))
                return;
            if (Request.UrlReferrer.AbsolutePath.EndsWith(RegisterPageUrl, StringComparison.OrdinalIgnoreCase))
                return;
            CheckObjectPermission(null, Request.UrlReferrer.AbsolutePath, PermissionTypes.Read);

        }

        public string CheckingUrl
        {
            get { return checkingUrl; }
            set { checkingUrl = value; }
        }

        public string LoginPageUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(SecuritySettings.Settings.LoginPageUrl))
                    return SecuritySettings.Settings.LoginPageUrl.Trim().Replace("\\", "/");
                return "";
            }
        }


        public string UserRole
        {
            get
            {
                Database db = SecuritySettings.GetDb();
                if (userContext.Id.Equals(BuiltinUsers.anonym, StringComparison.OrdinalIgnoreCase))
                    return BuiltinUsers.anonym;

                if (userContext.Id.Equals(BuiltinUsers.admin, StringComparison.OrdinalIgnoreCase))
                    return BuiltinUsers.admin;

                string sql = @"select role_id from xsys_user_roles where [user_id]=@userid";
                DbCommand cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "@userid", DbType.String, user.Id);
                object r = db.ExecuteScalar(cmd);

                if (r != null)
                    return r.ToString();

                return "";
            }
        }

        public bool IsAdminRoleUser
        {
            get
            {
                return UserRole.Equals(BuiltinUsers.admin, StringComparison.OrdinalIgnoreCase);
            }
        }

        #region ISecurity 成员

        private static bool isPermissionObject(string objectId)
        {
            Database db = SecuritySettings.GetDb();
            DbCommand cmd = db.GetSqlStringCommand(SecurityDataScripts.HasPermissionObject);
            db.AddInParameter(cmd, "@objectId", DbType.String, objectId);
            object r = db.ExecuteScalar(cmd);
            return (r is int) && (int)r > 0;
        }

        private static bool isAdminObject(string objectId)
        {
            foreach (string so in SecuritySettings.Settings.AdministratorObjects)
            {
                if (objectId.StartsWith(so))
                    return true;
            }
            return false;
        }

        public bool CheckObjectPermission(string objectType, string objectId, PermissionTypes permissionType)
        {
            if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.AbsoluteUri) && !Request.UrlReferrer.AbsoluteUri.EndsWith(LoginPageUrl))
                checkingUrl = Request.UrlReferrer.AbsoluteUri;

            if (isAdminObject(objectId) && !IsAdminRoleUser)
            {
                if (BuiltinUsers.anonym.Equals(user.Id))
                    throw new PermissionException(LoginPageUrl, user.Id, objectId);
                throw new Exception("操作" + objectId + "需要管理员权限," + user.Id + "无权操作");
            }

            if (!isPermissionObject(objectId)) return true;

            if (objectId.Equals("ds", StringComparison.OrdinalIgnoreCase))
                return true;


            if (IsAdminRoleUser)
                return true;

            List<Role> userRoles = UserAccount.getUserRoles(this.userContext.Id);

            foreach (Role userRole in userRoles)
            {
                PermissionTypes rolePerm = RoleManager.getRoleObjectPermission(userRole.Id, objectId);
                if (((permissionType & rolePerm) != 0) || rolePerm > permissionType)
                    return true;
            }

            throw new PermissionException(LoginPageUrl, user.Id, objectId);
        }

        public bool _CheckObjectPermission(string objectType, string objectId, PermissionTypes permissionType)
        {
            //nocheck this folder /xj-service/

            if (userContext.Id.Equals(BuiltinUsers.admin, StringComparison.OrdinalIgnoreCase))
                return true;

            if (objectId.StartsWith("/xj-service/", StringComparison.OrdinalIgnoreCase))
                return true;

            if (isAdminObject(objectType))
            {
                return IsAdminRoleUser;
            }
            if (isAdminObject(objectId))
            {
                return IsAdminRoleUser;
            }

            if (IsAdminRoleUser) return true;
            Database db = SecuritySettings.GetDb();
            DbConnection conn = db.CreateConnection();


            try
            {
                //   if (conn.State != ConnectionState.Open)
                conn.Open();
                DbCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                if (objectType.Equals("ListData", StringComparison.OrdinalIgnoreCase))
                {
                    if (objectId.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
                        objectId = objectId.Remove(objectId.LastIndexOf(".html", StringComparison.OrdinalIgnoreCase));
                    if (objectId.EndsWith(".htm", StringComparison.OrdinalIgnoreCase))
                        objectId = objectId.Remove(objectId.LastIndexOf(".htm", StringComparison.OrdinalIgnoreCase));
                    objectId = objectId.TrimStart('/');

                }
                cmd.CommandText = @"select [object_id] from xsys_role_permissions where [object_id]='" + objectId + "' and [object_type]='" + objectType + "'";
                DbDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (!reader.HasRows)
                        return true;
                }
                finally
                {
                    reader.Close();
                }
                //  cmd = conn.CreateCommand();

                cmd.CommandText = @"select [permission] from xsys_role_permissions where [object_id]='" + objectId + "' and [object_type]='" + objectType + "'" +
                      " and role_id in (select role_id from xsys_user_roles where [user_id]='" + userContext.Id + "') ";
                reader = cmd.ExecuteReader();
                try
                {
                    if (!reader.HasRows)
                        return false;

                    while (reader.Read())
                    {
                        PermissionTypes p = (PermissionTypes)reader.GetInt32(0);
                        if ((permissionType & p) != 0) return true;
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("安全检查时，安全系统配置错误" + e.Message);
                }
                finally
                {
                    reader.Close();
                }
                return false;
            }
            catch (Exception e)
            {
                throw new Exception("安全检查时，安全系统配置错误" + e.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public bool login(WeChatOS tokenInfo)
        {
            User user = WeChatLoginService.requestUser(tokenInfo);
            this.userContext = new LoginUser(user.Id);
            userContext.Name = user.DisplayName;
            userContext.HeadPhoto = user.HeadPhoto;
            return true;
        }

        public bool login(string userId, string password)
        {
            Database db = SecuritySettings.GetDb();
            DbConnection conn = db.CreateConnection();
            try
            {
                //  if (conn.State == ConnectionState.Closed)
                conn.Open();
                DbCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = @"select [user_id],[password],[display_name],[group_id] from xsys_user_account where [user_id]=@UserId" +
                                  @" or bind_email=@UserId" +
                                  @" or bind_mobile=@UserId";

                SqlParameter pr = new SqlParameter();
                pr.ParameterName = "@UserId";
                pr.Value = userId;
                cmd.Parameters.Add(pr);

                DbDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (!reader.HasRows)
                    {
                        throw new XUserException("用户或口令错误");
                    }
                    while (reader.Read())
                    {
                        object p = reader["password"];
                        object u = reader["user_id"];
                        object n = reader["display_name"];
                        object g = reader["group_id"];
                        if (p.ToString().Equals(Crypto.Encrypt(password)))
                        {
                            userContext.Id = u.ToString();
                            userContext.Name = n.ToString();
                            if (String.IsNullOrEmpty(userContext.Name))
                                userContext.Name = userContext.Id;
                            userContext.GroupId = g.ToString();
                            isLogin = true;
                            return true;

                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
                throw new XUserException("用户或口令错误");
            }
            finally
            {
                conn.Close();
            }
            throw new XUserException("用户或口令错误");
        }

        public void Logout()
        {
            userContext = new LoginUser(BuiltinUsers.anonym);
        }

        public IUserContext user
        {
            get
            {
                this.userContext.HostLoginName = Request.LogonUserIdentity.Name;
                userContext.IP = Request.UserHostAddress;

                return this.userContext;
            }
        }

        #endregion


        public bool isFriend(string p)
        {
            return true;
        }

    }
}
