<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DemoWebControl.aspx.cs" Inherits="BundlingTest.DemoWebControl" %>
<%@ Register Src="~/Units/BootstrapDemo.ascx" TagPrefix="uc1" TagName="BootstrapDemo" %>
<!DOCTYPE html>
<html lang="en">
<head>
    <title>Web Control Demo</title>

    <Bundling:StyleBundle ID="StyleBundle1" runat="server" Path="~/bundles/css" />

    <Bundling:ScriptBundle ID="ScriptBundle1" runat="server" Path="~/bundles/modernizr" />
</head>
<body>
    <uc1:BootstrapDemo runat="server" id="BootstrapDemo" />
    
	<Bundling:ScriptBundle ID="ScriptBundle2" runat="server" Path="~/bundles/js" />
</body>
</html>
