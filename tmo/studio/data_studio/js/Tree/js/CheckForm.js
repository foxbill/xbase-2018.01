function CheckForm(obj)
{
	var CheckSucess = true;
	for(i=0;i<obj.length && CheckSucess;i++)
	{
		if (obj[i].IsNum=="" && !isFinite(obj[i].value))
		{
			CheckSucess = false;
			alert("����Ϊ����,����!")
		}

		if (obj[i].NotEmpty=="" && obj[i].value.length<1)
		{
			CheckSucess = false;
			alert("��������Ϊ��,����");
		}
			
		//������ַ�Χ�������С��
		//������֤��������ʷ��
		//���绰����

		if (!CheckSucess)
		{
			obj[i].select();
			obj[i].focus();
		}
	}
	return CheckSucess 
}
