#pragma checksum "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\Function\_DeleteFunctionInfo.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2745d61e562c851eb39ecbc164535f7def363b60"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Function__DeleteFunctionInfo), @"mvc.1.0.view", @"/Views/Function/_DeleteFunctionInfo.cshtml")]
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
#line 2 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\Function\_DeleteFunctionInfo.cshtml"
using VnCompany.WebApp.Resources;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2745d61e562c851eb39ecbc164535f7def363b60", @"/Views/Function/_DeleteFunctionInfo.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"99313092a47a5e84e125cd027ab0be8096437f60", @"/Views/_ViewImports.cshtml")]
    public class Views_Function__DeleteFunctionInfo : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<VnCompany.WebApp.Models.FunctionViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<div class=\"modal-header\">\r\n    <h5 class=\"modal-title\"");
            BeginWriteAttribute("id", " id=\"", 142, "\"", 147, 0);
            EndWriteAttribute();
            WriteLiteral(">\r\n        ");
#nullable restore
#line 6 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\Function\_DeleteFunctionInfo.cshtml"
   Write(ManagerResource.CONFIRM_DELETE);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    </h5>\r\n    <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\">\r\n        <span aria-hidden=\"true\">\r\n            &times;\r\n        </span>\r\n    </button>\r\n</div>\r\n");
#nullable restore
#line 14 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\Function\_DeleteFunctionInfo.cshtml"
 using (Html.BeginForm())
{
    

#line default
#line hidden
#nullable disable
#nullable restore
#line 16 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\Function\_DeleteFunctionInfo.cshtml"
Write(Html.AntiForgeryToken());

#line default
#line hidden
#nullable disable
#nullable restore
#line 17 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\Function\_DeleteFunctionInfo.cshtml"
Write(Html.HiddenFor(m => m.Id));

#line default
#line hidden
#nullable disable
            WriteLiteral("    <div class=\"modal-footer\">\r\n        <button class=\"btn\" data-dismiss=\"modal\">No</button>\r\n        <input class=\"btn btn-danger\" type=\"submit\" value=\"Yes\" />\r\n    </div>\r\n");
#nullable restore
#line 23 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\Function\_DeleteFunctionInfo.cshtml"
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<VnCompany.WebApp.Models.FunctionViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
