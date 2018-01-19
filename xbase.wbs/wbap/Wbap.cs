using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wbs;
using System.Reflection;
using xData;
using xBase;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using xBase.Umc;
using XSecurity.Interface;
using XSecurity;

namespace wbs.wbap
{

    public class E_MethodNotRegistor : XException { public E_MethodNotRegistor(string msg) : base(msg) { } }
    public class E_CanNotGetParamData : XException { public E_CanNotGetParamData(string msg) : base(msg) { } }
    public class E_CanNotFindActionSchema : XException { public E_CanNotFindActionSchema(string msg) : base(msg) { } }
    public class E_RunMethodException : XException { public E_RunMethodException(string msg) : base(msg) { } }
    public class E_CanNotFindFieldInFormSchame : XException { public E_CanNotFindFieldInFormSchame(string msg) : base(msg) { } }


    public class Wbap
    {
        public enum VarFlagType { Element = 0, Table };
        public static readonly char[] VAR_TYPES = { '@', '#' };
        public const char PARAM_SPLITOR = ';';

        private PageController pageCtr;
        private string sessionId;
        private ISecurity _ISec;
        private WbapDataBody requestDataMap = new WbapDataBody();
        /// <summary>
        /// 安全检查者对象
        /// </summary>
        public ISecurity ISecHandler
        {
            get { return _ISec; }
            set { _ISec = value; }
        }

        /// <summary>
        /// 协议解析类
        /// </summary>
        /// <param name="pageName"></param>
        /// <param name="sessionId"></param>
        public Wbap(PageController pageCtr)
        {
            this.sessionId = pageCtr.SessionId; 
            this.pageCtr = pageCtr;
        }

        private WbdlSchema WbdlSchema
        {
            get
            {
                return pageCtr.Schame;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public WbapResponse Initialize(string pageName, string sessionId)
        {
            WbapResponse resp = null;
            //CheckPermissionRetrun ret = _ISec.CheckObjectPermission(sessionId, pageName, 1);
            //if (ret != CheckPermissionRetrun.Yes)
            //{
            //    resp = new WbapResponse();
            //    resp.ErrorNo = (int)(ret);                
            //    // Convert.ToInt32
            //    resp.Message = "Access denied: " + ret.ToString();
            //    resp.PageName = pageName;
            //    return resp;
            //}

            //pageCtr = new PageController(pageName,sessionId);

            this.sessionId = sessionId;
           // this.schema = pageCtr.Schame;
            pageCtr.Initialize();

            WbapRequest request = new WbapRequest();
            request.ActionId = "OnLoad";
            request.Step = 0;
            request.PageName = pageName;

            #region 权限管理
            //ret =_ISec.CheckObjectPermission(sessionId, streamRequest.ActionId, 2);
            //if (ret != CheckPermissionRetrun.Yes)
            //{
            //    resp = new WbapResponse();
            //    resp.ErrorNo = Convert.ToInt32(ret);
            //    resp.Message = "Access denied:" + ret.ToString();
            //    resp.PageName = pageName;
            //    return resp;
            //}
            #endregion

            ActionBroker action = GetAction(request, ref resp, pageName, sessionId, _ISec);

            if (action != null)
                resp = action.Execute();
            


            //元素捆绑
            foreach (FieldBindSchema fieldBindSchema in WbdlSchema.FieldBinds)
            {
                //ret = _ISec.CheckObjectPermission(sessionId, fieldBindSchema.Id, 2);
                //if (ret == CheckPermissionRetrun.Yes)
                this.InitualElement(resp, fieldBindSchema);
            }

            //捆绑列表列属性
            foreach (DataListBindSchema listBindSchema in WbdlSchema.DataListBinds)
            {
                //ret = _ISec.CheckObjectPermission(sessionId, listBindSchema.Id, 2);
                //if (ret == CheckPermissionRetrun.Yes)
                //{
                foreach (FieldBindSchema fieldBindSchema in listBindSchema.Columns)
                {
                    //ret = _ISec.CheckObjectPermission(sessionId, fieldBindSchema.Id, 2);
                    //if (ret == CheckPermissionRetrun.Yes)
                    this.InitualElement(resp, fieldBindSchema);
                }
                //}                
            }

            //控件捆绑
            foreach (WbdlControl wbdlControl in WbdlSchema.Controls)
            {
                ActionSchema ms = wbdlControl.DataFunction;
                MethodBroker mb = new MethodBroker(ms);

                object invokeRet = mb.Invoke(sessionId, WbdlSchema, request);

                WbapControl wbapControl = new WbapControl();
                wbapControl.Data = invokeRet;
                wbapControl.VenderObject = wbdlControl.VenderObject;
                wbapControl.VenderUrl = wbdlControl.VerderUrl;
                resp.Controls.Add(wbdlControl.Id, wbapControl);   
            }




            //捆绑数据；
            pageCtr.FillDataBodyWithDataSet(resp.ElementBinds);
            //捆绑列表

            //foreach (PageDataList pageList in pageCtr.Page.PageLists.Values)
            //{
            //    string listId = pageList.ElementId;
            //    if (!resp.ElementBinds.ContainsKey(listId + WbapList.TypeMarker))
            //    {
            //        WbapList wbapList = pageCtr.GetWbapList(listId);
            //        resp.ElementBinds.Add(listId + WbapList.TypeMarker, wbapList);
            //    }

            //}



            //捆绑事件
            foreach (EventSchema eventBindSchema in WbdlSchema.EventBinds)
            {
                string actionId = eventBindSchema.ActionFlow;
                WbapEvent wbapEvent = new WbapEvent();
                wbapEvent.EventName = eventBindSchema.EventName;
                BuildAction(actionId, 0, wbapEvent.Action, request);
                resp.ElementBinds.AddEvent(eventBindSchema.Id, wbapEvent);
            }

            //替换全局变量

            foreach (string elementKey in resp.ElementBinds.Keys)
            {
                if (elementKey.Length > 0 && elementKey[0] == '$')
                    resp.ElementBinds[elementKey] = request.ElementBinds[elementKey];
            }

            return resp;

        }

        /// <summary>
        /// 取得动作
        /// </summary>
        /// <param name="wbapRequest"></param>
        /// <param name="response"></param>
        /// <param name="pageName"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public ActionBroker GetAction(WbapRequest wbapRequest, ref WbapResponse response, string pageName, string sessionId, ISecurity secHandler)
        {
            SetSessionInfo(wbapRequest);
            ActionBroker action = null;
            SaveRequestData(wbapRequest);
            if (response == null)
                response = new WbapResponse();
            response.PageName = pageName;
            
            //if (pageCtr == null || pageCtr.BuzObject.Id != pageName)
            //    pageCtr = new PageController(pageName,sessionId);

            this.sessionId = sessionId;
            
            ActionFlowSchema actionSchema = null;

            if (pageCtr.Schame.Actions.FindItem(wbapRequest.ActionId) == null)
            {
                if (wbapRequest.ActionId.Equals("onLoad", StringComparison.OrdinalIgnoreCase))
                    return null;
                else
                {
                    action = new ActionBroker(wbapRequest, actionSchema, pageCtr, sessionId, WbdlSchema, secHandler);
                    response = action.ExecuteFar();
                    return null;
                }
            }

            try
            {
                actionSchema = pageCtr.Schame.Actions.GetItem(wbapRequest.ActionId);
            }
            catch (Exception)
            {
                throw (new E_CanNotFindActionSchema(wbapRequest.ActionId));
            }

            if (actionSchema == null)
                throw (new E_CanNotFindActionSchema(wbapRequest.ActionId));
            action = new ActionBroker(wbapRequest, actionSchema, pageCtr, sessionId, WbdlSchema, _ISec);
            return action;
        }

        private void SetSessionInfo(WbapRequest wbapRequest)
        {
            string sessionKey = "$SessionID";
            string userIdKey = "$UserID";
            string userIdStringKey = "$UserIDString";
            string userNameKey = "$UserName";
            #region set session info
            UserContext uc = UserContextContainer.GetUserContextBySessionId(this.sessionId);
            if (uc != null)
            {
                //wbapRequest.ElementBinds.Add("$CurrentYear", DateTime.Now.Year.ToString());
                //wbapRequest.ElementBinds.Add("$CurrentMonth", DateTime.Now.Month.ToString());
                //wbapRequest.ElementBinds.Add("$CurrentDay", DateTime.Now.Day.ToString());
                //wbapRequest.ElementBinds.Add("$CurrentWeek", System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek));
                if (!wbapRequest.ElementBinds.ContainsKey(sessionKey))
                    wbapRequest.ElementBinds.Add(sessionKey, this.sessionId);
                else
                    wbapRequest.ElementBinds[sessionKey] = this.sessionId;
                if (!wbapRequest.ElementBinds.ContainsKey(userIdKey))
                    wbapRequest.ElementBinds.Add(userIdKey, uc.UserId);
                else
                    wbapRequest.ElementBinds[userIdKey] = uc.UserId;
                if (!wbapRequest.ElementBinds.ContainsKey(userIdStringKey))
                    wbapRequest.ElementBinds.Add(userIdStringKey, "'" + uc.UserId + "'");
                else
                    wbapRequest.ElementBinds[userIdStringKey] = "'" + uc.UserId + "'";
                if (!wbapRequest.ElementBinds.ContainsKey(userNameKey))
                    wbapRequest.ElementBinds.Add(userNameKey, uc.UserName);
                else
                    wbapRequest.ElementBinds[userNameKey] = uc.UserName;
            }
            #endregion
        }

        /// <summary>
        /// 取得环境变量
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public RequestEnv GetRequestEnv(ActionFlowSchema action)
        {
            RequestEnv requestEnv = new RequestEnv();
            requestEnv.FormId = pageCtr.Schame.Id;
            requestEnv.ActionId = action.Id;

            foreach (ActionSchema method in action.Actions)
            {
                MethodRef methodBody = new MethodRef();
                methodBody.MethodName = method.MethodName;
               // methodBody.RunAt = method.RunAt;
                methodBody.ReturnValue = method.ReturnValue;
                requestEnv.Body.Add(method.Id, methodBody);

                foreach (ParameterSchema parameter in method.Parameters)
                {
                    Object parameterValue = EncloseClientRequestParam(parameter.Value);

                    methodBody.Parameters.Add(parameter.Id, parameterValue);

                }

            }

            return requestEnv;
        }

        /// <summary>
        /// 序列化WbapResponse
        /// </summary>
        /// <param name="wbapResponse"></param>
        /// <returns></returns>
        public static string SerializeResponse(WbapResponse wbapResponse)
        {
            string respText = JsonConvert.SerializeObject(wbapResponse);
            return respText;
        }

        /// <summary>
        /// 反序列化输入流
        /// </summary>
        /// <param name="requestText"></param>
        /// <returns></returns>
        public static WbapRequest DeserializeRequest(string requestText)
        {
            WbapRequest wbapRequest = new WbapRequest();
            JObject jObj = JObject.Parse(requestText);

            wbapRequest.PageName = jObj.Value<string>("PageName");
            wbapRequest.ActionId = jObj.Value<string>("ActionId");
            wbapRequest.Url = jObj.Value<string>("Url");

            JObject elementBinds = (JObject)jObj.Value<JObject>("ElementBinds");

            if (elementBinds == null) return wbapRequest;
            wbapRequest.Step = jObj.Value<int>("Step");
            foreach (JProperty elementBind in elementBinds.Properties())
            {
                string elementName = elementBind.Name;
                if (WbapDataBody.ElementIsDataType(elementName, WbapDataType._List))
                {
                    string s = elementBind.Value.ToString();
                    WbapList dataList = JsonConvert.DeserializeObject<WbapList>(s);
                    wbapRequest.ElementBinds.Add(elementBind.Name, dataList);
                }
                else if (WbapDataBody.ElementIsDataType(elementName, WbapDataType._String))
                {
                    string value = elementBind.Value.Value<string>();
                    wbapRequest.ElementBinds.Add(elementName, value);
                }
            }


            return wbapRequest;
        }


        private Object EncloseClientRequestParam(string param)
        {
            if (param == null)
            {
                return param;
            }

            if (!param.StartsWith("#"))
            {
                return param;
            }

            param = param.Remove(0, 1);

            XTable_1 xTable = new XTable_1();
            WbdlSchema schame = pageCtr.Schame;
            int i = 0;
            string[] fieldNames = { };

            foreach (FieldBindSchema fieldBind in schame.FieldBinds)
            {
                if (fieldBind.TableId.Equals(param, StringComparison.OrdinalIgnoreCase))
                {
                    i++;
                    Array.Resize<string>(ref fieldNames, i);
                    fieldNames[i - 1] = fieldBind.Id;
                }
            }


            foreach (DataListBindSchema dataList in schame.DataListBinds)
            {
                foreach (FieldBindSchema fieldBind in dataList.Columns)
                {
                    if (fieldBind.TableId.Equals(param, StringComparison.OrdinalIgnoreCase))
                    {
                        i++;
                        Array.Resize<string>(ref fieldNames, i);
                        fieldNames[i - 1] = fieldBind.Id;
                    }
                }
            }

            //  xTable.FieldNames = fieldNames;
            XDataRow dataRow = new XDataRow();
            //            dataRow.Values = new string[] { "1111", "222" };
            //    dataRow.Id = "1";
            xTable.Rows.Add(dataRow);
            dataRow = new XDataRow();
            //     dataRow.Id = "2";
            //            dataRow.Values = new string[] { "1111", "222" };
            xTable.Rows.Add(dataRow);
            return xTable;

        }

        private Object EncloseSeverMethodParam(Object param)
        {
            XTable_1 xTable = null;
            WbdlSchema schame = pageCtr.Schame;
            if (!(param is XTable_1))
            {
                return param;
            }

            xTable = (XTable_1)param;

            for (int i = 0; i < xTable.Rows[0].Values.Count; i++)
            {
                if (xTable.Rows[0].Values[i].Equals(""))
                {
                    xTable.Rows[0].Values[i] = "0";
                }
            }

            for (int i = 0; i < xTable.FieldNames.Count(); i++)
            {
                string elementId = xTable.FieldNames[i];
                foreach (FieldBindSchema fieldBind in schame.FieldBinds)
                {
                    if (fieldBind.Id.Equals(elementId, StringComparison.OrdinalIgnoreCase))
                    {
                        xTable.FieldNames[i] = fieldBind.FieldId;
                        xTable.Id = fieldBind.TableId;
                    }
                }


                foreach (DataListBindSchema dataList in schame.DataListBinds)
                {
                    foreach (FieldBindSchema fieldBind in dataList.Columns)
                    {
                        if (fieldBind.Id.Equals(elementId, StringComparison.OrdinalIgnoreCase))
                        {
                            xTable.FieldNames[i] = fieldBind.FieldId;
                            xTable.Id = fieldBind.TableId;
                        }
                    }
                }

            }


            return xTable;

        }

        private Object EncloseReponseReturn(Object returnValue)
        {
            WbdlSchema schame = pageCtr.Schame;

            XTable_1 xTable = null;

            if (!(returnValue is XTable_1))
            {
                return returnValue;
            }

            xTable = (XTable_1)returnValue;


            for (int i = 0; i < xTable.FieldNames.Count(); i++)
            {
                string fieldName = xTable.FieldNames[i];


                foreach (FieldBindSchema fieldBind in schame.FieldBinds)
                {
                    if (fieldBind.FieldId.Equals(fieldName, StringComparison.OrdinalIgnoreCase))
                    {
                        xTable.FieldNames[i] = fieldBind.Id;
                        //test
                        // xTable.FieldDisplayName[i] = fieldBind.DisplayName;
                    }
                }


                foreach (DataListBindSchema dataList in schame.DataListBinds)
                {
                    foreach (FieldBindSchema fieldBind in dataList.Columns)
                    {
                        if (fieldBind.FieldId.Equals(fieldName, StringComparison.OrdinalIgnoreCase))
                        {
                            xTable.FieldNames[i] = fieldBind.Id;
                            //test
                            // xTable.FieldDisplayName[i] = fieldBind.DisplayName;
                        }
                    }
                }
            }
            return xTable;
        }

        private object GetResponseEnvListValue(string schemaValue, object paramValue, ResponseEnv responseEnv)
        {
            if (!(paramValue is XTable_1)) return paramValue;

            XTable_1 table = (XTable_1)paramValue;

            for (int i = 0; i < table.FieldNames.Count(); i++)
            {
                string fieldName = table.FieldNames[i];
                if (responseEnv.Body.FieldBinds.ContainsKey(fieldName))
                {
                    foreach (XDataRow row in table.Rows)
                    {
                        row.Values[i] = responseEnv.Body.FieldBinds[fieldName];
                    }
                }
            }
            return table;
        }

        private object GetResponseEnvElementValue(string schemaValue, object paramValue, ResponseEnv responseEnv)
        {
            string elementId = schemaValue.Remove(0, 1);
            if (responseEnv.Body.FieldBinds.ContainsKey(elementId))
                return responseEnv.Body.FieldBinds[elementId];
            else
                return paramValue;
        }

        private void SaveRequestData(WbapRequest wbapRequest)
        {
            if (wbapRequest == null) return;

            foreach (KeyValuePair<string, object> data in wbapRequest.ElementBinds)
            {
                if (!requestDataMap.ContainsKey(data.Key))
                    requestDataMap.Add(data.Key, data.Value);
                else
                    requestDataMap[data.Key] = data.Value;
            }

        }

        private void InitualElement(WbapResponse resp, FieldBindSchema fieldBindSchema)
        {
            string tableName = fieldBindSchema.TableId;
            if (tableName == null)
            {
                throw (new E_WbdlPageFieldBindSchemaNotAssignedFieldId(fieldBindSchema.Id));
            }

            XTableSchema tableSchema = this.pageCtr.GetTableSchema(tableName);
//            XTableAdapter tableAdapter = new XTableAdapter(tableSchema);

            if (tableSchema == null) return;

            FieldSchema fieldSchame = tableSchema.Fields.FindItem(fieldBindSchema.FieldId);
            if (fieldSchame == null)
            {
                return;
                //throw (new E_CannotGetFieldSchame(fieldBindSchema.TableId + "." + fieldBindSchema.FieldId));
            }
            //fieldSchame.DisplayName = fieldBindSchema.DisplayName;
            //捆绑初始值
            //if (fieldSchame.DefaultValue != null && fieldSchame.DefaultValue != "")
            //{
            //    if (!resp.ElementBinds.ContainsKey(fieldBindSchema.Id))
            //        resp.ElementBinds.Add(fieldBindSchema.Id, fieldSchame.DefaultValue);
            //}

            //捆绑下拉列表
            string codeTableName = fieldSchame.CodeTableName;

            if (codeTableName != null && codeTableName != "")
            {

           //     XData xData = new XData();
                //try
                //{
                //    CodeTable codeTable = xData.GetCodeTable(codeTableName);
                //    Dictionary<string, string> option = new Dictionary<string, string>();

                //    foreach (CodeTableData codeTableData in codeTable.Datas)
                //    {
                //        option.Add(codeTableData.Id, codeTableData.Text);
                //    }
                //    resp.ElementBinds.AddOption(fieldBindSchema.Id, option);
                //}
                //catch (Exception e)
                //{
                //    throw (new Exception("Bind Element CodeTable Error Element Id is'" + fieldBindSchema.Id + "'  CodeTable is '" + codeTableName + "'," + e.Message));
                //}
            }


            //捆绑Lookup

            LookupFieldSchema lookupField = tableSchema.LookupFields.FindItem(fieldBindSchema.FieldId);

            if (lookupField != null)
            {
                //WbapEvent wbapEvent = new WbapEvent();
                //wbapEvent.EventName = "onpropertychange";
                //string  actionId= "GetLookup(" 
                //                  + fieldBindSchema.TableId 
                //                  + "," + fieldBindSchema.FieldId
                //                  + ","+field;
                //wbapEvent.Action.Request.ActionId =actionId; 
                //wbapEvent.Action.Request.PageName = schema.Id;
                //wbapEvent.Action.Request.ElementBinds.Add("fieldName", fieldBindSchema.TableId);
                //wbapEvent.Action.Request.ElementBinds.Add("fieldName", fieldBindSchema.FieldId);
                //wbapEvent.Action.Request.ElementBinds.Add("fieldValue", "@" + fieldBindSchema.Id);
                //wbapEvent.Action.Request.ElementBinds.Add("ReturnValue", "@" + fieldBindSchema.Id + "_Lookup");

                //fieldBindSchema.

                //resp.ElementBinds.AddEvent(fieldBindSchema.Id, wbapEvent);
            }

        }

        private void BuildAction(string actionId, int step, WbapAction action, WbapRequest request)
        {
            ActionFlowSchema actionSchema = WbdlSchema.Actions.FindItem(actionId);

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
                    BuildClientAction(methodSchema, clientAction, request);
                }
                else
                {
                    BuildServerRequest(action.Request, actionSchema, i);
                    hasBuildedRequest = true;
                    break;
                }
            }
        }

        private void BuildClientAction(ActionSchema methodSchema, ClientAction clientRequest, WbapRequest request)
        {
            clientRequest.FuncName = methodSchema.MethodName;
            clientRequest.ReturnValue = methodSchema.ReturnValue;
            foreach (ParameterSchema paramSchema in methodSchema.Parameters)
            {
                if (paramSchema.Value[0] == '$')
                    clientRequest.Parameters.Add(request.ElementBinds[paramSchema.Value]);
                else
                    clientRequest.Parameters.Add(paramSchema.Value);
            }
        }

        //private WbapResponse InvokeFarAction(WbapRequest wbapRequest)
        //{
        //    WbapResponse response = new WbapResponse();
        //    response.PageName = schema.Id;

        //    Dictionary<string, Object> namedParams = new Dictionary<string, object>();

        //    foreach (KeyValuePair<string, Object> elementItem in wbapRequest.ElementBinds)
        //    {
        //        string retElement = null;
        //        if (elementItem.Key.Equals("ReturnValue"))
        //        {
        //            retElement = elementItem.Value.ToString();
        //        }
        //        else
        //        {
        //            namedParams.Add(elementItem.Key, GetParamVarValue(elementItem.Value.ToString()));
        //        }
        //    }

        //    Umc.Umc.InvokeFunction("sid", "msid", wbapRequest.ActionId, namedParams);

        //    return response;

        //}        

        private void BuildServerRequest(WbapRequest request, ActionFlowSchema action, int Step)
        {
            request.Step = Step;
            request.PageName = WbdlSchema.Id;
            request.ActionId = action.Id;

            for (int i = Step; i < action.Actions.Count; i++)
            {

                ActionSchema methodSchema = action.Actions[i];

                if (methodSchema.IsRunAtClient()) break;

                for (int j = 0; j < methodSchema.Parameters.Count; j++)
                {
                    ParameterSchema paramSchema = methodSchema.Parameters[j];
                    if (string.IsNullOrEmpty(paramSchema.Value)) continue;
                    string[] realParamValues = paramSchema.Value.Split(PARAM_SPLITOR);
                    
                    foreach (string realParamValue in realParamValues)
                    {
                        if (Array.IndexOf(VAR_TYPES, realParamValue[0]) == (int)VarFlagType.Element)
                        {
                            string key = realParamValue.Remove(0, 1);

                            if (!request.ElementBinds.ContainsKey(key))
                                request.ElementBinds.Add(key, "");
                        }
                        else if (Array.IndexOf(VAR_TYPES, realParamValue[0]) == (int)VarFlagType.Table)
                        {
                            request.ElementBinds.ImportTableSchema(realParamValue.Remove(0, 1), WbdlSchema);

                        }
                        else if (realParamValue[0].Equals('$'))
                        {
                            request.ElementBinds.Add(realParamValue, "");
                        }
                        else if (pageCtr.ContainsDataTable(realParamValue))
                        {
                            pageCtr.BuildRequestDataBodyWithTable(request.ElementBinds, realParamValue,null);
                        }
                        else if (methodSchema.MethodName.Equals("UpdateDataSet",StringComparison.OrdinalIgnoreCase))
                        {
                            pageCtr.BuildRequestDataBodyWithDataSet(request.ElementBinds);
                        }

                    }
                }
            }
        }

        private ClientAction BuildClientAction(ActionSchema methodSchema, WbapRequest request)
        {
            ClientAction clientRequest = new ClientAction();
            BuildClientAction(methodSchema, clientRequest, request);
            return clientRequest;
        }

        private object GetParamVarValueForObject(string paramVar)
        {
            if (paramVar.Length < 1) return paramVar;
            char varFlag = paramVar[0];

            string varName = paramVar.Remove(0, 1);

            if (varFlag == VAR_TYPES[(int)VarFlagType.Element])
            {
                if (requestDataMap.ContainsKey(varName))
                    return requestDataMap[varName];
                else
                    throw new XException("Request var :'" + varName + "' not be got");

            }

            if (varFlag == VAR_TYPES[(int)VarFlagType.Table])
                return requestDataMap.GetTable(varName, WbdlSchema);

            return paramVar;
        }


    }
}
