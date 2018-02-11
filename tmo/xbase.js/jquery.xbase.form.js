
function Control(el) {
    var _this = this;
    if (!el)
        return;
    var $el = el;
    if (TypeUtils.isElement(el) || TypeUtils.isString(el)) {
        $el = $(el);
    }

    this.$ = $el;
    $el.data("ctrl", _this);
    $el.each(function () {
        this.ctrl = _this;
    });
}

function Form(el) {
    Control.call(this, el);
    var $form = this.$;

    this.getField = function (name) {
        var $field = $form.find("[wbo-bind='" + name + "']");
        if ($field.length == 0) {
            $field = $form.find("[name='" + name + "']");
        }
        return $field;
    }

    /***old replace by field*****/
    this.getValue = function (name) {
        var $field = this.getField(name);
        return $field.getText();
    }
    /***old replace by field*****/
    this.setValue = function (name, value) {
        var $field = this.getField(name);
        return $field.setText(value);
    }

    this.field = function (name, value) {
        var $field = this.getField(name);
        return $field.valu(value);
    }
}

function ListView(el, css) {
    Control.call(this, el);

    var _this = this;
    var $list = this.$;

    var _headBody = null;
    var _listBody = null;
    var $itemPlate = null;

    var _itemSelector = "";
    var _headSelector = "";

    var $activeItem = null;

    var listName = $list.attr("name");

    this._fixed_list = false;

    if (!listName)
        listName = $list.attr("id");

    var C = {
        css: {
            mo: "mo",
            act: "act"
        },
        obj: {
            attr: "obj"
        }
    };

    if (!css)
        css = C.css;


    //获取列表头
    function getHeadBody() {

        _headSelector = !listName ? "[name='head']" : "[name='" + listName + ".head']";
        _headBody = $list.find(_headSelector);//兼容旧版本


        if (_headBody.length == 0) {
            _headSelector = !listName ? "[wbo-bind='head']" : "[wbo-bind='" + listName + ".head']";
            _headBody = $list.find(_headSelector);
        }
    }

    //获取列表体
    function getListBody() {
        _itemSelector = !listName ? "[name='item']" : "[name='" + listName + ".item']";
        ir = $list.find(_itemSelector);//兼容旧版本

        if (ir.length == 0) {
            _itemSelector = !listName ? "[wbo-bind='item']" : "[wbo-bind='" + listName + ".item']";
            ir = $list.find(_itemSelector);
        }

        if (ir.length == 0) {
            if ($list[0].nodeName != "BUTTON" && $list[0].nodeName != "INPUT") {
                ir = $list;
                _itemSelector = null;
            }
            else
                return;
        }

        if (ir.length == 0) {
            console.error("list item not define", $list);
            debugger;
        }

        _listBody = ir.parent();

        if ($list.attr("fixed-list") == "true") {
            _this._fixed_list = true;
            $itemPlate = $(ir[0]).clone()
            ir.each(function () {
                bindItemEvent($(this));
            })
        }
        else {
            $itemPlate = ir.clone();
            bindItemEvent(ir);
        }

    }

    //设置行的鼠标事件
    function bindItemEvent(joItem) {
        joItem.click(function () {
            _this.setActiveItem(joItem);
        });
        joItem.focus(function () {
            _this.setActiveItem(joItem);
        });
        //joItem.find("*").focus(function () {
        //    _this.setActiveItem(joItem)
        //});

        joItem.mouseenter(function () {
            if (joItem != $activeItem)
                joItem.addClass(css.mo);//.find("*").addClass(css.mo);
        });
        joItem.mouseout(function () {
            if (joItem != $activeItem)
                joItem.removeClass(css.mo);//.find("*").removeClass(css.mo);
        });
        joItem.find("*").mouseenter(function () {
            if (joItem != $activeItem)
                joItem.addClass(css.mo);//.find("*").addClass(css.mo);
        });
        joItem.find("*").mouseout(function () {
            if (joItem != $activeItem)
                joItem.removeClass(css.mo)//;.find("*").removeClass(css.mo);
        });

    }

    this.moveUp = function (el) {
        var $c = $(el);
        var $p = $c.prev();
        $c.insertBefore($p);

        //if (el.ctrl) {
        //    //c = el.ctrl.$;
        //    //var p = el.ctrl.$.prev();
        //    //c.insertBefore(p);
        //};
    }

    this.moveDown = function (el) {
        var $c = $(el)
        var $after = $c.next();
        $c.insertAfter($after);

        //if (el.ctrl) {
        //    //    c = el.ctrl.$;
        //    //    var after = el.ctrl.$.next();
        //    //    c.insertAfter(after);
        //};
    }

    this.addItem = function () {
        if ($itemPlate == null) {
            //List is button or input
            return;
        }
        var ir = $itemPlate.clone();
        _listBody.append(ir);
        bindItemEvent(ir);
        new Form(ir);
        return ir;
    }

    this.insertBefore = function () {
        if (!$activeItem)
            return this.addItem();
        var ir = $itemPlate.clone();
        $activeItem.first().before(ir);
        bindItemEvent(ir);
        new Form(ir);
        return ir;
    }

    this.insertAfter = function () {
        if (!$activeItem)
            return this.addItem();
        var ir = $itemPlate.clone();
        $activeItem.last().after(ir);
        bindItemEvent(ir);
        new Form(ir);
        return ir;
    }

    this.prevActive = function () {
        if ($activeItem) {
            var prev = $activeItem.prev();
            if (prev && prev.length > 0)
                return this.activeItem(prev);
            return null;
        }
        else
            return this.activeItem(0);
    }

    this.nextActive = function () {
        if ($activeItem) {
            var next = $activeItem.next();
            if (next && next.length > 0)
                return this.activeItem(next);
            return null;
        }
        else
            return this.activeItem(0);
    }

    /**
    *---------------------
    *获取或设置被选中的项目
    *---------------------
    *@PARAM item 需要被设置为选中的JQuery元素或序号
    *@RETURN 返回被选中的html元素
    *
    *例：
    *  $("#elXX").data("ctrl").activeItem(); //返回id='elXX'列表中当前的选中对象
    *  $("#elXX").data("ctrl").activeItem(3); //将id='elXX'列表中的第4个元素设置为激活元素
    *  $("#elXX").data("ctrl").activeItem($("#a1")); //将id='elXX'列表中id='a1'的元素设置为激活元素
    **/
    this.activeItem = function (item) {
        if (item == undefined) {
            if (!$activeItem)
                return null;
            return $activeItem[0];
        }

        if (TypeUtils.isNumber(item))
            item = $(this.items()[item]);

        if ($activeItem != null) {
            $activeItem.removeClass(css.act);
        }

        item.addClass(css.act);//.find("*").addClass(css.act);
        item.removeClass(css.mo);//.find("*").removeClass(css.mo);

        $activeItem = item;
        return $activeItem[0];
    }

    //过期
    this.setActiveItem = function (joItem) {
        if (TypeUtils.isNumber(joItem))
            joItem = $(this.items()[0]);

        if ($activeItem != null) {
            $activeItem.removeClass(css.act);
        }

        joItem.addClass(css.act);
        joItem.removeClass(css.mo);

        $activeItem = joItem;
        return $activeItem;
    }

    //过期
    this.getActiveItem = function () {
        return new Form($activeItem);
    }


    this.removeItem = function (elItem) {
        $(elItem).remove();
    }

    this.items = function () {
        if (!_listBody)
            return $("");
        if (_itemSelector)
            return _listBody.find(_itemSelector);
        else
            return _listBody.children();
    }

    this.clearItem = function () {
        var items = this.items();
        if (items)
            items.remove();
    }

    this.itemCount = function () {
        return this.items().length;
    }

    getHeadBody();
    getListBody();

}











