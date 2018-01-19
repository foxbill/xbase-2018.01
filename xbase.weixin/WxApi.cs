using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xbase.weixin.contract;

namespace xbase.weixin
{
    public static class WxApi
    {
        public static PnUser getUserO(string openId)
        {
            string accToken = WeiXinUtils.tryGetAccessToken();
            return WeiXinUtils.getJsonObject<PnUser>(WxApiUrl.userInfo(accToken, openId));
        }

        public static string getUser(string openId)
        {
            
            string accToken = WeiXinUtils.tryGetAccessToken();
            return WeiXinUtils.get(WxApiUrl.userInfo(accToken, openId));
        }



    }
}
