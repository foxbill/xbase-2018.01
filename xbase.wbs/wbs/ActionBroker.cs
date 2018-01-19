#define DEBUG
#undef DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xData;
using wbs.wbap;

using xBase;
//using XMenu;
using Newtonsoft.Json;
//using XValidation;
using xBase.Umc;
using System.Data;
using XSecurity.Interface;

namespace wbs
{
    public enum MethodType
    {
        UnKnown = 0,
        Statement,
        FlowControl
    }

    /// <summary>
    /// 实参类型
    /// </summary>
    public enum RealParamFlagType
    {
        /// <summary>
        /// 元素
        /// </summary>
        String = 0,
        /// <summary>
        /// 表格
        /// </summary>
        Table,
        /// <summary>
        /// 函数地址
        /// </summary>
        Addr
    }

    /// <summary>
    /// 反射并运行一个方法 
    /// </summary>
    public class MethodBroker
    {
        private ActionSchema methodSchema;

        public MethodBroker(ActionSchema methodSchema)
        {
            this.methodSchema = methodSchema;
        }

        public object Invoke(string sessionId, WbdlSchema wbdlSchema, WbapRequest request)
        {

            Object[] parameters = new Object[methodSchema.Parameters.Count];
            for (int j = 0; j < methodSchema.Parameters.Count; j++)
            {
                ParameterSchema paramSchema = methodSchema.Parameters[j];

                object paramValue = null;
                if (string.IsNullOrEmpty(paramSchema.Value))
                {
                    parameters[j] = null;
                    continue;
                }

                if (paramSchema.Value[0] == '#')
                    paramValue = request.ElementBinds.GetTable(paramSchema.Value.Remove(0, 1), wbdlSchema);
                else if (paramSchema.Value[0] == '$')
                    paramValue = request.ElementBinds[paramSchema.Value];
                else
                    paramValue = paramSchema.Value;

                parameters[j] = paramValue;
            }

            Object invokeRet = Umc.InvokeFunction(sessionId, null, methodSchema.MethodName, parameters);


            return invokeRet;
        }

    }

    /// <summary>
    /// 动作接口实现类
    /// </summary>
    public class ActionBroker : IActionBroker
    {
        /// <summary>
        /// 变量标识符
        /// </summary>
        public static readonly char[] VAR_TYPES = { '@', '#', '$' };
        /// <summary>
        /// 变量分隔符
        /// </summary>
        /// 
        public const decimal DETA = 0.00001m;
        public const char VAR_SPLITOR = ';';
        private WbapRequest request;
        private ActionFlowSchema _ActionSchema;
        private WbdlSchema schema;
        private string sessionId;
        private PageController pageController;

        private const string GetMenu = "GETMENU";
        private const string IfElseGoto = "IFELSEGOTO";
        private const string Goto = "GOTO";
        private const string EqualGoto = "EQUALGOTO";
        private const string Break = "BREAK";
        private const int BREAK = -99;
        private string actionId;

        /// <summary>
        /// Action 编号
        /// </summary>
        public string ActionId
        {
            get { return actionId; }
            set { actionId = value; }
        }
        private ISecurity _ISec;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ActionBroker(WbapRequest wbapRequest, ActionFlowSchema actionSchema, PageController pageController, string sessionId, WbdlSchema schema, ISecurity secHandler)
        {
            request = wbapRequest;
            _ActionSchema = actionSchema;
            this.pageController = pageController;
            this.sessionId = sessionId;
            this.schema = schema;
            // this.Logger = new XLogging.XLoggingService();
            _ISec = secHandler;
            actionId = wbapRequest.ActionId;
            //在开始已经update
            // UpdatePage(wbapRequest);
        }

        //private void UpdatePage(WbapRequest request)
        //{
        //    foreach (KeyValuePair<string, object> keyValue in request.ElementBinds)
        //    {
        //        string elementId = keyValue.Key;
        //        object obj = keyValue.Value;

        //        if (elementId[0].Equals('#')) throw new XException("非法的ElementId:" + elementId);
        //        if (elementId[0].Equals('$')) throw new XException("非法的ElementId:" + elementId);
        //        if (elementId[0].Equals('@')) throw new XException("非法的ElementId:" + elementId);

        //        if (obj is WbapList)
        //        {
        //            pageController.SetWbapList(elementId, (WbapList)obj);
        //        }
        //        else if (obj is string)
        //        {
        //            pageController.SetStringElement(elementId, (string)obj);
        //        }
        //    }

        //}

        private WbapResponse _ExecuteFar()
        {
            WbapResponse response = new WbapResponse();
            response.PageName = this.request.PageName;

            Dictionary<string, Object> namedParams = new Dictionary<string, object>();

            foreach (KeyValuePair<string, Object> elementItem in request.ElementBinds)
            {
                string retElement = null;
                if (elementItem.Key.Equals("ReturnValue"))
                {
                    retElement = elementItem.Value.ToString();
                }
                else
                {
                    namedParams.Add(elementItem.Key, GetParamVarValue(elementItem.Value.ToString()));
                }
            }

            //    Umc.InvokeFunction("sid", "msid", request.ActionId, namedParams);
            return response;

        }

        private WbapResponse _Execute()
        {
            WbapResponse response = new WbapResponse();
            response.PageName = this.request.PageName;
            return __Execute(response);
        }

        private WbapResponse __Execute(WbapResponse response)
        {
            //CheckPermissionRetrun ret = _ISec.CheckObjectPermission(sessionId, _ActionSchema.Id, 1);
            //if (ret != CheckPermissionRetrun.Yes)
            //{
            //    response.ErrorNo = Convert.ToInt32(ret);
            //    response.Message = "Access denied:" + ret.ToString();
            //    return response;
            //}


            for (int i = request.Step; i < _ActionSchema.Actions.Count; i++)
            {

                ActionSchema methodSchema = _ActionSchema.Actions[i];

                #region client action
                if (methodSchema.IsRunAtClient())
                {
                    BuildAction(_ActionSchema.Id, i, response.Action);
                    if (response.Action.Request.ActionId != null && response.Action.Request.ActionId != "")
                        return response;
                    else
                        break;
                }
                #endregion

                string retVarName = methodSchema.ReturnValue;

                //#region Menu


                //if (methodSchema.MethodName.ToUpper() == GetMenu)
                //{
                //    if (methodSchema.Parameters[0].Value[0] == '$')
                //        methodSchema.Parameters[0].Value = (string)request.ElementBinds[methodSchema.Parameters[0].Value];
                //    XMenuObject xMenu = GetMenuBySession(this.sessionId, methodSchema.Parameters[0].Value);
                //    if (!response.ElementBinds.ContainsKey(retVarName))
                //        response.ElementBinds.Add(retVarName, xMenu);
                //    else
                //        response.ElementBinds[retVarName] = xMenu;
                //    continue;
                //}
                //#endregion

                #region preocess params
                Object[] parameters = new Object[methodSchema.Parameters.Count];
                for (int j = 0; j < methodSchema.Parameters.Count; j++)
                {

                    ParameterSchema paramSchema = methodSchema.Parameters[j];

                    object paramValue = null;
                    if (string.IsNullOrEmpty(paramSchema.Value))
                    {
                        parameters[j] = null;
                        continue;
                    }

                    if (paramSchema.Value[0] == '#')
                        paramValue = request.ElementBinds.GetTable(paramSchema.Value.Remove(0, 1), schema);
                    else if (paramSchema.Value[0] == '$')
                        paramValue = request.ElementBinds[paramSchema.Value];
                    else
                        paramValue = paramSchema.Value;
                    //paramValue = GetParamVarValue(paramSchema.Value);

                    if (paramValue == null)
                    {
                        //        BuildAction(_ActionSchema.Id, i, response.Action);
                        //       return response;
                    }

                    parameters[j] = paramValue;
                }

                #endregion

                #region flow control
                int idx = DetectFlowCtrlExec(methodSchema.MethodName, parameters);
                if (idx != -1)
                {
                    if (idx == BREAK) return response;
                    i = idx - 1;
                    continue;
                }

                #endregion

                #region Invoke
                Object invokeRet = null;

                //try
                //{
                invokeRet = Umc.InvokeFunction(sessionId, null, methodSchema.MethodName, parameters);
                //}
                //catch (Exception e)
                //{
                //    if (e.InnerException != null)
                //        throw e.InnerException;
                //    throw (e);
                //}

                #endregion

                ProcessMethodReturn(invokeRet, response.ElementBinds);
                if (!string.IsNullOrEmpty(retVarName))
                {
                    if (!response.ElementBinds.ContainsKey(retVarName))
                        response.ElementBinds.Add(retVarName, invokeRet);
                    else
                        response.ElementBinds[retVarName] = invokeRet;
                }

                //临时作废，考虑用GetType()分类型处理的方法
                #region process return value
                //if (retVarName != null && retVarName != "" && retVarName[0] == VAR_TYPES[(int)RealParamFlagType.Element])
                //{
                //    retVarName = retVarName.Remove(0, 1);
                //    if (methodReturnObj is CodeTable)
                //    {
                //        Dictionary<string, string> option = new Dictionary<string, string>();

                //        foreach (CodeTableData codeTableData in (methodReturnObj as CodeTable).Datas)
                //        {
                //            option.Add(codeTableData.Id, codeTableData.Text);
                //        }
                //        if (!response.ElementBinds.ContainsKey(retVarName))
                //            response.ElementBinds.Add(retVarName, option);
                //        else
                //            response.ElementBinds[retVarName] = option;

                //    }
                //    else
                //    {
                //        if (!request.ElementBinds.ContainsKey(retVarName))
                //        {
                //            request.ElementBinds.Add(retVarName, methodReturnObj);
                //        }
                //        else
                //        {
                //            request.ElementBinds[retVarName] = methodReturnObj;
                //        }
                //        if (!response.ElementBinds.ContainsKey(retVarName))
                //            response.ElementBinds.Add(retVarName, methodReturnObj);
                //        else
                //            response.ElementBinds[retVarName] = methodReturnObj;
                //    }
                //}

                //else if (retVarName != null && retVarName != "" && retVarName[0] == VAR_TYPES[(int)RealParamFlagType.Table])
                //{
                //    XTable table = (XTable)methodReturnObj;
                //    response.ElementBinds.ImportTable(table, pageController, retVarName.Remove(0, 1));
                //}
                //else if (retVarName != null && retVarName != "")
                //{
                //    if (methodReturnObj is MapTable)
                //    {
                //        WebLookupList lookupList = new WebLookupList();
                //        lookupList.ImportMapTable(methodReturnObj as MapTable, pageController);
                //        if (!response.ElementBinds.ContainsKey(retVarName))
                //            response.ElementBinds.Add(retVarName, lookupList);
                //        else
                //            response.ElementBinds[retVarName] = lookupList;
                //    }
                //    else
                //    {
                //        if (!response.ElementBinds.ContainsKey(retVarName))
                //            response.ElementBinds.Add(retVarName, methodReturnObj);
                //        else
                //            response.ElementBinds[retVarName] = methodReturnObj;
                //    }
                //}
                #endregion
            }
            //XData xData = Umc.FindObject<XData>(sessionId, null);
            //if (xData != null)
            //    xData.Commit();
            return response;
        }

        private void ProcessMethodReturn(object methodReturnObj, WbapDataBody responseDataBody)
        {
            //需要考虑用事件的方式通知pageControl要送回前端的数据内容，或者统一用HashDataTable的方式返回；
            if (methodReturnObj == null)
            {
                return;
            }
            else if (methodReturnObj is DataRow[])
            {
                pageController.FillDataBodyWithDataRows(responseDataBody, methodReturnObj as DataRow[]);
            }
            else if (methodReturnObj is Dictionary<string, DataRow[]>)
            {
                pageController.FillDataBodyWithHashRows(responseDataBody, methodReturnObj as Dictionary<string, DataRow[]>);
            }

        }

     //   private XMenuObject GetMenuBySession(string sessionId, string xmenuName)
      //  {
          //  XMenuAdapter xma = new XMenuAdapter(xmenuName);
          //  XMenuObject xmo = null;
          //  xmo = xma.GetMenu();
          //  XMenuItem parent = null;
#if DEBUG
            return xmo;
#else
          //  xmo = GetPermissionXmenu(xmo, ref parent, "");
#endif
           // return xmo;
      //  }

       // private XMenuObject GetPermissionXmenu(XMenuObject xmo, ref XMenuItem parent, string parentId)
        //{
            //CheckPermissionRetrun ret = _ISec.CheckObjectPermission(this.sessionId, xmo.Id,1);
            //if (ret != CheckPermissionRetrun.Yes)
            //{
            //    if (parent != null)
            //        parent.NextLevel = null;
            //    else
            //        xmo = null;
            //}
            //else
            //{               
          //  for (int i = 0; i < xmo.Items.Count; i++)
           // {
             //   XMenuItem item = xmo.Items[i];
             //   string pid = string.IsNullOrEmpty(parentId) ? item.Id : parentId + "|" + item.Id;

//                if (!_ISec.CheckObjectPermission(pid, PermissionTypes.Desplay))
  //              {
    //                xmo.Items.RemoveAt(i);
      //              i--;
        //        }
          //      else
            //    {
              //      item.NextLevel = GetPermissionXmenu(item.NextLevel, ref item, pid);
               // }
           // }
            //}
           // return xmo;
       // }

        private int DetectFlowCtrlExec(string p, object[] parameters)
        {
            switch (p.ToUpper())
            {
                case IfElseGoto:
                    return GetMethodIndex(_IfElseGoto(parameters[0].ToString(), parameters[1].ToString(), parameters[2].ToString()));
                case Goto:
                    return GetMethodIndex(parameters[0].ToString());
                case EqualGoto:
                    return GetMethodIndex(_EqualGoto(parameters[0].ToString(), parameters[1].ToString()));
                case Break:
                    return BREAK;
            }
            return -1;
        }

        private int GetMethodIndex(string methodId)
        {
            for (int i = 0; i < _ActionSchema.Actions.Count; i++)
            {
                if (_ActionSchema.Actions[i].Id == methodId) return i;
            }
            return -1;
        }

        private void _BuildAction(string actionId, int step, WbapAction action)
        {

            ActionFlowSchema actionSchema = schema.Actions.FindItem(actionId);

            if (actionSchema == null)
            {
                throw (new E_CanNotFindActionSchema(actionId));
            }

            bool hasBuildedRequest = false;

            for (int i = step; i < actionSchema.Actions.Count; i++)
            {
                ActionSchema methodSchema = actionSchema.Actions[i];
                if (methodSchema.IsRunAtClient())
                {

                    if (hasBuildedRequest) break;
                    ClientAction clientAction = new ClientAction();
                    action.ClientActions.Add(clientAction);
                    BuildClientAction(methodSchema, clientAction);
                }
                else
                {
                    BuildServerRequest(action.Request, actionSchema, i);
                    hasBuildedRequest = true;
                    break;
                }
            }
        }

        private void BuildClientAction(ActionSchema methodSchema, ClientAction clientRequest)
        {
            clientRequest.FuncName = methodSchema.MethodName;
            clientRequest.ReturnValue = methodSchema.ReturnValue;
            foreach (ParameterSchema paramSchema in methodSchema.Parameters)
            {
                if (string.IsNullOrEmpty(paramSchema.Value))
                {
                    clientRequest.Parameters.Add(paramSchema.Value);
                    continue;
                }
                if (paramSchema.Value[0] == '$')
                    clientRequest.Parameters.Add(request.ElementBinds[paramSchema.Value]);
                else
                    clientRequest.Parameters.Add(paramSchema.Value);

            }
        }

        /// <summary>
        /// 建立前端的请求框架，以及需要前端提供数据的，数据框架
        /// </summary>
        /// <param name="streamRequest"></param>
        /// <param name="action"></param>
        /// <param name="Step"></param>
        private void BuildServerRequest(WbapRequest request, ActionFlowSchema action, int Step)
        {
            request.Step = Step;
            request.PageName = schema.Id;
            request.ActionId = action.Id;

            ////通过wbdl的DataSourceSchema ，建立前端请求发送数据的框架
            //foreach (TableSourceSchema tbSchema in schema.DataSource.Tables)
            //{
            //    request.ElementBinds.ImportTableSchema(tbSchema.TableName, schema);
            //}

            //通过Action的过程参数，建立前端请求发送数据的框架
            for (int i = Step; i < action.Actions.Count; i++)
            {

                ActionSchema methodSchema = action.Actions[i];

                if (methodSchema.IsRunAtClient()) break;

                for (int j = 0; j < methodSchema.Parameters.Count; j++)
                {
                    ParameterSchema paramSchema = methodSchema.Parameters[j];
                    if (string.IsNullOrEmpty(paramSchema.Value))
                    {
                        //streamRequest.ElementBinds.Add();
                        continue;
                    }
                    string[] varArray = paramSchema.Value.Split(VAR_SPLITOR);

                    foreach (string var in varArray)
                    {
                        if (Array.IndexOf(VAR_TYPES, var[0]) == (int)RealParamFlagType.String)
                        {
                            string key = var.Remove(0, 1);

                            if (!request.ElementBinds.ContainsKey(key))
                                request.ElementBinds.Add(key, "");
                        }
                        else if (Array.IndexOf(VAR_TYPES, var[0]) == (int)RealParamFlagType.Table)
                        {
                            //已经通过DataSourceSchema，全部建立好了，所以注释这行，但以后仍需考虑，通过参数建立，以优化效率
                            // streamRequest.ElementBinds.ImportTableSchema(var.Remove(0, 1), schema);
                        }
                        else if (var[0].Equals('$'))
                        {
                            request.ElementBinds.Add(var, request.ElementBinds[var]);
                        }
                    }
                }
            }
        }

        private object GetParamVarValueForObject(string paramVar)
        {
            if (paramVar.Length < 1) return paramVar;
            char varFlag = paramVar[0];

            string varName = paramVar.Remove(0, 1);

            if (varFlag == VAR_TYPES[(int)RealParamFlagType.String])
            {
                if (request.ElementBinds.ContainsKey(varName))
                    return request.ElementBinds[varName];
                else
                    throw new XException("Request var :'" + varName + "' not be got");

            }

            if (varFlag == VAR_TYPES[(int)RealParamFlagType.Table])
                return request.ElementBinds.GetTable(varName, schema);

            return paramVar;
        }

        private object[] GetParamVarValueForArray(string[] paramAry)
        {
            string[] valueAry = new string[paramAry.Length];

            for (int i = 0; i < paramAry.Length; i++)
            {
                valueAry[i] = (string)GetParamVarValueForObject(paramAry[i]);
            }

            return valueAry;
        }

        private object GetParamVarValue(string paramVar)
        {
            string[] aryParam = paramVar.Split(VAR_SPLITOR);

            if (aryParam.Length > 1)
                return GetParamVarValueForArray(aryParam);
            else
                return GetParamVarValueForObject(paramVar);

        }


        private bool CalculateCondition(string expr, ref bool valid)
        {
            Calculator cl = new Calculator();
            cl.OnGetVariableValue += new GetVariableValueHandle(cl_OnGetVariableValue);
            string ret = cl.GetResult(expr);
            bool b;
            valid = bool.TryParse(ret, out b);

            if (valid)
                return b;
            else
                throw (new Exception("表达式错误"));

        }

        private string cl_OnGetVariableValue(string varName)
        {
            if (request.ElementBinds.ContainsKey(varName))
                return (string)request.ElementBinds[varName];
            return null;
        }

        private string _IfElseGoto(string expr, string trueId, string falseId)
        {
            bool bValid = false;
            bool bResult = CalculateCondition(expr, ref bValid);
            if (bValid)
                return (bResult) ? trueId : falseId;
            else
                throw new Exception("条件表达式错误");
        }

        private string _EqualGoto(string expr, string equId)
        {
            bool bValid = false;
            bool bResult = CalculateCondition(expr, ref bValid);
            if (bValid)
                return (bResult) ? equId : "-1";
            else
                throw new Exception("条件表达式错误");
        }


        #region IActionBroker 成员
        /// <summary>
        /// 实现执行动作接口
        /// </summary>
        /// <returns></returns>
        public WbapResponse Execute()
        {
            return _Execute();
        }

        public WbapResponse ExecuteFar()
        {
            return _ExecuteFar();
        }

        #endregion


        #region IActionBroker 成员

        /// <summary>
        /// 构造动作
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="step"></param>
        /// <param name="action"></param>
        public void BuildAction(string actionId, int step, WbapAction action)
        {
            _BuildAction(actionId, step, action);
        }

        #endregion


    }
}
