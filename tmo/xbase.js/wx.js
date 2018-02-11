//apis=['onMenuShareTimeline',  'onMenuShareAppMessage','onMenuShareQQ','onMenuShareWeibo','onMenuShareQZone']
function initWxApi(apis, fnReady, fnErr) {

    if (!$.rCall("BrowserAgent.isWeiXin"))
        return;

    var initCount = 0;

    wx.ready(function () {
        if (fnReady)
            fnReady();
    });

    wx.error(function (res) {
        if (initCount < 5) {
            setTimeout(function () {
                config();
                initCount++;
            }, 200);
        } else {
            if (fnErr)
                fnErr(res);
            else {
                console.error("微信接口初始失败，请刷新重试！", res);
                alert("微信接口初始失败，请刷新重试！");
            }
        }
    });


    function config() {
        var cfg = $.rCall("WxJsCfg.call");
        cfg.jsApiList = apis;    //qq空间
        wx.config(cfg);
    }
    config();
}