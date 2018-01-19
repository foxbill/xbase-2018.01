using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using System.Web;
using System.IO;

namespace xbase.web
{
    public class XJSHandler : IHttpHandler//,IRequiresSessionState// IReadOnlySessionState
    {

        #region IHttpHandler 成员

        bool IHttpHandler.IsReusable
        {
            get { return true; }
        }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            string sitePath = HttpRuntime.AppDomainAppPath;
            string reqFile = Path.GetFileNameWithoutExtension(Request.Url.LocalPath);
            Response.WriteFile(sitePath + "xbase.js\\" + reqFile + ".js");

        }

        #endregion
    }
}
