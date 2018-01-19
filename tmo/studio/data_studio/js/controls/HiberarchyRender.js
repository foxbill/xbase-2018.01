/*
**************************************************************************
Name: xHiberarchy Render & xHiberarchy Render Provider Interface
Author: Jerry, dingcx@chinalmtc.com
Version: 1.0
Last updates: 2010-3-1, 2010-2-26
Classes:
xHiberarchy Class
xHiberarchy Item Class
xHiberarchy Item Collection Class
xHiberarchy Render Providers Interface Class

3rd part class(*: implement xHiberarchy Render Providers Interface):
MenuProvider1 Class
**************************************************************************
*/

//-----------------------------------------------------------
//---------------------xHiberarchy Render Class [START]------
//-----------------------------------------------------------
var xHiberarchy = function() {
    //Private Variable Defines:

    //Properites:
    this.Items = new xHiberarchyItemCollection();
    this.ContainerID = null;

    //Mthods:
    this.Add = function(Item) {
        Item.IndexOfRoot = this.Items.Count;
        Item.ProcChildItemsIndexOfRoot(Item.IndexOfRoot);
        this.Items.Add(Item);
    }

    this.Remove = function(ItemIndex) {
        var tAry = new Array();
        var tObj;
        var aLen = this.Items.Count;
        for (var i = 0; i < aLen; i++) {
            tObj = this.Items.shift();
            if (ItemIndex != i) {
                tAry.push(tObj);
            } else { break; }
        }
        this.Items = tAry.concat(this.Items);
    }

    this.Render = function() {
        var tItems = "";
        for (var i = 0; i < this.Items.Count; i++) {
            tItems += this.Items.GetItem(i).RenderHTML();
        }
        tItems = xHiberarchyRenderProviders[xHiberarchy.RenderProviderID].BuildItemCollection({ Depth: 0 }, tItems);
        document.getElementById(this.ContainerID).innerHTML = xHiberarchyRenderProviders[xHiberarchy.RenderProviderID].BuildHiberarchy(tItems);
    }

    this.DataBind = function(xDataSet) {
        if (xHiberarchyDataDriver[xHiberarchy.DataDriverID].GetChildCount(xDataSet) == 0) return;
        this.Items.DataBind(xDataSet);
        for (var i = 0; i < this.Items.Count; i++) {
            with (this.Items.GetItem(i)) {
                IndexOfRoot = i;
                ProcChildItemsIndexOfRoot(i);
                ProcChildItemsDepth(0);
            }
        }
    }
}
xHiberarchy.RenderProviderID = 0;
xHiberarchy.DataDriverID = 0;
//---------------------xHiberarchy Render Class [END]------

//-------------------------------------------------------------------
//---------------------xHiberarchy Item Collection Class [START]-----
//-------------------------------------------------------------------
var xHiberarchyItemCollection = function() {
    //Private Variable Defines:
    var _xHiberarchyItemArray = new Array();

    //Properites:
    this.Count = 0;

    //Mthods:
    this.Add = function(Item) {
        _xHiberarchyItemArray.push(Item);
        this.Count++;
    }

    this.Remove = function(ItemIndex) {
        var tAry = new Array();
        var tObj;
        var aLen = _xHiberarchyItemArray.length;
        for (var i = 0; i < aLen; i++) {
            tObj = _xHiberarchyItemArray.shift();
            if (ItemIndex != i) {
                tAry.push(tObj);
            } else { this.Count--; break; }
        }
        _xHiberarchyItemArray = tAry.concat(_xHiberarchyItemArray);
    }

    this.GetItem = function(ItemIndex) {
        return _xHiberarchyItemArray[ItemIndex];
    }

    this.DataBind = function(xItemArray) {
        var tLen = xHiberarchyDataDriver[xHiberarchy.DataDriverID].GetChildCount(xItemArray);
        var tItemObj;
        if (tLen == 0) return;
        _xHiberarchyItemArray = new Array();
        for (var i = 0; i < tLen; i++) {
            var o = new xHiberarchyItem();
            tItemObj = xHiberarchyDataDriver[xHiberarchy.DataDriverID].GetChild(xItemArray, i);
            with (xHiberarchyDataDriver[xHiberarchy.DataDriverID].GetChildInfo(tItemObj)) {
                o.Text = Text;
                o.NavigateUrl = NavigateUrl;
                o.Target = Target;
            }
            this.Add(o);
            this.GetItem(i).DataBind(tItemObj);
        }
    }
}
//---------------------xHiberarchy Item Collection Class [END]-----

//-----------------------------------------------------------
//---------------------xHiberarchy Item Class [START]--------
//-----------------------------------------------------------
var xHiberarchyItem = function() {
    //Private Variable Defines:
    //Properites:
    this.Text = null;
    this.NavigateUrl = null;
    this.ImageUrl = null;
    this.Target = null;
    this.ToolTip = null;
    this.ChildItems = new xHiberarchyItemCollection();
    this.Depth = 0;
    this.IndexOfRoot = 0;

    //Methods:
    this.RenderHTML = function() {
        var subItems = "";
        for (var i = 0; i < this.ChildItems.Count; i++) {
            subItems += this.ChildItems.GetItem(i).RenderHTML();
        }
        var propertiesAry = { Text: this.Text, NavigateUrl: this.NavigateUrl, ImageUrl: this.ImageUrl, ToolTip: this.ToolTip, Target: this.Target, Depth: this.Depth, IndexOfRoot: this.IndexOfRoot };
        propertiesAry.Depth++;
        subItems = (subItems == "") ? "" : xHiberarchyRenderProviders[xHiberarchy.RenderProviderID].BuildItemCollection(propertiesAry, subItems);
        propertiesAry.Depth--;
        return xHiberarchyRenderProviders[xHiberarchy.RenderProviderID].BuildItem(propertiesAry, subItems);
    }

    this.AddChildItem = function(Item) {
        this.ChildItems.Add(Item);
        this.ProcChildItemsDepth(this.Depth);
    }

    this.ProcChildItemsDepth = function(cDepth) {
        for (var i = 0; i < this.ChildItems.Count; i++) {
            with (this.ChildItems.GetItem(i)) {
                Depth = cDepth + 1;
                ProcChildItemsDepth(Depth);
            }
        }
    }

    this.ProcChildItemsIndexOfRoot = function(cIndexOfRoot) {
        for (var i = 0; i < this.ChildItems.Count; i++) {
            with (this.ChildItems.GetItem(i)) {
                IndexOfRoot = cIndexOfRoot;
                ProcChildItemsIndexOfRoot(cIndexOfRoot);
            }
        }
    }

    this.DataBind = function(xItem) {
        with (xHiberarchyDataDriver[xHiberarchy.DataDriverID]) {
            if (GetChildCount(GetChildren(xItem)) == 0) return;
            this.ChildItems.DataBind(GetChildren(xItem));
        }
    }
}
//---------------------xHiberarchy Item Class [END]--------

//------------------------------------------------------------------
//--------------------xHiberarchy Render Providers [START]----------
//------------------------------------------------------------------
//------------------------------MenuProvider1 [START]----------------------------------
var MenuProvider1 = {
    MenuLevel1Items: "",
    BuildMenuItem: function(ItemProperties, subItemsHTML) {
        var itemHTML = "";
        switch (ItemProperties.Depth) {
            case 0:
                {
                    itemHTML = "<li" + ((ItemProperties.IndexOfRoot == 0) ? " class=\"selectTag\"" : "") + "><a onmouseover=\"MenuProvider1.selectTag('tagContent" + ItemProperties.IndexOfRoot + "',this)\" href=\"javascript:void(0)\">" + ItemProperties.Text + "</a></li>";
                    MenuProvider1.MenuLevel1Items += subItemsHTML;
                    break;
                }
            case 1:
                {
                    itemHTML = "<li><a class=\"hide\" style=\"cursor:hand;\"" + ((ItemProperties.NavigateUrl == "") ? "" : " href=\"" + ItemProperties.NavigateUrl + "\"") + " target=\"" + ItemProperties.Target + "\">" + ItemProperties.Text + "</a>"
                        + "<!--[if lte IE 6]><a href=\"" + ((ItemProperties.NavigateUrl == "") ? "#" : ItemProperties.NavigateUrl + "\"") + " target=\"" + ItemProperties.Target + "\">" + ItemProperties.Text + "<table><tr><td><![endif]-->"
                        + subItemsHTML
                        + "<!--[if lte IE 6]></td></tr></table></a><![endif]-->"
                        + "</li>";
                    break;
                }
            case 2:
                {
                    //itemHTML = "<li><a onclick=\"this.parentNode.parentNode.parentNode.style.display='none';\" href=\"" + ItemProperties.NavigateUrl + "\"" + ((ItemProperties.Text.length <= 6) ? " style=\"width:90px;\"" : "") + " target=\"" + ItemProperties.Target + "\">&nbsp;" + ItemProperties.Text + "</a></li>";
                    itemHTML = "<li><a href=\"" + ItemProperties.NavigateUrl + "\"" + ((ItemProperties.Text.length <= 6) ? " style=\"width:90px;\"" : "") + " target=\"" + ItemProperties.Target + "\">&nbsp;" + ItemProperties.Text + "</a></li>";
                    break;
                }
            default:
                {
                    itemHTML = "<li><a href=\"" + ItemProperties.NavigateUrl + "\" target=\"" + ItemProperties.Target + "\">" + ItemProperties.Text + "</a></li>";
                    break;
                }
        }
        return itemHTML;
    },

    BuildMenuItemCollection: function(ItemProperties, MenuItemsHTML) {
        var itemCollHTML = "";
        switch (ItemProperties.Depth) {
            case 0:
                {
                    itemCollHTML = "<ul id=\"tags\">" + MenuItemsHTML + "</ul>";
                    break;
                }
            case 1:
                {
                    itemCollHTML = "<div class=\"tagContent" + ((ItemProperties.IndexOfRoot == 0) ? " selectTag" : "") + "\" id=\"tagContent" + ItemProperties.IndexOfRoot + "\"><div class=\"menu\"><ul>" + MenuItemsHTML + "</ul><div class=\"clear\"></div></div></div>";
                    break;
                }
            case 2:
                {
                    itemCollHTML = "<div><ul>" + MenuItemsHTML + "</ul></div>";
                    break;
                }
            default:
                {
                    itemCollHTML = "<div><ul>" + MenuItemsHTML + "</ul></div>";
                    break;
                }
        }
        return itemCollHTML;
    },

    BuildMenu: function(ItemCollections) {
        return ItemCollections + "<div id=\"tagContent\">" + MenuProvider1.MenuLevel1Items + "</div>";
    },

    selectTag: function(showContent, selfObj) {
        var tag = document.getElementById("tags").getElementsByTagName("li");
        var taglength = tag.length;
        for (i = 0; i < taglength; i++) {
            tag[i].className = "";
        }
        selfObj.parentNode.className = "selectTag";
        for (i = 0; j = document.getElementById("tagContent" + i); i++) {
            j.style.display = "none";
        }
        try {
            document.getElementById(showContent).style.display = "block";
        } catch (e) { }
    }
}
//------------------------------MenuProvider1 [END]----------------------------------

//------------------------------Hiberarchy Render Providers Interface [START]--------
var xHiberarchyRenderProviders = {
    0: {
        //-----------Interface defines[START]-----------
        ProviderName: "MenuProvider1",
        Version: "1.0",
        BuildHiberarchy: MenuProvider1.BuildMenu,
        BuildItem: MenuProvider1.BuildMenuItem,
        BuildItemCollection: MenuProvider1.BuildMenuItemCollection,
        //-----------Interface defines[END]-------------

        //-----------3rd part defines[START]------------
        selectTag: MenuProvider1.selectTag
        //-----------3rd part defines[END]--------------
    }
}
//------------------------------Hiberarchy Render Providers Interface [END]---------

//-----------------Base Menu Data Driver [START]----------------
var xBaseMenuDataDriver = {
    GetChildCount: function(xDataSet) {
        return xDataSet.Items.length;
    },

    GetChildren: function(xDataSet) {
        return xDataSet["NextLevel"];
    },

    GetChildInfo: function(xDataSet) {
        return { Text: xDataSet["MenuText"], NavigateUrl: xDataSet["WindowId"], Target: xDataSet["Target"] };
    },

    GetChild: function(xDataSet, childIndex) {
        return xDataSet.Items[childIndex];
    }
}
//-----------------Base Menu Data Driver [END]----------------

//------------------------------Hiberarchy Data Driver Interface [START]-----------
var xHiberarchyDataDriver = {
    0: {
        //-----------Interface defines[START]-----------
        GetChildCount: xBaseMenuDataDriver.GetChildCount,
        GetChildren: xBaseMenuDataDriver.GetChildren,
        GetChildInfo: xBaseMenuDataDriver.GetChildInfo,
        GetChild: xBaseMenuDataDriver.GetChild
        //-----------Interface defines[END]-------------

        //-----------3rd part defines[START]------------
        //-----------3rd part defines[END]--------------
    }
}
//------------------------------Hiberarchy Data Driver Interface [END]--------------

//--------------------xHiberarchy Render Providers [END]----------