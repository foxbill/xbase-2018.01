function Marquee(scrollBoxElement, scrollListElement, speed) {
    this.speed = 10;
    var _this = this;

    if (speed > 0)
        this.speed = speed;

    var tab = typeof (scrollBoxElement) == "string" ?
                 document.getElementById(scrollBoxElement) : scrollBoxElement;
    var tab1 = typeof (scrollListElement) == "string" ?
                 document.getElementById(scrollListElement) : scrollListElement;

    //   var tab2 = document.getElementById("demo2");
    //     var tab2 = document.createElement(tab1.tagName);
    //    tab.appendChild(tab2);
    var tab2 = document.createElement(tab1.tagName);
    tab2.innerHTML = tab1.innerHTML;

    tab2.style.styleFlow = tab1.style.styleFlow;
    tab2.style.width = tab1.style.width;
    tab2.style.height = tab1.style.height;

    tab.appendChild(tab2);
    //     tab2.style.float = "left";

    function Marquee() {
        if (tab2.offsetWidth - tab.scrollLeft <= 0)
            tab.scrollLeft -= tab1.offsetWidth
        else {
            tab.scrollLeft++;
        }
    }
    var MyMar = setInterval(Marquee, this.speed);
    tab.onmouseover = function() { clearInterval(MyMar) };
    tab.onmouseout = function() {
        MyMar = setInterval(Marquee, _this.speed)
    };
}
