/***
*   v=1.2
***/

function WboCtrl(el, data) {
    var _url;
    var _data;
    var _param;

    //加载
    this.load = function (url, param) {
        _url = url;
        _param = param;
        var data = $.rCall(url, param);
        this.data(data);
    };

    //重新加载，翻页
    this.reload = function (param) {
        if (param)
            _param = $.extend(_param, param);
        if (!_url)
            $.messager.alert("err", "not url");
        this.load(_url, _param);
    };

    //查询数据，不翻页：param=null
    this.query = function (query, param) {
        if (param)
            _param = $.extend(_param, param);
        var ary = _url.split("?");
        var objMember = ary[0];
        if (query) {
            this.load(objMember + "?" + $.param(query), _param);
        }
        else
            this.load(objMember, _param);
    }

    this.data = function (data) {
        if (data)
            _data = data;
        return _data;
    }
    this.getUrl = function () {
        return _url;
    }

    this.getParams = function () {
        return _param;
    }

    //动态方法，继承者必须调用；

    //兼容旧版本，之后用 this.data(data)替代；
    //    this.setData = data;

    //兼容旧版本，之后用 this.data()替代；
    //  this.getData = data;

    if (data)
        this.data(data);
}

function JsonForm(element, data, fieldInfos, pName) {
    Form.call(this, element);
    WboCtrl.call(this, element, data);

    var isStrData = false;
    var jq = this.$;
    var _this = this;
    var obj = this.data();
    var sub_wbo_ctrls = {};
    var ctrlName;
    if (jq)
        ctrlName = jq.attr("name");

    if (ctrlName)
        pName = ctrlName;

    var fname = null;

    function jqField(fld) {
        fname = !pName ? fld : pName + "." + fld;
        return _this.getField(fname);
    }

    function bindElementSubProp(el, prop, value) {
        var names = prop.split(".");
        var o = el(names.shift());
        while (names.length > 1)
            o = o[names.shift()];
        o[names.shift()] = value;
    }

    this.getSubCtrl = function (fildName) {
        return sub_wbo_ctrls[fildName];
    };

    //检查attr-bind值计算方式，并且计算
    function checkEval(s, data, ctrlName) {
        function fieldValue(fld) {
            var i = fld.lastIndexOf(".");
            if (i > 0) {
                if (fld.substr(0, i) != ctrlName)
                    return false;
                fld = fld.substr(i + 1);
            }
            return data[fld];
        }

        s = s.trim();
        if (!s) return s;

        //如果值没有被()包括，值是一个字段值
        if ((s[0] != '('))
            return fieldValue(s);

        // s = s.substring(0, s.lastIndexOf(")"));
        //搜寻@fildName ,并替换成字段值
        var flds = /@\w+\.\w+|@\w+/.exec(s);
        for (var i = 0; i < flds.length; i++) {
            var fld = flds[i];
            if (fld) {
                var v = fld.substring(1);
                v = fieldValue(v);

                if (v == false) return false;

                v = "'" + v + "'";

                s = s.replace(fld, v);
            }
        }
        return eval(s);
    }

    function bindAttr(jo, data, ctrlName) {
        var attrs = jo.attr("wbo-attr");
        if (!attrs) return;
        if (attrs[0] != '{')
            attrs = '{' + attrs + '}';
        attrs = eval("(" + attrs + ")");

        for (var a in attrs) {
            var fld = attrs[a];
            var v = checkEval(fld, data, ctrlName);
            //if (data[fld] == 'undefined')
            //    v = fld;
            //jo.attr(a, v);
            var el = jo[0];
            if (a.indexOf(".") > 0)
                bindElementSubProp(el, a, v);
            else if (a == "innerHTML") {
                jo.html(v);
            }
            else if (el[a]) {
                el[a] = v;
            } else
                jo.attr(a, v);

            if (a == "checked")
                debugger;
        }
    }

    function bindChild() {
        var jq_ = jq.find("[wbo-embed='" + pName + "']");
        jq_.each(function () {
            //var name = $(this).attr("name");
            var wboId = $(this).attr("wbo");
            var param = $(this).attr("wbo-params");
            var ctrl = $(this).attr("wbo-ctrl");
            var query = $(this).attr("wbo-query");

            if (!ctrl)
                ctrl = $(this).attr("view");

            if (param) {
                param = eval("(" + param + ")");

                for (var n in param) {
                    var sv = param[n];
                    if ((typeof sv) != "string")
                        continue;
                    var h = pName + ".";
                    var v = sv.replace(h, "");
                    if (obj[v])
                        param[n] = v;
                }
            }



            if (!ctrl)
                ctrl = "jsonForm";

            var ctr = $(this)[ctrl]();

            if (query) {
                if (query[0] != '{')
                    query = '{' + query + '}';
                query = eval("(" + query + ")");
                for (var n in query) {
                    var sv = query[n];
                    if ((typeof sv) != "string")
                        continue;
                    var h = pName + ".";
                    var v = sv.replace(h, "");
                    if (obj[v])
                        query[n] = obj[v];
                }
                wboId = wboId + "?" + $.param(query);
            }
            ctr.load(wboId, param);
        })
    }


    this.setData = function (data) {
        obj = data;
        if (obj)
            isStrData = !TypeUtils.isObject(obj);
        if (!jq) return;
        //兼容旧版本，之后版本data不放Jquery.data("wbo")中，放在this.data()中；
        jq.data("wbo", data);
        //  bindAttr(jq, data);
        jq.find("[wbo-attr]").each(function () {
            bindAttr($(this), data, pName);
        });

        if (jq.attr("wbo-attr"))
            bindAttr(jq, data, pName);

        var selfBind = jq.attr("wbo-bind");
        if (selfBind) {
            var v = selfBind.split(".");
            var fld = v.length > 1 ? v[1] : v[0];
            jq.valu(data[fld]);
        }

        if (isStrData) {
            var jo = jqField("value")
            jo.valu(data);
            setBtnData();
            return;
        }

        for (var fld in data) {
            var jo = jqField(fld);
            //    bindAttr(jo, data);
            var val = data[fld];
            if (!jo || !jo.length)
                continue;
            var fi = null;
            if (fieldInfos && fieldInfos[fld])
                fi = fieldInfos[fld];


            var ctrl = jo.length ? jo[0].ctrl : null;
            if (ctrl)
                ctrl.data(val);
            else if (sub_wbo_ctrls[fld])
                sub_wbo_ctrls[fld].setData(val);
            else if (jo.attr("control")) {
                var strCtrl = jo.attr("control");
                ctrl = new window[strCtrl](jo, val, fieldInfos, fname);
                sub_wbo_ctrls[fld] = ctrl;
            }
            else if (jo.attr("wbo-ctrl")) {
                var strCtrl = jo.attr("wbo-ctrl");
                if (window[strCtrl])
                    ctrl = new window[strCtrl](jo, val, fieldInfos, fname);
                else
                    ctrl = jo[strCtrl](val);
                sub_wbo_ctrls[fld] = ctrl;
            }
            else if (TypeUtils.isArray(val)) {
                sub_wbo_ctrls[fld] = new JsonList(jo, val, fieldInfos, fname);
            }
            else if (TypeUtils.isObject(val)) {
                sub_wbo_ctrls[fld] = new JsonForm(jo, val, fieldInfos, fname);
            }
            else {
                if (fi && fi.Options)
                    jo.options(fi.Options);
                jo.setText(val);

                jo.each(function () {
                    this.dataForm = _this.ctrl;
                });
                //  bindAttr(jo, data);
            }

        }
        setBtnData();
        bindChild();
    }


    this.getData = function () {
        if (isStrData) {
            var jo = jqField(fname);
            return jo.valu();
        }

        if (!obj) {
            obj = {};
            var $flds = jq.find("[wbo-bind]");
            $flds.each(function () {
                var fldName = $(this).attr("wbo-bind");

                if (pName) {
                    var dotId = fldName.lastIndexOf(".");
                    var _ctrlName = fldName.substr(0, dotId);
                    fldName = fldName.substr(dotId + 1);
                    if (_ctrlName != pName)
                        return true;
                }

                if (this.ctrl)
                    obj[fldName] = this.ctrl.getData();
                else
                    obj[fldName] = $(this).valu();
            });

        } else
            for (var fld in obj) {
                var jo = jqField(fld);
                var val = obj[fld];
                //var fi = fieldInfos[fld];
                if (!jo.length) continue;


                var sObj = jo[0].ctrl;
                if (!sObj)
                    sObj = this[fld];

                if (sObj)
                    obj[fld] = sObj.data();
                else {
                    var elVal = jo.valu();

                    //                if (!(typeof val == "string") && Number(elVal) == NaN) {
                    //                    alert(fname + "输入格式有错");
                    //                    jo.focus();
                    //                    throw "输入格式有错";
                    //                }
                    //                else
                    if (elVal == "")
                        elVal = null;
                    obj[fld] = elVal;
                }

            }
        return obj;
    }

    var sup_data = this.data;
    this.data = function (data) {
        if (data) {
            sup_data(data);
            this.setData(data);
        } else {
            return this.getData();
        }
    }

    this.setList = function (list) {
        this.list = list;
        // setBtnData();
    }


    function setBtnData() {
        if (!jq) return;
        var els = jq.find("[name='" + pName + ".operate']");//兼容旧版

        if (!els.length) {
            var bName = pName ? pName + ".operate" : "operate";//兼容旧版
            els = jq.find("[wbo-bind='" + bName + "']");
        }

        if (!els.length) {
            var bName = pName ? pName + ".btn" : "btn";
            els = jq.find("[wbo-bind='" + bName + "']");
        }

        if (!els.length) {
            var bName = pName ? pName + ".row" : "row";
            els = jq.find("[data-get='" + bName + "']");
        }

        els.each(function () {
            this.data = obj;
            this.dataForm = _this;
            this.row = _this;
            if (_this.list)
                this.list = _this.list;
        });
    }


    //  setBtnData();

    if (data) {
        this.data(data);
    }


}

//JsonList.constructor = ListView;
//JsonList.prototype = new ListView();

function JsonList(el, data, fieldInfos) {
    ListView.call(this, el);
    WboCtrl.call(this, el, data);

    var lastPaging = 1;
    var pageEnd = false;

    var obj = data;
    var _this = this;
    var jq = this.$;
    //    this.prototype = new ListView(el);
    var demoData = null;

    var forms = [];
    if (!jq)
        return;
    var pName = jq.attr("name");
    if (!pName)
        pName = jq.attr("id");


    this.init = function () {
        pageEnd = false;
        lastPaging = 1;
    }

    this.setData = function (data) {
        pageEnd = false;
        var params = this.getParams();
        if (params)
            lastPaging = params.page;
        obj = data;
        jq.data("wbo", data);
        if (!demoData && obj && obj.length) {
            demoData = obj[0];
        }

        if (this._fixed_list) {
            forms = [];
            var items = this.items()
            for (var i = 0; i < items().length; i++) {
                if (i < obj.length) {
                    var val = obj[i];
                    buildFormItem(items[i], val);
                } else {
                    items[i].hide();
                }
            }
        } else {
            this.clearItem();
            //   alert("cleared " + this.jItems.length);
            for (var i = 0; i < obj.length; i++) {
                var val = obj[i];
                this.addDataItem(val);
            }
        }
        //        alert(this.itemCount());
    };

    this.getData = function () {
        if (pName == "References")
            debugger;
        obj = obj || [];
        var items = this.items();
        for (var i = 0; i < items.length; i++) {
            var ctrl = items[i].ctrl;
            if (!ctrl)
                ctrl = $(items[i]).data("ctrl");
            if (obj.length < i + 1) {
                var d = ctrl.data();
                obj.push(d);
            }
            else
                obj[i] = ctrl.data();
        }
        //for (var i = 0; i < forms.length; i++) {
        //    var form = forms[i];
        //    if (obj.length < i + 1)
        //        obj.push(form.data());
        //    else
        //        obj[i] = form.data();
        //}
        if (obj == "")
            obj = null;

        return obj
    };


    var supAddItem = this.addItem;
    this.addItem = function () {
        var $item = supAddItem();
        var jsonform = new JsonForm($item, null, fieldInfos, pName);
        if (!TypeUtils.isFunction(jsonform.setList))
            debugger;
        jsonform.setList(_this);

        var $ctrl = $item.find("[wbo-ctrl]");
        $ctrl.each(function () {
            var ctrlName = $(this).attr("wbo-ctrl");
            if ($(this)[ctrlName])
                this.ctrl = $(this)[ctrlName]();
            else if (window[ctrlName])
                this.ctrl = new window[ctrlName](this);
        });
        //     jsonform.reset();
        forms.push(jsonform);
        return jsonform;
    };
    this.addDataItem = function (data) {
        //  if (!demoData) {
        //      alert("列表为空无法添加新行，请从服务器获取列表数据！");
        //      return;
        //  }
        var $item = supAddItem();
        return buildFormItem($item, data);
    }
    ;
    this.addPaging = function (rowCount) {
        if (pageEnd)
            return false;

        var url = this.getUrl();
        var param = this.getParams();

        lastPaging++;

        if (!param)
            param = { page: lastPaging, rows: rowCount };
        else
            param.page = lastPaging;

        //if (!param.rows)
        //    param.rows = 15;

        var data = $.rCall(url, param);
        if (!data.length) {
            lastPaging--;
            param.page = lastPaging;
            pageEnd = true;
            return false;
        }

        for (var i = 0; i < data.length; i++) {
            this.addDataItem(data[i]);
        }

        return true;
    }

    function buildFormItem(jItem, data) {
        var jsonform = new JsonForm(jItem, data, fieldInfos, pName);
        if (!TypeUtils.isFunction(jsonform.setList))
            debugger;
        jsonform.setList(_this);

        forms.push(jsonform);
        return jsonform;
    }

    var moveUp = this.moveUp;
    this.moveUp = function (el) {
        moveUp(el);
    }

    var moveDown = this.moveDown;
    this.moveDown = function (el) {
        moveDown(el)
    }

    var supRemoveItem = this.removeItem;

    this.removeItem = function (el) {
        if ((typeof el) == "JsonForm")
            el = el.$[0];
        else if (!TypeUtils.isElement(el))
            el = el[0];

        var data = el.ctrl.data();
        obj.removeItem(data);
        supRemoveItem(el);
    }
    //测试滚动加载
    //$("listBody").scroll(function () {
    //    var scrollTop = $(this).scrollTop();
    //    var scrollHeight = this.scrollHeight;
    //    var windowHeight = $(this).height();
    //    if (scrollTop + windowHeight == scrollHeight) {
    //        alert("you are in the bottom");
    //    }
    //});

    var sup_data = this.data;
    this.data = function (data) {
        if (data) {
            sup_data(data);
            this.setData(data);
            return data;
        } else
            return this.getData(data);
    }
    this.clearItem();
    if (obj) {
        this.data(obj);
    }
}

function Paging(el, dataCtrl) {
    Control.call(this, el);
    var jq = this.$;
    var rows = jq.attr("rows");
    var total = dataCtrl.data().total;
    var page = 1;
    var pageCount = getPageCount();
    var pageBtns = jq.find("[role='page']");
    var pageBtnCount = pageBtns.length;
    if (!rows) rows = 8;

    setState();

    function setState() {
        if (page <= 1)
            jq.find("[role='prior']").addClass("disabled");
        else
            jq.find("[role='prior']").removeClass("disabled");

        if (page >= pageCount) {
            jq.find("[role='next']").addClass("disabled");
        } else {
            jq.find("[role='next']").removeClass("disabled");
        }

        var os = parseInt($(pageBtns[0]).find("a").html());

        if (!os) {
            os = 1;
        }

        var m = Math.ceil(pageBtns.length / 2);
        var s = page - os + 1 <= m ? os : page - m;
        if (s <= m)
            s = 1;

        pageBtns.each(function () {
            if (s > pageCount)
                $(this).hide();
            $(this).children().html(s);

            if (s == page)
                $(this).addClass("active");
            else
                $(this).removeClass("active");
            s++;
        });
    }

    function getPageCount() {
        return Math.ceil(total / rows);
    }

    jq.find("[role='prior']").click(function () {
        if ($(this).hasClass("disabled"))
            return;
        if (page > 1) {
            page--;
            dataCtrl.reload({ page: page, rows: rows });
        }
        setState();
    })


    jq.find("[role='next']").click(function () {
        if ($(this).hasClass("disabled"))
            return;
        if (page <= pageCount) {
            page++;
            dataCtrl.reload({ page: page, rows: rows });
        }
        setState();
    })

    pageBtns.click(function () {
        page = $(this).children().html();
        dataCtrl.reload({ page: page, rows: rows });
        setState();
    })

}





(function ($) {
    function loadUi() {
        var jq = $("[data-form]");
        jq.each(function () {
            var ds = $(this).attr("data-form");
            if (ds.substr(0, 1) == "(")
                ds = eval(ds);
            var query = $(this).attr("wbo-query");
            var params = $(this).attr("wbo-params");
            var html = $.rCall("DataUi.formHtml", { dsName: ds, wboParams: params, wboQuery: query });
            $(this).html(html);
        });
        var jq = $("[data-list]");
        jq.each(function () {
            var ds = $(this).attr("data-list");
            if (ds.substr(0, 1) == "(")
                ds = eval(ds);
            var query = $(this).attr("wbo-query");
            var params = $(this).attr("wbo-params");
            var html = $.rCall("DataUi.listHtml", { dsName: ds, wboParams: params, wboQuery: query });
            $(this).html(html);
        });
    }
    function bindWbo(jq) {
        if (jq)
            jq = jq.find("[wbo]");
        else
            jq = $("[wbo]");
        jq = jq.not("[wbo-embed]");
        jq.each(function () {
            var name = $(this).attr("name");
            var wboId = $(this).attr("wbo");
            var param = $(this).attr("wbo-params");
            var ctrl = $(this).attr("wbo-ctrl");
            var query = $(this).attr("wbo-query");

            if (!ctrl)
                ctrl = $(this).attr("view");

            if (param)
                param = eval("(" + param + ")");

            if (!ctrl)
                ctrl = "jsonForm";

            var ctr = $(this)[ctrl]();
            if (query) {
                if (query.charAt(0) != '{')
                    query = "{" + query + "}";
                query = eval("(" + query + ")");
                wboId = wboId + "?" + $.param(query);
            }

            ctr.load(wboId, param);
            var paging = ctr.$.attr("paging");
            if (paging)
                paging = new Paging(paging, ctr);

        })
    }

    $.fn.extend({
        jsonForm: function (obj, fields) {
            var ctrl = $(this).data("ctrl");
            if (!ctrl) {
                ctrl = new JsonForm(this, obj, fields);
                $(this).data("ctrl", ctrl);
            } else
                ctrl.data(obj);

            return ctrl;
        }
    });

    $.fn.extend({
        jsonList: function (obj, fields) {
            var ctrl = $(this).data("ctrl");
            if (!ctrl) {
                ctrl = new JsonList(this, obj, fields);
                $(this).data("ctrl", ctrl);
            }
            else {
                ctrl.data(obj);
            }
            return ctrl;
        }
    });

    var oldLoad = $.fn.load;
    $.fn.extend({
        load: function (url, data, fn) {
            var jq = this;
            if (TypeUtils.isFunction(data))
                oldLoad.call(this, url, function () {
                    bindWbo(jq);
                    if (data) data();
                });
            else
                oldLoad.call(this, url, data, function () {
                    bindWbo.call(jq);
                    if (fn) fn(data);
                });
        }
    });

    //捆绑Wbo对象
    $(function () {
        if (location.protocol == "file:")
            return;
        loadUi();
        bindWbo();
    });

    //查找滑屏加载
    $(function () {
        var bot = 200;
        $loading = $("#loading");
        function scrollEnd() {
            var iscroll = this;
            var $scroller = $(iscroll.scroller);
            if (Math.abs(iscroll.y) + bot > $scroller.height() - $(iscroll.wrapper).height()) {
                $loading.show();
                setTimeout(function () {
                    if ($scroller.jsonList().addPaging()) {
                        setTimeout(function () {
                            iscroll.refresh();
                            $loading.hide();
                        }, 50);
                    }
                }, 50);
            }
        }

        if (window.IScroll && xBase.Agent.isWx) {
            $("[data-scroll]").each(function () {
                $(this).css("overflow", "hidden");
                var op = $(this).attr("data-scroll");
                op = eval("(" + op + ")");
                op.click = true;
                this.xbaseIScroll = new IScroll(this, op);
                this.xbaseIScroll.on("scrollEnd", scrollEnd);
            });
        }

        $("[data-slid-load]").each(function () {
            var option = eval("(" + $(this).attr("data-slid-load") + ")");

            var $slidList = $(this);

            //            $loading = $("#loading");
            $loading.hide();


            var $ScrollBox = $(window);
            if (option.scrollBox)
                $ScrollBox = $(option.scrollBox);

            var $Scroll = $(document);
            if (option.scroll)
                $Scroll = $(option.scroll);

            var loading = false;
            // var hasNext = true;
            $ScrollBox.scroll(function () {
                //$(window).scrollTop()这个方法是当前滚动条滚动的距离
                //$(window).height()获取当前窗体的高度
                //$(document).height()获取当前文档的高度
                //   if (!hasNext) return;
                if (loading) return;
                // var bot = 200; //bot是底部距离的高度
                if ((bot + $ScrollBox.scrollTop()) >= ($Scroll.height() - $ScrollBox.height())) {
                    //当底部基本距离+滚动的高度〉=文档的高度-窗体的高度时；
                    //我们需要去异步加载数据了
                    //$.getJSON("url", { page: "2" }, function (str) { alert(str); });
                    loading = true;
                    $loading.show();
                    setTimeout(function () {
                        //hasNext =
                        $slidList.jsonList().addPaging();
                        $loading.hide();
                        //    $(pageLoading).show();
                        //$("[wbo='MaGoods.rows']").jsonList().addPaging();
                        //  $(pageLoading).hide();
                        loading = false;
                    }, 100);
                }
            });

        });

    });

})(jQuery);