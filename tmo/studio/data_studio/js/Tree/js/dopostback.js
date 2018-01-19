//翻页用	
function __doPostback(ButtonID, NewPage) {
	Form1.NewPageNo.value = NewPage;
	__doPostBack(ButtonID,'')
}

//排序用
function __doPostbackSort(ButtonID, SortName, Sort) {
	Form1.NewSortName.value = SortName;
	Form1.NewSort.value = Sort;
	__doPostBack(ButtonID,'')
}

//显示分页用
function PageList(PageNo, AllPage)
{
	var i;
	if (PageNo==1)		//如果是第一页，则这两个不能点。
		document.write("首页&nbsp;上页&nbsp;");
	else
	{
		document.write("<a href=\"javascript:__doPostback('','1')\">首页</a>&nbsp;");
		document.write("<a href=\"javascript:__doPostback('','" + (PageNo-1) + "')\">上页</a>&nbsp;");
	}
	
	
	for (i=0; i<AllPage; i++)
	{
		if (i>0 && i % 20==0)
			document.write("<br>");
		document.write("&nbsp;");
		if (PageNo!=(i+1))
			document.write("<A href=\"javascript:__doPostback('','" + (i+1) + "')\">");
		document.write(i+1);
		if (PageNo!=(i+1))
			document.write("</a>");
	}
	if (PageNo >= AllPage )	//最后一页则下页和末页不能点
		document.write("&nbsp;&nbsp;下页&nbsp末页&nbsp;;");
	else
	{
		document.write("&nbsp;&nbsp;<a href=\"javascript:__doPostback('','" + (PageNo+1) + "')\">下页</a>&nbsp;");
		document.write("<a href=\"javascript:__doPostback('','" + AllPage + "')\">末页</a>&nbsp;");
	}
	document.write("&nbsp; 第<font color=#ff6600>" + PageNo + "</font>页/共" + AllPage + "页");
}

//用来显示列表的头部
function CreateTitle(NewSort, FieldName, TitleName)
{
	//alert(SortName);
	document.write("<td class=search_td_2>");
	document.write("<a class=search_title href=\"javascript:__doPostbackSort('', '" + FieldName + "', '" + ((NewSort=="Desc")?"":"Desc") + "')\">");
	document.write(TitleName);
	try
	{
		if (SortName!=null && SortName==FieldName)
		document.write("<IMG  height=7 src='../../images/" + ((NewSort=="Desc")?"arrowhead_bottom.gif":"arrowhead_top.gif") + "' width=8 align=absMiddle border=0>");
	}
	catch(e)
	{}
	document.write("</a>");
	document.write("</td>");
}

//显示票的状态
function DocTicketStat(Ticketstat)
{
	switch (Ticketstat)
	{
		case "0":	return "未出票";
		case "1":	return "正常出票";
		case "2":	return "退票";
		case "3":	return "废票";
		case "4":	return "取消";
	}
}

//Pnr退票状态
function DocPnrStat(PnrStat)
{
	switch (PnrStat)
	{
		case "0":	return "整体";
		case "1":	return "部分";
	}
	return "";
}

//执行状态
function DocTicketActionStat(TicketAction)
{
	switch (TicketAction)
	{
		case "0":	return "完成";
		case "1":	return "未完成";
	}
	return "";
}