<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Default2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>Sample PDF file from google</div>
    <div>
    <iframe src="http://103.131.93.13:99/src/Report.pdf"
  style="width:718px; height:700px;" frameborder="0"></iframe>

  <object data="data:application/pdf;base64, JVBERi0xLjQKJeLjz9MKMyA..." type="application/pdf" width="160px">
    <embed src="data:application/pdf;base64, JVBERi0xLjQKJeLjz9MKMyA..." type="application/pdf" />
</object>
    </div>
    </form>
</body>
</html>
