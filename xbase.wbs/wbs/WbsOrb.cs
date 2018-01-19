using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using wbs.wbap;
using xBase;
using XSecurity.Interface;

namespace wbs
{

    /// <summary>
    /// 服务端运行时服务实现类
    /// </summary>
    public static class WbsOrb
    {
        public const string SERVER_ERR = "ServiceError::";
        private static ISecurity _ISec;
//        private WbapRequest request;
        /// <summary>
        /// 需要存储到Umc.Sessions[sessionId].Pages[pageName]中;
        /// </summary>
//        private static PageController pageController=null;

        public static ISecurity ISecHandler
        {
            get
            {
                return _ISec;
            }
            set
            {
                _ISec = value;
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public static WbapResponse Invoke(string textRequest, string sessionId)
        {
            WbapRequest request = DeserializeRequest(textRequest);
            return   Invoke(request, sessionId);
        }

        public static WbapResponse Invoke(Stream streamRequest, string sessionId)
        {
            StreamReader reader = new StreamReader(streamRequest);
            string textRequest = reader.ReadToEnd();
            reader.Close();
            WbapRequest request = DeserializeRequest(textRequest);
            return  Invoke(request, sessionId);
        }

        public static WbapResponse Invoke(WbapRequest request, string sessionId)
        {

            PageController pageController = new PageController(request.PageName, sessionId);

            pageController.SetRequestData(request.ElementBinds);
            
            Wbap wbap = new Wbap(pageController);
            wbap.ISecHandler = _ISec;

            string pageName = request.PageName;
            string actionId = request.ActionId;


            WbapResponse wbapResponse = null;

            if (string.IsNullOrEmpty(actionId) || actionId.Equals("Initialize", StringComparison.OrdinalIgnoreCase))
            {
                wbapResponse = wbap.Initialize(pageName, sessionId);
            }
            else
            {
                wbapResponse = new WbapResponse();
                ActionBroker action = wbap.GetAction(request, ref wbapResponse, pageName, sessionId, _ISec);
                if (action != null)
                    wbapResponse = action.Execute();
            }
            return wbapResponse;

        }

        
        public static string SerializeResponse(WbapResponse response)
        {
           return  Wbap.SerializeResponse(response);
        }


        public static WbapRequest DeserializeRequest(string str){
            return Wbap.DeserializeRequest(str);
        }

        #region IErrorHandler 成员
        /// <summary>
        /// 错误处理函数
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static  string HandleError(Exception ex)
        {
            string err =ex.GetType()+"->" +ex.Message+"\n"+ex;
            if (!(ex is XException))
                err = "系统故障，请与系统管理员联系";
            StringBuilder _sbErrorMsg = new StringBuilder();
            _sbErrorMsg.Append(SERVER_ERR);
            _sbErrorMsg.AppendLine();
            _sbErrorMsg.Append(err);
            _sbErrorMsg.AppendLine();
            _sbErrorMsg.AppendLine(ex.Message);
            return _sbErrorMsg.ToString();
        }
        /// <summary>
        /// 调试期间错误处理
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string DebugHandleError(Exception ex)
        {
            StringBuilder _sbErrorMsg = new StringBuilder();
            _sbErrorMsg.Append(SERVER_ERR);
            _sbErrorMsg.AppendLine();
            _sbErrorMsg.Append(ex.Message);
            _sbErrorMsg.AppendLine();
            _sbErrorMsg.Append(ex);
            return _sbErrorMsg.ToString();
        }

        #endregion
    }
}
