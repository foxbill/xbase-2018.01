
function ListTable(table) {
    var _table;
    var _tBody;
    var _tempRow;


    if (typeof (table) == "object")
        _table = table;
    else
        _table = document.getElementById(table);

    _tBody = table.childNodes[0];

    if (_tBody != null && _tBody.nodeName == "TBODY")
        _tempRow = _tBody.childNodes[1];
    else {
        _tempRow = _tBody
        _tBody = _table;
    }

    /**********************
     *   Event
     **********************/
    this.OnRowClick = null;

    this.Clear = function() {
        while (_tBody.childNodes.length > 2)
            _tBody.removeChild(_tBody.childNodes[2]);
    }

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
        return _tBody.childNodes.length - 2;
    }

    this.DeleteRow = function(rowNo) {
        _tBody.removeChild(_tBody.childNodes[rowNo + 1]);
    }

    this.SetColumnValue = function(columnName, rowNo, value) {
        var cells = document.getElementsByName(columnName);

        if (cells.length < 1) {
            alert(_table.nodeName + "中，没有发现列" + columnName);
            return;
        }

        if (_tBody.childNodes.length < rowNo) {
            while (_tBody.childNodes.length < rowNo)
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

    this.AddRow = function() {
        var r = _tempRow.cloneNode(true);
        _tBody.appendChild(r);
        r.style.display = "";
        var cells = document.getElementsByName("fieldList_Id");

        if (this.OnRowClick != null) {
            r.onclick = function() {
                this.OnRowClick(r);
            }
        }

        return r;
    }


}

