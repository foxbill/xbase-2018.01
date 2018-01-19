/*************************************************************************
* <p>Title: listview 1.0</p>
* <p>Description: SScompanyTest管理系统</p>
* <p>Copyright: Copyright (c) 2005 SScompany, Co.Ltd. All Rights Reserved</p>
* <p>Company: SScompany </p>
* <p>WebSite: http://www.SScompany.com.cn</p>
*
* @author   : SScompany
* @version : 1.00
* @date     : 2006-11-08
* @direction: Test系统多行显示/输入表格
* @comment : XinYQ formatted on 2006-11-08
**************************************************************************/


/*
$import(system.js);
$import(dom_utils.js);
*/


ListDef = {
    IS_LIST_ATTR: "isList",
    LIST_DATAROW_ATTR: "dataRow", //定义数据行的属性名
    LIST_ELEMENT_ID: "_List",
    LIST_BODY_ELEMENT_ID: "_ListBody",
    ITEM_BODY_ELEMENT_ID: "_ItemBody",
    COL_ELEMENT_ID: "[#]"
}


/**
* 列表项
* 即列表中的一行
* @constructor
* @base Shape
*/
function ListItem(itemElement, listView) {
    var _listView = listView;
    var _itemElement = itemElement;
    var colElements = new Object;


    this.getCol = function(cellName) {
        var col = colElements[cellName];
        if (!col || col == undefined) {
            col = DomUtils.drillChildByAttr(_itemElement, "name", cellName);
            if (!col || col == undefined)
                col = DomUtils.drillChildByAttr(_itemElement, "id", cellName);

            colElements[cellName] = col;
        }
        return col;
    }
    //!---Old Interface ver: 2010-04-30;
    this.getCell = this.getCol;

    this.newCol = function(celName) {
        col = this.document.createElement("TEXT");
        col.name = _columns[i];
        if (_itemElement.nodeName == "TR") {
            var cent = _itemElement.lastChild;
            if (!cent || cent == undefined) cent = cent.insertCell(-1);
            cent.appendChild(col);
        } else {
            _itemElement.appendChild(col);
        }
        return col;
    }


    this.setColValue = function(colName, colValue) {
        var col = this.getCol(colName);
        if (!col) {
            alert("can not find col " + "'" + colName + "' with attribute 'name'");
            return null;
        }
        DomUtils.setElementValue(col, colValue);
    }
    //!---Old Interface ver: 2010-04-30;
    this.setCellValue = this.setColValue;

    this.getColValue = function(colName) {
        var col = this.getCol(colName);
        if (!col) {
            alert("can not find col " + "'" + cellName + "' with attribute 'name'");
            return null;
        }
        return DomUtils.getElementValue(col);
    }

    //!---Old Interface ver: 2010-04-30;
    this.getCellValue = this.getColValue;

    this.getIndex = function() {
        if (listView && listView.getItemIndex) {
            return _listView.getItemIndex(this);
        }
        else {
            alert("list item not assigned ListView");
            return -1;
        }
    }

    this.getElement = function() {
        return _itemElement;
    }

    this.setCellEditor = function(cellName, editElement) {
        var cell = this.getCell(cellName);
        if (!cell) return;

        if (!editElement.attributes["name"] || editElement.attributes["name"].value != cellName) {
            alert("新元素的name属性必须和老元素的一样");
            return;
        }

        if (cell.nodeName == "TR") {
            cell.attributes["name"].value = "";
            cell.appendChild(editElement);
        } else {
            document.replaceChild(editElement, cell);
        }
    }
}

/**
* 列表类 Listview
* 
* @constructor
* @base object
*/
function Listview(listElement, dataRow) {
    var _this = this;
    var _listElement;
    var _listBodyElement;
    var _itemBodyElement;
    var _listItems = [];
    var _columns = [];

    
    function initItembody() {


        var listItem = _listElement;

        if (!_listBodyElement) {
            alert("listElement not initualize");
        }

        var intDataRow = 0;

        if (dataRow && dataRow != undefined && dataRow != "") {
            intDataRow = parseInt(dataRow);
        } else {
            var strDataRow = "";
            var attr = listElement.getAttribute(ListDef.LIST_DATAROW_ATTR)
            if (attr)
                strDataRow = attr.value;

            if (strDataRow && strDataRow != undefined && strDataRow != "") {
                intDataRow = parseInt(strDataRow);
            }
        }

        _itemBodyElement = _listBodyElement.childNodes[intDataRow];
        _itemBodyElement.id = "";
        listElement.setAttribute(ListDef.LIST_DATAROW_ATTR, intDataRow);

        DomUtils.forEachChild(_itemBodyElement, function(child) {
            var id = DomUtils.getElementAttr(child, "id");
            var name = DomUtils.getElementAttr(child, "name");
            if (id && id != undefined && id != "" && (name == null || name == "" || name == undefined)) {
                DomUtils.setElementAttr("name", id);
            }
            return false;
        });

        for (var i = _listBodyElement.childNodes.length - 1; i >= intDataRow; i--) {
            _listBodyElement.removeChild(_listBodyElement.childNodes[i]);
        }
    }



    /**
    * 初始化-initualize
    * @param listElement 显示列表的元素，一个 table 或 div
    */
    this.initualize = function(listElement) {
        if (!listElement) {
            return;
        }

        this.document = listElement.ownerDocument;

        //process _listElement
        if (typeof (listElement) == "object") {
            _listElement = listElement;
            this.document = listElement.ownerDocument;
        }
        else {
            _listElement = document.getElementById(listElement);
            this.document = document;
        }


        if (!listElement) {
            alert("can not find listElement " + listElement);
            return;
        }



        //process _listBodyElement
        if (!_listBodyElement) {
            if (_listElement.nodeName && _listElement.nodeName == "TABLE") {
                _listBodyElement = _listElement.childNodes[0];
            } else {
                _listBodyElement = _listElement;
            }
        }

        initItembody();

    }

    this.initualize(listElement);

    this.setColumns = function(columnNames) {
        this.clear();
        _columns = columnNames;

        var tempItem = new ListItem(_itemBodyElement, this);

        for (var i = 0; i < _columns.length; i++) {
            var col = tempItem.getCol(_columns[i]);
            if (!col || col == undefined)
                tempItem.newCol(_columns[i]);
        }
        tempItem = null;
    };

    this.newItem = function() {
        var newItemElement = DomUtils.cloneNode(_itemBodyElement, true);
        _listBodyElement.appendChild(newItemElement);
        var newItem = new ListItem(newItemElement, this);
        _listItems.push(newItem);
        return newItem;
    }
    //!--- Old Interface ver 2010-04-30 ----
    this.newListItem = this.newItem;

    this.getItems = function() {
        return _listItems;
    }

    /**
    * getItemIndex(item)
    * 获取列表项的行号
    * @param listElement 显示列表的元素，一个 table 或 div
    * @return;
    */
    this.getItemIndex = function(item) {
        for (var i = 0; i < _listItems.length; i++) {
            if (item == _listItems[i]) {
                return i;
            }
        }
        return -1;
    }

    this.clear = function() {
        while (_listItems.length > 0) {
            var itemElement = _listItems[0].getElement();
            _listBodyElement.removeChild(itemElement);
            _listItems.splice(0, 1);
        }
    }

    //document.attributes

    /**********************
    *   -----Event----
    **********************/
    this.onRowClick = null;




    this.GetColumnValues = function(columnName) {

        var cells = document.getElementsByName(columnName);

        var ary = new Array();
        for (var i = 1; i < cells.length; i++) {
            var cell = cells[i];
            ary[i - 1] = DomUtil.GetElementValue(cell);
        }
        alert(ary.toString());
        return ary;
    }

    this.GetRowCount = function() {
        return _listBodyElement.childNodes.length - 2;
    }

    this.DeleteRow = function(rowNo) {
        _listBodyElement.removeChild(_listBodyElement.childNodes[rowNo + 1]);
    }

    this.SetColumnValue = function(columnName, rowNo, value) {
        var cells = document.getElementsByName(columnName);

        if (cells.length < 1) {
            alert(_listElement.nodeName + "中，没有发现列" + columnName);
            return;
        }

        if (_listBodyElement.childNodes.length < rowNo) {
            while (_listBodyElement.childNodes.length < rowNo)
                this.AddRow();
            cells = document.getElementsByName(columnName);
        }
        DomUtil.SetElementValue(cells[rowNo], value, false);
    }

    this.GetCellObjectRowNo = function(obj) {
        var cells = document.getElementsByName(obj.name);
        for (var i = 1; i < cells.length; i++) {
            if (obj == cells[i]) {
                return i;
            }
        }
    }

    this.GetColumnValue = function(columnName, rowNo) {
        var cells = document.getElementsByName(columnName);
        return DomUtil.GetElementValue(cells[rowNo]);
    }

    //！---不用了---- 
    this.AddRow = function() {
        var r = _itemBodyElement.cloneNode(true);
        _listBodyElement.appendChild(r);
        r.style.display = "";
        var cells = document.getElementsByName("fieldList_Id");

        if (this.OnRowClick != null) {
            r.onclick = function() {
                this.OnRowClick(r);
            }
        }
        return r;
    }

    this.getListItems = function() {
        return _listItems;
    }
}

//静态方法
Listview.getElementListViewElement = function(element) {
    var ret = null;
    ret = DomUtils.drillUpByAttr(element, ListDef.IS_LIST_ATTR, "1");
    if (!ret) {
        ret = DomUtils.drillUpByAttr(element, ListDef.IS_LIST_ATTR, "yes");
    }
    return ret;
}


/**
* 属性表-PropertyList
* 用于显示和管理对象属性
* @constructor
* @base Shape
*/

PropertyList_DEF = {
    NAME_COL: "name",
    VALUE_COL: "value",
    TITLE_COL: "title"
}

//继承Listview
PropertyList.prototype = new Listview;
PropertyList.constructor = PropertyList;
PropertyList.prototype.superMethod = function(methodName, args) { Listview.prototype[methodName].call(this, args); };


function PropertyList(listElemnt) {

    var _this = this;

    Listview.call(this, listElemnt);

    this.getPropertyItem = function(name) {
        var items = this.getListItems();
        for (var i = 0; i < items.length; i++) {
            var item = items[i];
            if (item.getCellValue(PropertyList_DEF.NAME_COL) == name) {
                return item;
            }
        }
        return null;
    }

    //    this.newProperty = function(name, title) {
    //      
    //    }

    this.onPropertyChange = function(propertyName, propertyValue) {
        alert(propertyName + "=" + propertyValue);
    };

    this.setProperty = function(name, value, title) {
        var item = this.getPropertyItem(name);

        if (!item) {
            item = this.newListItem();

            var cellElement = item.getCell(PropertyList_DEF.VALUE_COL);

            cellElement.onchange = function(event) {
                if (_this.onPropertyChange) _this.onPropertyChange(name, DomUtils.getElementValue(cellElement));
            }
        }

        item.setCellValue(PropertyList_DEF.NAME_COL, name);
        item.setCellValue(PropertyList_DEF.VALUE_COL, value);
        if (title)
            item.setCellValue(PropertyList_DEF.TITLE_COL, title);
    }

    this.getProperty = function(name) {
        var item = this.getPropertyItem(name);
        if (!item) {
            return null;
        }
        return item.getCellValue(PropertyList_DEF.VALUE_COL);
    }

    this.setPropertyEditor = function(name, editorElement) {
        var item = this.getPropertyItem(name);
        item.setCellEditor(PropertyList_DEF.VALUE_COL, editorElement);
    }
}



function DataForm(formElement) {
    var _this = this;
    var _formElement = null;
    var _fieldbuff = [];
    var _owner = null;
    var _enabled = true;

    this.document = null;


    this.initualize = function(formElement) {
        _formElement = formElement;
        this.document = formElement.ownerDocument;
        _owner = formElement.parentNode;
    }

    this.getDataElement = function(fieldId) {

        var element = _fieldbuff[fieldId];

        if (!element) {
            element = this.document.getElementById(fieldId);
            if (!element) {
                alert(fieldId + " element not find");
                return null;
            }
            _fieldbuff[fieldId] = element;
        }
        return element;
    }

    this.setDataValue = function(fieldId, fieldValue) {
        var element = this.getDataElement(fieldId);
        if (element)
            DomUtils.setElementValue(element, fieldValue);
    }

    this.getDataValue = function(fieldId) {
        var element = this.getDataElement(fieldId);
        if (element)
            return DomUtils.getElementValue(element);
    }

    this.addFieldElement = function(fieldElement) {

        if (!fieldElement.id) {
            alert("fieldElement.id not assigned");
            return;
        }
        _fieldbuff[fieldElement.id] = fieldElement;

        var _this = this;
        fieldElement.onchange = function() {
            _this.onDataChange(fieldElement);
        }

    }

    this.getEnabled = function() {
        return _enabled;
    }

    this.setEnabled = function(enabled) {
        if (_enabled == enabled) return;
        _enabled = enabled;

        var inputs = formElement.getElementsByTagName("input");
        for (var i = 0; i < inputs.length; i++) {
            var input = inputs[i];
            input.disabled = !enabled;
        }

        var inputs = formElement.getElementsByTagName("select");
        for (var i = 0; i < inputs.length; i++) {
            var input = inputs[i];
            input.disabled = !enabled;
        }
        var disp = enabled ? "block" : "none";
        formElement.style.display = disp;
    }

    this.initualize(formElement);
}

//事件句柄
DataForm.prototype.onDataChange = function(fieldElement) {
    alert(fieldElement.id + " Changed");
}


XList.constructor = XList;
XList.prototype = new Listview();
XList.prototype.superMethod = function(methodName, args) { Listview.prototype[methodName].call(this, args); };

function XList(elementId, elementData) {
    if (typeof (elementId) == "string") elementId = document.getElementById(elementId);
    Listview.call(this, elementId, elementData.RowData);

    this.elementData = elementData;
    this.setColumns(elementData.Columns);


    //共有成员
    this.Bind = function() {
        var data = elementData.Data;
        var columns = elementData.Columns;

        for (var r = 0; r < dataRows.length; r++) {
            var item = this.newItem();
            var row = data[r];
            for (var c = 0; c < columns.length; c++) {
                item.setColValue(columns[c], row[c]);
            }
        }
    }

    this.AddRow = function() {
        var item = this.newItem(); ;
        return item.getElement();
    }

    this.DelRow = function() {
    }
    //-------------------------------聚合----------------------------------------------------
    //求和
    this.SumColumn = function(colName) {
        var items = this.getItems();
        var ret = 0;
        for (var i = 0; i < items.lenght; i++) {
            ret += parseFloat(items[i].getColValue(colName));
        }
        return ret;
    }
    //求平均
    this.AverageColumn = function(colName) {
    }
    //求最大数
    this.MaxColumn = function(colName) {
    }
    //求最小数
    this.MinColumn = function(colName) {
    }
    this.RowCount = function(tableId) {
        return this.getItems().length;
    }
}
