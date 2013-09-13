<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BundlingTest.Default" %>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <title>Test</title>

	<%: Styles.Render("~/bundles/css") %>

    <Bundling:StyleBundle ID="StyleBundle1" runat="server" Path="~/bundles/css" />
	
    <Bundling:ScriptBundle ID="ScriptBundle1" runat="server" Path="~/bundles/modernizr" />

	<%: Bundling.Extensions.BundlingHelper.RenderStyles("~/bundles/css") %>

    <link href="<%: Bundling.Extensions.BundlingHelper.RenderStylesUrl("~/bundles/css") %>" rel="stylesheet" />

</head>
<body>
	

    <div class="container">

      <h1>Bootstrap starter template</h1>
      <p>Use this document as a way to quick start any new project.<br> All you get is this message and a barebones HTML document.</p>

    </div>
    
	<Bundling:ScriptBundle ID="ScriptBundle2" runat="server" Path="~/bundles/js" />
</body>
</html>
