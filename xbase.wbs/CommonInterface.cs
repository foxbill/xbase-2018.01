using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wbs.wbap;
using System.IO;
using System.Collections;


namespace wbs
{

    /// <summary>
    /// 服务端运行时服务接口
    /// </summary>
    public interface IWbsOrb
    {
        /// <summary>
        /// 执行Request.InputStream中的请求命令
        /// </summary>
        /// <param name="requestInput"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        string HandleRequest(Stream requestInput, string sessionId);
        string HandleRequest(string requestText, string sessionId);
        WbapResponse HandleRequest(string sessionId, WbapRequest wbapRequest);
        WbapResponse Invoke(string requestText, string sessionId);
        string SerializeResponse(WbapResponse response);
       
    }

    /// <summary>
    /// 动作接口
    /// </summary>
    public interface IActionBroker
    {
        /// <summary>
        /// 执行动作
        /// </summary>
        /// <returns></returns>
        WbapResponse Execute();
        /// <summary>
        /// 执行动作
        /// </summary>
        /// <returns></returns>
        WbapResponse ExecuteFar();
        /// <summary>
        /// 构造动作
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="step"></param>
        /// <param name="action"></param>
        void BuildAction(string actionId, int step, WbapAction action);
    }

    /// <summary>
    /// 服务器端协议解析接口
    /// </summary>
    public interface IWbap
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        WbapResponse Initialize();
        /// <summary>
        /// 序列化WbapResponse
        /// </summary>
        /// <param name="wbapResponse"></param>
        /// <returns></returns>
        string SerializeResponse(WbapResponse wbapResponse);
        /// <summary>
        /// 反序列化输入流
        /// </summary>
        /// <param name="requestText"></param>
        /// <returns></returns>
        WbapRequest DeserializeRequest(string requestText);
        /// <summary>
        /// 取得动作
        /// </summary>
        /// <param name="wbapRequest"></param>
        /// <param name="response"></param>
        /// <param name="pageName"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
    }

    /// <summary>
    /// WBDL实体运行时接口
    /// </summary>
    public interface IWBDL
    {

    }
    /// <summary>
    /// 异常处理对象接口
    /// </summary>
    public interface IExceptionHandler
    {
        /// <summary>
        /// 运行时异常处理接口
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        string HandleError(Exception ex);
        /// <summary>
        /// 调试期异常处理接口
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        string DebugHandleError(Exception ex);
    }


}
