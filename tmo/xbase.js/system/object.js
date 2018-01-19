function obj2str(o) {
    var r = [];
    if (typeof o == "string") return "\"" + o.replace(/([\'\"\\])/g, "\\$1").replace(/(\n)/g, "\\n").replace(/(\r)/g, "\\r").replace(/(\t)/g, "\\t") + "\"";
    if (typeof o == "undefined") return "";
    if (typeof o == "object") {
        if (o === null) return "null";
        else if (o.length == undefined || o.length == null) {
            for (var i in o)
                r.push(i + ":" + this.obj2str(o[i]))
            r = "{" + r.join() + "}"
        } else {
            for (var i = 0; i < o.length; i++)
                r.push(this.obj2str(o[i]))
            r = "[" + r.join() + "]"
        }
        return r;
    }
    return o.toString();
}
//克隆
function clone(myObj) {
    if (typeof (myObj) != 'object') return myObj;
    if (myObj == null) return myObj;
    var myNewObj = new Object();
    for (var i in myObj) myNewObj[i] = this.clone(myObj[i]);
    return myNewObj;
}

////全局对象

//Page = {
//    ElementData: new Object(),
//    Document: null,
//    PageName: null,

//    fillData: function() { },
//    getObjById: function() { }
//}
