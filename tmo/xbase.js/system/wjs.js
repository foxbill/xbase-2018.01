var Page=new Object();
function JoapRequest(objType, objName, methodName, methodParams) {
    this.jrObj = new Object();
    this.jrObj.Id = new Date().toString();

    if (objType && (objType != undefined))
        this.jrObj.ObjCls = objType;
    else
        this.jrObj.ObjCls = "";

    if (objName && (objName != undefined))
        this.jrObj.ObjName = objName;
    else
        this.jrObj.ObjName = "";

    if (methodName && (methodName != undefined))
        this.jrObj.Method = methodName;
    else
        this.jrObj.Method = "";

    if (methodParams && (methodParams != undefined))
        this.jrObj.Paramates = methodParams;
    else
        this.jrObj.Paramates = new Object();


    this.jrObj.ObjData = new Object();
}

JoapRequest.prototype.setObjCls = function(objCls) {
    this.jrObj.ObjCls = objCls;
}

JoapRequest.prototype.setObjName = function(objName) {
    this.jrObj.ObjName = objName;
}

JoapRequest.prototype.setMethod = function(method) {
    this.jrObj.Method = method;
}

JoapRequest.prototype.setObjData = function(objData) {
    this.jrObj.ObjData = objData;
}

JoapRequest.prototype.addParamate = function(paramName, paramValue) {
    this.jrObj.Paramates[paramName] = paramValue;
}


JoapRequest.prototype.commit = function(hideErrAlert) {
//  var url = "/xj-service/WjsNetPort.aspx";
    var url = "wbos.axd";
    var requestStr = XAjax.obj2str(this.jrObj);
    var strResponse = XAjax.SynRequest(url, requestStr);
    //  alert(strResponse);
    if (strResponse == null) return null;

    var response = eval("(" + strResponse + ")");

    if (response.Err) {
        var err = response.Err;
        if (hideErrAlert)
            return response;
        else {
            if (Page.onNoPermission) {
                Page.onNoPermission(err);
                return null;
            }
            alert(err.ErrText);
            if (err.ErrUrl) {
                window.location.href = err.ErrUrl;
                //   window.location.reload(err.ErrUrl);
            }
            return null;
        }
    }
    // alert(response);

    return response;
}

JoapRequest.prototype.invoke = function() {
    var resp = this.commit();
    if (resp) {

        if (resp.ObjData && (this.jrObj.Method == undefined || this.jrObj.Method == null || this.jrObj.Method == ""))
            return resp.ObjData.Data;
        else if (resp.RetData)
            return resp.RetData.Data;
        else
            return null;

    } else
        return null;
}
