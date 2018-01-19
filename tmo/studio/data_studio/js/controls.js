
function ListTable(table) {
    var _table;
    var _tBody;
    var _tempRow;

    //    bill.aa();
    this.OnRowClick = null;

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

    //    if (hasSelectCol && _tempRow.childNodes.length > 0) {
    //        if (_tBody.childNodes.length > 2) {
    //            var headRow = _tBody.childNodes[0];
    //            var headcol = headRow.firstChild.cloneNode(false);
    //            headRow.insertBefore(headcol, headRow.firstChild);
    //        }
    //        var selcol = _tempRow.firstChild.cloneNode(false);
    //        _tempRow.insertBefore(selcol, _tempRow.firstChild);
    //        var chk = document.createElement("<input   type='checkbox'   name='"+table.id+"_Sel'>");
    //        selcol.appendChild(chk);
    //        bill.aa();
    //    }

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

    this.SetColumnValue = function(columnName, rowNo, value) {
        //       document.body.childNodes.length
        //       if(_tBody.childNodes.length<rowNo)
        var cells = document.getElementsByName(columnName);
        //    alert(cells);
        //    alert(cells.length);
        if (cells.length < 1) {
            alert(_table.nodeName + "中，没有发现列" + columnName);
            return;
        }

        if (_tBody.childNodes.length < rowNo) {
            while (_tBody.childNodes.length < rowNo)
                this.AddRow();
            cells = document.getElementsByName(columnName);
        }
        //        alert(value);
        //              alert(cells[rowNo].nodeName);
        DomUtil.SetElementValue(cells[rowNo], value, false);
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



function TabItem(tabLabel, tabPanel) {
    this.TabPanel = null;
    this.TabLabel = null;

    if (typeof (tabLabel) == "object")
        this.TabLabel = tabLabel;
    else {
        this.TabLabel = document.getElementById(tabLabel);
        if (this.TabLabel == null) return null;
    }

    if (typeof (tabPanel) == "object")
        this.TabPanel = tabPanel;
    else {
        this.TabPanel = document.getElementById(tabPanel);
        if (this.TabPanel == null) return null;
    }
}

var XDialog = {
    ShowDlg: function(url, setData, width, height) {
        var _minSize = 50;
        var _width = 600;
        var _height = 300;

        if (width != null && width >= 0) {
            _width = width;
        }

        if (height != null && height >= 0) {
            _height = height;
        }

        if (_width < _minSize) {
            _width = _minSize;
        }

        if (_height < _minSize) {
            _height = _minSize;
        }

        //主窗体信息
        var pwnd = window.top;
        var blWidth = pwnd.document.body.clientWidth;
        var blHeight = pwnd.document.body.clientHeight;
        var titleHeight = 30;
        var buttonBarHeight = 50;

        //创建遮挡
        var div = window.top.document.createElement("DIV");

        div.style.position = "absolute";
        div.style.top = 0;
        div.style.left = 0;
        //    div.style.right = 0;
        //    div.style.bottom = 0;
        //    div.style.overflow = "hidden";
        div.style.filter = "Alpha(opacity=90)"
        //             filter:Alpha(opacity=30）
        //      div.style.border = "1px solid #0fa0db";

        div.style.width = blWidth;
        div.style.height = blHeight;

        div.style.backgroundColor = "blue";

        //            div.style.
        window.top.document.body.appendChild(div);
        // FieldListDiv.

        var frame = window.top.document.createElement("IFRAME");
        frame.width = "100%";
        frame.height = "100%";
        frame.style.backgroundColor = "red";
        frame.frameBorder = 0;
        div.appendChild(frame);

        //创建对话框
        var dlgTop = (blHeight - _height) / 2;
        var dlgLeft = (blWidth - _width) / 2;

        //对话框窗体
        var dlgDiv = window.top.document.createElement("DIV");
        dlgDiv.style.position = "absolute";
        dlgDiv.style.top = dlgTop;
        dlgDiv.style.left = dlgLeft;
        dlgDiv.style.width = _width;
        dlgDiv.style.height = _height;
        dlgDiv.style.backgroundColor = "red";
        div.appendChild(dlgDiv);


        //对话框标题
        var dlgTitle = window.top.document.createElement("DIV");
        dlgTitle.style.height = titleHeight;
        dlgTitle.style.backgroundColor = "blue";
        dlgDiv.appendChild(dlgTitle);

        //对话框页面
        var frame = window.top.document.createElement("IFRAME");
        //        frame.style.position = "absolute";
        //        frame.style.top = titleHeight;
        frame.style.width = "100%";
        frame.style.height = _height - titleHeight - buttonBarHeight;
        frame.src = url;
        frame.name = "dlgFrame";
        frame.id = "dlgFrame;"
        dlgDiv.appendChild(frame);

        //对话框按钮条
        var buttonBar = window.top.document.createElement("DIV");
        //    buttonBar.style.position = "absolute";
        //    buttonBar.style.top =_height -  buttonBarHeight;
        buttonBar.style.height = buttonBarHeight;
        buttonBar.style.backgroundColor = "gray";
        dlgDiv.appendChild(buttonBar);

        var btnOk = window.top.document.createElement("<input type='button' value='确定'>");
        buttonBar.appendChild(btnOk);



        var btnCancel = window.top.document.createElement("<input type='button' value='取消'>");
        buttonBar.appendChild(btnCancel);

        btnCancel.onclick = function() {
            //    bill.bil();
            div.removeNode(true)

        }



        var dataFieldsDoc = frame.contentWindow.document;
        dataFieldsDoc.onreadystatechange = function() {
            if (dataFieldsDoc.readyState == "complete") {
                dataFieldsDoc.parentWindow.SetData(setData);
                btnOk.onclick = function() {
                    dataFieldsDoc.parentWindow.UpdateData(setData);
                }
            }

        }

    }
}



function TabPage() {
    var _tabs = new Array();
    var _curTab = null;



    function SetCurTab(tab) {
        tab.TabPanel.style.display = "";
        for (var i = 0; i < _tabs.length; i++) {
            if (_tabs[i] != tab)
                _tabs[i].TabPanel.style.display = "none";

            _curTab = tab;
        }
    }

    function TabClick(tab) {
        SetCurTab(tab);
    }

    function InitTab(tab) {
        //     var tab = new TabItem();
        tab.TabPanel.style.display = "none";
        tab.TabLabel.onclick = function() {
            TabClick(tab);
        }
    }

    this.AddTab = function(tabLabel, tabPanel) {
        var tab = new TabItem(tabLabel, tabPanel);
        if (tab != null) {

            _tabs.push(tab);

            InitTab(tab);

            if (_curTab == null) SetCurTab(tab);

        }
    }

}