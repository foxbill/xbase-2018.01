using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Collections;
using System.IO.Compression;
using xbase.security.wechat;
using Newtonsoft.Json;
using xbase.Exceptions;

namespace xbase.security.wechat
{
    public static class WeChatLoginService
    {
        public const string WeChatUrl = "https://api.weixin.qq.com/sns/userinfo?access_token=[ACCESS_TOKEN]&openid=[OPENID]";
        private const string DefaultUserAgent = "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/21.0.1180.89 Safari/537.1/(2.0 xBase)";

        private static void BugFix_CookieDomain(CookieContainer cookieContainer)
        {
            System.Type _ContainerType = typeof(CookieContainer);
            Hashtable table = (Hashtable)_ContainerType.InvokeMember("m_domainTable",
                                       System.Reflection.BindingFlags.NonPublic |
                                       System.Reflection.BindingFlags.GetField |
                                       System.Reflection.BindingFlags.Instance,
                                       null,
                                       cookieContainer,
                                       new object[] { });
            ArrayList keys = new ArrayList(table.Keys);
            foreach (string keyObj in keys)
            {
                string key = (keyObj as string);
                if (key[0] == '.')
                {
                    string newKey = key.Remove(0, 1);
                    table[newKey] = table[keyObj];
                }
            }
        }

        internal static string utf8ToAnsi(string UtfString)
        {
            return Encoding.UTF8.GetString(Encoding.ASCII.GetBytes(UtfString.ToCharArray()));
        }
        private static string DoGet(String url)
        {
            String html = "";
            StreamReader reader = null;
            HttpWebRequest webReqst = (HttpWebRequest)WebRequest.Create(url);
            CookieContainer CC = new CookieContainer();
            webReqst.Method = "GET";
            webReqst.UserAgent = DefaultUserAgent;
            webReqst.KeepAlive = true;
            webReqst.CookieContainer = CC;
            webReqst.Timeout = 30000;
            webReqst.ReadWriteTimeout = 30000;
            try
            {
                HttpWebResponse webResponse = (HttpWebResponse)webReqst.GetResponse();
                BugFix_CookieDomain(CC);
                if (webResponse.StatusCode == HttpStatusCode.OK && webResponse.ContentLength < 1024 * 1024)
                {
                    Stream stream = webResponse.GetResponseStream();
                    stream.ReadTimeout = 30000;
                    if (webResponse.ContentEncoding == "gzip")
                    {
                        reader = new StreamReader(new GZipStream(stream, CompressionMode.Decompress), Encoding.Default);
                    }
                    else
                    {
                        reader = new StreamReader(stream, Encoding.UTF8);
                    }
                    html = reader.ReadToEnd();
                }
            }
            catch
            {

            }

            return html;
        }

        internal static User requestUser(WeChatOS tokenInfo)
        {
            string url = WeChatUrl.Replace("[ACCESS_TOKEN]", tokenInfo.access_token);
            url = url.Replace("[OPENID]", tokenInfo.openid);

            string s = DoGet(url);
            if (string.IsNullOrEmpty(s))
                throw new Exception("微信登录服务没有返回");
            if (s.Contains("errcode") && s.Contains("errmsg"))
            {
                WeChatErr err = JsonConvert.DeserializeObject<WeChatErr>(s);
                throw new XException("微信认证错误Colde：" + err.errcode + "Msg:" + err.errmsg);
            }
            WeChatPersonalInfo wpi = JsonConvert.DeserializeObject<WeChatPersonalInfo>(s);
            User user = new User();
            user.Id = wpi.unionid;
            user.DisplayName = wpi.nickname;
            user.HeadPhoto = wpi.headimgurl;
            return user;
        }
    }
}
