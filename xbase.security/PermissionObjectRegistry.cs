using System;
using System.Collections.Generic;
using System.Data;

using System.Data.Common;
using System.ComponentModel;

namespace XSecurity
{
    public class PermissionObjectRegistry
    {
        private string _objectid;
        private string _permissionTypeId;  //该控件的类型
        private string _parentid;   //父节点id
        private string _objectdescription;

        public PermissionObjectRegistry()
        { }
        public PermissionObjectRegistry(string objectid,string permissiontypeid, string parentid, string description)
        {
            this.ObjectId = objectid;
            this.PermissionTypeId = permissiontypeid;
            this.ParentId = parentid;
            this.Description = description;
        }

        public string ObjectId
        {
            get { return _objectid; }
            set { _objectid = value; }
        }
        public string PermissionTypeId
        {
            get { return _permissionTypeId; }
            set { _permissionTypeId = value; }
        }
        public string ParentId
        {
            get { return _parentid; }
            set { _parentid = value; }
        }
        public string Description
        {
            get { return _objectdescription; }
            set { _objectdescription = value; }
        }
    }

    public class PermissionObjectRegistryBO
    {
        Database database = DatabaseFactory.CreateDatabase();
        DbCommand dbcommand;
        /// <summary>
        /// 根据PermissionTypeId查询控件
        /// </summary>
        /// <param name="typeid"></param>
        /// <returns></returns>
        public DataSet PermissionObjectByType(string typeid)
        {
            DataSet ds = new DataSet();
            try
            {
                dbcommand = database.GetSqlStringCommand("select ObjectId,PermissionTypeId,ParentId,ObjectDescription from PermissionObjectRegistry where PermissionTypeId=@PermissionTypeId");
                database.AddInParameter(dbcommand, "@PermissionTypeId", DbType.String, typeid);
                ds = database.ExecuteDataSet(dbcommand);
            }
            catch { }
            return ds;
        }
        /// <summary>
        /// 根据系统id加载系统对应的所有菜单对象
        /// </summary>
        /// <returns></returns>
        public DataSet GetMenuObject()
        {
            DataSet ds = null;
            try
            {
                dbcommand = database.GetSqlStringCommand("select ObjectId,PermissionTypeId,ParentId,ObjectDescription from PermissionObjectRegistry where PermissionObjectRegistry.PermissionTypeId='2'");
                ds = database.ExecuteDataSet(dbcommand);
            }
            catch { }
            return ds;
        }
        /// <summary>
        /// 在系统中增加菜单
        /// </summary>
        /// <param name="objectid">菜单ID</param>
        /// <param name="objectdescription">菜单名称</param>
        /// <param name="parentid">菜单的上级节点</param>
        /// <returns></returns>
        public bool AddMenuForSysId(string objectid, string objectdescription, string parentid)
        {
            bool result = false;
            try
            {
                dbcommand = database.GetSqlStringCommand("insert into PermissionObjectRegistry(ObjectId,PermissionTypeId,ParentId,ObjectDescription) values (@ObjectId,'2',@ParentId,@ObjectDescription)");
                database.AddInParameter(dbcommand, "@ObjectId", DbType.String, objectid);
                database.AddInParameter(dbcommand, "@ParentId", DbType.String, parentid);
                database.AddInParameter(dbcommand, "@ObjectDescription", DbType.String, objectdescription);
                if (database.ExecuteNonQuery(dbcommand) == 1)
                {
                    result = true;
                }
            }
            catch { }
            return result;
        }
        /// <summary>
        /// 在系统中删除菜单
        /// </summary>
        /// <param name="objectid">菜单名称</param>
        /// <returns></returns>
        public bool DelMenuForSysId(string objectid)
        {
            bool result = false;
            try
            {
                dbcommand = database.GetSqlStringCommand("delete from PermissionObjectRegistry where ObjectId=@ObjectId or ParentId =@ObjectId");
                database.AddInParameter(dbcommand, "@ObjectId", DbType.String, objectid);
                if (database.ExecuteNonQuery(dbcommand) > 0)
                {
                    result = true;
                }
            }
            catch { }
            return result;
        }
        /// <summary>
        /// 更新系统菜单信息
        /// </summary>
        /// <param name="newobjectid">新菜单ID</param>
        /// <param name="objectdescription">菜单名称</param>
        /// <param name="originalobjectid">旧菜单ID</param>
        /// <returns></returns>
        public bool UpdateMenuForSysId(string newobjectid, string objectdescription,string originalobjectid)
        {
            bool result = false;
            try
            {
                dbcommand = database.GetSqlStringCommand("update PermissionObjectRegistry set ObjectId=@NewObjectId,ObjectDescription=@ObjectDescription where ObjectId=@OriginalObjectId");
                database.AddInParameter(dbcommand, "@NewObjectId", DbType.String, newobjectid);
                database.AddInParameter(dbcommand, "@ObjectDescription", DbType.String, objectdescription);
                database.AddInParameter(dbcommand, "@OriginalObjectId", DbType.String, originalobjectid);
                if (database.ExecuteNonQuery(dbcommand) == 1)
                {
                    result = true;
                }
            }
            catch { }
            return result;
        }
        /// <summary>
        /// 获取所有对象
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllObject()
        {
            DataSet ds = null;
            try
            {
                dbcommand = database.GetSqlStringCommand("select ObjectId,PermissionTypeId,ParentId,ObjectDescription from PermissionObjectRegistry");
                ds = database.ExecuteDataSet(dbcommand);
            }
            catch { }
            return ds;
        }
        /// <summary>
        /// 获取所有模块类信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetObjectByPermissionTypeId(string permissiontypeid)
        {
            DataSet ds = null;
            try
            {
                dbcommand = database.GetSqlStringCommand("select ObjectId,PermissionTypeId,ParentId,ObjectDescription from PermissionObjectRegistry where PermissionTypeId=@permissiontypeid");
                database.AddInParameter(dbcommand, "@permissiontypeid", DbType.String, permissiontypeid);
                ds = database.ExecuteDataSet(dbcommand);
            }
            catch { }
            return ds;
        }
        /// <summary>
        /// 获取父节点下的所有子节点信息（不包括父节点）ObjectId,PermissionTypeId,ParentId,ObjectDescription
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public DataSet GetChildByParentId(string parentid)
        {
            DataSet ds = null;
            try
            {
                dbcommand = database.GetSqlStringCommand("select a.* from PermissionObjectRegistry a inner join dbo.f_getchildid(@ParentId) b on a.ObjectId=b.id");
                database.AddInParameter(dbcommand, "@ParentId", DbType.String, parentid);
                ds = database.ExecuteDataSet(dbcommand);
            }
            catch { }
            return ds;
        }
    }
}
