<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BundlingTest.Default" %>
<%@ Register Src="~/Units/BootstrapDemo.ascx" TagPrefix="uc1" TagName="BootstrapDemo" %>
<!DOCTYPE html>
<html>
<head>
    <title>No Bundling Demo</title>

    <link href="/Content/bootstrap/bootstrap.less" rel="stylesheet" />
    <link href="/Content/bootstrap/theme.less" rel="stylesheet" />
    <link href="/Content/main.less" rel="stylesheet" />

    <script src="/Scripts/modernizr-2.8.3.js"></script>
</head>
<body>
    <uc1:BootstrapDemo runat="server" id="BootstrapDemo" />
    
    <script src="/Scripts/jquery-2.1.1.js"></script>
    <script src="/Scripts/bootstrap.js"></script>
    <script src="/Scripts/holder.js"></script>
</body>
</html>
