using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.data
{
    /// <summary>
    /// 数据源配置参数
    /// </summary>
    public static class DataSourceConst
    {
        /// <summary>
        /// 物理数据列表显示的最大列数
        /// </summary>
        public const int MaxCol = 20;

        public static int DefPageSize = 15;

        /// <summary>
        /// 数据列表页面大小选项表
        /// </summary>
        public static int[] PageSizeList = new int[] { DefPageSize, 18, 20, 30, 50, 100, 150 };


        public const string PaginationSpText = @"
        /****** Object:  StoredProcedure [dbo].[sp_pagination]    Script Date: 06/01/2014 17:39:32 ******/
        CREATE PROCEDURE  [dbo].[sp_pagination]
        /*
        ***************************************************************
        ** 千万数量级分页存储过程 **
        ***************************************************************
        参数说明:
        1.TableName :表名称,视图
        2.PrimaryKey :主关键字
        3.OrderBy :排序语句，不带Order By 比如：NewsID Desc,OrderRows Asc
        4.PageNo :当前页码
        5.PageSize :分页尺寸
        6.Where :过滤语句，不带Where
        7.Group :Group语句,不带Group By

        ***************************************************************/
        (
        @TableName  varchar ( 1000 ),
        @PrimaryKey  varchar ( 100 ),
        @OrderBy  varchar ( 200 ) =  NULL ,
        @PageNo  int  =  1 ,
        @PageSize  int  =  10 ,
        @Fields  varchar ( 1000 ) =  '*' ,
        @Where  varchar ( 1000 ) =  NULL ,
        @Group varchar ( 1000 ) =  NULL,
        @Total int Output,
        @PageCount int Output
        )
        AS
        /*默认排序*/

        declare @CountSqlStr nvarchar(1000) = 'select @Total=COUNT(*) from ' + @TableName;
        if not (@Where is null or @Where='')
          set @CountSqlStr=@CountSqlStr +' where ' +@Where;
  
        exec sp_executesql @CountSqlStr,N'@Total int output',@Total Output;  

        set @PageCount=@Total / @PageSize;
        if @Total % @PageSize>0
           set @PageCount=@PageCount+1;
        IF  @OrderBy  IS NULL OR  @OrderBy =  ''
        SET  @OrderBy = @PrimaryKey
        DECLARE  @SortTable  varchar ( 100 )
        DECLARE  @SortName  varchar ( 100 )
        DECLARE  @strSortColumn  varchar ( 200 )
        DECLARE  @operator  char ( 2 )
        DECLARE  @type  varchar ( 100 )
        DECLARE  @prec  int
        /*设定排序语句.*/
        IF  CHARINDEX( 'DESC' ,@OrderBy)> 0
        BEGIN
        SET  @strSortColumn = REPLACE(@OrderBy,  'DESC' ,  '' )
        SET  @operator =  '<='
        END
        ELSE
        BEGIN
        SET  @strSortColumn=@OrderBy
        IF  CHARINDEX( 'ASC' , @OrderBy) >  0
        SET  @strSortColumn = REPLACE(@OrderBy,  'ASC' ,  '' )
        SET  @operator =  '>='
        END
        IF  CHARINDEX( '.' , @strSortColumn) >  0
        BEGIN
        SET  @SortTable = SUBSTRING(@strSortColumn,  0 , CHARINDEX( '.' ,@strSortColumn))
        SET  @SortName = SUBSTRING(@strSortColumn, CHARINDEX( '.' ,@strSortColumn) +  1 , LEN(@strSortColumn))
        END
        ELSE
        BEGIN
        SET  @SortTable = @TableName
        SET  @SortName = @strSortColumn
        END
        SELECT  @type=t.name, @prec=c.prec
        FROM  sysobjects o
        JOIN  syscolumns c  on  o.id=c.id
        JOIN  systypes t  on  c.xusertype=t.xusertype
        WHERE  o.name = @SortTable  AND  c.name = @SortName
        IF  CHARINDEX( 'char' , @type) >  0
        SET  @type = @type +  '('  +  CAST (@prec  AS varchar ) +  ')'
        DECLARE  @strPageSize  varchar ( 50 )
        DECLARE  @strStartRow  varchar ( 50 )
        DECLARE  @strFilter  varchar ( 1000 )
        DECLARE  @strSimpleFilter  varchar ( 1000 )
        DECLARE  @strGroup  varchar ( 1000 )
        /*默认当前页*/
        IF  @PageNo <  1
        SET  @PageNo =  1
        /*设置分页参数.*/
        SET  @strPageSize =  CAST (@PageSize  AS varchar ( 50 ))
        SET  @strStartRow =  CAST (((@PageNo -  1 )*@PageSize +  1 )  AS varchar ( 50 ))
        /*筛选以及分组语句.*/
        IF  @Where  IS NOT NULL AND  @Where !=  ''
        BEGIN
        SET  @strFilter =  ' WHERE '  + @Where +  ' '
        SET  @strSimpleFilter =  ' AND '  + @Where +  ' '
        END
        ELSE
        BEGIN
        SET  @strSimpleFilter =  ''
        SET  @strFilter =  ''
        END
        IF  @Group IS NOT NULL AND  @Group  !=  ''
        SET  @strGroup =  ' GROUP BY '  + @Group  +  ' '
        ELSE
        SET  @strGroup =  ''
        /*执行查询语句*/
        EXEC (
        '
        DECLARE @SortColumn '  + @type +  '
        SET ROWCOUNT '  + @strStartRow +  '
        SELECT @SortColumn=isnull('  + @strSortColumn +  ','''') FROM '  + @TableName + @strFilter +  ' '  + @strGroup +  ' ORDER BY '  + @OrderBy +  '
        SET ROWCOUNT '  + @strPageSize +  '
        SELECT '  + @Fields +  ' FROM '  + @TableName +  ' WHERE '  + @strSortColumn + @operator +  ' @SortColumn '  + @strSimpleFilter +  ' '  + @strGroup +  ' ORDER BY '  + @OrderBy +  '
        '
        )

        ";

        public const string PaginationSpName = "sp_pagination";
        public const string PagingPageNo = "@PageNo";
        public const string PagingPageSize = "@PageSize";
        public const string PagingPageCount = "@PageCount";
        public const string PagingTotal = "@Total";
        public const string ExProDbType = "DbType";
        public const string ExProDescription = "Description";
    }
}
