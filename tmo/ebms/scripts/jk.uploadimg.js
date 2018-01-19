	//添加图片的方法
function showImage(url) {

    var newItem =$(ShowPanel);
    //newItem.attr("id", "");
    newItem.find("img").attr({"src":url}).removeAttr("width");
    //$(lastImage).before(newItem);
}

function showImages(fileInput, urlCreater) {
    for (var i = 0; i < fileInput.files.length; i++) {
        var url = urlCreater.createObjectURL(fileInput.files[i]);
        showImage(url);
    }
}

function getFileUrl(fileInput) {

    if (!fileInput.files.length)
        return;

    if (window.webkitURL) {
        showImages(fileInput, window.webkitURL);
    }
    else if (window.FileReader) {
        for (var i = 0; i < fileInput.files.length; i++) {
            var reader = new FileReader();
            reader.onload = function (e) {
                if (!e.target.result)
                    return;
                showImage(e.target.result);
            }
            reader.readAsDataURL(fileInput.files[i]);
        }
    } else if (window.URL) {
        showImages(fileInput, window.URL);
    } else {
        alert("请使用火狐、谷歌、或 IE10以上浏览器");
    }
    //   $.messager.alert("请使用火狐、谷歌、或 IE10以上浏览器");

}
function addImage(obj) {
    $(obj).change(function () {
        var input = $(obj)[0]
        getFileUrl(input);
    });
    ShowPanel;
    $(obj).click();
}
function addImage(obj, showPanel) {
    $(obj).change(function () {
        var input = $(obj)[0]
        getFileUrl(input);
    });
    ShowPanel = $(showPanel);
    $(obj).click();
}