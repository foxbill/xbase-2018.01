using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using xbase.umc;
using xbase.wbs;
using xbase;
using xbase.security;
using xbase.Exceptions;

namespace wbs
{
    public class JoapObject
    {
        private string objectClass = "";
        private string objectUrl = "";
        private object data = null;

        public string ObjectClass
        {
            get { return objectClass; }
            set { objectClass = value; }
        }

        public string ObjectUrl
        {
            get { return objectUrl; }
            set { objectUrl = value; }
        }

        public object Data
        {
            get { return data; }
            set { data = value; }
        }
    }

    public class JoapRequest
    {
        private string id;
        private string objCls;
        private string objName;
        private string method;
        private string sessionId;
        private object objData;

        private Dictionary<string, string> paramates = new Dictionary<string, string>();

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string ObjCls
        {
            get { return objCls; }
            set { objCls = value; }
        }

        public string ObjName
        {
            get { return objName; }
            set { objName = value; }
        }

        public string Method
        {
            get { return method; }
            set { method = value; }
        }

        public object ObjData
        {
            get { return objData; }
            set { objData = value; }
        }

        public Dictionary<string, string> Paramates
        {
            get { return paramates; }
            set { paramates = value; }
        }
    }

    public class JoapResonse : JsonResponse
    {
        private string id;
        private JoapObject objData = new JoapObject();
        private JoapObject retData = new JoapObject();

        public JoapObject ObjData
        {
            get { return objData; }
            set { objData = value; }
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public JoapObject RetData
        {
            get { return retData; }
            set { retData = value; }
        }
    }

    public class Wjs:HttpWbo
    {
        public JoapResonse Invoke(JoapRequest joapRequest, string sessionId)
        {
            try
            {
                object o = Umc.GetObject(joapRequest.ObjName, joapRequest.ObjCls);

                JoapResonse jr = new JoapResonse();
                if (!string.IsNullOrEmpty(joapRequest.Method))
//                    jr.RetData.Data = Umc.InvokeMethod(o,joapRequest.ObjCls, joapRequest.Method, joapRequest.Paramates);
                   // jr.RetData.Data = Umc.InvokeMethod(o, joapRequest.Method, joapRequest.Paramates);
                jr.ObjData.Data = o;
                return jr;
            }
            catch (Exception err)
            {
                string msg = err.Message;
                Exception et = err.InnerException;
                while (et != null)
                {
                    msg += ",\n" + et.Message;
                    et = et.InnerException;
                }
                Exception newErr = new Exception("执行命令" + joapRequest.Method + "时，发生错误" + msg, err);
                throw newErr;
            }

        }
    }

}
