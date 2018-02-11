$(function () {
    var user = xBase.User;
    if (user.Name == '游客')
        $(navUserName).html("登录");



    var nav = $.rCall("PcNav.row?" + $.param({ id: $.paramVal("navId") }));
    if (nav.text)
        $("title").html(nav.text);
    $("[nid='" + $.paramVal("navId") + "']").css("color", "#96C4E6");


    $("nav.navbar .navbar-right .dropdown a").click(function () {
        $("#imgWxfx").attr("src",
        "http://qr.liantu.com/api.php?text="
         + location.href
         + "?from=2dcode"
         + "&w=160&m=5"
      );
    });
})

function wxshare() {
    $(shareit).show();
    $("#shareit .close-share").click(function () {
        $(shareit).hide();
    });
}
