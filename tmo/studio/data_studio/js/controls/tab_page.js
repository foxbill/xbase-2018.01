

function TabItem(tabLabel, tabPanel) {
    this.TabPanel = null;
    this.TabLabel = null;

    if (typeof (tabLabel) == "object")
        this.TabLabel = tabLabel;
    else {
        this.TabLabel = document.getElementById(tabLabel);
        if (this.TabLabel == null) return null;
    }

    if (typeof (tabPanel) == "object")
        this.TabPanel = tabPanel;
    else {
        this.TabPanel = document.getElementById(tabPanel);
        if (this.TabPanel == null) return null;
    }
}


function TabPage() {
    var _tabs = new Array();
    var _curTab = null;
    var self = this;
    this.selectTagClass = "selectTag";
    

    function SetCurTab(tab) {


//        selfObj.parentNode.className = self.selectTagClass;

        tab.TabPanel.style.display = "block";
        tab.TabLabel.className = self.selectTagClass;
        for (var i = 0; i < _tabs.length; i++) {

            if (_tabs[i] != tab) {
                _tabs[i].TabLabel.className = "";
                _tabs[i].TabPanel.style.display = "none";
            }

            _curTab = tab;
        }
    }

    function TabClick(tab) {
        SetCurTab(tab);
    }

    function InitTab(tab) {
        //     var tab = new TabItem();
        tab.TabPanel.style.display = "none";
        tab.TabLabel.onclick = function() {
        TabClick(tab);
            tab.TabLabel.blur();
        }
    }

    this.AddTab = function(tabLabel, tabPanel) {
        var tab = new TabItem(tabLabel, tabPanel);
        if (tab != null) {

            _tabs.push(tab);

            InitTab(tab);

            if (_curTab == null) SetCurTab(tab);

        }
    }

    this.SelectTab = function(tabLabel) {
        if (typeof (tabLabel) == "string") {
            tabLabel = document.getElementById(tabLabel);
        }
        for (var i = 0; i < _tabs.length; i++) {
            var tab = _tabs[i];
            if (tab.TabLabel == tabLabel) {
                SetCurTab(tab);
                break;
            }
        }
    }

}