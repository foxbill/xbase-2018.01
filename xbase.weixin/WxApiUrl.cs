using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xbase.weixin
{
    public sealed class WxApiUrl
    {
        //获取用户接口
        private const string GET_USER_INFO = @"https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN";
        public static string userInfo(string accessToken, string openid)
        {
            return string.Format(GET_USER_INFO, accessToken, openid);
        }

        /// <summary>
        ///临时媒体下载接口 
        /// </summary>
        private const string MEDIA_DOWN = @"https://api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}";
        public static string mediaDown(string accessToken, string mediaId)
        {
            return string.Format(MEDIA_DOWN, accessToken, mediaId);
        }

    }
}
