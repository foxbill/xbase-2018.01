function CheckForm(obj)
{
	var CheckSucess = true;
	for(i=0;i<obj.length && CheckSucess;i++)
	{
		if (obj[i].IsNum=="" && !isFinite(obj[i].value))
		{
			CheckSucess = false;
			alert("必须为数字,请检查!")
		}

		if (obj[i].NotEmpty=="" && obj[i].value.length<1)
		{
			CheckSucess = false;
			alert("该栏不能为空,请检查");
		}
			
		//检测数字范围（最大、最小）
		//检测身份证（正则表达史）
		//检测电话号码

		if (!CheckSucess)
		{
			obj[i].select();
			obj[i].focus();
		}
	}
	return CheckSucess 
}
