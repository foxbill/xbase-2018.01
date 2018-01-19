

function getUrlParameter(paramName) {
    var urlinfo = window.location.href;  //获取当前页面的url
    var len = urlinfo.length; //获取url的长度
    var offset = urlinfo.indexOf("?"); //设置参数字符串开始的位置
    var newsidinfo = urlinfo.substr(offset, len)//取出参数字符串 这里会获得类似“id=1”这样的字符串
    //newsids = newsidinfo.split("="); //对获得的参数字符串按照“=”进行分割
    //newsid = newsids[1]; //得到参数值
    //   newsid = newsidinfo.substring(paramName + "=", /&\s/);
    pName = paramName + "=";
    var params = newsidinfo.split('&');
    for (var i = 0; i < params.length; i++) {
        var p = params[i];
        if (p.indexOf(pName) > -1) {
            return p.substr(pName.length+1, p.length);
        }
    }
    return null;
}
