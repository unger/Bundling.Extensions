<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DemoStandardHelperClass.aspx.cs" Inherits="BundlingTest.DemoStandardHelperClass" %>
<%@ Register Src="~/Units/BootstrapDemo.ascx" TagPrefix="uc1" TagName="BootstrapDemo" %>
<!DOCTYPE html>
<html lang="en">
<head>
    <title>Standard Helper Class Demo</title>

	<%: Styles.Render("~/bundles/css") %>

	<%: Scripts.Render("~/bundles/modernizr") %>
</head>
<body>
    <uc1:BootstrapDemo runat="server" id="BootstrapDemo" />
    
	<%: Scripts.Render("~/bundles/js") %>
</body>
</html>