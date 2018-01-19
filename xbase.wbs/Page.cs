using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wbs;
using xbase.Interface;
using wbs.wbap;
using System.Reflection;
using System.Text.RegularExpressions;
using xbase.umc;
using System.IO;
using xbase.Validation;
using xbase.security;
using xbase.Exceptions;
using xbase.wbs.wbdl;
using xbase.umc.attributes;

namespace xbase.wbs
{
    [WboAttr(Id = "ListData", Title = "页面", Version = 1.1, LifeCycle = LifeCycle.Session, ContainerType = typeof(WbdlSchemaContainer)
   , IsPublish = true)]
    public class Page:HttpWbo
    {
        const char Element_Var_Mark = '$';//元素变量标记
        const char ArySpliter = ';';//字符串数组分割符
        const string RequestStrMark = "QueryString.";//url 查询参数标记
        const string RequestSender_Var_Value = "EventSender.Value";//url 查询参数标记
        const string RequestSender_Var_Index = "EventSender.Index";//url 查询参数标记
        const string RequestSender_Var_ElementName = "EventSender.ElementName";//url 查询参数标记
        const string RequestSender_Var_RowId = "EventSender.RowId";//url 查询参数标记
        const string USER_NAVE_VAR = "User.UserName";
        const string USER_Group_VAR = "User.Group";
        const string DATE_NOW_VAR = "Date.Now";
        const string DATE_Date_VAR = "Date.Date";
        const string DATE_Today_VAR = "Date.Today";
        const string LOGING_FORM_VAR = "LoginForm.";


        private string pageId;
        private string sessionId;
        private ISecurity security = null;
        private WbdlSchema pageSchema;

        private Dictionary<string, object> dataSources = new Dictionary<string, object>();

        private Dictionary<string, string[]> elementDatas = new Dictionary<string, string[]>();
        private List<string> changedElementsIds = new List<string>();
        private Dictionary<string, string> queryStrings;
        private ISession session;
        private string executingFlow;//正在执行的流程

        private int nextStep = 0;//要执行流程的下一个活动
        private List<String> needDatas = new List<string>();//所需的客户端数据
        private WbpsResquest request = null;
        private IXServer server;
        private string url;
        private string clientScript;
        private bool isOpen = false;

        public string Url
        {
            get { return url; }
        }

        public string ExecutingFlow
        {
            get { return executingFlow; }
            set { executingFlow = value; }
        }

        public string PageId
        {
            get { return pageId; }
            set { pageId = value; }
        }

        public static string getPageId(string path)
        {
            string pageFile = XSite.MapPath(path);

            string pageId = pageFile.Remove(0, XSite.SitePhysicalPath.Length);
            if (WbdlSchemaContainer.Instance().Contains(pageId))
                return pageId;

            string oldPageId = Path.ChangeExtension(pageFile, "").TrimEnd('.');
            oldPageId = oldPageId.Remove(0, XSite.SitePhysicalPath.Length);
            if (WbdlSchemaContainer.Instance().Contains(oldPageId))
                return oldPageId;

            return pageId;
        }

        public Page(WbpsResquest req, string sessionId, ISecurity security,  ISession session)
        {
            this.sessionId = sessionId;
            this.url = req.Url;
            this.security = security;
            this.session = session;


            this.pageId = getPageId(req.PageId);
            string query = req.Query;

            if (!string.IsNullOrEmpty(query))
            {
                queryStrings = new Dictionary<string, string>();
                string[] nameValues = query.Split('&');
                for (int i = 0; i < nameValues.Length; i++)
                {

                    string[] nameValue = nameValues[i].Split('=');
                    if (nameValue.Length > 1)
                        queryStrings.Add(nameValue[0], nameValue[1].Trim());
                }
            }

            pageSchema = null;
            if (WbdlSchemaContainer.Instance().Contains(this.pageId))
            {
                pageSchema = WbdlSchemaContainer.Instance().GetItem(this.pageId);
                pageSchema.ChangedEvent += new Schema.SchemaEventHandler(PageSchema_Changed);
                Initialize();
            }
            this.isOpen = true;
        }
        private void Initialize()
        {
            OpenDataSource();
            InitElementDatas();
        }

        public Dictionary<string, object> DataSources
        {
            get { return dataSources; }
        }
        public WbdlSchema PageSchema
        {
            get { return pageSchema; }
            set { pageSchema = value; }
        }

        private string getDataSourceId(IDataSource ds)
        {
            foreach (string key in dataSources.Keys)
            {
                if (dataSources[key] == ds) return key;
            }
            return null;
        }

        private void Data_OnSetField(IDataSource ds, string fieldFolder)
        {
            string dsId = getDataSourceId(ds);
            SetElementDataToObject(dsId, ds, fieldFolder);
        }

        private bool Data_OnSetData(IDataSource ds)
        {
            string dsId = getDataSourceId(ds);
            SetElementDataToObject(dsId, ds);
            return true;
        }


        private void PageSchema_Changed(object sender)
        {
            if (sender is WbdlSchema)
            {
                pageSchema = (sender as WbdlSchema);
                pageSchema.ChangedEvent += new Schema.SchemaEventHandler(PageSchema_Changed);
            }
            Initialize();
        }

        internal string CheckValueVar(string value)
        {
            return CheckValueVar(value, this.elementDatas, true);
        }

        private string checkRequestValueVar(string value)
        {
            object obj = CheckValueVar(value, this.request.ElementDatas, false);
            return CheckValueVar(obj.ToString(), this.elementDatas, true);
        }


        /// <summary>
        ///  //检查WBDL中定义的值是否是标量，并将给出变量的值 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal string CheckValueVar(string value, Dictionary<string, string[]> elementDatas, bool throwErr)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            string s = value.Replace(USER_NAVE_VAR, security.user.Name);
            s = s.Replace(USER_Group_VAR, security.user.GroupId);
            s = s.Replace(DATE_NOW_VAR, DateTime.Now.ToString());
            s = s.Replace(DATE_Date_VAR, DateTime.Now.ToShortDateString());
            s = s.Replace(DATE_Today_VAR, DateTime.Today.ToString());


            //临时
            s = s.Replace("LoginForm.DM", security.user.GroupId);

            //   bool hasNeedData = false;

            //检查事件源变量
            if (request != null)
            {
                if (request.Sender != null && request.Sender.ElementName != null)
                    s = s.Replace(RequestSender_Var_ElementName, request.Sender.ElementName);
                else
                    s = s.Replace(RequestSender_Var_ElementName, "");

                if (request.Sender != null && request.Sender.Index != 0)
                    s = s.Replace(RequestSender_Var_Index, request.Sender.Index.ToString());
                else
                    s = s.Replace(RequestSender_Var_Index, "");

                if (request.Sender != null && request.Sender.RowId != null)
                    s = s.Replace(RequestSender_Var_RowId, request.Sender.RowId);
                else
                    s = s.Replace(RequestSender_Var_RowId, "");

                if (request.Sender != null && request.Sender.Value != null)
                    s = s.Replace(RequestSender_Var_Value, request.Sender.Value);
                else
                    s = s.Replace(RequestSender_Var_Value, "");
            }

            //检查来至URL的查询字符
            VarChecker UrlParamChecker = new VarChecker(queryStrings, RequestStrMark);
            Regex regex = new Regex(RequestStrMark + @"[A-Za-z]\w+");//匹配@aaaa,@bbb=@ccc;@za23sdzz@z123@123
            s = regex.Replace(s, new MatchEvaluator(UrlParamChecker.CapText));


            //元素标记的标量
            VarChecker elementTagVarChecker = new VarChecker(elementDatas, Element_Var_Mark + "");
            Regex regex_ElementMark = new Regex(@"\" + Element_Var_Mark + @"[A-Za-z]\w+");//匹配$aaaa,@bbb=@ccc;@za23sdzz@z123@123
            s = regex_ElementMark.Replace(s, new MatchEvaluator(elementTagVarChecker.CapText));


            //检查来至登录表单的元素数据
            VarChecker loginFormVarChecker = new VarChecker(security.LoginFormDatas, LOGING_FORM_VAR);
            Regex regex_LoginForm = new Regex(LOGING_FORM_VAR + @"[A-Za-z]\w+");//匹配@aaaa,@bbb=@ccc;@za23sdzz@z123@123
            s = regex_LoginForm.Replace(s, new MatchEvaluator(loginFormVarChecker.CapText));

            return s;
        }


        private List<string> GetElementVarsFromString(string str)
        {
            List<string> ret = new List<string>();

            Regex regex_ElementMark = new Regex(@"\" + Element_Var_Mark + @"[A-Za-z]\w+");//匹配$aaaa,@bbb=@ccc;@za23sdzz@z123@123
            MatchCollection ms_ElementMark = regex_ElementMark.Matches(str);

            foreach (Match match in ms_ElementMark)
            {
                foreach (Capture c in match.Captures)
                {
                    string elName = c.Value;
                    string elKey = elName.Trim(new char[] { Element_Var_Mark });
                    ret.Add(elKey);
                }
            }
            return ret;
        }

        private void OpenDataSource()
        {
            dataSources.Clear();
            foreach (WbdlDataSchema dss in pageSchema.DataSources)
            {
                try
                {
                    object obj = Umc.GetObject(dss.Name, dss.DataType);

                    if (obj is ISessionWbo)
                        (obj as ISessionWbo).Session = this.session;

                    if (obj is ISecurityWbo)
                        (obj as ISecurityWbo).UserContext = this.security.user;

                    if (dss.Props != null)
                    {
                        SetObjPropertis(dss.Props, obj);
                        //    if (obj is IDataSource)
                        //        (obj as IDataSource).RefreshData();
                    }
                    dataSources.Add(dss.Id, obj);
                }
                catch (Exception e)
                {
                    string s = e.Message;
                    if (e.InnerException != null)
                        s = s += e.InnerException.Message;
                    throw new XException(s + "Page在打开数据源" + dss.Name + "是发生错误，数据Id：" + dss.Id);
                }
            }
        }


        internal bool SetObjPropertis(List<DataPropertySchema> Props, object obj)
        {
            bool ret = true;
            for (int i = 0; i < Props.Count; i++)
            {
                try
                {
                    Type t = obj.GetType();
                    string propDefValue = Props[i].Value.Trim();

                    object propValue = CheckValueVar(propDefValue);
                    if (propValue == null)
                    {
                        ret = false;
                        continue;
                    }
                    string propName = Props[i].Name.Trim();
                    PropertyInfo p = t.GetProperty(propName);

                    object v = Convert.ChangeType(propValue, p.PropertyType);
                    p.SetValue(obj, v, null);


                }
                catch (Exception e)
                {
                    var err = new Exception("设置对象状态时，发生错误,属性名：" + Props[i].Name, e);
                    throw err;
                }
                //                        t.InvokeMember(propName, BindingFlags.SetProperty, null, obj, new object[] { propValue });
                // t.GetProperty(dss.Props[i].Name);

                //System.Reflection.FieldInfo p= t.GetField(dss.Props[i].Name);
                //p.SetValue(obj,propValue);
            }
            return ret;
        }

        /// <summary>
        /// 初始元素值
        /// </summary>
        private void InitElementDatas()
        {
            elementDatas.Clear();
            if (pageSchema.ElementDatas == null) return;

            foreach (ElementDataSchema eds in pageSchema.ElementDatas)
            {
                string dataId = eds.Id;
                if (eds.BindType == ElementBindType.REP)
                    dataId = eds.BindType.ToString() + "_" + dataId;

                if (!String.IsNullOrEmpty(eds.Value) || string.IsNullOrEmpty(eds.DataSourceId) || string.IsNullOrEmpty(eds.DataField)) // 值绑定
                {
                    string value = CheckValueVar(eds.Value).ToString();
                    elementDatas.Add(dataId, new string[] { value });
                    changedElementsIds.Add(dataId);
                }
                else
                {
                    if (!dataSources.ContainsKey(eds.DataSourceId))
                        throw new XException("元素捆绑错误，在元素" + eds.Id + "中，不能找到数据源" + eds.DataSourceId);
                    object data = dataSources[eds.DataSourceId];
                    if (data is IDataSource)
                    {
                        IDataSource ds = data as IDataSource;
                        elementDatas.Add(dataId, ds.GetFieldData(eds.DataField));
                        changedElementsIds.Add(dataId);
                    }
                }
            }
        }

        private void ValidateElement(ElementDataSchema eds, string[] value)
        {
            if (eds.Validation.Validators.Count > 0)
            {

                string errHint = "";
                if (value == null || value.Length < 1)
                {

                    if (!Validation.ValidatorFactory.InvokeValid(eds.Validation, null, CheckValueVar, out errHint))
                        throw new Validation.EValidateException(errHint);
                }
                for (int i = 0; i < value.Length; i++)
                {
                    if (!Validation.ValidatorFactory.InvokeValid(eds.Validation, value[i], CheckValueVar, out errHint))
                        throw new Validation.EValidateException(errHint);
                }
            }
        }

        public void ValidateRequestData(Dictionary<string, string[]> elementDatas)
        {
            foreach (string key in elementDatas.Keys)
            {
                string[] value = elementDatas[key];

                ElementDataSchema eds = this.pageSchema.ElementDatas.FindItem(key);
                if (eds == null) continue;
                if (eds.Validation == null) continue;
                if (eds.Validation.Validators.Count > 0)
                {
                    string errHint = "";
                    if (value == null || value.Length < 1)
                    {
                        if (!Validation.ValidatorFactory.InvokeValid(eds.Validation, null, checkRequestValueVar, out errHint))
                            throw new Validation.EValidateException(errHint);
                    }
                    for (int i = 0; i < value.Length; i++)
                    {
                        if (!Validation.ValidatorFactory.InvokeValid(eds.Validation, value[i], checkRequestValueVar, out errHint))
                            throw new Validation.EValidateException(errHint);
                    }
                }
            }
        }


        public void ReceiveRequestData(Dictionary<string, string[]> elementDatas)
        {
            foreach (string key in elementDatas.Keys)
            {
                string[] value = elementDatas[key];
                if (this.elementDatas.ContainsKey(key))
                    this.elementDatas[key] = value;
                else
                    this.elementDatas.Add(key, value);
            }
        }
        private bool FlowNeedValid(ActionFlowSchema fs)
        {

            foreach (ActionSchema act in fs.Actions)
            {
                if (act.IsRunAtClient()) continue;
                if (string.IsNullOrEmpty(act.MethodName)) continue;
                WbdlDataSchema dss = pageSchema.DataSources.GetItem(act.DataSourceId);
                if (umc.Umc.GetObjectPermissionTypes(dss.DataType, act.MethodName) == PermissionTypes.Write)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 执行一个Wbps请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public WbpsResponse InvokeRequest(WbpsResquest request)
        {
            WbpsResponse ret = new WbpsResponse();
            if (pageSchema == null) return ret;
            string flowId = request.FlowId;

            ActionFlowSchema fs = null;
            if (!string.IsNullOrEmpty(flowId))
            {
                fs = pageSchema.ActionFlows.FindItem(flowId);
                if (fs == null)
                    throw new XException("不能发现流程配置" + flowId);
            }

            if (request.Step > 0)
            {
                this.nextStep = request.Step;
                request.Sender = this.request.Sender;
            }

            this.request = request;

            bool isEnd = true;

            if (!security.CheckObjectPermission("ListData", request.PageId, PermissionTypes.Read))
            {
                ret.Err = JsonExceptionUtils.ThrowErr(SecErrs.NotPemission, security.LoginPageUrl).Err;
                return ret;
            }

            if (request.ElementDatas != null)
            {
                if (fs != null && FlowNeedValid(fs))
                    ValidateRequestData(request.ElementDatas);
                ReceiveRequestData(request.ElementDatas);
            }


            if (fs != null)
            {
                isEnd = InvokeFlow(fs, request.FlowVars);
            }

            ret.ElementDatas = GetChangedElements();

            if (!isEnd)
            {
                ret.BackRequest = new WbpsResquest();
                ret.BackRequest.PageId = request.PageId;
                ret.BackRequest.FlowId = request.FlowId;
                ret.BackRequest.Step = this.nextStep;
                ret.BackRequest.SessionId = this.sessionId;

                for (int i = 0; i < NeedDatas.Count; i++)
                {
                    ret.BackRequest.ElementDatas.Add(NeedDatas[i], null);
                }

                this.executingFlow = request.FlowId;
            }
            else
            {
                this.executingFlow = null;
            }

            if (!string.IsNullOrEmpty(this.clientScript))
            {
                ret.ClientScript = CheckValueVar(this.clientScript).ToString();
            }

            this.clientScript = "";


            if (fs == null)
            {
                ret.Events = getWbapEvents();
            }

            return ret;

        }

        /// <summary>
        /// 执行任务流程
        /// </summary>
        /// <param name="flowId"></param>
        /// <param name="flowVars"></param>
        /// <returns>流程结束返回真</returns>
        public bool InvokeFlow(ActionFlowSchema fs, Dictionary<string, string> flowVars)
        {

            changedElementsIds.Clear();
            this.needDatas.Clear();

            for (int step = this.nextStep; step < fs.Actions.Count; step++)
            {
                ActionSchema ss = fs.Actions[step];
                if (ss.RunAt == RunAtType.Client)
                {
                    this.nextStep = CreateBackAction(step, fs);
                    return (nextStep == 0);
                }

                if (!InvokeAction(ss, flowVars))
                {
                    this.nextStep = step;
                    return false;
                }
            }

            nextStep = 0;
            return true;
        }

        private int CreateBackAction(int step, ActionFlowSchema flowSchema)
        {
            this.clientScript = "";
            //int ret = 0;
            for (int i = step; i < flowSchema.Actions.Count; i++)
            {
                ActionSchema ss = flowSchema.Actions[i];
                if (ss.RunAt != RunAtType.Client)
                {
                    return i;
                }
                this.clientScript += ss.ClientScript + ";";
            }
            return 0;
        }

        private bool Obj_OnSetDataNotify(IDataSource dataSource)
        {
            return true;
        }


        private bool InvokeAction(ActionSchema actionSchema, Dictionary<string, string> flowVars)
        {
            object obj = null;
            string dsId = actionSchema.DataSourceId;

            try
            {
                obj = dataSources[dsId];
            }
            catch
            {
                throw new E_NotFindDataObject(actionSchema.DataSourceId);
            }

            if (obj is IDataSource)
            {
                (obj as IDataSource).OnSetField += this.Data_OnSetField;
                (obj as IDataSource).OnSetData += this.Data_OnSetData;
            }

            if (actionSchema.Props != null)
            {
                if (!SetObjPropertis(actionSchema.Props, obj))
                    return false;

            }

            //没有方法定义，就此获取页面后退出
            if (String.IsNullOrEmpty(actionSchema.MethodName))
            {

                //流程中途更新页元素面数据：
                GetElementDataFormObject(dsId, obj);

                return true;
            }

            WbdlDataSchema dataObjSchema = pageSchema.DataSources.GetItem(actionSchema.DataSourceId);
            Dictionary<string, string> parametes = new Dictionary<string, string>();

            bool needData = false;
            foreach (ParameterSchema paramSchema in actionSchema.Parameters)
            {
                //  Type paramInfo = umc.Umc.GetClassMethodParamInfo(dataObjSchema.DataType, actionSchema.MethodName, paramSchema.Id).ParameterType;

                string paramValue = CheckValueVar(paramSchema.Value.Trim(), flowVars);
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


            //  SetElementDataToObject(dsId, obj);
            //       DataSourceSetDataNotifty onSet = delegate(IDataSource ds)
            //       {
            //           SetElementDataToObject(dsId, ds);
            //           return true;
            //       };

            //        if (obj is IDataSource)
            //        {
            //            (obj as IDataSource).OnSetData += onSet;
            //        }

            try
            {
                //  object retObj = umc.Umc.InvokeMethod(obj, actionSchema.MethodName, parametes);
               // object retObj = umc.Umc.InvokeMethod(obj, dataObjSchema.DataType, actionSchema.MethodName, parametes);
            }
            catch (Exception e)
            {
                if (e.InnerException is XUserException)
                    throw e.InnerException;

                string inErrMessge = "";
                if (e.InnerException != null) inErrMessge = e.InnerException.Message;
                Exception err = new Exception("调用活动时间，发生错误，活动名：" + actionSchema.Title + ",错误内容：" + e.Message + inErrMessge, e);
                throw err;
            }

            //            if (obj is IDataSource)
            //            {
            //                (obj as IDataSource).OnSetData -= onSet;
            //            }
            if (obj is IDataSource)
            {
                (obj as IDataSource).OnSetField -= this.Data_OnSetField;
                (obj as IDataSource).OnSetData -= this.Data_OnSetData;
            }


            GetElementDataFormObject(dsId, obj);

            return true;

        }

        internal string CheckValueVar(string value, Dictionary<string, string> varNameValues)
        {
            if (varNameValues != null)
            {
                foreach (string key in varNameValues.Keys)
                {
                    Regex regex = new Regex(@"\b" + key + @"\b");
                    regex.Replace(value, varNameValues[key]);
                }
            }

            string ret = CheckValueVar(value);
            return ret;
        }

        internal void GetElementDataFormObject(string dsId, object obj)
        {
            foreach (ElementDataSchema eds in pageSchema.ElementDatas)
            {
                IDataSource ds = null;
                if (obj is IDataSource)
                {
                    ds = obj as IDataSource;
                    if (eds.DataSourceId.Equals(dsId, StringComparison.OrdinalIgnoreCase))
                    {
                        string[] value = ds.GetFieldData(eds.DataField);
                        if (elementDatas.ContainsKey(eds.Id))
                            elementDatas[eds.Id] = value;
                        else
                            elementDatas.Add(eds.Id, value);

                        changedElementsIds.Add(eds.Id);
                    }
                }
                else
                {
                    throw new E_CurrentVesionNoSuportsNonIDatasourceObject();
                }

            }
        }

        private void SetElementDataToObject(string dsId, object obj)
        {
            SetElementDataToObject(dsId, obj, null);
        }
        private void SetElementDataToObject(string dsId, object obj, string fieldFolder)
        {
            if (!isOpen) return;
            foreach (ElementDataSchema eds in pageSchema.ElementDatas)
            {
                IDataSource ds = null;

                if (obj is IDataSource)
                {
                    ds = obj as IDataSource;
                    if (eds.DataSourceId.Equals(dsId, StringComparison.OrdinalIgnoreCase))
                    {

                        if (elementDatas.ContainsKey(eds.Id))
                        {
                            bool isLikeField = (string.IsNullOrEmpty(fieldFolder)) ||
                                               (eds.DataField.StartsWith(fieldFolder + ".")) ||
                                          (eds.DataField.IndexOf('.') < 0 && fieldFolder.Equals(DataSourceFieldFolder.PageList.ToString()));
                            if (isLikeField)
                            {
                                string[] values = elementDatas[eds.Id];

                                if (eds.MapServerPath)
                                    values = Server.MapPath(values);

                                //ValidateElement(eds, values);

                                ds.SetFieldData(eds.DataField, values);
                            }
                        }
                        else
                            throw new XException("系统错误，wbdl elementDatas 没有初始," + eds.Id + ",在配置文件" + pageSchema.Id + "中。");
                    }
                }
                else
                {
                    throw new E_CurrentVesionNoSuportsNonIDatasourceObject();
                }
            }
        }


        internal Dictionary<string, string[]> GetChangedElements()
        {
            Dictionary<string, string[]> ret = new Dictionary<string, string[]>();
            foreach (string key in changedElementsIds)
            {
                if (!ret.ContainsKey(key))
                    ret.Add(key, elementDatas[key]);
            }
            return ret;
        }

        internal List<WbpsEvent> getWbapEvents()
        {
            List<WbpsEvent> ret = new List<WbpsEvent>();
            for (int i = 0; i < pageSchema.Events.Count; i++)
            {
                EventSchema eventSchema = pageSchema.Events[i];
                WbpsEvent wEvent = new WbpsEvent();
                wEvent.EventName = eventSchema.EventName;

                if (String.IsNullOrEmpty(wEvent.ElementName))//兼容旧版
                    wEvent.ElementName = eventSchema.Id;

                wEvent.ElementName = eventSchema.ElementName;
                wEvent.EventRequest.FlowId = eventSchema.ActionFlow;
                wEvent.EventRequest.ElementDatas = getFlowNeedElementDatas(eventSchema.ActionFlow);
                wEvent.EventRequest.PageId = request.PageId;
                wEvent.EventRequest.SessionId = this.sessionId;
                wEvent.ActionFlow = eventSchema.ActionFlow;



                //if (eventSchema.FlowVars != null)
                //{
                //    foreach (NameValue prop in eventSchema.FlowVars)
                //    {
                //        wevent.Vars.Add(prop.Name, prop.Value);
                //    }
                //}
                ret.Add(wEvent);
            }
            return ret;
        }

        private Dictionary<string, string[]> getFlowNeedElementDatas(string actionFlowId)
        {
            Dictionary<string, string[]> ret = new Dictionary<string, string[]>();
            ActionFlowSchema afs = pageSchema.ActionFlows.GetItem(actionFlowId);
            foreach (ActionSchema acs in afs.Actions)
            {
                //流程对象属性所需的元素
                if (acs.Props != null)
                    foreach (DataPropertySchema prop in acs.Props)
                    {
                        string s = prop.Value;
                        //元素标记的标量
                        Regex regex_ElementMark = new Regex(@"\" + Element_Var_Mark + @"[A-Za-z]\w+");//匹配$aaaa,@bbb=@ccc;@za23sdzz@z123@123
                        MatchCollection ms_ElementMark = regex_ElementMark.Matches(s);

                        foreach (Match match in ms_ElementMark)
                        {
                            foreach (Capture c in match.Captures)
                            {
                                string elName = c.Value;
                                string elKey = elName.Trim(new char[] { Element_Var_Mark });
                                if (!ret.ContainsKey(elKey))
                                    ret.Add(elKey, new string[] { });
                            }
                        }

                    }

                //流程对象捆绑的元素数据
                if (pageSchema.ElementDatas != null)
                    foreach (ElementDataSchema eds in pageSchema.ElementDatas)
                    {
                        if (eds.ReadOnly) continue;
                        if (eds.Id.EndsWith("_attr_options"))
                            continue;
                        if (eds.DataSourceId.Equals(acs.DataSourceId, StringComparison.OrdinalIgnoreCase))
                        {
                            if (!ret.ContainsKey(eds.Id))
                                ret.Add(eds.Id, new string[] { });
                        }
                    }

                //流程对象函数参数捆绑的元素
                if (acs.Parameters != null)
                    foreach (ParameterSchema ps in acs.Parameters)
                    {
                        List<string> keys = GetElementVarsFromString(ps.Value);
                        foreach (string key in keys)
                        {
                            if (!ret.ContainsKey(key))
                                ret.Add(key, new string[] { });
                        }
                    }

            }
            return ret;
        }

        public IXServer Server
        {
            get { return server; }
            set { server = value; }
        }


        /// <summary>
        /// 所需的客户端数据
        /// </summary>
        public List<String> NeedDatas
        {
            get { return needDatas; }
        }


        public Dictionary<string, string> QueryStrings
        {
            get { return queryStrings; }
            set { queryStrings = value; }
        }

        public ISession Session
        {
            get { return session; }
            set { session = value; }
        }


    }


}
