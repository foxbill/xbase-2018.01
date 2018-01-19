using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.security;


namespace xbase.security
{
    public class PermissionObject
    {
        private string objectType;

        private PermissionTypes permission;

        /// <summary>
        /// 设置permission权限标志
        /// </summary>
        /// <param name="flag">要标志的权限</param>
        /// <param name="enable">是否允许</param>
        private void setFlage(PermissionTypes flag, bool enable)
        {
            permission = permission | flag;
            if (!enable)
                permission = permission ^ flag;
        }

        public PermissionTypes Permission
        {
            get { return permission; }
            set { permission = value; }
        }

        public string ObjectType
        {
            get { return objectType; }
            set { objectType = value; }
        }

        private string objectId;

        public string ObjectId
        {
            get { return objectId; }
            set { objectId = value; }
        }

        public bool Read
        {
            get { return (PermissionTypes.Read & Permission) != 0; }
            set { setFlage(PermissionTypes.Read, value); }

        }

        public bool Write
        {
            get { return (PermissionTypes.Write & Permission) != 0; }
            set { setFlage(PermissionTypes.Write, value); }
        }

        public bool Delete
        {
            get { return (PermissionTypes.Delete & Permission) != 0; }
            set { setFlage(PermissionTypes.Delete, value); }
        }

        public bool Execute
        {
            get { return (PermissionTypes.Execute & Permission) != 0; }
            set { setFlage(PermissionTypes.Execute, value); }
        }
        public bool DoAll
        {
            get { return (PermissionTypes.DoAll & Permission) != 0; }
            set { setFlage(PermissionTypes.DoAll, value); }
        }
        public Dictionary<string, bool> PermissionList
        {
            get
            {
                Dictionary<string, bool> ret = new Dictionary<string, bool>();
                foreach (PermissionTypes p in Enum.GetValues(typeof(PermissionTypes)))
                {
                    ret.Add(Enum.GetName(typeof(PermissionTypes), p), (p & permission) != 0);
                }
                return ret;
            }
            set
            {
                permission = PermissionTypes.None;
                foreach (string key in value.Keys)
                {
                    PermissionTypes pType = (PermissionTypes)Enum.Parse(typeof(PermissionTypes), key);
                    permission = permission | pType;
                    if (!value[key])
                        permission = permission ^ pType;
                }
            }
        }
    }

    public class RolePermisssions
    {
        private string roleId;

        public string RoleId
        {
            get { return roleId; }
            set { roleId = value; }
        }
        private List<PermissionObject> permissionObjList = new List<PermissionObject>();

        public List<PermissionObject> PermissionObjList
        {
            get { return permissionObjList; }
            set { permissionObjList = value; }
        }

    }
}
