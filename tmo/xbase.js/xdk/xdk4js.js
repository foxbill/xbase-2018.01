LunguageMapId = {
    CN: 0, //
    CShap: 1,
    DB: 2,
    SQL: 3
};



ValidMethods =
{
    StringLengthValidator: {
        title: "字符串长度验证",
        params: [
          { name: "最小长度", TypeTitle: "数字", type: "System.Int32, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" },
          { name: "最大长度", TypeTitle: "数字", type: "System.Int32, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" }
          ]
    },
    RangeValidator: {
        title: "取值范围验证",
        params: [
          { name: "最小长度", TypeTitle: "数字", type: "System.Int32, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" },
          { name: "最大长度", TypeTitle: "数字", type: "System.Int32, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" }
          ]
    },
    NotNullValidator: {
        title: "非空验证",
        params: [
          { name: "验证失败信息", TypeTitle: "字符串", type: "System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" }
          ]
    }
}


DataTypeMap = {
    t1: ["字符", "System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", "System.Data.DbType.String", "vChar"],
    t3: ["整数", "System.Int32, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", "System.Data.DbType.Int32", "int"],
    t2: ["小数", "System.Decimal, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", "System.Data.DbType.Decimal", "decimal"],
    t8: ["货币", "System.Single, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", "System.Data.DbType.Single", "money"],
    t4: ["日期", "System.DateTime, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", "System.Data.DbType.DateTime", "datetime"],
    t5: ["二进制", "System.Byte, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", "System.Data.DbType.Binary", "binary"],
    t6: ["图片", "System.Object, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", "System.Data.DbType.Binary", "image"],
    t7: ["Office文件", "System.Object, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", "System.Data.DbType.Binary", "binary"]
}


//实体类型

$TableInfo = {
    "Name": "NewDataSet",
    "FieldInfo": [
        {
            "MaxLength": "-1",
            "DataType": "System.String", "IsNull": "True", "Name": "MZH"
        },
        {
            "MaxLength": "-1", "DataType": "System.String", "IsNull": "True", "Name": "HZXM"
        }
     ]
}






$ReturnObject = {
    "References": [],
    "LookupFields": [],
    "KeyField": "XH",
    "InsertCommand": "",
    "UpdateCommand": "",
    "DeleteCommand": "",
    "SelectCommand": " SELECT XH, CZY, KSRQ, JSRQ, KSPH1, JSPH1, KSPH2, JSPH2, GHSL, (GHJE + ZCJE + QTJE) as sxjje, THSL, (THJE + TZCJE +TQTJE) as txjje, JSBS, BDZS, ZPZS, ZPJE, XJZS, XJJE FROM MGHYHZ WHERE CZY = @CZY AND JSBS IS NULL ",
    "Fields": [
          { "Valids": [],
              "DataType": null,
              "DisplayName": "",
              "Visibility": "0",
              "EditFormat": ".*",
              "CodeTableName": "",
              "DefaultValue": "",
              "Id": "XH"
          },
          {
              "Valids": [],
              "DataType": null,
              "DisplayName": "",
              "Visibility": "",
              "EditFormat": ".*",
              "CodeTableName": "",
              "DefaultValue": "",
              "Id": "CZY"
          },
          {
              "Valids": [],
              "DataType": null,
              "DisplayName": "",
              "Visibility": "",
              "EditFormat": ".*",
              "CodeTableName": "",
              "DefaultValue": "",
              "Id": "KSRQ"
          },
                     {
                         "Valids": [],
                         "DataType": null,
                         "DisplayName": "",
                         "Visibility": "",
                         "EditFormat": ".*",
                         "CodeTableName": "",
                         "DefaultValue": "",
                         "Id": "JSRQ"
                     },
                             {
                                 "Valids": [],
                                 "DataType": null,
                                 "DisplayName": "",
                                 "Visibility": "",
                                 "EditFormat": ".*",
                                 "CodeTableName": "",
                                 "DefaultValue": "",
                                 "Id": "KSPH1"
                             },
                                   {
                                       "Valids": [],
                                       "DataType": null,
                                       "DisplayName": "",
                                       "Visibility": "",
                                       "EditFormat": ".*",
                                       "CodeTableName": "",
                                       "DefaultValue": "",
                                       "Id": "JSPH1"
                                   },
                                       {
                                           "Valids": [],
                                           "DataType": null,
                                           "DisplayName": "",
                                           "Visibility": "",
                                           "EditFormat": ".*",
                                           "CodeTableName": "",
                                           "DefaultValue": "",
                                           "Id": "KSPH2"
                                       }, { "Valids": [], "DataType": null, "DisplayName": "", "Visibility": "", "EditFormat": ".*", "CodeTableName": "", "DefaultValue": "", "Id": "JSPH2" }, { "Valids": [], "DataType": null, "DisplayName": "", "Visibility": "", "EditFormat": ".*", "CodeTableName": "", "DefaultValue": "0", "Id": "GHSL" }, { "Valids": [], "DataType": null, "DisplayName": "", "Visibility": "", "EditFormat": ".*", "CodeTableName": "", "DefaultValue": "0", "Id": "GHJE" }, { "Valids": [], "DataType": null, "DisplayName": "", "Visibility": "", "EditFormat": ".*", "CodeTableName": "", "DefaultValue": "0", "Id": "ZCJE" }, { "Valids": [], "DataType": null, "DisplayName": "", "Visibility": "", "EditFormat": ".*", "CodeTableName": "", "DefaultValue": "0", "Id": "THSL" }, { "Valids": [], "DataType": null, "DisplayName": "", "Visibility": "", "EditFormat": ".*", "CodeTableName": "", "DefaultValue": "0", "Id": "THJE" }, { "Valids": [], "DataType": null, "DisplayName": "", "Visibility": "", "EditFormat": ".*", "CodeTableName": "", "DefaultValue": "0", "Id": "TZCJE" }, { "Valids": [], "DataType": null, "DisplayName": "", "Visibility": "", "EditFormat": ".*", "CodeTableName": "", "DefaultValue": "", "Id": "JSBS" }, { "Valids": [], "DataType": null, "DisplayName": "", "Visibility": "", "EditFormat": ".*", "CodeTableName": "", "DefaultValue": "0", "Id": "BDZS" }, { "Valids": [], "DataType": null, "DisplayName": "", "Visibility": "", "EditFormat": ".*", "CodeTableName": "", "DefaultValue": "0", "Id": "ZPZS" }, { "Valids": [], "DataType": null, "DisplayName": "", "Visibility": "", "EditFormat": ".*", "CodeTableName": "", "DefaultValue": "0", "Id": "ZPJE" }, { "Valids": [], "DataType": null, "DisplayName": "", "Visibility": "", "EditFormat": ".*", "CodeTableName": "", "DefaultValue": "0", "Id": "XJZS" }, { "Valids": [], "DataType": null, "DisplayName": "", "Visibility": "", "EditFormat": ".*", "CodeTableName": "", "DefaultValue": "0", "Id": "XJJE" }, { "Valids": [], "DataType": null, "DisplayName": "", "Visibility": "", "EditFormat": ".*", "CodeTableName": "", "DefaultValue": "0", "Id": "QTJE" }, { "Valids": [], "DataType": null, "DisplayName": "", "Visibility": "", "EditFormat": ".*", "CodeTableName": "", "DefaultValue": "0", "Id": "TQTJE" }, { "Valids": [], "DataType": null, "DisplayName": "", "Visibility": "", "EditFormat": ".*", "CodeTableName": "", "DefaultValue": "0", "Id": "sxjje" }, { "Valids": [], "DataType": null, "DisplayName": "", "Visibility": "", "EditFormat": ".*", "CodeTableName": "", "DefaultValue": "0", "Id": "txjje"}],
    "Id": "mghyhz_ghgl"
}

$TreeNode = {
    ID: "", Name: "", Data: "dir", Nodes: []
}





var XdkRequester = {

    version: "1.0.0.0",

    RequestType: {
        Schema: "Schema",
        Database: "DataBase",
        IO: "IO",
        FileAdmin: "FileAdmin"
    },

    OperationName: {
        GetSchema: "GetSchema",
        AddSchema: "AddSchema",
        UpdateSchema: "UpdateSchema",
        DeleteSchema: "DeleteSchema",
        IsExist: "IsExist",
        GetIDs: "GetIDs",
        GetByObjectID: "GetByObjectID",
        GetAll: "GetAll",
        GetFolders: "GetFolders",
        GetIDByFolder: "GetIDByFolder",

        GetTableNames: "GetTableNames",
        GetTableInfo: "GetTableInfo",
        GetConnectionInfo: "GetConnectionInfo",
        SetConnectionInfo: "SetConnectionInfo",
        GetStoredProcedures: "GetStoredProcedures",
        GetViews: "GetViews",
        SearchTableInfo: "SearchTableInfo",
        GetDbExplorer: "GetDbExplorer",

        GetPathTree: "GetPathTree",

        SaveFile: "SaveFile"
    },

    ObjType: {
        XTable: "XTable",
        XForm: "XForm",
        ServerFunction: "ServerFunction",
        ServerObject: "ServerObject",
        ClientFunction: "ClientFunction",
        ClientObject: "ClientObject",
        Menu: "Menu",
        XTable: "XTable"
    },
    /**
    * @summary：同步请求
    * @example：XAjax.request("http:xxx?ss=ss",myFunction);
    * @Param1 requestType   :  请求类型 Schema,DataBase
    * @Param2 operationName :  操作名称，即函数的名称
    * @Param3 paramName     :  参数，即路径
    * @Param4 objType       :对象类型[XTable|| XForm||ServerFunction||ServerObject||ClientFunction||ClientObject|| Menu||XTable]
    **/
    SynRequest: function(requestType, operationName, paramName, objType, data, hideErrAlert) {

        var requestJson = {
            RequestType: requestType,
            ParamName: paramName,
            OperationName: operationName,
            ObjType: objType,
            Data: data
        }

        //        var url = "/xj-service/WbdlDesigner.aspx";
        var url = "wbdlDesigner.axd";

        var requestStr = XAjax.obj2str(requestJson);

        var strResponse = XAjax.SynRequest(url, requestStr);
        //  alert(strResponse);
        if (strResponse == null) return null;

        var response = eval("(" + strResponse + ")");

        if (response.ErrorNo != 0) {
            if (hideErrAlert)
                return response;
            else {
                alert(response.ErrorMessage);
                return null;
            }
        }
        // alert(response);

        if (response.ReturnObject == null)
            return 1;
        else
            return response.ReturnObject;

    },



    //测试用
    SynRequestTest: function(requestType, operationName, paramName, objType, data) {
        var requestJson = {
            RequestType: requestType,
            ParamName: paramName,
            OperationName: operationName,
            ObjType: objType,
            Data: data
        }

        var url = "Service.aspx";
        var requestStr = XAjax.obj2str(requestJson);

        var strResponse = XAjax.SynRequest(url, requestStr);
        return strResponse;

    }

}

var XDataDkRequester = {

    version: "1.0.0.0",
    /**
    * @summary：请求
    * @example：XAjax.request("http:xxx?ss=ss",myFunction);
    * @Param1 requestType   :  请求类型 Schema,DataBase
    * @Param2 operationName :  操作名称，即函数的名称
    * @Param3 paramName     :  参数，即路径
    * @Param4 objType       :对象类型[XTable|| XForm||ServerFunction||ServerObject||ClientFunction||ClientObject|| Menu||XTable]
    **/


    //获取xTableSchema信息
    getTableInfo: function(tableName) {
        return XdkRequester.SynRequest(XdkRequester.RequestType.Schema, XdkRequester.OperationName.GetSchema, tableName, XdkRequester.ObjType.XTable, null);
    },

    //保存xTableSchema信息
    SaveTableInfo: function(schema) {
        XdkRequester.SynRequest(XdkRequester.RequestType.Schema, XdkRequester.OperationName.UpdateSchema, schema.Id, XdkRequester.ObjType.XTable, schema);
    },

    //获取物理数据库Table信息
    getDatabaseTableInfo: function(tableName) {
        return XdkRequester.SynRequest(XdkRequester.RequestType.Database, XdkRequester.OperationName.GetTableInfo, tableName, null, null);
    },

    //设置连接
    SetConnectionInfo: function(connectionInfo) {
        XdkRequester.SynRequest(XdkRequester.RequestType.Database, XdkRequester.OperationName.SetConnectionInfo, null, null, connectionInfo);
    },

    //获取物理数据库执行SQL语句后得到的表的信息
    SearchTableInfo: function(sql) {
        return XdkRequester.SynRequest(XdkRequester.RequestType.Database, XdkRequester.OperationName.SearchTableInfo, sql, null, null);
    },

    //获取所以XTableSchema的Titles--返回hashTable格式,需要后台增加getTableTitles功能 以改善效率
    getTableTitles: function(folderId) {
        var ret = new Object();
        var tableNames = XdkRequester.SynRequest("Schema", "GetIDByFolder", "", "XTable", null);
        for (var i = 0; i < tableNames.length; i++) {
            var tableName = tableNames[i];
            var table = this.getTableInfo(tableName);
            var title = table.Title;
            if (!title || title == "") title = table.Id;
            ret[table.Id] = title;
        }
        return ret;
    },

    /** 
    * 获取物理数据库的，所以物理表的表名
    * @connId xDataBaseSchema的连接名，如果为空，则使用缺省连接
    * @return 返回格式为字符数组
    */
    getDatabaseTableNames: function(connId) {
        return XdkRequester.SynRequest(XdkRequester.RequestType.Database, XdkRequester.OperationName.GetTableNames, "", "", null);
    },

    getDatabaseViewNames: function(connId) {
        return XdkRequester.SynRequest(XdkRequester.RequestType.Database, XdkRequester.OperationName.GetViews, "", "", null);
    },

    /** 
    * 获取物理数据库 表和视图结构
    * @connId xDataBaseSchema的连接名，如果为空，则使用缺省连接
    * @return 返回格式为字符数组
    */
    getDatabaseSchema: function(connId) {
        return XdkRequester.SynRequest(XdkRequester.RequestType.Database, XdkRequester.OperationName.GetDbExplorer, "", "", null);
    }

}


function XTableAgent(XTable) {
    var _table = XTable;
    this.GetColumn = function(columnId) {
        for (var i = 0; i < _table.Fields.length; i++) {
            if (_table.Fields[i].Id != null && _table.Fields[i].Id == columnId) {
                return _table.Fields[i];
            }
        }
        return null;
    }
}



/***
*  WbdlXdk 类
*  用于实现wbdl的json对象的编辑工具
*  @Param wbdl： wbdl的json对象 
***/


/***
*     常数段
***/
NO_DATA_BINDS = ["TABLE", "BODY"];
WBDL_CNST = {
    DataElementIdParamMask: "_param_",
    DataElementIdAttrMask: "_attr_"
}

WbdlUtils = {
    decodeDataElementId: function(dataElementId) {

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
    encodeDataElementId: function(name, attr, param) {
        var s = name;

        if (attr != undefined && attr != null && attr != "")
            s += WBDL_CNST.DataElementIdAttrMask + attr.replace(".", WBDL_CNST.DataElementIdAttrMask);

        if (param != undefined && param != null && param != "")
            s += WBDL_CNST.DataElementIdParamMask + param;

        return s;
    },
    getDataElementDispName: function(dataElementId) {
        var dId = this.decodeDataElementId(dataElementId);
        var s = dId.name;

        if (dId.attr != undefined && dId.attr != null && dId.attr != "")
            s += "." + dId.attr;

        if (dId.param != undefined && dId.param != null && dId.param != "")
            s += "." + dId.param;

        return s
    }

}



function WbdlXdk(wbdl) {
    //常数


    var _this = this;

    var _wbdl = wbdl;

    if (!_wbdl) {
        _wbdl = new Object();
    }

    function initWbdl() {
        if (!_wbdl.Actions) _wbdl.Actions = [];
        if (!_wbdl.FieldBinds) _wbdl.FieldBinds = [];
        if (!_wbdl.Events) _wbdl.Events = [];
        if (!_wbdl.DataListBinds) _wbdl.DataListBinds = [];
    }

    this.getWbdl = function() {
        return _wbdl;
    };


    initWbdl();

}


WbdlXdk.isDataBindElement = function(element) {
    for (var i = 0; i < NO_DATA_BINDS.length; i++) {
        if (element.nodeName == NO_DATA_BINDS[i])
            return false;
    }
    return true;
}

WbdlXdk.prototype.newFieldBind = function(id, tableId, fieldId) {
    var o = new Object;
    o.FieldId = fieldId;
    o.TableId = tableId;
    o.Id = id;
    return o;
}


WbdlXdk.prototype.getDataListBindTitles = function() {

    var wbdl = this.getWbdl();
    var ret = new Object();

    if (wbdl) {
        for (var ui = 0; ui < wbdl.DataListBinds.length; ui++) {
            var datalist = wbdl.DataListBinds[ui];
            var title = datalist.Title;
            if (!title || title == "")
                title = datalist.Id;
            ret[datalist.Id] = title;
        }
        return ret;
    }

    return null;
}

WbdlXdk.prototype.getDataListBind = function(dataListBindId) {
    var wbdl = this.getWbdl();
    if (wbdl) {
        for (var ui = 0; ui < wbdl.DataListBinds.length; ui++) {

            var datalist = wbdl.DataListBinds[ui];
            if (datalist.Id == dataListBindId) return datalist;
        }
    }
    return null;
}


WbdlXdk.prototype.forEachFieldBind = function(fn) {

    var wbdl = this.getWbdl();

    if (wbdl.FieldBinds) {
        for (var i = 0; i < wbdl.FieldBinds.length; i++) {
            var fieldBind = wbdl.FieldBinds[i];
            if (fn(fieldBind, wbdl.FieldBinds, i, null))
                return fieldBind;

        }
    }

    if (wbdl.DataListBinds) {
        for (var ui = 0; ui < wbdl.DataListBinds.length; ui++) {

            var datalist = wbdl.DataListBinds[ui];

            for (var i = 0; i < datalist.Columns.length; i++) {
                var fieldBind = datalist.Columns[i];
                if (fn(fieldBind, datalist.Columns, i, datalist))
                    return fieldBind;
            }

        }
    }

    return null;

}

WbdlXdk.prototype.getFieldBindAndListBind = function(elementId) {
    var park = null;
    this.forEachFieldBind(function(fieldBind, inArray, atIndex, dataList) {
        if (fieldBind.Id == elementId) {
            park = new Object();
            park.fieldBind = fieldBind;
            park.dataListBind = dataList;
            return true;
        }
    });
    return park;
}


WbdlXdk.prototype.forEachAction = function(fn) {
    var wbdl = this.getWbdl();
    if (!wbdl.Actions || wbdl.Actions == undefined) return;
    for (var i = 0; i < wbdl.Actions.length; i++) {
        var action = wbdl.Actions[i];
        if (fn(action, i)) break;
    }
}

WbdlXdk.prototype.getAction = function(actionId) {
    var ret = null;

    this.forEachAction(function(action) {
        if (action.Id == actionId) {
            ret = action;
            return true;
        }
    });

    return ret;
}

WbdlXdk.prototype.addAction = function(newAction) {
    var wbdl = this.getWbdl();
    if (this.getAction(newAction.Id)) {
        alert("Action.Id重复");
        return false;
    }
    wbdl.Actions.push(newAction);
    return true;
}

WbdlXdk.prototype.replaceAction = function(oldAction, newAction) {
    var wbdl = this.getWbdl();
    var exists = false;
    var mIndex = -1;

    this.forEachAction(function(action, index) {
        if (action != oldAction && action.Id == newAction.Id) {
            exists = true;
            return true;
        }
        if (action == oldAction) {
            mIndex = index;
        }
    });

    if (exists) {
        alert("Action.Id重复");
        return false;
    }

    if (mIndex > -1) {
        wbdl.Actions[mIndex] = newAction;
    } else {
        wbdl.Actions.push(newAction);
    }
    return true;

}

/**** 事件管理部分 ****/


WbdlXdk.prototype.getEvent = function(elementName, eventName) {

    if (!elementName) return;

    var wbdl = this.getWbdl();

    for (var i = 0; i < wbdl.Events.length; i++) {
        var event = wbdl.Events[i];
        if (event.ElementName == elementName) {
            if (!eventName)
                return event;

            if (event.EventName == eventName)
                return event;
        }
    }
    return null;
}

WbdlXdk.prototype.removeEvent = function(elementName, eventName) {

    var wbdl = this.getWbdl();

    for (var i = 0; i < wbdl.Events.length; i++) {
        var event = wbdl.Events[i];
        if (event.ElementName == elementName && event.EventName == eventName) {
            if (wbdl.Events)
                wbdl.Events.splice(i, 1);
            return true;
        }
    }
    return false;
}

WbdlXdk.prototype.updateEvent = function(elementName, eventName, actionFlow) {

    if (!elementName || !eventName) return false;

    var wbdl = this.getWbdl();
    if (!wbdl.Events)
        wbdl.Events = [];

    var event = null;

    for (var i = 0; i < wbdl.Events.length; i++) {
        var ev = wbdl.Events[i];
        if (ev.ElementName == elementName && ev.EventName == eventName) {
            if (!actionFlow) {
                wbdl.Events.splice(i, 1);
                return;
            }
            event = ev;
            break;
        }
    }


    if (!event) {
        event = Object();

        wbdl.Events.push(event);
    }

    event.Id = elementName + "_" + eventName;
    event.ElementName = elementName;
    event.EventName = eventName;
    event.ActionFlow = actionFlow;

    return true;
}

/**
* 根据FlowId获取ActionFlow
*/
WbdlXdk.prototype.getActionFlow = function(flowId) {
    var wbdl = this.getWbdl();
    if (!wbdl.ActionFlows)
        return null;
    for (var i = 0; i < wbdl.ActionFlows.length; i++) {
        var flow = wbdl.ActionFlows[i];
        if (flow.Id == flowId)
            return flow;
    }
    return null;
}

WbdlXdk.prototype.appendActionFlow = function(actionFlow) {
    if (!actionFlow) return;
    var wbdl = this.getWbdl();

    if (!actionFlow.Id) {
        alert("任务流程对象必须包含一个Id");
        return;
    }
    var flowId = actionFlow.Id;
    if (!wbdl.ActionFlows)
        wbdl.ActionFlows = [];
    for (var i = 0; i < wbdl.ActionFlows.length; i++) {
        var flow = wbdl.ActionFlows[i];
        if (flow.Id == flowId) {
            alert("任务流程Id已经存在，不能追加这个任务");
            return;
        }
    }
    wbdl.ActionFlows.push(actionFlow);
}

WbdlXdk.prototype.removeFieldBind = function(elementId) {
    this.forEachFieldBind(function(fieldBind, bindSet, bindIndex, bindDataList) {
        if (fieldBind.Id == elementId) {
            bindSet.splice(bindIndex, 1);
        }
    });
}

WbdlXdk.prototype.getTopFieldBind = function(elementId) {
    var ret = null;

    this.forEachFieldBind(function(fieldBind, bindSet, bindIndex, bindDataList) {
        if (bindDataList && bindDataList != undefined) {
            ret = null;
            return true;
        }
        if (fieldBind.Id == elementId) {
            ret = fieldBind;
            return true;
        }
    });

    return ret;
}



WbdlXdk.prototype.getFieldBind = function(elementId) {
    var ret = null;

    this.forEachFieldBind(function(fieldBind, bindSet, bindIndex, bindDataList) {
        if (fieldBind.Id == elementId) {
            ret = fieldBind;
            return true;
        }
    });

    return ret;
}

WbdlXdk.prototype.updateFieldBind = function(elementId, dataListBindId, tableId, fieldId) {
    var wbdl = this.getWbdl();

    var updated = false;

    this.removeFieldBind(elementId);

    if (!tableId || tableId == undefined || tableId == "") return;
    if (!fieldId || fieldId == undefined || fieldId == "") return;

    if (!dataListBindId || dataListBindId == undefined || dataListBindId == "") {
        wbdl.FieldBinds.push(this.newFieldBind(elementId, tableId, fieldId));
        return null;
    }


    var dataList = this.getDataListBind(dataListBindId);

    if (!dataList) {
        dataList = new Object();
        dataList.Columns = [];
        dataList.Id = dataListBindId;
        dataList.Title = dataListBindId;
        wbdl.DataListBinds.push(dataList);
    }
    dataList.Columns.push(this.newFieldBind(elementId, tableId, fieldId));
    return dataList;
}


//数据源操作
WbdlXdk.prototype.appendTable = function(tableId) {
    var wbdl = this.getWbdl();
    var dataSource = wbdl.DataSource;

    if (!dataSource || dataSource == undefined) {
        dataSource = new Object();
        wbdl.DataSource = dataSource;
    }

    if (!dataSource.Tables || dataSource.Tables == undefined) {
        dataSource.Tables = [];
    }

    var table = null;

    for (var i = 0; i < dataSource.Tables.length; i++) {
        var tb = dataSource.Tables[i];
        if (tb.Id == tableId) {
            table = tb;
            return;
        }
    }

    table = new Object();
    table.Id = tableId;
    table.Name = tableId;
    dataSource.Tables.push(table);

}

WbdlXdk.prototype.appendTables = function(tableIds) {
    if (!tableIds || tableIds == undefined) return;
    if (!tableIds.length) return;
    for (var i = 0; i < tableIds.length; i++) {
        var tableId = tableIds[i];
        this.appendTable(tableId);
    }
}

WbdlXdk.prototype.deleteTable = function(tableId) {
    var wbdl = this.getWbdl();
    var dataSource = wbdl.DataSource;

    if (!dataSource || dataSource == undefined)
        return;

    if (!dataSource.Tables || dataSource.Tables == undefined)
        return;

    var table = null;
    for (var i = 0; i < dataSource.Tables.length; i++) {
        var tb = dataSource.Tables[i];
        if (tb.Id == tableId) {
            dataSource.Tables.splice(i, 1);
            return;
        }
    }
}

WbdlXdk.prototype.getDataSources = function() {
    var wbdl = this.getWbdl();
    if (!wbdl.DataSources || wbdl.DataSources == undefined) {
        wbdl.DataSources = [];
    }
    return wbdl.DataSources;
}


WbdlXdk.prototype.containsDataSource = function(dataSource) {

    var datas = this.getDataSources();

    for (var i = 0; i < datas.length; i++) {
        var adata = datas[i];
        if (adata.Id.toUpperCase() == dataSource.Id.toUpperCase())
            return true;
    }

    return false;
}


WbdlXdk.prototype.appendDataSource = function(dataSource) {

    if (this.containsDataSource(dataSource)) {
        alert("数据源：" + dataSource.Id + ",已经存在，请重新输入");
        return false;
    }

    var datas = this.getDataSources();
    datas.push(dataSource);
    return true;
}

WbdlXdk.prototype.getDataSource = function(dataSourceId) {
    var datas = this.getDataSources();

    for (var i = 0; i < datas.length; i++) {
        var adata = datas[i];
        if (adata.Id.toUpperCase() == dataSourceId.toUpperCase())
            return adata;
    }
    return null;
}

WbdlXdk.prototype.getDataSourceFieldInfos = function(dataSourceId) {
    var ds = this.getDataSource(dataSourceId);
    var ret = [];
    if (!ds || ds == undefined) return ret;
    var jr = new JoapRequest(ds.DataType, ds.Name, "GetFieldInfos");
    ret = jr.invoke();
    return ret;
}

WbdlXdk.prototype.getElementDatas = function() {
    var wbdl = this.getWbdl();

    if (!wbdl.ElementDatas || wbdl.ElementDatas == undefined)
        wbdl.ElementDatas = [];

    return wbdl.ElementDatas

}

WbdlXdk.prototype.getElementDataById = function(dataElementId) {
    var elementDatas = this.getElementDatas();
    for (var i = 0; i < elementDatas.length; i++) {
        var item = elementDatas[i];
        if (item.Id.toUpperCase() == dataElementId.toUpperCase())
            return item;
    }
    return null;
}

WbdlXdk.prototype.getElementData = function(elementName, elementAttr, param) {
    var id = WbdlUtils.encodeDataElementId(elementName, elementAttr, param);
    return this.getElementDataById(id);
}


WbdlXdk.prototype.addElementData = function(elementName, elementAttr, param, dataSourceId, dataField, value) {

    var ed = this.getElementData(elementName, elementAttr, param);

    if (ed == null) {
        ed = new Object();
        ed.Id = WbdlUtils.encodeDataElementId(elementName, elementAttr, param);
        var elementDatas = this.getElementDatas();
        elementDatas.push(ed);
    }
    if (dataSourceId != undefined)
        ed.DataSourceId = dataSourceId;
    if (dataField != undefined)
        ed.DataField = dataField;
    if (value != undefined)
        ed.Value = value;
    return ed;
}


WbdlXdk.prototype.removeElementDataById = function(elementBindId) {
    var elementDatas = this.getElementDatas();
    for (var i = elementDatas.length - 1; i > -1; i--) {
        var item = elementDatas[i];
        if (item.Id.toUpperCase() == elementBindId.toUpperCase()) {
            elementDatas.splice(i, 1);
            break;
        }
    }
}

WbdlXdk.prototype.removeElementData = function(elementName, elementAttr, param) {

    var id = WbdlUtils.encodeDataElementId(elementName, elementAttr, param);
    this.removeElementDataById(id);

}


WbdlXdk.prototype.getElementDataAttr = function(elementData) {
    var id = elementData.Id;
    var i = id.indexOf(WBDL_CNST.DataElementIdAttrMask);

    if (i < 0)
        return "";

    i += WBDL_CNST.DataElementIdAttrMask.length;

    return id.substr(i);
}


WbdlXdk.prototype.getElementDataByName = function(elementName) {
    var elementDatas = this.getElementDatas();

    for (var i = 0; i < elementDatas.length; i++) {
        var item = elementDatas[i];
        var str = (elementName + WBDL_CNST.DataElementIdAttrMask).toUpperCase();
        if (item.Id.toUpperCase().indexOf(str, 0) == 0)
            return item;
        if (item.Id.toUpperCase() == elementName.toUpperCase())
            return item;
    }

    return null;
}

function ActionXdk(action) {
    if (!action) {
        action = new Object();
        action.Id = "new Action";
        action.Title = "新活动";
        action.Methods = [];
    }
    this.action = action;
}

ActionXdk.prototype.forEachMethod = function(fn) {
    if (!this.action || this.action == undefined) return;
    if (!this.action.Methods || this.action.Methods == undefined) return;

    for (var i = 0; i < this.action.Methods.length; i++) {
        var method = this.action.Methods[i];
        if (fn(method)) break;
    }
}

ActionXdk.prototype.createMethod = function(methodName) {
    var methodDef = XdkRequester.SynRequest(XdkRequester.RequestType.Schema, XdkRequester.OperationName.GetSchema, methodName, XdkRequester.ObjType.ServerFunction, null);
    var title = methodDef.Title;

    if (!title || title == undefined || title == "")
        title = methodDef.Id;

    var method = new Object();

    method.Id = "1";
    method.MethodName = methodDef.Id;
    method.Title = title;
    method.Parameters = [];

    if (methodDef.ParamDefs) {
        for (var i = 0; i < methodDef.ParamDefs.length; i++) {
            var paramDef = methodDef.ParamDefs[i];
            var param = new Object();
            param.Id = paramDef.ParamName;
            var title = paramDef.Title;
            if (!title || title == undefined || title == "")
                title = paramDef.ParamName;
            param.Title = title;
            param.Value = "";
            method.Parameters.push(param);
        }
    }
    this.action.Methods.push(method);
    return method;
}

ActionXdk.prototype.checkMethodIds = function() {
    var ret = true;

    var list = new Object();

    this.forEachMethod(function(method) {
        if (!method.Id || method.Id == undefined || method.Id == "") {
            ret = false;
            return true;
        }
        if (list[method.Id] == method.Id) {
            ret = false;
            return true;
        }
        list[method.Id] = method.Id;
    });

    return ret;
}


function MethodXdk(method) {
    this.method = method;
}

MethodXdk.prototype.checkInit = function() {
    if (!this.method) alert("method not Initual");
}

MethodXdk.prototype.forEachParam = function(fn) {
    this.checkInit();
    if (!this.method.Parameters || this.method.Parameters == undefined)
        return;
    for (var i = 0; i < this.method.Parameters.length; i++) {
        var parameter = this.method.Parameters[i];
        if (fn(parameter)) break;
    }
}

MethodXdk.prototype.getParameter = function(parameterId) {
    var ret = null;
    this.forEachParam(function(parameter) {
        if (parameter.Id == parameterId) {
            ret = parameter;
            return true;
        }
    });
    return ret;
}

MethodXdk.prototype.setParameterValue = function(parameterId, parameterValue) {
    var param = this.getParameter(parameterId);
    if (param) param.Value = parameterValue;
}

DataListBindXdk = function(dataListBind) {
    this.dataListBind = dataListBind;
}

DataListBindXdk.prototype.getFieldBind = function(elementId) {
    var lb = this.dataListBind;
    if (!lb || !lb.Columns || !lb.Columns.length) return null;
    for (var i = 0; i < lb.Columns.length; i++) {
        var fieldBind = lb.Columns[i];
        if (fieldBind.Id == elementId) return fieldBind;
    }
    return null;
}; 
