


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
