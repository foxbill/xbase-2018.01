(function ($) { 

	$.fn.pageTool = function (options) {//options 经常用这个表示有许多个参数。
	var pageCount=0; 
var defaultVal = { 
datatotal: 12, 
pagesize:10,
currnum:1,
maxpagenum:5,
tooltype:'pagination',
showpagenum:true,
jumpPageFunc:function  (num) {},
customStyle:{
	liClass:'',
	prevClass:'',
	nextClass:'',
	prevContext:'&laquo;',
	nextContext:'&raquo;'
}
}; 
　　　　　　//默认值 
var obj = $.extend({},defaultVal, options); 

var count_ = obj.datatotal * 1.0 / (obj.pagesize * 1.0);

pageCount = Math.ceil(count_);


var _this=$(this);
var isInit=true;

if(obj.maxpagenum>pageCount)
	obj.maxpagenum=pageCount;

function initPageTool (num) {
	var step= Math.floor(obj.maxpagenum/2);
		if(!isInit)
		{
		if(((pageCount)-num)<step||num<=step)
		{
			return;
		}
		}
		
		//计算是要变动的分页按钮序列
		var lp_num=(num+step);
		var i=1;
		if(lp_num>obj.maxpagenum)
		{
			i=num-step;
			if(lp_num>pageCount)
			{
				i=pageCount-obj.maxpagenum+1;
			}
			if(i<1)
			{
				i=1;
			}

		}
		_this.empty();
		//上一页
		var prv=$("<li class='"+obj.customStyle.prevClass+"' data-num='0'><a href='javascript:;' data-num='0' >"+obj.customStyle.prevContext+"</a></li>");
		prv.bind("click",prevPage);
		_this.append(prv);
		//页码
		if(obj.tooltype=="pager")
		{
			if(obj.showpagenum)
			{
				var li=$("<li class='"+obj.customStyle.liClass+"' style='margin: 0 10px;' data-content></li>");
				_this.append(li);	
			}		
		}
		else
		{
		for (var j = 0; j< obj.maxpagenum; j++) {
			var li=$("<li class='"+obj.customStyle.liClass+"' data-num='"+(i)+"'><a href='javascript:;' data-num='"+(i)+"'>"+(i)+"</a></li>");		
			_this.append(li);
			li.find("a").bind("click",jumpPage);
			i++;
		}

		}
		//下一页
		var nxt=$("<li class='"+obj.customStyle.nextClass+"' data-num='"+(pageCount+1)+"'><a href='javascript:;' data-num='"+(pageCount+1)+"'>"+obj.customStyle.nextContext+"</a></li>");
		nxt.bind("click",nextPage);
		_this.append(nxt);

		if(obj.currnum===null||obj.currnum===undefined)
		{
			obj.currnum=1;
		}

		_this.attr("data-current-page",obj.currnum);
		var a=_this.find("li[data-num='"+(obj.currnum)+"']")
		a.addClass("active");

		//禁用对应按钮
		if(obj.currnum===1)
		{
			var f=_this.find("li[data-num='0']")
			f.addClass("disabled");
		}
		if(obj.currnum===pageCount)
		{
			var f=_this.find("li[data-num='"+(pageCount+1)+"']")
			f.addClass("disabled");
		}
		//翻页操作
		if(obj.tooltype=="pager"&&obj.showpagenum)
		{
			var p=_this.find("li[data-content]");
			p.html(obj.currnum+"/"+pageCount);
		}


		isInit=false;
	}


	function jumpPage  () {
		// body...
		var num=parseInt($(this).attr("data-num"));
	
		jumpPageByNum(num);
	}

	function jumpPageByNum (num) {
		// body...

		initPageTool(num);

		var a=_this.find("li[data-num='"+(num)+"']");
		a.addClass("active").siblings().removeClass("active");		
		if (num === 0) return;
		if (num === 1)
		{
			var f=_this.find("li[data-num='0']");
			f.addClass("disabled").siblings().removeClass("disabled");
		}else if(num===pageCount)
		{
			var e=_this.find("li[data-num='"+(pageCount+1)+"']");
			e.addClass("disabled").siblings().removeClass("disabled");;
		}else
		{
			_this.find("li").removeClass("disabled");
		}

		_this.attr("data-current-page",num);

		if(obj.tooltype=="pager"&&obj.showpagenum)
		{
			var p=_this.find("li[data-content]");
			p.html(num+"/"+pageCount);
		}

		obj.jumpPageFunc(num)
	}

	function  prevPage() {
		// body...
		var o=_this.attr("data-current-page");
		if(o===null||o===undefined)
		{
			o=1;
		}
		o--;
		if(o<=1)
		{
			o==1;
		}

		jumpPageByNum(parseInt(o));

	}

	function nextPage () {
		// body...
		var o=_this.attr("data-current-page");
		if(o===null||o===undefined)
		{
			o=pageCount;
		}
		o++;
		if(o>pageCount)
		{
			o=pageCount;
		}
		jumpPageByNum(parseInt(o));

	}

		initPageTool(1);


	}
	
})(jQuery); 
