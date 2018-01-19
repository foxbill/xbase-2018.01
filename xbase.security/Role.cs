using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace XSecurity
{
    public class Role
    {
        private string _roleId;
        private string _roleDescription; //角色名称
        private string _roleRemark;  //备注
        private string _definerId;  //角色建立者Id
        public string _definerName;  //角色建立者姓名
        private DateTime _defineDate;  //角色建立时间
        private string _sysId;     //角色具有权限的系统Id
        private string _objectid;
        private int _permission;

        public Role()
        { }
        
        public string RoleId
        {
            get { return _roleId; }
            set { _roleId = value; }
        }
        public string RoleDescription
        {
            get { return _roleDescription; }
            set { _roleDescription = value; }
        }
        public string RoleRemark
        {
            get { return _roleRemark; }
            set { _roleRemark = value; }
        }
        public string DefinerId
        {
            get { return _definerId; }
            set { _definerId = value; }
        }
        public string DefineName
        {
            get { return _definerName; }
            set { _definerName = value; }
        }
        public DateTime DefinDate
        {
            get { return _defineDate; }
            set { _defineDate = value; }
        }
        public string SysId
        {
            get { return _sysId; }
            set { _sysId = value; }
        }
        public string ObjectId
        {
            get { return _objectid; }
            set { _objectid = value; }
        }
        public int Permission
        {
            get { return _permission; }
            set { _permission = value; }
        }
    }

    public class RoleBO
    {
        Database database = DatabaseFactory.CreateDatabase();
        DbCommand dbcommand;
        DbConnection connection;
        DbTransaction transaction;

        /// <summary>
        /// 获取所有角色信息(RoleId,RoleDescription,RoleRemark,DefinerId,DefineDate,UserName)
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllRole()
        {
            DataSet ds = null;
            dbcommand = database.GetSqlStringCommand("select Role.*,UserAccount.UserName from Role left join UserAccount on Role.DefinerId=UserAccount.UserId ");
            ds = database.ExecuteDataSet(dbcommand);
            return ds;
        }
        /// <summary>
        /// 判断角色ID是否存在
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public bool IsRoleidExists(string roleid)
        {
            bool result = false;
            try
            {
                dbcommand = database.GetSqlStringCommand("select count(*) from [Role] where RoleId=@RoleId)");
                database.AddInParameter(dbcommand, "@RoleId", DbType.String, roleid);
                DataSet ds = database.ExecuteDataSet(dbcommand);
                if (ds.Tables[0].Rows[0][0].ToString() == "1")
                {
                    result = true;
                }
            }
            catch { }
            return result;
        }
        /// <summary>
        /// 增加角色
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="roledescription"></param>
        /// <returns></returns>
        public bool AddRole(string roleid, string roledescription, string roleremark, string definerid, DateTime definedate)
        {
            bool result = false;
            try
            {
                dbcommand = database.GetSqlStringCommand("insert into [Role] (RoleId,RoleDescription,RoleRemark,DefinerId,DefineDate) values (@RoleId,@RoleDescription,@RoleRemark,@DefinerId,@DefineDate)");
                database.AddInParameter(dbcommand, "@RoleId", DbType.String, roleid);
                database.AddInParameter(dbcommand, "@RoleDescription", DbType.String, roledescription);
                database.AddInParameter(dbcommand, "@RoleRemark", DbType.String, roleremark);
                database.AddInParameter(dbcommand, "@DefinerId", DbType.String, definerid);
                database.AddInParameter(dbcommand, "@DefineDate", DbType.DateTime, definedate);
                if (database.ExecuteNonQuery(dbcommand) == 1)
                {
                    result = true;
                }
            }
            catch { }
            return result;
        }
        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="roledescription"></param>
        /// <param name="roleremark"></param>
        /// <param name="originalroleid"></param>
        /// <returns></returns>
        public bool UpdateRole(string roleid, string roledescription, string roleremark, string originalroleid)
        {
            bool result = false;
            try
            {
                dbcommand = database.GetSqlStringCommand("update [Role] set RoleId=@RoleId,RoleDescription=@RoleDescription,RoleRemark=@RoleRemark where RoleId=@OriginalRoleId");
                database.AddInParameter(dbcommand, "@RoleId", DbType.String, roleid);
                database.AddInParameter(dbcommand, "@RoleDescription", DbType.String, roledescription);
                database.AddInParameter(dbcommand, "@RoleRemark", DbType.String, roleremark);
                database.AddInParameter(dbcommand, "@OriginalRoleId", DbType.String, originalroleid);
                if (database.ExecuteNonQuery(dbcommand) == 1)
                {
                    result = true;
                }
            }
            catch { }
            return result;
        }
        /// <summary>
        /// 删除角色相关的所有信息
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public bool DelRole(string roleid)
        {
            bool result = false;
            try
            {
                string strsql = "delete from Role where RoleId=@RoleId;"
                                + "delete from UserRole where RoleId=@RoleId;"
                                + "delete from RolePermission where RoleId=@RoleId;"
                                + "delete from RoleSystem where RoleId=@RoleId;";
                dbcommand = database.GetSqlStringCommand(strsql);
                database.AddInParameter(dbcommand, "@RoleId", DbType.String, roleid);
                if (database.ExecuteNonQuery(dbcommand) > 0)
                {
                    result = true;
                }
            }
            catch { }
            return result;
        }        
        /// <summary>
        /// 获取角色具有权限的系统信息
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public DataSet GetSysInfoByRoleId(string roleid)
        {
            DataSet ds = null;
            try
            {
                dbcommand = database.GetSqlStringCommand("select RoleSystem.MySystem as SysId,MySystem.SysDescription,MySystem.SysRemark from RoleSystem left join MySystem on RoleSystem.MySystem = MySystem.SysId where RoleSystem.RoleId=@RoleId");
                database.AddInParameter(dbcommand, "@RoleId", DbType.String, roleid);
                ds = database.ExecuteDataSet(dbcommand);
            }
            catch { }
            return ds;
        }
        /// <summary>
        /// 获取角色没有权限的系统信息
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public DataSet GetNotSysInfoByRoleId(string roleid)
        {
            DataSet ds = null;
            try
            {
                dbcommand = database.GetSqlStringCommand("select MySystem.SysId,MySystem.SysDescription,MySystem.SysRemark from MySystem where MySystem.SysId not in (select RoleSystem.MySystem from RoleSystem where RoleSystem.RoleId =@RoleId)");
                database.AddInParameter(dbcommand, "@RoleId", DbType.String, roleid);
                ds = database.ExecuteDataSet(dbcommand);
            }
            catch { }
            return ds;
        }
        /// <summary>
        /// 为角色增加系统权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public bool AddSysInfoByRoleId(string roleid,string sysid)
        {
            bool result = false;
            try
            {
                dbcommand = database.GetSqlStringCommand("insert into RoleSystem(RoleId,MySystem) values (@RoleId,@SysId)");
                database.AddInParameter(dbcommand, "@RoleId", DbType.String, roleid);
                database.AddInParameter(dbcommand, "@SysId", DbType.String, sysid);
                if (database.ExecuteNonQuery(dbcommand) == 1)
                {
                    result = true;
                }
            }
            catch { }
            return result;
        }
        /// <summary>
        /// 为角色删除系统权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public bool DelSysInfoByRoleId(string roleid,string sysid)
        {
            bool result = false;
            try
            {
                dbcommand = database.GetSqlStringCommand("delete from RoleSystem where RoleId=@RoleId and MySystem=@SysId");
                database.AddInParameter(dbcommand, "@RoleId", DbType.String, roleid);
                database.AddInParameter(dbcommand, "@SysId", DbType.String, sysid);
                if (database.ExecuteNonQuery(dbcommand) == 1)
                {
                    result = true;
                }
            }
            catch { }
            return result;
        }

        /// <summary>
        /// 根据用户Id获取该用户具有权限的所有子系统
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataSet GetSysByUserId(string userid)
        {
            DataSet ds = null;
            try
            {
                string strsql = "select MySystem.* from mysystem where SysId in"
                                + "(select distinct(RoleSystem.MySystem) from UserRole left join RoleSystem on UserRole.RoleId=RoleSystem.RoleId where UserRole.UserId=@UserId)";                
                dbcommand = database.GetSqlStringCommand(strsql);
                database.AddInParameter(dbcommand, "@UserId", DbType.String, userid);
                ds = database.ExecuteDataSet(dbcommand);
            }
            catch { }
            return ds;
        }

        /// <summary>
        /// 判断角色是否有该权限
        /// </summary>
        /// <param name="roldid">角色id</param>
        /// <param name="objid">控件id</param>
        /// <param name="permissionValue">要判断的权限值</param>
        /// <returns></returns>
        public bool checkPermission(string roleid, string objid, int permissionValue)
        {
            bool result = false;
            try
            {
                dbcommand = database.GetSqlStringCommand("select Permission from RolePermission where RoleId=@RoleId and ObjectId=@ObjectId");
                database.AddInParameter(dbcommand, "@RoleId", DbType.String, roleid);
                database.AddInParameter(dbcommand, "@ObjectId", DbType.String, objid);
                DataSet ds = database.ExecuteDataSet(dbcommand);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int permissionOriginal = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                    if (permissionValue <= permissionOriginal && Convert.ToBoolean(permissionOriginal & permissionValue))
                    {
                        result = true;
                    }
                }
            }
            catch { }
            return result;
        }
        public bool checkPermission(string roleid, string objid)
        {
            bool result = false;
            try
            {
                dbcommand = database.GetSqlStringCommand("select count(*) from RolePermission where RoleId=@RoleId and ObjectId=@ObjectId");
                database.AddInParameter(dbcommand, "@RoleId", DbType.String, roleid);
                database.AddInParameter(dbcommand, "@ObjectId", DbType.String, objid);
                DataSet ds = database.ExecuteDataSet(dbcommand);
                if (ds.Tables[0].Rows[0][0].ToString() == "1")
                {
                    result = true;
                }
            }
            catch { }
            return result;
        }
        /// <summary>
        /// 对角色增加权限
        /// </summary>
        /// <param name="roleid">角色id</param>
        /// <param name="objid">控件id</param>
        /// <param name="permissionValue">要添加的权限值</param>
        /// <returns></returns>
        public bool AddPermission(string roleid, string objid, int permissionValue)
        {
            bool result = false;
            try
            {
                dbcommand = database.GetSqlStringCommand("select Permission from RolePermission where RoleId=@RoleId and ObjectId=@ObjectId");
                database.AddInParameter(dbcommand, "@RoleId", DbType.String, roleid);
                database.AddInParameter(dbcommand, "@ObjectId", DbType.String, objid);
                DataSet ds = database.ExecuteDataSet(dbcommand);
                if (ds.Tables[0].Rows.Count > 0) //更新权限值
                {
                    int permissionOriginal = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                    permissionOriginal = permissionOriginal | permissionValue;
                    dbcommand = database.GetSqlStringCommand("update RolePermission set Permission=@Permission where RoleId=@RoleId and ObjectId=@ObjectId");
                    if (database.ExecuteNonQuery(dbcommand) == 1)
                    {
                        result = true;
                    }
                }
                else  //增加行
                {
                    dbcommand = database.GetSqlStringCommand("insert into RolePermission values (@RoleId,@ObjectId,@Permission)");
                    database.AddInParameter(dbcommand, "@RoleId", DbType.String, roleid);
                    database.AddInParameter(dbcommand, "@ObjectId", DbType.String, objid);
                    database.AddInParameter(dbcommand, "@Permission", DbType.Int32, permissionValue);
                    if (database.ExecuteNonQuery(dbcommand) == 1)
                    {
                        result = true;
                    }
                }
            }
            catch { }
            return result;
        }
        /// <summary>
        /// 对角色删除权限
        /// </summary>
        /// <param name="roleid">角色id</param>
        /// <param name="objid">控件id</param>
        /// <param name="permissionValue">要删除的权限值</param>
        /// <returns></returns>
        public bool DelPermission(string roleid, string objid, int permissionValue)
        {
            bool result = false;
            try
            {
                dbcommand = database.GetSqlStringCommand("select Permission from RolePermission where RoleId=@RoleId and ObjectId=@ObjectId");
                database.AddInParameter(dbcommand, "@RoleId", DbType.String, roleid);
                database.AddInParameter(dbcommand, "@ObjectId", DbType.String, objid);
                DataSet ds = database.ExecuteDataSet(dbcommand);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int permissionOriginal = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                    permissionOriginal = permissionOriginal & ~permissionValue;
                    dbcommand = database.GetSqlStringCommand("update RolePermission set Permission=@Permission where RoleId=@RoleId and ObjectId=@ObjectId");
                    if (database.ExecuteNonQuery(dbcommand) == 1)
                    {
                        result = true;
                    }
                }
            }
            catch { }
            return result;
        }

        public bool RolePermissionManage(string roleid, string objid, List<int> del, List<int> add)
        {
            bool result = false;
            try
            {
                connection = database.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                if (del != null)
                {
                    for (int i = 0; i < del.Count; i++)   //删除权限值
                    {
                        dbcommand = database.GetSqlStringCommand("update RolePermission set Permission = Permission &~ @DelPermission where RoleId=@RoleId and ObjectId=@ObjectId");
                        database.AddInParameter(dbcommand, "@DelPermission", DbType.Int32, del[i]);
                        database.AddInParameter(dbcommand, "@RoleId", DbType.String, roleid);
                        database.AddInParameter(dbcommand, "@ObjectId", DbType.String, objid);
                        database.ExecuteNonQuery(dbcommand, transaction);
                    }
                }
                if (add != null)
                {
                    for (int i = 0; i < add.Count; i++)   //增加权限值
                    {
                        string strsql = "if exists (select * from RolePermission where RoleId=@RoleId and ObjectId=@ObjectId) " +
                                        "update RolePermission set Permission = Permission | @AddPermission where RoleId=@RoleId and ObjectId=@ObjectId " +
                                        "else insert into RolePermission values (@RoleId,@ObjectId,@AddPermission)";
                        dbcommand = database.GetSqlStringCommand(strsql);
                        database.AddInParameter(dbcommand, "@AddPermission", DbType.Int32, add[i]);
                        database.AddInParameter(dbcommand, "@RoleId", DbType.String, roleid);
                        database.AddInParameter(dbcommand, "@ObjectId", DbType.String, objid);
                        database.ExecuteNonQuery(dbcommand, transaction);
                    }
                }
                transaction.Commit();
                result = true;
            }
            catch
            {
                transaction.Rollback();
                connection.Close();
            }
            finally
            {
                connection.Close();
            }
            return result;
        }

        /// <summary>
        /// 获取所有控件信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetPermission()
        {
            DataSet ds = null;
            try
            {
                string strsql = "select PermissionObjectRegistry.ObjectId,PermissionObjectRegistry.ObjectDescription,PermissionObjectRegistry.ParentId,"
                                + "PermissionTypeValue.PermissionTypeId,PermissionTypeValue.PermissionDescription,PermissionTypeValue.PermissionTypeValue "
                                + "from PermissionObjectRegistry left join PermissionTypeValue on PermissionObjectRegistry.PermissionTypeId=PermissionTypeValue.PermissionTypeId";
                dbcommand = database.GetSqlStringCommand(strsql);
                ds = database.ExecuteDataSet(dbcommand);
            }
            catch { }
            return ds;
        }
        /// <summary>
        /// 获取控件的权限类型
        /// </summary>
        /// <param name="objid"></param>
        /// <returns></returns>
        public DataSet GetPermissionByObjectId(string objid)
        {
            DataSet ds = null;
            try
            {
                string strsql = "select PermissionTypeValue.PermissionDescription,PermissionTypeValue.PermissionTypeValue from PermissionObjectRegistry "
                                + "left join  PermissionTypeValue on PermissionObjectRegistry.PermissionTypeId=PermissionTypeValue.PermissionTypeId "
                                + "where PermissionObjectRegistry.ObjectId=@ObjectId and PermissionTypeValue.PermissionTypeValue is not null";
                dbcommand = database.GetSqlStringCommand(strsql);
                database.AddInParameter(dbcommand, "@ObjectId", DbType.String, objid);
                ds = database.ExecuteDataSet(dbcommand);
            }
            catch { }
            return ds;
        }

        /// <summary>
        /// 判断用户是否有该权限
        /// </summary>
        /// <param name="userid">角色id</param>
        /// <param name="objid">控件id</param>
        /// <param name="permissionValue">要判断的权限值</param>
        /// <returns></returns>
        public bool checkPermissionDeatilByUserId(string userid,string objid,int permissionValue)
        {
            bool result = false;
            try
            {
                string strsql = "select UserRole.RoleId,RolePermission.ObjectId,RolePermission.Permission from UserRole left join RolePermission "
                            + "on UserRole.RoleId =RolePermission.RoleId where UserRole.UserId=@UserId and RolePermission.ObjectId=@ObjectId";
                dbcommand = database.GetSqlStringCommand(strsql);
                database.AddInParameter(dbcommand, "@UserId", DbType.String, userid);
                database.AddInParameter(dbcommand, "@ObjectId", DbType.String, objid);
                DataSet ds = database.ExecuteDataSet(dbcommand);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int permissionOriginal = Convert.ToInt32(ds.Tables[0].Rows[i][2].ToString());
                    if (permissionValue <= permissionOriginal && Convert.ToBoolean(permissionOriginal & permissionValue))
                    {
                        result = true;
                        break;
                    }
                }
            }
            catch { }
            return result;
        }
        public bool checkPermissionDeatilByUserId(string userid, string objid)
        {
            bool result = false;
            try
            {
                string strsql = "select count(*) from UserRole left join RolePermission "
                            + "on UserRole.RoleId =RolePermission.RoleId where UserRole.UserId=@UserId and RolePermission.ObjectId=@ObjectId";
                dbcommand = database.GetSqlStringCommand(strsql);
                database.AddInParameter(dbcommand, "@UserId", DbType.String, userid);
                database.AddInParameter(dbcommand, "@ObjectId", DbType.String, objid);
                DataSet ds = database.ExecuteDataSet(dbcommand);
                if (Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString()) > 0)
                {
                    result = true;
                }
            }
            catch { }
            return result;
        }
        /// <summary>
        /// 根据权限类型Id获取权限信息(PermissionTypeId,PermissionDescription，PermissionTypeValue)
        /// </summary>
        /// <param name="typeid"></param>
        /// <returns></returns>
        public DataSet GetPermissionByType(string typeid)
        {
            DataSet ds = null;
            try
            {
                string strsql = "select * from permissiontypevalue where PermissionTypeId=@TypeId";
                dbcommand = database.GetSqlStringCommand(strsql);
                database.AddInParameter(dbcommand, "@TypeId", DbType.String, typeid);
                ds = database.ExecuteDataSet(dbcommand);
            }
            catch { }
            return ds;
        }


        /// <summary>
        /// 保存权限列表（菜单权限和窗体控件权限）
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="objlist"></param>
        /// <returns></returns>
        public bool SavePermissionList(string roleid, Hashtable objlist)
        {
            bool result = false;
            try
            {
                connection = database.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                string strsql;
                foreach (DictionaryEntry de in objlist)
                {
                    if (de.Value.ToString() == "0")
                    {
                        strsql = "delete from RolePermission where RoleId=@RoleId and ObjectId=@ObjectId";
                    }
                    else
                    {
                        strsql = "if exists (select * from RolePermission where RoleId=@RoleId and ObjectId=@ObjectId) " +
                                  "update RolePermission set Permission = @Permission where RoleId=@RoleId and ObjectId=@ObjectId " +
                                  "else insert into RolePermission values (@RoleId,@ObjectId,@Permission)";
                    }
                    dbcommand = database.GetSqlStringCommand(strsql);
                    database.AddInParameter(dbcommand, "@RoleId", DbType.String, roleid);
                    database.AddInParameter(dbcommand, "@ObjectId", DbType.String, de.Key.ToString());
                    database.AddInParameter(dbcommand, "@Permission", DbType.Int32, Convert.ToInt32(de.Value.ToString()));
                    database.ExecuteNonQuery(dbcommand, transaction);
                }
                transaction.Commit();
                result = true;
            }
            catch
            {
                transaction.Rollback();
                connection.Close();
            }
            finally
            {
                connection.Close();
            }
            return result;
        }


        
    }
}