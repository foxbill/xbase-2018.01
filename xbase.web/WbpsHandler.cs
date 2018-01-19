using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.IO;
using xbase.security;
using xbase.security;
using wbs;
using xbase.wbs;
using Newtonsoft.Json;
using Common.Logging;
using xbase.Exceptions;
using xbase.umc;


namespace xbase.web
{
    public class WbpsHandler : IHttpHandler, IRequiresSessionState// IReadOnlySessionState //IRequiresSessionState
    {
        private HttpContext context;
        private Umc umc;

        private static ILog log = LogManager.GetLogger("Logger");

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (context.Session == null) return;
            log.Debug("wbps ProcessRequest " + context.Session.SessionID);

            this.context = context;
            try
            {
                umc = Umc.getInstance(context);
                string url = context.Server.UrlDecode(context.Request.UrlReferrer.PathAndQuery);
                string pageId = context.Server.UrlDecode(context.Request.UrlReferrer.AbsolutePath);
                StreamReader reader = new StreamReader(context.Request.InputStream);
                string requestText = reader.ReadToEnd();

                string logUri = umc.Security.LoginPageUrl;

                if (!logUri.StartsWith("/"))
                    logUri = "/" + logUri;
                logUri = (XSite.SiteVirPath + logUri).Replace("//", "/");

                if (!pageId.Equals(logUri, StringComparison.OrdinalIgnoreCase))
                {
                    umc.Security.CheckingUrl = context.Request.UrlReferrer.AbsoluteUri;
                }

                if (!umc.Security.CheckObjectPermission("ListData", pageId, PermissionTypes.Read))
                {
                    var s = JsonExceptionUtils.ThrowErr(SecErrs.NotPemission, XSite.SiteVirPath + umc.Security.LoginPageUrl).Serialize();
                    context.Response.Write(s);
                }
                else
                {


                    WbpsResquest req = JsonConvert.DeserializeObject<WbpsResquest>(requestText);

                    req.PageId = pageId;
                    req.Url = url;
                    req.Query = context.Request.UrlReferrer.Query;

                    if (!string.IsNullOrEmpty(req.SessionId) && !string.IsNullOrEmpty(req.FlowId) && !HasWbps())
                        throw new Exceptions.XUserException("会话操作已经过期，请F5重新刷新页面");

                    Wbps wbps = getSessionWBPS();

                    WbpsResponse resp = wbps.invoke(req, context.Session.SessionID);

                    string respText = JsonConvert.SerializeObject(resp);
                    context.Response.Write(respText);
                }
            }
            catch (Exception err)
            {
                context.Response.Write(JsonExceptionUtils.ThrowErr(err).Serialize());
            }
        }


        private Wbps getSessionWBPS()
        {

            Wbps wbps = (Wbps)context.Session[Wbps.SESSION_KEY];

            if (wbps == null)
            {
              //  wbps = new Wbps(umc.Security, XSite.GetSession(context.Session.SessionID));
                context.Session[Wbps.SESSION_KEY] = wbps;
            }

            return wbps;
        }

        private bool HasWbps()
        {
            Wbps wbps = (Wbps)context.Session[Wbps.SESSION_KEY];
            return wbps != null;
        }


    }
}
