#pragma checksum "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\Widgets\_Status.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "abda0583f4b953ea6e5b870f4292c266c057c314"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Widgets__Status), @"mvc.1.0.view", @"/Views/Widgets/_Status.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\_ViewImports.cshtml"
using NetCoreMVC;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\_ViewImports.cshtml"
using VnCompany.WebApp.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\Widgets\_Status.cshtml"
using VnCompany.WebApp.Resources;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"abda0583f4b953ea6e5b870f4292c266c057c314", @"/Views/Widgets/_Status.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"99313092a47a5e84e125cd027ab0be8096437f60", @"/Views/_ViewImports.cshtml")]
    public class Views_Widgets__Status : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<int>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 4 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\Widgets\_Status.cshtml"
 if (Model == 1)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <span class=\"m-badge m-badge--wide m-badge--success\"><i class=\"fa fa-check\"></i> ");
#nullable restore
#line 6 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\Widgets\_Status.cshtml"
                                                                                Write(ManagerResource.LB_ACTIVATED);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n");
#nullable restore
#line 7 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\Widgets\_Status.cshtml"
}
else if (Model == 0)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <span class=\"m-badge m-badge--wide m-badge--danger\"><i class=\"fa fa-lock\"></i> ");
#nullable restore
#line 10 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\Widgets\_Status.cshtml"
                                                                              Write(ManagerResource.LB_LOCKED);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n");
#nullable restore
#line 11 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\Widgets\_Status.cshtml"
}

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<int> Html { get; private set; }
    }
}
#pragma warning restore 1591
