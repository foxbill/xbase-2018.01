String.prototype.str2Array = function() {
    var str = this;
    if (str == "") {
        return new Array();
    }
    else {
        if (str.lastIndexOf(",") == str.length - 1)
            str = str.substr(0, str.length - 1);
        return str.split(",");
    }
}