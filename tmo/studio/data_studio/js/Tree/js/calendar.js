if (!document.getElementById("CalFrame"))
{
	document.write('<iframe id=CalFrame name=CalFrame frameborder=0 src=../../xbase.js/calendar.htm style=display:none;position:absolute;z-index:100></iframe>');
	document.onclick=hideCalendar;
}
function showCalendar(sInput,sFont,sInputName,sNextP,sNextD,sStartD,sEndD,sVD,sOE,sVDE,sOT,s3F,sStartDate,sCallback)
{
	//1.sInput��������λ�õĿؼ�������,
	//2.sFont������ɫ��־,
	//3.sInputNameȡ�����ڵĿؼ�����,
	
	
	//4.sNextPѡȡ���ں�������������λ�õĿؼ�����,�ɲ�����,
	//5.sNextDѡȡ���ں���������������ȡֵ�Ŀؼ�����,�ɲ�����,
	
	//6.sStartD��ʼ��Чʱ��,
	//7.sEndD������Чʱ��,
	
	//8.sVD�ܼ���Ч,���磺'1,3,5',ע��������Ӧ����'0'
	//9.sOE,��˫����Ч,'0'��ʾ˫��,'1'��ʾ����
	//10.sVDE��������,����'2004-10-11,2004-11-20,',���һ��Ҫ����','
	//11.sOT�����ֱ�����ı����е��'text',
	//12.s3F��һ������������ѡ����ȡ���ں�λ���µĿؼ�
	
	//sStartDate,��������Ĭ������
	//sCallback
	var oInputname,oStartdate;
	var CFrame=document.getElementById("CalFrame");
	var WCFrame=window.frames.CalFrame;
	var oInput=document.getElementById(sInput);
	if(!oInput){alert("���ƶ��󲻴��ڣ�");return;}
	if(!sInputName){alert("����ؼ�δָ����");return;}
	oInputname=document.getElementById(sInputName);
	if(!oInputname){alert("����ؼ������ڣ�");return;}
	if(oInputname.tagName!="INPUT"||oInputname.type!="text"){alert("����ؼ����ʹ���");return;}
	if(sStartDate)
	{
		oStartdate=document.getElementById(sStartDate);
		if(!oStartdate){alert("�ο��ؼ������ڣ�");return;}
		if(oStartdate.tagName!="INPUT"||(oStartdate.type!="text"&&oStartdate.type!="hidden")){alert("�ο��ؼ����ʹ���");return;}
	}
	if(!WCFrame.bCalLoaded){alert("����δ�ɹ�װ�أ���ˢ��ҳ�棡");return;}
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