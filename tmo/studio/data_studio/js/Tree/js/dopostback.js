//��ҳ��	
function __doPostback(ButtonID, NewPage) {
	Form1.NewPageNo.value = NewPage;
	__doPostBack(ButtonID,'')
}

//������
function __doPostbackSort(ButtonID, SortName, Sort) {
	Form1.NewSortName.value = SortName;
	Form1.NewSort.value = Sort;
	__doPostBack(ButtonID,'')
}

//��ʾ��ҳ��
function PageList(PageNo, AllPage)
{
	var i;
	if (PageNo==1)		//����ǵ�һҳ�������������ܵ㡣
		document.write("��ҳ&nbsp;��ҳ&nbsp;");
	else
	{
		document.write("<a href=\"javascript:__doPostback('','1')\">��ҳ</a>&nbsp;");
		document.write("<a href=\"javascript:__doPostback('','" + (PageNo-1) + "')\">��ҳ</a>&nbsp;");
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
	if (PageNo >= AllPage )	//���һҳ����ҳ��ĩҳ���ܵ�
		document.write("&nbsp;&nbsp;��ҳ&nbspĩҳ&nbsp;;");
	else
	{
		document.write("&nbsp;&nbsp;<a href=\"javascript:__doPostback('','" + (PageNo+1) + "')\">��ҳ</a>&nbsp;");
		document.write("<a href=\"javascript:__doPostback('','" + AllPage + "')\">ĩҳ</a>&nbsp;");
	}
	document.write("&nbsp; ��<font color=#ff6600>" + PageNo + "</font>ҳ/��" + AllPage + "ҳ");
}

//������ʾ�б��ͷ��
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

//��ʾƱ��״̬
function DocTicketStat(Ticketstat)
{
	switch (Ticketstat)
	{
		case "0":	return "δ��Ʊ";
		case "1":	return "������Ʊ";
		case "2":	return "��Ʊ";
		case "3":	return "��Ʊ";
		case "4":	return "ȡ��";
	}
}

//Pnr��Ʊ״̬
function DocPnrStat(PnrStat)
{
	switch (PnrStat)
	{
		case "0":	return "����";
		case "1":	return "����";
	}
	return "";
}

//ִ��״̬
function DocTicketActionStat(TicketAction)
{
	switch (TicketAction)
	{
		case "0":	return "���";
		case "1":	return "δ���";
	}
	return "";
}