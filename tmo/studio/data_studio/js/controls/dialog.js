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
        // div.style.filter = "Alpha(opacity=90)"
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
        dlgDiv.style.filter = "Alpha(opacity=100)";
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
            div.removeNode(true);

        }



        var dataFieldsDoc = frame.contentWindow.document;

        dataFieldsDoc.onreadystatechange = function() {
            if (dataFieldsDoc.readyState == "complete") {
                dataFieldsDoc.parentWindow.SetData(setData);
                btnOk.onclick = function() {
                    dataFieldsDoc.parentWindow.UpdateData(setData);
                    div.removeNode(true);

                }
            }

        }

    }
}
