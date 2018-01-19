function Form(el) {
    var joForm = el;
    if (TypeUtils.isElement(el) || TypeUtils.isString(el)) {
        var joForm = $(el);
    }

    this.setValue = function (name, value) {
        var cell = this.getField(name);
        return cell.setText(value);
    }

    this.getField = function (name) {
        return joForm.find("[name='" + name + "']");
    }

    this.getValue = function (name) {
        var cell = this.getField(name);
        return cell.getText();
    }
    this.getJObj = function () {
        return joForm;
    }
}

function ListView(el, css) {
    var _this = this;
    var _headBody = null;
    var _listBody = null;
    var _rowBody = null;
    var activeJoItem = null;
    var jItems = new Array();
    var joList = el;
    if (TypeUtils.isElement(el) || TypeUtils.isString(el))
        joList = $(el);
    var elName = joList.attr("name");

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
        _headBody = joList.find("[" + C.list.attr + "='" + C.list.head + "']");
        _headBody = _headBody.filter("[" + C.obj.attr + "='" + elName + "']");
    }

    //获取列表体
    function getListBody() {
        //        var rCells = $(el).find("." + css.ic);
        //        if (rCells.length < 1)
        //            alert("不能发现数据行单元格，列表不能正常工作，请用class='ic'标记列表行");
        //        var r = rCells.parent();
        var ir = joList.find("[" + C.list.attr + "='" + C.list.item + "']");
        ir = ir.filter("[" + C.obj.attr + "='" + elName + "']");

        _rowBody = ir.clone();
        _listBody = ir.parent();
        jItems.push(ir);
        bindItemEvent(ir);
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

    this.getJObj = function () {
        return joList;
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
        for (var i = 0; i < jItems.length; i++) {
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


    getHeadBody();
    getListBody();

}
