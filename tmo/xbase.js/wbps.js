if (!window.parent || !window.parent.xdesignMode) {


    document.write("<div align='left' id=~xBaseLoadingWaitDiv~ style='BACKGROUND: #ffffff; FILTER: Alpha(opacity=85); LEFT: 0px; PADDING-BOTTOM: 5px; WIDTH: 2150px; POSITION: absolute; TOP: 0px; HEIGHT: 2150px; display: block'>正在装载系统，请稍后...</div>");

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
        initRequest: function () {
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
        request: function (url, fun, postData) {
            var ajxObj = this.initRequest();

            ajxObj.onreadystatechange = function () {
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
                        alert("服务器错误：" + ajxObj.status + "\r\n" + ajxObj.responseText);
                        document.documentElement.outerHTML = ajxObj.responseText;
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
        SynRequest: function (url, postData) {
            var ajxObj = this.initRequest();
            var ajaxRet = null;
            var hasRet = false;

            ajxObj.onreadystatechange = function () {
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
                        alert("服务器错误：" + ajxObj.status + "\r\n" + ajxObj.responseText);
                        document.open();
                        document.clear();
                        document.write(ajxObj.responseText);
                        document.close();
                        //                    document.documentElement.outerHTML = ajxObj.responseText;

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

        obj2str: function (o) {
            var r = [];
            if (typeof o == "string") return "\"" + o.replace(/([\'\"\\])/g, "\\$1").replace(/(\n)/g, "\\n").replace(/(\r)/g, "\\r").replace(/(\t)/g, "\\t") + "\"";
            if (typeof o == "undefined") return "";
            if (typeof o == "object") {
                if (o === null) return "null";
                else if (o.length == undefined || o.length == null) {
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
        },

        // 未整理，不能使用
        submitForm: function () {
            //获取用户输入  
            var title = document.forms[0].title.value;
            var author = document.forms[0].author.value;
            var content = document.forms[0].content.value;
            //创建XMLHttpRequest对象  
            var xmlhttp;
            try {
                xmlhttp = new XMLHttpRequest();
            } catch (e) {
                xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
            }
            //创建请求结果处理程序  
            xmlhttp.onreadystatechange = function () {
                if (4 == xmlhttp.readyState) {
                    if (200 == xmlhttp.status) {
                        var date = xmlhttp.responseText;
                        addToList(date);
                    } else {
                        alert("error");
                    }
                }
            }
            //打开连接，true表示异步提交  
            xmlhttp.open("post", "ajaxAdd.asp", true);
            //当方法为post时需要如下设置http头  
            xmlhttp.setRequestHeader('Content-type', 'application/x-www-form-urlencoded');
            //发送数据  
            xmlhttp.send("title=" + escape(title) + "&author=" + escape(author) + "&content=" + escape(content));
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
    function UrlParamRequest(strName) {
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
            if ((typeof element).toLowerCase() == "string") return;
            if (element.nodeName == "#text") return;
            if (!element) alert("not selected element");

            if (element[attrName] && typeof (element[attrName]) != "object")
                element[attrName] = attrValue;
            if (element.getAttribute(attrName)) {
                if (attrName == "style") {
                    if (typeof (attrValue) == "string")
                        element["style"].cssText = attrValue;
                    else
                        element["style"] = attrValue;
                } else
                    element.setAttribute(attrName, attrValue);

            } else {
                var attr = document.createAttribute(attrName);
                attr.value = attrValue;
                element.attributes.setNamedItem(attr);
            }
        },

        getElementAttr: function (element, attrName) {

            if (element.nodeName == "#text") return null;

            if (element[attrName] && typeof (element[attrName]) != "object")
                return element[attrName];

            if (element[attrName] && typeof (element[attrName]) == "object" && attrName == "style")
                return element[attrName].cssText;

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



    var ReponeseStatus = {
        lockEvent: false
    }


    /***   文档元素代理类(ElementBrk)  1.0
    *    1. 根据elementId 查找元素，及元素的属性    
    *    2. 将元素值赋到元素，及对应的属性
    *   
    *    @ena
    全称：Element Name Attribuite
    *      类型：字符
    *      描述：元素名和元素属性
    *      格式：[elementName]_attr_[elementAttribute]   
    ***/
    var BindTypes = {
        SEQ: "SEQ_",
        REP: "REP_"
    }
    //设置元素捆绑值到文档元素元素
    function setBindIdValue(bindId, Values) {

    }
    //从文档中取出元素的捆绑值
    function getBindIdValues(bindId) {

    }


    function ElementBrk(ena) {


        var _this = this;
        this.loaded = false;


        var elementName = ena;

        var isRepeat = false;
        if (elementName.indexOf(BindTypes.REP, 0) == 0) {
            elementName = elementName.substring(BindTypes.REP.length, elementName.length);
            isRepeat = true;
        }

        var bindType = 0;
        var elements = [];

        var bind = WbdlUtils.decodeDataElementId(elementName);
        elementName = bind.name;

        elements = DomUtils.getElementsByName(document.body, elementName, true);
        if (!elements || !elements.length || elements.length == 0) {
            alert("在页面上不能找到name为 " + elementName + " 的元素，请检查页面，或删除该元素的配置");
            return null;
        }
        var elementAttr = bind.attr;

        if (!elementAttr)
            elementAttr = getDefaultAttr();

        var _attrNames = elementAttr.split(".");
        var _lastAttrName = _attrNames[_attrNames.length - 1];
        this.loaded = true;

        function getDefaultAttr() {
            var tagName = elements[0].tagName;
            var el = elements[0];

            switch (tagName) {
                case "INPUT":
                    {
                        switch (el.type) {
                            case "checkbox":
                                {
                                    return "checked";
                                }
                            default:
                                {
                                    return "value";
                                }
                        }
                        break;
                    }
                case "IMG":
                    {
                        return "src";
                        break;
                    }
                case "IFRAME":
                    {
                        return "src";
                        break;
                    }
                case "SELECT":
                    {
                        return "value";
                        break;
                    }
                default:
                    {
                        return "innerHTML";
                        break;
                    }
            }

        }

        function isFunctionAttr(attrName) {
            var eventNames = " onclick ondblclick  onmousedown  onmouseup onmouseover onmouseout onmousemove onchange onkeydown onkeyup onkeypress";
            return eventNames.indexOf(attrName, 0) >= 0;
        }


        function setOptions(element, values) {
            var v = element.value;
            element.options.length = 0;
            if (v) {
                var op = new Option(v, v);
                element.add(op);
            }
            for (var j = 0; j < values.length; j++) {
                if (values[j] != v) {
                    var op = new Option(values[j], values[j]);
                    element.add(op);
                }
            }
        }

        function getOptions(element) {
            var ret = [];
            for (var j = 0; j < element.options.length; j++) {
                var op = element.options[j];
                ret.push(op.value);
            }
            return ret;
        }

        function getElementAttrObj(element) {
            var atrObj = element;
            for (var j = 0; j < _attrNames.length - 1; j++) {
                var att = _attrNames[j];
                atrObj = atrObj[att];
            }
            return atrObj;
        }

        this.getValue = function () {
            var ret = [];

            if (elements.length < 1) return null;

            if (isFunctionAttr(_lastAttrName)) {
                return null;
            }
            for (var i = 0; i < elements.length; i++) {

                var el = elements[i];
                var atrObj = getElementAttrObj(el);
                var elValue = atrObj[_lastAttrName];

                if (_lastAttrName == "options") {
                    return getOptions(atrObj);
                }
                else if (atrObj.tagName && atrObj.tagName.toUpperCase() == "IFRAME" && (_lastAttrName == "innerHTML" || _lastAttrName == "outerHTML")) {
                    var frameDoc = null;
                    try {
                        frameDoc = atrObj.contentWindow.document;
                    } catch (e) {
                        try {
                            frameDoc = atrObj.contentDocument;
                        } catch (e) {
                        }
                    }
                    if (frameDoc == null) {
                        alert("文档访问失败！文章内容无法保存");
                        break;
                    }

                    try {
                        frameDoc.body.contentEditable = "false";
                        //                        elemnt.contentDocument.body.contentEditable = undefined;
                        frameDoc.body.removeAttribute('contentEditable');
                    } catch (e) {
                    }

                    elValue = frameDoc.documentElement.outerHTML;
                }
                else if (_lastAttrName.indexOf("checked") >= 0) {
                    elValue = elValue == true ? "1" : (elValue == 1 ? "1" : "0");
                }

                if (el.BindData)
                    ret.push(elValue);

            }

            return ret;
        }


        this.setValue = function (values) {

            if (elements.length < 1) return;
            var k = 0; //value index

            for (var i = 0; i < elements.length; i++) {

                var el = elements[i];
                var atrObj = getElementAttrObj(el);
                var elValue = atrObj[_lastAttrName];
                //  if (el.id == undefined || el.id == null || el.id == "")
                //      el.id = el.uniqueID;
                el.BindData = true;

                //            var elId = el.id;


                //非重复显示  
                var value = "norecord";
                if (k < values.length && !isRepeat) {
                    value = values[k];
                    if (el.tagName && el.tagName.toLowerCase() == "td")
                        el.parentNode.style.display = "";
                    else
                        el.style.display = "";
                    //  continue;
                }
                else if (isRepeat && (values.length > 0)) {
                    //                k = 0;
                    value = values[0];
                } else if (!isRepeat && _lastAttrName != "options") {
                    if (el.tagName && el.tagName.toLowerCase() == "td")
                        el.parentNode.style.display = "none";
                    else
                        el.style.display = "none";

                    el.BindData = false;
                    continue;
                }

                k++;

                //            var elValue = "";
                //            elementReadyRun(el, function() {


                if (elValue && (typeof elValue) == "string" && bind.param != null && bind.param != "")
                    elValue = elValue.replace("[" + bind.param + "]", value);
                else
                    elValue = value;

                if (_lastAttrName == "options") {
                    setOptions(atrObj, values);
                }
                else if (_lastAttrName.indexOf("backgroundImage") >= 0)
                    elValue = "url(" + elValue + ")";
                else if (isFunctionAttr(_lastAttrName)) {
                    try {
                        atrObj[_lastAttrName] = new Function(elValue);
                    } catch (e) {
                        alert("元素绑定的函数有错。" + "元素:" + el.tagName + "，属性：" + lastAtrName + ",函数：" + elValue);
                        return;
                    }
                    el.style.cursor = "pointer";
                }
                else if (_lastAttrName.indexOf("checked") >= 0) {
                    var v = elValue;
                    if (v == null || v == undefined)
                        v = "0";
                    v = v.toLowerCase();
                    atrObj[_lastAttrName] = v == "true" ? true : (v == "1" ? true : false);
                }
                else if (atrObj.tagName == "SELECT" && _lastAttrName == "value") {
                    atrObj[_lastAttrName] = elValue;
                    if (!atrObj[_lastAttrName]) {
                        var op = new Option(elValue, elValue);
                        atrObj.options.add(op);
                        atrObj.value = elValue;
                    }
                    //                for (var k = 0; k < atrObj.options.length; k++) {
                    //                    var op = atrObj.options[k];
                    //                    if (op.value = elValue) {
                    //                        op.selected = true;
                    //                        break;
                    //                    }
                    //                }
                }
                else
                    atrObj[_lastAttrName] = elValue;
                //            eval("document.getElementById('" + el.id + "')." + attr + "='" + elValue + "';");

                //            });


            }
        }
    }


    function ElementDatas(jsonData) {
        this.jsonData = jsonData;
    }

    ElementDatas.prototype.bindData = function () {

        for (var ena in this.jsonData) {
            var elementBrk = new ElementBrk(ena);
            if (elementBrk.loaded)
                elementBrk.setValue(this.jsonData[ena]);
        }
    }

    function EventsBrk(events) {

        function mAttachEvent_(el, elIndex, wbapEvent) {

            if (wbapEvent.EventName.toLowerCase() == "onclick")
                el.style.cursor = "pointer";


            if (el["_tachedEvent_" + wbapEvent.EventName])
                return;
            //            el.detachEvent(wbapEvent.EventName, el["_tachedEvent_" + wbapEvent.EventName]);
            el["_tachedEvent_" + wbapEvent.EventName] = function (evt) {

                if (ReponeseStatus.lockEvent)
                    return;

                evt = evt ? evt : window.event;
                if (evt.propertyName != "value" && evt.type == "propertychange")
                    return;


                ReponeseStatus.lockEvent = true;

                var reqData = wbapEvent.EventRequest;
                reqData.Sender.ElementName = wbapEvent.ElementName;
                reqData.Sender.Index = elIndex;

                if (el["Value"] != undefined)
                    reqData.Sender.Value = el["Value"];
                else if (el["value"] != undefined)
                    reqData.Sender.Value = el["value"];


                if (el["RowId"])
                    reqData.Sender.RowId = el["RowId"];

                ReponeseStatus.lockEvent = false;

                var req = new Request(reqData);
                var resp = req.commit();
                if (resp) {
                    var respBrk = new Response(resp);
                    //    respBrk.bindData();
                }
            };

            if (window.isIe) {
                el.attachEvent(wbapEvent.EventName, el["_tachedEvent_" + wbapEvent.EventName]);
            } else {
                var evName = wbapEvent.EventName;
                if (evName.substr(0, 2) == "on")
                    evName = evName.substr(2);
                if (evName == "propertychange")
                    evName = "input";
                el.addEventListener(evName, el["_tachedEvent_" + wbapEvent.EventName]);
            }

        }

        function mAttachEvent(el, elIndex, wbapEvent) {

            if (wbapEvent.EventName.toLowerCase() == "onclick")
                el.style.cursor = "pointer";


            //        if (el["_tachedEvent_" + wbapEvent.EventName])
            //            return;
            //            el.detachEvent(wbapEvent.EventName, el["_tachedEvent_" + wbapEvent.EventName]);
            var evName = wbapEvent.EventName;

            if (evName == "onpropertychange" && !window.isIe)
                evName = "oninput";
            //        el.addEventListener(evName, el["_tachedEvent_" + wbapEvent.EventName]);

            el[evName] = function (evt) {

                if (ReponeseStatus.lockEvent)
                    return;

                evt = evt ? evt : window.event;
                if (evt.propertyName != "value" && evt.type == "propertychange")
                    return;
                ReponeseStatus.lockEvent = true;


                var reqData = wbapEvent.EventRequest;
                reqData.Sender.ElementName = wbapEvent.ElementName;
                reqData.Sender.Index = elIndex;

                if (el["Value"] != undefined)
                    reqData.Sender.Value = el["Value"];
                else if (el["value"] != undefined)
                    reqData.Sender.Value = el["value"];

                if (el["RowId"])
                    reqData.Sender.RowId = el["RowId"];

                ReponeseStatus.lockEvent = false;

                var req = new Request(reqData);
                var resp = req.commit();
                if (resp) {
                    var respBrk = new Response(resp);
                    //    respBrk.bindData();
                }
            };

            //   el.attachEvent(wbapEvent.EventName, el["_tachedEvent_" + wbapEvent.EventName]);
        }

        this.bindData = function () {

            if (!events) return;

            for (var i = 0; i < events.length; i++) {
                var wbpsEvent = events[i];
                var elementId = wbpsEvent.ElementName;

                if (!wbpsEvent.EventName)
                    continue;

                if (!wbpsEvent.ActionFlow)
                    continue;


                var elements = DomUtils.getElementsByName(document.body, elementId);
                if (!elements || events.length <= 0) {
                    alert("服务器指定了元素 Name 为" + elementId + "上的事件，但是页面上没有发现这个元素，请检查页面元素");
                    continue;
                }

                for (var j = 0; j < elements.length; j++) {
                    var el = elements[j];
                    mAttachEvent(el, j, wbpsEvent);
                }

            }
        }
    }


    function Response(responseData) {
        var _this = this;

        this.respData = responseData;

        this.elementDatas = new ElementDatas(responseData.ElementDatas);
        this.eventsBrk = new EventsBrk(responseData.Events);

        function bindData() {
            ReponeseStatus.lockEvent = true;
            _this.elementDatas.bindData();
            _this.eventsBrk.bindData();
            ReponeseStatus.lockEvent = false;

            var respData = responseData;

            var jsRet;
            if (respData.ClientScript) {
                try {
                    var _runClientScript = new Function(respData.ClientScript);
                    jsRet = _runClientScript();
                } catch (e) {
                    alert("服务返回脚本有误，不能被执行，错误提示，" + e.message);
                }
            }

            if (respData.BackRequest) {
                var reqData = respData.BackRequest;
                reqData.JsRet = jsRet;
                var req = new Request(reqData);
                var resp = req.commit();
                if (resp) {
                    var respBrk = new Response(resp);
                    //  respBrk.bindData();
                }
            }
        }
        bindData();
        //    this.bindData();
    }


    //Response.prototype.bindData = function() {
    // //   debugger;
    //}




    Response.prototype.setObject = function (obj) {
        //   this.superMethod("setObject", obj);

        var dataBinds = new DataBinds(obj.ElementBinds);
        dataBinds.GetBindData();

        Page.State = Page.ST_LOADING;

        for (var option in dataBinds.GetOptions()) {
            if (dataBinds.GetOptions().hasOwnProperty(option)) {
                var optionBind = new XOption(option, dataBinds.GetOptions()[option]);
                optionBind.Bind();
            }
        }

        for (var item in dataBinds.GetItems()) {
            if (dataBinds.GetItems().hasOwnProperty(item)) {
                var itemBind = new XItem(item, dataBinds.GetItems()[item]);
                itemBind.Bind();
            }
        }



        for (var event in dataBinds.GetEvents()) {
            if (dataBinds.GetEvents().hasOwnProperty(event)) {
                var eventBind = new XEvent(event, dataBinds.GetEvents()[event]);
                eventBind.Bind();
            }
        }

        var lists = dataBinds.GetLists();

        for (var listId in lists) {
            var xList = Page[listId];
            if (!xList || xList == undefined) {
                var xList = new XList(listId, lists[listId]);
                Page[listId] = xList;
            }
            xList.setData(lists[listId]);
        }

        var lookups = dataBinds.GetLookups();
        for (var lookupId in lookups) {
            if (lookups.hasOwnProperty(lookupId)) {
                var xLookUp = new XLookUp(lookupId, lookups[lookupId]);
                xLookUp.Bind();
                Page[lookupId] = xLookUp;
                Page[lookupId]["JsonData"] = lookups[lookupId];
            }
        }

        var references = dataBinds.GetReferences();
        for (var referenceId in references) {
            if (references.hasOwnProperty(referenceId)) {
                var xReference = new XReference(referenceId, references[referenceId]);
                xReference.Bind();
                Page[referenceId] = xReference;
                Page[referenceId]["JsonData"] = references[referenceId];
            }
        }

        Page["TempData"] = new Object();
        var tempdatas = dataBinds.GetTempDatas();
        for (var tempdataId in tempdatas) {
            Page["TempData"][tempdataId] = tempdatas[tempdataId];
        }


        for (var controlElementId in obj.Controls) {
            var control = obj.Controls[controlElementId];
            var venderName = control.VenderObject;
            var vender = null;
            var venderClass = eval(venderName);
            vender = new venderClass(controlElementId);
            vender.setData(control.Data);
            Page[controlElementId] = vender;
        }


        var action = new Action();
        var requestData = action.GetAction(obj.Action); //onload事件
        return requestData;


        document.onkeydown = function () {
            if (window.event.keyCode == 13 && window.event.srcElement.type != "button") {
                window.event.keyCode = 9;
            }
        }
    }




    RequestDef = {
        Types: {
            List: "List",
            Option: "Option",
            Event: "Event"
        }
    }


    function Request(reqData) {

        this.reqData = reqData;


        if (!this.reqData || this.reqData == undefined) {
            this.reqData = new Object;
            this.reqData.ElementDatas = new Object();
        }

        this.reqData.PageId = document.location.pathname + document.location.search;

        if (this.reqData.ElementDatas) {
            for (elName in this.reqData.ElementDatas) {
                this.reqData.ElementDatas[elName] = [];
                var brk = new ElementBrk(elName);
                if (brk.loaded)
                    this.reqData.ElementDatas[elName] = brk.getValue();
            }
        }

    }

    Request.prototype.commit = function () {

        var url = "wbps.axd";

        var requestStr = XAjax.obj2str(this.reqData);
        var strResponse = XAjax.SynRequest(url, requestStr);

        if (strResponse == null) return null;

        var response = eval("(" + strResponse + ")");

        if (response.Err) {
            var err = response.Err;
            if (Page.onNoPermission) {
                Page.onNoPermission(response.Err);
                return null;
            }
            alert(err.ErrText);
            if (err.ErrUrl)
                window.location = err.ErrUrl;
            return null;
        }

        return response;
    }





    var xBase$WaitHint = document.getElementById("~xBaseLoadingWaitDiv~");

    window.onresize = function () {
        if (xBase$WaitHint) {
            xBase$WaitHint.style.width = document.body.clientWidth + "px";
            xBase$WaitHint.style.height = document.body.clientHeight + "px";
        }
    }

    var Page = {
        loaded: false,
        onLoaded: function () {
            //    waitHint = document.getElementById("divWait");
            //   debugger;
            if (xBase$WaitHint)//
                xBase$WaitHint.style.display = "none";
            //document.removeChild(Page.waitHint);
        },
        onDocumentReady: function () {
        },
        onbodyLoaded: function () {
        }
    }


    var wbap_loaded = false;


    String.prototype.endWith = function (str) {
        return this.lastIndexOf(str, this.length - str.length) > -1;
    };

    WbsRuntime = function () {
        var LIB_PATH = "/xbase.js/";
        var files = [];

        var _this = this;


        this.Import = function (packageName) {
            files.push(LIB_PATH + packageName.replace(".", "/") + ".js");
        }


        this.Ready = function (fn, agrs) {

            //  var loadCount = files.length;
            //   var loadedCount = 0;
            var timeout = 2000;
            var t = 0;
            var loadedFiles = new Object();
            var isFireReadEvent = false;
            var isFireBodyLoaded = false;


            function setOnloadEvent(obj, index, scriptId) {
                obj.onload = function () {
                    if (!loadedFiles[scriptId] || loadedFiles[scriptId] == undefined)
                        loadedFiles[scriptId] = obj;
                }
            }


            for (var i = 0; i < files.length; i++) {
                var scritpId = "xBaseRutimeScript_" + i;
                scriptOb = document.createElement("script");
                scriptOb.id = scritpId;

                setOnloadEvent(scriptOb, i, scritpId);

                scriptOb.type = "text/javascript";

                scriptOb.src = files[i];
                scriptOb.async = false;
                document.getElementsByTagName("head")[0].appendChild(scriptOb);
            }



            var timer = setInterval(function () {

                window.focus();
                //装载超时
                t++;
                if (t > timeout) {
                    clearInterval(timer);
                    alert("系统长时间不能确定脚本程序是否装载，运行可能出现问题,或浏览器不兼容！");
                    return;
                }

                if (document.body) {
                    if (!isFireBodyLoaded && Page && Page.onbodyLoaded) {
                        // debugger;
                        Page.onbodyLoaded();

                        //       alert("body loaded");
                    }
                    isFireBodyLoaded = true;
                }

                //如果文档装载没有完成，等待文档装载
                if (!document.readyState || (document.readyState != "loaded" && document.readyState != "complete"))
                    return;

                if (!isFireReadEvent && Page && Page.onDocumentReady) {
                    Page.onDocumentReady();
                }

                isDocumentReady = true;

                //如过文档在设计模式则取消装载，并且不执行CallBack函数
                var design = window.parent.document.designMode.toLowerCase();
                if (design == "on" || design == "yes" || design == "1") {
                    clearInterval(timer);
                    //  alert("设计模式中");
                    return;
                }

                for (var i = files.length - 1; i >= 0; i--) {
                    var sId = "xBaseRutimeScript_" + i;

                    var scriptOb = document.getElementById(sId);
                    if (scriptOb) {
                        if (scriptOb.readyState && (scriptOb.readyState == 'loaded' || scriptOb.readyState == 'complete')) {
                            if (!loadedFiles[sId] || loadedFiles[sId] == undefined)
                                loadedFiles[sId] = scriptOb;
                        }
                    }

                    if (loadedFiles[sId] == scriptOb)
                        files.splice(i, 1);
                }

                if (files.length < 1 && document.readyState && (document.readyState == "loaded" || document.readyState == "complete")) {
                    clearInterval(timer);
                    if (fn) fn(agrs);

                }


            }, 1);

        }


    }




    WBDL_CNST = {
        DataElementIdParamMask: "_param_",
        DataElementIdAttrMask: "_attr_"
    }


    WbdlUtils = {
        decodeDataElementId: function (dataElementId) {

            var ret = new Object();


            var i = dataElementId.indexOf(WBDL_CNST.DataElementIdParamMask);

            ret.name = dataElementId;
            ret.param = "";
            ret.attr = "";

            if (i > -1) {
                ret.name = dataElementId.substring(0, i);
                ret.param = dataElementId.substring(i + WBDL_CNST.DataElementIdParamMask.length, dataElementId.length);
            }

            var s = ret.name;
            var j = s.indexOf(WBDL_CNST.DataElementIdAttrMask);

            if (j > -1) {
                ret.name = s.substring(0, j);
                ret.attr = s.substring(j + WBDL_CNST.DataElementIdAttrMask.length, s.length);
                ret.attr = ret.attr.replace(WBDL_CNST.DataElementIdAttrMask, ".");
            };

            return ret;
        },
        encodeDataElementId: function (name, attr, param) {
            var s = name;

            if (attr != undefined && attr != null && attr != "")
                s += WBDL_CNST.DataElementIdAttrMask + attr.replace(".", WBDL_CNST.DataElementIdAttrMask);

            if (param != undefined && param != null && param != "")
                s += WBDL_CNST.DataElementIdParamMask + param;

            return s;
        },
        getDataElementDispName: function (dataElementId) {
            var dId = this.decodeDataElementId(dataElementId);

            var s = dId.name;

            if (dId.attr != undefined && dId.attr != null && dId.attr != "")
                s += "." + dId.attr;

            if (dId.param != undefined && dId.param != null && dId.param != "")
                s += "." + dId.param;

            return s
        }

    }


    var timeout = 3000;
    var t = 0;
    var timer = setInterval(function () {

        window.focus();
        //装载超时
        t++;
        if (t >= timeout) {
            clearInterval(timer);
            alert("系统长时间不能确定脚本程序是否装载，运行可能出现问题,或浏览器不兼容！");
            return;
        }


        //如果文档装载没有完成，等待文档装载
        if (!document.readyState)
            return;


        //如过文档在设计模式则取消装载，并且不执行CallBack函数
        var design = window.parent.document.designMode.toLowerCase();
        if (design == "on" || design == "yes" || design == "1") {
            clearInterval(timer);
            //  alert("设计模式中");
            return;
        }

        if (document.readyState == "loaded" || document.readyState == "complete") {
            clearInterval(timer);

            Page.wbap = new WBAP();
            //   xBase$WaitHint.style.display = "none";
            //   xBase$WaitHint.style.width = 0 + "px";
            //   xBase$WaitHint.style.height = 0 + "px";
            var xBase$WaitHint = document.getElementById("~xBaseLoadingWaitDiv~");
            if (xBase$WaitHint) {
                document.body.removeChild(xBase$WaitHint);
            }
        }
    }, 1)



    //if (document.body.style.display == "none")
    //    document.body.style.display = "block";

    //if (Page.onLoaded) {
    //    Page.onLoaded();
    //}




    WbsDef = {
        Url: {
            HREF: "wbps.axd",     // "/xj-service/WbpsHttpPort.aspx", //服务器端接收对象的相对位置
            FORM_PARAM: "form",
            TIME_PARAM: "time"
        }
    }

    function WBAP() {

        var wbap = this;
        var response;

        this.initalize = function () {
            var request = new Request();
            var responseData = request.commit();

            if (responseData == null) {
                return false;
            }

            var response = new Response(responseData);
            //  response.bindData();
        }

        /**
        * 从json字符串获取wbap对象；
        * @respText wbap json字符串
        */
        function getRespObj(respText) {

            var wbap = null;

            try {
                wbap = eval("(" + respText + ")");
            } catch (err) {
                alert(respText);
                return null;
            }

            if (parseInt(wbap.ErrorNo) != 0) {
                var errUrl = wbap.OnErrUrl;
                if (errUrl && errUrl != undefined) {
                    window.location.href = errUrl;
                    return null;
                }
                alert(wbap.Message);
                return null;
            }

            return wbap;

        }

        this.receiveResponse = function (respText) {
            //    debugger;
            var resp = getRespObj(respText);

            if (!resp) return;

            response = new Response(resp);

            var requestData = response.object;

            if (requestData != null && requestData.ActionId != null && requestData.ActionId != "") {
                wbap.sendRequest(requestData);
            }
        }

        this.sendRequest = function (sendObj) {

            var url = WbsDef.Url.HREF + "?" + WbsDef.Url.TIME_PARAM + "=" + new Date() + "&" + WbsDef.Url.FORM_PARAM + "=" + sendObj["PageName"];

            XAjax.request(url, wbap.receiveResponse, obj2str(sendObj));

        }

        this.initalize();

    }

    wbap_loaded = true;
}