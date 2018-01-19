if (!document.getElementById("CalFrame"))
{
	document.write('<iframe id=CalFrame name=CalFrame frameborder=0 src=../../xbase.js/calendar.htm style=display:none;position:absolute;z-index:100></iframe>');
	document.onclick=hideCalendar;
}
function showCalendar(sInput,sFont,sInputName,sNextP,sNextD,sStartD,sEndD,sVD,sOE,sVDE,sOT,s3F,sStartDate,sCallback)
{
	//1.sInput日历弹出位置的控件的名称,
	//2.sFont字体颜色标志,
	//3.sInputName取得日期的控件名称,
	
	
	//4.sNextP选取日期后新日历弹出的位置的控件名称,可不输入,
	//5.sNextD选取日期后新日历弹出从中取值的控件名称,可不输入,
	
	//6.sStartD开始有效时间,
	//7.sEndD截至有效时间,
	
	//8.sVD周几有效,例如：'1,3,5',注意星期日应输入'0'
	//9.sOE,单双日有效,'0'表示双日,'1'表示单日
	//10.sVDE特殊日期,例如'2004-10-11,2004-11-20,',最后一定要输入','
	//11.sOT如果是直接在文本框中点击'text',
	//12.s3F下一个日历弹出并选择后点取日期后定位到新的控件
	
	//sStartDate,弹出日历默认日期
	//sCallback
	var oInputname,oStartdate;
	var CFrame=document.getElementById("CalFrame");
	var WCFrame=window.frames.CalFrame;
	var oInput=document.getElementById(sInput);
	if(!oInput){alert("控制对象不存在！");return;}
	if(!sInputName){alert("输入控件未指定！");return;}
	oInputname=document.getElementById(sInputName);
	if(!oInputname){alert("输入控件不存在！");return;}
	if(oInputname.tagName!="INPUT"||oInputname.type!="text"){alert("输入控件类型错误！");return;}
	if(sStartDate)
	{
		oStartdate=document.getElementById(sStartDate);
		if(!oStartdate){alert("参考控件不存在！");return;}
		if(oStartdate.tagName!="INPUT"||(oStartdate.type!="text"&&oStartdate.type!="hidden")){alert("参考控件类型错误！");return;}
	}
	if(!WCFrame.bCalLoaded){alert("日历未成功装载！请刷新页面！");return;}
	WCFrame.n_position=sNextP;
	WCFrame.n_textdate=sNextD;
	WCFrame.startdate=sStartD;
	WCFrame.enddate=sEndD;
	WCFrame.vailidday=sVD;
	WCFrame.oddeven=sOE;
	WCFrame.vailiddate=sVDE;
	WCFrame.objecttype=sOT;
	WCFrame.thirdfocus=s3F;
	if(CFrame.style.display=="block"){CFrame.style.display="none";return;}
	
	var eT=0,eL=0,p=oInput;
	var sT=document.body.scrollTop,sL=document.body.scrollLeft;
	var eH=oInput.height,eW=oInput.width;
	while(p&&p.tagName!="BODY"){eT+=p.offsetTop;eL+=p.offsetLeft;p=p.offsetParent;}
	if(sOT=="text")
	{
		CFrame.style.top=+(document.body.clientHeight-(eT-sT)-eH>=CFrame.height)?eT+eH+20:eT-CFrame.height;		
	}
	else
	{
		CFrame.style.top=(document.body.clientHeight-(eT-sT)-eH>=CFrame.height)?eT+eH:eT-CFrame.height;		
	}
	CFrame.style.left=(document.body.clientWidth-(eL-sL)>=CFrame.width)?eL:eL+eW-CFrame.width;
	CFrame.style.display="block";
	
	WCFrame.oFont=sFont;
	WCFrame.oInputname=oInputname;
	WCFrame.oStartdate=oStartdate;
	WCFrame.callback=sCallback;
	WCFrame.initCalendar();
}
function hideCalendar()
{
	var CFrame=document.getElementById("CalFrame");
	CFrame.style.display="none";
}