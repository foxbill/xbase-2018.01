using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.BaseTypes;
using System.Reflection;

namespace xbase.ObjectAction
{
    public static class ActionUtils
    {
        public static void SetObjProperties(object obj, IdsObjectList<IdValueObject> properties)
        {
            for (int i = 0; i < properties.Count; i++)
            {
                try
                {
                    Type t = obj.GetType();
                    object propValue = properties[i].Value.Trim();
                    string propName = properties[i].Id.Trim();
                    PropertyInfo p = t.GetProperty(propName);
                    p.SetValue(obj, propValue, null);
                }
                catch (Exception e)
                {
                    var err = new Exception("设置对象状态时，发生错误,属性名：" + properties[i].Id, e);
                    throw err;
                }
            }
        }
    }
}
