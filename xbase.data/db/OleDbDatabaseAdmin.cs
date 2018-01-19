using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
namespace xbase.data.db
{
    public class OleDbDatabaseAdmin : DatabaseAdmin
    {

        public OleDbDatabaseAdmin(Database db)
            : base(db)
        {
        }

        /// <summary>
        /// 创建数据库表
        /// </summary>
        /// <param name="tableDef"></param>
        /// <returns></returns>
        public override bool createTable(TableDef tableDef)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 返回数据库表集合
        /// </summary>
        /// <returns></returns>
        public override List<string> getTableNames()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 修改数据库表
        /// </summary>
        /// <param name="tableDef"></param>
        /// <returns></returns>
        public override bool modifyTable(TableDef tableDef)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建存储过程
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="procText"></param>
        /// <returns></returns>
        public override bool createProc(string procName, string procText)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 返回存储过程集合
        /// </summary>
        /// <returns></returns>
        public override List<string> getProcNames()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 返回存储过程内容
        /// </summary>
        /// <param name="procName"></param>
        /// <returns></returns>
        public override string getProcText(string procName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 修改存储过程
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="procText"></param>
        /// <returns></returns>
        public override bool modifyProc(string procName, string procText)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建触发器
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="triggerText"></param>
        /// <returns></returns>
        public override bool createTrigger(string tableName, string triggerName, string triggerText)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 返回触发器集合
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public override List<string> getTriggerNames(string tableName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 修改触发器
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="triggerText"></param>
        /// <returns></returns>
        public override bool modifyTrigger(string tableName, string triggerName, string triggerText)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 返回当前触发器内容
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="triggerName"></param>
        /// <returns></returns>
        public override string getTriggerText(string tableName, string triggerName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 返回当前表结束集合
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public override DataTable getConstraintTable(string tableName)
        {
            DataTable tb = null;


            return tb;

        }

        /// <summary>
        /// 返回约束内容
        /// </summary>
        /// <param name="contraintName"></param>
        /// <returns></returns>
        public override string getConstraintText(string contraintName, string tbName)
        {
            string strRet = "";
            return strRet;
        }

        /// <summary>
        /// 返回当前数据库字段数据类型
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, string> getFieldTypeList()
        {
            return null;
        }

        public override List<string> getViewNames()
        {
            return null;
        }

        public override DataTable getViewData(string viewName)
        {
            return null;
        }

        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="tbName"></param>
        /// <returns></returns>
        public override bool deleteTable(string tbName)
        {

            return false;
        }

        /// <summary>
        /// 删除视图
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public override bool deleteView(string viewName)
        {

            return false;
        }

        /// <summary>
        /// 删除过程
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public override bool deleteProcedure(string spName)
        {

            return false;
        }

        /// <summary>
        /// 删除触发器
        /// </summary>
        /// <param name="tgName"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public override bool deleteTrigger(string tgName)
        {

            return false;
        }

        public override List<string> GetPrimaryKeys(string tableName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool isIdentityField(string tableName, string fieldName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool isRowGuidField(string tableName, string fieldName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override DataTable getFieldDefsToTable(string tableName)
        {
            throw new NotImplementedException();
        }

        public override string getViewScript(string viewName)
        {
            throw new NotImplementedException();
        }

        public override void modifyViewScript(string viewName, string script)
        {
            throw new NotImplementedException();
        }

        public override bool ExistsSp(string spName)
        {
            throw new NotImplementedException();
        }



        public override DbType getDbType(string typeString)
        {
            throw new NotImplementedException();
        }

        public override string getTableTitle(string tableName)
        {
            throw new NotImplementedException();
        }

        public override string getTableDescription(string tableName)
        {
            throw new NotImplementedException();
        }

        public override void renameTable(string tableName, string newTableName)
        {
            throw new NotImplementedException();
        }

        public override void renameFieldName(string tableName, string fieldName, string newName)
        {
            throw new NotImplementedException();
        }

        public override string typeOfDotNetType(Type type)
        {
            throw new NotImplementedException();
        }

        public override string typeOfDbType(DbType dbType)
        {
            throw new NotImplementedException();
        }
    }
}
