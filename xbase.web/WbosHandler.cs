using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using wbs;
using System.IO;
using xbase.security;
using xbase.security;
using Common.Logging;
using xbase.Exceptions;
using xbase.umc;


namespace xbase.web
{
    public class WbosHandler : IHttpHandler, IRequiresSessionState// IReadOnlySessionState// IRequiresSessionState 
    {

        private static ILog log = LogManager.GetLogger("Logger");
        private Umc umc;

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {

            try
            {
                if (context.Session == null) return;
                log.Debug("wbos ProcessRequest " + context.Session.SessionID);

                string sitePath = context.Request.ApplicationPath;
                if (sitePath.EndsWith("/"))
                    sitePath = sitePath.TrimEnd('/');

                StreamReader reader = new StreamReader(context.Request.InputStream);
                string requestText = reader.ReadToEnd();
                reader.Close();
                umc = Umc.getInstance(context);
                ISecurity security = umc.Security;
                Wjs wjs = new Wjs();
                wjs.setContext(umc);

                JoapRequest req = JsonToJoapRequest(requestText);

                JoapResonse resp = new JoapResonse();

                if (!security.CheckObjectPermission(req.ObjCls, req.ObjName, PermissionTypes.Read))
                {
                    security.CheckingUrl = context.Request.UrlReferrer.AbsoluteUri;
                    resp.Err = JsonExceptionUtils.ThrowErr(SecErrs.NotPemission, sitePath + security.LoginPageUrl).Err;
                }
                else
                    resp = wjs.Invoke(req, context.Session.SessionID);
                //  string jsonResp = JsonConvert.SerializeObject(resp);
                //            string jsonResp = JsonConvert.SerializeObject(resp);

                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string jsonResp = serializer.Serialize(resp);


                context.Response.Write(jsonResp);
            }
            catch (Exception err)
            {
                context.Response.Write(JsonExceptionUtils.ThrowErr(err).Serialize());
            }
        }

        private JoapRequest JsonToJoapRequest(string jsonRequest)
        {
            JoapRequest ret = JsonConvert.DeserializeObject<JoapRequest>(jsonRequest);
            return ret;
            //            JsonConvert.DeserializeObject<JoapRequest>(

            //JObject jObj = JObject.Parse(jsonRequest);

            //ListData<string> pks = ret.Paramates.Keys.ToList();
            //for (int i = 0; i < pks.Count; i++)
            //{
            //    string paramName = pks[i];
            //    JToken jt = jObj["Paramates"][paramName];
            //    //ParameterInfo pInfo = umc.Umc.GetClassMethodParamInfo(ret.ObjCls, ret.Method, paramName);
            //    string js = jt.ToString();
            //    //object o = JsonConvert.DeserializeObject(js, pInfo.ParameterType);
            //    ret.Paramates[paramName] = js;
            //}

            //return ret;

        }

    }
}
