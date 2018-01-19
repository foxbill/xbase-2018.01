/*
import "../../xbase.js/system/array.js";
import "../../xbase.js/controls/tree/tree.js";
import "../../xbase.js/xdk/xdk4js.js"
import "../../xbase.js/utils/net_util.js"
*/

DataTree.constructor = Tree;
DataTree.prototype = new Tree();
DataTree.prototype.superMethod = function(methodName, args) { return Tree.prototype[methodName].call(this, args); };

DataTree = new Tree;

function DataTree(objName, menuId) {
    Tree.call(this, objName);
    var _this = this;

    this.config.useCookies = false;
    this.config.target = "mainFrame";

    buildTree();

    function buildTree() {

        var dbs = XDataDkRequester.getDatabaseSchema();

        var index = 1;

        //   this.add(index, -1, "数据库", "", "数据库");
        var root = _this.newNode(null, "数据库", "root");

        var tableNode = _this.newNode(root, "数据表", "table");

        var tables = dbs.Tables;

        var checkBoxStr = "<input type='checkbox' name='" + table.Name + "'/>";

        for (var i = 0; i < tables.length; i++) {
            var table = tables[i];
            _this.newNode(tableNode, checkBoxStr + table.Title, table.Name);
        }

        var viewNode = _this.newNode(root, "视图", "View");
        var views = dbs.Views;

        for (var i = 0; i < views.length; i++) {
            var view = views[i];
            _this.newNode(viewNode, checkBoxStr + view.Title, view.Name);
        }

    }

}

DataTree.prototype.getCheckedNames = function() {
    var ret = [];
    var els = document.getElementsByTagName("input");
    for (var i = 0; i < els.length; i++) {
        var el = els[i];
        if (el.type == "checkbox" && el.checked)
            ret.push(el.name);
    }
    return ret;
}
