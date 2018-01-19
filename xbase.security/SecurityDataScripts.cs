using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase;
using System.Data;
using xbase.security;

namespace xbase.security
{
    internal static class SecurityDataScripts
    {
        internal static string RepasswordSql =
            @"UPDATE [xsys_user_account] SET 
                [password]=@password
            WHERE  
                [user_id]=@Id";

        internal static string CreateSystemTablesSQL
        {
            get
            {
                return
                @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[xsys_user_account]') AND type in (N'U'))
                begin
                CREATE TABLE [dbo].[xsys_user_account](

                [id]  [int]  IDENTITY(1,1) NOT NULL,
                [user_id]  [varchar] (50) NULL,
                [display_name]  [varchar] (50) NULL,
                [password]  [varchar] (50) NULL,
                [create_date]  [varchar] (50) NULL,
                [disabled]  [bit]   NULL,
                [account_type]  [int]   NULL,
                [active_code]  [varchar] (20) NULL,
                [actived]  [bit]   NULL,
                [bind_email]  [varchar] (20) NULL,
                [bind_mobile]  [varchar] (20) NULL,
                [group_id]  [varchar] (20) NULL,

                CONSTRAINT [PK_xsys_user_account] PRIMARY KEY CLUSTERED ([id] ASC)
                )
                ON [PRIMARY];

                INSERT INTO [xsys_user_account] ([user_id], [password]) VALUES ('" + BuiltinUsers.admin + @"','" + Crypto.Encrypt("admin") + @"');

                end;


                IF NOT  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[xsys_role]') AND type in (N'U')) 
                begin
                CREATE TABLE [dbo].[xsys_role](

                [id]  [int]  IDENTITY(1,1) NOT NULL,
                [role_id]  [varchar] (50) NULL,
                [display_name]  [varchar] (50) NULL,
                [desctiption]  [varchar] (50) NULL,
                [role_type]  [int]  ,
                [remark]  [text]   NULL,

                CONSTRAINT [PK_xsys_role] PRIMARY KEY CLUSTERED ([id] ASC)
                )
                ON [PRIMARY]
                end;


                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[xsys_user_roles]') AND type in (N'U'))
                begin
                CREATE TABLE [dbo].[xsys_user_roles](

                [id]  [int]  IDENTITY(1,1) NOT NULL,
                [user_id]  [varchar] (50) NULL,
                [role_id]  [varchar] (50) NULL,

                CONSTRAINT [PK_xsys_user_roles] PRIMARY KEY CLUSTERED ([id] ASC)
                )
                ON [PRIMARY]
                end;

                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[xsys_role_permissions]') AND type in (N'U')) 
                begin
                CREATE TABLE [dbo].[xsys_role_permissions](

                [id]  [int]  IDENTITY(1,1) NOT NULL,
                [role_id]  [varchar] (50) NULL,
                [object_type]  [varchar] (50) NULL,
                [object_id]  [varchar] (50) NULL,
                [permission]  [int],

                CONSTRAINT [PK_xsys_role_permissions] PRIMARY KEY CLUSTERED ([id] ASC)
                )
                ON [PRIMARY]
                end;



                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[xsys_user_group]') AND type in (N'U'))
                begin
                CREATE TABLE [dbo].[xsys_user_group](

                [id]  [int]  IDENTITY(1,1) NOT NULL,
                [group_id]  [varchar] (50) NULL,
                [up_group_id]  [varchar] (50) NULL,
                [display_name]  [varchar] (50) NULL,

                CONSTRAINT [PK_xsys_user_group] PRIMARY KEY CLUSTERED ([id] ASC)
                )
                ON [PRIMARY]
                end;

                ";
            }

        }


        internal static string GetUserListSQL(string groupId)
        {
            string w;
            if (String.IsNullOrEmpty(groupId))
                w = "";
            else
                w = "where group_id='" + groupId + "'";

            return @"select  *  from xsys_user_account " + w;

        }


        internal static string InsertUserSql =
                 @"INSERT INTO  [xsys_user_account] 
                        ( [user_id],
                          [display_name],
                          [password],
                          [create_date],
                          [disabled],
                          [actived],
                          [bind_email],
                          [bind_mobile],
                          [group_id]
                       ) 
                       VALUES 
                      (
                        @Id,
                        @DisplayName,
                        @Password,
                        @create_date,
                        @IsDisable,
                        @IsActive,
                        @Email,
                        @Mobile,
                        @GroupId
                      )";



        internal const string UpdateUserSql =
                  @"UPDATE [xsys_user_account] SET 
                        [user_id]=@Id,
                        [display_name]=@DisplayName,
                        [disabled]=@IsDisable,
                        [actived]=@IsActive,
                        [bind_email]=@Email,
                        [group_Id]=@GroupId,
                        [bind_mobile]=@Mobile
                     WHERE  
                        [user_id]=@oldId";



        internal const string CheckUserExistsSQL =
           @"select  user_id, display_name  from xsys_user_account where user_id=@user_id";

        internal const string DeleteUserSQL =
           @"delete from xsys_user_account where user_id=@user_id";




        internal static string GetRoleListSQL()
        {
            return @"select * from xsys_role";
        }

        internal static string CheckRoleExistsSQL(string roleId)
        {
            return @"select  role_id  from xsys_role where role_id='" + roleId + "'";
        }

        internal const string InsertRoleSql =
              @"INSERT INTO  [xsys_role] 
                      ( 
                          [role_id],
                          [display_name],
                          [remark]
                       ) 
                       VALUES 
                      (
                          @Id,
                          @DisplayName,
                          @Remark
                      );";


        internal const string UpdateRoleSql =
              @"UPDATE [xsys_role] SET 
                   [role_id]=@Id,
                   [display_name]=@DisplayName,
                   [remark]=@Remark
                WHERE  
                   [role_id]=@oldId;";



        internal const string DeleteRoleSQL =
          @"delete from xsys_role where role_id=@Id";

        internal const string GetPermissionObjectsSql =
          @"select * from [xsys_role_permissions] where role_id=@roleId and (object_type like @objType or object_type is null)";


        internal static string SavePermissionSql(string roleId, string objType, string objId, PermissionTypes permission)
        {
            DataTable tb = SecuritySettings.Execute(@"select * from xsys_role_permissions where role_id='" +
                                roleId + "' and object_type='" + objType + "' and object_id='" + objId + "'");

            int intPer = (int)permission;

            if (tb.Rows.Count > 0)
            {
                return @"UPDATE [xsys_role_permissions]  "
                 + " SET [permission]=" + intPer
                 + " where role_id='" + roleId + "' and object_type='" + objType + "' and object_id='" + objId + "'";
            }
            else
            {
                return @"INSERT INTO  [xsys_role_permissions] 
                        ( [role_id],
                          [object_type],
                          [object_id],
                          [permission]
                       ) 
                       VALUES 
                      (" +
                            "'" + roleId + "'," +
                            "'" + objType + "'," +
                            "'" + objId + "', " +
                            intPer +
                       ");"
                    ;
            }

        }

        internal static string DeleteRolePermissionSQL(string roleId, string objectType, string objectId)
        {
            return @"delete from xsys_role_permissions "
                 + " where role_id='" + roleId + "' and object_type='" + objectType + "' and object_id='" + objectId + "'";
        }

        internal const string deleteUserRoleSQL =
                @"delete from xsys_user_roles  where role_id= @roleId and user_id=@userId";


        internal static string GetUserRoleListSQL(string userId)
        {
            return @"select * from xsys_role where role_id in (select role_id from xsys_user_roles where user_id='" + userId + "')";
        }

        internal const string CheckUserRolesSQl =
           @"
              select 
                 count(*) from xsys_user_roles
              where 
                 role_id=@roleId and user_id =@userId;
         ";

        internal const string AppendUserRolesSQl =
                      @"INSERT INTO  [xsys_user_roles] 
                        ( [role_id],
                          [user_id]
                       ) 
                       VALUES 
                       ( 
                            @roleId ,
                            @userId
                       )";


        internal const string RoleObjectPermissionSql =
            @"Select 
                  permission 
              From 
                  xsys_role_permissions
              Where
                  object_id=@objectId and
                  role_id=@roleId";

        internal const string ExistsRoleObjectPermission =
            @"Select 
                count(*)
              From 
                  xsys_role_permissions
              Where
                  object_id=@objectId and
                  role_id=@roleId";

        public static string SetRoleObjectPermissionSql =
        @"Delete From 
              xsys_role_permissions
          Where
             object_id=@objectId and
             role_id=@roleId;
          insert into  
             xsys_role_permissions(
                role_id,
                object_id,
                permission
             )
             values(
                @roleId,
                @objectId,
                @permission
             )"
        ;

        public const string HasPermissionObject = "	select COUNT(object_id) from xsys_role_permissions where object_id=@objectId;";

    }
}
