using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.Validation;
using xbase.umc;
using xbase.Exceptions;
using xbase.data.db;
using xbase.umc.attributes;


namespace xbase.data.Validation
{
    [WboAttr(Title = "数据库表达式校验器", Description = "数据库表达式校验器")]
    public class DbExpressionValidator : BaseValidator
    {
        private string connect;
        private string dbProvider;
        private string expression;

        [WboPropertyAttr(Title = "连接名", Description = "连接名，缺省为空")]
        public string Connect
        {
            get { return connect; }
            set { connect = value; }
        }

        [WboPropertyAttr(Title = "条件表达式", Description = "验证合法性的条件表达式")]
        public string Expression
        {
            get { return expression; }
            set { expression = value; }
        }

        public override bool Check(string value)
        {
            //XDatabaseFactory dbfact = XDatabaseFactory.Instance;
            DatabaseAdmin db = DatabaseAdmin.getInstance(connect);
            object o = null;
            try
            {
                o = db.Database.ExecuteScalar("if  " + expression +" select 1 else select 0");
            }
            catch (Exception e)
            {
                throw new XException("表达式校验器在执行["+expression+"]时发生错误,"+e.Message);
            }


            if (o is bool)
                return (bool)o;

            string s = o.ToString();
            if (string.IsNullOrEmpty(s))
                return false;

            return s != "0";

        }


    }
}
