using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using xbase.umc;
using xbase.security;
using xbase.umc.attributes;
using xbase.Exceptions;
using xbase.data.db;
using System.Data.Common;


namespace xbase.security
{
    [WboAttr(Id = "RoleManager", Title = "角色管理", Version = 1.1, LifeCycle = LifeCycle.Global)]
    public class RoleManager
    {
        [WboMethodAttr(Title = "获取角色列表", Description = "获取角色列表")]
        public List<Role> getRoles()
        {
            string sql = SecurityDataScripts.GetRoleListSQL();
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

        public bool existsRole(string roleId)
        {
            string sql = SecurityDataScripts.CheckRoleExistsSQL(roleId);
            object roleObj = SecuritySettings.ExecuteScalar(sql);
            return !(roleObj == null);
        }

        public bool addRole(Role role)
        {
            if (existsRole(role.Id))
                throw new XUserException("角色" + role.Id + "已经存在");

            DatabaseAdmin dba = SecuritySettings.getDBA();
            DbCommand cmd = dba.getSqlStringCommand(SecurityDataScripts.InsertRoleSql);
            dba.addInParameter(cmd, "@Id", DbType.String, role.Id);
            dba.addInParameter(cmd, "@DisplayName", DbType.String, role.DisplayName);
            dba.addInParameter(cmd, "@Remark", DbType.String, role.Remark);
            bool ret = dba.execNonQuery(cmd) != 0;
            if (!ret)
                throw new XUserException("角色添加失败");
            return ret;
        }
        public bool updateRole(Role role, string roleId)
        {
            if (String.IsNullOrEmpty(roleId))
                throw new Exception("角色Id不能为空");

            if (existsRole(role.Id) && !roleId.Equals(role.Id, StringComparison.OrdinalIgnoreCase))
                throw new Exception("角色Id" + role.Id + "已经存在，不能将" + roleId + "修改成" + role.Id);

            DatabaseAdmin dba = SecuritySettings.getDBA();
            DbCommand cmd = dba.getSqlStringCommand(SecurityDataScripts.UpdateRoleSql);
            dba.addInParameter(cmd, "@Id", DbType.String, role.Id);
            dba.addInParameter(cmd, "@DisplayName", DbType.String, role.DisplayName);
            dba.addInParameter(cmd, "@Remark", DbType.String, role.Remark);
            dba.addInParameter(cmd, "@oldId", DbType.String, roleId);
            bool ret = dba.execNonQuery(cmd) != 0;
            if (!ret)
                throw new XUserException("角色修改失败,角色" + roleId + "不存在");
            return ret;

        }

        public bool deleteRole(string roleId)
        {
            DatabaseAdmin dba = SecuritySettings.getDBA();
            DbCommand cmd = dba.getSqlStringCommand(SecurityDataScripts.DeleteRoleSQL);
            dba.addInParameter(cmd, "@Id", DbType.String, roleId);
            return dba.execNonQuery(cmd) != 0;
        }

        public List<PermissionObject> getRolePermissions(string roleId)
        {
            return getRolePermissions(roleId, "%");
        }

        public List<PermissionObject> getRolePermissions(string roleId, string objType)
        {
            DatabaseAdmin dba = SecuritySettings.getDBA();
            DbCommand cmd = dba.getSqlStringCommand(SecurityDataScripts.GetPermissionObjectsSql);
            dba.addInParameter(cmd, "@roleId", DbType.String, roleId);
            dba.addInParameter(cmd, "@objType", DbType.String, objType);
            DataTable tb = dba.executeTable(cmd);

            List<PermissionObject> ret = new List<PermissionObject>();
            foreach (DataRow r in tb.Rows)
            {
                int p = 0;
                PermissionObject permObj = new PermissionObject();
                permObj.ObjectId = r["object_id"].ToString();
                permObj.ObjectType = r["object_type"].ToString();
                int.TryParse(r["permission"].ToString(), out p);
                permObj.Permission = (PermissionTypes)p;
                ret.Add(permObj);
            }
            return ret;
        }

        public Dictionary<string, PermissionObject> getRolePermissionDict(string roleId)
        {
            return getRolePermissionDict(roleId, "%");
        }

        public Dictionary<string, PermissionObject> getRolePermissionDict(string roleId, string objType)
        {
            DatabaseAdmin dba = SecuritySettings.getDBA();
            DbCommand cmd = dba.getSqlStringCommand(SecurityDataScripts.GetPermissionObjectsSql);
            dba.addInParameter(cmd, "@roleId", DbType.String, roleId);
            dba.addInParameter(cmd, "@objType", DbType.String, objType);
            DataTable tb = dba.executeTable(cmd);

            Dictionary<string, PermissionObject> ret = new Dictionary<string, PermissionObject>();
            foreach (DataRow r in tb.Rows)
            {
                int p = 0;
                PermissionObject permObj = new PermissionObject();
                permObj.ObjectId = r["object_id"].ToString();
                permObj.ObjectType = r["object_type"].ToString();
                int.TryParse(r["permission"].ToString(), out p);
                permObj.Permission = (PermissionTypes)p;
                ret.Add(permObj.ObjectId, permObj);
            }
            return ret;
        }
        public static bool existsRoleObjectPermission(string roleId, string objectId)
        {
            DatabaseAdmin dba = SecuritySettings.getDBA();
            DbCommand cmd = dba.getSqlStringCommand(SecurityDataScripts.RoleObjectPermissionSql);
            dba.addInParameter(cmd, "@roleId", DbType.String, roleId);
            dba.addInParameter(cmd, "@objectId", DbType.String, objectId);
            object ret = dba.executeScalar(cmd);
            if (ret != null && ret is int)
                return (int)ret != 0;
            else
                return false;
        }

        public static PermissionTypes getRoleObjectPermission(string roleId, string objectId)
        {
            DatabaseAdmin dba = SecuritySettings.getDBA();
            DbCommand cmd = dba.getSqlStringCommand(SecurityDataScripts.RoleObjectPermissionSql);
            dba.addInParameter(cmd, "@roleId", DbType.String, roleId);
            dba.addInParameter(cmd, "@objectId", DbType.String, objectId);
            object ret = dba.executeScalar(cmd);
            if (ret != null && ret is int)
                return (PermissionTypes)ret;
            else
                return PermissionTypes.None;
        }

        /// <summary>
        /// 用权限类型设置角色的对象权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="objectId">对象ID</param>
        /// <param name="type">权限类型字符串：None/Read/Write/Execute/DoAll</param>
        /// <param name="enable"></param>
        public static void setPermission(string roleId, string objectId, string type, bool enable)
        {
            PermissionTypes permission = (PermissionTypes)Enum.Parse(typeof(PermissionTypes), type);
            PermissionTypes oldPerm = getRoleObjectPermission(roleId, objectId);
            oldPerm = oldPerm | permission;
            if (!enable)
                oldPerm = oldPerm ^ permission;

            DatabaseAdmin dba = SecuritySettings.getDBA();
            DbCommand cmd = dba.getSqlStringCommand(SecurityDataScripts.SetRoleObjectPermissionSql);
            dba.addInParameter(cmd, "@roleId", DbType.String, roleId);
            dba.addInParameter(cmd, "@objectId", DbType.String, objectId);
            dba.addInParameter(cmd, "@permission", DbType.Int32, oldPerm);
            dba.execNonQuery(cmd);
        }
        /// <summary>
        /// 设置角色的对象权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="objectId"></param>
        /// <param name="permission"></param>
        public static void setPermission(string roleId, string objectId, PermissionTypes permission)
        {
            PermissionTypes oldPerm = getRoleObjectPermission(roleId, objectId);
            oldPerm = oldPerm | permission;

            DatabaseAdmin dba = SecuritySettings.getDBA();
            DbCommand cmd = dba.getSqlStringCommand(SecurityDataScripts.SetRoleObjectPermissionSql);
            dba.addInParameter(cmd, "@roleId", DbType.String, roleId);
            dba.addInParameter(cmd, "@objectId", DbType.String, objectId);
            dba.addInParameter(cmd, "@permission", DbType.Int32, oldPerm);
            dba.execNonQuery(cmd);
            //for(PermissionTypes 
        }

        public static bool savePermissions(RolePermisssions rolePermissions)
        {
            List<PermissionObject> permissions = rolePermissions.PermissionObjList;
            string roleId = rolePermissions.RoleId;
            bool ret = false;
            for (int i = 0; i < permissions.Count; i++)
            {
                PermissionObject po = permissions[i];
                string sql = SecurityDataScripts.SavePermissionSql(roleId, po.ObjectType, po.ObjectId, po.Permission);
                SecuritySettings.ExecuteNonQuery(sql);
            }
            ret = true;
            return ret;
        }

        public bool deletePermission(string roleId, string objectType, string objectId)
        {
            string sql = SecurityDataScripts.DeleteRolePermissionSQL(roleId, objectType, objectId);
            SecuritySettings.ExecuteNonQuery(sql);
            return true;
        }


    }

    public class Role
    {
        private string id;
        private string displayName;
        private string remark;

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

        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }
    }


}
