function getParametes(str) {

    var ary = str.split(/[\s+\-*\/\(\)\[\]\,\;]/);
    var ret = new Array();
    for (var i = 0; i < ary.length; i++) {
        var s = ary[i];
        if (s.indexOf('@',0) == 0)
            ret.push(ary[i]);
    }
    return ret;
}