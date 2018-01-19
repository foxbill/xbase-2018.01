
Array.prototype.indexOf = function (item) {

    for (var i = 0; i < this.length; i++) {
        if (item == this[i])
            return i;
    }
    return -1;

}

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


function TabControl(tabs, tabPanels, activeEventName, activeClass, deActiveClass) {
    var _tabs = new Array();
    var _panels = new Array();
    var _curTab = null;
    var _actIndex = 0;

    //   this.activeClassName = null;
    //    this.deActiveClassName = null;
    this.switchDisplay = false;
    var _this = this;
    var _stopPlay = false;


    this.onTabActive = function (tab) {
    }
    function SetCurTab(tab) {
        //  tab.TabPanel.style.display = "";
        _actIndex = _tabs.indexOf(tab);
        var actPanel = _panels[_actIndex];

        if (!activeClass)
            activeClass = deActiveClass;


        if (_this.switchDisplay) {
            if (actPanel.style.display == "block" || !actPanel.style.display)
                actPanel.style.display = "none";
            else
                actPanel.style.display = "block";

            if (actPanel.style.display != "none")
                tab.className = activeClass
            else
                tab.className = deActiveClass;
            return;
        } else {
            actPanel.style.display = "block";
            tab.className = activeClass
        }

        if (!_this.switchDisplay) {
            for (var i = 0; i < _panels.length; i++) {
                if (i != _actIndex) {
                    _panels[i].style.display = "none";
                    _tabs[i].className = deActiveClass;
                }
            }
        }


        _curTab = tab;

        if (_this.onTabActive) {
            _this.onTabActive(tab);
        }
    }


    if (tabs.length && !deActiveClass)
        deActiveClass = tabs[tabs.length - 1].className;

    var acts = [];
    for (var i = 0; i < tabs.length; i++) {
        var tab = tabs[i];
        if ((typeof tab).toLowerCase() == "string")
            tab = document.getElementById(tab);
        if (activeEventName) {
            tab[activeEventName] = function () {
                _stopPlay = true;
                SetCurTab(this);
            };
            tab.onmouseout = function () {
                _stopPlay = false;
            }
        }
        else
            tab.onclick = function () {
                SetCurTab(this);
            }

        _tabs.push(tab);

        if (i < tabPanels.length) {
            var panel = tabPanels[i];
            if (panel.style.display != "none") {
                _actIndex = i;
                acts.push(i);
            }
            panel.onmouseover = function () {
                _stopPlay = true;
            }
            panel.onmouseout = function () {
                _stopPlay = false;
            }
            if ((typeof panel).toLowerCase() == "string")
                panel = document.getElementById(panel);
            _panels.push(panel);
        }

    }

    if (acts.length > 0)
        _actIndex = 0;

    SetCurTab(tabs[_actIndex]);

    this.closeAll = function () {
        for (var i = 0; i < _panels.length; i++) {
            _panels[i].style.display = "none";
        }
    }


    function TabClick(tab) {
        SetCurTab(tab);
    }

    //    function InitTab(tab) {
    //        //     var tab = new TabItem();
    //        tab.TabPanel.style.display = "none";
    //        tab.TabLabel.onclick = function () {
    //            TabClick(tab);
    //        }
    //    }

    //    this.AddTab = function (tabLabel, tabPanel) {
    //        var tab = new TabItem(tabLabel, tabPanel);
    //        if (tab != null) {

    //            _tabs.push(tab);

    //            InitTab(tab);

    //            if (_curTab == null) SetCurTab(tab);

    //        }
    //    }

    this.SelectTab = function (tabLabel) {
        if (typeof (tabLabel) == "string") {
            tabLabel = document.getElementById(tabLabel);
        }
        SetCurTab(tabLabel);
    }

    this.getActiveTab = function () {
        return _curTab;
    }

    this.selectTab = this.SelectTab;

    var _playIndex = 0;


    this.play = function (speed) {
        var _this = this;
        setInterval(function () {
            if (_stopPlay) return;
            SetCurTab(_tabs[_playIndex]);
            _playIndex++;
            if (_playIndex >= _tabs.length)
                _playIndex = 0;
        }, speed);
    }

}

function NavBar(tabs, tabPanels, activeEventName) {
    var _tabs = new Array();
    var _panels = new Array();
    var _curTab = null;
    var _actIndex;

    //   this.activeClassName = null;
    //   this.deActiveClassName = null;

    function SetCurTab(tab) {
        //  tab.TabPanel.style.display = "";
        _actIndex = _tabs.indexOf(tab);
        for (var i = 0; i < _panels.length; i++) {
            if (i != _actIndex)
                _panels[i].style.display = "none";
            else
                _panels[i].style.display = "block";
        }

    }

    for (var i = 0; i < tabs.length; i++) {
        var tab = tabs[i];

        if ((typeof tab).toLowerCase() == "string")
            tab = document.getElementById(tab);
        if (activeEventName)
            tab[activeEventName] = function () {
                SetCurTab(this);
            }
        else
            tab.onclick = function () {
                SetCurTab(this);
            }

        _tabs.push(tab);

        if (i < tabPanels.length) {
            var panel = tabPanels[i];
            if ((typeof panel).toLowerCase() == "string")
                panel = document.getElementById(panel);
            _panels.push(panel);
        }

    }

    this.closeAll = function () {
        for (var i = 0; i < _panels.length; i++) {
            _panels[i].style.display = "none";
        }
    }

    function TabClick(tab) {
        SetCurTab(tab);
    }

    //    function InitTab(tab) {
    //        //     var tab = new TabItem();
    //        tab.TabPanel.style.display = "none";
    //        tab.TabLabel.onclick = function () {
    //            TabClick(tab);
    //        }
    //    }

    //    this.AddTab = function (tabLabel, tabPanel) {
    //        var tab = new TabItem(tabLabel, tabPanel);
    //        if (tab != null) {

    //            _tabs.push(tab);

    //            InitTab(tab);

    //            if (_curTab == null) SetCurTab(tab);

    //        }
    //    }

    this.SelectTab = function (tabLabel) {
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