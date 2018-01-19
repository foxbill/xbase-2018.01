var isClick = 0;
$(document).ready(function () {
    showbtn();//下一步按钮显示
    var menu_ul_lis = $("#usermenu li");

    $("#usermenu .all ul li .plus").click(function () {
        plus(this)
    });

    $("#usermenu .all ul li .minus").click(function () {
        minus(this);
    });


    $("#ILike").click(function () {
        var menu_ul_lis = $(".all li.like");
        //alert(menu_ul_lis.length)
        var like_ul = $(".likediv ul");
        like_ul.html("");
        for (var i = 0; i < menu_ul_lis.length; i++) {
            var li = document.createElement("li");
            li.id = menu_ul_lis[i].id;
            li.className = menu_ul_lis[i].className;
            li.innerHTML = menu_ul_lis[i].innerHTML;
            like_ul.append(li);
            $("input", $(li)).val($("input", $(menu_ul_lis[i])).val())
        }

        $(".likediv").show();
        $(".all").hide();
        $("#typeList li").removeClass("on");
        $(this).addClass("on");

        $("#usermenu .likediv ul li .plus").click(function () {
            var li = $(this).parent().parent().parent().parent();
            var li_id = li.get(0).id
            plus(this, li_id);

        });
        $("#usermenu .likediv ul li .minus").click(function () {
            var li = $(this).parent().parent().parent().parent();
            var li_id = li.get(0).id
            minus(this, li_id);
        });
        $("#usermenu  ul li img").click(function () {
            showDetails(this);
        });
    });


    $("#usermenu  ul li img").click(function () {
        showDetails(this);
    });

    $(".details  .content a").click(function () {
        hideDetails();
    });
    $(".details  .bottom").click(function () {
        hideDetails();
    });
    $("#typeList li").click(function () {
        var idStr = this.id;
        var id = idStr.replace("li_type", "");
        $("#typeList li").removeClass("on");
        $("#ILike").removeClass("on");
        $(this).addClass("on")
        $(".likediv").hide();
        $("#ILike .hartblckgray").removeClass("on");
        $(".all").show();
        var top = $("#ul_type" + id + "").get(0).offsetTop;
        //alert(top)
        var topM = $("#usermenu").get(0).scrollHeight - $("#usermenu").get(0).offsetHeight;
        var st = $("#usermenu").scrollTop();
        if (st <= topM) {
            if (top <= topM) {
                $("#usermenu").scrollTop(top);
            }
            else {
                $("#usermenu").scrollTop(topM);
            }
        }
        isClick = 1;


    });
    $("#usermenu").get(0).onscroll = function () {
        if (isClick == 1) {
            isClick = 0;
            return false;
        }
        var st = $("#usermenu").scrollTop();
        var uls = $("#usermenu .all ul");
        var h = 0;
        if (uls.length > 0) {
            h = $(uls[0]).height();
        }
        if (st > 0) {
            for (i = 1; i < uls.length; i++) {
                var h_ul = $(uls[i]).height() + h;
                if (h < st & st < h_ul) {
                    var u_id = uls[i].id;
                    id = u_id.replace("ul_type", "");
                    $("#typeList li").removeClass("on");
                    $("#li_type" + id + "").addClass("on");
                    return;
                }
                h = h_ul;

            }
        }
        else {
            $("#typeList li").removeClass("on");
            $($("#typeList li")[0]).addClass("on");
        }
    }
    setcart();
});
var ischeckOK = 0;
function setcart() {
    var _count = $("#input_menucount").val();
    if (_count == 0) {
        $("#menucount").html("");
        $("#allmoney").parent().hide();
        $("#menucount").addClass("cart");
        $("#btn_submit").addClass("gray");
        ischeckOK = 0;
    }
    else {
        ischeckOK = 1;
        $("#menucount").html(_count);
        $("#allmoney").parent().show();
        $("#menucount").removeClass("cart");
        $("#btn_submit").removeClass("gray");
        //if (_count > 99) {
        //    $("#menucount").html("");
        //    $("#menucount").addClass("slh");
        //}
        //else {
        //    $("#menucount").removeClass("slh");
        //}
    }
}
function showDetails(elm) {
    $(".details").show();
    showDetailInfo(elm);
}
function showDetailInfo(elm) {
    var li = $(elm).parent().parent();
    var classname_li = li[0].className;
    if (classname_li == "like") {
        $(".details .heart.red").show();
        $(".details .heart.gray").hide();
    }
    else {
        $(".details .heart.red").hide();
        $(".details .heart.gray").show();
    }
    var des = elm.alt;
    var name = $(">label>span", li).html();
    var money = $(">label>label .price_n", li).html();
    var price_str = $(".price", li).html();
    var num = $("input", li);
    var count = num.get(0).value;
    $(".details input").val(count)
    if (count == 0) {
        $(".details .minus").hide();
        $(".details input").hide();
    }
    else {
        $(".details .content>label>label").addClass("border");
        $(".details .minus").show();
        $(".details input").show();
    }
    $(".details .heart.gray").get(0).onclick = function () {
        li.toggleClass("like");
        var id = li.get(0).id.replace("li", "");
        check_i_like(id);
        //$("#usermenu .all ul #"+li.get(0).id+"").removeClass("like");
        //$("#usermenu .likediv ul #"+li.get(0).id+"").remove();	
        $(".details .heart.red").show();
        $(".details .heart.gray").hide();
    }
    $(".details .heart.red").get(0).onclick = function () {
        li.toggleClass("like");
        var id = li.get(0).id.replace("li", "");
        check_i_like(id);
        $("#usermenu .all ul #" + li.get(0).id + "").removeClass("like");
        $("#usermenu .likediv ul #" + li.get(0).id + "").remove();
        $(".details .heart.red").hide();
        $(".details .heart.gray").show();
    }

    $(".details .minus").get(0).onclick = function () {
        var L_elm = $(".details .content>label>label")
        var m_elm = $(".details .minus");
        var num = $(".details input");
        var nstr = num.val();
        if (nstr == "") { nstr = 0 }
        var n = parseInt(nstr) - 1;
        if (n >= 0) { num.val(n); }

        if (n == 0) {
            m_elm.hide();
            num.hide();
            L_elm.removeClass("border");
        }
        minus($(".minus", li).get(0))
    };

    $(".details .plus").get(0).onclick = function () {
        var L_elm = $(".details .content>label>label")
        var m_elm = $(".details .minus");
        var num = $(".details input");
        var nstr = num.val();
        if (nstr == "") { nstr = 0 }
        var n = parseInt(nstr) + 1;
        num.val(n);
        if (n > 0) {
            m_elm.show();
            num.show();
            L_elm.addClass("border")
        }
        plus($(".plus", $(".all #" + li.get(0).id + "")).get(0));
        plus($(".plus", $(".likediv #" + li.get(0).id + "")).get(0));
    };

    $(".details .name").html(name);
    $(".details .price").html(price_str);
    $(".details p").html(des);
    $(".details img").get(0).src = elm.src;
    var p_ts = $('.p_ts', li).html();
    if (p_ts == undefined) {
        $(".details .p_ts").hide();
    }
    else {
        $(".details .p_ts").show();
        $(".details .p_ts").html(p_ts);
    }
}


function hideDetails() {
    $(".details").hide();
}

function plus(elm,id, c) {
    var L_elm = $(elm).parent();
    var m_elm = $(".minus", L_elm);
    var num = $("input", L_elm);
    var nstr = num.val();
    if (nstr == "") { nstr = 0 }
    var maxNum = "";
    if (!isNaN(num.attr("max"))) {
        maxNum = num.attr("max");
    }

    var n = parseInt(nstr) + 1;
    if (c) n = c;

    if (maxNum == "" || (maxNum >= n) || maxNum == "-1") {

        num.val(n);
        if (n > 0) {
            m_elm.show();
            num.show();
            L_elm.addClass("border")
        }

        if (id != undefined && id!=null) {
        	var allListNum_elm = $("#usermenu .all ul #"+id+" input")
        	allListNum_elm.val(n);
        	if(n>0){
        		 $("#usermenu .all ul #"+id+" .minus").show();
        		 allListNum_elm.show();
        	}
        }

        var p = parseFloat($(".price_n", L_elm.parent().parent()).html());

        if (c)
            p = p * c;

        var allmoney_elm = $("#allmoney");
        var allmoney = parseFloat(allmoney_elm.html());
        var c_allmoney = parseFloat(allmoney + p).toFixed(2);
        if (!isNaN(c_allmoney)) {
            allmoney_elm.html(c_allmoney);
        }

        var menucount_elm = $("#input_menucount");
        var menucount = parseInt(menucount_elm.val());

        var mc = menucount + 1;

        if (c) mc = menucount + c

        menucount_elm.val(mc);
        $("#menucount").html(mc);
        setcart();
        showbtn();
    }


}
function minus(elm, id) {
    var L_elm = $(elm).parent();
    var m_elm = $(elm);
    var num = $("input", L_elm);
    var nstr = num.val();
    if (nstr == "") { nstr = 0 }
    var n = parseInt(nstr) - 1;
    if (n >= 0) { num.val(n); }

    if (n == 0) {
        m_elm.hide();
        num.hide();
        L_elm.removeClass("border");
    }
    if (id != undefined) {
        var allListNum_elm = $("#usermenu .all ul #" + id + " input")
        allListNum_elm.val(n);
        if (n == 0) {
            $("#usermenu .all ul #" + id + " .minus").hide();
            allListNum_elm.hide();
        }
    }

    var p = parseFloat($(".price_n", L_elm.parent().parent()).html())
    var allmoney_elm = $("#allmoney");
    var allmoney = parseFloat(allmoney_elm.html())
    var c_allmoney = parseFloat(allmoney - p).toFixed(2)
    allmoney_elm.html(c_allmoney);

    var menucount_elm = $("#input_menucount");
    var menucount = parseInt(menucount_elm.val());
    var mc = menucount - 1;
    menucount_elm.val(mc);
    $("#menucount").html(mc);
    setcart();
    showbtn();

}

function showbtn() {
    var mc = parseInt($("#menucount").html())
    if (mc == 0) {
        $(".btn.show").hide()
        $(".btn.disabled").show()
    } else {
        $(".btn.show").show()
        $(".btn.disabled").hide()
    }
}
function addmark(elm) {
    var li = $(elm).parent().parent().parent();
    $(".markinput", li).toggle();
}

function cancel() {
    hidepopup();
}


function getMyMenulist() {
    var lis = $("#usermenu li");
    var list = [];
    for (i = 0; i < lis.length; i++) {
        var mark = $(".markinput", lis[i]).get(0).value;
        var count = $(".num >input", lis[i]).get(0).value;
        if (count > 0) {
            var id = lis[i].id;
            var info = { 'id': id, 'count': count, 'mark': mark }
            list.push(info);
        }

    }
    var allmark = $("#allmark").get(0).value;
    return { 'data': list, mark: allmark };
}
function getMenuChecklist() {
    var lis = $("#usermenu .all li");
    var list = [];
    for (i = 0; i < lis.length; i++) {
        var count = $(".num >input", lis[i]).get(0).value;
        if (count > 0) {
            var id = lis[i].id.replace("li", "");

            var info = { 'id': id, 'count': count }
            list.push(info);
        }

    }

    return list;
}
