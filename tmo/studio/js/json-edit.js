function JsonForm(element, json, fieldMap) {
       
}


function JsonList(listTable, json, fieldMap) {
    var _this = this;
    ListView.call(this, listTable);

    if (!isArray(json)) {
        alert("JsonList 只接受数组对象的json 参数");
    }

    var template = json[0];
    if (fieldMap) {
        for (var f in fieldMap) {
            var d = fieldMap[f];
            var e = _this.getItemElement(_this.listItemElement, f);

            if (e) {
                var t = typeof (template[f]);

                if (d.isHide && (d.isHide == true || d.isHide == "true")) {
                    var he = _this.getItemElement(_this.listHead, f);
                    _this.listHead.removeChild(he);
                    _this.listItemElement.removeChild(e);
                }
                else if (t == "object") {
                    appendButton(e);
                }
                else if (d.inputElement) {
                    var ne = document.createElement(d.inputElement); //IE10以下有效
                    if (ne.tagName && ne.tagName.toLowerCase() == "select") {
                        for (var i = 0; i < d.selectDatas.length; i++) {
                            var sd = d.selectDatas[i];
                            ne.options.add(new Option(sd.text, sd.value));
                        }
                    }
                    clearElement(e);
                    e.appendChild(ne);
                }
            }
            if (d.title) {
                var cell = _this.getItemElement(_this.listHead, f);
                if (cell) {
                    DomUtils.setElementValue(cell, d.title, false);
                }
            }
        }
    }
    for (var i = 0; i < json.length; i++) {
        var o = json[i];
        var item = _this.addItem();
        for (var f in o) {
            var v = o[f];
            var def;
            if (fieldMap) {
                def = fieldMap[f];
            }
            var e = this.getItemElement(item, f);

            if (e) {
                var t = typeof (v);
                if (t == "object") {
                    if (!fieldMap)
                        appendButton(e);
                    if (e.childNodes && e.childNodes.length > 0) {
                        var echild = e.childNodes[0];
                        echild.jsonData = v;
                        echild.onclick = function () {
                            _this.onCheckObj(echild);
                        }
                    }
                }
                else if (def && e.childNodes) {
                    for (var idx = 0; idx < e.childNodes.length; idx++) {
                        var ne = e.childNodes[idx];
                        DomUtils.setElementValue(ne, v, false);
                    }
                }
                else {
                    e.innerHTML = v;
                }
            }
        }
    }
    function appendButton(e) {
        var n = document.createElement("input");
        n.type = "button";
        n.value = "查看";
        clearElement(e);
        e.appendChild(n);
        return n;
    }
    function clearElement(e) {
        if (!e || !e.childNodes) return;
        for (var j = 0; j < e.childNodes.length; j++) {
            e.removeChild(e.childNodes[j]);
        }
    }
    this.onCheckObj = function (event) {
        if (event && event.jsonData) {
            return event.jsonData;
        }
    }

    this.setCellValue = function (colName, colValue, rowNo) {
        rowNo++;
        var cells = DomUtils.getElementsByName(_this.body, colName);
        if (cells.length < 1) {
            alert("没有发现列" + colName);
            return;
        }
        if (rowNo >= cells.length)
            return;
        var el = cells[rowNo];

        if (el.childNodes) {
            for (var i = 0; i < el.childNodes.length; i++) {
                DomUtils.setElementValue(el.childNodes[i], colValue, false);
            }
        }
        else
            DomUtils.setElementValue(el, colValue, false);
    }
    this.getCellValue = function (colName, rowNo) {
        var cells = DomUtils.getElementsByName(_this.body, colName);
        if (cells.length < 1) {
            alert("没有发现列" + colName);
            return;
        }
        if (rowNo >= cells.length)
            return;
        var el = cells[rowNo];
        if (el.childNodes && el.chidlNodes.length > 0) {
            return DomUtils.getElementValue(el.childNodes[0]);
        }
        else
            return DomUtils.getElementValue(el);
    }
}
