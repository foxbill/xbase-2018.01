function PropertyList(listElement) {
    //begin 继承dTree(objName)-----//
    var _baseObj = new Listview(objName);
    for (var p in _baseObj) { PropertyList.prototype[p] = _baseObj[p]; }
    //end   继承dTree(objName)-----//
    
    
}