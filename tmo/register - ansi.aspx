<%@ import namespace="xbase" %>
<%
   dim accNo as string
   dim ret as string
   dim u
   accNo=Request.Form("AccreditCode") 
   ret=""

    if not IsNothing(accNo) and accNo<>"" then
         u=Register.setAccreditString(accNo)
         if  not IsNothing(u) then
             ret="ע��ɹ�,"+"��Ȩ�û���"+u.userName
         end if
    end if

%>
<html>
<head>
    <title>�ޱ����ĵ�</title>
    <meta content="text/html; charset=utf-8" http-equiv="Content-Type">
    <link rel="stylesheet" type="text/css" href="css/style.css">
    <script src="xbase.js/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="xbase.js/jquery.xbase.js" type="text/javascript"></script>
    <script type="text/javascript">
    </script>
    <style type="text/css">
        .style1
        {
            width: 511px;
        }
        #AccreditCode
        {
            width: 508px;
        }
        .style2
        {
            height: 88px;
        }
        .style3
        {
            width: 511px;
            height: 88px;
        }
    </style>
</head>
<body>
    <form action="register.aspx" method="post" name="form1">
    <table border="0" cellspacing="5" cellpadding="5" style="width: 704px">
        <tr>
            <td align="left" valign="top" class="style2">
                ע���룺
            </td>
            <td class="style3">
                <textarea rows="5" style="width: 508px"><%=Register.getRegisterString()%></textarea>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                ��Ȩ�룺
            </td>
            <td class="style1">
                <textarea name="AccreditCode" id="AccreditCode" rows="5"><%=accNo%></textarea>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right" valign="top">
                <input type="submit" name="Submit" id="Submit" value="�ύ">
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                ��ʾ
            </td>
            <td class="style1">
                <%=ret %>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
