using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase;
using System.Data;
using xbase.umc;
using xbase.data;
using xbase.umc.attributes;
using Microsoft.Practices.EnterpriseLibrary.Data;
using xbase.data.db;
using System.Data.Common;
using xbase.regular;
using xbase.Exceptions;

namespace xbase.security
{
    public class User
    {
        private string id;
        private string displayName;
        private string email;
        private string mobile;
        private string memo;
        private bool active;
        private bool disable;
        private string groupId;
        private string password;
        private string createDate;

        public string CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }


        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }


        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }
        public string Memo
        {
            get { return memo; }
            set { memo = value; }
        }

        public bool IsActive
        {
            get { return active; }
            set { active = value; }
        }

        public bool IsDisable
        {
            get { return disable; }
            set { disable = value; }
        }

        public string GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }


        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }


        public string HeadPhoto { get; set; }
    }

    [WboAttr(Id = "UserAccount", Title = "用户管理", Version = 1.1, LifeCycle = LifeCycle.Global)]
    public class UserAccount : HttpWbo
    {
        private int pageSize;


        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }


        [WboMethodAttr(Title = "获取用户列表", Description = "根据组名，获取改组的用户列表")]
        public List<User> getUsers(string groupId)
        {
            string sql = SecurityDataScripts.GetUserListSQL(groupId);
            DataTable tb = SecuritySettings.Execute(sql);
            List<User> ret = new List<User>();
            foreach (DataRow r in tb.Rows)
            {
                User user = new User();
                user.Id = r["user_id"].ToString();
                user.DisplayName = r["display_name"].ToString();
                if (string.IsNullOrEmpty(user.DisplayName))
                    user.DisplayName = user.Id;
                user.CreateDate = r["create_date"].ToString();
                user.Password = "";
                user.IsDisable = r["disabled"] is System.DBNull ? false : (bool)r["disabled"];
                user.IsActive = r["actived"] is System.DBNull ? false : (bool)r["actived"];
                user.Email = r["bind_email"].ToString();
                user.Mobile = r["bind_mobile"].ToString();
                user.GroupId = r["group_id"].ToString();
                ret.Add(user);
            }
            return ret;
        }

        public bool repassword(string password1, string password2)
        {
            if (!Security.IsLogin)
                throw new XUserException("请先登录");

            if (password1.Equals(password2))
                throw new XUserException("两次输入的密码不一致");
            DatabaseAdmin dba = SecuritySettings.getDBA();
            DbCommand cmd = dba.getSqlStringCommand(SecurityDataScripts.InsertUserSql);
            dba.addInParameter(cmd, "@Id", DbType.String, Security.user.Id);
            return dba.execNonQuery(cmd) != 0;
        }

        public bool addUser(User user)
        {
            if (String.IsNullOrEmpty(user.Id) || !(UserInfoExpress.isEmail(user.Id)
             || UserInfoExpress.isMobile(user.Id)))
                throw new XUserException("新用户注册，必须填写手机号或电子邮件！");

            if (existsUser(user.Id))
                throw new XUserException("新用户注册，用户" + user.Id + "已经被别人使用！");

            user.Password = Crypto.Encrypt(user.Password);
            if (UserInfoExpress.isEmail(user.Id) && string.IsNullOrEmpty(user.Email))
                user.Email = user.Id;

            if (UserInfoExpress.isMobile(user.Id) && string.IsNullOrEmpty(user.Mobile))
                user.Mobile = user.Id;

            DatabaseAdmin dba = SecuritySettings.getDBA();
            DbCommand cmd = dba.getSqlStringCommand(SecurityDataScripts.InsertUserSql);
            dba.addInParameter(cmd, "@Id", DbType.String, user.Id);
            dba.addInParameter(cmd, "@DisplayName", DbType.String, user.DisplayName);
            dba.addInParameter(cmd, "@Password", DbType.String, user.Password);
            dba.addInParameter(cmd, "@create_date", DbType.DateTime, DateTime.Now);
            dba.addInParameter(cmd, "@IsDisable", DbType.Boolean, user.IsDisable);
            dba.addInParameter(cmd, "@IsActive", DbType.Boolean, user.IsActive);
            dba.addInParameter(cmd, "@Email", DbType.AnsiString, user.Email);
            dba.addInParameter(cmd, "@Mobile", DbType.AnsiString, user.Mobile);
            dba.addInParameter(cmd, "@GroupId", DbType.AnsiString, user.GroupId);
            bool ret = dba.execNonQuery(cmd) != 0;
            if (!ret)
                throw new XUserException("用户添加失败");
            return ret;
        }

        public bool existsUser(string userId)
        {
            DatabaseAdmin dba = SecuritySettings.getDBA();
            DbCommand cmd = dba.getSqlStringCommand(SecurityDataScripts.CheckUserExistsSQL);
            dba.addInParameter(cmd, "@user_id", DbType.String, userId);
            object o = dba.executeScalar(cmd);
            return (o != null);
        }

        public bool updateUser(User user, string userId)
        {
            // if (!(Security.user.Id.Equals(user.Id) && Security.IsAdminRoleUser))
            //     throw new XUserException("无权操作");
            if (existsUser(user.Id) && user.Id != userId)
                throw new XUserException("用户" + user.Id + "已经存在,法将用户" + userId + "改为" + user.Id);

            DatabaseAdmin dba = SecuritySettings.getDBA();
            DbCommand cmd = dba.getSqlStringCommand(SecurityDataScripts.UpdateUserSql);
            dba.addInParameter(cmd, "@Id", DbType.String, user.Id);
            dba.addInParameter(cmd, "@DisplayName", DbType.String, user.DisplayName);
            dba.addInParameter(cmd, "@IsDisable", DbType.Boolean, user.IsDisable);
            dba.addInParameter(cmd, "@IsActive", DbType.Boolean, user.IsActive);
            dba.addInParameter(cmd, "@Email", DbType.AnsiString, user.Email);
            dba.addInParameter(cmd, "@Mobile", DbType.AnsiString, user.Mobile);
            dba.addInParameter(cmd, "@GroupId", DbType.AnsiString, user.GroupId);
            dba.addInParameter(cmd, "@oldId", DbType.AnsiString, userId);
            bool ret = dba.execNonQuery(cmd) != 0;
            if (!ret)
                throw new XUserException(userId + "用户未发现");
            return ret;
        }

        public bool deleteUser(string userId)
        {
            DatabaseAdmin dba = SecuritySettings.getDBA();
            DbCommand cmd = dba.getSqlStringCommand(SecurityDataScripts.DeleteUserSQL);
            dba.addInParameter(cmd, "@user_id", DbType.String, userId);
            return dba.execNonQuery(cmd) != 0;
        }


        public void appendUserRole(string userId, string roleId)
        {
            DatabaseAdmin dba = SecuritySettings.getDBA();
            DbCommand cmd = dba.getSqlStringCommand(SecurityDataScripts.CheckUserRolesSQl);
            dba.addInParameter(cmd, "@roleId", DbType.String, roleId);
            dba.addInParameter(cmd, "@userId", DbType.String, userId);
            object c = dba.executeScalar(cmd);
            if ((int)c < 1)
            {
                cmd = dba.getSqlStringCommand(SecurityDataScripts.AppendUserRolesSQl);
                dba.addInParameter(cmd, "@roleId", DbType.String, roleId);
                dba.addInParameter(cmd, "@userId", DbType.String, userId);
            }
            dba.execNonQuery(cmd);
        }

        public static void deleteUserRole(string userId, string roleId)
        {
            DatabaseAdmin dba = SecuritySettings.getDBA();
            DbCommand cmd = dba.getSqlStringCommand(SecurityDataScripts.deleteUserRoleSQL);
            dba.addInParameter(cmd, "@roleId", DbType.String, roleId);
            dba.addInParameter(cmd, "@userId", DbType.String, userId);
            dba.execNonQuery(cmd);
        }


        public static List<Role> getUserRoles(string userId)
        {
            string sql = SecurityDataScripts.GetUserRoleListSQL(userId);
            DataTable tb = SecuritySettings.Execute(sql);
            List<Role> ret = new List<Role>();
            foreach (DataRow r in tb.Rows)
            {
                Role role = new Role();
                role.Id = r["role_id"].ToString();
                role.DisplayName = r["display_name"].ToString();
                role.Remark = r["remark"].ToString();
                ret.Add(role);
            }
            return ret;
        }

        public bool appendRoles(UserRoleIds userRoleIds)
        {

            string userId = userRoleIds.UserId;
            DatabaseAdmin dba = SecuritySettings.getDBA();

            for (int i = 0; i < userRoleIds.RoleIds.Count; i++)
            {
                DbCommand cmd = dba.getSqlStringCommand(SecurityDataScripts.CheckUserRolesSQl);
                string roleId = userRoleIds.RoleIds[i];
                dba.addInParameter(cmd, "@roleId", DbType.String, roleId);
                dba.addInParameter(cmd, "@userId", DbType.String, userId);
                object c = dba.executeScalar(cmd);
                if ((int)c < 1)
                {
                    cmd = dba.getSqlStringCommand(SecurityDataScripts.AppendUserRolesSQl);
                    dba.addInParameter(cmd, "@roleId", DbType.String, roleId);
                    dba.addInParameter(cmd, "@userId", DbType.String, userId);
                }
                dba.execNonQuery(cmd);
            }
            return true;

        }
    }

    public class UserRoleIds
    {
        private string userId;

        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        private List<string> roleIds = new List<string>();

        public List<string> RoleIds
        {
            get { return roleIds; }
            set { roleIds = value; }
        }

    }
}
