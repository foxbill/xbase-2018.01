
/***  
*       easyui datagrid for xbase 扩展  
*
*   1. 覆盖缺省loader方法，使其在后台发生错误的时候，能够报错。
*   2. 扩展editors，使其能能够支持，datetimebox控件。
*   3. 覆盖过滤操作 operators ,使过滤操作支持中文，并支持空和非空的查询。
*   4. 覆盖扩展beginEdit方法 ，在beginEdit时，先endEdit,增加了回车跳格的功能。
*   5. 增加方法：
*       1）getSelectIndex()给出当前选中行Index。
*       2）deleteSel()删除选中的行。
*       3）getEditIndex()给出当前正在编辑的行。
*       4) setEditMode(bool) 设置编辑模式，bool为真进入编辑，bool为假退出编辑。
***/

(function ($) {


    /***
    *   扩展方法：getSelectIndex();
    *   获取选中行的index,如果没有选中的行则返回0;
    ***/
    function getSelectIndex(jq) {
        var row = jq.datagrid("getSelected");
        if (row)
            return jq.datagrid("getRowIndex", row);
        return 0;
    }

    function deleteSel(jq) {
        var index = getSelectIndex(jq);
        var edIndex = getEditIndex(jq);
        if (edIndex = index)
            jq.datagrid("cancelEdit", index);
        jq.datagrid("deleteRow", index);
    }


    function getEditIndex(jq) {
        var opts = jq.datagrid("options");
        return opts._EditIndex;
    }

    function setEditMode(jq, mode) {
        var opts = jq.datagrid("options");
        var rowIndex = getSelectIndex(jq);
        opts.editMode = mode;
        if (opts.editMode) {
            jq.datagrid("beginEdit", rowIndex);

            opts.onClickRow = function (rIndex, row) {
                if ($.fn.datagrid.defaults.onClickRow)
                    $.fn.datagrid.defaults.onClickRow.call(opts, rIndex, row);
                jq.datagrid("beginEdit", rIndex);
            };
        } else {
            if (opts._EditIndex > -1)
                jq.datagrid('endEdit', opts._EditIndex);
            opts.onClickRow = $.fn.datagrid.defaults.onClickRow;
        }
    }



    function getHCMenu(jq) {
        var opts = jq.datagrid("options");
        if (!opts.headCMenu)
            opts.headCmenu = createHCMenu();
        return opts.headCmenu;

        function createHCMenu() {
            var cmenu = $('<div/>').appendTo('body');
            cmenu.menu({
                onClick: function (item) {
                    if (item.iconCls == 'icon-ok') {
                        jq.datagrid('hideColumn', item.name);
                        cmenu.menu('setIcon', {
                            target: item.target,
                            iconCls: 'icon-empty'
                        });
                    } else {
                        jq.datagrid('showColumn', item.name);
                        cmenu.menu('setIcon', {
                            target: item.target,
                            iconCls: 'icon-ok'
                        });
                    }
                }
            });

            var fields = $('#dg').datagrid('getColumnFields');
            for (var i = 0; i < fields.length; i++) {
                var field = fields[i];
                var col = $('#dg').datagrid('getColumnOption', field);
                cmenu.menu('appendItem', {
                    text: col.title,
                    name: field,
                    iconCls: 'icon-ok'
                });
            }
            return cmenu;
        }

    }
    /***
    *  覆盖loader属性
    ***/
    function gridLoader(param, success, error) {
        var that = $(this);
        var opts = that.datagrid("options");
        if (!opts.url) {
            return false;
        }
        //     var cache = that.data().datagrid.cache;
        //     if (!cache) {
        $.ajax({
            type: opts.method,
            url: opts.url + "?" + $.param(opts.queryParams),
            data: param,
            dataType: "json",
            success: function (data) {
                if (data.Err) {
                    alert(data.Err.Text);
                    error();
                    return false;
                }
                // that.data().datagrid['cache'] = data;
                success(data);
            },
            error: function () {
                error.apply(this, arguments);
            }
        });
    }


    $.extend($.fn.datagrid.defaults.editors, {
        datetimebox: {
            init: function (container, options) {
                var input = $('<input type="text">').appendTo(container);
                if (!options) {
                    options = new Object();
                    options.required = false;
                    options.showSeconds = true;
                }
                input.datetimebox(options);
                return input;
            },
            destroy: function (target) {
                $(target).datetimebox("destroy");
            },
            getValue: function (target) {
                return $(target).datetimebox("getValue");
            },
            setValue: function (target, value) {
                $(target).datetimebox("setValue", value);
            },
            resize: function (target, width) {
                $(target).datetimebox("resize", width);
            }
        }
    });

    /***
    *   扩展并覆盖属性，以适应远程过滤特点
    ***/
    $.extend($.fn.datagrid.defaults, {
        filterBtnIconCls: 'icon-filter',
        filterMenuIconCls: 'icon-ok',
        remoteFilter: true,
        filterDefaultText: false, //add by @lhp 2013-11-21 默认不显示过滤项  
        filterDelay: 500,
        filterRules: [],
        filterStringify: function (data) { return JSON.stringify(data); },
        loader: gridLoader,
        editMode: false,
        _EditIndex: -1,
        headCMenu: null
    });


    $.fn.datagrid.defaults.operators = {
        nofilter: {
            text: '取消查询'
        },
        contains: {
            text: '包含',
            isMatch: function (source, value) {
                return source.toLowerCase().indexOf(value.toLowerCase()) >= 0;
            }
        },
        equal: {
            text: '等于',
            isMatch: function (source, value) {
                return source == value;
            }
        },
        notequal: {
            text: '不等于',
            isMatch: function (source, value) {
                return source != value;
            }
        },
        beginwith: {
            text: '以开始',
            isMatch: function (source, value) {
                return source.toLowerCase().indexOf(value.toLowerCase()) == 0;
            }
        },
        endwith: {
            text: '以结束',
            isMatch: function (source, value) {
                return source.toLowerCase().indexOf(value.toLowerCase(), source.length - value.length) !== -1;
            }
        },
        less: {
            text: '小于',
            isMatch: function (source, value) {
                return source < value;
            }
        },
        lessorequal: {
            text: '小于等于',
            isMatch: function (source, value) {
                return source <= value;
            }
        },
        greater: {
            text: '大于',
            isMatch: function (source, value) {
                return source > value;
            }
        },
        greaterorequal: {
            text: '大于等于',
            isMatch: function (source, value) {
                return source >= value;
            }
        },
        isnull: {
            text: '为空',
            isMatch: function (source, value) {
                return source == value;
            }
        },
        notnull: {
            text: '不为空',
            isMatch: function (source, value) {
                return source != value;
            }
        }
    };

    if ($.fn.treegrid)
        $.fn.treegrid.defaults.operators = $.fn.datagrid.defaults.operators;



    /***
    *  覆盖方endEdit法扩展
    *  1.在endEdit之前先校验
    ***/
    var oldEndEdit = $.fn.datagrid.methods.endEdit;
    $.fn.datagrid.methods.endEdit = function (jq) {
        var opts = jq.datagrid("options");
        var editIndex = getEditIndex(jq);
        if (editIndex == undefined || editIndex < 0) {
            return true
        }
        if (jq.datagrid('validateRow', editIndex)) {
            oldEndEdit.call($.fn.datagrid.methods, jq, editIndex)
            //jq.datagrid('endEdit', editIndex);
            opts._EditIndex = -1;
            return true;
        } else {
            return false;
        }
    }

    /***
    *  覆盖方beginEdit法扩展
    *  1.支持回车跳格
    ***/
    var oldBeginEdit = $.fn.datagrid.methods.beginEdit;
    $.fn.datagrid.methods.beginEdit = function (jq, index) {

        if (!jq.datagrid('endEdit'))
            return;

        var opts = jq.datagrid("options");
        //     var opts = $.data(jq, "datagrid").options;

        // var grd = jq[0];

        //        if (opts._EditIndex > -1)
        //            jq.datagrid('endEdit', opts._EditIndex);

        opts._EditIndex = index;

        //        if (grd.editingRow || grd.editingRow == 0)
        //            jq.datagrid('endEdit', grd.editingRow);

        //        grd.editingRow = index;

        //oldBeginEdit.call($.fn.datagrid.methods, jq, index);
        oldBeginEdit(jq, index);
        //    jq.datagrid('beginEdit', index);


        var eds = jq.datagrid('getEditors', index);


        function getEditorElement(ed) {
            if (ed.type == "text" || ed.type == "numberbox")
                return ed.target[0];
            else
                return ed.target.combo("textbox")[0];
        }

        for (var i = 0; i < eds.length; i++) {
            var ed = eds[i];
            var el = getEditorElement(ed);
            el.tabIndex = i;
            $(el).keydown(function (e) {
                var tabIndex = this.tabIndex;
                if (e.keyCode == 13) {
                    if (tabIndex < eds.length - 1) {
                        var el1 = getEditorElement(eds[tabIndex + 1]);
                        $(el1).focus();
                        $(el1).select();
                    }
                    else {
                        if (opts.editMode)
                            jq.datagrid('beginEdit', opts._EditIndex + 1);
                        else
                            jq.datagrid('endEdit');
                    }

                }
            })

        }

        jq.datagrid('selectRow', index);

        if (eds.length) {
            $(eds[0].target).focus();
            $(eds[0].target).select();
        }

    };



    $.extend($.fn.datagrid.methods, {
        getSelectIndex: getSelectIndex,
        setEditMode: setEditMode,
        getEditIndex: getEditIndex,
        deleteSel: deleteSel
    })


})(jQuery)