var v = "?v=0.0.0.61";


function avgChild() {
    $("[avgChild]").each(function () {
        var cols = parseInt($(this).attr("avgChild"));
        var ow = $(this).width() / cols;
        var bd = $(this).children().outerWidth() - $(this).children().width();

        $(this).children()
            .css("overflow-x", "hidden")
            .css("max-width", ow - bd)
            .outerWidth(ow)
        ;

        $(this).children().find("img").outerWidth(
                $(this).children().width()
        );
    })
}

function goto(url, frameId) {
    url += v;
    if (frameId) {
        var iframe = document.getElementById(frameId);
        $("#" + frameId).one("load", function () {
            //  $(headerTitle).html(($("title", iframe.contentWindow.document).html()));
        });
        iframe.src = url;
    }
    else
        window.location = url;
}


$(function () {
    //    size4c()
    //    $(window).resize(function () {
    //        size4c();
    //    });
    avgChild()
    $(window).resize(function () {
        avgChild();
    });

    //加载工具条
    $(".nav-list-item").click(function () {
        var url = $(this).attr("link-url");
        if (url)
            location = url + v;
    });

})


