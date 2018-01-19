using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xbase.weixin.users
{
    public static class UserContext
    {
        public static UserInfo CurrentUser(string openId)
        {
            string accToken = WeiXinUtils.tryGetAccessToken();
            return WeiXinUtils.getJsonObject<UserInfo>(WxApiUrl.userInfo(accToken, openId));
        }
    }
}
