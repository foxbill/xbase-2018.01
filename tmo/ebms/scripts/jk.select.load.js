/*属性格式：<select    wbo-select="{'tar':'pt_productType','frmfield':'Type','nxt':'pmodel','filter':'ParentId'}"  onchange="selChange(this)"></select>
    tar:数据源
    frmfield:表单提交字段id
    nxt:级联select id
    filter:过滤父级字段
*/
function initSelect(selector, parentid, selval) {

	 var slsattr=selector.attr("wbo-select");

   slsattr=eval("("+slsattr+")");

    var db = slsattr.tar+ ".data";
    var filter=slsattr.filter;
    var list;
    if(filter!=undefined&&filter!="")
    {
    	list = $.rCall(db, { page: 1, rows: 100, filterRules: [{ field: filter, op: 'equal', value: parentid }] });

    }else
    {
    	list = $.rCall(db, { page: 1, rows: 100, filterRules: [] });
    }

    selector.empty();
    selector.append("<option value='-1' >请选择</option>");

    var frmhiden=slsattr.frmfield;
     $("#"+frmhiden).val("-1");

    for (var i = 0; i < list.total; i++) {
        selector.append("<option value='" + list.rows[i].ID + "'>" + list.rows[i].Name + "</option>");
    }
    if (selval != null && selval != undefined) {
    	selector.find("option").removeAttr("selected");
        selector.find("option[value=" + selval + "]").attr("selected", true);

         $("#"+frmhiden).val(selval);
        // alert(selector.find("option [value=" + selval + "]").text());
    }
    /*
    if (selector.next() != undefined) {
        selector.next().empty();
        selector.next().append("<option value='-1' >请选择</option>");
    }
    */

}
//级联改变
function selChange(obj) {
    var el = $(obj);

   var slsattr=el.attr("wbo-select");

   slsattr=eval("("+slsattr+")");
  
   var frmhiden=slsattr.frmfield;
    
    var v = el.find("option:selected").val();
    $("#"+frmhiden).val(v);
    if (v != -1) {

     var nextid=slsattr.nxt;
     if(nextid!=undefined&&nextid!="")
     {
     	var nextobj=$("#"+nextid);
        initSelect(nextobj, v);
     }
    	
    }
}