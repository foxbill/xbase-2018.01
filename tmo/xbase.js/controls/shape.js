Array.prototype.arr2Str = function() {
    var arr = this;
    var str = "";
    for (var i = 0; i < arr.length; i++) {
        str += arr[i] + ",";
    }
    if (str == "") { return str; }
    else { return str.substr(0, str.length - 1); }
}

Array.prototype.indexOf = function(item) {
    for (var i = 0; i < this.length; i++) {
        if (item == this[i])
            return i;
    }
    return -1;
}

Array.prototype.searchItem = function(searchHandle) {
    for (var i = 0; i < this.length; i++) {
        if (searchHandle(this[i]))
            return this[i];
    }
    return null;
}


Array.prototype.removeItem = function(item) {
    var index = this.indexOf(item);
    if (index < 0) return false;
    this.splice(index, 1);
    return true;
}

Array.prototype.hasText = function(text) {
    for (var i = 0; i < this.length; i++) {
        var item = this[i];
        if (text.toLowerCase && item.toLowerCase) {
            if (text.toLowerCase() == item.toLowerCase())
                return true;
        }
    }
    return false;
}

Array.prototype.remove = function(index) {
    this.splice(index, 1);
}

Array.prototype.clear = function() {
    for (var i = this.length - 1; i > -1; i--) {
        this.splice(i, 1);
    }
}


SHAPE_ENUM = {
    STROKE: { Block: "Block", Classic: "Classic", Diamond: "Diamond", Oval: "Oval", Open: "Open" }
}


function VShape(canvas) {

    var _shape = null;

    this.drawStart = function(canvas) {
        return "<v:shape></v:shape>";
    }

    this.drawEnd = function(canvas) {
        return "<v:shape></v:shape>";
    }

    this.draw = function(canvas) {
        var nodeStr = this.drawStart() + this.drawEnd();
        var _shape = canvas.appendChild(document.createElement(nodeStr));
        return _shape;
    }

}

ShapeCanvas.prototype.constructor = ShapeCanvas;
ShapeCanvas.prototype = new Control();
function ShapeCanvas(canvasElement) {
    Control.call(this);
    this.events.onSelectShape = "onSelectShape";

    this.canvas = null;
    this.document = null;

    this.initialize = function() {
        if (canvasElement && typeof (canvasElement) == "object" && canvasElement.nodeName) {
            this.canvas = canvasElement;

            this.document = canvasElement.ownerDocument;
        } else {
            if (canvasElement) alert("非法的画布控件");
            return;
        }
    }
    this.initialize();
    this.clearShape = function() {
        canvasElement.innerHTML = "";
    }
}

ShapeCanvas.prototype.appendRect = function(left, top, width, height) {

    if (!width) width = 50;
    if (!height) height = 30;

    var strElement = "<v:rect title='" + "box'"
    + "' style=position:absolute"
    + ";top:" + top
    + ";left:" + left
    + ";width:" + width
    + ";height:" + height
    + "'></v:rect>";

    var element = this.document.createElement(strElement);
    var _this = this;
    element.onclick = function() {
        _this.notifyEvent(_this.events.onSelectShape, element);
    }
    //    element.attributes("title").value = "aaa";
    element.title = "aa";
    this.canvas.appendChild(element);
    return element;
}


ShapeCanvas.prototype.appendPolyLine = function(points) {
    var strElement = "<v:rect title='" + "box'"
    + "  style='position:absolute"
    + ";top:" + top
    + ";left:" + left
    + ";width:" + width
    + ";height:" + height
    + "'></v:rect>";

    var element = this.document.createElement(strElement)
    var _this = this;
    element.onclick = function() {
        _this.notifyEvent(_this.events.onSelectShape, element);
    }
    //    element.attributes("title").value = "aaa";
    element.title = "aa";
    this.canvas.appendChild(element);
    return element;
}

ShapeCanvas.prototype.appendRoundRect = function(left, top, width, height) {

    var strElement = "<v:RoundRect title='" + "box'"
    + " style='position:absolute;"
    + ";top:" + top
    + ";left:" + left
    + ";width:" + width
    + ";height:" + height
    + "'></v:RoundRect>";

    var element = this.document.createElement(strElement);
    var _this = this;
    element.onclick = function() {
        _this.notifyEvent(_this.events.onSelectShape, element);
    }

    this.canvas.appendChild(element);
    return element;
}

ShapeCanvas.prototype.addLineStroke = function(lineElement, start, end) {

    var strElement = "<v:stroke "
    + "StartArrow='" + start + "'"
    + "EndArrow='" + end + "'"
    + "/>";

    var element = this.document.createElement(strElement);
    var _this = this;
    element.onclick = function() {
        _this.notifyEvent(_this.events.onSelectShape, element);
    }

    lineElement.appendChild(element);
    return lineElement;

}

ShapeCanvas.prototype.setBorderColor = function(element, color) {
    element.strokecolor = color;
}

ShapeCanvas.prototype.appendLine = function(x, y, x1, y1) {

    var strElement = "<v:Line title='" + "流程'"
    + " style='position:absolute'"
    + " from='" + x + "," + y + "'"
    + " to='" + x1 + "," + y1 + "'"
    + "></v:Line>";

    var element = this.document.createElement(strElement);
    var _this = this;
    element.onclick = function() {
        _this.notifyEvent(_this.events.onSelectShape, element);
    }
    this.canvas.appendChild(element);
    return element;
}



/**
* 流程画布-FlowCanvas
* 用于显示和管理对象属性
* @constructor
* @base Shape
*/

//继承ShapeCanvas
FlOW_ENUM = {
    FLOW_DIR: { RIGHT: 0, DOWN: 1 }
}

FLOW_CONST = {
    BORDER_COLOR: "Black",
    ACTIVE_BORDER_COLOR: "Blue"
}

//继承属性
FlowCanvas.prototype = new ShapeCanvas();
FlowCanvas.constructor = FlowCanvas;
FlowCanvas.prototype.superMethod = function(methodName, args) { return ShapeCanvas.prototype[methodName].call(this, args); };

//新属性
FlowCanvas.prototype.FLOW_START_LEFT = 100;
FlowCanvas.prototype.FLOW_START_TOP = 50;
FlowCanvas.prototype.PRO_WIDTH = 100;
FlowCanvas.prototype.PRO_HEIGHT = 40;
FlowCanvas.prototype.PRO_SPACE = 30;
FlowCanvas.prototype.FLOW_DIR = FlOW_ENUM.FLOW_DIR.DOWN;
FlowCanvas.prototype.actionList = [];
FlowCanvas.prototype.ProForm = null;
FlowCanvas.prototype.paramListForm = null;
FlowCanvas.prototype.currentPro = null;
FlowCanvas.prototype.action = null;


function FlowCanvas(canvasElement, actionFlow) {
    ShapeCanvas.call(this, canvasElement);

    this.events.OnSelectAction = "OnSelectAction";
    this.events.OnExitAction = "OnExitAction";
    this.activeActionNode = null;

    if (actionFlow)
        this.setActionFlow(actionFlow);

    this.cleaActionList = function() {
        while (this.actionList.length > 0) {
            var element = this.actionList[0];
            this.canvas.removeChild(element.element);
            this.actionList.splice(0, 1);
        }
        this.clearShape();
    }

    this.setActionFlow = function(newActionFlow) {
        actionFlow = newActionFlow;
        this.cleaActionList();
        //    this.action = action;

        //    var actCtrl = new ActionXdk(action);
        var _this = this;

        if (!actionFlow.Actions)
            actionFlow.Actions = [];

        for (var i = 0; i < newActionFlow.Actions.length; i++) {
            var action = newActionFlow.Actions[i];
            this.appendAction(action);
        }
        // actCtrl.forEachMethod(function(method) {
        //     _this.appendAction(method);
        // });

    }

    this.getActionFlow = function() {
        actionFlow.Actions = [];
        for (var i = 0; i < this.actionList.length; i++) {
            var actionNode = this.actionList[i];
            var action = actionNode.actionData;
            action.Id = i + "";
            actionFlow.Actions.push(action);
        }
        return actionFlow;
    }

    this.deleteAction = function() {
        if (!this.activeActionNode) return;
        var action = this.activeActionNode.actionData;
        if (!action) return;
        this.getActionFlow();
        actionFlow.Actions.removeItem(action);
        this.setActionFlow(actionFlow);
    }
}


function ActionNode(element, actionData) {

    this.PRO_SPACE = FlowCanvas.prototype.PRO_SPACE;
    this.element = element;
    this.actionData = actionData;
    this.next = null;
    this.proir = null;


    this.getLeft = function() {
        return parseInt(element.style.left);
    }

    this.setLeft = function(left) {
        element.style.left = left;
    }

    this.getTop = function() {
        return parseInt(element.style.top);
    }

    this.setTop = function(top) {
        element.style.top = top;
    }

    this.getWidth = function() {
        return parseInt(element.style.width);
    }

    this.setWidth = function(width) {
        element.style.width = width;
    }

    this.getHeight = function() {
        return parseInt(element.style.height);
    }

    this.setHeight = function(height) {
        element.style.height = height;
    }

    this.getNextProPos = function() {
        var point = new Object();

        if (FlowCanvas.prototype.FLOW_DIR == FlOW_ENUM.FLOW_DIR.DOWN) {
            point.x = this.getLeft();
            point.y = this.getTop() + +this.getHeight() + this.PRO_SPACE;
        } else {
            point.x = this.getLeft() + this.getWidth() + this.PRO_SPACE;
            point.y = this.getTop();
        }
        return point;
    }

    this.getNextProLineStart = function() {
        var point = new Object();

        if (FlowCanvas.prototype.FLOW_DIR == FlOW_ENUM.FLOW_DIR.DOWN) {
            point.x = this.getLeft() + Math.round(this.getWidth() / 2);
            point.y = this.getTop() + this.getHeight();
        } else {
            point.x = this.getLeft() + this.getWidth();
            point.y = this.getTop() + Math.round(this.getHeight() / 2);
        }
        return point;
    }

    this.getNextProLineEnd = function() {

        var point = this.getNextProPos();

        if (FlowCanvas.prototype.FLOW_DIR == FlOW_ENUM.FLOW_DIR.DOWN) {
            point.x += Math.round(this.getWidth() / 2);
        } else {
            point.y += Math.round(this.getHeight() / 2);
        }
        return point;
    }

    this.getPriorLinePoint = function() {
        var point = new Object();
        point.x = this.getLeft();
        point.y = this.getTop() + Math.round(this.getHeight() / 2);
        return point;
    }
    this.refresh = function() {
        var texts = element.getElementsByTagName("TextBox");
        if (texts.length > 0) {
            var text = texts[0];
            text.innerText = actionData.Title;
        }
    }
}

FlowCanvas.prototype.doPropertyChange = function(propertyName, propertyValue) {

    if (!this.activeActionNode) return;

    var method = this.activeActionNode.actionObj;

    var name = propertyName.replace("method", "");

    method[name] = propertyValue;
}

FlowCanvas.prototype.setProForm = function(dataForm) {

    this.ProForm = dataForm;
    var _this = this;

    dataForm.onDataChange = function(fieldElement) {

        var name = fieldElement.id;

        var value = dataForm.getDataValue(name);

        _this.doPropertyChange(name, value);

    }

};

FlowCanvas.prototype.doParamChange = function(paramName, paramValue) {
    if (!this.activeActionNode) return;
    var method = this.activeActionNode.actionObj;
    var methodXdk = new MethodXdk(method);
    methodXdk.setParameterValue(paramName, paramValue);
}

FlowCanvas.prototype.setParamListForm = function(paramListForm) {
    this.paramListForm = paramListForm;
    var _this = this;

    paramListForm.onPropertyChange = function(propertyName, propertyValue) {
        _this.doParamChange(propertyName, propertyValue);
    }

};


FlowCanvas.prototype.doActionSelected = function(actionNode) {

    if (actionNode == this.activeActionNode)
        return;

    if (this.activeActionNode) {
        var curElement = this.activeActionNode.element;
        this.setBorderColor(curElement, FLOW_CONST.BORDER_COLOR);
        if (actionNode != this.activeActionNode)
            this.notifyEvent(this.events.OnExitAction, this.activeActionNode);
        //        var pro = this.activeActionNode.actionObj;
    }

    this.activeActionNode = actionNode;

    var element = actionNode.element;
    this.setBorderColor(element, FLOW_CONST.ACTIVE_BORDER_COLOR);


    //   var pro = actionNode.actionObj;
    this.notifyEvent(this.events.OnSelectAction, actionNode);
}



FlowCanvas.prototype.appendAction = function(actionObj) {

    var left = this.FLOW_START_LEFT;
    var top = this.FLOW_START_TOP;

    var lastAction = this.actionList.pop();

    if (lastAction) {
        var point = lastAction.getNextProPos();
        left = point.x;
        top = point.y;

        var lineStart = lastAction.getNextProLineStart();
        var lineEnd = lastAction.getNextProLineEnd();
        var flowLine = this.appendLine(lineStart.x, lineStart.y, lineEnd.x, lineEnd.y);
        this.addLineStroke(flowLine, SHAPE_ENUM.STROKE.Oval, SHAPE_ENUM.STROKE.Classic);

        this.actionList.push(lastAction);
    }

    var element = this.appendRoundRect(left, top, this.PRO_WIDTH, this.PRO_HEIGHT);
    var shadow = this.document.createElement('<v:shadow on="T" type="single" color="#b3b3b3" offset="5px,5px"/>');
    var text = this.document.createElement('<v:TextBox inset="2pt,2pt,2pt,2pt" style="font-size:10.2pt; text-align:center">Hello world!</v:TextBox>');

    var title = actionObj.Title;

    if (!title && actionObj.MethodName)
        title = actionObj.MethodName;

    text.innerText = title;
    element.title = title;
    element.appendChild(shadow);
    element.appendChild(text);
    element.strokeweight = "2px";

    var actionNode = new ActionNode(element, actionObj);
    this.actionList.push(actionNode);


    var _this = this;

    element.onclick = function(event) {
        _this.doActionSelected(actionNode);
    }
    this.doActionSelected(actionNode);
}
