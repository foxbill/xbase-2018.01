using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using xbase.umc;
using System.IO;


namespace xbase.web
{
    public class GridHandler : IHttpHandler, IRequiresSessionState
    {
        private string objType;
        private string objName;
        private HttpRequest Request;
        private HttpServerUtility Server;
        private HttpSessionState Session;
        private HttpResponse Response;

        public void ProcessRequest(HttpContext context)
        {
            Request = context.Request;
            Server = context.Server;
            Session = context.Session;
            Response = context.Response;


            parseRequestPath(context.Request.Path);
            //   Dictionary<string, string> jsonNameValues = this.getRequestGetNameValueParams(context.Request);
            //  if (jsonNameValues == null)

            Dictionary<string, string> jsonNameValues = WboHandlerTools.getRequestPostNameValueParams(context.Request);
            Umc umc = Umc.getInstance(context);
            object obj = umc.GetObject(objName, objType);
           // object resp = Umc.invoke(obj, objType, "getPageRows", jsonNameValues);
        }

        private void parseRequestPath(string path)
        {
            path = Path.GetFileNameWithoutExtension(path);
            string[] ary = path.Split(',');
            if (ary.Length == 2)
            {
                objType = ary[0];
                objName = ary[1];
            }
            else if (ary.Length == 1)
            {
                objType = XSite.DataSourceTypeId;
                objName = ary[0];
            }

        }


        public bool IsReusable
        {
            get { return false; }
        }
    }
}
