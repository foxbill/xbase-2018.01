using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using System.Web;
using System.IO;
using xbase.umc;
using xbase.data;
using xbase.Exceptions;
using Newtonsoft.Json;

namespace xbase.web
{
    public class DataAccessHandler : IHttpHandler, IRequiresSessionState
    {

        public bool IsReusable
        {
            get { return true; }
        }


        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string path = context.Request.Path;
                string ext = Path.GetExtension(path);
                string dsName = Path.GetFileNameWithoutExtension(path);

                Dictionary<string, string> postParams = HandlerUtils.getPostParams(context);
                Dictionary<string, string> queryParams = HandlerUtils.getQueryParams(context);

                Umc umc = Umc.getInstance(context);
                
              //  object ds = umc.GetObject(dsName, "ds");

                //if (ds is DataSource)
                //    (ds as DataSource).queryParams = HandlerUtils.getQueryParams(context);

                //    foreach (string propName in postParams.Keys)
                //   {
                //        Umc.invoke(ds, "ds", propName, new Dictionary<string, string>() { { "value", postParams[propName] } });
                //   }

                string memberName = ext.TrimStart('.');
                if (ext.EndsWith(".form"))
                    postParams = null;
                object resp = umc.Invoke("ds", dsName, memberName, postParams);
                string strResp = JsonConvert.SerializeObject(resp);
                context.Response.Write(strResp);
            }
            catch (System.Reflection.TargetInvocationException e1)
            {
                string jsonErr = JsonExceptionUtils.ThrowErr(e1.InnerException).Serialize();
                if (e1.InnerException is PermissionException)
                    jsonErr = JsonExceptionUtils.ThrowErr(SecErrs.NotPemission, (e1.InnerException as PermissionException).LoginUrl).Serialize();
                context.Response.Write(jsonErr);
            }
            catch (PermissionException e2)
            {
                string jsonErr = JsonExceptionUtils.ThrowErr(10, e2.Message, e2.LoginUrl).Serialize();
                context.Response.Write(jsonErr);
            }

            catch (Exception e)
            {
                string jsonErr = JsonExceptionUtils.ThrowErr(e).Serialize();
                context.Response.Write(jsonErr);
            }
        }

    }
}
