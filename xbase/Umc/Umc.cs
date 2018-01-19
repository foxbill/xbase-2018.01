#define DEBUG
#undef DEBUG
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using Common.Logging;
using xbase;
using xbase.Exceptions;
using xbase.security;
using Newtonsoft.Json;
using System.Web;
using System.Web.SessionState;
using System.Diagnostics;
using System.Xml;
using xbase.local;


namespace xbase.umc
{
    public class Umc
    {
        public const string MemberSpliter = ".";

        private const string SESSION_OBJECTS_KEY = "~xbase.SessionObjects~";
        const string UMC_SESSION_KEY = "~xbase.UMC~";
        const string OBJECT_CONTAINNER_DIR = "object\\";
        const string FUNCTION_CONTAINNER_DIR = "function\\";
        const string JS_OBJECT_CONTAINNER_DIR = "JS_object\\";
        const string JS_FUNCTION_CONTAINNER_DIR = "JS_function\\";
        const string SECURITY_COMID = "Security";
        const string USER_COMID = "LoginUser";
        const string REG_PAGE = "/register.aspx";



        //  private static XLogging.XLoggingService _Log = new XLogging.XLoggingService();
        private static Dictionary<string, object> objects = new Dictionary<string, object>();
        private ISecurity security;

        private static ILog log = LogManager.GetLogger("Logger");
        private HttpContext context;

        private static bool isInit;

        public static Umc getInstance(HttpContext context)
        {
            if (context.Request.Cookies["xBaseUser"] == null)
                context.Response.Cookies["xBaseUser"].Value = Guid.NewGuid().ToString();

            Umc ret = context.Session[UMC_SESSION_KEY] as Umc;
            if (ret == null || !(ret is Umc))
            {
                ret = new Umc(context);
                context.Session.Add(UMC_SESSION_KEY, ret);
            }
            ret.context = context;
            return ret as Umc;
        }

        protected Umc(HttpContext context)
        {
            if (!Register.checkActive())
                throw new LicenseException();

            XSite.Open(context.Server);

            this.context = context;
            // XSite.Open(context.Server);
            //    object user = GetObject("", USER_COMID);
            object sec = GetObject("", SECURITY_COMID);

            if (sec is ISecurity)
                this.security = sec as ISecurity;
            else
                throw new XException("Securty Component not implement xbase.security.ISecurity interface,please reregister security component");


            if (!isInit)
                Initialize(XSite.AppSchemaPath + "umc");
        }


        private static void Initialize(string umcPath)
        {
            string objPath = umcPath + OBJECT_CONTAINNER_DIR;
            if (!Directory.Exists(objPath))
                Directory.CreateDirectory(objPath);

            string funPath = umcPath + FUNCTION_CONTAINNER_DIR;
            if (!Directory.Exists(funPath))
                Directory.CreateDirectory(funPath);

            //    WboSchemaContainer.Initialize(objPath);
            //    FunctionSchemaContainer.Initialize(funPath);

            //    JSObjectSchemaContainer.Initialize(umcPath + JS_OBJECT_CONTAINNER_DIR);
            //    JSFunctionSchemaContainer.Initialize(umcPath + JS_FUNCTION_CONTAINNER_DIR);

            //      RegisterClass();
            isInit = true;
        }

        public ISecurity Security
        {
            get { return this.security; }
        }

        public HttpContext Context
        {
            get { return context; }
        }

        private static object CreateObject(WboSchema objectSchema, string objectName)
        {
            WboProxy wboProxy = WboProxyFactory.getWboProxy(objectSchema);

            //从持久存档文件中获取对象
            string file = getWboFileName(objectName, objectSchema.Id);
            if (File.Exists(file))
            {
                XmlReader xmlReader = XmlReader.Create(file);

                try
                {
                    if (xmlReader == null) throw (new EContainerCanNotOpenSchameFile());
                    XmlSerializer xmls = new XmlSerializer(wboProxy.getWboType());
                    return xmls.Deserialize(xmlReader);
                }
                catch (Exception e)
                {
                    throw new Exception("打开文件" + file + "时发生错误." + e.Message, e);
                }
                finally
                {
                    xmlReader.Close();
                }
            }


            return wboProxy.createObject(objectName);
        }


        private static object GetGlobalObject(WboSchema objectSchema, string objectName)
        {
            object obj = null;

            string objHashKey = getObjHashKey(objectSchema.Id, objectName);
            if (objects.ContainsKey(objHashKey))
            {
                obj = objects[objHashKey];
            }
            else
            {
                obj = CreateObject(objectSchema, objectName);
                if (obj != null)
                    objects.Add(objHashKey, obj);
            }

            return obj;

        }

        public HttpSessionState Session
        {
            get
            {
                return context.Session;
            }
        }

        public Dictionary<string, object> SessionObjects
        {
            get
            {
                Dictionary<string, object> ret = (Dictionary<string, object>)Session[SESSION_OBJECTS_KEY];
                if (ret == null)
                {
                    ret = new Dictionary<string, object>();
                    Session.Add(SESSION_OBJECTS_KEY, ret);
                }
                return ret;
            }
        }
        private static string getObjHashKey(string comId, string objName)
        {
            string ret = comId;
            if (!string.IsNullOrEmpty(objName))
                ret = ret + ":" + objName;
            return ret;
        }



        private object GetSessionObject(WboSchema objectSchema, string objectName)
        {
            string sesssionId = Session.SessionID;
            string objHashKey = getObjHashKey(objectSchema.Id, objectName);
            object obj = null;
            if (!SessionObjects.ContainsKey(objHashKey))
            {
                log.Debug("start getObject By Create new,object is" + objectName);
                obj = CreateObject(objectSchema, objectName);
                SessionObjects.Add(objHashKey, obj);
                log.Debug("End getObject By Create new,object is" + objectName);

            }
            obj = SessionObjects[objHashKey];
            return obj;
        }
        private object GetObject(string objectName, WboSchema objectSchema)
        {

            string sessionId = context.Session.SessionID;
            object obj = null;

            //    log.Debug("\n");

            //    log.Debug("start getObject,object is" + objectName);

            WboProxy wboProxy = WboProxyFactory.getWboProxy(objectSchema);

            if (wboProxy.getWboType().IsSubclassOf(typeof(ISessionWbo)))
            {
                obj = GetSessionObject(objectSchema, objectName);
            }
            else
                switch (objectSchema.LifeCycle)
                {
                    case LifeCycle.Global:
                        log.Debug(" start getObject from globle ");
                        obj = GetGlobalObject(objectSchema, objectName);
                        break;
                    case LifeCycle.Session:
                        log.Debug("start getObject from session: " + sessionId + ", object:" + objectName);
                        obj = GetSessionObject(objectSchema, objectName);
                        break;
                    default:
                        obj = CreateObject(objectSchema, objectName);
                        break;
                }

            if (obj is IHttpHandler)
                (obj as IHttpHandler).ProcessRequest(context);
            if (obj is IHttpWbo)
                (obj as IHttpWbo).setContext(this);
            return obj;

        }


        public object GetObject(string objectName, string comId)
        {
            //            log.Debug("WboSchemaContainer.Instance().GetItem(objectType)");
            WboSchema objectSchema = WboSchemaContainer.Instance().GetItem(comId);

            //            log.Debug("end WboSchemaContainer.Instance().GetItem(objectType)");
            object obj = GetObject(objectName, objectSchema);
            return obj;
        }

        public T GetObject<T>(string objectName)
        {
            string comId = WboSchemaRegisterUtils.getTypeRegId(typeof(T));

            if (!WboSchemaContainer.Instance().Contains(comId))
                throw new E_UmcNotFindRegObjcect(comId);
            WboSchema objectSchema = WboSchemaContainer.Instance().GetItem(comId);

            object obj = GetObject(objectName, objectSchema);

            if (obj == null)
                throw new E_UmcNotFindRegObjcect(objectName);

            if (!(obj is T))
                throw new E_UmcNoMatchObjectType(obj.GetType().ToString() + ":" + typeof(T).ToString());

            return (T)obj;
        }




        //public  T FindObject<T>(string sessionId, string messageId)
        //where T : class
        //{
        //    string objectId = typeof(T).Name;
        //    WboSchema objectSchema = WboSchemaContainer.Instance().GetItem(objectId);
        //    object obj = GetObject(messageId, objectSchema);
        //    if (obj is T)
        //        return (T)obj;
        //    return null;
        //}


        private static object[] GetParameters(ParameterInfo[] paramInfos, Dictionary<string, object> reqParameters)
        {
            object[] ret = new object[paramInfos.Length];
            for (int i = 0; i < paramInfos.Length; i++)
            {
                ParameterInfo pInfo = paramInfos[i];
                try
                {
                    ret[i] = reqParameters[pInfo.Name];
                }
                catch (Exception e)
                {
                    throw new Exception("缺少参数" + pInfo.Name + "," + e.Message, e);
                }
            }
            return ret;
        }

        ///// <summary>
        ///// 执行对象中的一个方法 
        ///// </summary>
        ///// <param name="wboSchema"></param>
        ///// <param name="methodId"></param>
        ///// <param name="namedParameters"></param>
        ///// <returns></returns>
        //public static object InvokeMethod(object obj, string methodId, Dictionary<string, object> namedParameters)
        //{
        //    Type t = obj.GetType();
        //    MethodInfo method = t.GetMethod(methodId);
        //    if (method == null)
        //    {
        //        throw new Exception("对象方法没有找到" + t.Name + "." + methodId);
        //    }
        //    ParameterInfo[] pInfos = method.GetParameters();
        //    object[] parameters = GetParameters(pInfos, namedParameters);
        //    return method.Invoke(obj, parameters);
        //}

        //public static MethodInfo GetClassMethodInfo(string objectType, string methodName)
        //{
        //    WboSchema objectSchema = WboSchemaContainer.Instance().GetItem(objectType);

        //    Assembly assembly = Assembly.Load(objectSchema.AssemblyName);
        //    Type t = assembly.GetType(objectSchema.ClassName, true);
        //    MethodInfo method = t.GetMethod(methodName);

        //    if (method == null)
        //    {
        //        throw new Exception("对象方法没有找到" + t.Name + "." + methodName);
        //    }

        //    return method;
        //}

        //public static ParameterInfo GetClassMethodParamInfo(string objectType, string methodName, string paramName)
        //{
        //    MethodInfo m = GetClassMethodInfo(objectType, methodName);
        //    ParameterInfo[] pars = m.GetParameters();
        //    for (int i = 0; i < pars.Length; i++)
        //    {
        //        if (pars[i].Name.Equals(paramName, StringComparison.OrdinalIgnoreCase))
        //            return pars[i];
        //    }
        //    return null;
        //}

        public static PermissionTypes GetObjectPermissionTypes(string objectType, string methodName)
        {
            WboSchema objectSchema = WboSchemaContainer.Instance().GetItem(objectType);
            WboMethodSchema wms = objectSchema.Methods.GetItem(methodName);
            return wms.PermissionTypes;
        }


        /// <summary>
        /// isStatic 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool isTypeStaticClass(Type type)
        {
            if (type == null)
                return false;
            return (type.IsClass && type.IsAbstract && type.IsSealed);
        }

        /// <summary>
        /// 通过传入wboSchema对象构架，和wbo对象的方法名及命名参数列表，调用methodName指定的方法
        /// </summary>
        /// <param name="wboSchema">要调用的对象的对象架构</param>
        /// <param name="memberName">要调用的方法名称</param>
        /// <param name="jsonNamedParams">json命名参数列表规范的参数</param>
        /// <returns>methodName指定的方法的返回结果</returns>
        public object Invoke(string wboId, string objectName, string memberName, Dictionary<string, string> jsonNamedParams)
        {
            WboSchema wboSchema = WboSchemaContainer.Instance().GetItem(wboId);
            object obj = GetObject(objectName, wboId);
            WboProxy wbop = WboProxyFactory.getWboProxy(wboSchema);

            checkPermission(wboId, objectName, memberName, wboSchema);

            return wbop.invoke(obj, memberName, jsonNamedParams);
        }

        private void checkPermission(string wboId, string objectName, string memberName, WboSchema wboSchema)
        {
            if (wboId.Equals("Security"))
                return;

            PermissionTypes mPermission = wboSchema.PermissionTypes;


            string objId = wboId;

            if (!string.IsNullOrEmpty(objectName))
                objId += "." + objectName;

            if (!string.IsNullOrEmpty(memberName))
            {
                WboMethodSchema wms = wboSchema.Methods.FindItem(memberName);
                Schema wmp = wboSchema.Properties.FindItem(memberName);
                if (wms == null && wmp == null)
                    throw new Exception(wboId + "." + memberName + "没有注册不能调用");
                if (wms != null)
                    mPermission = wms.PermissionTypes;
                if (wmp != null)
                    mPermission = PermissionTypes.Read;

                objId += "." + memberName;
            }

            if (wboId.Equals("ds"))
                Security.CheckObjectPermission(null, objectName, mPermission);
            else
                Security.CheckObjectPermission(null, objId, mPermission);
        }

        private static object InvokeMethod(object o, string wboId, string methodName, Dictionary<string, string> jsonNamedParams)
        {
            WboSchema wboSchema = WboSchemaContainer.Instance().GetItem(wboId);
            WboProxy wbop = WboProxyFactory.getWboProxy(wboSchema);
            return wbop.invokeMethd(o, methodName, jsonNamedParams);
        }

        /// <summary>
        /// 返回指定组件序列化存储的所有对象列表
        /// </summary>
        /// <param name="comId"></param>
        /// <returns></returns>
        public static List<string> dir(string comId)
        {
            List<string> ret = new List<string>();
            string path = XSite.WboPath + comId + "\\";
            if (Directory.Exists(path))
            {
                string[] fileNames = Directory.GetFiles(path);
                foreach (string fileName in fileNames)
                    ret.Add(Path.GetFileNameWithoutExtension(fileName));
            }
            return ret;
        }

        public object invoke(string req, Dictionary<string, string> jsonNamedParams)
        {
            if (string.IsNullOrEmpty(req))
                throw new Exception(string.Format(Lang.RequestNameIsNull, req));
            string memberName = null;
            string objName = null;
            string[] names = req.Split('.');
            string comId = names[0];
            object obj;

            if (USER_COMID.Equals(comId, StringComparison.OrdinalIgnoreCase))
            {
                obj = security.user;
                if (names.Length == 1)
                    return obj;
                PropertyInfo pi = obj.GetType().GetProperty(names[1]);
                return pi.GetValue(obj, null);
            }

            WboSchema wboSchema = WboSchemaContainer.Instance().GetItem(comId);
            WboProxy wbop = WboProxyFactory.getWboProxy(wboSchema);

            if (names.Length > 1)
            {
                memberName = names[names.Length - 1];
                if (wbop.hasMember(memberName))
                    objName = UmcTools.getObjName(names, 1, names.Length - 2);
                else
                {
                    // memberName = null;
                    objName = UmcTools.getObjName(names, 1);
                    if (!string.IsNullOrEmpty(objName))
                        memberName = null;
                }
            }

            checkPermission(comId, objName, memberName, wboSchema);

            obj = GetObject(objName, comId);

            if (string.IsNullOrEmpty(memberName))
                return obj;

            return invoke(obj, comId, objName, memberName, jsonNamedParams);
        }

        private object invoke(string comId, string memberName, Dictionary<string, string> jsonNamedParams)
        {
            WboSchema objectSchema = WboSchemaContainer.Instance().GetItem(comId);
            object obj = GetObject(null, objectSchema);
            if (string.IsNullOrEmpty(memberName))
                return obj;
            WboProxy wp = WboProxyFactory.getWboProxy(objectSchema);
            return wp.invoke(obj, memberName, jsonNamedParams);
        }


        public static object invoke(object o, string comId, string objName, string memberName, Dictionary<string, string> jsonNamedParams)
        {

            //WboProxy wp = WboProxyFactory.getWboProxy(objectSchema);
            if (string.IsNullOrEmpty(memberName))
                return o;

            WboSchema objectSchema = WboSchemaContainer.Instance().GetItem(comId);
            WboProxy wp = WboProxyFactory.getWboProxy(objectSchema);

            return wp.invoke(o, memberName, jsonNamedParams);
        }

        public static string getWboFileName(string wboName, string comId)
        {
            return XSite.WboPath + "\\" + comId + "\\" + wboName + ".xml";
        }

        public void delWbo(string wboName, string wboId)
        {
            freeWbo(wboName, wboId);
            string wboFile = getWboFileName(wboName, wboId);
            if (File.Exists(wboFile))
                File.Delete(wboFile);
        }

        public void keepWbo(string comId, string name, string wboJSON, string newName)
        {
            newName = string.IsNullOrEmpty(newName) ? name : newName;
            freeWbo(name, comId);
            object wbo = setWbo(comId, newName, wboJSON);

            string schemaPath = getWboFileName(newName, comId);
            string path = Path.GetDirectoryName(schemaPath);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            using (XmlWriter writer = XmlWriter.Create(schemaPath))
            {
                try
                {
                    XmlSerializer xmls = new XmlSerializer(wbo.GetType());
                    xmls.Serialize(writer, wbo);
                }
                finally
                {
                    writer.Close();
                }
            }
        }

        public object setWbo(string comId, string wboName, string wboJSON)
        {
            WboSchema objectSchema = WboSchemaContainer.Instance().GetItem(comId);
            WboProxy wboProxy = WboProxyFactory.getWboProxy(objectSchema);

            object wbo;
            try
            {
                wbo = JsonConvert.DeserializeObject(wboJSON, wboProxy.getWboType());
            }
            catch
            {
                throw new XException(string.Format(Lang.WboTypeNotMatchComId, comId));
            }

            if (wboProxy.getWboType().IsSubclassOf(typeof(ISessionWbo)))
                objectSchema.LifeCycle = LifeCycle.Session;

            string objHashKey = getObjHashKey(objectSchema.Id, wboName);
            switch (objectSchema.LifeCycle)
            {
                case LifeCycle.Global:
                    if (objects.ContainsKey(objHashKey))
                        objects[objHashKey] = wbo;
                    else
                        objects.Add(objHashKey, wbo);
                    break;
                case LifeCycle.Session:
                    string sesssionId = Session.SessionID;
                    if (SessionObjects.ContainsKey(objHashKey))
                        SessionObjects[objHashKey] = wbo;
                    else
                        SessionObjects.Add(objHashKey, wbo);
                    break;
            }
            return wbo;
        }
        public void freeWbo(string wboName, string wboTypeId)
        {
            WboSchema objectSchema = WboSchemaContainer.Instance().GetItem(wboTypeId);

            WboProxy wboProxy = WboProxyFactory.getWboProxy(objectSchema);
            if (wboProxy.getWboType().IsSubclassOf(typeof(ISessionWbo)))
                objectSchema.LifeCycle = LifeCycle.Session;

            string objHashKey = getObjHashKey(objectSchema.Id, wboName);
            switch (objectSchema.LifeCycle)
            {
                case LifeCycle.Global:

                    if (objects.ContainsKey(objHashKey))
                        objects.Remove(objHashKey);
                    break;
                case LifeCycle.Session:
                    string sesssionId = Session.SessionID;
                    if (SessionObjects.ContainsKey(objHashKey))
                        SessionObjects.Remove(objHashKey);
                    break;
            }
        }

        public static string getComId(Type type)
        {
            string[] typeIds = WboSchemaContainer.Instance().GetSchemaIds();
            foreach (string comId in typeIds)
            {
                WboSchema schema = WboSchemaContainer.Instance().GetItem(comId);
                Type regType = WboProxyFactory.getWboProxy(schema).getWboType();
                if (regType.Equals(type))
                    return comId;
            }
            return type.Name;
        }

    }
}