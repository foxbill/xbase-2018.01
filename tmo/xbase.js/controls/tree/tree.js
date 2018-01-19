/*--------------------------------------------------|
|  Tree 2.05 | www.destroydrop.com/javascript/tree/ |
|---------------------------------------------------|
| Copyright (c) 2002-2003 Geir Landr?               |
|                                                   |
| This script can be used freely as long as all     |
| copyright messages are intact.                    |
|                                                   |
| Updated: 17.04.2003                               |


| ******  Sample  *******                           |
|---------------------------------------------------|

    var tree=new Tree('tree');
    var topNode=tree.newNode(null,"Root",null);
    var childNode=tree.newNode(topNode,"child",null);    


|--------------------------------------------------*/

// Node object
function Node(id, pid, name, url, title, target, icon, iconOpen, open) {
    this.id = id;
    this.pid = pid;
    this.name = name;
    this.url = url;
    this.title = title;
    this.target = target;
    this.data = null;
    this.icon = icon;
    this.iconOpen = iconOpen;
    this._io = open || false;
    this._is = false;
    this._ls = false;
    this._hc = false;
    this._ai = 0;
    this._p;
};

// Tree object
function Tree(objName) {
    this.config = {
        target: null,
        folderLinks: false,
        useSelection: true,
        useCookies: false,
        useLines: true,
        useIcons: true,
        useStatusText: false,
        closeSameLevel: false,
        autoLink: false,
        inOrder: false
    }
    this.icon = {
        root: '../../xbase.js/controls/tree/images/base1.gif',
        folder: '../../xbase.js/controls/tree/images/folder.gif',
        folderOpen: '../../xbase.js/controls/tree/images/folderopen.gif',
        node: '../../xbase.js/controls/tree/images/page.gif',
        empty: '../../xbase.js/controls/tree/images/empty.gif',
        line: '../../xbase.js/controls/tree/images/line.gif',
        join: '../../xbase.js/controls/tree/images/join.gif',
        joinBottom: '../../xbase.js/controls/tree/images/joinbottom.gif',
        plus: '../../xbase.js/controls/tree/images/plus.gif',
        plusBottom: '../../xbase.js/controls/tree/images/plusbottom.gif',
        minus: '../../xbase.js/controls/tree/images/minus.gif',
        minusBottom: '../../xbase.js/controls/tree/images/minusbottom.gif',
        nlPlus: '../../xbase.js/controls/tree/images/nolines_plus.gif',
        nlMinus: '../../xbase.js/controls/tree/images/nolines_minus.gif'
    };
    this.obj = objName;
    this.aNodes = [];
    this.aIndent = [];
    this.root = new Node(-1);
    this.selectedNode = null;
    this.selectedFound = false;
    this.completed = false;


    var newNodeId = 1;

    this.newNode = function(parentNode, nodeText, nodeData) {
        var ret = null;
        if (!parentNode || !parentNode.id)
            ret = this.add(newNodeId, this.root.id, nodeText);
        else
            ret = this.add(newNodeId, parentNode.id, nodeText);

        newNodeId++;
        ret.data = nodeData;
        return ret;
    }
};

Tree.prototype.activeNode = null;

// Adds a new node to the node array
Tree.prototype.add = function(id, pid, name, url, title, target, icon, iconOpen, open) {
    //  if (!target) target = this.config.target;
    var newNode = new Node(id, pid, name, url, title, target, icon, iconOpen, open);
    this.aNodes[this.aNodes.length] = newNode;
    return newNode;
};

// Open/close all nodes
Tree.prototype.openAll = function() {
    this.oAll(true);
};

Tree.prototype.closeAll = function() {
    this.oAll(false);
};

// Outputs the tree to the page
Tree.prototype.toString = function() {
    var str = '<div class="dtree">\n';
    if (document.getElementById) {
        if (this.config.useCookies)
            this.selectedNode = this.getSelected();
        str += this.addNode(this.root);
    } else {
        str += 'Browser not supported.';
    }
    str += '</div>';
    //  if (!this.selectedFound) this.selectedNode = null;
    this.completed = true;
    return str;
};

// Creates the tree structure
Tree.prototype.addNode = function(pNode) {
    var str = '';
    var n = 0;

    if (this.config.inOrder)
        n = pNode._ai;

    for (n; n < this.aNodes.length; n++) {
        if (this.aNodes[n].pid == pNode.id) {
            var cn = this.aNodes[n];
            cn._p = pNode;
            cn._ai = n;
            this.setCS(cn);
            if (!cn.target && this.config.target) cn.target = this.config.target;
            if (cn._hc && !cn._io && this.config.useCookies) cn._io = this.isOpen(cn.id);
            if (!this.config.folderLinks && cn._hc) cn.url = null;
            if (this.config.useSelection && cn.id == this.selectedNode && !this.selectedFound) {
                cn._is = true;
                this.selectedNode = n;
                this.selectedFound = true;
            }
            str += this.node(cn, n);
            if (cn._ls) break;
        }
    }
    return str;
};


Tree.prototype.NodeClick = function(node) {
    alert(node.url);
    alert(node.name);
}

//内部NodeClick句柄
Tree.prototype._NodeClick = function(id) {
    var node = this.aNodes[id];
    this.activeNode = node;
    if (this.NodeClick) this.NodeClick(node);
}

//内部FolderClick句柄
Tree.prototype._FolderClick = function(id) {
    var node = this.aNodes[id];
    if (this.folderClick) this.folderClick(node);
}


// Creates the node icon, url and text
Tree.prototype.node = function(node, nodeId) {

    var str = '<div class="dTreeNode">' + this.indent(node, nodeId);
    if (this.config.useIcons) {
        if (!node.icon) node.icon = (this.root.id == node.pid) ? this.icon.root : ((node._hc) ? this.icon.folder : this.icon.node);
        if (!node.iconOpen) node.iconOpen = (node._hc) ? this.icon.folderOpen : this.icon.node;
        if (this.root.id == node.pid) {
            node.icon = this.icon.root;
            node.iconOpen = this.icon.root;
        }
        str += '<img id="i' + this.obj + nodeId + '" src="' + ((node._io) ? node.iconOpen : node.icon) + '" alt="" />';

    }


    var url = node.url;
    if (!url) url = "";

    //    str += '<a id="s' + this.obj + nodeId + '" class="' + ((this.config.useSelection) ? ((node._is ? 'nodeSel' : 'node')) : 'node') + '"';
    var activeId = this.activeNode ? this.activeNode.id : -2;
    str += '<a id="s' + this.obj + nodeId + '"  style="cursor:pointer"  class="' + ((this.config.useSelection) ? ((node.id == activeId ? 'nodeSel' : 'node')) : 'node') + '"';

    if (node.title)
        str += ' title="' + node.title + '"';

    if (url != "")
        str += ' href="' + url + '"';

    if (node.target)
        str += ' target="' + node.target + '"';


    if (this.config.useStatusText) str += ' onmouseover="window.status=\'' + node.name + '\';return true;" onmouseout="window.status=\'\';return true;" ';

    str += ' onclick="javascript: ' + this.obj + '.s(' + nodeId + ');"';

    str += '>';

    //捆绑FolderClick事件
    //    else if ((!this.config.folderLinks || !node.url) && node._hc && node.pid != this.root.id) {
    //   str += '<a  href="javascript: ' + this.obj + '.o(' + nodeId + ');" class="node">';
    //        str += '<a href="javascript:' + this.obj + '.NodeClick(' + nodeId + ');" class="node">';

    //   }
    str += node.name;
    //if (node.url || ((!this.config.folderLinks || !node.url) && node._hc))
    str += '</a>';

    str += '</div>';

    if (node._hc) {
        str += '<div id="d' + this.obj + nodeId + '" class="clip" style="display:' + ((this.root.id == node.pid || node._io) ? 'block' : 'none') + ';">';
        str += this.addNode(node);
        str += '</div>';
    }
    this.aIndent.pop();
    return str;
};

// Adds the empty and line icons
Tree.prototype.indent = function(node, nodeId) {
    var str = '';
    if (this.root.id != node.pid) {
        for (var n = 0; n < this.aIndent.length; n++)
            str += '<img src="' + ((this.aIndent[n] == 1 && this.config.useLines) ? this.icon.line : this.icon.empty) + '" alt="" />';
        (node._ls) ? this.aIndent.push(0) : this.aIndent.push(1);
        if (node._hc) {
            str += '<a href="javascript:' + this.obj + '.o(' + nodeId + ');"><img id="j' + this.obj + nodeId + '" src="';
            if (!this.config.useLines) str += (node._io) ? this.icon.nlMinus : this.icon.nlPlus;
            else str += ((node._io) ? ((node._ls && this.config.useLines) ? this.icon.minusBottom : this.icon.minus) : ((node._ls && this.config.useLines) ? this.icon.plusBottom : this.icon.plus));
            str += '" alt="" /></a>';
        } else str += '<img src="' + ((this.config.useLines) ? ((node._ls) ? this.icon.joinBottom : this.icon.join) : this.icon.empty) + '" alt="" />';
    }
    return str;
};

// Checks if a node has any children and if it is the last sibling
Tree.prototype.setCS = function(node) {
    var lastId;
    node._ls = false;
    for (var n = 0; n < this.aNodes.length; n++) {
        if (this.aNodes[n].pid == node.id) node._hc = true;
        if (this.aNodes[n].pid == node.pid) lastId = this.aNodes[n].id;
    }
    if (lastId == node.id) node._ls = true;
};

// Returns the selected node
Tree.prototype.getSelected = function() {
    var sn = this.getCookie('cs' + this.obj);
    return (sn) ? sn : null;
};

// Highlights the selected node
Tree.prototype.s = function(id) {
    //   if (!this.config.useSelection) return;
    var cn = this.aNodes[id];
    //   if (cn._hc && !this.config.folderLinks) return;

    if (id != this.selectedNode) {
        if (this.selectedNode || this.selectedNode == 0) {
            eOld = document.getElementById("s" + this.obj + this.selectedNode);
            if (eOld)
                eOld.className = "node";
        }
        eNew = document.getElementById("s" + this.obj + id);
        eNew.className = "nodeSel";

        this.selectedNode = id;

        if (this.config.useCookies) this.setCookie('cs' + this.obj, cn.id);
    }

    this.activeNode = cn;


    if (this.NodeClick) {
        this.NodeClick(cn);
    }

};

// Toggle Open or close
Tree.prototype.o = function(id) {
    var cn = this.aNodes[id];
    this.nodeStatus(!cn._io, id, cn._ls);
    cn._io = !cn._io;
    if (this.config.closeSameLevel) this.closeLevel(cn);
    if (this.config.useCookies) this.updateCookie();
};

// Open or close all nodes
Tree.prototype.oAll = function(status) {
    for (var n = 0; n < this.aNodes.length; n++) {
        if (this.aNodes[n]._hc && this.aNodes[n].pid != this.root.id) {
            this.nodeStatus(status, n, this.aNodes[n]._ls)
            this.aNodes[n]._io = status;
        }
    }
    if (this.config.useCookies) this.updateCookie();
};

// Opens the tree to a specific node
Tree.prototype.openTo = function(nId, bSelect, bFirst) {
    if (!bFirst) {
        for (var n = 0; n < this.aNodes.length; n++) {
            if (this.aNodes[n].id == nId) {
                nId = n;
                break;
            }
        }
    }
    var cn = this.aNodes[nId];
    if (cn.pid == this.root.id || !cn._p) return;
    cn._io = true;
    cn._is = bSelect;
    if (this.completed && cn._hc) this.nodeStatus(true, cn._ai, cn._ls);
    if (this.completed && bSelect) this.s(cn._ai);
    else if (bSelect) this._sn = cn._ai;
    this.openTo(cn._p._ai, false, true);
};

// Closes all nodes on the same level as certain node
Tree.prototype.closeLevel = function(node) {
    for (var n = 0; n < this.aNodes.length; n++) {
        if (this.aNodes[n].pid == node.pid && this.aNodes[n].id != node.id && this.aNodes[n]._hc) {
            this.nodeStatus(false, n, this.aNodes[n]._ls);
            this.aNodes[n]._io = false;
            this.closeAllChildren(this.aNodes[n]);
        }
    }
}

// Closes all children of a node
Tree.prototype.closeAllChildren = function(node) {
    for (var n = 0; n < this.aNodes.length; n++) {
        if (this.aNodes[n].pid == node.id && this.aNodes[n]._hc) {
            if (this.aNodes[n]._io) this.nodeStatus(false, n, this.aNodes[n]._ls);
            this.aNodes[n]._io = false;
            this.closeAllChildren(this.aNodes[n]);
        }
    }
}

// Change the status of a node(open or closed)
Tree.prototype.nodeStatus = function(status, id, bottom) {
    eDiv = document.getElementById('d' + this.obj + id);
    eJoin = document.getElementById('j' + this.obj + id);
    if (this.config.useIcons) {
        eIcon = document.getElementById('i' + this.obj + id);
        eIcon.src = (status) ? this.aNodes[id].iconOpen : this.aNodes[id].icon;
    }
    eJoin.src = (this.config.useLines) ?
	((status) ? ((bottom) ? this.icon.minusBottom : this.icon.minus) : ((bottom) ? this.icon.plusBottom : this.icon.plus)) :
	((status) ? this.icon.nlMinus : this.icon.nlPlus);
    eDiv.style.display = (status) ? 'block' : 'none';
};


// [Cookie] Clears a cookie
Tree.prototype.clearCookie = function() {
    var now = new Date();
    var yesterday = new Date(now.getTime() - 1000 * 60 * 60 * 24);
    this.setCookie('co' + this.obj, 'cookieValue', yesterday);
    this.setCookie('cs' + this.obj, 'cookieValue', yesterday);
};

// [Cookie] Sets value in a cookie
Tree.prototype.setCookie = function(cookieName, cookieValue, expires, path, domain, secure) {
    document.cookie =
		escape(cookieName) + '=' + escape(cookieValue)
		+ (expires ? '; expires=' + expires.toGMTString() : '')
		+ (path ? '; path=' + path : '')
		+ (domain ? '; domain=' + domain : '')
		+ (secure ? '; secure' : '');
};

// [Cookie] Gets a value from a cookie
Tree.prototype.getCookie = function(cookieName) {
    var cookieValue = '';
    var posName = document.cookie.indexOf(escape(cookieName) + '=');
    if (posName != -1) {
        var posValue = posName + (escape(cookieName) + '=').length;
        var endPos = document.cookie.indexOf(';', posValue);
        if (endPos != -1) cookieValue = unescape(document.cookie.substring(posValue, endPos));
        else cookieValue = unescape(document.cookie.substring(posValue));
    }
    return (cookieValue);
};

// [Cookie] Returns ids of open nodes as a string
Tree.prototype.updateCookie = function() {
    var str = '';
    for (var n = 0; n < this.aNodes.length; n++) {
        if (this.aNodes[n]._io && this.aNodes[n].pid != this.root.id) {
            if (str) str += '.';
            str += this.aNodes[n].id;
        }
    }
    this.setCookie('co' + this.obj, str);
};

// [Cookie] Checks if a node id is in a cookie
Tree.prototype.isOpen = function(id) {
    var aOpen = this.getCookie('co' + this.obj).split('.');
    for (var n = 0; n < aOpen.length; n++)
        if (aOpen[n] == id) return true;
    return false;
};


Tree.prototype.deleteNode = function(node) {

    for (var i = this.aNodes.Length - 1; i >= 0; i++) {
        var aNode = this.aNodes[i];
        if (aNode.pid == node.id) {
            this.deleteNode(aNode);
        }
    }

    this.aNodes.removeItem(node);
};

Tree.prototype.getNodeIndex = function(node) {
    for (var i = 0; i < this.aNodes.length; i++) {
        if (node == this.aNodes[i])
            return i;
    }
    return -1;
}

Tree.prototype.getNodeElement = function(node) {
    var nodeIndex = this.getNodeIndex(node);
    if (nodeIndex < 0) return null;
    return document.getElementById("s" + this.obj + nodeIndex);
}


// If Push and pop is not implemented by the browser
if (!Array.prototype.push) {
    Array.prototype.push = function array_push() {
        for (var i = 0; i < arguments.length; i++)
            this[this.length] = arguments[i];
        return this.length;
    }
};

if (!Array.prototype.pop) {
    Array.prototype.pop = function array_pop() {
        lastElement = this[this.length - 1];
        this.length = Math.max(this.length - 1, 0);
        return lastElement;
    }
};