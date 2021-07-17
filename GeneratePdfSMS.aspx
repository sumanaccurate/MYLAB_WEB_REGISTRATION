<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GeneratePdfSMS.aspx.cs" Inherits="GeneratePdfSMS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="div" runat="server" >
        <asp:Label ID="lblPatientId" runat="server" Text="" Visible="false"></asp:Label>
        <asp:Label ID="lblType" runat="server" Text="" Visible="false"></asp:Label>
        <asp:Label ID="lblRandomKey" runat="server" Text="" Visible="false"></asp:Label>
         <asp:Label ID="lblAPIKey" runat="server" Text="" Visible="false"></asp:Label>
         <asp:Label ID="lblKey" runat="server" Text="" Visible="false"></asp:Label>
         <asp:Label ID="ltEmbed" runat="server" Text="" Visible="false"></asp:Label>
    </div>
    <div id="divNotFound" runat="server" visible="false">
          <h1>File Not Found..</h1>
    </div>
     <div id="div1" runat="server" visible="false">
            <h1>Thank You</h1>
     </div>
    </form>
</body>
</html>
