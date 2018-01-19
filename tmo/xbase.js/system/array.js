Array.prototype.toSplitString = function(sChar) {
    if (sChar == undefined || sChar == null)
        sChar = ","
    var arr = this;
    var str = "";
    for (var i = 0; i < arr.length; i++) {
        str += arr[i] + sChar;
    }
    if (str == "") { return str; }
    else { return str.substr(0, str.length - 1); }
}

Array.prototype.indexOf = function(item) {

    for (var i = 0; i < this.length; i++) {
        if (item == this[i])
            return i;
    }
    return -1;

}
Array.prototype.insert = function(vVal, nIdx) {
    var arrTemp = this;
    if (nIdx > arrTemp.length) nIdx = arrTemp.length;
    if (nIdx < -arrTemp.length) nIdx = 0;
    if (nIdx < 0) nIdx = arrTemp.length + nIdx;
    for (var ii = arrTemp.length; ii > nIdx; ii--) {
        arrTemp[ii] = arrTemp[ii - 1];
    }
    arrTemp[nIdx] = vVal;
    return arrTemp;
}

Array.prototype.removeItem = function(item) {
    var index = this.indexOf(item);
    if (index < 0) return false;
    this.splice(index, 1);
    return true;
}

Array.prototype.hasText = function(text) {
    for (var i = 0; i < this.length; i++) {
        var item = this[i];
        if (text.toLowerCase && item.toLowerCase) {
            if (text.toLowerCase() == item.toLowerCase())
                return true;
        }
    }
    return false;
}

