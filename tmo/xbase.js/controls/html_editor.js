//import(/sytem/system.js);
//import(/contorls/listview.js);


DomDict = {
    COMM: {
        title: "元素",
        proList: {
            id: { title: "唯一编码", get: "", set: "" },
            name: { title: "名称", get: "", set: "" },
            className: { title: "样式类名", get: "", set: "" },
            style: { title: "式样", get: "", set: "" }
        }
    },
    BODY: {
        title: "文档体",
        proList: {
            leftMargin: { title: "左页边距", get: "getId", set: "setId" },
            topMargin: { title: "顶页边距", get: "getWidth", set: "setWidth" },
            bgproperties: { title: "背景属性", get: "getId", set: "setId" },
            text: { title: "文本颜色", get: "getWidth", set: "setWidth" },
            background: { title: "背景图片", get: "getClassName", set: "setClassName" },
            bgColor: { title: "背景颜色", get: "getBgColor", set: "setBgColor" }
        }
    },
    INPUT: {
        type: "asdfadf",
        text: {
            title: "文本输入框",
            proList: {
                value: { title: "值", get: "getValue", set: "setValue" },
                disabled: { title: "不可用", get: "getDisabled", set: "setDisabled", value: [false, true] },
                readonly: { title: "只读", get: "getReadonly", set: "setReadonly", value: [false, true] }
            }
        },
        title: "表单域",
        proList: {
            type: { title: "控件类型", get: "getValue", set: "setValue", value: ["text", "button", "checkbox", "file", "hidden", "image", "password", "radio", "reset", "submit"] },
            value: { title: "值", get: "getValue", set: "setValue" },
            disabled: { title: "不可用", get: "getDisabled", set: "setDisabled", value: [false, true] },
            readonly: { title: "只读", get: "getReadonly", set: "setReadonly", value: [false, true] }
        }
    },

    IMG: {
        title: "图片",
        proList: {
            src: { title: "源文件", get: "getSrc", set: "setSrc" }
        }
    },

    TABLE: {
        title: "表格",
        proList: {
            cellPadding: { title: "填充", get: "getCellPadding", set: "setCellPadding" },
            cellSpacing: { title: "间距", get: "getCellSpacing", set: "setCellSpacing" },
            align: { title: "对齐", get: "getAlign", set: "setAlign", value: ["left", "center", "right"] },
            border: { title: "边框", get: "getCellPadding", set: "setCellPadding" },
            bgColor: { title: "背景" },
            borderColor: { title: "边框颜色" }
        }
    },
    TD: {
        title: "单元格",
        proList: {
            width: { title: "宽度", get: "getCellPadding", set: "setCellPadding" },
            height: { title: "高度", get: "getCellSpacing", set: "setCellSpacing" },
            background: { title: "背景图片", get: "getClassName", set: "setClassName" },
            bgColor: { title: "背景颜色", get: "getBgColor", set: "setBgColor" },
            vAlign: { title: "垂直对齐", get: "getCellPadding", set: "setCellPadding" },
            align: { title: "水平对齐", get: "getAlign", set: "setAlign", value: ["left", "center", "right"] }
        }
    },
    TR: {
        title: "表格行",
        proList: {
            height: { title: "高度", get: "getCellSpacing", set: "setCellSpacing" },
            background: { title: "背景图片", get: "getClassName", set: "setClassName" },
            bgColor: { title: "背景颜色", get: "getBgColor", set: "setBgColor" },
            valign: { title: "垂直对齐", get: "getCellPadding", set: "setCellPadding" },
            align: { title: "水平对齐", get: "getAlign", set: "setAlign", value: ["left", "center", "right"] }
        }
    },
    IFRAME: {
        title: "帧",
        proList: {
            marginWidth: { title: "左右空白", get: "getCellSpacing", set: "setCellSpacing" },
            marginHeight: { title: "上下空白", get: "getClassName", set: "setClassName" },
            frameBorder: { title: "边框", get: "getBgColor", set: "setBgColor" },
            src: { title: "页面路径", get: "getCellPadding", set: "setCellPadding" }
        }
    },
    A: {
        title: "链接",
        proList: {
            href: { title: "连接地址", get: "getCellPadding", set: "setCellPadding" },
            target: { title: "target目标显示位置", get: "getCellPadding", set: "setCellPadding" }
        }
    }

};

function quoteString(s) {
    return "\"" + s + "\"";
}

/**
* @title 网页在线编辑器 
* @name WBDL Editoer 
* @base tree.js
* @constructor HtmlEditor
* $impport tree.js
*/

var isMSIE = (navigator.appName == "Microsoft Internet Explorer");


function HtmlEditor(frame, toolsDiv) {

    var _this = this;
    var editorDoc = null;
    var _frameElement;
    var _cententWindow;

    this.selPath = [];
    this.event = new Object();
    this.event.onChangeUrl = "onChangeUrl";

    if (!frame) return;

    if (!frame.tagName && frame.frameElement) {
        _frameElement = frame.frameElement;
    } else if (frame.tagName == "IFRAME") {
        _frameElement = frame;
    } else {
        alert("非法的Frame元素，无法创建Editor");
    }

    _cententWindow = _frameElement.contentWindow;
    _cententWindow.contentEditable = true;
    _frameElement.contentEditable = true;
    //    _frameElement.draggable = true;


    this.url = "";

    this.toolsDiv = toolsDiv;

    if (toolsDiv) this.createTools();

    this._Events = new Object();
    this._Events.onSelectObj = [];
    this._Events.onChangeUrl = [];



    this.notifyEvent = function (eventName, args) {
        var event = this._Events[eventName];

        if (!event)
            throw new Error("HTMLEditor没有定义事件:" + eventName);

        var n = event.length;
        if (typeof (event) != "object" || typeof (n) != "number")
            throw new Error("事件:" + eventName + ",被非法覆盖,请不要调用系统中下划线开头的函数或属性，比如_Events");

        for (var i = 0; i < n; i++) {
            var fn = event[i];
            fn(args);
        }
    }

    this.attachEvent = function (eventName, fn) {
        var event = this._Events[eventName];

        if (!event)
            throw new Error("HTMLEditor没有定义事件:" + eventName);

        var n = event.length;
        if (typeof (event) != "object" || typeof (n) != "number")
            throw new Error("事件:" + eventName + ",被非法覆盖,请不要调用系统中下划线开头的函数或属性，比如_Events");

        for (var i = 0; i < n; i++) {
            if (event == fn) return;
        }
        event.push(fn);
    }


    //填写元素属性到元素属性表
    function fullElementProperty(element) {

        var list = _this.elementPropertyList;
        if (!list) return;

        //        if (!element.id || element.id == "" || element.id == undefined) {
        //            if (element.nodeName && element.nodeName != "BODY")
        //                element.id = element.uniqueID;
        //        }


        list.clear();

        var nodeName = element.nodeName;
        list.setProperty("nodeName", nodeName, "元素");

        for (var p in DomDict.COMM.proList) {
            var value = $(element).attr(p);  //DomUtils.getElementAttr(element, p);
            var title = DomDict.COMM.proList[p].title;
            list.setProperty(p, value, title);
        }

        var nodeDict = DomDict[nodeName];
        if (nodeDict) {
            for (var p in nodeDict.proList) {
                var value = $(element).attr(p)//
                //DomUtils.getElementAttr(element, p);
                var title = nodeDict.proList[p].title;
                list.setProperty(p, value, title);
            }
        }


    }

    function hookDocument() {

        //editorDoc = _cententWindow.document;
        //        [-]		contentDocument	{...}	DispHTMLDocument
        editorDoc = _frameElement.contentDocument || _cententWindow.document;
        if (!editorDoc) {
            //          return;
            alert("文档访问失败！文章内容无法保存");
        }



        if (editorDoc.nodeName == "#document" && editorDoc.location.href == "about:blank") {
            editorDoc.charset = "utf-8";
            //   editorDoc.open();
            editorDoc.write("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /></head><body>新文档</body></html>");
            //   editorDoc.close();
        }

        editorDoc.designMode = 'on';
        editorDoc.contentEditable = true;
        if (editorDoc.body) {
            editorDoc.body.contentEditable = true;

            //            editorDoc.body.draggable = true;

        }

        var sUrl = _frameElement.src;
        //        var sI = sUrl.indexOf("?");
        //        if (sI > 0)
        //            sUrl = sUrl.substr(0, sI);

        if (_this.url != sUrl) {
            _this.url = sUrl;
            _this.doChangeUrl(sUrl);
        }

        //        editorDoc.onselect = function () {
        //            var sel = getSelectedElement();
        //        }

        //        if (editorDoc.body) {
        //            editorDoc.body.onselect = function () {
        //                var sel = getSelectedElement();
        //            }
        //        }

        //editorDoc.onselectionchange = function () {
        //        if ($.browser.msie && parseInt($.browser.version) < 8)
        //            editorDoc.onselectionchange = onSelect;
        //        else
        //            editorDoc.onclick = onSelect;
        // editorDoc.onselectionchange = onSelect;
        editorDoc.onmouseup = onSelect;
        //     _frameElement.onmouseup = onSelect;
        //    editorDoc.onselect = onSelect;
        //    editorDoc.onclick = onSelect;

        function onSelect() {

            var sel = getSelectedElement();
            if (!sel) return;

            getSelPath(sel);

            if (sel.ownerDocument != editorDoc) return;


            if (typeof (sel) == "string") {
                window.status = sel;
                return
            }

            if (sel.nodeType == 3)
                sel = sel.parentNode;

            if (sel.nodeName) {
                window.status = sel.nodeName;

                fullElementProperty(sel);
                _this.doSelected(sel);
                return;

            }

            window.status = sel.toString();
        }

        //firefox
        // editorDoc.ondbclick = editorDoc.onmouseup;

        //_frameElement.onreadystatechange = function () {
        //    SystemPro.windowDocumentReadyRun(_cententWindow, hookDocument, null);
        // }



        //   if (!_frameElement.src || _frameElement.src == "about:blank") {

        //            var htmUrl = _this.submit();
        //          _this.changeUrl(htmUrl);
        //
        //       }

    }



    function initEditor() {
        if (!_frameElement) {
            return;
        }
        hookDocument();
        _frameElement.onload = function () {
            hookDocument();
        }

        //   SystemPro.windowDocumentReadyRun(_cententWindow, hookDocument, null);
    }


    function insertHTML(htmlText) {
        editorDoc.parentWindow.focus();
        var rang = document.selection.createRange();
        try {
            rang.pasteHTML(htmlText);
        } catch (err) {
        }
    }

    function insertBr(e) {

        e = editorDoc.parentWindow.event;
        if (!e || e.keyCode != 13) return true;

        var rang = editorDoc.selection.createRange();
        if ((rang != null) && rang.length)
            return true;

        insertHTML("<br>");
        // 工作代码
        var sel = editorDoc.selection; // 获取文本
        var rng = sel.createRange(); // 创建一个选区

        rng.moveStart('character', 0); // 移动起始点，只是调用，并不实际移动
        rng.collapse(true); // 在起始点重叠
        rng.select(); // 显示光标
        return false;


    }

    //事件句柄
    this.onSelected = function (selectedElement) { };
    this.onElementPropertyChange = function (element, propertyName, propertyValue) { };
    this.getWindow = function () {
        return _cententWindow;
    }

    function createSelPathNode(se) {
        var _ea = document.createElement("a");

        _this.selBar.appendChild(_ea);
        _ea.innerText = "[" + se.nodeName + "]";
        _ea.text = "[" + se.nodeName + "]";
        _ea.href = "javascript:void(0);";
        _ea.onclick = function () {
            var sel = null;
            if ($.browser.msie) {
                sel = _this.selectElement_IE6(se);
            } else {
                sel = _this.selectElement(se);
            }
            fullElementProperty(sel);
            _this.doSelected(sel);
        }
    }
    function getSelPath(el) {
        _this.selPath = $(el).parentsUntil("html").get();
        //if(_this.selPath
        if (_this.selBar) {
            _this.selBar.innerHTML = "";
            for (var i = _this.selPath.length - 1; i >= 0; i--) {
                var se = _this.selPath[i];
                createSelPathNode(se);
            }
            createSelPathNode(el);
        }
    }

    function getSelectedElement() {
        // try {

        if ((!$.browser.msie) && editorDoc.activeElement && editorDoc.activeElement.tagName != "BODY")
            return editorDoc.activeElement;

        //ie6   var rang = editorDoc.selection.createRange();



        var sel = rangy.getSelection(_cententWindow);
        var rang = sel.getRangeAt(0);
        // } catch (e) {
        //     return;
        // }

        var nodes = rang.getNodes();

        if (!nodes || !nodes.length || nodes.length < 1)
            return rang.endContainer;

        return nodes[0];
        //        var selectedUnqueID = null;
        if ((rang != null) && rang.length) {
            // selectedUnqueID = rang(0).uniqueID;
            return rang[0];
        }
        if ((rang != null) && rang.text) {
            var node = rang.parentElement();
            // selectedUnqueID = rang(0).uniqueID;
            return node;
            // selectedUnqueID = rang(0).uniqueID;
            //            return rang.text;
        }
        if (rang) {
            var node = rang.parentElement();
            // selectedUnqueID = rang(0).uniqueID;
            if (node.nodeName.toUpperCase() != "BODY")
                return node;
        }
        return null;
    }

    this.getEditorDocument = function () {
        return editorDoc;
    }

    this.selectElement = function (elementId) {
        _cententWindow.focus();
        var element = elementId;
        if ((typeof element) == "string")
            element = editorDoc.getElementById(elementId);

        //        rangy.selectNode(element);

        //var selection=

        if (element.nodeType == 3)
            element = element.parentNode;

        var selection = null;
        try {
            selection = _cententWindow.getSelection(); //ff
        } catch (e) {
        }
        if (!selection)//ie9 ,ff,chrome
            selection = editorDoc.selection;

        var range = editorDoc.createRange();
        range.selectNode(element);
        selection.removeAllRanges();
        selection.addRange(range);
        //        sel = rangy.getSelection();
        //        sel.setSingleRange(range);
        //        if (element.setSelectionRange) { // Firefox 
        //            // element.setSelectionRange();
        //            var sel = _cententWindow.getSelection();
        //            sel.removeAllRanges();
        //            var range = editorDoc.createRange();
        //            range.selectNode(element);
        //            sel.addRange(range);
        //        }
        //        //sel.refresh(true);
        if (element)
            element.focus();
        return element;

    }

    this.selectElement_IE6 = function (elementId) {

        var element = elementId;
        if ((typeof element) == "string")
            element = editorDoc.getElementById(elementId);

        if (!element) return;

        //  if (!element.contentEditable || !element.contentEditable != true)
        //      element.contentEditable = true;
        // editorDoc.parentWindow.focus();
        editorDoc.selection.empty();


        try {
            var rng = editorDoc.body.createControlRange();
            // rng.addElement(element);
            rng.add(element);
            rng.select();
            //   rng.scrollIntoView(0);
        }
        catch (Error) {
            // element.select();
            if (element.nodeType == 3)
                element = element.parentNode;
            var rng = editorDoc.body.createTextRange();
            rng.moveToElementText(element);

            //var rng = element.createControlRange();
            //rng.add(element);
            rng.select();
        }

        return element;

    }


    this.elementPropertyList = null;
    this.setElementPropertyList = function (propertyList) {
        var _this = this;
        propertyList.onPropertyChange = function (propertyName, propertyValue) {
            var oldvalue = $(_this.activeElement).attr(propertyName);
            //DomUtils.getElementAttr(_this.activeElement, propertyName);
            $(_this.activeElement).attr(propertyName, propertyValue);
            //DomUtils.setElementAttr(_this.activeElement, propertyName, propertyValue);
            _this.doElementPropertyChange(_this.activeElement, propertyName, propertyValue, oldvalue);
        }
        this.elementPropertyList = propertyList;
    };


    this.changeUrl = function (url) {
        //      _frameElement.onreadystatechange = function() { };
        //        s_frameElement.frameElement.src = url + "?time=" + new Date().toLocaleTimeString() + "_" + Math.random();
        //       _frameElement.src = url + "?time=" + new Date().toLocaleTimeString() + "_" + Math.random();
        //    _frameElement.contentWindow.location = url + "?time=" + new Date().toLocaleTimeString() + "_" + Math.random();
        //        _frameElement.location = url + "?time=" + new Date().toLocaleTimeString() + "_" + Math.random();
        if (url.substr(0, 2) == "//")
            url = url.replace("//", "/");
        _frameElement.src = url + "?time=" + new Date().toLocaleTimeString() + "_" + Math.random();

        //        editorDoc = _frameElement.contentWindow.document;
        //        editorDoc.location = url + "?time=" + new Date().toLocaleTimeString() + "_" + Math.random();
        //   editorDoc = _frameElement.document;
        //   hookDocument();
        //        elementReadyRun(_frameElement.frameElement, hookDocument, null);
    }

    this.getDocWindow = function () {
        return _cententWindow;
    }

    initEditor();
}


HtmlEditor.prototype.activeElement = null;


HtmlEditor.prototype.doChangeUrl = function (url) {
    this.url = url;
    this.notifyEvent(this.event.onChangeUrl, url);
    if (this.onChangeUrl) this.onChangeUrl(url);
};

//事件句柄
HtmlEditor.prototype.onChangeUrl = function (url) { };

//动态方法
HtmlEditor.prototype.doSelected = function (selectedElement) {

    if (this.activeElement != selectedElement) {
        this.activeElement = selectedElement;
        this.notifyEvent("onSelectObj", selectedElement);
        if (this.onSelected) this.onSelected(selectedElement);
    }
};

HtmlEditor.prototype.doElementPropertyChange = function (element, propertyName, propertyValue, oldvalue) {
    if (this.onElementPropertyChange != null && this.onElementPropertyChange != undefined)
        this.onElementPropertyChange(element, propertyName, propertyValue, oldvalue);
};

//字体特效 － 加粗方法一
HtmlEditor.prototype.addBold = function () {
    //  editorDoc.parentWindow.focus();
    //所有字体特效只是使用execComman()就能完成。
    editorDoc.execCommand("Bold", false, null);
}

//插入表格
HtmlEditor.prototype.insertTable = function (colCount, rowCount) {
    var document = this.getEditorDocument();
    document.parentWindow.focus();

    if (this.activeElement.nodeName == "TABLE") {
        this.setTableColRow(colCount, rowCount);
        return;
    }

    var table = "<table border='1'>";

    for (var r = 0; r < rowCount; r++) {
        table += "<tr>";
        for (var c = 0; c < colCount; c++) {
            table += "<td>";
            table += "&nbsp;";
            table += "</td>";
        }
        table += "</tr>";
    }
    table += "</table>";

    try {
        var rang = document.selection.createRange();
        rang.pasteHTML(table);

    } catch (err) {
        alert("不能在这个位置插入表格");
    }
};

//插入层
HtmlEditor.prototype.insertDiv = function () {
    var document = this.getEditorDocument();
    document.parentWindow.focus();

    var div = "<div style='height:150px; width:200px; border:solid 1px'></div>";

    try {
        var rang = document.selection.createRange();
        rang.pasteHTML(div);

    } catch (err) {
        alert("不能在这个位置插入层");
    }
};

//插入链接
HtmlEditor.prototype.insertLink = function (id, name, href, target) {
    if (this.activeElement.tagName.toUpperCase() == "A") {
        alert("选定的文本已经存在链接，不能再插入链接！");
        return;
    }
    var document = this.getEditorDocument();
    document.parentWindow.focus();
    var rang = document.selection.createRange();
    if (rang.text && rang.text != "") {
        var oHtml = " <a  name=" + quoteString(name) + " id=" + quoteString(id) + " href= " + quoteString(href) + " target=" + quoteString(target) + ">" + rang.text + " </a> ";
        rang.pasteHTML(oHtml);
        //   document.execCommand("CreateLink");
    }

};

//设置表格属性
HtmlEditor.prototype.setTableColRow = function (colCount, rowCount) {

    var table = this.activeElement;

    //  debugger;

    if (!table || table == undefined || table.nodeName != "TABLE") return;


    var oldRowCount = table.rows.length;

    var oldColCount = 0;
    if (table.rows.length > 0)
        oldColCount = table.rows[0].cells.length;

    if (rowCount > oldRowCount) {
        for (var i = 0; i < rowCount - oldRowCount; i++) {
            var row = table.insertRow(-1);
            for (var j = 0; j < oldColCount; j++) {
                var cell = row.insertCell(-1);
                cell.innerHTML = "&nbsp;";
            }
        }
    }

    if (rowCount < oldRowCount) {
        for (var i = 0; i < oldRowCount - rowCount; i++) {
            table.deleteRow(table.rows.length - 1);
        }
    }

    for (var r = 0; r < table.rows.length; r++) {
        var row = table.rows[r];
        var isDelete = oldColCount > colCount;
        var cc = Math.abs(oldColCount - colCount);
        for (var c = 0; c < cc; c++) {
            if (isDelete) {
                row.deleteCell(row.cells.length - 1);
            } else {
                var cell = row.insertCell(-1);
                cell.innerHTML = "&nbsp;";
            }
        }
    }

};


//重設為一個InputButtong一樣,
HtmlEditor.prototype.insertInputButton = function () {
    var document = this.getEditorDocument();
    var id = document.uniqueID;
    this.getWindow().focus();

    document.execCommand('InsertInputButton', true, id); //true或false無效 

    document.all[id].value = "按钮"
}

//插入button
HtmlEditor.prototype.insertButton = function () {
    var document = this.getEditorDocument();
    var id = document.uniqueID;
    this.getWindow().focus();

    document.execCommand('InsertButton', true, id); //true或false無效 

    document.all[id].value = "按钮"
}
//重設為一個fieldset 

/*document.execCommand('InsertFieldSet',true,"aa"); 

document.all.aa.innerText="刀劍如夢";*/

//插入一個水平線 

//document.execCommand('InsertHorizontalRule',true,"aa"); 

//插入一個iframe
HtmlEditor.prototype.insertIFrame = function () {
    var document = this.getEditorDocument();
    document.parentWindow.focus();
    document.execCommand('InsertIFrame', true);
}
//插入一個InsertImage,設為true時需要圖片,false時不需圖片
HtmlEditor.prototype.insertImage = function () {

    //    var htmUrl = document.getElementById("HtmlFileName").value;

    var htmUrl = this.url;

    if (!htmUrl)
        htmUrl = this.submit();

    var win = this.getDocWindow();
    var _this = this;
    SystemPro.windowDocumentReadyRun(win, function () {
        var doc = _this.getEditorDocument();
        var imgUrl = doc.location.pathname + ".files/";

        var fraDoc = win.document;
        win.focus();





        var ret = window.showModalDialog("../dialogs/upload_dlg.html", imgUrl,
               "dialogHeight:300px;dialogWidth:500px;center:yes;resizable:yes;scroll:no");
        if (!ret)
            return;

        var paths = ret.split("/");

        var src = paths.pop();
        src = paths.pop() + "/" + src;

        fraDoc.execCommand('InsertImage', false, src);

        _this.submit();
    }
, null);

    //    this.changeUrl(htmUrl);
}

//插入一個checkbox 
HtmlEditor.prototype.insertCheckBox = function () {
    var document = this.getEditorDocument();
    document.parentWindow.focus();


    document.execCommand('InsertInputCheckbox', true);
}
//插入一個file類型的object 

HtmlEditor.prototype.insertInputFileUpload = function () {
    var document = this.getEditorDocument();
    document.parentWindow.focus();


    document.execCommand('InsertInputFileUpload', false);
}

//插入一個hidden
HtmlEditor.prototype.insertInputHidden = function () {
    var document = this.getEditorDocument();
    document.parentWindow.focus();
    document.execCommand('InsertInputHidden', false)
}

//插入一個Password
HtmlEditor.prototype.insertInputPassword = function () {
    var document = this.getEditorDocument();
    document.parentWindow.focus();
    document.execCommand('InsertInputPassword', true);
}


//插入一個图象域
HtmlEditor.prototype.insertInputImage = function () {
    var document = this.getEditorDocument();
    document.parentWindow.focus();
    document.execCommand('InsertInputImage', true);
}

//插入一個图象域
HtmlEditor.prototype.insertLabel = function () {
    var document = this.getEditorDocument();

    document.parentWindow.focus();
    var rang = document.selection.createRange();
    if (!rang) return;

    if (rang.length && !rang.text) {
        alert("不能在这个地方插入Label");
        return;
    }

    var html = "<Label>";

    if (rang.length)
        html += rang(0).text + "</Label>";
    else if (rang.text)
        html += rang.text + "</Label>";
    else
        html += "标签</Label>";
    //            return rang(0);
    //        }
    //    if ((rang != null) && rang.text)
    //        html += rang.text + "</Label>";
    //    else
    //        html += "标签</Label>";

    rang.pasteHTML(html);
}




//插入一個Radio 
HtmlEditor.prototype.insertRadio = function () {
    var document = this.getEditorDocument();
    document.parentWindow.focus();

    document.execCommand('InsertInputRadio', true);
}
//插入一個Reset 

//document.execCommand('InsertInputReset',true,"aa"); 

//插入一個Submit 

//document.execCommand('InsertInputSubmit',false,"aa"); 

//插入一個input text
HtmlEditor.prototype.insertInput = function () {
    var document = this.getEditorDocument();
    document.parentWindow.focus();

    document.execCommand('InsertInputText', true);
}

//插入一個textarea 
HtmlEditor.prototype.insertText = function () {
    var document = this.getEditorDocument();
    document.parentWindow.focus();

    document.execCommand('InsertTextArea', true);
}

//插入一個 select list box
HtmlEditor.prototype.insertListBox = function () {
    var document = this.getEditorDocument();
    document.parentWindow.focus();

    document.execCommand('InsertSelectListbox', true);
}

//插入一個single select
HtmlEditor.prototype.insertSelect = function () {
    var document = this.getEditorDocument();
    document.parentWindow.focus();
    document.execCommand('InsertSelectDropdown', true);
}
//插入一個line break(硬回車??) 

//document.execCommand('InsertParagraph');


//重设为一个fieldset
HtmlEditor.prototype.insertFieldSet = function () {
    var document = this.getEditorDocument();
    var id = document.uniqueID;
    document.parentWindow.focus();

    document.parentWindow.focus();
    document.execCommand('InsertFieldSet', true, id);
    var el = document.all[id];
    el.style.width = 200;
    el.style.height = 150;
    var leg = document.createElement("legend");
    leg.innerText = "分组框";
    el.appendChild(leg);
}

HtmlEditor.prototype.exePaste = function () {
    var document = this.getEditorDocument();
    document.parentWindow.focus();

    document.execCommand("Paste");
}

HtmlEditor.prototype.exeCopy = function () {
    var document = this.getEditorDocument();
    //   document.parentWindow.focus();
    document.execCommand("Copy");
}

HtmlEditor.prototype.exeCut = function () {
    var document = this.getEditorDocument();
    //   document.parentWindow.focus();
    document.execCommand("Cut");
}

HtmlEditor.prototype.exeUndo = function () {
    var document = this.getEditorDocument();
    //    document.parentWindow.focus();
    document.execCommand("Undo");
}

HtmlEditor.prototype.exeRedo = function () {
    var document = this.getEditorDocument();
    //    document.parentWindow.focus();
    document.execCommand("Redo");
}

HtmlEditor.prototype.exeBlod = function () {
    var document = this.getEditorDocument();
    //   document.parentWindow.focus();
    document.execCommand("Bold");
}

HtmlEditor.prototype.exeItalic = function () {
    var document = this.getEditorDocument();
    //    document.parentWindow.focus();
    document.execCommand("Italic");
}

HtmlEditor.prototype.exeUnderline = function () {
    var document = this.getEditorDocument();
    //  document.parentWindow.focus();
    document.execCommand("Underline");
}

HtmlEditor.prototype.exeForeColor = function () {
    var document = this.getEditorDocument();
    //  document.parentWindow.focus();
    document.execCommand("ForeColor", true);
}

HtmlEditor.prototype.exeBackColor = function () {
    var document = this.getEditorDocument();
    //  document.parentWindow.focus();
    document.execCommand("BackColor", true);
}

HtmlEditor.prototype.exeJustifyLeft = function () {
    var document = this.getEditorDocument();
    //  document.parentWindow.focus();
    document.execCommand("JustifyLeft");
}

HtmlEditor.prototype.exeJustifyCenter = function () {
    var document = this.getEditorDocument();
    //   document.parentWindow.focus();
    document.execCommand("JustifyCenter");
}

HtmlEditor.prototype.exeJustifyRight = function () {
    var document = this.getEditorDocument();
    //  document.parentWindow.focus();
    document.execCommand("JustifyRight");
}

HtmlEditor.prototype.exeJustifyFull = function () {
    var document = this.getEditorDocument();
    //  document.parentWindow.focus();
    document.execCommand("JustifyFull");
}

HtmlEditor.prototype.insertOrderList = function () {
    var document = this.getEditorDocument();
    //    document.parentWindow.focus();
    document.execCommand("InsertOrderedList");
}

HtmlEditor.prototype.insertUnorderedList = function () {
    var document = this.getEditorDocument();
    //    document.parentWindow.focus();
    document.execCommand("InsertUnorderedList");
}

HtmlEditor.prototype.exeIndent = function () {
    var document = this.getEditorDocument();
    // document.parentWindow.focus();
    //    this.getDocWindow().focus();
    document.execCommand("Indent");
}

HtmlEditor.prototype.exeOutdent = function () {
    var document = this.getEditorDocument();
    //    document.parentWindow.focus();
    document.execCommand("Outdent");
}

HtmlEditor.prototype.exeUnlink = function () {
    var document = this.getEditorDocument();
    //   document.parentWindow.focus();
    document.execCommand("Unlink");
}


HtmlEditor.prototype.insertLine = function () {
    var document = this.getEditorDocument();
    document.parentWindow.focus();
    document.execCommand("InsertHorizontalRule");
}

HtmlEditor.prototype.exeFont = function (fontName) {
    var document = this.getEditorDocument();
    //   document.parentWindow.focus();
    document.execCommand('FontName', false, fontName);      //true或false都可以 
}

HtmlEditor.prototype.exeFontSize = function (fontSize) {
    var document = this.getEditorDocument();
    document.parentWindow.focus();
    document.execCommand('FontSize', false, fontSize);      //true或false都可以  
}

/*
class UploadHtml
{
public string FileUrl="";
public string Text="";
public string Charset="";
}

class UploadReturn
{
public bool Ret=false;
public string Message = "";
public string FileUrl = "";
}

*/

HtmlEditor.prototype.submit = function () {

    //    var efileName = document.getElementById("HtmlFileName");

    var fraDoc = this.getEditorDocument();
    var url = "/xj-service/HtmlPoster.aspx";

    var file = new Object();
    if (!fraDoc.documentElement) return;

    fraDoc.body.removeAttribute("contentEditable");

    file.Text = fraDoc.documentElement.outerHTML;
    //    file.FileUrl = efileName.value;
    file.FileUrl = this.url;
    file.Charset = document.charset;

    var sfile = XAjax.obj2str(file);
    var s = XAjax.SynRequest(url, sfile);
    var ret = eval("(" + s + ")");

    if (!ret.Ret) {
        alert(ret.Message);
        return null;
    }

    //    efileName.value = ret.FileUrl;

    if (this.url != ret.FileUrl)
        this.changeUrl(ret.FileUrl);

    return ret.FileUrl;
}

HtmlEditor.prototype.createTools = function () {
    var _this = this;

    var elPaste = document.getElementById("btnPaste");
    var elCut = document.getElementById("btnCut");
    var elCopy = document.getElementById("btnCopy");
    var elUndo = document.getElementById("btnUndo");
    var elRedo = document.getElementById("btnRedo");
    var elBlod = document.getElementById("btnBlod");
    var elItalic = document.getElementById("btnItalic");
    var elUnderLine = document.getElementById("btnUnderLine");
    var elColor = document.getElementById("btnColor");
    var elBgColor = document.getElementById("btnBgColor");
    var elLeftAlign = document.getElementById("btnLeftAlign");
    var elCenterAlign = document.getElementById("btnCenterAlign");
    var elRightAlign = document.getElementById("btnRightAlign");
    var elJustAlign = document.getElementById("btnJustAlign");
    var elOrderList = document.getElementById("btnOrderList");
    var elList = document.getElementById("btnList");
    var elOutdent = document.getElementById("btnOutdent");
    var elIndent = document.getElementById("btnIndent");
    var elInsertLine = document.getElementById("btnInsertLine");
    var elInsertface = document.getElementById("btnInsertface");
    var elInsertPicture = document.getElementById("btnInsertPicture");
    var elInsertLink = document.getElementById("btnInsertLink");
    var elInsertTable = document.getElementById("btnInsertTable");
    var elInsertDiv = document.getElementById("btnInsertDiv");
    var elInsertIframe = document.getElementById("btnInsertIframe");

    var elInsertInputText = document.getElementById("btnInsertInput");
    var elInsertHidden = document.getElementById("btnInsertHidden");
    var elInsertTextArea = document.getElementById("btnInsertTextArea");
    var elInsertRadio = document.getElementById("btnInsertRadio");
    var elInsertCheckBox = document.getElementById("btnInsertCheckBox");
    var elInsertDropDawn = document.getElementById("btnInsertDropDown");
    var elInsertInputFile = document.getElementById("btnInsertFile");
    var elInsertLabel = document.getElementById("btnInsertLabel");
    var elInsertInputButton = document.getElementById("btnInsertInputButton");
    var elInsertInputImage = document.getElementById("btnInsertInputImage");
    var elInsertLabel = document.getElementById("btnInsertLabel");


    var elPreview = document.getElementById("btnPreview");

    var elFont = document.getElementById("inputFont");
    var elFontSize = document.getElementById("inputFontSize");

    if (elPaste)
        elPaste.onclick = function () { _this.exePaste() };
    if (elCut)
        elCut.onclick = function () { _this.exeCut() };
    if (elCopy)
        elCopy.onclick = function () { _this.exeCopy() };
    if (elUndo)
        elUndo.onclick = function () { _this.exeUndo() };
    if (elRedo)
        elRedo.onclick = function () { _this.exeRedo() };
    if (elBlod)
        elBlod.onclick = function () { _this.exeBlod() };
    if (elItalic)
        elItalic.onclick = function () { _this.exeItalic() };
    if (elUnderLine)
        elUnderLine.onclick = function () { _this.exeUnderline() };
    if (elColor)
        elColor.onclick = function () { _this.exeForeColor() };
    if (elBgColor)
        elBgColor.onclick = function () { _this.exeBackColor() };
    if (elLeftAlign)
        elLeftAlign.onclick = function () { _this.exeJustifyLeft() };
    if (elCenterAlign)
        elCenterAlign.onclick = function () { _this.exeJustifyCenter() };
    if (elRightAlign)
        elRightAlign.onclick = function () { _this.exeJustifyRight() };
    if (elJustAlign)
        elJustAlign.onclick = function () { _this.exeJustifyFull() };
    if (elOrderList)
        elOrderList.onclick = function () { _this.insertOrderList() };
    if (elList)
        elList.onclick = function () { _this.insertUnorderedList() };
    if (elOutdent)
        elOutdent.onclick = function () { _this.exeOutdent() };
    if (elIndent)
        elIndent.onclick = function () { _this.exeIndent() };
    if (elInsertLine)
        elInsertLine.onclick = function () { _this.insertLine() };

    if (elInsertface)
        elInsertface.onclick = function () { };
    if (elInsertPicture)
        elInsertPicture.onclick = function () { _this.insertImage() };

    //    btnInsertLink.onclick = function() {};
    //btnInsertTable.onclick = function() { };
    if (elInsertDiv)
        elInsertDiv.onclick = function () { _this.insertDiv() }
    if (elInsertIframe)
        elInsertIframe.onclick = function () { _this.insertIFrame() }
    if (elInsertInputText)
        elInsertInputText.onclick = function () { _this.insertInput() }
    if (elInsertHidden)
        elInsertHidden.onclick = function () { _this.insertInputHidden() }
    if (elInsertTextArea)
        elInsertTextArea.onclick = function () { _this.insertText() }

    if (elInsertRadio)
        elInsertRadio.onclick = function () { _this.insertRadio() }
    if (elInsertCheckBox)
        elInsertCheckBox.onclick = function () { _this.insertCheckBox() }

    if (elInsertDropDawn)
        elInsertDropDawn.onclick = function () { _this.insertSelect() }
    if (elInsertInputFile)
        elInsertInputFile.onclick = function () { _this.insertInputFileUpload() }
    if (elInsertLabel)
        elInsertLabel.onclick = function () { _this.insertLabel() }
    if (elInsertInputButton)
        elInsertInputButton.onclick = function () { _this.insertInputButton() }
    if (elInsertInputImage)
        elInsertInputImage.onclick = function () { _this.insertInputImage() }

    if (elFont)
        elFont.onchange = function () {
            _this.exeFont(elFont.value);
        }

    if (elFontSize)
        elFontSize.onchange = function () {
            var size = Number(elFontSize.value);
            if (size && size > 0)
                _this.exeFontSize(size);
        }


}
