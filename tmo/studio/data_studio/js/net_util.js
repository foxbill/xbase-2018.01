/**
* @summary：异步传输
* @example：XAjax.request("http:xxx?ss=ss",myFunction);
**/
XAjax = {
    //版本号
    version: "1.0",

    //错误标识
    SERVER_ERR: "ServiceError::",

    //内部方法，创建HttpRequest
    initRequest: function() {
        var http_request = false;
        if (window.ActiveXObject) { //IE
            try {
                http_request = new ActiveXObject("Msxml2.XMLHTTP");
            }
            catch (e) {
                try {
                    http_request = new ActiveXObject("Microsoft.XMLHTTP");
                }
                catch (e) {
                }
            }
        }
        else {
            if (window.XMLHttpRequest) { //Mozilla 
                http_request = new XMLHttpRequest();
                if (http_request.overrideMimeType) {//MiME
                    http_request.overrideMimeType("text/xml");
                }
            }
        }
        if (!http_request) {
            // 
            window.alert("Ajax object cannt create, XMLHttpRequest=null");
            return false;
        }
        return http_request;
    },

    /**
    * @summary：同步请求
    * @example：XAjax.request("http:xxx?ss=ss",myFunction);
    **/
    request: function(url, fun, postData) {
        var ajxObj = this.initRequest();

        ajxObj.onreadystatechange = function() {
            if (ajxObj.readyState == 4) {
                if (ajxObj.status == 200) {
                    var ret = ajxObj.responseText;
                    if (ret.indexOf(XAjax.SERVER_ERR, 0) > -1)
                        alert(ret);
                    else if (fun != null) {
                        progressBarHide();
                        fun(ret);
                    }
                }
                else {
                    alert("服务器错误：" + ajxObj.status);
                    return;
                }
            }
            else {
                // window.status = "ajax ready is " + ajxObj.readyState;
                progressBarShow();
            }

        }
        ajxObj.open("post", url, false);
        ajxObj.send(postData);
    },
//
    SynRequest: function(url, postData) {
        var ajxObj = this.initRequest();
        var ajaxRet = null;
        var hasRet = false;

        ajxObj.onreadystatechange = function() {
            if (ajxObj.readyState == 4) {
                if (ajxObj.status == 200) {
                    var ret = ajxObj.responseText;
                    if (ret.indexOf(XAjax.SERVER_ERR, 0) > -1)
                        alert(ret);
                    else {
                        progressBarHide();
                        ajaxRet = ret;
                        hasRet = true;
                    }
                }
                else {
                    alert("服务器错误：" + ajxObj.status);
                    hasRet = true;
                    //return;
                }
            }
            else {
                // window.status = "ajax ready is " + ajxObj.readyState;
                progressBarShow();
            }

        }
        ajxObj.open("post", url, false);
        ajxObj.send(postData);
        while (!hasRet) { }
        return ajaxRet
    },

    obj2str: function(o) {
        var r = [];
        if (typeof o == "string") return "\"" + o.replace(/([\'\"\\])/g, "\\$1").replace(/(\n)/g, "\\n").replace(/(\r)/g, "\\r").replace(/(\t)/g, "\\t") + "\"";
        if (typeof o == "undefined") return "";
        if (typeof o == "object") {
            if (o === null) return "null";
            else if (!o.sort) {
                for (var i in o)
                    r.push(i + ":" + this.obj2str(o[i]))
                r = "{" + r.join() + "}"
            } else {
                for (var i = 0; i < o.length; i++)
                    r.push(this.obj2str(o[i]))
                r = "[" + r.join() + "]"
            }
            return r;
        }
        return o.toString();
    }
}
function progressBarShow() {
    //    var pro = document.getElementById("ProgressBar_IMG");
    //    if (pro == null) {
    //        pro = document.createElement("img");
    //        pro.id = "ProgressBar_IMG";
    //        pro.src = "../ProgressBar.gif";
    //        pro.style.position = "absolute";
    //        //        pro.style.zIndex = -10;
    //        var clientHeight = document.documentElement.clientHeight;
    //        var clientWidth = document.documentElement.clientWidth;
    //        pro.style.left = clientWidth / 2 - pro.width;
    //        pro.style.top = clientHeight / 2 - pro.height;
    //        document.body.insertAdjacentElement("beforeend", pro);
    //    }
    //    pro.style.display = "block";

}
function progressBarHide() {
    var pro = document.getElementById("ProgressBar_IMG");
    if (pro != null) {
        pro.style.display = "none";
    }
}
function URLParamRequest(strName) {
    var strHref = document.location.toString();

    var intPos = strHref.indexOf("?");
    var strRight = strHref.substr(intPos + 1); //获取到右边的参数部分
    if (strHref.indexOf("&") < 0)//只有一个参数时，添加&符方便分割成数组
    {
        strRight += "&";
    }
    var arrTmp = strRight.split("&"); //以&分割成数组

    for (var i = 0; i < arrTmp.length; i++) //循环数组
    {
        var dIntPos = arrTmp[i].indexOf("=");
        var paraName = arrTmp[i].substr(0, dIntPos);
        var paraData = arrTmp[i].substr(dIntPos + 1);

        if (paraName.toUpperCase() == strName.toUpperCase()) {
            return decodeURIComponent(paraData);
        }
    }
    return "";
}
