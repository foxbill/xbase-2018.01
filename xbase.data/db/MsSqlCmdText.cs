using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.data.db
{
    public sealed class MsSqlCmdText
    {
        /// <summary>
        /// 获取字段信息存储过程名
        /// </summary>
        public const string GetFieldDefsSpName = "xsp_getFieldDef";

        /// <summary>
        /// 获取字段信息存储过程
        /// </summary>
        public const string GetFieldDefsSp =
            @"
            CREATE PROCEDURE " + GetFieldDefsSpName + @"
	            @TableName  varchar(100)='abill5'
            AS
            BEGIN
	            SELECT
	               tbName=case when a.colorder=1 then d.name else d.name end, 
	               colName=a.name, 
	               growMark=case when COLUMNPROPERTY(a.id,a.name,'IsIdentity')=1 then 'True' else 'False' end,  
	               pkName=case when exists(SELECT 1 FROM sysobjects where xtype= 'PK' and name in (  SELECT name FROM sysindexes WHERE indid in(  SELECT indid FROM sysindexkeys WHERE id = a.id AND colid=a.colid  ))) then 'True' else 'False' end,  
	               colIsUnique=case when exists(SELECT 1 FROM sysobjects where xtype= 'UQ' and name in (  SELECT name FROM sysindexes WHERE indid in(  SELECT indid FROM sysindexkeys WHERE id = a.id AND colid=a.colid  ))) then 'True' else 'False' end,  
	               colType=b.name,  
	               byteLength=a.length,  
	               colLength=COLUMNPROPERTY(a.id,a.name, 'PRECISION'),  
	               pointCount=isnull(COLUMNPROPERTY(a.id,a.name, 'Scale'),0),  
	               colIsNull=case when a.isnullable=1 then 'True'else 'False' end,  
	               colVal=isnull(e.text, ''),  
	               colNote=isnull(g.[value], ''),
	               title=isnull(t.[value], ''),
	               alias=isnull(al.[value], '')
	 
	            FROM syscolumns a  
	               left join systypes b on a.xtype=b.xusertype  
	               inner join sysobjects d on a.id=d.id and d.xtype= 'U' and d.name <> 'dtproperties' and d.name = @TableName
	               left join syscomments e on a.cdefault=e.id  
	               left join sys.extended_properties g on a.id=g.major_id and a.colid=g.minor_id and g.name='MS_Description'  
	               left join sys.extended_properties t on a.id=t.major_id and a.colid=t.minor_id and t.name='Title'  
	               left join sys.extended_properties al on a.id=al.major_id and a.colid=al.minor_id and al.name='Alias'  
	 
	            order by a.id,a.colorder;
            END

        ";


        public const string GetTableExtendPropSpName = "xsp_GetTableExtendProp";

        public const string GetTableExtendProp = @"
            CREATE PROCEDURE " + GetTableExtendPropSpName + @" 
	            @TableName  varchar(100)='abill5',
	            @PropName  varchar(100)='Title'
            AS
            BEGIN
                declare @Id varchar(30);

                select @Id=id from  sysobjects 
                   where name = @TableName;

                select value from sys.extended_properties where major_id=@Id and minor_id=0 and name =@PropName; 
            END
          ";
    }

}
