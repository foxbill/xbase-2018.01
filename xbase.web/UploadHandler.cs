using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json;
using xbase.umc;
using System.IO;
using xbase.admin;
using xbase.Exceptions;
using xbase.local;


namespace xbase.web
{
    public class UploadHandler : IHttpHandler, IRequiresSessionState
    {
        const string PATH = "path";

        private HttpRequest Request;
        private HttpServerUtility Server;
        private HttpSessionState Session;
        private HttpResponse Response;
        private string url;
        private bool isUploaded;
        private string err;

        public bool IsUploaded
        {
            get { return isUploaded; }
            set { isUploaded = value; }
        }

        public string RetHtml
        {
            get
            {
                if (isUploaded)
                    return Lang.UploadSucceed + url;
                else

                    return Lang.UploadFail + err;
            }
        }

        public string Url
        {
            get { return url; }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }



        public void ProcessRequest(HttpContext context)
        {

            //ISecurity sec = SecurityFactory.GetSecurity(Session.SessionID);
            //            _userId = sec.UserContext.UserId;

            Request = context.Request;
            Server = context.Server;
            Session = context.Session;
            Response = context.Response;

            isUploaded = false;
            try
            {
                if (Request.Files.Count < 1)
                    throw new XUserException(Lang.NoUploadFile);

                HttpPostedFile file = Request.Files[0];

                string virPath = "/upload-files/" + context.Session.SessionID;
                if (!string.IsNullOrEmpty(Request.QueryString[PATH]))
                    virPath = Request.QueryString[PATH];
                if (!virPath.EndsWith("/"))
                    virPath = virPath + "/";

                virPath = virPath + Path.GetFileName(file.FileName);


                string diskPath = Server.MapPath(virPath);

                string dir = Path.GetDirectoryName(diskPath);

                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                file.SaveAs(diskPath);
                url = virPath;
                isUploaded = true;
                Response.Write(RetHtml);

            }
            catch (Exception e)
            {
                string jsonErr = JsonExceptionUtils.ThrowErr(e).Serialize();
                err = e.Message;
                context.Response.Write(jsonErr);
                return;
            }
        }
    }
}