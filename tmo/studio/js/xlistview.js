function ListView(listViewElement) {
    if (listViewElement == undefined || listViewElement == null) return;

    var _this = this;
    var _body = typeof (listViewElement) == "string" ? document.getElementById(listViewElement) : listViewElement;
    if (_body != undefined && _body != null && _body.nodeName == "TABLE")
        _body = listViewElement.getElementsByTagName("TBODY")[0];

    var items = new Array();
    var activeItem = null;
    for (var i in _body.childNodes) {
        var node = _body.childNodes[i];
        if (node.attributes && node.attributes['ishead'] && node.attributes['ishead'].nodeValue === '1') {
            this.listHead = node;
        }
        if (node.attributes && node.attributes['isitem'] && node.attributes['isitem'].nodeValue === '1') {
            this.listItemElement = node;
        }
    }
    this.itemCss = !_this.listItemElement.className ? "" : _this.listItemElement.className;
    this.activeItemCss = null;
    this.overItemCss = null;
    this.body = _body;
    function initListItem() {
        _body.removeChild(_this.listItemElement);
        if (_this.listItemElement.className != undefined && _this.listItemElement.className != null && _this.listItemElement.className != null)
            _this.itemCss = _this.listItemElement.className;
    }
    function addItemCol(item, colName, colTitle) {
        if (!item || !item.childNodes)
            return;
        var tag;
        for (var i in _this.listHead.childNodes) {
            var n = _this.listHead.childNodes[i];
            if (n.tagName) {
                tag = n.tagName;
                break;
            }
        }
        if (!tag)
            return;
        var newCol = document.createElement(tag);
        newCol.name = colName;
        if (colTitle)
            newCol.innerHTML = colTitle;
        item.appendChild(newCol);
    }
    function removeItemCol(item, index) {
        if (!item || !item.childNodes)
            return;
        var ea = new Array();
        for (var i in item.childNodes) {
            var n = item.childNodes[i];
            if (n.nodeType && n.nodeType == 1) {
                ea.push(n);
            }
        }
        if (ea.length <= index) return;

        item.removeChild(ea[index]);
    }
    function getItemValue(item, elementName) {
        var els = DomUtils.getElementsByName(item, elementName);
        var el = els[0];
        return DomUtils.getElementValue(el);
    }
    initListItem();


    this.deleteItem = function (item) {
        items.removeItem(item);
        _body.removeChild(item);
    }
    this.removeItem = this.deleteItem;

    //
    this.addItem = function () {
        if (this.listItemElement == undefined || this.listItemElement == null) {
            alert("没有指定模板行，无法添加行，请先使用listView.tmpRow=element，指定模板行,还可使用this.listHead指定标题行");
            return;
        }

        var r = DomUtils.cloneNode(this.listItemElement);

        _body.appendChild(r);
        items.push(r);

        r.style.display = ""; //缺省显示

        r.onfocus = function () {
            _this.setActiveItem(this);
        }

        r.onmouseover = function () {
            if (this != activeItem && _this.overItemCss)
                this.className = _this.overItemCss;
        }

        r.onmouseout = function () {
            if (this != activeItem && _this.itemCss)
                this.className = _this.itemCss;
        }

        r.onclick = function () {
            _this.setActiveItem(this);
        }

        return r;
    }
    //
    this.removeItemByIndex = function (itemIndex) {
        if (itemIndex < 0 || itemIndex >= items.length)
            return;
        var item = items[itemIndex];
        items.remove(itemIndex);
        _body.removeChild(item);
    }
    
    //
    this.addColumn = function (colName, colTitle) {

        addItemCol(_this.listHead, colName, colTitle);
        addItemCol(_this.listItemElement, colName, null);
        for (var i in items) {
            var item = items[i];
            addItemCol(item, colName, null);
        }
    }
    //
    this.removeColumnByIndex = function (colIndex) {
        removeItemCol(_this.listHead, colIndex);
        removeItemCol(_this.listItemElement, colIndex);
        for (var i in items) {
            var item = items[i];
            removeItemCol(item, colIndex);
        }
    }
    //
    this.clearAll = function () {
        for (var i = items.length - 1; i > -1; i--) {
            var item = items[i];
            items.remove(i);
        }
        if (_body.childNodes && _body.childNodes.length) {
            for (var i = _body.childNodes.length - 1; i > -1; i--) {
               _body.removeChild(_body.childNodes[i]);
            }
        }
    }
    //
    this.clear = function () {
        for (var i = items.length - 1; i > -1; i--) {
            var item = items[i];
            _body.removeChild(item);
            items.remove(i);
        }
        if (_body.childNodes && _body.childNodes.length) {
            for (var i = _body.childNodes.length - 1; i > -1; i--) {
                var cld = _body.childNodes[i];
                if (cld != this.listHead)
                    _body.removeChild(cld);

            }
        }
    }
    //
    this.setActiveItem = function (item) {
        if (!item) return;
        if (activeItem && activeItem!= item ) {
            if (this.onExitItem)
                  this.onExitItem(activeItem);
        }
        
        for (var i = 0; i < items.length; i++) {
            var aitem = items[i];
            aitem.className = this.itemCss;
        }
        activeItem = item;
        if (this.activeItemCss)
            activeItem.className = this.activeItemCss;


        if (this.onActiveItem)
             this.onActiveItem(item);
    }
    //
    this.getActiveItem = function () {
        return activeItem;
    }
    //
    this.getItemByField = function (fieldName, fieldValue) {
        for (var i = 0; i < items.length; i++) {
            var item = items[i];
            var value = getItemValue(item, fieldName);
            if (value.toLowerCase() == fieldValue.toLowerCase())
                return item;
        }
        return null;
    }
    //
    this.getItemElement = function (item, elementName) {
        var els = DomUtils.getElementsByName(item, elementName);
        if (!els || !els.length || els.length < 0)
            return null;
        var el = els[0];
        return el;
    }
    //
    this.onActiveItem = function (item) {
    }
    //
    this.onExitItem=function(item){
    }
    //
    this.setCellValue = function (colName, colValue, rowNo) {
        rowNo++;
        var cells = DomUtils.getElementsByName(_body, colName);
        if (cells.length < 1) {
            alert("没有发现列" + colName);
            return;
        }
        if (rowNo >= cells.length)
            return;
        var el = cells[rowNo];

        if (el.tagName.toLowerCase() == "a")
            el.href = value;
        else
            DomUtils.setElementValue(el, colValue, false);
    }
    //
    this.getCellValue = function (colName, rowNo) {
        var cells = DomUtils.getElementsByName(_body, colName);
        if (cells.length < 1) {
            alert("没有发现列" + colName);
            return;
        }
        if (rowNo >= cells.length)
            return;
        var el = cells[rowNo];
        if (el.tagName.toLowerCase() == "a")
            return el.href;
        else
            return DomUtils.getElementValue(el);
    }
}