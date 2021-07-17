<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QRReport.aspx.cs" Inherits="QRReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<style>
    .lbl
    {
        font-size: medium;
        font-family:Arial Black;
        color: Red;
    }
</style>
<body>
    <form id="form1" runat="server">
    <div id="div" runat="server" runat="server" visible="false">
        <asp:Label ID="lb" runat="server" CssClass="lbl" Text="My Lab Next Gen" Height="10px"
            Visible="true"></asp:Label><br />
        <br />
        <br />
        <asp:LinkButton ID="BtnLink" runat="server" Text="Download" OnClick="Btn_Click" Visible="false"></asp:LinkButton><br />
        <br />
        <br />
        <asp:Label ID="lblPatientId" runat="server" Text="" Visible="false"></asp:Label>
        <asp:Label ID="lblType" runat="server" Text="" Visible="false"></asp:Label>
        <asp:Label ID="lblRandomKey" runat="server" Text="" Visible="false"></asp:Label>
        <asp:Label ID="lblAPIKey" runat="server" Text="" Visible="false"></asp:Label>
        <asp:Label ID="lblKey" runat="server" Text="" Visible="false"></asp:Label>
        <asp:Label ID="ltEmbed" runat="server" Text="" Visible="false"></asp:Label>
    </div>
    <div id="div1" runat="server" visible="false">
        <h1>
            Thank You</h1>
    </div>
    <div id="divNotFound" runat="server" visible="false">"
        <h1 style="text-align:center">
            Requested reports did not found on this place
        </h1>
        <br />
        <h3 style="text-align:center">
            It may have move to archive directory or deleted by
            <asp:Label ID="lbllab" CssClass="lbl" runat="server"></asp:Label>
            <br />
            for More Information about this document you may contact :
            <asp:Label ID="lbllab1" CssClass="lbl" runat="server"></asp:Label>
            <br />
            Contact Person :
            <asp:Label ID="lblcontact" CssClass="lbl" runat="server"></asp:Label>
            <br />
            Mobile No :
            <asp:Label ID="lblmobile" CssClass="lbl" runat="server"></asp:Label>
            <br />
            You can write a query to :
            <asp:Label ID="lblemail" CssClass="lbl" runat="server"></asp:Label>
            <br />
        </h3>
    </div>
    
    </form>
</body>
</html>
