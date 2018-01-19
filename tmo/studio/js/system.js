/****************************   
*    system.js 单元
*   
*    1.处理浏览器兼容
*    2.数据类型扩展方法  
*    3.文档操作工具
*
*****************************/


function elementReadyRun(element, fun, args) {
    var _this = this;

    if (!element) {
        alert(" not element when execute elementReadyRun()");
        return;
    }

    if (element.readyState && (element.readyState == "complete" || element.readyState == "loaded")) {
        fun(args);
        return;
    }

    var t = 0;


    var timer = setInterval(function () {
        if (element.readyState && (element.readyState == "complete" || element.readyState == "loaded")) {
            clearInterval(timer);
            fun(args);
            return;
        }

        t++;

        if (t > 5000) {
            alert("等待原始装载超时，tagName=" + element.tagName + ",Id=" + element.id + ",");
            clearInterval(timer);
        }

    }, 50);

}

var SystemPro = {

    //确保一个元素装载成功后运行函数
    eTime: null,
    wTime: null,

    elementReadyRun: function (element, fun, args) {
        var _this = this;

        if (!element) {
            alert(" not element when execute elementReadyRun()");
            return;
        }

        if (element.readyState && (element.readyState == "complete" || element.readyState == "loaded")) {
            if (_this.eTime)
                clearInterval(_this.eTime);
            fun(args);
            return;
        }

        var t = 0;


        _this.eTime = setInterval(function () {
            if (element.readyState && (element.readyState == "complete" || element.readyState == "loaded")) {
                clearInterval(_this.eTime);
                fun(args);
                return;
            }

            t++;

            if (t > 5000) {
                alert("等待原始装载超时，tagName=" + element.tagName + ",Id=" + element.id + ",");
                clearInterval(_this.eTime);
            }

        }, 50);

    },


    windowDocumentReadyRun: function (win, fun, args) {
        var _this = this;
        if (!win) {
            alert(" not element when execute elementReadyRun()");
            return;
        }

        var document = null;

        try { document = win.document; } catch (e) { }

        if (document && document.readyState && (document.readyState == "complete" || document.readyState == "loaded")) {
            fun(args);
            return;
        }


        var t = 0;

        var timer = setInterval(function () {
            t++;
            if (t > 200) {
                alert("等待文档装载超时");
                clearInterval(timer);
            }

            var document = null;

            try { document = win.document; } catch (e) { }

            if (!document) return;

            if (document.readyState && (document.readyState == "complete" || document.readyState == "loaded")) {
                clearInterval(timer);
                fun(args);
                return;
            }



        }, 50);

    }

}


//浏览器类型
window.isIe = (navigator.userAgent.indexOf("MSIE") > 0);
window.isOpra = (navigator.userAgent.indexOf("Opera") > -1);

//数据类型扩展函数
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

/***
*   文档对象工具 
*   静态类
***/
DomUtils = {

    drillChildByAttr: function (node, attrName, attrValue) {
        for (var i = 0; i < node.childNodes.length; i++) {
            var subNode = node.childNodes[i];
            if (subNode.attributes && subNode.attributes[attrName] && subNode.attributes[attrName].value == attrValue)
                return subNode
            else {
                var subSubNode = this.drillChildByAttr(subNode, attrName, attrValue);
                if (subSubNode) return subSubNode;
            }
        }
        return null;
    },

    drillUpByAttrLike: function (node, attrName, attrValue) {
        var p = node.parentNode;

        if (!p) return null;

        if (p.attributes && p.attributes[attrName] && (p.attributes[attrName].value.lastIndexOf(attrValue, 200) > -1)) {
            return p;
        } else {
            return this.drillUpByAttrLike(p, attrName, attrValue);
        }
    },

    drillUpByAttr: function (node, attrName, attrValue) {
        var p = node.parentNode;

        if (!p) return null;

        if (p.attributes && p.attributes[attrName] && (p.attributes[attrName].value == attrValue)) {
            return p;
        } else {
            return this.drillUpByAttr(p, attrName, attrValue);
        }
    },

    nodeHasChild: function (node, childNode) {
        for (var i = 0; i < node.childNodes.length; i++) {
            var subNode = node.childNodes[i];
            if (subNode == childNode)
                return true;
            else {
                if (this.nodeHasChild(subNode, childNode))
                    return true;
            }
        }
        return false;
    },

    forEachChild: function (node, fn) {
        var stop = false;
        for (var i = 0; i < node.childNodes.length; i++) {
            var subNode = node.childNodes[i];
            if (fn(subNode)) {
                stop = true;
                return stop;
            };
            stop = this.forEachChild(subNode, fn);
            if (stop) return stop;
        }
        return stop;
    },

    setElementAttr: function (element, attrName, attrValue) {
        //      var element = node;
        if (element.nodeName == "#text") return;
        if (!element) alert("not selected element");

        if (typeof (element[attrName]) == "string") {
            element[attrName] = attrValue;
            return;
        }

        if (attrName == "style") {
            element[attrName]["cssText"] = attrValue;
            return;
        }

        if (element.getAttribute(attrName)) {
            element.setAttribute(attrName, attrValue);
        } else {
            var attr = document.createAttribute(attrName);
            attr.value = attrValue;
            element.attributes.setNamedItem(attr);
        }
    },

    getElementAttr: function (element, attrName) {


        if (element.nodeName == "#text")
            return null;

        if (attrName == "style") {
            return element[attrName]["cssText"];
        }

        if (element[attrName] && typeof (element[attrName]) != "object")
            return element[attrName];

        var attr = element.getAttribute(attrName);
        if (attr) {
            return attr.value;
        }
        return null;
    },

    optionsHaveValue: function (options, value) {

        for (var i = 0; i < options.length; i++) {
            if (options[i].value == value)
                return true;
        }
        return false;

    },

    setElementValue: function (element, value, isAutoCreate) {
        if (value == null) {
            value = "";
        }

        if (element == undefined) return;
        if (element == null) return;

        var elementID = "";


        if (typeof (element) == "string") {
            elementID = element;
            element = document.getElementById(elementID);
        }


        //动态创建隐藏域
        if (element == null) {
            if (!isAutoCreate) return;
            element = document.createElement("INPUT");
            element.style.display = "none";
            element.id = elementID;
            element.onclick = function () { alert(element.id); }
            document.body.appendChild(element);
        }

        switch (element.nodeName) {
            case "INPUT":
                if (!element.attributes["type"] || element.attributes["type"] == undefined)
                    element.attributes["type"] = "text";
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
            case "LABEL":
                element.innerText = value;
                break;
            case "SELECT":
                if (!this.optionsHaveValue(element.options, value)) {
                    element.options.add(new Option(value, value));
                }
                element.value = value;
                break;
            case "TD":
                element.innerText = value;
                break;
            case "SPAN":
                element.innerText = value;
                break;
            case "IMG":
                element.src = value;
                break;
            case "#text":
                element.nodeValue = value;
                break
            default:
                element.innerText = value;
                break;
        }
        return element;
    },


    getElementValue: function (element) {
        var value = "";

        if (typeof (elementID) == "string") {
            element = document.getElementById(elementID);
            if (elementID.indexOf("$") > 0 && element == null) {
                var listId = elementID.substr(elementID.indexOf("$") + 1);
                var selectedValue = this.gatherElement(listId + "_Selected");
                if (selectedValue != "") {
                    var index = selectedValue.substring(selectedValue.indexOf("[") + 1, selectedValue.indexOf("]"));
                    element = document.getElementById(elementID.substr(0, elementID.indexOf("$")) + "[" + index + "]");
                }
            }
        }

        if (element == null) return "";
        switch (element.nodeName) {
            case "INPUT":
                switch (element.attributes["type"].value) {
                    case "checkbox":
                        value = element.checked;
                        //                        if (element.checked == "true" || element.checked != "") {
                        //                            value = "1";
                        //                        }
                        //                        else {
                        //                            value = "0";
                        //                        }
                        break;
                    case "radio":
                        var elementRadio = document.getElementsByName(element.name);
                        for (var i = 0; i < elementRadio.length; i++) {
                            if (elementRadio[i].checked == "true" || elementRadio[i].checked != "") {
                                value = elementRadio[i].value;
                            }
                        }
                        value = "";
                        break;
                    default:
                        value = element.value;
                        break;
                }
                break;
            case "LABEL":
                value = element.innerText;
                break;
            case "SELECT":
                value = element.value;
                break;
            case "IFRAME":
                value = element.src;
                break;
            case "IMG":
                value = element.src;
                break;
            default:
                value = element.innerText;
                break;
        }
        return value;
        //        var id = "";
        //        if (element.id.indexOf("[") >= 0) {
        //            id = element.id.substring(0, element.id.indexOf("["));
        //        }
        //        else id = element.id;

        //        if (Page[id] != null || Page[id] != undefined) {
        //            value = DomUtil.getHashValue(value, Page[id]);
        //        }
        //        return value;
    },
    elementReplace: function (str) {
        if (str != null) {
            var newStr = str;
            var strArray = str.split('');
            var i = 0;
            var isDataListElement = false;

            var tempElementID = "";
            var tempListId = "";

            for (i = 0; i < strArray.length; i++) {
                if (strArray[i] == "@" || strArray[i] == "$") {
                    switch (strArray[i]) {
                        case "@":
                            isDataListElement = false;
                            tempElementID = "";
                            for (var j = i + 1; j < strArray.length; j++) {
                                if (strArray[j] == ")" || strArray[j] == "," || strArray[j] == " " || strArray[j] == "'" || strArray[j] == "." || strArray[j] == "&" || strArray[j] == "$") {
                                    i = j - 1;
                                    break;
                                }
                                else {
                                    tempElementID += strArray[j];
                                }
                            }
                            if (strArray[j] != "$") {
                                var value = this.gatherElement(tempElementID);
                                try { eval(value) } catch (Error) {
                                    value = "'" + value + "'";
                                }
                                return this.elementReplace(newStr.replace("@" + tempElementID, value));
                            }
                            break;

                        case "$":
                            isDataListElement = true;
                            tempListId = "";
                            for (var k = i + 1; k < strArray.length; k++) {
                                if (strArray[k] == ")" || strArray[k] == "," || strArray[k] == " " || strArray[k] == "'" || strArray[k] == "." || strArray[k] == "&") {
                                    i = k;
                                    break;
                                }
                                else {
                                    tempListId += strArray[k];

                                }
                            }
                            if (tempListId != "" && tempElementID != "") {
                                var selected = this.gatherElement(tempListId + "_Selected");
                                var index = selected.substring(selected.indexOf("[") + 1, selected.indexOf("]"));
                                var elementId = tempElementID + "[" + index + "]";
                                var value = this.gatherElement(elementId);
                                try { eval(value) } catch (Error) {
                                    value = "'" + value + "'";
                                }
                                var curStr = "@" + tempElementID + "$" + tempListId;

                                return this.elementReplace(newStr.replace(curStr, value));
                            }
                            break;
                    }
                }
            }
            return newStr;
        }
        else {
            return null;
        }
    },
    hideElement: function (element) {
        if (typeof (element) == "string")
            element = document.getElementById(element);
        element.style.display = "none";
        return element;
    },
    appearElement: function (element) {
        if (typeof (element) == "string")
            element = document.getElementById(element);
        element.style.display = "block";
        return element;
    },
    getElementById: function (id) {
        if (!id) return null;
        if (id.indexOf("$") > 0)//表示Table中的元素
        {
            var tableId = id.substr(id.indexOf("$") + 1);
            var selected = this.gatherElement(tableId + "_Selected");
            var index = selected.substring(selected.indexOf("[") + 1, selected.indexOf("]"));
            var elementId = id.replace("$" + tableId, "").replace("@", "");
            return document.getElementById(elementId + "[" + index + "]");
        }
        else {
            return document.getElementById(id.replace("@", ""));
        }
    },
    getHashValue: function (key, hashObject) {
        for (var p in hashObject) {
            if (hashObject[p] == key) {
                return p;
            }
        }
        return key;
    },

    getUrlRequest: function () {
        var url = location.search; //获取url中"?"符后的字串
        var theRequest = new Object();

        if (url.indexOf("?") != -1) {
            var str = url.substr(1);
            strs = str.split("&");
            for (var i = 0; i < strs.length; i++) {
                theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
            }
        }
        return theRequest;
    },


    cloneNode: function (node, deep) {
        if (deep == undefined) deep = true;

        if (isIe)
            return node.cloneNode(deep);

        var ret = document.createElement(node.nodeName);
        for (var i = 0; i < node.attributes.length; i++) {
            var attr = node.attributes[i];
            ret.setAttribute(attr.name, attr.value);
        }
        ret.className = node.className;
        ret.innerHTML = node.innerHTML;
        return ret;
    },

    getElementsByName: function (upElemnt, name, useBuffer) {


        if (!window.ElementBuffer)
            window.ElementBuffer = new Object();

        if (window.ElementBuffer[name] && useBuffer)
            return window.ElementBuffer[name]

        function drillElementsByAttr(elements, node, attrName, attrValue) {
            for (var i = 0; i < node.childNodes.length; i++) {
                var subNode = node.childNodes[i];
                if (subNode.attributes && subNode.attributes[attrName] && subNode.attributes[attrName].value == attrValue) {
                    elements.push(subNode);
                }
                else {
                    drillElementsByAttr(elements, subNode, attrName, attrValue);
                }
            }
            return null;
        }

        //        return document.getElementsByName(name);
        var ret = new Array();

        if (window.isIe) {
            //            return document.getElementsByName(name);
            if (!upElemnt) return null;
            drillElementsByAttr(ret, upElemnt, "name", name);
        } else
            ret = document.getElementsByName(name);

        window.ElementBuffer[name] = ret;
        return ret;


    },
    clearChild: function (node) {
        while (node.firstChild) {
            node.removeChild(node.firstChild);
        }
    }
};

//判断是否为数组
isArray = function (source) {
    return '[object Array]' == Object.prototype.toString.call(source);
};
//判断是否为日期对象
isDate = function (o) {
    // return o instanceof Date;
    return {}.toString.call(o) === "[object Date]" && o.toString() !== 'Invalid Date' && !isNaN(o);
};
//判断是否为Element对象
isElement = function (source) {
    return !!(source && source.nodeName && source.nodeType == 1);
};
//判断目标参数是否为function或Function实例
isFunction = function (source) {
    // chrome下,'function' == typeof /a/ 为true.
    return '[object Function]' == Object.prototype.toString.call(source);
};
//判断目标参数是否number类型或Number对象
isNumber = function (source) {
    return '[object Number]' == Object.prototype.toString.call(source) && isFinite(source);
};
//判断目标参数是否为Object对象
isObject = function (source) {
    return 'function' == typeof source || !!(source && 'object' == typeof source);
};
//判断目标参数是否string类型或String对象
isString = function (source) {
    return '[object String]' == Object.prototype.toString.call(source);
};
//判断目标参数是否Boolean对象
isBoolean = function (o) {
    return typeof o === 'boolean';
};

