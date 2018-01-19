
var xBase = { debug: true };

/***
*  数据类型判断工具
*
***/

TypeUtils = {
    //判断是否为数组
    isArray: function (source) {
        return '[object Array]' == Object.prototype.toString.call(source);
    },
    //判断是否为日期对象
    isDate: function (o) {
        // return o instanceof Date;
        return {}.toString.call(o) === "[object Date]" && o.toString() !== 'Invalid Date' && !isNaN(o);
    },
    //判断是否为Element对象
    isElement: function (source) {
        return !!(source && source.nodeName && source.nodeType == 1);
    },
    //判断目标参数是否为function或Function实例
    isFunction: function (source) {
        // chrome下,'function' == typeof /a/ 为true.
        return '[object Function]' == Object.prototype.toString.call(source);
    },
    //判断目标参数是否number类型或Number对象
    isNumber: function (source) {
        return '[object Number]' == Object.prototype.toString.call(source) && isFinite(source);
    },
    //判断目标参数是否为Object对象
    isObject: function (source) {
        return 'function' == typeof source || !!(source && 'object' == typeof source);
    },
    //判断目标参数是否string类型或String对象
    isString: function (source) {
        return '[object String]' == Object.prototype.toString.call(source);
    },
    //判断目标参数是否Boolean对象
    isBoolean: function (o) {
        return typeof o === 'boolean';
    }
};


/***
*  数据类型扩展函数
*
***/
Array.prototype.arr2Str = function () {
    var arr = this;
    var str = "";
    for (var i = 0; i < arr.length; i++) {
        str += arr[i] + ",";
    }
    if (str == "") { return str; }
    else { return str.substr(0, str.length - 1); }
};

Array.prototype.indexOf = function (item) {
    for (var i = 0; i < this.length; i++) {
        if (item == this[i])
            return i;
    }
    return -1;
};

Array.prototype.searchItem = function (searchHandle) {
    for (var i = 0; i < this.length; i++) {
        if (searchHandle(this[i]))
            return this[i];
    }
    return null;
};

//vVal:要插入的对象，nIdex要查如的位置
Array.prototype.insert = function (vVal, nIdx) {
    var arrTemp = this;
    if (nIdx > arrTemp.length) nIdx = arrTemp.length;
    if (nIdx < -arrTemp.length) nIdx = 0;
    if (nIdx < 0) nIdx = arrTemp.length + nIdx;

    for (var ii = arrTemp.length; ii > nIdx; ii--) {
        arrTemp[ii] = arrTemp[ii - 1];
    }
    arrTemp[nIdx] = vVal;
    return arrTemp;
};

Array.prototype.insertAfter = function (vVal, nIdx) {
    var arrTemp = this;
    if (!arrTemp.length) {
        this[0] = vVal;
        return;
    }
    for (var i = arrTemp.length; i > nIdx; i--) {
        arrTemp[i] = arrTemp[i - 1];
    }

    arrTemp[nIdx + 1] = vVal;
    return;
};


Array.prototype.removeItem = function (item) {
    var index = this.indexOf(item);
    if (index < 0) return false;
    this.splice(index, 1);
    return true;
};

Array.prototype.hasText = function (text) {
    for (var i = 0; i < this.length; i++) {
        var item = this[i];
        if (text.toLowerCase && item.toLowerCase) {
            if (text.toLowerCase() == item.toLowerCase())
                return true;
        }
    }
    return false;
};

Array.prototype.remove = function (index) {
    this.splice(index, 1);
};

Array.prototype.clear = function () {
    for (var i = this.length - 1; i > -1; i--) {
        this.splice(i, 1);
    }
};



function rCall(objMember, params, fn) {
    var ret;
    var func;
    var qryStr = "";
    var ary = objMember.split("?");
    objMember = ary[0];
    if (ary.length > 1)
        qryStr = ary[1];


    if (fn && TypeUtils.isFunction(fn))
        func = fn;
    else if (params && TypeUtils.isFunction(params))
        func = params

    var handles = [".wbo", ".call", ".dir", ".free", ".disp", ".del", ".set", ".keep", ".data", ".cols", ".grid", ".row", ".rows", ".update", ".delete", ".insert", ".sub"];

    var handle = objMember.substr(objMember.lastIndexOf("."));

    if (handles.indexOf(handle) < 0)
        objMember += ".call";

    if (qryStr)
        objMember = objMember + "?" + qryStr;

    var ErrDic = {
        "timeout": "请求超时",
        "error": "请求错误",
        "notmodified": "服务器内容不可修改",
        "parsererror": "解析返回的数据格式发生错误"
    }

    var fParams = params;

    if (params && TypeUtils.isObject(params)) {
        for (var name in params) {
            if (TypeUtils.isString(params[name]))
                fParams[name] = params[name];
            else
                fParams[name] = $.getJsonStr(params[name]);
            //这个方法有问题：JSON.stringify(params[name]);
        }
    }

    function showErr(msg, no) {
        document.open()
        document.clear();
        document.write(msg);
        document.close();
    }

    $.ajax({
        url: objMember,
        data: fParams,
        type: "post",
        async: !!func,
        processData: true,
        timeout: 10000,
        dataType: "json",
        success: function (json) {
            ret = json;
            if (ret && ret.Err) {

                if (ret.Err.Url)
                    location = ret.Err.Url;
                else if (xBase.debug) {
                    document.open();
                    document.clear();
                    document.writeln(ret.Err.Text);
                    document.writeln("<br/>");
                    document.writeln(ret.Err.ErrStack);
                    document.close();
                }
                else
                    showErr(ret.Err.Text);
            }


            //alert(this.async);
            if (func)
                func(ret);

            //  return ret;
        },
        error: function (xhr, err) {
            var msg;
            if (err)
                msg = ErrDic[err] + "," + xhr.status + ": " + xhr.statusText;
            else
                msg = err + "," + xhr.status + ": " + xhr.statusText;

            showErr("rCall " + objMember + ":" + msg);
        },
        complete: function (XHR, TS) {
            //  return ret;
        }
    });

    return ret;
}


(function ($) {
    function Url() {
        var s = window.location.href;
        var vars = [], hash;
        if (s.indexOf("?") > 0) {
            var hashes = s.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < hashes.length; i++) {
                hash = hashes[i].split('=');
                vars.push(hash[0]);
                try {
                    vars[hash[0]] = decodeURIComponent(hash[1]);
                } catch (err) {
                    vars[hash[0]] = hash[1];
                }
            }
        }
        this.queryString = vars;
    }



    $.extend({
        url: new Url(),
        rCall: rCall
    })

    $.extend({
        timeVer: function () {
            var d = new Date();
            return d.getDate() + "." + d.getHours() + "." + d.getMinutes() + "." + d.getSeconds() + "." + d.getMilliseconds() + "." + Math.random();
        },
        paramVal: function (name) {
            //alert(name);
            var x = name;
            var url = new Url();
            if (!url.queryString[x])
                return "";
            else
                return url.queryString[x]
        }
    })

    $.fn.extend({
        autoHight: function () {
            var p = $(this).parent();
            var pe = p.get()[0];
            var _this = this;
            var el = _this.get()[0];
            //_this.height(pe.clientHeight - 2);
            //_this.outerWidth(30);
            //_this.outerHeight(30);
            _this.outerWidth(p.width());
            _this.outerHeight(p.height());
            $(window).resize(function () {
                //_this.outerWidth(30);
                //_this.outerHeight(30);
                _this.outerWidth(p.width());
                _this.outerHeight(p.height());
            });
        }

    })

    $.fn.extend({
        setText: function (value, isAutoCreate) {

            var elements = $(this).get();

            for (var i = 0; i < elements.length; i++) {
                var element = elements[i];

                if (!value && value != 0)
                    value = "";

                switch (element.nodeName) {
                    case "SELECT": case "TEXTAREA":
                        element.value = value;
                        break;
                    case "INPUT":
                        if (!element.attributes["type"] || element.attributes["type"] == undefined) {
                            element.value = value;
                            break;
                        }
                        switch (element.attributes["type"].value) {
                            case "checkbox":
                                if (value == "1") {
                                    element.checked = true;
                                }
                                else if (value == "0") {
                                    element.checked = false;
                                }
                                else {
                                    element.checked = value;
                                }
                                break;
                            case "radio":
                                var elementRadio = document.getElementsByName(element.name);
                                for (var i = 0; i < elementRadio.length; i++) {
                                    if (elementRadio[i].value == value) {
                                        elementRadio[i].checked = true;
                                        break;
                                    }
                                }
                                break;
                            default:
                                element.value = value;
                                break;
                        }
                        break;
                    case "#text":
                        element.nodeValue = value;
                        break
                    case "IMG": case "IFRAME":
                        element.src = value;
                        break;
                    case "A":
                        element.href = value;
                        break;
                    default:
                        element.innerHTML = value;
                        element.innerHTML = element.innerText;
                        //$(element).html(value);
                        break;
                }
            }
        }
    });

    $.fn.extend({
        getText: function () {
            var elements = $(this).get();

            for (var i = 0; i < elements.length; i++) {
                var element = elements[i];
                switch (element.nodeName) {
                    case "TEXTAREA":
                        return element.value;
                    case "INPUT":
                        switch (element.attributes["type"].value) {
                            case "checkbox":
                                return element.checked;
                            case "radio":
                                var elementRadio = document.getElementsByName(element.name);
                                for (var i = 0; i < elementRadio.length; i++) {
                                    if (elementRadio[i].checked == "true" || elementRadio[i].checked != "") {
                                        return elementRadio[i].value;
                                    }
                                }
                                return null;
                            default:
                                return element.value;
                        }
                        break;
                    case "SELECT":
                        return element.value;
                    case "IFRAME": case "IMG":
                        return element.src;
                    case "A":
                        return element.href;
                    default:
                        return $(element).text();
                }
            }
        }
    });

    $.fn.extend({
        valu: function (text) {
            if (text != undefined)
                $(this).setText(text);

            return $(this).getText();
        }
    });

    $.fn.extend({
        ah: function () {
            $(this).autoHight();
        }
    });

    $.fn.extend({
        options: function (optionData) {
            //使用$(this)表示对哪个对象添加了扩展方法，这里是指$('#textbox' )
            var els = $(this).get();
            for (var i = 0; i < els.length; i++) {
                var el = els[i];
                if (el.tagName == "SELECT") {
                    el.options.length = 0;
                    var j = 0;
                    if (TypeUtils.isArray(optionData))
                        for (var j = 0; j < optionData.length; j++)
                            el.options.add(new Option(optionData[j], optionData[j]));
                    else
                        for (var key in optionData) {
                            el.options.add(new Option(optionData[key], key, j));
                            j++;
                        }
                }
            }
        }
    });

    $.fn.extend({
        loadIframe: function (url, fn) {
            $(this).one("load", fn);
            this.attr("src", url);
        }
    })

    $.extend({
        getJsonStr: function (json) {
            return obj2str(json);
        }
    });

    function obj2str(o) {
        var r = [];
        if (typeof o == "string") return "\"" + o.replace(/([\'\"\\])/g, "\\$1").replace(/(\n)/g, "\\n").replace(/(\r)/g, "\\r").replace(/(\t)/g, "\\t") + "\"";
        if (typeof o == "undefined") return "";
        if (typeof o == "object") {
            if (o === null) return "null";
            else if (o.length == undefined || o.length == null) {
                for (var i in o)
                    r.push(i + ":" + obj2str(o[i]))
                r = "{" + r.join() + "}"
            } else {
                for (var i = 0; i < o.length; i++)
                    r.push(obj2str(o[i]))
                r = "[" + r.join() + "]"
            }
            return r;
        }
        return o.toString();
    }


})(jQuery);

$(function () {
    var ret = $.rCall("Security.checkPage");
})