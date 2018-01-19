/*
import "../../xbase.js/system/array.js";
*/


FileTree.prototype = new Tree;
function FileTree(objName, menuId) {
    Tree.call(this, objName);
    var _this = this;

    this.config.useCookies = false;
    this.config.target = "mainFrame";


    var treeData = XdkRequester.SynRequest(XdkRequester.RequestType.IO, XdkRequester.OperationName.GetPathTree, "", null, "*.htm|*.html");


    addTreeData(treeData, null);

    function addTreeData(treeData, parentNode) {

        var nodeData = new Object;
        nodeData.Data = treeData.Data;
        nodeData.Id = treeData.ID;
        nodeData.Name = treeData.Name;

        var newNode = _this.newNode(parentNode, nodeData.Name, nodeData);

        var items = treeData.Nodes;

        for (var i = 0; i < items.length; i++) {
            var item = items[i];
            addTreeData(item, newNode);
        }
    }


}

