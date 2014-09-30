<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DemoBundlingHelperClassUrl.aspx.cs" Inherits="BundlingTest.DemoBundlingHelperClassUrl" %>
<%@ Register Src="~/Units/BootstrapDemo.ascx" TagPrefix="uc1" TagName="BootstrapDemo" %>
<!DOCTYPE html>
<html>
<head>
    <title>Bundling Helper Class Url Demo</title>

    <link href="<%: Bundling.Extensions.BundlingHelper.RenderStylesUrl("~/bundles/css") %>" rel="stylesheet" />

    <script src="<%: Bundling.Extensions.BundlingHelper.RenderScriptUrl("~/bundles/modernizr") %>"></script>
</head>
<body>
    <uc1:BootstrapDemo runat="server" id="BootstrapDemo" />
    
    <script src="<%: Bundling.Extensions.BundlingHelper.RenderScriptUrl("~/bundles/js") %>"></script>
</body>
</html>
