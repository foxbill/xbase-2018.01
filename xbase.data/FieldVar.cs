using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.security;
using xbase.umc;
using xbase.Exceptions;

namespace xbase.data
{
    public static class FieldVar
    {
        public static string GetValue(string varName, Umc umc)
        {
            string name = varName.Remove(0, 2);
            int fid = name.LastIndexOf('.');
            string fieldName = name.Substring(fid + 1);
            string tableName = name.Substring(0, fid);
            DataSource table = umc.GetObject<DataSource>(tableName);
            
            if(table==null)
                throw new ESchemaFileException("不能找到变量定义的字段值，" + varName);


            Dictionary<string, string> rec = new Dictionary<string, string>();// table.ActiveRecord;

            if (rec != null && rec.ContainsKey(fieldName))
            {
                string value = rec[fieldName];
                if (value == null)
                    value = "NULL";
                return value ;
            }
            else if (rec == null || rec.Count < 1)
                return "NULL";
            else
                throw new ESchemaFileException("不能找到变量定义的字段值，" + varName);

        }

        internal static bool IsVar(string varName)
        {
            return (!string.IsNullOrEmpty(varName)) && varName.StartsWith("@@") && varName.Contains('.');
        }
    }
}
