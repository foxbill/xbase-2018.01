/*
����Ԫ�ؿؼ��Ļ���
*/



function Control() {

    var _EventHandls = new Object();

    this.events = new Object();

    this.notifyEvent = function(event, args) {

        if (!event) {
            alert("֪ͨ�¼�ʱû���¼����ơ�");
            return;
        }

        var handles = _EventHandls[event];

        if (!handles)
            return;
        // throw new Error("û�ж����¼�:" + event + ",����attachEvent�����������¼�");

        var n = handles.length;
        for (var i = 0; i < n; i++) {
            var fn = handles[i];
            fn(args);
        }
    }

    this.attachEvent = function(event, fn) {

        if (!event) {
            alert("�����¼�ʱû���¼����ơ�");
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