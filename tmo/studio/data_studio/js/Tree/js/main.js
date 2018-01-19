	var strShow = '显示';
	var strHide = '隐藏';
	var strRtl = '';
	var iflag = 0;

	function go(url)
	{
		window.open(url, "_self");
	}

	function OpenInNewWindow(url)
	{
		go(url);
	}
	function ControlLegs2(theDivName)
	{
		if(theDivName.style.display == "block")
		{
			theDivName.style.display = "none";
			
		}
		else
		{
			theDivName.style.display = "block";
			
		}
	}

	function IniDiv(iID,ispackage)
	{
		var spans = document.body.getElementsByTagName("span");
		for (i=0; i < spans.length; i++)
		{
			if(spans[i].id.indexOf("ok" + ispackage + iID) >= 0)
			{
				
				document.all["ChangeLegDiv" + ispackage + iID].innerHTML += spans[i].innerHTML;
			}
		}
	}

	function ControlItinerary(theDivName,ying)
	{
		if(ying.checked)
		{
			var mydivs = document.body.getElementsByTagName("div");
			var k = mydivs.length;
			var i = 0;
			while(i < k)
			{
				if(mydivs[i].id.indexOf("myDiv") >= 0)
				{
					document.all[mydivs[i].id].style.display = "none";
				}
				i ++;
			}
			theDivName.style.display = "block";
		}
	}
	function ControlLegs(sourceDivName,targetDivname,ying)
	{
		//targetDivname.innerHTML = sourceDivName.innerHTML;
		ControlItinerary(sourceDivName,ying);
	}
	function IniDivs(sourceDivName,targetDivname)
	{
		if(iflag == 0)
		{
			targetDivname.innerHTML = sourceDivName.innerHTML;
		}
	}


function ExpandDiv(theDivName)
{
	InitializeGlobalData();

	if (null == theDivName || typeof(theDivName) == "undefined") return; var theDiv = allDivsInPage[theDivName]; if (null == theDiv || typeof(theDiv) == "undefined") return;
	theDiv.style.display = "block";

	var thePic = allImagesInPage[theDivName + "_img"];
	if (null != thePic && typeof(thePic) != "undefined")
		{
		thePic.src = "../images/none.gif";
		thePic.alt = strHide;
		}
}

function CollapseDiv(theDivName)
{
	InitializeGlobalData();

	if (null == theDivName || typeof(theDivName) == "undefined") return; var theDiv = allDivsInPage[theDivName]; if (null == theDiv || typeof(theDiv) == "undefined") return;
	theDiv.style.display = "none";

	var thePic = allImagesInPage[theDivName + "_img"];
	if (null != thePic && typeof(thePic) != "undefined")
		{
		thePic.src = "../images/block" + strIsRtl + ".gif";
		thePic.alt = strShow;
		}
}

function ToggleDiv(theDivName)
{
	InitializeGlobalData();

	if (null == theDivName || typeof(theDivName) == "undefined") return; var theDiv = allDivsInPage[theDivName]; if (null == theDiv || typeof(theDiv) == "undefined") return;

	if (theDiv.style.display.toUpperCase() != "BLOCK")
		ExpandDiv(theDivName);
	else
		CollapseDiv(theDivName);
}

function AlterAllDivs(displayStyle)
{
	InitializeGlobalData();

	if (null == allDivsInPage || typeof(allDivsInPage) == "undefined")
		return;

	if (typeof(allDivsInPage["divShowAll"]) != "undefined" &&
		typeof(allDivsInPage["divHideAll"]) != "undefined")
		{
		if (displayStyle == "block")
			{
			allDivsInPage["divShowAll"].style.display = "none";
			allDivsInPage["divHideAll"].style.display = "block";
			}
		else
			{
			allDivsInPage["divShowAll"].style.display = "block";
			allDivsInPage["divHideAll"].style.display = "none";
			}
		}

	AlterAllDivsSpans(document.body.getElementsByTagName("DIV"), displayStyle);
	AlterAllDivsSpans(document.body.getElementsByTagName("SPAN"), displayStyle);
}

function ToggleAllDivs()
{
	InitializeGlobalData();

	if (fExpandedAssistance)
		AlterAllDivs("none");
	else
		AlterAllDivs("block");

	fExpandedAssistance = !fExpandedAssistance;
}

function ToggleAll()
{
	InitializeGlobalData();
	ToggleAllDivs();
}

function InitializeGlobalData()
{
	if ('undefined' != typeof(strRtl))
		strIsRtl = strRtl;

	var divs = document.body.getElementsByTagName("DIV");
	var spans = document.body.getElementsByTagName("SPAN");

	var countDiv = 0;
	var countSpan = 0;
	if (typeof(divs) != "undefined" && null != divs)
		countDiv = divs.length;

	if (typeof(spans) != "undefined" && null != spans)
		countSpan = spans.length;

	allDivsInPage = new Array();
	for (i=0; i < countDiv; i++)
		if (typeof(divs[i].id) != "undefined" &&
			null != divs[i].id &&
			divs[i].id.length > 0)
			allDivsInPage[divs[i].id] = divs[i];

	for (i=0; i < countSpan; i++)
		if (typeof(spans[i].id) != "undefined" &&
			null != spans[i].id &&
			spans[i].id.length > 0)
			allDivsInPage[spans[i].id] = spans[i];

	allImagesInPage = document.body.getElementsByTagName("IMG");
}

function OnSeeAlsoClicked()
{
	InitializeGlobalData();

	if (null == allDivsInPage || typeof(allDivsInPage) == "undefined")
		return;

	if (typeof(allDivsInPage["divSeeAlsoShowBullet"]) != "undefined" &&
		typeof(allDivsInPage["divSeeAlsoHideBullet"]) != "undefined")
		{
		if (allDivsInPage["divSeeAlsoShowBullet"].style.display.toUpperCase() == "INLINE")
			{
			allDivsInPage["divSeeAlso"].style.display = "block";
			allDivsInPage["divSeeAlsoShowBullet"].style.display = "none";
			allDivsInPage["divSeeAlsoHideBullet"].style.display = "inline";
			}
		else
			{
			allDivsInPage["divSeeAlso"].style.display = "none";
			allDivsInPage["divSeeAlsoShowBullet"].style.display = "inline";
			allDivsInPage["divSeeAlsoHideBullet"].style.display = "none";
			}
		}
}
//全选
function SelAllCheck(tabID)
{
	var Tab = document.all(tabID);
	if(Tab == null )
	return;
	var chks = Tab.all.tags("input");
	if(chks == null )
	return;
	if(chks.length)
	{
		for(i=0;i<chks.length;i++)
		{
			if(chks[i].type == "checkbox" )
			{
				chks[i].checked = true;
			}
		}
	}
	else if(chks.type == "checkbox" )
	{
		chks.checked = true;
	}
}
//反选
function SelAllNotCheck(TabID)
{
	var Tab = document.all(TabID);
	if(Tab == null )
	return;
	var chks = Tab.all.tags("input");
	if(chks == null )
	return;
	if(chks.length)
	{
		for(i=0;i<chks.length;i++)
		{
			if(chks[i].type == "checkbox")
			{
				chks[i].checked = !chks[i].checked;
			}
		}
	}
	else if(chks.type == "checkbox")
	{
		chks.checked = !chks.checked;	
	}
}
function OnLangChange(langID,tableID)
{
    var lang = $("#" +langID).val();
    var trs =$("#" + tableID).find("tr").each(function()
    {              
        var tr = $(this);
        var trlang = tr.attr("lang");        
      
        if(trlang == "")
            return;           
                  
        if(trlang != lang)
        {
            tr.hide();
        }
        else
        {
            tr.show();
        }    
        
        if(lang != "cn")
        {
            if(trlang == "cn")
            {
                tr.show();
                EnableChild(tr,true);
            }            
        }
        else
        {
            if(trlang == "cn")
            {                
                EnableChild(tr,false);
            }            
        }      
    });   
}    
function EnableChild(el,disabled)
{    
    el.find("input").each(function()
    {
        $(this)[0].disabled = disabled;        
    });                             
    el.find("textarea").each(function()
    {
        $(this)[0].disabled = disabled;
    });                             
    el.find("select").each(function()
    {
        $(this)[0].disabled = disabled;
    });        
    
}
function TreeView_ToggleNode( index, node, lineType, children) 
{
    var img = node.childNodes[0];
    var newExpandState;
    if (children.style.display == "none") 
    {
        children.style.display = "block";
        newExpandState = "e";
        if ((typeof(img) != "undefined") && (img != null)) 
        {
            if(img.src == tv_img_plus)
                img.src=tv_img_minus;
            else
                img.src= tv_img_minusbottom;
                
            img.alt= "收缩";                                       
        }
    }
    else
    {
        children.style.display = "none";                       
        newExpandState = "c";
        if ((typeof(img) != "undefined") && (img != null)) 
        {
            if(img.src == tv_img_minus)
                img.src=tv_img_plus;
            else
                img.src= tv_img_plusbottom;
                                      
            img.alt = "展开";
        }
    }
}

if (!document.getElementById("CalFrame"))
{
	document.write('<iframe id=CalFrame name=CalFrame frameborder=0 src=../../../xbase.js/calendar.htm style=display:none;position:absolute;z-index:100></iframe>');
	document.onclick=hideCalendar;		
	document.write("<div id='HaveDate'></div>");
}

function showCalendar(sImg,bOpenBound,sFld1,sFld2,sCallback,bCanSelect)
{
	var fld1,fld2,fld3,CanSelect;
	var cf=document.getElementById("CalFrame");
	var wcf=window.frames.CalFrame;
	var oImg=document.getElementById(sImg);
	if(!oImg){alert("控制对象不存在！");return;}
	if(!sFld1){alert("输入控件未指定！");return;}
	fld1=document.getElementById(sFld1);
	if(!fld1){alert("输入控件不存在！");return;}
	if(fld1.tagName!="INPUT"||fld1.type!="text"){alert("输入控件类型错误！");return;}
	if(sFld2)
	{
		fld2=document.getElementById(sFld2);
		if(!fld2){alert("参考控件不存在！");return;}
		if(fld2.tagName!="INPUT"||fld2.type!="text"){alert("参考控件类型错误！");return;}
	}
	if(!wcf.bCalLoaded){alert("日历未成功装载！请刷新页面！");return;}
	if(cf.style.display=="block"){cf.style.display="none";return;}
	
	var eT=0,eL=0,p=oImg;
	var sT=document.body.scrollTop,sL=document.body.scrollLeft;
	var eH=oImg.height,eW=oImg.width;
	while(p&&p.tagName!="BODY"){eT+=p.offsetTop;eL+=p.offsetLeft;p=p.offsetParent;}
	cf.style.top=((document.body.clientHeight-(eT-sT)-eH>=cf.height)?eT+eH:eT-cf.height)+20;
	cf.style.left=(document.body.clientWidth-(eL-sL)>=cf.width)?eL:eL+eW-cf.width;
	cf.style.display="block";
	
	wcf.openbound=bOpenBound;
	wcf.fld1=fld1;
	wcf.fld2=fld2;
	wcf.fld3=sFld1;	
	wcf.CanSelect=bCanSelect;
	wcf.callback=sCallback;
	wcf.initCalendar();						
}
function hideCalendar()
{
	var cf=document.getElementById("CalFrame");
	cf.style.display="none";
}

function setCheckInDate(d,tom)
{
	document.getElementById(tom).value=d;
	callMethod(d);				//定义这个方法，可入自己的逻辑
}
var DateCtrlID = 0;
function init()
{    
    $("span.spndate").each(function()
    {      
        DateCtrlID++;
    
        var html = "<input type='text' value='2008-02-01' readonly='readonly' id='DateCtrl" 
        + DateCtrlID + "' onclick=\"javascript:event.cancelBubble=true;showCalendar('DateCtrl" 
        + DateCtrlID + "',false,'DateCtrl"
        + DateCtrlID + "','','')\" class='editdate' />&nbsp;<img style='CURSOR: hand' onclick=\"javascript:event.cancelBubble=true;showCalendar('DateCtrl"
        + DateCtrlID + "',false,'DateCtrl"
        + DateCtrlID + "','','')\" src='../../images/date.gif' align='absMiddle'>&nbsp; ";
                
        $(this).html(html);        
    });
    $("span.spntel").each(function()
    {                 
        var html = "<input type='text' style='width:50px'/> <input type='text' style='width:100px'/> ";
        $(this).html(html);        
    });
}

