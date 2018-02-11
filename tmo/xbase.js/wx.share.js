
var xBaseWXAPI = {
    SHARE_PARAM: "ShareSign",
    onShareTimeLine: function () { },
    onWxReady: function () {
    }
};
var WxShareType = {
    TL: 'Tl',   //朋友圈
    AM: 'AM',    //朋友
    QQ: 'QQ',   //QQ
    WB: 'WB',   //腾讯微博
    QZ: 'QZ'     //QQ空间
};

//初始分享信息
(function ($) {
    if (!$.rCall("BrowserAgent.isWeiXin"))
        return;

    var link;
    var desc;

    //xBase.onWeixinReady = function () {
    //    link = getLink();
    //    desc = $(".summary").html();
    //};
    initApi();

    function getLink() {
        var oldShareBy = /ShareSign=[a-z0-9]{8}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{12}&ShareType=\w+/gi;
        var s = location.href;
        s = s.replace(/\?from=2dcode/gi, "");//处理图灵二维码的bug
        var i = s.search(oldShareBy);
        if (i > 0)
            s = s.substr(0, i);
        if (s[s.length - 1] == "&")
            s = s.substr(0, s.length - 1);

        if (s.indexOf("?") > 0)
            s += '&'
        else
            s += '?';

        s += "ShareSign=" + xBase.User.Id;


        return s;
    }

    function getImgUrl() {
        var ret = "";
        $(".wxshare-img img").each(function () {
            if (this.src && this.src != this.baseURI && this.src.indexOf("qrcode.jpg") < 0) {
                ret = this.src;
                return false;
            }
        })
        return ret;
    }

    var initCount = 0;
    wx.error(function (res) {
        if (initCount < 5) {
            setTimeout(function () {
                initApi();
                initCount++;
            }, 200);
        } else {
            alert("微信接口初始失败，请刷新重试！");
        }
    });

    wx.ready(function () {

        wx.onMenuShareTimeline({
            title: $("title").html(), // 分享标题
            link: link + "&ShareType=" + WxShareType.TL, // 分享链接
            imgUrl: getImgUrl(),
            desc: $(".summary").html(),
            success: function () {
                xBaseWXAPI.onShareTimeLine();
            },
            cancel: function () {

            }
        });

        wx.onMenuShareAppMessage({
            title: $("title").html(), // 分享标题
            link: link + "&ShareType=" + WxShareType.AM, // 分享链接
            imgUrl: getImgUrl(),
            desc: $("[class='summary']").html(),
            success: function () {
                xBaseWXAPI.onShareTimeLine();
            },
            cancel: function () {

            }
        });//朋友
        wx.onMenuShareQQ({
            title: $("title").html(), // 分享标题
            desc: $("[class='summary']").html(),
            link: link + "&ShareType=" + WxShareType.QQ, // 分享链接
            imgUrl: getImgUrl(),
            success: function () {
                xBaseWXAPI.onShareTimeLine();
            },
            cancel: function () {

            }
        });        //qq好友
        wx.onMenuShareWeibo({
            title: $("title").html(), // 分享标题
            desc: $("[class='summary']").html(),
            link: link + "&ShareType=" + WxShareType.WB, // 分享链接
            imgUrl: getImgUrl(),
            success: function () {
                xBaseWXAPI.onShareTimeLine();
            },
            cancel: function () {

            }
        });//腾讯微博
        wx.onMenuShareQZone({
            title: $("title").html(), // 分享标题
            desc: $("[class='summary']").html(),
            link: link + "&ShareType=" + WxShareType.QZ, // 分享链接
            imgUrl: getImgUrl(),
            success: function () {
                xBaseWXAPI.onShareTimeLine();
            },
            cancel: function () {

            }
        });//qq空间

        if (xBaseWXAPI.onWxReady) {
            xBaseWXAPI.onWxReady();
        }

    });

    function initApi() {
        var cfg = $.rCall("WxJsCfg.call");
        cfg.jsApiList = [
            'onMenuShareTimeline',  //朋友圈
            'onMenuShareAppMessage',//朋友
            'onMenuShareQQ',        //qq好友
            'onMenuShareWeibo',     //腾讯微博
            'onMenuShareQZone'];    //qq空间
        wx.config(cfg);
    }

})(jQuery)

