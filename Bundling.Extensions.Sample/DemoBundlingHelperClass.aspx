<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DemoBundlingHelperClass.aspx.cs" Inherits="BundlingTest.DemoBundlingHelperClass" %>
<%@ Register Src="~/Units/BootstrapDemo.ascx" TagPrefix="uc1" TagName="BootstrapDemo" %>
<!DOCTYPE html>
<html>
<head>
    <title>Bundling Helper Class Demo</title>

	<%: Bundling.Extensions.BundlingHelper.RenderStyles("~/bundles/css") %>

	<%: Bundling.Extensions.BundlingHelper.RenderScript("~/bundles/modernizr") %>
</head>
<body>
    <uc1:BootstrapDemo runat="server" id="BootstrapDemo" />
    
	<%: Bundling.Extensions.BundlingHelper.RenderScript("~/bundles/js") %>
</body>
</html>
