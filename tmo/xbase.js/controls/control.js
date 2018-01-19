/*
所有元素控件的基类
*/



function Control() {

    var _EventHandls = new Object();

    this.events = new Object();

    this.notifyEvent = function(event, args) {

        if (!event) {
            alert("通知事件时没有事件名称。");
            return;
        }

        var handles = _EventHandls[event];

        if (!handles)
            return;
        // throw new Error("没有定义事件:" + event + ",调用attachEvent方法，定义事件");

        var n = handles.length;
        for (var i = 0; i < n; i++) {
            var fn = handles[i];
            fn(args);
        }
    }

    this.attachEvent = function(event, fn) {

        if (!event) {
            alert("附加事件时没有事件名称。");
            return;
        }

        var handles = _EventHandls[event];

        if (!handles) {
            handles = [];
            _EventHandls[event] = handles;
        }

        var n = handles.length;
        for (var i = 0; i < n; i++) {
            var handle = handles[i];
            if (handle == fn) return;
        }
        handles.push(fn);
    }

}

//Control.prototype.Events.onSelect = "onSelect";
//Sample
SubControl.prototype.constructor = SubControl;
SubControl.prototype = new Control();
function SubControl() {
    Control.call(this, arguments);
    //    Control.apply(this,arguments);
    this.events.onBreak = "onBreak";
    this.events.onSelect = "onSelect";
    this.aaaa = function() {
    }
}

var a = new SubControl();

//a.Events.onBreak.name