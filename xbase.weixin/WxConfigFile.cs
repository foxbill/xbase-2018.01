using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace xbase.weixin
{
    //    <!--网站域名，必须80端口，不要以/结尾-->
    //<add key="Domain" value="http://www.tianmiwo.com"/>
    //<!--微信Token-->
    //<add key="Token" value="56154246320520315251"/>
    //<!--微信消息体加密对应的EncodingAESKey-->
    //<add key="EncodingAESKey" value="8MpVVYtUJ6UpEX5B1LLNYE2bSGfjOgkGE1PqKQoCGRM"/>
    //<!--微信AppId-->
    //<add key="AppID" value="wx989129ff4b73859c"/>
    //<!--微信AppSecret-->
    //<add key="AppSecret" value="975871a5d2bcd3eaa9a5e9f14b0b984a"/>

    //<!--用于微信支付的PartnerKey-->
    //<add key="PartnerKey" value=""/>
    //<!--用于微信支付的商户号-->
    //<add key="mch_id" value=""/>
    //<!--用于微信支付的设备号-->
    //<add key="device_info" value=""/>
    //<!--用于微信支付的服务端IP地址-->
    //<add key="spbill_create_ip" value=""/>

    //<!--微信Oauth: 
    // snsapi_base:      不弹出授权页面，直接跳转，只能获取用户openid;
    // snsapi_userinfo:  出授权页面，可通过openid拿到昵称、性别、所在地。并且，即使在未关注的情况下，只要用户授权，也能获取其信息.-->
    //<add key="OauthScope" value="snsapi_base"/>

    //<!--是否开启微信JS接口，1：开启，0：不开启（由于开启JS功能需要定时获取jstickect,会消耗一部分性能，所以不需要JS接口的请写0）-->
    //<add key="OpenJSSDK" value="1"/>



    public class WxConfig
    {

        private string domain = "http://www.tianmiwo.com";
        private string token = "56154246320520315251";
        private string encodingAESKey = "8MpVVYtUJ6UpEX5B1LLNYE2bSGfjOgkGE1PqKQoCGRM";
        private string appID = "wx989129ff4b73859c";
        private string appID_Open = "wxf96452b66298ba26";
        private string component_appid = "wx33dff875ebf4930e";
        private string appSecret = "975871a5d2bcd3eaa9a5e9f14b0b984a";
        private string appSecret_Open = "d331ddf2d72d8f229bdb1d9097379226";
        private string component_appsecret = "0c79e1fa963cd80cc0be99b20a18faeb";
        private string partnerKey = "65489423gjkjlkjjkl564894ydfytgoj";
        private string mchId = "1244882902";
        private string deviceInfo = "";
        private string spBillCreateIp = "";
        private string sslcertPath = "cert/apiclient_cert.p12";
        private string coupon_stock_id = "206619";
        private string xBaseKey = "xbase@2015";
        private string agentToken = "56154246320520315251"; //代理Token
        private string signKey = "530113aorqYEIR0356";

        public string AgentToken
        {
            get { return agentToken; }
            set { agentToken = value; }
        }


        public string XBaseKey
        {
            get { return xBaseKey; }
            set { xBaseKey = value; }
        }
        /// <summary>
        /// 网站域名
        /// </summary>
        public string Domain
        {
            get { return domain; }
            set { domain = value; }
        }
        /// <summary>
        /// 微信Token
        /// </summary>
        public string Token
        {
            get { return token; }
            set { token = value; }
        }
        /// <summary>
        /// 微信消息体加密对应的EncodingAESKey
        /// </summary>
        public string EncodingAESKey
        {
            get { return encodingAESKey; }
            set { encodingAESKey = value; }
        }

        /// <summary>
        /// 公众平台AppID
        /// </summary>
        public string AppID
        {
            get { return appID; }
            set { appID = value; }
        }

        /// <summary>
        /// 公众平台Secret
        /// </summary>
        public string AppSecret
        {
            get { return appSecret; }
            set { appSecret = value; }
        }


        /// <summary>
        /// 开放平台AppID
        /// </summary>
        public string AppID_Open
        {
            get { return appID_Open; }
            set { appID_Open = value; }
        }
        /// <summary>
        /// 第三方平台appid
        /// </summary>
        public string ComponentAppid
        {
            get { return component_appid; }
            set { component_appid = value; }
        }

        /// <summary>
        /// 第三方平台appsecret
        /// </summary>
        public string ComponentAppsecret
        {
            get { return component_appsecret; }
            set { component_appsecret = value; }
        }


        /// <summary>
        /// 开放平台AppSecret
        /// </summary>
        public string AppSecret_Open
        {
            get { return appSecret_Open; }
            set { appSecret_Open = value; }
        }

        /// <summary>
        /// 用于微信支付的PartnerKey
        /// </summary>
        public string PartnerKey
        {
            get { return partnerKey; }
            set { partnerKey = value; }
        }
        /// <summary>
        /// 用于微信支付的商户号
        /// </summary>
        public string MchId
        {
            get { return mchId; }
            set { mchId = value; }
        }
        /// <summary>
        ///  证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        /// </summary>
        public string SslcertPath
        {
            get { return sslcertPath; }
            set { sslcertPath = value; }
        }
        /// <summary>
        /// 用于微信支付的设备号
        /// </summary>
        public string DeviceInfo
        {
            get { return deviceInfo; }
            set { deviceInfo = value; }
        }
        /// <summary>
        /// 用于微信支付的服务端IP地址
        /// </summary>
        public string SpBillCreateIp
        {
            get { return spBillCreateIp; }
            set { spBillCreateIp = value; }
        }

        /// <summary>
        /// 代金券ID
        /// </summary>
        public string CouponStockId
        {
            get { return coupon_stock_id; }
            set { coupon_stock_id = value; }
        }




        public string SignKey { get { return signKey; } set { signKey = value; } }
    }

    public class WxConfigData : WxConfig
    {
        public string Head_img { get; set; }
        public string Nick_name { get; set; }

    }

    public static class WxConfigFile
    {
        private static WxConfig wxCfg;
        private static string configPath = System.AppDomain.CurrentDomain.BaseDirectory + "wx-config.xml";


        static WxConfigFile()
        {
            wxCfg = new WxConfig();
            if (File.Exists(configPath))
            {
                XmlSerializer xs = new XmlSerializer(typeof(WxConfig));
                XmlReader reader = XmlReader.Create(configPath);

                try
                {

                    wxCfg = xs.Deserialize(reader) as WxConfig;
                }
                finally
                {
                    reader.Close();
                }

            }
            else
            {
                XmlWriter writer = XmlWriter.Create(configPath);
                try
                {
                    XmlSerializer xmlFormat = new XmlSerializer(typeof(WxConfig));
                    xmlFormat.Serialize(writer, wxCfg);//序列化对象

                }
                finally
                {
                    writer.Close();
                }
            }

        }

        public static void writeConfig(WxConfig wxConfig)
        {
            if (wxConfig == null || wxConfig.AppID == null)
                return;
            XmlWriter writer = XmlWriter.Create(configPath);
            try
            {
                XmlSerializer xmlFormat = new XmlSerializer(typeof(WxConfig));
                xmlFormat.Serialize(writer, wxConfig);//序列化对象
                wxCfg = wxConfig;
            }
            finally
            {
                writer.Close();
            }

        }

        public static WxConfig config()
        {
            return wxCfg;
        }
    }
}
