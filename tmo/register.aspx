<%@ Import Namespace="xbase" %>
<%
    Dim accNo As String
    Dim ret As String
    Dim u
    accNo = Request.Form("AccreditCode")
    ret = ""

    If Not IsNothing(accNo) And accNo <> "" Then
        u = Register.setAccreditString(accNo)
        If Not IsNothing(u) Then
            ret = "注册成功," + "授权用户：" + u.userName
        End If
    End If

%>
<html>
<head>
    <title>无标题文档</title>
    <meta content="text/html; charset=utf-8" http-equiv="Content-Type">
    <link rel="stylesheet" type="text/css" href="css/style.css">
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
                注册码：
            </td>
            <td class="style3">
                <textarea rows="5" style="width: 508px"><%=Register.getRegisterString()%></textarea>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                授权码：
            </td>
            <td class="style1">
                <textarea name="AccreditCode" id="AccreditCode" rows="5"><%=accNo%></textarea>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right" valign="top">
                <input type="submit" name="Submit" id="Submit" value="提交">
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                提示
            </td>
            <td class="style1">
                <%=ret %>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
