#pragma checksum "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\MenuService\Edit.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "fa0ba4cafecb9a72a8fda3ab999c491a7a4e42ad"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_MenuService_Edit), @"mvc.1.0.view", @"/Views/MenuService/Edit.cshtml")]
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
#line 1 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\MenuService\Edit.cshtml"
using VnCompany.WebApp.Resources;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\MenuService\Edit.cshtml"
using Newtonsoft.Json;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fa0ba4cafecb9a72a8fda3ab999c491a7a4e42ad", @"/Views/MenuService/Edit.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"99313092a47a5e84e125cd027ab0be8096437f60", @"/Views/_ViewImports.cshtml")]
    public class Views_MenuService_Edit : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<VnCompany.WebApp.Models.MenuServiceCreateOrUpdateViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/Scripts/Workflow/search.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/Scripts/Workflow/main.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/Scripts/Workflow/update.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/Content/Plugins/formio/formio.full.min.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 5 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\MenuService\Edit.cshtml"
  
    ViewBag.Title = "Menu Service";

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\MenuService\Edit.cshtml"
Write(Html.Partial("_Notifications"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n");
#nullable restore
#line 10 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\MenuService\Edit.cshtml"
Write(Html.Partial("../Widgets/Modals/_DefaultModal"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"

<div class=""row pl30 pr30"">
    <div class=""col-lg-12"">
        <div class=""m-portlet"">
            <div class=""m-portlet__head"">
                <div class=""m-portlet__head-caption"">
                    <div class=""m-portlet__head-title"">
                        <span class=""m-portlet__head-icon m--hide"">
                            <i class=""la la-gear""></i>
                        </span>
                        <h3 class=""m-portlet__head-text"">
                            Chỉnh sửa Menu Service
                        </h3>
                    </div>
                </div>
            </div>
            <input type=""hidden"" id=""data-workflow""");
            BeginWriteAttribute("value", " value=\"", 930, "\"", 986, 1);
#nullable restore
#line 27 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\MenuService\Edit.cshtml"
WriteAttributeValue("", 938, JsonConvert.SerializeObject(@Model.WorkFlows), 938, 48, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n            <!--begin::Form-->\r\n");
#nullable restore
#line 29 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\MenuService\Edit.cshtml"
             using (Html.BeginForm("Edit", "MenuService", FormMethod.Post, new { @class = "m-form m-form--fit", role = "form", id = "editMenuService" }))
            {
                

#line default
#line hidden
#nullable disable
#nullable restore
#line 31 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\MenuService\Edit.cshtml"
           Write(Html.Partial("Partials/_UpdateForm", Model));

#line default
#line hidden
#nullable disable
#nullable restore
#line 31 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\MenuService\Edit.cshtml"
                                                            
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </div>\r\n    </div>\r\n</div>\r\n");
            DefineSection("PageInlineScripts", async() => {
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "fa0ba4cafecb9a72a8fda3ab999c491a7a4e42ad7892", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "fa0ba4cafecb9a72a8fda3ab999c491a7a4e42ad8991", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "fa0ba4cafecb9a72a8fda3ab999c491a7a4e42ad10090", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "fa0ba4cafecb9a72a8fda3ab999c491a7a4e42ad11190", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"
    <script type=""text/javascript"">

        function changeWorkflow(el) {
            let workflowId = el.id.replace(""detail_"", """");

            var workflow = WorkflowControl.SelectedItems.find(c => c.info.Id == workflowId);
            setTimeout(function() {
                WorkflowControl.ClearHtml();

                for (let i = 0; i < workflow.info.Forms.length; i++) {
                    WorkflowControl.GenerateHtmlWorkflowDetail(workflow.info.Forms[i], ""formio_id"")
                }
            }, 100);

            $("".m-wizard__step"").each(function() {
                var ct = $(this);
                ct.removeClass(""m-wizard__step--current"")
            });
            $(""#step_"" + workflowId).addClass(""m-wizard__step--current"");
        }
        $(""#btnSave"").on(""click"", function() {
            debugger
            let workFlowIds = WorkflowControl.SelectedItems.map(c => c.info.Id);
            let arr = [];
            for (let i = 0; i < workFlowIds.length; i++) {");
                WriteLiteral(@"
                let menuWorkflow = {
                    WorkFlowId: workFlowIds[i],
                    SortOrder: i
                }
                arr.push(menuWorkflow);
            }
            $(""#workflows"").val(JSON.stringify(arr));
            $(""#editMenuService"").submit();
        });

        $(function() {
            let dt = JSON.parse($(""#data-workflow"").val());
            if (dt && dt.length > 0) {
                dt.forEach(item => {
                    let data = {
                        info: item
                    }
                    WorkflowControl.PushItem(data);
                });
                WorkflowControl.GenerateHtmlStep();
                if (WorkflowControl.SelectedItems[0].Forms && WorkflowControl.SelectedItems[0].Forms.length > 0) {
                    WorkflowControl.SelectedItems[0].Forms.each(item => {
                        WorkflowControl.GenerateHtmlWorkflowDetail(item.info, ""formio_id"");
                    });
                }
 ");
                WriteLiteral("           }\r\n        });\r\n    </script>\r\n");
            }
            );
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<VnCompany.WebApp.Models.MenuServiceCreateOrUpdateViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
