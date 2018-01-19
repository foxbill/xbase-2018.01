var Page = new Object();
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

JoapRequest.prototype.setObjCls = function (objCls) {
    this.jrObj.ObjCls = objCls;
}

JoapRequest.prototype.setObjName = function (objName) {
    this.jrObj.ObjName = objName;
}

JoapRequest.prototype.setMethod = function (method) {
    this.jrObj.Method = method;
}

JoapRequest.prototype.setObjData = function (objData) {
    this.jrObj.ObjData = objData;
}

JoapRequest.prototype.addParamate = function (paramName, paramValue) {
    this.jrObj.Paramates[paramName] = paramValue;
}
JoapRequest.prototype.setParams = function (params) {
    this.jrObj.Paramates = params;
}



JoapRequest.prototype.submit = function (hideErrAlert) {
    //  var url = "/xj-service/WjsNetPort.aspx";
    var url = "wbos.axd";
    for (var p in this.jrObj.Paramates) {
        if (typeof this.jrObj.Paramates[p] != "string")
            this.jrObj.Paramates[p] = JSON.stringify(this.jrObj.Paramates[p]);
    }
    var requestStr = JSON.stringify(this.jrObj); // XAjax.obj2str(this.jrObj);
    var strResponse = XAjax.SynRequest(url, requestStr);
    //  alert(strResponse);
    if (strResponse == null) return true;

    var response = eval("(" + strResponse + ")");

    if (response.Err && response.Err.Text) {
        var err = response.Err;

        if (hideErrAlert)
            return response;
        else {
            if (Page.onNoPermission) {
                Page.onNoPermission(err);
                return null;
            }
            alert(err.Text);
            if (err.Url)
                self.navigate(err.Url);
            //                self.location.href = err.ErrUrl;
            //   window.location.reload(err.ErrUrl);
            return false;
        }
    }
    // alert(response);

    return response;
}

//过期方法，兼容旧版本。
JoapRequest.prototype.commit = JoapRequest.prototype.submit;

JoapRequest.prototype.invoke = function (methodName, params) {
    if (methodName)
        this.setMethod(methodName);
    if (params)
        this.setParams(params);

    var resp = this.commit();
    if (resp && (typeof resp == "object")) {

        if (resp.ObjData && (this.jrObj.Method == undefined || this.jrObj.Method == null || this.jrObj.Method == ""))
            return resp.ObjData.Data;
        else if (resp.RetData)
            return resp.RetData.Data == null ? true : resp.RetData.Data;
        else
            return true;

    } else
        return resp;
}

function $Wbo(wboId, wboName) {
    return new JoapRequest(wboId, wboName);
}