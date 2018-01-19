
function ListTable(table) {
    var _table;
    var _tBody;
    var _headRow;
    var _tempRow;
    var _activeRowNo = 0;
    var _headRowCount = 1;


    if (typeof (table) == "object")
        _table = table;
    else
        _table = document.getElementById(table);

    _tBody = table.childNodes[0];

    this.onRowClick = null;

    this.getActiveRowNo = function() {
        return _activeRowNo;
    }

    if (_tBody != null && _tBody.nodeName == "TBODY") {
        _headRow = _tBody.childNodes[0];
        _tempRow = _tBody.childNodes[1];
    }
    else {
        _tempRow = _tBody;
        _tBody = _table;
    }

    this.clearData = function() {
        while (_tBody.childNodes.length > 2)
            _tBody.removeChild(_tBody.childNodes[2]);
    }

    this.clearAll = function() {
        while (_headRow.childNodes.length > 0)
            _headRow.removeChild(_headRow.childNodes[0]);
        while (_tempRow.childNodes.length > 0)
            _tempRow.removeChild(_tempRow.childNodes[0]);
        this.clearData();
    }

    this.addColumn = function(name, caption, editControl, button) {
        //        if (!_headRow) {
        //            _headRow = document.createElement("tr");

        //            if (_tBody.childNodes.lenght > 0)
        //                _tBody.insertBefore(_headRow, _tBody.childNodes[0]);
        //            else
        //                _tBody.appendChild(_headRow);
        //        }

        //        if (!_tempRow) {
        //            _tempRow = document.createElement("tr");
        //            if (_tBody.childNodes.length > 1)
        //                _tBody.insertBefore(_tempRow, _tBody.childNodes[1]);
        //            else
        //                _tBody.appendChild(_tempRow);
        //        }

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


    this.getColumnValues = function(columnName) {

        var cells = document.getElementsByName(columnName);

        var ary = new Array();

        for (var i = 1; i < cells.length; i++) {
            var cell = cells[i];
            ary[i - 1] = DomUtils.getElementValue(cell);
        }

        alert(ary.toString());
        return ary;
    }




    this.getRowCount = function() {
        return _tBody.childNodes.length - 2;
    }

    this.deleteRow = function(rowNo) {
        _tBody.removeChild(_tBody.childNodes[rowNo + 1]);
    }

    this.setColumnValue = function(columnName, rowNo, value) {
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
            DomUtils.setElementValue(el, value, false);
    }

    this.getCellObjectRowNo = function(obj) {
        var cells = document.getElementsByName(obj.name);
        for (var i = 1; i < cells.length; i++) {
            if (obj == cells[i]) {
                return i;
            }
        }
    }

    this.getColumnValue = function(columnName, rowNo) {
        var cells = document.getElementsByName(columnName);
        var el = cells[rowNo];
        if (el.tagName.toLowerCase() == "a")
            return el.href;
        else
            return DomUtils.getElementValue(el);
    }

    this.addRow = function() {
        var r = DomUtils.cloneNode(_tempRow);
        _tBody.appendChild(r);
        r.style.display = "";

        //		alert(_tBody.nodeName);
        //		var s="";

        //		for( var i=0;i<r.childNodes.length;i++){
        //			var node=r.childNodes[i];
        //			var subNode=node.firstChild;
        //			s+=subNode.nodeName+" :" +subNode.name+",";
        //		}
        //		document.body.firstChild;
        //		alert(s);

        //        var cells = document.getElementsByName("fieldList_Id");

        var rowNo = _tBody.childNodes.length - _headRowCount;
        r.onclick = function() {
            _activeRowNo = rowNo;
            if (this.OnRowClick != null) {
                this.OnRowClick(r);
            }
        }

        return r;
    }
}

