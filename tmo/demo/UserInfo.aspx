<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserInfo.aspx.cs" Inherits="WxDemo.UserInfo"  %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
         <% WxDemo.Model.WxUserInfo user = (WxDemo.Model.WxUserInfo)Session["userInfo"]; %>
        <h2>微信用户信息：</h2>
        <label>昵称：</label><%= user.nickname %>
        <label>OpenID：</label><%= user.openid %>
        <label>性别：</label><%= user.sex==1?"男":"女" %>
        <label>城市：</label><%= user.nickname %>
        <label>省份：</label><%= user.province %>
        <label>国家：</label><%= user.country %>
        <label>头像：</label><%= user.nickname %>
    </div>
    </form>
</body>
</html>
