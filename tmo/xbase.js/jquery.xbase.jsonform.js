

function WboCtrl(el, data) {
    var _url;
    var _data;
    var _param;
    this.load = function (url, param) {
        _url = url;
        _param = param;
        var data = $.rCall(url, param);
        this.data(data);
    };

    this.reload = function (param) {
        if (param)
            _param = param;
        if (!_url)
            $.messager.alert("err", "not url");
        this.load(_url, _param);
    };


    this.data = function (data) {
        if (data)
            _data = data;
        return _data;
    }

    //动态方法，继承者必须调用；

    //兼容旧版本，之后用 this.data(data)替代；
    this.setData = data;

    //兼容旧版本，之后用 this.data()替代；
    this.getData = data;

    if (data)
        this.data(data);
}

function JsonForm(element, data, fieldInfos, pName) {
    Form.call(this, element);
    WboCtrl.call(this, element, data);


    var isStrData = false;
    var jq = this.jq();
    var _this = this;
    var obj = this.data();

    if (!pName)
        pName = jq.attr("name");
    // if (!pName)
    //    pName = jq.attr("id");

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

    function bindAttr(jo, data) {
        var attrs = jo.attr("wbo-attr");
        if (!attrs) return;
        if (attrs[0] != '{')
            attrs = '{' + attrs + '}';
        attrs = eval("(" + attrs + ")");

        for (var a in attrs) {
            var v = data[attrs[a]];
            jo.attr(a, v);
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

        //兼容旧版本，之后版本data不放Jquery.data("wbo")中，放在this.data()中；
        jq.data("wbo", data);
        bindAttr(jq, data);

        if (isStrData) {
            var jo = jqField("value")
            jo.valu(data);
            return;
        }
        for (var fld in data) {
            var jo = jqField(fld);
            var val = data[fld];
            if (!jo || !jo.length)
                continue;
            var fi = null;
            if (fieldInfos && fieldInfos[fld])
                fi = fieldInfos[fld];


            var ctrl = jo.length ? jo[0].ctrl : null;
            if (ctrl)
                ctrl.data(val);
            else if (this[fld])
                this[fld].setData(val);
            else if (jo.attr("control")) {
                var strCtrl = jo.attr("control");
                ctrl = new window[strCtrl](jo, val, fieldInfos, fname);
                this[fld] = ctrl;
            }
            else if (jo.attr("wbo-ctrl")) {
                var strCtrl = jo.attr("wbo-ctrl");
                ctrl = new window[strCtrl](jo, val, fieldInfos, fname);
                this[fld] = ctrl;
            }
            else if (TypeUtils.isArray(val)) {
                this[fld] = new JsonList(jo, val, fieldInfos, fname);
            }
            else if (TypeUtils.isObject(val)) {
                this[fld] = new JsonForm(jo, val, fieldInfos, fname);
            }
            else {
                if (fi && fi.Options)
                    jo.options(fi.Options);
                jo.setText(val);

                jo.each(function () {
                    this.dataForm = _this.ctrl;
                });
                bindAttr(jo, data);
            }

        }
        bindChild();
    }

    this.reset = function () {
        if (isStrData) {
            var fld = "value";
            var fname = !pName ? fld : pName + "." + fld;
            var jo = this.getField(fname);
            jo.valu("");
            return;
        }
        for (var fld in obj) {
            var fname = !pName ? fld : pName + "." + fld;
            var val = obj[fld];
            var jo = this.getField(fname);

            var fi = null;
            if (fieldInfos && fieldInfos.length)
                fi = fieldInfos[fld];

            if (!jo.length)
                continue;

            if (TypeUtils.isArray(val))
                this[fld].clearItem();
            else if (TypeUtils.isObject(val))
                this[fld].reset();
            else {
                if (fi && fi.Options)
                    jo.options(fi.Options);
                jo.valu("");
            }

        }
    }

    this.getData = function () {
        if (isStrData) {
            var jo = jqField(fname);
            return jo.valu();
        }
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



    function setBtnData() {
        var els = jq.find("[name='" + pName + ".operate']");//兼容旧版

        if (!els.length) {
            var bName = pName ? pName + ".operate" : "operate";
            els = jq.find("[wbo-bind='" + bName + "']");
        }
        els.each(function () {
            this.data = obj;
            this.dataForm = _this;
            this.row = _this;
        });
    }

    if (obj)
        this.data(obj);

    setBtnData();

}

//JsonList.constructor = ListView;
//JsonList.prototype = new ListView();

function JsonList(el, data, fieldInfos) {
    ListView.call(this, el);
    WboCtrl.call(this, el, data);
    var obj = data;
    var _this = this;
    var jq = this.jq();
    //    this.prototype = new ListView(el);
    var demoData = null;

    var forms = [];

    var pName = jq.attr("name");
    if (!pName)
        pName = jq.attr("id");

    this.setData = function (data) {
        obj = data;
        jq.data("wbo", data);
        if (!demoData && obj && obj.length) {
            demoData = obj[0];
        }

        if (this._fixed_list) {
            forms = [];
            for (var i = 0; i < this.items.length; i++) {
                if (i < obj.length) {
                    var val = obj[i];
                    buildFormItem(this.items[i], val);
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
        if (!obj) obj = [];
        for (var i = 0; i < forms.length; i++) {
            var form = forms[i];
            if (obj.length < i + 1)
                obj.push(form.data());
            else
                obj[i] = form.data();
        }
        if (obj == "")
            obj = null;

        return obj
    };


    var supAddItem = this.addItem;
    this.addItem = function () {
        var form = supAddItem();
        var jsonform = new JsonForm(form.jq(), $.extend(true, {}, demoData), fieldInfos, pName);
        //   jsonform.reset();
        jsonform.list = this;
        forms.push(jsonform);
        return jsonform;
    };

    var supClearItem = this.clearItem;
    this.clearItem = function () {
        supClearItem();
        forms = [];
    }

    var supRemoveItem = this.removeItem;
    this.removeItem = function (form) {
        var list = jq.data("wbo");
        var item = form.getJObj().data("wbo");
        list.removeItem(item);

        supRemoveItem(form);
        forms.removeItem(form);
    }

    function buildFormItem(jItem, data) {
        var jsonform = new JsonForm(jItem, data, fieldInfos, pName);
        jsonform.list = this;
        forms.push(jsonform);
        return jsonform;
    }

    this.addDataItem = function (data) {
        //  if (!demoData) {
        //      alert("列表为空无法添加新行，请从服务器获取列表数据！");
        //      return;
        //  }
        var form = supAddItem();
        return buildFormItem(form.jq(), data)
    };


    this.itemCount = function () {
        return forms.length;
    }

    var sup_data = this.data;
    this.data = function (data) {
        if (data) {
            sup_data(data);
            this.setData(data);
        }
        return this.getData();
    }

    if (obj) {
        this.clearItem();
        this.data(obj);
    }
}






(function ($) {
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
        })
    }

    $.fn.extend({
        jsonForm: function (obj, fields) {
            return new JsonForm(this, obj, fields);
        }
    });

    $.fn.extend({
        jsonList: function (obj, fields) {
            return new JsonList(this, obj, fields);
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
        bindWbo();
    });

})(jQuery);