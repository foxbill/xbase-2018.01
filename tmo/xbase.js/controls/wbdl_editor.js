
/**
* WBDL Editoer
* @summary 用于编辑WBDL文件 
* @impport tree.js
* @impport /xdk/xdk4js.js
* @base /contorls/html_Editor.js
*/

//常数
var WbdlEditorConst = {
    DEF_ATTR_CODE: "default"
}


WbdlEditor.constructor = WbdlEditor;
WbdlEditor.prototype = new HtmlEditor();
WbdlEditor.prototype.superMethod = function(methodName, args) { HtmlEditor.prototype[methodName].call(this, args); };

function WbdlEditor(editorWindow, elEditToolBox) {
    HtmlEditor.call(this, editorWindow,elEditToolBox);
    var _this = this;

    var _dataPropertyForm = null;


    this.wbdl = new Object();
    this.wbdlFullId = "";
    this.isNewWbdl = true;

    this._Events.onWbdlChange = [];
    this._Events.onLoadWbdl = [];
    this.event.onWbdlChange = "onWbdlChange";


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

    this.doChangeUrl = function(url) {
        this.superMethod("doChangeUrl", url);
        this.requestWbdl();
        this.initDataForm();
        //   this.initEventForm();
    };

    this.getPath = function() {
        var doc = this.getEditorDocument();
        return doc.location.pathname;
    }

    //    this.requestWbdl();
}


//事件句柄

//动态方法

WbdlEditor.prototype.onWbdlChange = function() { }

WbdlEditor.prototype.doWbdlChange = function() {
    this.notifyEvent(this.event.onWbdlChange, this.wbdl);
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

    var ed = null;
    var isStdName = false;
    var oldName = "";
    if (elementName)
        ed = w_ctr.getElementData(elementName, attr);
    if (ed) {
        oldName = (ed.DataSourceId + "_" + ed.DataField).replace(/\./g, "_");
        isStdName = elementName == oldName;
    }

    //元素捆绑属性改变
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
        if (!elementName || isStdName) {
            elementName = (dsId + "_" + fieldId).replace(/\./g, "_"); ;
            el.name = elementName;
            el.id = elementName;
            this.elementPropertyList.setProperty("name", elementName);
            this.elementPropertyList.setProperty("id", elementName);
        }

        if (isStdName) {
            w_ctr.removeElementData(oldName, attr);
        }

        //        if (!ed || ed == undefined) {
        ed = w_ctr.addElementData(elementName, attr);
        //        }
        //        ed.Id = elementName;
        ed.DataSourceId = dsId;
        ed.DataField = fieldId;
    } else if (elementName) {
        w_ctr.removeElementData(elementName, attr);
        if (isStdName) {
            el.name = "";
            el.id = "";
            this.elementPropertyList.setProperty("name", "");
            this.elementPropertyList.setProperty("id", "");
        }

    }


    //ToDo：检查元素规范，是否合符DataListBind

    //  w_ctr.updateFieldBind(elementId, dataListBindId, tableId, fieldId);
    this.doWbdlChange();
}

WbdlEditor.prototype.onReadyData = function(wbdl) {

}

WbdlEditor.prototype.getWbdlId = function(url) {

    var extIndex = url.toLowerCase().lastIndexOf(".html");

    if (extIndex < 0)
        extIndex = url.toLowerCase().lastIndexOf(".htm");

    if (extIndex > 0)
        return url.substring(0, extIndex);
    else {
        return url;
    }

}

WbdlEditor.prototype.requestWbdl = function() {

    var url = this.getPath();
    var id = this.getWbdlId(url);
    status = url;


    var req = new JoapRequest("SiteAdmin", "", "getWbdl");
    req.addParamate("pageFullName", url);
    var wbdl = req.invoke();

    this.wbdlFullId = id;

    this.wbdl = wbdl;
    this.isNewWbdl = false;

    if (this.onReadyData) {
        this.onReadyData(this.wbdl);
    }
    
    this.notifyEvent("onLoadWbdl", this.wbdl);
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
    var elDsId = form.getDataElement("cmbDsId");
    if (this.wbdl) {

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
        fileName = this.getWbdlId(fileName);
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


