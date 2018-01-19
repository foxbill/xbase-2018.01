//import(/sytem/system.js);
//import(/contorls/listview.js);

DomDict = {
    COMM: {
        title: "元素",
        proList: {
            id: { title: "唯一编码", get: "getId", set: "setId" },
            name: { title: "名称", get: "getWidth", set: "setWidth" },
            "class": { title: "样式类", get: "getClassName", set: "setClassName" },
            style: { title: "式样", get: "getBgColor", set: "setBgColor" }
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
            width: { title: "宽度", get: "getCellPadding", set: "setCellPadding" },
            height: { title: "高度", get: "getCellSpacing", set: "setCellSpacing" },
            cellPadding: { title: "填充", get: "getCellPadding", set: "setCellPadding" },
            cellSpacing: { title: "间距", get: "getCellSpacing", set: "setCellSpacing" },
            align: { title: "对齐", get: "getAlign", set: "setAlign", value: ["left", "center", "right"] },
            isList: { title: "设为列表", get: "getCellPadding", set: "setCellPadding", value: [0, 1] },
            dataRow: { title: "模版行号", get: "getCellPadding", set: "setCellPadding" },
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
        title: "单元格",
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


function HtmlEditor(editorWindow, toolsDiv) {

    var _this = this;
    var editorDoc;

    this.toolsDiv = toolsDiv;

    this._Events = new Object();
    this._Events.onSelectObj = [];


    this.notifyEvent = function(eventName, args) {
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

    this.attachEvent = function(eventName, fn) {
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


    var isMSIE = (navigator.appName == "Microsoft Internet Explorer");

    if (!editorWindow) return;

    if (editorWindow.contentWindow) {
        //        editorWindow.designMode = 'On';
        //        editorWindow.contentEditable = true;
        editorWindow = editorWindow.contentWindow;
    }

    this.url = "";


    initEditor();

    function fullElementProperty(element) {

        var list = _this.elememntPropertyList;
        if (!list) return;

        if (!element.id || element.id == "" || element.id == undefined) {
            if (element.nodeName && element.nodeName != "BODY")
                element.id = element.uniqueID;
        }


        list.clear();

        var nodeName = element.nodeName;
        list.setProperty("nodeName", nodeName, "元素");

        for (var p in DomDict.COMM.proList) {
            var value = DomUtils.getElementAttr(element, p);
            var title = DomDict.COMM.proList[p].title;
            list.setProperty(p, value, title);
        }

        var nodeDict = DomDict[nodeName];
        if (nodeDict) {
            for (var p in nodeDict.proList) {
                var value = DomUtils.getElementAttr(element, p);
                var title = nodeDict.proList[p].title;
                list.setProperty(p, value, title);
            }
        }


    }

    function hookEditor() {

        editorDoc = editorWindow.document;
        editorDoc.designMode = 'on';
        editorDoc.contentEditable = true;
        if (editorDoc.body)
            editorDoc.body.contentEditable = true;


        //        if (isMSIE) {
        //            editorDoc.onkeypress = insertBr;
        //        }

        if (_this.url != editorDoc.location.pathname) {
            _this.url = editorDoc.location.pathname;
            _this.doChangeUrl(editorWindow.location.pathname);
        }

        editorDoc.onselectionchange = function() {

            var sel = getSelectedElement();
            if (sel.ownerDocument != editorDoc) return;

            if (!sel) return;

            if (typeof (sel) == "string") {
                window.status = sel;
            }
            else if (sel.nodeName) {
                window.status = sel.nodeName;
                //      var element = sel;

                if (sel.id != "" && sel.nodeName != "BODY") {
                    var itvId = setTimeout(function() {
                        var ref = sel.ownerDocument.parentWindow[sel.id];
                        if (ref && ref != undefined && ref != sel && ref.length && ref.length > 1) {
                            sel.id = sel.uniqueID;
                            fullElementProperty(sel);
                        }
                        clearTimeout(itvId);
                    }, 10);
                }

                fullElementProperty(sel);
            }
            else {
                window.status = sel.toString();
                return;
            }

            _this.doSelected(sel);
        }

    }

    function initEditor() {

        if (!editorWindow) {
            return;
        }

        editorDoc = editorWindow.document;

        if (editorDoc.nodeName == "#document" && editorDoc.location.href == "about:blank") {
            editorDoc.charset = "utf-8";
            editorDoc.open();
            editorDoc.write("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /></head><body>aaaaa<input type='text' value='aaa'/></body></html>");
            editorDoc.close();
        }

        editorDoc.onreadystatechange = null;
        editorDoc.designMode = 'on';
        editorDoc.contentEditable = true;
        if (editorDoc.body)
            editorDoc.body.contentEditable = true;
        _this.url = editorDoc.location.pathname;

        //        elementReadyRun(editorDoc, hookEditor);
        elementReadyRun(editorWindow.frameElement, hookEditor, null);

        //        editorDoc.onreadystatechange = function() {
        //            if (editorDoc.readyState == "complete") {
        //                hookEditor();
        //            }
        //        }

        editorDoc.parentWindow.focus();

        //        editorDoc = editorWindow.document;
        //        elementReadyRun(editorDoc, hookEditor);

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
    this.onSelected = function(selectedElement) { };
    this.onElementPropertyChange = function(element, propertyName, propertyValue) { };


    function getSelectedElement() {
        var rang = editorDoc.selection.createRange();
        //        var selectedUnqueID = null;
        if ((rang != null) && rang.length) {
            // selectedUnqueID = rang(0).uniqueID;
            return rang(0);
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
            return node;
        }
        return null;
    }

    this.changeUrl = function(url) {
        editorWindow.onreadystatechange = function() { };
        editorWindow.frameElement.src = url + "?time=" + new Date();
        //   editorDoc = editorWindow.document;
        //   hookEditor();
        elementReadyRun(editorWindow.frameElement, hookEditor, null);
    }

    this.getEditorDocument = function() {
        return editorDoc;
    }

    this.selectElement = function(elementId) {
        var element = editorDoc.getElementById(elementId);

        if (!element) return;

        if (!element.contentEditable)
            element.contentEditable = true;
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
            var rng = editorDoc.body.createTextRange();
            rng.moveToElementText(element);
            rng.select();
        }

    }

    this.elememntPropertyList = null;
    this.setElememntPropertyList = function(propertyList) {
        var _this = this;
        propertyList.onPropertyChange = function(propertyName, propertyValue) {
            var oldvalue = DomUtils.getElementAttr(_this.activeElement, propertyName);
            DomUtils.setElementAttr(_this.activeElement, propertyName, propertyValue);
            _this.doElementPropertyChange(_this.activeElement, propertyName, propertyValue, oldvalue);
        }
        this.elememntPropertyList = propertyList;
    };




}


HtmlEditor.prototype.activeElement = null;


//事件句柄
HtmlEditor.prototype.onChangeUrl = function(url) { };

//动态方法
HtmlEditor.prototype.doSelected = function(selectedElement) {
    if (this.activeElement != selectedElement) {
        this.activeElement = selectedElement;
        this.notifyEvent("onSelectObj", selectedElement);
        if (this.onSelected) this.onSelected(selectedElement);
    }
};

HtmlEditor.prototype.doElementPropertyChange = function(element, propertyName, propertyValue, oldvalue) {
    if (this.onElementPropertyChange != null && this.onElementPropertyChange != undefined)
        this.onElementPropertyChange(element, propertyName, propertyValue, oldvalue);
};

HtmlEditor.prototype.doChangeUrl = function(url) {
    this.url = url;
    if (this.onChangeUrl) this.onChangeUrl(url);
};


//字体特效 － 加粗方法一
HtmlEditor.prototype.addBold = function() {
    editorDoc.parentWindow.focus();
    //所有字体特效只是使用execComman()就能完成。
    editorDoc.execCommand("Bold", false, null);
}

//插入表格
HtmlEditor.prototype.insertTable = function(colCount, rowCount) {
    var document = this.getEditorDocument();
    document.parentWindow.focus();

    if (this.activeElement.nodeName == "TABLE") {
        this.setTableColRow(colCount, rowCount);
        return;
    }

    var table = "<table border='1' width='300' height='300'>";

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
HtmlEditor.prototype.insertDiv = function() {
    var document = this.getEditorDocument();
    document.parentWindow.focus();

    var div = "<div style='height:150px; width:200px; border-style:solid; border-width:thin'></div>";

    try {
        var rang = document.selection.createRange();
        rang.pasteHTML(div);

    } catch (err) {
        alert("不能在这个位置插入层");
    }
};

//插入链接
HtmlEditor.prototype.insertLink = function(id, name, href, target) {
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
HtmlEditor.prototype.setTableColRow = function(colCount, rowCount) {

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


/*重設為一個button(InsertButton和InsertInputButtong一樣, 

隻不前者是button,後者是input)*/
HtmlEditor.prototype.insertButton = function() {
    var document = this.getEditorDocument();
    var id = document.uniqueID;
    document.parentWindow.focus();

    document.execCommand('InsertButton', true, id); //true或false無效 

    document.all[id].value = "按钮"
}
//重設為一個fieldset 

/*document.execCommand('InsertFieldSet',true,"aa"); 

document.all.aa.innerText="刀劍如夢";*/

//插入一個水平線 

//document.execCommand('InsertHorizontalRule',true,"aa"); 

//插入一個iframe
HtmlEditor.prototype.insertIFrame = function() {
    var document = this.getEditorDocument();
    document.parentWindow.focus();
    document.execCommand('InsertIFrame', true);
}
//插入一個InsertImage,設為true時需要圖片,false時不需圖片 
HtmlEditor.prototype.insertImage = function() {
    var document = this.getEditorDocument();
    document.parentWindow.focus();

    document.execCommand('InsertImage', true);
}

//插入一個checkbox 
HtmlEditor.prototype.insertCheckBox = function() {
    var document = this.getEditorDocument();
    document.parentWindow.focus();


    document.execCommand('InsertInputCheckbox', true);
}
//插入一個file類型的object 

//document.execCommand('InsertInputFileUpload',false,"aa"); 

//插入一個hidden 

/*document.execCommand('InsertInputHidden',false,"aa"); 

alert(document.all.aa.id);*/

//插入一個InputImage 

/*document.execCommand('InsertInputImage',false,"aa"); 

document.all.aa.src="F-a10.gif";*/

//插入一個Password 

//document.execCommand('InsertInputPassword',true,"aa"); 

//插入一個Radio 
HtmlEditor.prototype.insertRadio = function() {
    var document = this.getEditorDocument();
    document.parentWindow.focus();

    document.execCommand('InsertInputRadio', true);
}
//插入一個Reset 

//document.execCommand('InsertInputReset',true,"aa"); 

//插入一個Submit 

//document.execCommand('InsertInputSubmit',false,"aa"); 

//插入一個input text
HtmlEditor.prototype.insertInput = function() {
    var document = this.getEditorDocument();
    document.parentWindow.focus();

    document.execCommand('InsertInputText', true);
}

//插入一個textarea 
HtmlEditor.prototype.insertText = function() {
    var document = this.getEditorDocument();
    document.parentWindow.focus();

    document.execCommand('InsertTextArea', true);
}

//插入一個 select list box
HtmlEditor.prototype.insertListBox = function() {
    var document = this.getEditorDocument();
    document.parentWindow.focus();

    document.execCommand('InsertSelectListbox', true);
}

//插入一個single select
HtmlEditor.prototype.insertSelect = function() {
    var document = this.getEditorDocument();
    document.parentWindow.focus();
    document.execCommand('InsertSelectDropdown', true);
}
//插入一個line break(硬回車??) 

//document.execCommand('InsertParagraph');


//重设为一个fieldset
HtmlEditor.prototype.insertFieldSet = function() {
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


/**
* WBDL Editoer 
* $impport tree.js
* @base tree.js
*/

//常数
var WbdlEditorConst = {
    DEF_ATTR_CODE: "default"
}


WbdlEditor.prototype = new HtmlEditor();
WbdlEditor.prototype.superMethod = function(methodName, args) { HtmlEditor.prototype[methodName].call(this, args); };

function WbdlEditor(editorWindow) {


    HtmlEditor.call(this, editorWindow);
    var _this = this;
    var _dataPropertyForm = null;


    this.wbdl = null;
    this.wbdlFullId = "";
    this.isNewWbdl = true;

    this._Events.onWbdlChange = [];
    this._Events.onLoadWbdl = [];

    this.setDataPropertyForm = function(form) {
        _dataPropertyForm = form;
        form.onDataChange = function(changedElement) {
            _this.doDataFormDataChange(changedElement);
        }
        _this.initDataForm();

    }

    this.getDataPropertyForm = function(form) {
        return _dataPropertyForm;
    }

    this.requestWbdl();
}


//事件句柄

//动态方法

WbdlEditor.prototype.onWbdlChange = function() { }

WbdlEditor.prototype.doWbdlChange = function() {
    this.notifyEvent(onWbdlChange, this.wbdl);
    if (this.onWbdlChange) this.onWbdlChange();
    if (this.onReadyData) this.onReadyData(this.wbdl);
}


WbdlEditor.prototype.doDataFormDataChange = function(changedElement) {
    var wbdl = this.wbdl;
    var el = this.activeElement;
    var form = this.getDataPropertyForm();

    if (!form) return;
    if (!el) return;

    if (el.nodeName == "BODY") {
        alert("当前没有选中的对象，不能进行数据捆绑");
        return;
    }

    if (!el.name || el.name == undefined || el.name == "") {
        alert("当前选中的对象没有设置名称(name)，不能进行数据捆绑");
        return;
    }

    var w_ctr = null;

    if (wbdl)
        w_ctr = new WbdlXdk(wbdl);
    else {
        w_ctr = new WbdlXdk();
        wbdl = w_ctrl.wbdl;
        this.wbdl = wbdl;
        this.isNewWbdl = true;
    }



    var attr = form.getDataValue("cmbAttr");



    var elementName = el.name;

    var ed = w_ctr.getElementData(elementName, attr);

    if (changedElement.id == "cmbAttr") {
        if (ed != null) {
            form.setDataValue("cmbDsId", ed.DataSourceId);
            this.initForm_FieldId(ed.DataSourceId);
            form.setDataValue("cmbField", ed.DataField);
        } else {
            form.setDataValue("cmbDsId", "");
            form.setDataValue("cmbField", "");
        }
        return;
    }


    var dsId = form.getDataValue("cmbDsId");
    var fieldId = form.getDataValue("cmbField");

    if (changedElement.id == "cmbDsId") {
        this.initForm_FieldId(dsId);
    }

    if (dsId && dsId != "" && dsId != undefined && fieldId && fieldId != "" && fieldId != undefined) {
        if (!ed || ed == undefined) {
            ed = w_ctr.addElementData(elementName, attr);
        }
        ed.DataSourceId = dsId;
        ed.DataField = fieldId;
    } else {
        w_ctr.removeElementData(elementName, attr);
    }


    //ToDo：检查元素规范，是否合符DataListBind

    //  w_ctr.updateFieldBind(elementId, dataListBindId, tableId, fieldId);
    this.doWbdlChange();
}

WbdlEditor.prototype.onReadyData = function(wbdl) {

}

function getWbdlIdByUrl(url) {

    var extIndex = url.toLowerCase().lastIndexOf(".html");

    if (extIndex < 0)
        extIndex = url.toLowerCase().lastIndexOf(".htm");

    if (extIndex > 0)
        return url.substring(0, extIndex);
    else {
        alert("不能从url获得ID" + url);
        return;
    }

}

WbdlEditor.prototype.requestWbdl = function() {

    var id = getWbdlIdByUrl(this.url);
    status = this.url;



    var wbdl = XdkRequester.SynRequest(XdkRequester.RequestType.Schema, XdkRequester.OperationName.GetSchema, id, XdkRequester.ObjType.XForm, null, true);

    this.wbdlFullId = id;

    if (wbdl.ErrorNo && wbdl.ErrorNo != 0) {

        if (wbdl.ErrorMessage.indexOf("未能找到路径") > -1 || wbdl.ErrorMessage.indexOf("未能找到文件") > -1) {
            var wbdlCtrler = new WbdlXdk();
            this.wbdl = wbdlCtrler.getWbdl();
            this.wbdl.Id = id.substring(id.lastIndexOf("/") + 1, id.length);
            this.isNewWbdl = true;
            if (this.onReadyData)
                this.onReadyData(this.wbdl);
        } else {
            alert(wbdl.ErrorMessage);
        };

    }
    else {
        this.wbdl = wbdl;

        this.isNewWbdl = false;

        if (this.onReadyData) {
            this.onReadyData(this.wbdl);
          
            this.notifyEvent("onLoadWbdl", this.wbdl);
        }
    }
};

WbdlEditor.prototype.doChangeUrl = function(url) {
    this.superMethod("doChangeUrl", url);
    this.requestWbdl();
    this.initDataForm();
    //   this.initEventForm();
};


/**
* 初始化数据属性页面
* @Method  InitForm
* @return void
*/
WbdlEditor.prototype.initDataForm = function() {
    var form = this.getDataPropertyForm();
    if (!form) return;

    //初始数据源列表    
    if (this.wbdl) {

        var elDsId = form.getDataElement("cmbDsId");
        var wbdlXdk = new WbdlXdk(this.wbdl);
        var dsList = wbdlXdk.getDataSources();

        elDsId.options.length = 0;

        var option = new Option("", "");
        elDsId.options.add(option);

        for (var i = 0; i < dsList.length; i++) {
            var ds = dsList[i];
            var option = new Option(ds.Title, ds.Id);
            elDsId.options.add(option);
        }
    }



    //初始字段下拉选项
    this.initForm_FieldId(elDsId.value);


    //初始化元素属性选项
    var elAttr = form.getDataElement("cmbAttr");

    elAttr.options.length = 0;

    var option = new Option("默认属性", "");
    elAttr.options.add(option);

    var option = new Option("innerHtml", "innerHtml");
    elAttr.options.add(option);

    var option = new Option("value", "value");
    elAttr.options.add(option);

    var option = new Option("src", "src");
    elAttr.options.add(option);

    var option = new Option("href", "href");
    elAttr.options.add(option);

    var option = new Option("背景", "style_attr_backgroundImage");
    elAttr.options.add(option);

    form.Inited = true;
};

WbdlEditor.prototype.initForm_FieldId = function(dsId) {
    var form = this.getDataPropertyForm();
    if (!form) return;

    var elField = form.getDataElement("cmbField");
    elField.options.length = 0;
    elField.options.add(new Option("", ""));

    //    if (!tableId || tableId == undefined || tableId == "") {
    //        tableId = form.getDataElement("TableId").value;
    //    }

    if (!dsId || dsId == undefined || dsId == "") return;

    var wbdlXdk = new WbdlXdk(this.wbdl);
    //    var dsList = wbdlXdk.getDataSources();

    var fieldInfos = wbdlXdk.getDataSourceFieldInfos(dsId);

    for (var i = 0; i < fieldInfos.length; i++) {
        var fieldInfo = fieldInfos[i];
        var option = new Option(fieldInfo.Title, fieldInfo.Name);
        elField.options.add(option);
    }

    //  ---获取FieldSchema信息
    //    var table = XDataDkRequester.getTableInfo(tableId);


    //    for (var i = 0; i < table.Fields.length; i++) {
    //        var fld = table.Fields[i];
    //        var title = fld.Title;
    //        if (!title || title == "") title = fld.Id;
    //        var option = new Option(title, fld.Id);
    //        fieldIdElement.options.add(option);
    //    }
};

WbdlEditor.prototype.post = function(fileName) {

    if (!fileName || fileName == undefined || fileName == "")
        fileName = this.wbdlFullId;
    else {
        this.isNewWbdl = true;
        fileName = getWbdlIdByUrl(fileName);
    }
    if (this.isNewWbdl) {
        var saveObj = XdkRequester.SynRequest(
                 XdkRequester.RequestType.Schema,
                 XdkRequester.OperationName.AddSchema,
                 fileName,
                 XdkRequester.ObjType.XForm,
                 this.wbdl
        );
        if (saveObj) this.isNewWbdl = false;
    }
    else {
        XdkRequester.SynRequest(
                 XdkRequester.RequestType.Schema,
                 XdkRequester.OperationName.UpdateSchema,
                 this.wbdlFullId,
                 XdkRequester.ObjType.XForm,
                 this.wbdl
        );
    }
};
/*
WbdlEditor.prototype.setEventPropertyForm = function(form) {
//  this.eventPropertyForm = form;
//    this.initEventForm();
//   var _this = this;
//    form.onDataChange = function(changedElement) {
//        _this.doEventFormDataChange(changedElement);
//   }
};
*/
WbdlEditor.prototype.doEventFormDataChange = function(changedElement) {
    var wbdl = this.wbdl;
    var el = this.activeElement;
    var form = this.eventPropertyForm;

    //   debugger;

    if (!form) return;
    if (!el) return;

    if (el.nodeName == "BODY") {
        alert("当前没有选中的对象，不能进行数据捆绑");
        return;
    }

    if (!el.id || el.id == "" || el.id == undefined) {
        alert("当前选中的对象没有设置ID，不能进行数据捆绑");
        return;
    }



    var actionId = form.getDataValue("ActionId");
    var eventName = form.getDataValue("EventName");
    var elementId = el.id;

    //  if (!actionId || actionId == undefined || actionId == "") return;
    //  if (!eventName || eventName == undefined || eventName == "") return;

    var w_ctr = null;

    if (wbdl)
        w_ctr = new WbdlXdk(wbdl);
    else {
        w_ctr = new WbdlXdk();
        wbdl = w_ctrl.wbdl;
        this.isNewWbdl = true;
        this.wbdl = wbdl;
    }

    //ToDo：检查元素规范，是否合符DataListBind

    w_ctr.updateEventBind(elementId, actionId, eventName);
    this.doWbdlChange();

};
/*
WbdlEditor.prototype.initEventForm = function() {
var wbdl = this.wbdl;
var form = this.eventPropertyForm;
if (!form) return
if (!wbdl) return

var actionIdElement = form.getDataElement("ActionId");
actionIdElement.options.length = 0;
actionIdElement.options.add(new Option("", ""));

var wbdlCtrl = new WbdlXdk(wbdl);
wbdlCtrl.forEachAction(function(action) {
var title = action.Title;
if (!title || title == undefined || title == "") title = action.Id;
actionIdElement.options.add(new Option(title, action.Id));
});

}
*/

//覆盖方法
WbdlEditor.prototype.doElementPropertyChange = function(element, propertyName, propertyValue, oldvalue) {
    this.superMethod("doElementPropertyChange", arguments);

    var wbdlChanged = false;
    if (!this.wbdl) throw new Error("editorDoc.wbdl is null");

    var wbdlXdk = new WbdlXdk(this.wbdl);
    if (propertyName == "id") {
        if (WbdlXdk.isDataBindElement(element)) {
            var fieldBind = wbdlXdk.getFieldBind(oldvalue);
            if (fieldBind) {
                fieldBind.Id = element.id;
                wbdlChanged = true;
            }
        }
        else {
            var dataListBind = wbdlXdk.getDataListBind(oldvalue);
            if (dataListBind) {
                dataListBind.Id = element.id;
                wbdlChanged = true;
            }
        }

    }

    if (propertyName == "dataRow" && element.nodeName == "TABLE") {
        var dataListBind = wbdlXdk.getDataListBind(element.id);
        if (dataListBind) {
            var dataRow = DomUtils.getElementAttr(element, "dataRow");

            if (!dataListBind.DataRow || dataListBind.DataRow != dataRow) {
                dataListBind.DataRow = dataRow;
                wbdlChanged = true;

            }
        }
    }

    if (wbdlChanged) this.doWbdlChange();
}

WbdlEditor.prototype.doSelected = function(sel) {

    this.superMethod("doSelected", sel);

    var form = this.getDataPropertyForm();

    if (!form) return;
    if (!form.Inited) this.initDataForm();
    //  if (this.eventPropertyForm && !this.eventPropertyForm.Inited) this.initEventForm();
    if (!this.wbdl) return;


    if (sel.nodeName == "BODY")
        form.setEnabled(false);
    else
        form.setEnabled(true);


    var wbdlXdk = new WbdlXdk(this.wbdl);


    //显示数据属性
    var elemenData = null;
    if (sel.name && sel.name != undefined && sel.name != "") {
        elemenData = wbdlXdk.getElementDataByName(sel.name);
    }

    if (elemenData != null) {
        form.setDataValue("cmbDsId", elemenData.DataSourceId);
        this.initForm_FieldId(elemenData.DataSourceId);
        form.setDataValue("cmbField", elemenData.DataField);
        form.setDataValue("cmbAttr", wbdlXdk.getElementDataAttr(elemenData));
    } else {
        form.setDataValue("cmbDsId", "");
        form.setDataValue("cmbField", "");
        form.setDataValue("cmbAttr", "");
    }





    //显示事件属性
    /*
    var selId = sel.id;
    var eventBind = wbdlXdk.getEventBind(sel.id);
    if (eventBind) {
    this.eventPropertyForm.setDataValue("EventName", eventBind.EventName);
    this.eventPropertyForm.setDataValue("ActionId", eventBind.ActionId);
    } else {
    this.eventPropertyForm.setDataValue("EventName", "");
    this.eventPropertyForm.setDataValue("ActionId", "");
    }

    //   if (changed) this.doWbdlChange();
    */
};

WbdlEditor.prototype.appendDataSource = function(dataSource) {
    var wxdk = new WbdlXdk(this.wbdl);
    if (wxdk.appendDataSource(dataSource)) {
        this.initDataForm();
        return true;
    }
    return false;
}

WbdlEditor.prototype.getDataSources = function() {
    var wbdlXdk = new WbdlXdk(this.wbdl);
    return dsList = wbdlXdk.getDataSources();
}

WbdlEditor.prototype.getGetDataFieldInfos = function(dataSourceId) {
    var wbdlXdk = new WbdlXdk(this.wbdl);
    return wbdlXdk.getDataSourceFieldInfos(dataSourceId);
}

WbdlEditor.prototype.addElementData = function(elementName, elementAttr, param, dataSourceId, dataField, value) {
    var wbdlXdk = new WbdlXdk(this.wbdl);
    return wbdlXdk.addElementData(elementName, elementAttr, param, dataSourceId, dataField, value);
}


WbdlEditor.prototype.removeElementDataById = function(elementBindId) {
    var wbdlXdk = new WbdlXdk(this.wbdl);
    return wbdlXdk.removeElementDataById(elementBindId);
}

WbdlEditor.prototype.getXdk = function() {
    return (new WbdlXdk(this.wbdl));
}