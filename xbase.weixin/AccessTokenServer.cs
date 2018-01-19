using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xbase.weixin
{
    public static class AccessTokenServer
    {

        private static AccessToken _accessToken = null;
        private static string _code=;

        public static string ReqUrl
        {
            get
            {
                string reqUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
                return string.Format(reqUrl, WxConfigFile.config().AppID, WxConfigFile.config().AppSecret);
            }
        }

        public static string ReqUrlOA20
        {
            get
            {
                string reqUrl = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={2}&code={3}&grant_type=authorization_code";
                return string.Format(reqUrl, WxConfigFile.config().AppID, WxConfigFile.config().AppSecret);
            }
        }

        public static string AccessToken
        {
            get
            {
                if (_accessToken == null)
                    getToken();
                return _accessToken.access_token;
            }
        }

        private static void getToken()
        {
            _accessToken = WeiXinUtils.getJsonObject<AccessToken>(ReqUrl);
        }

        private static void refreshToken()
        {

        }

    }
}
