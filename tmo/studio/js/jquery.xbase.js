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
}


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
}

Array.prototype.indexOf = function (item) {
    for (var i = 0; i < this.length; i++) {
        if (item == this[i])
            return i;
    }
    return -1;
}

Array.prototype.searchItem = function (searchHandle) {
    for (var i = 0; i < this.length; i++) {
        if (searchHandle(this[i]))
            return this[i];
    }
    return null;
}

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
}

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
}


Array.prototype.removeItem = function (item) {
    var index = this.indexOf(item);
    if (index < 0) return false;
    this.splice(index, 1);
    return true;
}

Array.prototype.hasText = function (text) {
    for (var i = 0; i < this.length; i++) {
        var item = this[i];
        if (text.toLowerCase && item.toLowerCase) {
            if (text.toLowerCase() == item.toLowerCase())
                return true;
        }
    }
    return false;
}

Array.prototype.remove = function (index) {
    this.splice(index, 1);
}

Array.prototype.clear = function () {
    for (var i = this.length - 1; i > -1; i--) {
        this.splice(i, 1);
    }
}

function $Url() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    this.queryString = vars
    //    return this;
}



$.extend({
    url: new $Url()
})

$.extend({
    timeVer: function () {
        var d = new Date();
        return d.getDate() + "." + d.getHours() + "." + d.getMinutes() + "." + d.getSeconds() + "." + d.getMilliseconds();
    }
})

$.fn.extend({
    autoHight: function () {
        var p = $(this).parent();
        var pe = p.get()[0];
        var _this = this;
        var el = _this.get()[0];
        _this.height(pe.clientHeight - 2);
        $(window).resize(function () {
            _this.height(10);
            _this.height(
                pe.clientHeight - (parseInt(el.style.borderTopWidth) || 0)
                - (parseInt(el.style.borderBottomWidth) || 0)
               );
            //            $(this).css("height", pe.clientHeight + "px");
            //            if (document.getElementById("mm2").clientHeight < document.getElementById("mm1").clientHeight) {
            //                document.getElementById("mm2").style.height = document.getElementById("mm1").offsetHeight + "px";

            //            }
            //            else {
            //                document.getElementById("mm1").style.height = document.getElementById("mm2").offsetHeight + "px";
            //            }
        })
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
                case "INPUT": case "SELECT":
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
                default:
                    element.innerText = value;
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
                default:
                    return element.innerText;
            }
        }
    }
});