using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.BaseTypes;
using System.Reflection;
using xbase.Exceptions;

namespace xbase.ObjectAction
{
    public class Action
    {
        private ActionData actionData;
        private object obj;

        public Action(object obj, ActionData actionData)
        {
            this.actionData = actionData;
            this.obj = obj;
        }

        public object invoke()
        {

            SetObjPropertis();

            if (String.IsNullOrEmpty(actionData.MethodName))
                return null;

            Type t = obj.GetType();
            MethodInfo m = t.GetMethod(actionData.MethodName);
            if (m == null)
            {
                throw new XException("对象方法没有找到" + actionData.MethodName);
            }


            object[] parameters = GetParameters(m, actionData.Options.MethodParams);
            return m.Invoke(obj, parameters);

        }


        private object[] GetParameters(MethodInfo methodInfo, IdsObjectList<IdValueObject> ParamDatas)
        {
            ParameterInfo[] paramInfos = methodInfo.GetParameters();

            object[] ret = new object[paramInfos.Length];

            for (int i = 0; i < paramInfos.Length; i++)
            {
                ParameterInfo pInfo = paramInfos[i];
                if (ParamDatas.ContainsId(pInfo.Name))
                    ret[i] = ParamDatas.GetItem(pInfo.Name);
            }
            return ret;
        }

        private void SetObjPropertis()
        {
            IdsObjectList<IdValueObject> Props = actionData.Options.Propertys;
            ActionUtils.SetObjProperties(obj, Props);
        }

    }
}
