function inputDlg(dlgCaption, hint, validationFun) {
    var args = new Object();
    args.dlgCaption = dlgCaption;    
    args.hint = hint;
    args.validation = validationFun;    
    return window.showModalDialog("/xbase.js/dlgs/input_dlg.html", args,
               "dialogHeight:150px;dialogWidth:450px;center:yes;resizable:yes;scroll:no");
}