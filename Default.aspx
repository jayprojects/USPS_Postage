<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        Service:
        <asp:DropDownList ID="DropDownListService" runat="server" AutoPostBack="True" 
            onselectedindexchanged="DropDownListService_SelectedIndexChanged">
            <asp:ListItem>FIRST CLASS</asp:ListItem>
            <asp:ListItem>PRIORITY</asp:ListItem>
        </asp:DropDownList>
        <br />
        From Zipcode:&nbsp; 
        <asp:TextBox ID="TextBoxZipForm" runat="server" Width="60px"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; To Zipcode:
        <asp:TextBox ID="TextBoxZipTo" runat="server" Width="58px"></asp:TextBox>
        <br />
        Type:
        <asp:DropDownList ID="DropDownListPackageType" runat="server" AutoPostBack="True" 
            onselectedindexchanged="DropDownListShape_SelectedIndexChanged">
            <asp:ListItem>LETTER</asp:ListItem>
            <asp:ListItem>PARCEL</asp:ListItem>
        </asp:DropDownList>
        &nbsp;<asp:Panel ID="PanelDimension" runat="server" Visible="False">
            Width:
            <asp:TextBox ID="TextBoxWidth" runat="server" Width="40px"></asp:TextBox>
            &nbsp;Lenght:
            <asp:TextBox ID="TextBoxLenght" runat="server" Width="55px"></asp:TextBox>
            &nbsp;Height:
            <asp:TextBox ID="TextBoxHeight" runat="server" Width="49px"></asp:TextBox>
        </asp:Panel>
        Pounds:
        <asp:TextBox ID="TextBoxPounds" runat="server" Width="47px"></asp:TextBox>
&nbsp;Ounces:
        <asp:TextBox ID="TextBoxOunces" runat="server" Width="47px"></asp:TextBox>
        <br />
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
            Text="Ger Rate" />
        <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="Button" />
    
    </div>
    </form>
</body>
</html>
