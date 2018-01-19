/*
@import system/system.js
*/

function ListView(listViewElement, listItemElement, listHeadElement) {


    var _this = this;
    var _body = typeof (listViewElement) == "string" ? document.getElementById(listViewElement) : listViewElement;
    var items = new Array();
    var activeItem = null;


    this.listItemElement = listItemElement;
    this.listHead = listHeadElement;
    this.itemCss = !listItemElement.className ? "" : listItemElement.className;
    this.activeItemCss = null;
    this.overItemCss = null;


    if (listViewElement == undefined || listViewElement == null) return;

    if (_body != undefined && _body != null && _body.nodeName == "TABLE")
        _body = listViewElement.getElementsByTagName("TBODY")[0];

    function initListItem() {
        //_this.listItemElement.display = "none";
        _body.removeChild(_this.listItemElement);
        if (listItemElement.className != undefined && listItemElement.className != null && listItemElement.className != null)
            _this.itemCss = listItemElement.className;
    }

    initListItem();

    this.getItem = function (index) {
        return items[index];
    }

    this.items = items;

    this.getCount = function () {
        return items.length;
    }

    this.getActiveIndex = function () {
        return items.indexOf(activeItem);
    }

    this.setActiveIndex = function (index) {
        this.setActiveItem(items[index]);
    }


    this.setItemValue = function (item, elementName, value, hint) {
        var els = DomUtils.getElementsByName(item, elementName);
        if (!els || !els.length) return;

        var el = els[0];
        if (hint) {
            el.alt = hint;
            if (el.title != undefined)
                el.title = hint;
        }
        DomUtils.setElementValue(el, value);
    }

    this.getItemValue = function (item, elementName) {
        var els = DomUtils.getElementsByName(item, elementName);
        var el = els[0];
        return DomUtils.getElementValue(el);
    }
    this.getItemElement = function (item, elementName) {
        var els = DomUtils.getElementsByName(item, elementName);
        if (!els || !els.length || els.length < 0)
            return null;
        var el = els[0];
        return el;
    }

    this.getItemByField = function (fieldName, fieldValue) {
        for (var i = 0; i < items.length; i++) {
            var item = items[i];
            var value = this.getItemValue(item, fieldName);
            if (value.toLowerCase() == fieldValue.toLowerCase())
                return item;
        }
        return null;
    }


    this.setValue = function (elementName, value) {
        if (!activeItem) return;
        this.setItemValue(activeItem, elementName, value);
    }

    this.getValue = function (elementName) {
        if (!activeItem)
            return null;
        return this.getItemValue(activeItem, elementName);
    }

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

    this.addColumn = function (name, caption, editControl, button) {

        alert("no check this function");
        return 0;

        var newCol = document.createElement("td");
        newCol.innerHTML = caption;
        _headRow.appendChild(newCol);

        var newCol = document.createElement("<td  nowrap='nowrap'>");
        _tempRow.appendChild(newCol);
        if (editControl) {
            editControl.name = name;
            newCol.appendChild(editControl);
        } else {
            newCol.name = "name";
            newCol.innerHTML = "&nbsp;";
        }

        if (button) {
            newCol.appendChild(button);
        }

        for (var i = 2; i < _tBody.childNodes.length; i++) {
            var row = _tBody.childNodes[i];
            var newCol = document.createElement("td");
            newCol.name = "name";
            row.appendChild(newCol);
        }
    }


    this.getColumnValues = function (columnName) {

        var cells = document.getElementsByName(columnName);

        var ary = new Array();

        for (var i = 1; i < cells.length; i++) {
            var cell = cells[i];
            ary[i - 1] = DomUtil.GetElementValue(cell);
        }

        alert(ary.toString());
        return ary;
    }




    this.getRowCount = function () {
        return _tBody.childNodes.length - 2;
    }

    this.deleteRow = function (rowNo) {
        _tBody.removeChild(_tBody.childNodes[rowNo + 1]);
    }

    this.setColumnValue = function (columnName, rowNo, value) {
        var cells = document.getElementsByName(columnName);


        if (_tBody.childNodes.length < rowNo) {
            while (_tBody.childNodes.length < rowNo)
                this.AddRow();
            cells = document.getElementsByName(columnName);
        }

        if (cells.length < 1) {
            alert(_table.nodeName + "中，没有发现列" + columnName);
            return;
        }

        var el = cells[rowNo];

        if (el.tagName.toLowerCase() == "a")
            el.href = value;
        else
            DomUtil.SetElementValue(el, value, false);
    }

    this.getCellObjectRowNo = function (obj) {
        var cells = document.getElementsByName(obj.name);
        for (var i = 1; i < cells.length; i++) {
            if (obj == cells[i]) {
                return i;
            }
        }
    }

    this.getColumnValue = function (columnName, rowNo) {
        var cells = document.getElementsByName(columnName);
        var el = cells[rowNo];
        if (el.tagName.toLowerCase() == "a")
            return el.href;
        else
            return DomUtil.GetElementValue(el);
    }

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

    //   this.onRowClick = function(rowElement) {
    //  }

    this.getItemByElement = function (element) {
        for (var i = 0; i < items.length; i++) {
            var item = items[i];
            if (DomUtils.nodeHasChild(item, element))
                return item;
        }
        return null;
    }



    this.deleteItem = function (item) {
        items.removeItem(item);
        _body.removeChild(item);
    }

    this.deleteActive = function () {
        if (activeItem == null) {
            alert("请选择需要删除的条目");
            return;
        }
        this.deleteItem(activeItem);
    }

    this.deleteByElement = function (element) {
        var item = this.getItemByElement(element);

        if (item == undefined || item == null) {
            alert("不能发现包含元素" + element.nodeName + "  " + element.id + "项目");
        }

        this.deleteItem(item);
    }

    this.onExitItem = function (item) { return true }

    this.setActiveItem = function (item) {
        if (activeItem) {
            if (this.onExitItem)
                if (!this.onExitItem(activeItem)) return;
        }
        if (!item) return;
        for (var i = 0; i < items.length; i++) {
            var aitem = items[i];
            aitem.className = this.itemCss;
        }
        activeItem = item;
        if (this.activeItemCss)
            activeItem.className = this.activeItemCss;

        if (this.onRowClick)
            this.onRowClick(item);

        if (this.onActiveItem)
            this.onActiveItem(item);
    }


    this.getActiveItem = function () {
        return activeItem;
    }

}
