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

Request.prototype.commit = function() {

    var url = "/xj-service/WbpsHttpPort.aspx";

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
