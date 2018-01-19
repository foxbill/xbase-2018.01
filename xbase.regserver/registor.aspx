<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="registor.aspx.cs" Inherits="xbase.regserver.registor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table style="width:100%;">
            <tr>
                <td>
                    用户名</td>
                <td>
                    <asp:TextBox ID="txtUserName" runat="server" Width="347px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    注册码</td>
                <td>
                    <asp:TextBox ID="txtRegCode" runat="server" Height="138px" TextMode="MultiLine" 
                        Width="346px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    授权码</td>
                <td>
                    <asp:TextBox ID="txtAccCode" runat="server" Height="119px" TextMode="MultiLine" 
                        Width="342px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    授权信息</td>
                <td>
                    <asp:Label ID="labMemo" runat="server" Text="      "></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnSubmit" runat="server" onclick="btnSubmit_Click" Text="提交" />
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
