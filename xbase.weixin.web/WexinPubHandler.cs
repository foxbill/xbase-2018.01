
using System.IO;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Xml;
using System;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;
using xbase.weixin.messagehandle;
using Common.Logging;


namespace xbase.weixin.web
{
    public class WexinPubHandler : IHttpHandler
    {
        private static ILog log = LogManager.GetLogger("Logger");
        public void ProcessRequest(HttpContext context)
        {

            String postStr = String.Empty;
            string Token = WxConfigFile.config().Token;
            string signature = context.Request.QueryString["signature"];
            string timestamp = context.Request.QueryString["timestamp"];
            string nonce = context.Request.QueryString["nonce"];

            if (HttpContext.Current.Request.HttpMethod.ToUpper() == "GET")
            {
                string echostr = context.Request.QueryString["echoStr"];
                if (CheckSignature.Check(signature, timestamp, nonce, Token))
                {
                    context.Response.Write(echostr); //返回随机字符串则表示验证通过
                }
                else
                {
                    context.Response.Write("如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
                }
            }
            else
            {
                if (!CheckSignature.Check(signature, timestamp, nonce, Token))
                {
                    context.Response.Write("参数错误！");
                }

                StreamReader reader = new StreamReader(context.Request.InputStream);
                postStr = reader.ReadToEnd();
                string sMsg = string.Empty;
                // LogUtil.WriteLog("微信服务器信息：" + sMsg);
                if (!String.IsNullOrEmpty(postStr))
                {
                    var postModel = new PostModel()
                    {
                        Signature = signature,
                        Msg_Signature = context.Request.QueryString["msg_signature"],
                        Timestamp = timestamp,
                        Nonce = nonce,
                        //以下保密信息不会（不应该）在网络上传播，请注意
                        Token = Token,
                        EncodingAESKey = WxConfigFile.config().EncodingAESKey,//根据自己后台的设置保持一致
                        AppId = WxConfigFile.config().AppID //根据自己后台的设置保持一致
                    };
                    //v4.2.2之后的版本，可以设置每个人上下文消息储存的最大数量，防止内存占用过多，如果该参数小于等于0，则不限制
                    var maxRecordCount = 10;

                    //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
                    var messageHandler = new CustomMessageHandler(context.Request.InputStream, postModel, maxRecordCount);

                    /* 如果需要添加消息去重功能，只需打开OmitRepeatedMessage功能，SDK会自动处理。
                     * 收到重复消息通常是因为微信服务器没有及时收到响应，会持续发送2-5条不等的相同内容的RequestMessage*/
                    messageHandler.OmitRepeatedMessage = true;


                    //执行微信处理过程
                    messageHandler.Execute();

                    //测试时可开启，帮助跟踪数据

                    //if (messageHandler.ResponseDocument == null)
                    //{
                    //    throw new Exception(messageHandler.RequestDocument.ToString());
                    //}
                    context.Response.Write(messageHandler.ResponseDocument.ToString());
                    //LogUtil.WriteLog("messageHandler.ResponseDocument:" + messageHandler.ResponseDocument.ToString());
                }



            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

}