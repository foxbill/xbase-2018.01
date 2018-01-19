

function JsonForm(element, obj, fieldInfos, pName) {
    Form.call(this, element);
    var _this = this;
    var joObject = this.getJObj();

    if (!pName)
        pName = joObject.attr("name");

    // var  = obj.FieldInfos;
    var subObjs = new Object();

    this.setData = function (data) {
        for (var fld in data) {
            var fname = !pName ? fld : pName + "." + fld;
            var val = data[fld];
            var jo = this.getField(fname);
            var fi = fieldInfos[fname];

            if (!jo.length)
                continue;

            if (TypeUtils.isArray(val))
                subObjs[fld] = new JsonList(jo, val, fieldInfos, fname);
            else if (TypeUtils.isObject(val))
                subObjs[fld] = new JsonForm(jo, val, fieldInfos, fname);
            else {
                if (fi && fi.Options)
                    jo.options(fi.Options);
                jo.setText(val);
            }

        }
    }

    this.getData = function () {
        for (var fld in obj) {
            var val = data[fld];
            var jo = this.getField(fld);
            //var fi = fieldInfos[fld];
            if (!jo.length) continue;
            var sObj = subObjs[fld];
            if (sObj)
                obj[fld] = sObj.getData();
            else
                obj[fld] = jo.getText();
        }
        return obj;
    }

    this.showa = function () {
        alert("aaa");
    }
    if (obj)
        this.setData(obj);
}

JsonList.constructor = ListView;
//JsonList.prototype = new ListView();

function JsonList(el, obj, fieldInfos) {
    var _this = this;
    ListView.call(this, el);
    this.prototype = new ListView(el);


    var forms = [];

    var pName = this.getJObj().attr("name");

    this.setData = function (obj) {

        this.clearItem();

        //   alert("cleared " + this.jItems.length);
        for (var i = 0; i < obj.length; i++) {
            var val = obj[i];
            this.addDataItem(val);
        }
        //        alert(this.itemCount());
    }

    this.getData = function () {
        if (!obj) obj = [];
        for (var i = 0; i < forms.length; i++) {
            var form = forms[i];
            if (obj.length < i + 1)
                obj.push(form.getData());
            else
                obj[i] = form.getData();
        }
        return obj
    }


    this.addDataItem = function (data) {
        var form = this.prototype.addItem();
        var jsonform = new JsonForm(form.getJObj(), data, fieldInfos, pName)
        forms.push(jsonform);
        return obj;
    }

    this.addItem = function () {
        var form = this.prototype.addItem();
        var jsonform = new JsonForm(form.getJobj(), null, fieldInfos, pName);
        forms.push(jsonform);
        return jsonform;
    }

    this.clearItem = function () {
        this.prototype.clearItem();
        forms = [];
    }
    this.itemCount = function () {
        return forms.length;
    }

    if (obj)
        this.setData(obj);
}

$.fn.extend({
    jsonForm: function (obj, fields) {
        //使用$(this)表示对哪个对象添加了扩展方法，这里是指$('#textbox' )
        JsonForm.call(this, $(this), obj, fields);
        return this;
    }
});

$.fn.extend({
    options: function (optionData) {
        //使用$(this)表示对哪个对象添加了扩展方法，这里是指$('#textbox' )
        var els = $(this).get();
        for (var i = 0; i < els.length; i++) {
            var el = els[i];
            if (el.tagName == "SELECT") {
                el.options.length = 0;
                for (var key in optionData) {
                    el.options.add(new Option(key, optionData[key]));
                }
            }
        }
    }
});
