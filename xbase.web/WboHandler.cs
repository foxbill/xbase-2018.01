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
using xbase.data;
using xbase.security;


namespace xbase.web
{
    public class WboHandler : IHttpHandler, IRequiresSessionState
    {
        private string reqName;
        private string wboTypeId = null;
        private string wboName = null;
        private string memberName = null;
        private string ext;
        private string connName;
        private string dsName;
        private string dsFieldName;
        private Umc umc;

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


        private string getWboNameFromArray(string[] reqAry)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i < reqAry.Length; i++)
            {
                sb.Append('.');
                sb.Append(reqAry[i]);
            }
            sb.Remove(0, 1);
            return sb.ToString();
        }

        private void parseRequestPath(string requestPath)
        {
            string[] singleObjActions = new string[] { ".del", ".update", ".free" };
            reqName = Path.GetFileNameWithoutExtension(requestPath);
            string[] reqAry = reqName.Split('.');
            ext = Path.GetExtension(requestPath);

            this.wboTypeId = reqAry[0];

            if (singleObjActions.Contains(ext))
            {
                wboName = getWboNameFromArray(reqAry);
            }
            else if (reqAry.Length == 2)
                this.memberName = reqAry[1];
            else if (reqAry.Length == 3)
            {
                this.wboName = reqAry[1];
                this.memberName = reqAry[2];
            }
            else
                this.memberName = null;
        }

        private static void writeToJsonResponse(HttpResponse resp, object obj)
        {

            string strResp = JsonConvert.SerializeObject(obj);
            //context.Response.end
            resp.Clear();
            resp.BufferOutput = true;
            //    if (resp.GetType() == typeof(string))
            //        context.Response.Write(resp);
            //    else
            resp.Write(strResp);

        }

        public void ProcessRequest(HttpContext context)
        {

            try
            {
               

                umc = Umc.getInstance(context);
                parseRequestPath(context.Request.Path);
                Dictionary<string, string> jsonNameValues = HandlerUtils.getPostParams(context);

                if (ext.Equals(".keep"))
                {
                    umc.keepWbo(
                       this.wboTypeId,
                       this.wboName,
                       jsonNameValues["wboJSON"],
                       jsonNameValues["newName"]
                    );

                    writeToJsonResponse(context.Response, null);
                    return;
                }

                if (ext.Equals(".free"))
                {
                    umc.freeWbo(wboName, wboTypeId);
                    writeToJsonResponse(context.Response, null);
                    return;
                }

                if (ext.Equals(".del"))
                {
                    umc.delWbo(wboName, wboTypeId);
                    writeToJsonResponse(context.Response, null);
                    return;
                }

                if (ext.Equals(".disp", StringComparison.OrdinalIgnoreCase))
                {
                    object ret = umc.GetObject(wboName, this.wboTypeId);
                    if (!(ret is IVisualWbo))
                        throw new XException(Lang.ObjectIsNotVisualWbo);
                    string elementName = "";
                    if (jsonNameValues.ContainsKey("elementName"))
                        elementName = jsonNameValues["elementName"];
                    string html = (ret as IVisualWbo).Render(elementName);
                    context.Response.Write(html);
                    return;
                }

                if (reqName.EndsWith(".post"))
                    jsonNameValues = null;

                object obj;
                if (ext.Equals(".dir"))
                    obj = Umc.dir(wboTypeId);
                else
                    obj = umc.invoke(reqName, jsonNameValues);

                writeToJsonResponse(context.Response, obj);

                //context.Response.End();
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