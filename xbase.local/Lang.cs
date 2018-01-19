using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.local
{
    public sealed class Lang
    {
        public const string NoUploadFile = "需要上传文件,请选择上传文件。";
        public const string UploadSucceed = "文件长传成功。";
        public const string UploadFail = "文件上传失败。";
        public const string CanNotIsNull = "不能为空";
        public const string ObjectIsNotVisualWbo = "被调用的组件不是VisualWbo对象，无法执行显示";
        public const string LicenseErr = "系统授权过期，请重新激活";
        public const string Loading = "正在加载数据...";
        public const string NotAssignedSQL = "查询命令没有指定";
        public const string NoMainKey = "数据库没有指定主键不能被打开";
        public const string DataSourceNameIsNull = "数据源名称为空，不能打开数据";
        public const string UpdateNoKey = "没有定义主键字段，数据不能被更新";


        public const string DataSourceNotSelectCommand = "数据源没有指定查询命令";

        public const string DateTimeFormat = "yyyy-MM-dd hh:mm:ss";
        public const string NoUpLoadCommand = "数据的更新命令没有定义，不能执行数据更新";
        public const string NotSupportsTableDirectCommand = "DatabaseAdmin支持TableDirect类型的SQL命令";
        public const string RowNoKeyField = "行数据里面不能发现主键字段，不能获取指定的行";
        public const string FieldNotFind = "数据库中不存在字段:{0}";
        public const string SchemaColNotFieldAndNotExpression = "列'{0}'指定数据字段无法找到也没指定计算公式，不能显示数据";
        public const string unknowDbType = "不能识别表'{0}'字段'{1}'的字段类型";
        public const string DatabaseNotSuportsDataType = "数据库不只是这种数据类型{0}";

        public const string WrongObjectRequestName = "{0}是非法的对象调用名";
        public const string RequestNameIsNull = "请求名称为空，不能执行调用";
        public const string FileExists = "{0}已经存在，请先删除";
        public const string WboTypeNotMatchComId = "提交的Wbo对象和组件'{0}'的类型不匹配";

        public const string UploadFileNoData = "上传的文件中没有任何数据，不能确定数据类型";

        public const string RowNoOldVer = "要进行对比更新的行没有版本数据，无法进行对比更新";

        public const string FormNotItem = "表单没有定义任何输入项目，不能创建";

        public const string NoSpecifyDataSource = "没有指定数据源";
        public const string OlapNoDefineLevel = "级别没有定义";

        public const string OlapLevelOver = "超出定义的级别级数";
        public const string AssemblyCannotLoad = "程序集{0},不能被装载";
        public const string TypeCannotLoad = "程序集{0},不能被装载类型{1}";

        public const string DataSourceNotTableNameCannotUploadFile = "数据源没有指定原始表名，无法取得文件上传目录";

        public const string NotFindMethod = "在组件{0}中没有发现参数个数为{1}的方法{2}";

        public const string AccreditSystemErr = "授权系统故障，请联系技术厂家";

        public const string AccreditFileErr = "授权文件不合法";

        public const string FiledTypeMustFill = "字段'{0}',类型必须填写。";

        public const string DsCommandParamsNotDefined = "数据源的命令参数没定义'{0}'";
        public const string WboDllNotUpload = "组件{0}没有上传Dll文件，不能被加载";
        public const string RegWboMustIsDll = "组件注册，必须上传Dll文件";
        public const string NoFileName = "文件名没有指定，不能上传文件";

        public const string SubTableNoDefine = "子表'{0}'没有在数据源'{1}'中定义";
        public const string SubTableSelCommandTypeOnlyIsTable = "子表'{0}'的查询命令类型必须是‘表’";
    }

}
