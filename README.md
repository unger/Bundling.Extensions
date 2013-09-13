Bundling.Extensions
===================

Extensions for Microsoft ASP.NET Web Optimization Framework (System.Web.Optimization)

##Installation

http://nuget.org/packages/Bundling.Extensions/

    Install-Package Bundling.Extensions

##Usage

Register bundles as usual

    var cssBundle = new Bundle("~/bundles/css");
    cssBundle.Include("~/css/styles1.css");
    cssBundle.Include("~/css/styles2.css");
    // Add transforms here
    BundleTable.Bundles.Add(cssBundle);

#Urls without querystring

To override bundle url generation call this after you added your bundles to the BundleTable.Bundles

    RouteTable.Routes.AddBundleRoutes();

To output the custom urls use the following methods

    <%: BundlingHelper.RenderStyles("~/bundles/css") %>
    <%: BundlingHelper.RenderScripts("~/bundles/js") %>

This will generate urls with a timestamp in the bundle url instead like this

    <link href="/bundles/css/20130315191550" rel="stylesheet"/>

and in debug mode (BundleTable.EnableOptimizations = false)

    <link href="/bundles/css/20130315191550/css/styles1.css" rel="stylesheet"/>
    <link href="/bundles/css/20130315191550/css/styles2.css" rel="stylesheet"/>

the timestamp is for the last edited file of the included files

NOTE: Transformation is also applied in debug mode

#ASP.NET Web Forms controls

It is also possible to use the following controls

    <Bundling:StyleBundle runat="server" Path="~/bundles/css" />
    <Bundling:ScriptBundle runat="server" Path="~/bundles/js" />

Also add this to Web.Config

    <system.web>
    <pages>
      <controls>
        <add assembly="Bundling.Extensions" namespace="Bundling.Extensions.Controls" tagPrefix="Bundling"/>
      </controls>
    </pages>
    </system.web>

#Use HttpHandler to rewrite bundle urls

If you still like to use the standard way to render the bundles, it is possible to use the included HttpHandler that rewrites the output with a Response.Filter

Example:

    <%: Styles.Render("~/bundles/css") %>

Will generate

    <link href="/bundles/css?v=_NNIf4XxdPCITzjlKPMgZwHMSUsPyxxGaNCIe6mgAkg1" rel="stylesheet"/>

Register the BundleRewriteModule in Web.Config

    <system.webServer>
        <modules runAllManagedModulesForAllRequests="true">
	    <add name="BundleRewriteModule" type="Bundling.Extensions.BundleRewriteModule" preCondition=""/>
	</modules>
    </system.webServer>
    
After this the generated link will become

    <link href="/bundles/css/_NNIf4XxdPCITzjlKPMgZwHMSUsPyxxGaNCIe6mgAkg1test" rel="stylesheet"/>

NOTE: You still have to call RouteTable.Routes.AddBundleRoutes() otherwise you will get a 404 error

##License

    Copyright (C) 2013 Magnus Unger
    
    Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
    documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
    the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
    and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
    The above copyright notice and this permission notice shall be included in all copies or substantial portions 
    of the Software.
    
    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
    CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS 
    IN THE SOFTWARE.
