
function XdkTree(objName, xNode) {

    //begin 继承dTree(objName)-----//
    var _baseObj = new dTree(objName);
    for (var p in _baseObj) { XdkTree.prototype[p] = _baseObj[p]; }
    XdkTree.prototype.toString = _baseObj.toString;
    //end   继承dTree(objName)-----//

    var _xNode = xNode;
    var _this = this;

    this.config.useCookies = false;
    this.config.target = "mainFrame";

    var nodeNo = 1;
    function AddNode(node, parentNo) {
        var thisNodeNo = nodeNo;
        _this.add(nodeNo, parentNo, node.Name,  node.ID);
        //     document.writeln(nodeNo + "," + parentNo + "<br>");
        nodeNo++;
        var subNodeNo = nodeNo + 1;
        for (var i in node.Nodes) {
            var subNode = node.Nodes[i];
            AddNode(subNode, thisNodeNo);
            nodeNo++;
        }
    }

    this.SetData = function(xNode) {

        if (!xNode) return;

        _xNode = xNode;
        AddNode(xNode, -1);

    }

    this.SetData(xNode);
}



function WbdlTree(objName, wbdl) {

    //begin 继承dTree(objName)-----//
    var _baseObj = new dTree(objName);
    for (var p in _baseObj) { WbdlTree.prototype[p] = _baseObj[p]; }
    WbdlTree.prototype.toString = _baseObj.toString;
    //end   继承dTree(objName)-----//

    var _wbdl = wbdl;
    var _this = this;

    this.config.useCookies = false;
    this.config.target = "mainFrame";

    var nodeNo = 1;

    //添加活动节点（已经作废）
    function addActions(actions, parentNo) {
        var thisNodeNo = nodeNo;

        _this.add(nodeNo, parentNo, "页面活动", "", "Actions");
        nodeNo++;
        if (!actions) return;
        //        var subNodeNo = nodeNo + 1;
        for (var i = 0; i < actions.length; i++) {
            var action = actions[i];
            var newNode = _this.add(nodeNo, thisNodeNo, action.Id, action.Id, action.Title);
            newNode.WbdlType = "Actions";
            nodeNo++;
        }
    }

    //添加DataListBinds和FieldBinds节点
    function addSchemas(schemas, parentNo, schemaType, title) {
        var thisNodeNo = nodeNo;

        _this.add(nodeNo, parentNo, title, "", schemaType);
        nodeNo++;

        if (!schemas) return;

        for (var i = 0; i < schemas.length; i++) {
            var schema = schemas[i];
            var newNode = _this.add(nodeNo, thisNodeNo, schema.Id, schema.Id, schema.Title);
            var newNodeNo = nodeNo;
            nodeNo++;
            newNode.WbdlType = schemaType;
            if (schemaType == "DataListBinds") {
                addSchemas(schema.Columns, newNodeNo, "Columns", "列字段");
            }
        }
    }

    //引入wbdl数据，并加载所有的节点
    this.SetData = function(wbdl) {
        var thisNodeNo = nodeNo;
        if (!wbdl) return;
        _wbdl = wbdl;
        _this.add(nodeNo, -1, _wbdl.Id, _wbdl.Title, _wbdl.Description);

        nodeNo++;
        addSchemas(_wbdl.FieldBinds, thisNodeNo, "FieldsBinds", "数据字段");
        addSchemas(_wbdl.DataListBinds, thisNodeNo, "DataListBinds", "数据列表");
        addSchemas(_wbdl.EventBinds, thisNodeNo, "EventBinds", "事件对象");
        addSchemas(_wbdl.Actions, thisNodeNo, "Actions", "页面活动");
    }

    this.SetData(wbdl);
}

/**
* Wbdl 数据源树
*
*/
WbdlTableTree.prototype = new dTree;
function WbdlTableTree(objName, wbdl) {
    dTree.call(this, objName);
    var _wbdl = wbdl;
    var _this = this;

    this.config.useCookies = false;
    this.config.target = "mainFrame";

    var nodeNo = 1;

    function addSchemas(schemas, parentNo, schemaType, title) {
        var thisNodeNo = nodeNo;

        _this.add(nodeNo, parentNo, title, "", schemaType);
        nodeNo++;

        if (!schemas) return;

        for (var i = 0; i < schemas.length; i++) {
            var schema = schemas[i];
            var newNode = _this.add(nodeNo, thisNodeNo, schema.Id, schema.Id, schema.Title);
            var newNodeNo = nodeNo;
            nodeNo++;
            newNode.WbdlType = schemaType;
        }
    }

    this.SetData = function(wbdl) {


        if (!wbdl) return;
        _wbdl = wbdl;

        _this.add(nodeNo, -1, _wbdl.Id, _wbdl.Title, _wbdl.Description);
        var thisNodeNo = nodeNo;
        nodeNo++;
        if (_wbdl.DataSource && _wbdl.DataSource.Tables && _wbdl.DataSource.Tables.length)
            addSchemas(_wbdl.DataSource.Tables, thisNodeNo, "Tables", "数据表");
    }

    this.SetData(wbdl);
}



/**
*  DbExplore
*  数据库资源树
*/
function DbExplorer(objName, wbdl) {

    //begin 继承dTree(objName)-----//
    var _baseObj = new dTree(objName);
    for (var p in _baseObj) { WbdlTree.prototype[p] = _baseObj[p]; }
    WbdlTree.prototype.toString = _baseObj.toString;
    //end   继承dTree(objName)-----//

    var _wbdl = wbdl;
    var _this = this;

    this.config.useCookies = false;
    this.config.target = "mainFrame";

    var nodeNo = 1;

    function addSchemas(schemas, parentNo, schemaType, title) {
        var thisNodeNo = nodeNo;

        _this.add(nodeNo, parentNo, title, "", schemaType);
        nodeNo++;

        if (!schemas) return;

        for (var i = 0; i < schemas.length; i++) {
            var schema = schemas[i];
            var newNode = _this.add(nodeNo, thisNodeNo, schema.Id, schema.Id, schema.Title);
            var newNodeNo = nodeNo;
            nodeNo++;
            newNode.WbdlType = schemaType;
        }
    }

    this.SetData = function(wbdl) {
        var thisNodeNo = nodeNo;
        if (!wbdl) return;
        _wbdl = wbdl;
        _this.add(nodeNo, -1, _wbdl.Id, _wbdl.Title, _wbdl.Description);

        nodeNo++;
        addTables(_wbdl.DataSetSchema.Tables, thisNodeNo, "Tables", "数据表");
    }

    this.SetData(wbdl);
}


