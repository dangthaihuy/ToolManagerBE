#pragma checksum "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\WorkFlow\Partials\_FormContent.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e35c289be25ad1f7ced5d8ee8d955f791a6e7220"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_WorkFlow_Partials__FormContent), @"mvc.1.0.view", @"/Views/WorkFlow/Partials/_FormContent.cshtml")]
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
#line 1 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\WorkFlow\Partials\_FormContent.cshtml"
using VnCompany.WebApp.Helpers;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\WorkFlow\Partials\_FormContent.cshtml"
using VnCompany.WebApp.Resources;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\WorkFlow\Partials\_FormContent.cshtml"
using Newtonsoft.Json;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e35c289be25ad1f7ced5d8ee8d955f791a6e7220", @"/Views/WorkFlow/Partials/_FormContent.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"99313092a47a5e84e125cd027ab0be8096437f60", @"/Views/_ViewImports.cshtml")]
    public class Views_WorkFlow_Partials__FormContent : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<VnCompany.WebApp.Models.FormViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"<div id=""modalSearchFormContent"">
    <div class=""m-portlet"">
        <div class=""m-portlet__body"" style=""max-height: 50vh; overflow-y: scroll"">
            <div class=""m-section"">
                <div class=""m-section__content table-responsive"">
                    <table id=""FormSearchResults"" class=""table table-bordered"">
                        <thead>
                            <tr>
                                <th class=""text-center"" width=""3%""></th>
                                <th class=""text-center"" width=""50"">#</th>
                                <th class=""text-center"">");
#nullable restore
#line 15 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\WorkFlow\Partials\_FormContent.cshtml"
                                                   Write(ManagerResource.LB_CODE);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                                <th class=\"text-center\">");
#nullable restore
#line 16 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\WorkFlow\Partials\_FormContent.cshtml"
                                                   Write(ManagerResource.LB_NAME);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                                <th></th>\r\n                            </tr>\r\n                        </thead>\r\n                        <tbody>\r\n");
#nullable restore
#line 21 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\WorkFlow\Partials\_FormContent.cshtml"
                             if (Model.SearchResult != null && Model.SearchResult.Count > 0)
                            {
                                var stt = 0;
                                foreach (var record in Model.SearchResult)
                                {
                                    stt++;

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                                    <tr>
                                        <td class=""text-center"">
                                            <label class=""m-checkbox m-checkbox--brand"" style=""transform: translate(5px,-5px);"">
                                                <input class=""search-form-item-cbx new-item hidden"" type=""checkbox"" name=""SearchFormsSelected[]""");
            BeginWriteAttribute("value", " value=\"", 1714, "\"", 1732, 1);
#nullable restore
#line 30 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\WorkFlow\Partials\_FormContent.cshtml"
WriteAttributeValue("", 1722, record.Id, 1722, 10, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" data-id=\"");
#nullable restore
#line 30 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\WorkFlow\Partials\_FormContent.cshtml"
                                                                                                                                                                        Write(record.Id);

#line default
#line hidden
#nullable disable
            WriteLiteral("\"\r\n                                               data-info=\"");
#nullable restore
#line 31 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\WorkFlow\Partials\_FormContent.cshtml"
                                                      Write(JsonConvert.SerializeObject(record));

#line default
#line hidden
#nullable disable
            WriteLiteral("\" />\r\n                                                <span></span>\r\n                                            </label>\r\n\r\n                                        </td>\r\n                                        <td class=\"text-center\">");
#nullable restore
#line 36 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\WorkFlow\Partials\_FormContent.cshtml"
                                                            Write(stt + (Model.CurrentPage - 1) * Model.PageSize);

#line default
#line hidden
#nullable disable
            WriteLiteral(" </td>\r\n                                        <td>");
#nullable restore
#line 37 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\WorkFlow\Partials\_FormContent.cshtml"
                                       Write(record.Code);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>\r\n                                            ");
#nullable restore
#line 39 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\WorkFlow\Partials\_FormContent.cshtml"
                                       Write(record.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                                        </td>
                                        <td class=""text-center"">
                                            <a href=""javascript:;"" class=""text-info search-form-item-view new-item"" data-detail=""/WorkFlow/SearchFormViewDetail/");
#nullable restore
#line 42 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\WorkFlow\Partials\_FormContent.cshtml"
                                                                                                                                                           Write(record.Id);

#line default
#line hidden
#nullable disable
            WriteLiteral("\"><i class=\"fa fa-eye\"></i></a>\r\n                                        </td>\r\n                                    </tr>\r\n");
#nullable restore
#line 45 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\WorkFlow\Partials\_FormContent.cshtml"
                                }
                            }
                            else
                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                <tr>\r\n                                    <td colspan=\"11\">\r\n                                        ");
#nullable restore
#line 51 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\WorkFlow\Partials\_FormContent.cshtml"
                                   Write(ManagerResource.LB_NO_RECORD);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                    </td>\r\n                                </tr>\r\n");
#nullable restore
#line 54 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\WorkFlow\Partials\_FormContent.cshtml"
                            }

#line default
#line hidden
#nullable disable
            WriteLiteral("                        </tbody>\r\n                    </table>\r\n\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n");
#nullable restore
#line 62 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\WorkFlow\Partials\_FormContent.cshtml"
      
        await RenderPaging();
    

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n\r\n");
        }
        #pragma warning restore 1998
#nullable restore
#line 67 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\WorkFlow\Partials\_FormContent.cshtml"
            
    private async Task RenderPaging()
    {

#line default
#line hidden
#nullable disable
        WriteLiteral("        <div class=\"m_datatable m-datatable m-datatable--default m-datatable--brand m-datatable--loaded\">\r\n            <div class=\"m-datatable__pager m-datatable--paging-loaded clearfix\">\r\n");
#nullable restore
#line 72 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\WorkFlow\Partials\_FormContent.cshtml"
                 if (Model.WasOverRecordsInPage())
                {
                    

#line default
#line hidden
#nullable disable
#nullable restore
#line 74 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\WorkFlow\Partials\_FormContent.cshtml"
               Write(HtmlHelpers.BootstrapPager(Model.CurrentPage, index => "javascript:;", Model.TotalCount, pageSize: Model.PageSize, numberOfLinks: 10, onclickEvent: "FormSearchTool.Search"));

#line default
#line hidden
#nullable disable
#nullable restore
#line 74 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\WorkFlow\Partials\_FormContent.cshtml"
                                                                                                                                                                                                 
                }

#line default
#line hidden
#nullable disable
        WriteLiteral("\r\n");
#nullable restore
#line 77 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\WorkFlow\Partials\_FormContent.cshtml"
                 if (Model.TotalCount > 0)
                {

#line default
#line hidden
#nullable disable
        WriteLiteral("                    <div class=\"m-datatable__pager-info\">\r\n                        <span class=\"m-datatable__pager-detail\">\r\n                            ");
#nullable restore
#line 81 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\WorkFlow\Partials\_FormContent.cshtml"
                       Write(ManagerResource.LB_RECORDS_ALL);

#line default
#line hidden
#nullable disable
        WriteLiteral(" ");
#nullable restore
#line 81 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\WorkFlow\Partials\_FormContent.cshtml"
                                                       Write(Model.TotalCount);

#line default
#line hidden
#nullable disable
        WriteLiteral(" ");
#nullable restore
#line 81 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\WorkFlow\Partials\_FormContent.cshtml"
                                                                         Write(ManagerResource.LB_RECORDS);

#line default
#line hidden
#nullable disable
        WriteLiteral(" ");
#nullable restore
#line 81 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\WorkFlow\Partials\_FormContent.cshtml"
                                                                                                     Write(Model.StartCount());

#line default
#line hidden
#nullable disable
        WriteLiteral(" 〜 ");
#nullable restore
#line 81 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\WorkFlow\Partials\_FormContent.cshtml"
                                                                                                                           Write(Model.EndCount(Model.SearchResult.Count()));

#line default
#line hidden
#nullable disable
        WriteLiteral("\r\n                        </span>\r\n                    </div>\r\n");
#nullable restore
#line 84 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\WorkFlow\Partials\_FormContent.cshtml"
                }

#line default
#line hidden
#nullable disable
        WriteLiteral("            </div>\r\n        </div>\r\n");
#nullable restore
#line 87 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\WorkFlow\Partials\_FormContent.cshtml"
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<VnCompany.WebApp.Models.FormViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
