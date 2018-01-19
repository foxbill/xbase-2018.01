using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Common.Logging;
using System.IO;
using wbs;
using xbase.umc;
using xbase.Exceptions;

namespace xbase.web
{
    public class WbcHandler  : IHttpHandler, IRequiresSessionState// IReadOnlySessionState// IRequiresSessionState 
    {

        private static ILog log = LogManager.GetLogger("Logger");

        public bool IsReusable
        {
            get { return true  ; }
        }

        public void ProcessRequest(HttpContext context)
        {

            try
            {
                HttpRequest hReq=context.Request;
                HttpResponse hResp = context.Response;
                HttpSessionState hSes=context.Session;


                string objType= Path.GetFileNameWithoutExtension(hReq.Url.LocalPath);
                string name = hReq.QueryString["name"];
                Umc umc = Umc.getInstance(context);
                object wbc = umc.GetObject(name, objType);

                if (!(wbc is IVisualWbo)) throw new XException("调用的对象不是可视化对象，不能在页面上展示");
                hResp.Write((wbc as IVisualWbo).Render(""));
            }
            catch (Exception err)
            {
                context.Response.Write(JsonExceptionUtils.ThrowErr(err).Serialize());
            }
        }


    }
}
