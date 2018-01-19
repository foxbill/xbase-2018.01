function bind(object, type, fn) {
    if (object.attachEvent) {//IE浏览器 
        object.attachEvent("on" + type, (function() {
            return function(event) {
                window.event.cancelBubble = true; //停止时间冒泡 
                object.attachEvent = [fn.apply(object)]; //----这里我要讲的是这里 
            }
        })(object), false);
    } else if (object.addEventListener) {//其他浏览器 
        object.addEventListener(type, function(event) {
            event.stopPropagation(); //停止时间冒泡 
            fn.apply(this)
        });
    }

} 

