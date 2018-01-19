function FilterDropdownList(elInput, listName, listDiv) {

    var _activeInput;
    var _this = this;

    if (!listDiv || listDiv == undefined) {
        listDiv = document.createElement("div");
        listDiv.style.position = "absolute";
        listDiv.style.display = "none";
        listDiv.style.overflow = "visible";
        document.body.appendChild(listDiv);
    }

    // debugger;
    var selLists = listDiv.getElementsByTagName("select");
    var selList = null;
    if (selLists.length > 0)
        selList = selLists[0];

    if (!selList) {
        selList = document.createElement("select");
        listDiv.appendChild(selList);
    }

    selList.size = 10;
    selList.options.length = 0;
    selList.style.height = "auto";

    selList.onclick = function() {
        setValue();
    }

    if (listName) {
        selList.name = listName;
        selList.id = listName;
    }


    for (var i = 0; i < 10; i++) {
        selList.options.add(new Option(i, i));
    }


    this.bindElement = function(el) {

        _activeInput = el;
        _activeInput.onkeydown = updown
    }

    function attach(el) {
        el.onfocus = function() {
            _this.bindElement(el);
        }
        el.onblur = function() {
            if (document.activeElement != selList) {
                // alert(selList.selectedIndex + "." + selList.value);
                setValue();
            }
        }
    }
    this.bindElements = function(inputs) {
        for (var i = 0; i < inputs.length; i++) {
            var el = inputs[i];
            attach(el);
        }
    }


    if (elInput)
        this.bindElement(elInput);


    function show() {
        //            if (event.propertyName != "value") return;
        _activeInput.selectedValue = false;
        listDiv.style.display = "";
        if (_activeInput != undefined && _activeInput != null) {
            var x = getElementLeft(_activeInput);
            var y = getElementTop(_activeInput) + _activeInput.offsetHeight;
            listDiv.style.left = x + "px";
            listDiv.style.top = y + "px";
        }

    }

    function getElementLeft(element) {
        var actualLeft = element.offsetLeft;
        var current = element.offsetParent;
        while (current !== null) {
            actualLeft += current.offsetLeft;
            current = current.offsetParent;
        }
        return actualLeft;
    }

    function getElementTop(element) {
        var actualTop = element.offsetTop;
        var current = element.offsetParent;
        while (current !== null) {
            actualTop += current.offsetTop;
            current = current.offsetParent;
        }
        
        return actualTop;
    }

    function updown(e) {

        var keyCode;
        var evt = e ? e : (window.event ? window.event : null);
        if (!evt) return;

        keyCode = evt.keyCode;

        if ((keyCode >= 65 && keyCode <= 90) || (keyCode >= 48 && keyCode <= 57) || (keyCode >= 96 && keyCode <= 105))
            show(this, listDiv);

        switch (keyCode) {
            case 38:
                {
                    if (selList.selectedIndex > -1)
                        selList.selectedIndex--;
                    break;
                }
            case 40:
                {
                    if (selList.selectedIndex < selList.options.length - 1)
                        selList.selectedIndex++;
                    break;
                }
                //     case 9:
            case 13:
                {
                    evt.keyCode = 9;
                    break;
                }
        }
        return true;
    }

    function setValue() {
        //if (_activeInput.value)
        if (!_activeInput.selectedValue) {
            _activeInput.value = selList.value;
            _activeInput.selectedValue = true;
        }
        listDiv.style.display = "none";
        selList.selectedIndex = -1;
    }

}
