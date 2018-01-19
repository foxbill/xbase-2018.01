using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wbs;
using xbase.Interface;
using System.Reflection;
using xbase.wbs.wbdl;

namespace xbase.wbs
{
    public class Action
    {
        private Page page;
        private ActionSchema actionSchema;
        private object obj = null;
        private string dsId;
        private object returnValue;
        private int index;
        private Action parentAction;

        public Action(Page page, ActionSchema actionSchema, int index, Action parentAction)
        {
            this.page = page;
            this.actionSchema = actionSchema;
            this.index = index;
            this.parentAction = parentAction;

            this.dsId = actionSchema.DataSourceId;
            try
            {
                this.obj = page.DataSources[dsId];
            }
            catch
            {
                throw new E_NotFindDataObject(actionSchema.DataSourceId);
            }
        }

        public string ActionId
        {
            get
            {
                string ret = index.ToString();
                if (parentAction != null)
                    ret = parentAction.ActionId + "." + ret;
                return ret;
            }
        }

        private void InvokeControl()
        {
            FlowStatus fs = FlowStatus.Continue;
            IFlowControl ctrl = (obj as IFlowControl);
            while (ctrl.CheckDo() == FlowStatus.Continue)
            {
                InvokeSubActions();
                if (!ctrl.IsIterate) break;
            }
        }

        private void InvokeSubActions()
        {
            for (int i = 0; i < actionSchema.Actions.Count; i++)
            {
                Action act = new Action(this.page, actionSchema.Actions[i], i, this);
                act.Invoke();
            }
        }

        public bool Invoke()
        {
            if (actionSchema.Props != null)
            {
                if (!page.SetObjPropertis(actionSchema.Props, obj))
                    return false;
            }

            if (obj is IFlowControl)
                InvokeControl();

            //没有方法定义，就此获取页面后退出
            if (String.IsNullOrEmpty(actionSchema.MethodName))
            {

                //流程中途更新页元素面数据：
                page.GetElementDataFormObject(dsId, obj);

                return true;
            }

            WbdlDataSchema dataObjSchema = page.PageSchema.DataSources.GetItem(actionSchema.DataSourceId);
            Dictionary<string, string> parametes = new Dictionary<string, string>();

            bool needData = false;
            foreach (ParameterSchema paramSchema in actionSchema.Parameters)
            {
                //    Type paramInfo = umc.Umc.GetClassMethodParamInfo(dataObjSchema.DataType, actionSchema.MethodName, paramSchema.Id).ParameterType;

                string paramValue = page.CheckValueVar(paramSchema.Value.Trim());
                if (paramValue == null)
                {
                    needData = true;
                }
                else
                {
                    parametes.Add(paramSchema.Id, paramValue);
                }
            }
            if (needData)
            {
                return false;
            }



            try
            {
                //  this.returnValue = umc.Umc.InvokeMethod(obj, actionSchema.MethodName, parametes);
               // this.returnValue = umc.Umc.invoke(obj, dataObjSchema.DataType, actionSchema.MethodName, parametes);
            }
            catch (Exception e)
            {
                string inErrMessge = "";
                if (e.InnerException != null) inErrMessge = e.InnerException.Message;
                Exception err = new Exception("调用活动时间，发生错误，活动名：" + actionSchema.Title + ",错误内容：" + e.Message + inErrMessge, e);
                throw err;
            }

            page.GetElementDataFormObject(dsId, obj);

            InvokeSubActions();

            return true;

        }

        public bool isEndFlow()
        {
            foreach (DecisionControlSchema dcs in actionSchema.Decisions)
            {
                object targetValue = page.CheckValueVar(dcs.TargetValue);
                if (dcs.Target.Equals(ControlWord.ReturnValue, StringComparison.OrdinalIgnoreCase))
                {
                    return returnValue.Equals(targetValue);
                }

                Type t = obj.GetType();
                PropertyInfo p = t.GetProperty(dcs.Target);
                object v = p.GetValue(obj, null);

                if (targetValue.ToString().Equals(v.ToString(), StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

    }
}
