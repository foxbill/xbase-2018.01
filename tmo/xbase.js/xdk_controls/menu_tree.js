/*
import "../../xbase.js/system/array.js";
*/


MenuTree.prototype = new Tree;
function MenuTree(objName, menuId) {
    Tree.call(this, objName);
    var _this = this;

    this.config.useCookies = false;
    this.config.target = "mainFrame";
    this.icon.folder = '../../xbase.js/controls/tree/images/page.gif';
    this.icon.folderOpen = '../../xbase.js/controls/tree/images/page.gif';

    //    var nodeNo = 1;

    var topMenu = XdkRequester.SynRequest(XdkRequester.RequestType.Schema, XdkRequester.OperationName.GetSchema, menuId, XdkRequester.ObjType.Menu, null);
    var nodeData = new Object();
    nodeData.menu = topMenu;
    var topNode = this.newNode(null, topMenu.Description, nodeData);

    addMenuItems(topMenu, topNode);

    function addMenuItems(menu, parentNode) {
        var items = menu.MenuItem;
        for (var i = 0; i < items.length; i++) {
            var item = items[i];

            var nodeData = new Object();
            nodeData.menu = menu;
            nodeData.menuItem = item;
            var itemNode = _this.newNode(parentNode, item.MenuText, nodeData);

            if (item.NextLevel && item.NextLevel != undefined && item.NextLevel != "") {
                var subMenu = XdkRequester.SynRequest(XdkRequester.RequestType.Schema, XdkRequester.OperationName.GetSchema, item.NextLevel, XdkRequester.ObjType.Menu, null);
                if (subMenu) {
                    addMenuItems(subMenu, itemNode);
                }
            }
        }
    }


}

MenuTree.prototype.getNodeIndex = function(node) {
    for (var i = 0; i < this.aNodes.length; i++) {
        if (node == this.aNodes[i])
            return i;
    }
    return -1;
}

MenuTree.prototype.getNodeElement = function(node) {
    var nodeIndex = this.getNodeIndex(node);
    if (nodeIndex < 0) return null;
    return document.getElementById("s" + this.obj + nodeIndex);
}

MenuTree.prototype.updateNode = function(node) {
    var ret = XdkRequester.SynRequest(XdkRequester.RequestType.Schema, XdkRequester.OperationName.UpdateSchema, node.data.menu.Id, XdkRequester.ObjType.Menu, node.data.menu);
    var nodeElement = this.getNodeElement(node);

    if (node.data.menuItem) {
        nodeElement.innerText = node.data.menuItem.MenuText;
    } else if (node.data.menu) {
        nodeElement.innerText = node.data.menu.Description;
    }

}

MenuTree.prototype.deleteMenuNode = function(menuNode) {

    var menu = menuNode.data.menu;
    var menuItem = menuNode.data.menuItem;
    var subMenuId = menuItem.NextLevel;

    if (!menuItem)
        return false;

    if (subMenuId)
        var ret = XdkRequester.SynRequest(XdkRequester.RequestType.Schema, XdkRequester.OperationName.DeleteSchema, subMenuId, XdkRequester.ObjType.Menu);

    menu.MenuItem.removeItem(menuItem);
    var ret = XdkRequester.SynRequest(XdkRequester.RequestType.Schema, XdkRequester.OperationName.UpdateSchema, menu.Id, XdkRequester.ObjType.Menu, menu);

    this.deleteNode(menuNode);
    return true;
}

MenuTree.prototype.addChildMenu = function(menuNode, menuText, menuId) {
    var srcMenu = menuNode.data.menu;
    var upMenu = menuNode.data.menu;
    var upMenuItem = menuNode.data.menuItem;
    var isNewMenu = false;
    //    var subMenu = menu;

    if (upMenuItem) {
        //    debugger;
        var upMenuId = upMenuItem.NextLevel;

        if (upMenuId)
            upMenu = XdkRequester.SynRequest(XdkRequester.RequestType.Schema, XdkRequester.OperationName.GetSchema, upMenuId, XdkRequester.ObjType.Menu, null);
        else {
            upMenu = null;
            upMenuId = upMenuItem.Id;
        }

        if (!upMenu) {
            isNewMenu = true;
            upMenu = new Object();
            upMenu.Id = upMenuId;
            if (!upMenu.Id) {
                alert("当前菜单的Id没有指定，不能创建子菜单");
                return false;
            }
            upMenuItem.NextLevel = upMenu.Id;
            upMenu.MenuItem = [];
        }
    }

    var menuItem = new Object();
    menuItem.Id = menuId;
    menuItem.MenuText = menuText;
    upMenu.MenuItem.push(menuItem);

    if (isNewMenu) {
        var ret = XdkRequester.SynRequest(XdkRequester.RequestType.Schema, XdkRequester.OperationName.UpdateSchema, srcMenu.Id, XdkRequester.ObjType.Menu, srcMenu);
        var ret = XdkRequester.SynRequest(XdkRequester.RequestType.Schema, XdkRequester.OperationName.AddSchema, upMenu.Id, XdkRequester.ObjType.Menu, upMenu);
    }
    else {
        var ret = XdkRequester.SynRequest(XdkRequester.RequestType.Schema, XdkRequester.OperationName.UpdateSchema, upMenu.Id, XdkRequester.ObjType.Menu, upMenu);
    }

    var nodeData = new Object();
    nodeData.menu = upMenu;
    nodeData.menuItem = menuItem;
    var itemNode = this.newNode(menuNode, menuText, nodeData);
}


//获取菜单IDs
//var ret = XdkRequester.SynRequest(XdkRequester.RequestType.Schema, XdkRequester.OperationName.GetIDs, node.data.menu.Id, XdkRequester.ObjType.Menu);
