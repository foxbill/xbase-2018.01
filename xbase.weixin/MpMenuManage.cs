using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xbase.weixin
{
    public class MpMenuManage
    {
        private ButtonGroup buildMenu()
        {
            ButtonGroup bg = new ButtonGroup();

            //单击
            bg.button.Add(new SingleClickButton()
            {
                name = "单击测试",
                key = "OneClick",
                type = ButtonType.click.ToString(),//默认已经设为此类型，这里只作为演示
            });

            //二级菜单
            var subButton = new SubButton()
            {
                name = "二级菜单"
            };
            subButton.sub_button.Add(new SingleClickButton()
            {
                key = "SubClickRoot_Text",
                name = "返回文本"
            });
            subButton.sub_button.Add(new SingleClickButton()
            {
                key = "SubClickRoot_News",
                name = "返回图文"
            });
            subButton.sub_button.Add(new SingleClickButton()
            {
                key = "SubClickRoot_Music",
                name = "返回音乐"
            });
            subButton.sub_button.Add(new SingleViewButton()
            {
                url = "http://wwww.tianmiwo.com",
                name = "Url跳转"
            });
            bg.button.Add(subButton);

            return bg;
        }

        public WxJsonResult createMenu()
        {
            //var accessToken = AccessTokenContainer.TryGetToken(appId, appSecret).access_token;
            return CommonApi.CreateMenu(WeiXinUtils.tryGetAccessToken(), buildMenu());
        }

        public GetMenuResult getMenu()
        {
            return CommonApi.GetMenu(WeiXinUtils.tryGetAccessToken());
        }


    }
}
