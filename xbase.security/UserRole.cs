using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;


namespace XSecurity
{
    public class UserRole
    {

    }

    public class UserRoleBO
    {
        Database database = DatabaseFactory.CreateDatabase();
        DbConnection connection;
        DbTransaction transaction;
        DbCommand dbcommand;

        /// <summary>
        /// 获取用户归属的角色组(RoleId,RoleDescription,RoleRemark,DefinerId,DefineDate,UserName)
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataSet GetRoleByUserId(string userid)
        {
            DataSet ds = null;
            try
            {
                dbcommand = database.GetSqlStringCommand("select a.*,UserAccount.UserName from (select [Role].RoleId,[Role].RoleDescription,[Role].RoleRemark,[Role].DefinerId,Role.DefineDate from [UserRole] " +
                                                        "left join [Role] on UserRole.RoleId =[Role].RoleId where UserRole.UserId=@UserId) a " +
                                                        "left join UserAccount on a.DefinerId =UserAccount.UserId");
                database.AddInParameter(dbcommand, "@UserId", DbType.String, userid);
                ds = database.ExecuteDataSet(dbcommand);
            }
            catch
            { }
            return ds;
        }
        /// <summary>
        /// 获取用户未归属的角色组
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataSet GetNotRoleByUserId(string userid)
        {
            DataSet ds = null;
            try
            {
                dbcommand = database.GetSqlStringCommand("select [Role].RoleId,[Role].RoleDescription,[Role].RoleRemark from [Role] where [Role] .RoleId not in (select UserRole.RoleId from UserRole where UserId=@UserId)");
                database.AddInParameter(dbcommand, "@UserId", DbType.String, userid);
                ds = database.ExecuteDataSet(dbcommand);
            }
            catch { }
            return ds;
        }
        /// <summary>
        /// 为用户增加角色组
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool AddRoleByUserId(string userid,string roleid)
        {
            bool result = false;
            try
            {
                dbcommand = database.GetSqlStringCommand("insert into UserRole(UserId,RoleId) values (@UserId,@RoleId)");
                database.AddInParameter(dbcommand, "@UserId", DbType.String, userid);
                database.AddInParameter(dbcommand, "@RoleId", DbType.String, roleid);
                if (database.ExecuteNonQuery(dbcommand) == 1)
                {
                    result = true;
                }
            }
            catch { }
            return result;
        }
        /// <summary>
        /// 为用户保存角色组列表
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool AddRoleListByUserId(string userid, List<string> roleid)
        {
            bool result = false;
            try
            {
                connection = database.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                dbcommand = database.GetSqlStringCommand("delete from UserRole where UserId=@UserId");
                database.AddInParameter(dbcommand, "@UserId", DbType.String, userid);
                database.ExecuteNonQuery(dbcommand, transaction);
                for (int i = 0; i < roleid.Count; i++)
                {
                    dbcommand = database.GetSqlStringCommand("insert into UserRole(UserId,RoleId) values (@UserId,@RoleId)");
                    database.AddInParameter(dbcommand, "@UserId", DbType.String, userid);
                    database.AddInParameter(dbcommand, "@RoleId", DbType.String, roleid[i]);
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
        /// <summary>
        /// 为用户删除角色组
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool DelRoleByUserId(string userid,string roleid)
        {
            bool result = false;
            try
            {
                dbcommand = database.GetSqlStringCommand("delete from UserRole where UserId=@UserId and RoleId=@RoleId");
                database.AddInParameter(dbcommand, "@UserId", DbType.String, userid);
                database.AddInParameter(dbcommand, "@RoleId", DbType.String, roleid);
                if (database.ExecuteNonQuery(dbcommand) == 1)
                {
                    result = true;
                }
            }
            catch { }
            return result;
        }
        /// <summary>
        /// 获取归属某角色组的所有用户
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public DataSet GetUserByRoleId(string roleid)
        {
            DataSet ds = null;
            try
            {
                dbcommand = database.GetSqlStringCommand("select UserRole.UserId,UserAccount.UserName,UserAccount.Spell  from UserRole left join UserAccount on UserRole.UserId =UserAccount.UserId where UserRole.RoleId =@RoleId order by UserRole.UserId ");
                database.AddInParameter(dbcommand, "@RoleId", DbType.String, roleid);
                ds = database.ExecuteDataSet(dbcommand);
            }
            catch
            { }
            return ds;
        }
        /// <summary>
        /// 获取未归属该角色组的所有用户
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public DataSet GetNotUserByRoleId(string roleid)
        {
            DataSet ds = null;
            try
            {
                dbcommand = database.GetSqlStringCommand("select * from UserAccount where UserAccount.UserId not in (select UserRole.UserId from UserRole where UserRole.RoleId =@RoleId)");
                database.AddInParameter(dbcommand, "@RoleId", DbType.String, roleid);
                ds = database.ExecuteDataSet(dbcommand);
            }
            catch
            { }
            return ds;
        }


    }
}
