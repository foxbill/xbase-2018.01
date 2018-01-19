
function Control(el) {

    var joForm = el;
    var _this = this;

    if (TypeUtils.isElement(el) || TypeUtils.isString(el)) {
        joForm = $(el);
    }

    joForm.each(function () {
        this.ctrl = _this;
    });

    //淘汰方法
    this.getJObj = function () {
        return joForm;
    }

    //淘汰方法
    this.getJQuery = function () {
        return joForm;
    }

    this.jq = function () {
        return joForm;
    }
}

function Form(el) {
    Control.call(this, el);
    var joForm = this.jq();

    this.setValue = function (name, value) {
        var cell = this.getField(name);
        return cell.setText(value);
    }

    this.getField = function (name) {
        var joField = joForm.find("[name='" + name + "']");
        if (joField.length == 0) {
            joField = joForm.find("[wbo-bind='" + name + "']");
        }

        return joField;
    }

    this.getValue = function (name) {
        var cell = this.getField(name);
        return cell.getText();
    }
}

function ListView(el, css) {
    Control.call(this, el);
    var _this = this;
    var _headBody = null;
    var _listBody = null;
    var _rowBody = null;
    var activeJoItem = null;
    var jItems = new Array();
    var joList = this.getJQuery();
    var elName = joList.attr("name");

    this._fixed_list = false;

    if (!elName)
        elName = joList.attr("id");

    var C = {
        css: {
            mo: "mo",
            act: "act"
        },
        list: {
            attr: "list",
            head: "hr",
            item: "ir"
        },
        obj: {
            attr: "obj"
        }
    };

    if (!css)
        css = C.css;


    //获取列表头
    function getHeadBody() {
        _headBody = joList.find("[" + C.list.attr + "='" + C.list.head + "']");//兼容旧版本
        _headBody = _headBody.filter("[" + C.obj.attr + "='" + elName + "']");//兼容旧版本
        if (_headBody.length == 0) //兼容旧版本
            _headBody = joList.find("[name='" + elName + ".head']");//兼容旧版本


        if (_headBody.length == 0) {
            var bName = !elName ? "head" : elName + ".head";
            _headBody = joList.find("[wbo-bind='" + bName + "']");
        }
    }

    //获取列表体
    function getListBody() {
        //        var rCells = $(el).find("." + css.ic);
        //        if (rCells.length < 1)
        //            alert("不能发现数据行单元格，列表不能正常工作，请用class='ic'标记列表行");
        //        var r = rCells.parent();
        var ir = joList.find("[" + C.list.attr + "='" + C.list.item + "']");//兼容旧版本
        ir = ir.filter("[" + C.obj.attr + "='" + elName + "']");//兼容旧版本
        if (ir.length == 0)//兼容旧版本
            ir = joList.find("[name='" + elName + ".item']");//兼容旧版本

        if (ir.length == 0) {
            var bName = !elName ? "item" : elName + ".item";
            ir = joList.find("[wbo-bind='" + bName + "']");//兼容旧版本
        }

        if (ir.length == 0) {
            if (joList[0].nodeName != "BUTTON" && joList[0].nodeName != "INPUT")
                ir = joList;
        }

        if (ir.length == 0)
            console.error("list item not define", joList);

        _listBody = ir.parent();

        if (joList.attr("fixed-list") == "true") {
            _this._fixed_list = true;
            _rowBody = $(ir[0]).clone()
            ir.each(function () {
                jItems.push($(this));
                bindItemEvent($(this));
            })
        }
        else {
            _rowBody = ir.clone();
            jItems.push(ir);
            bindItemEvent(ir);
        }

    }

    //设置行的鼠标事件
    function bindItemEvent(joItem) {
        joItem.click(function () {
            _this.setActiveItem(joItem)
        });
        joItem.focus(function () {
            _this.setActiveItem(joItem)
        });
        joItem.find("*").focus(function () {
            _this.setActiveItem(joItem)
        });

        joItem.mouseenter(function () {
            if (joItem != activeJoItem)
                joItem.addClass(css.mo).find("*").addClass(css.mo);
        });
        joItem.mouseout(function () {
            if (joItem != activeJoItem)
                joItem.removeClass(css.mo).find("*").removeClass(css.mo);
        });
        joItem.find("*").mouseenter(function () {
            if (joItem != activeJoItem)
                joItem.addClass(css.mo).find("*").addClass(css.mo);
        });
        joItem.find("*").mouseout(function () {
            if (joItem != activeJoItem)
                joItem.removeClass(css.mo).find("*").removeClass(css.mo);
        });

    }


    this.addItem = function () {
        var ir = _rowBody.clone();
        _listBody.append(ir);
        jItems.push(ir);
        bindItemEvent(ir);

        //        _this.activeItem(r);
        return new Form(ir);
    }

    this.insertBefore = function () {
        if (!activeJoItem)
            return this.addItem();

        var ir = _rowBody.clone();
        var i = jItems.indexOf(activeJoItem);
        jItems.insert(ir, i);
        activeJoItem.first().before(ir);
        bindItemEvent(ir);

        return new Form(ir);
    }

    this.insertAfter = function () {
        if (!activeJoItem)
            return this.addItem();
        var ir = _rowBody.clone();
        var i = jItems.indexOf(activeJoItem);
        jItems.insertAfter(ir, i);
        activeJoItem.last().after(ir);
        bindItemEvent(ir);
        return new Form(ir);
    }

    this.setActiveItem = function (joItem) {
        if (activeJoItem != null) {
            activeJoItem.removeClass(css.act).find("*").removeClass(css.act);
        }

        joItem.addClass(css.act).find("*").addClass(css.act);
        joItem.removeClass(css.mo).find("*").removeClass(css.mo);

        activeJoItem = joItem;
    }

    this.clearItem = function () {
        for (var i = jItems.length - 1; i > -1; i--) {
            jItems[i].remove();
        }
        jItems = [];
        activeJoItem = null;
    }

    this.getActiveItem = function () {
        return new Form(activeJoItem);
    }

    this.itemCount = function () {
        return jItems.length;
    }

    this.removeItem = function (form) {
        var item = form.getJObj();
        jItems.removeItem(item);
        item.remove();
        //_listBody.remove(item);
    }

    this.jItems = jItems;
    this.items = jItems;

    getHeadBody();
    getListBody();

}











