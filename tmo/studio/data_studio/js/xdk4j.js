var XDK4J = {

    version: "1.0.0.0",

    RequestType: {
        Schema: "Schema",
        Database: "DataBase"
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
        SearchTableInfo: "SearchTableInfo"
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
    * @summary：请求
    * @example：XAjax.request("http:xxx?ss=ss",myFunction);
    * @Param1 requestType   :  请求类型 Schema,DataBase
    * @Param2 operationName :  操作名称，即函数的名称
    * @Param3 paramName     :  参数，即路径
    * @Param4 objType       :对象类型[XTable|| XForm||ServerFunction||ServerObject||ClientFunction||ClientObject|| Menu||XTable]
    **/
    SynRequest: function(requestType, operationName, paramName, objType, data) {
        var requestJson = {
            RequestType: requestType,
            ParamName: paramName,
            OperationName: operationName,
            ObjType: objType,
            Data: data
        }

        var url = "/xj-service/WbdlDesigner.aspx";

        var requestStr = XAjax.obj2str(requestJson);

        var strResponse = XAjax.SynRequest(url, requestStr);
        //  alert(strResponse);
        if (strResponse == null) return null;

        var response = eval("(" + strResponse + ")");

        if (response.ErrorNo != 0) {
            alert(response.ErrorMessage);
            return null;
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


var XDataDk = {

    version: "1.0.0.0",
    /**
    * @summary：请求
    * @example：XAjax.request("http:xxx?ss=ss",myFunction);
    * @Param1 requestType   :  请求类型 Schema,DataBase
    * @Param2 operationName :  操作名称，即函数的名称
    * @Param3 paramName     :  参数，即路径
    * @Param4 objType       :对象类型[XTable|| XForm||ServerFunction||ServerObject||ClientFunction||ClientObject|| Menu||XTable]
    **/

    //    SynRequest: function(requestType, operationName, paramName, objType, data) {
    GetTableInfo: function(tableName) {
        return XDK4J.SynRequest(XDK4J.RequestType.Schema, XDK4J.OperationName.GetSchema, tableName, XDK4J.ObjType.XTable, null);
    },

    SaveTableInfo: function(schema) {
        XDK4J.SynRequest(XDK4J.RequestType.Schema, XDK4J.OperationName.UpdateSchema, schema.Id, XDK4J.ObjType.XTable, schema);
    },

    SetConnectionInfo: function(connectionInfo) {
        XDK4J.SynRequest(XDK4J.RequestType.Database, XDK4J.OperationName.SetConnectionInfo, null, null, connectionInfo);
    },

    SearchTableInfo: function(sql) {
        return XDK4J.SynRequest(XDK4J.RequestType.Database, XDK4J.OperationName.SearchTableInfo, sql, null, null);
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


//实体类型

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