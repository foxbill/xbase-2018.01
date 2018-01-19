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
    /*
    if (elementName.indexOf(".", 0) > 0) {
    var es = elementName.split(".");
    elementName = es[0];
    if (!elementAttr && es.length > 0) {

            for (var i = 1; i < es.length; i++)
    elementAttr += es[i] + ".";

            elementAttr = elementAttr.substr(0, elementAttr.length - 1);
    }
    }
    */
    //    var markPos = ena.toLowerCase().indexOf(ATTR_MARK.toLowerCase(), 0);

    //    if (markPos > -1) {
    //        elementName = ena.substring(0, markPos);
    //        elementAttr = ena.substr(markPos + ATTR_MARK.length);
    //    }

    // elements = document.getElementsByName(elementName);

    //    drillElementsByAttr(elements, document.body, "name", elementName);

    //    function drillElementsByAttr(elements, node, attrName, attrValue) {
    //        for (var i = 0; i < node.childNodes.length; i++) {
    //            var subNode = node.childNodes[i];
    //            if (subNode.attributes && subNode.attributes[attrName] && subNode.attributes[attrName].value == attrValue) {
    //                elements.push(subNode);
    //            }
    //            else {
    //                drillElementsByAttr(elements, subNode, attrName, attrValue);
    //            }
    //        }
    //        return null;
    //    }

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
        for (var j = 0; j < values.length; j++) {
            var op = new Option(values[j], values[j]);
            element.add(op);
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

    this.getValue = function() {
        var ret = [];

        if (elements.length < 1) return null;


        for (var i = 0; i < elements.length; i++) {

            var el = elements[i];
            var atrObj = getElementAttrObj(el);
            var elValue = atrObj[_lastAttrName];

            if (_lastAttrName == "options") {
                return getOptions(atrObj);
            }
            else if (atrObj.tagName && atrObj.tagName.toUpperCase() == "IFRAME" && (_lastAttrName == "innerHTML" || lastAtrName == "outerHTML")) {
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


    this.setValue = function(values) {

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
                    //     var op = new Option(elValue, elValue);
                    //     atrObj.options.add(op);
                    //     atrObj.value = elValue;
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

ElementDatas.prototype.bindData = function() {

    for (var ena in this.jsonData) {
        var elementBrk = new ElementBrk(ena);
        if (elementBrk.loaded)
            elementBrk.setValue(this.jsonData[ena]);
    }
}
//18206751061
//352522391



function EventsBrk(events) {

    function mAttachEvent_(el, elIndex, wbapEvent) {

        if (wbapEvent.EventName.toLowerCase() == "onclick")
            el.style.cursor = "pointer";


        if (el["_tachedEvent_" + wbapEvent.EventName])
            return;
        //            el.detachEvent(wbapEvent.EventName, el["_tachedEvent_" + wbapEvent.EventName]);
        el["_tachedEvent_" + wbapEvent.EventName] = function(evt) {

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
                respBrk.bindData();
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

        el[evName] = function(evt) {

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
                respBrk.bindData();
            }
        };

        //   el.attachEvent(wbapEvent.EventName, el["_tachedEvent_" + wbapEvent.EventName]);
    }

    this.bindData = function() {

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

    this.respData = responseData;

    this.elementDatas = new ElementDatas(responseData.ElementDatas);


    this.eventsBrk = new EventsBrk(responseData.Events);
}


Response.prototype.bindData = function() {

    ReponeseStatus.lockEvent = true;
    this.elementDatas.bindData();
    this.eventsBrk.bindData();
    ReponeseStatus.lockEvent = false;

    var respData = this.respData;

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
            respBrk.bindData();
        }
    }
}




Response.prototype.setObject = function(obj) {
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


    document.onkeydown = function() {
        if (window.event.keyCode == 13 && window.event.srcElement.type != "button") {
            window.event.keyCode = 9;
        }
    }
}

