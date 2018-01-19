excelUtlity = {}

//导出EXCEL
function exportExcel(tableid) {
    //整个表格拷贝到EXCEL中
    var curTbl = document.getElementById(tableid);
    var oXL = new ActiveXObject("Excel.Application");
    //创建AX对象excel
    var oWB = oXL.Workbooks.Add();
    //获取workbook对象
    var xlsheet = oWB.Worksheets(1);
    //激活当前sheet
    var sel = document.body.createTextRange();
    //把表格中的内容移到TextRange中
    sel.moveToElementText(curTbl);
    //全选TextRange中内容
    sel.select();
    //复制TextRange中内容
    sel.execCommand("Copy");
    //粘贴到活动的EXCEL中
    xlsheet.Paste();
    //设置excel可见属性        
    oXL.Visible = true;
    try {
        var fname = oXL.Application.GetSaveAsFilename("save.xls", "Excel Spreadsheets (*.xls), *.xls");
        if (fname) {
            oWB.SaveAs(fname);
        }
    }
    catch (e) {
        print("Nested catch caught " + e);
    }
    finally {
        oWB.Close(savechanges = false);
        oXL.Quit();
        oXL = null;
        //结束excel进程，退出完成
        CollectGarbage();
    }
}


//导入EXCEL到table中
function importExcel(tableid, filePath) {
    try {
        var oXL = new ActiveXObject("Excel.application");
    }
    catch (e) {

    }
    var oWB = oXL.Workbooks.open(filePath);
    var row = null, col = null;
    var tableNode = document.getElementById(tableid);
    oWB.worksheets(1).select();
    var oSheet = oWB.ActiveSheet;
    try {
        if (row == null) { row = oSheet.usedrange.rows.count; } else if (isNaN(row)) { row = oSheet.usedrange.rows.count; }
        if (col == null) { col = oSheet.usedrange.Columns.count; } else if (isNaN(col)) { col = oSheet.usedrange.Columns.count; }
        for (var i = 1; i <= row; i++) {
            var newRow = tableNode.insertRow();
            for (var j = 1; j <= col; j++) {
                var newCell = newRow.insertCell();
                var a = oSheet.Cells(i, j).value.toString() == "undefined" ? "" : oSheet.Cells(i, j).value;
                newCell.innerHTML = a;
            }
        }
    }
    catch (e) {
    }
    oXL.Quit();
    CollectGarbage();
}