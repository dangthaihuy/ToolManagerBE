#pragma checksum "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b5dec6e5b890083306139984dcfb4f2c0ba8713e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_RolesAdmin_Operation_Index), @"mvc.1.0.view", @"/Views/RolesAdmin/Operation/Index.cshtml")]
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
#line 1 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
using VnCompany.WebApp.Resources;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b5dec6e5b890083306139984dcfb4f2c0ba8713e", @"/Views/RolesAdmin/Operation/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"99313092a47a5e84e125cd027ab0be8096437f60", @"/Views/_ViewImports.cshtml")]
    public class Views_RolesAdmin_Operation_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<VnCompany.WebApp.Models.OperationViewModel>
    {
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 4 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
  
    ViewBag.Title = "Operation management";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 8 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
Write(Html.Partial("_Notifications"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"

<!-- modal placeholder-->
<div id='frmUpdate' class='modal fade in'>
    <div class=""modal-dialog"">
        <div class=""modal-content"">
            <div>
                <div class=""modal-header"">
                    <button type=""button"" class=""close"" data-dismiss=""modal"" aria-hidden=""true"">&times;</button>
                    <h4 class=""modal-title"" id=""frmUpdateLabel"">Create new access</h4>
                </div>
                <div class=""modal-body"">
");
#nullable restore
#line 20 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
                     using (Html.BeginForm("Create", "Operation", FormMethod.Post, new { role = "form" }))
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <div class=\"row\">\r\n                            <div class=\"col-sm-12 col-xs-12\">\r\n                                <div class=\"form-group\">\r\n                                    ");
#nullable restore
#line 25 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
                               Write(Html.AntiForgeryToken());

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                    ");
#nullable restore
#line 26 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
                               Write(Html.HiddenFor(m => m.OperationId, new { id = "OperationId" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                    <label>Access name (Controller) </label>\r\n                                    <select");
            BeginWriteAttribute("id", " id=\"", 1227, "\"", 1260, 1);
#nullable restore
#line 28 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
WriteAttributeValue("", 1232, Html.NameFor(m=>m.AccessId), 1232, 28, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("name", " name=\"", 1261, "\"", 1296, 1);
#nullable restore
#line 28 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
WriteAttributeValue("", 1268, Html.NameFor(m=>m.AccessId), 1268, 28, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"form-control\">\r\n");
#nullable restore
#line 29 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
                                         if (Model.AllAccess != null && Model.AllAccess.Count > 0)
                                        {
                                            foreach (var item in Model.AllAccess)
                                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "b5dec6e5b890083306139984dcfb4f2c0ba8713e7003", async() => {
#nullable restore
#line 33 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
                                                                    Write(item.AccessName);

#line default
#line hidden
#nullable disable
                WriteLiteral(" (");
#nullable restore
#line 33 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
                                                                                      Write(item.Description);

#line default
#line hidden
#nullable disable
                WriteLiteral(")");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            BeginWriteTagHelperAttribute();
#nullable restore
#line 33 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
                                                   WriteLiteral(item.Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("value", __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
#nullable restore
#line 34 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
                                            }
                                        }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                                    </select>
                                </div>
                            </div>
                            <div class=""col-sm-12 col-xs-12"">
                                <div class=""form-group"">
                                    <label>Action name</label>
                                    ");
#nullable restore
#line 42 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
                               Write(Html.TextBoxFor(m => m.ActionName, new { id = "ActionName", @class = "form-control" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                    ");
#nullable restore
#line 43 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
                               Write(Html.ValidationMessageFor(m => m.ActionName));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                                </div>
                            </div>
                        </div>
                        <div class=""modal-footer"">
                            <input class=""btn btn-danger"" type=""submit"" value=""Save"" />
                            <button class=""btn"" data-dismiss=""modal"">Cancel</button>
                        </div>
");
#nullable restore
#line 51 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n\r\n");
#nullable restore
#line 58 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
Write(Html.Partial("~/Views/Widgets/Modals/_LargeModal.cshtml"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"

<!-- modal placeholder-->
<div id='myModal' class='modal fade in'>
    <div class=""modal-dialog"">
        <div class=""modal-content"">
            <div id='myModalContent'></div>
        </div>
    </div>
</div>

<div class=""row"">
    <div class=""col-sm-12"">

        <div class=""clearfix pull-right"">
            <button type=""button"" onclick=""UpdateData('create')"" class=""btn btn-white btn-info"">Create</button>
        </div>
        <br />
        <br />
        <table id=""sample-table-1"" class=""table table-striped table-bordered table-hover"">
            <thead>
                <tr>
                    <th class=""text-center""></th>
                    <th class=""text-center"">Action name</th>
                    <th class=""text-center"">Desc</th>
                    <th class=""text-center"">Actions</th>
                </tr>
            </thead>

            <tbody>
");
#nullable restore
#line 88 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
                 if (Model.AllOperations != null && Model.AllOperations.Count > 0)
                {
                    var count = 0;
                    foreach (var record in Model.AllOperations)
                    {
                        count++;

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <tr>\r\n                            <td class=\"text-center\">");
#nullable restore
#line 95 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
                                               Write(count);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 96 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
                           Write(record.ActionName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>\r\n                                <div>");
#nullable restore
#line 98 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
                                Write(record.OperationName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n                            </td>\r\n                            <td class=\"text-center\">\r\n                                <div class=\"btn-group\">\r\n                                    <input type=\"hidden\"");
            BeginWriteAttribute("id", " id=\"", 4473, "\"", 4501, 2);
            WriteAttributeValue("", 4478, "hdActionName_", 4478, 13, true);
#nullable restore
#line 102 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
WriteAttributeValue("", 4491, record.Id, 4491, 10, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("value", " value=\"", 4502, "\"", 4528, 1);
#nullable restore
#line 102 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
WriteAttributeValue("", 4510, record.ActionName, 4510, 18, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n                                    <input type=\"hidden\"");
            BeginWriteAttribute("id", " id=\"", 4590, "\"", 4612, 2);
            WriteAttributeValue("", 4595, "hdDesc_", 4595, 7, true);
#nullable restore
#line 103 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
WriteAttributeValue("", 4602, record.Id, 4602, 10, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("value", " value=\"", 4613, "\"", 4642, 1);
#nullable restore
#line 103 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
WriteAttributeValue("", 4621, record.OperationName, 4621, 21, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n                                    <a class=\"main-color btn btn-xs btn-info\" href=\"#\"");
            BeginWriteAttribute("onclick", " onclick=\"", 4734, "\"", 4776, 3);
            WriteAttributeValue("", 4744, "ShowOperationLang(\'", 4744, 19, true);
#nullable restore
#line 104 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
WriteAttributeValue("", 4763, record.Id, 4763, 10, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 4773, "\');", 4773, 3, true);
            EndWriteAttribute();
            BeginWriteAttribute("title", " title=\"", 4777, "\"", 4820, 1);
#nullable restore
#line 104 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
WriteAttributeValue("", 4785, ManagerResource.LB_MULTI_LANGUAGES, 4785, 35, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                                        <i class=\"ace-icon fa fa-language bigger-130\" data-id=\"");
#nullable restore
#line 105 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
                                                                                          Write(record.Id);

#line default
#line hidden
#nullable disable
            WriteLiteral("\"></i>\r\n                                    </a>\r\n                                    <button class=\"btn btn-xs btn-info\"");
            BeginWriteAttribute("onclick", " onclick=\"", 5050, "\"", 5091, 3);
            WriteAttributeValue("", 5060, "UpdateData(\'edit\',\'", 5060, 19, true);
#nullable restore
#line 107 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
WriteAttributeValue("", 5079, record.Id, 5079, 10, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 5089, "\')", 5089, 2, true);
            EndWriteAttribute();
            WriteLiteral(">\r\n                                        <i class=\"ace-icon fa fa-pencil bigger-120\"></i>\r\n                                    </button>\r\n\r\n                                    <a class=\"btn btn-xs btn-danger\"");
            BeginWriteAttribute("href", " href=\"", 5302, "\"", 5376, 1);
#nullable restore
#line 111 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
WriteAttributeValue("", 5309, Url.Action("DeleteOperation", "Operation", new { id = record.Id }), 5309, 67, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@" data-modal="""" title=""Delete this operation"">
                                        <i class=""ace-icon fa fa-trash-o bigger-120""></i>
                                    </a>
                                </div>
                            </td>
                        </tr>
");
#nullable restore
#line 117 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
                    }
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </tbody>\r\n        </table>\r\n    </div>\r\n\r\n</div>\r\n\r\n");
            DefineSection("PagePluginStyles", async() => {
                WriteLiteral("\r\n    <link rel=\"stylesheet\" href=\"/assets/css/jquery.gritter.css\" />\r\n");
            }
            );
            WriteLiteral("\r\n");
            DefineSection("PageInlineStyles", async() => {
                WriteLiteral("\r\n    <style>\r\n    </style>\r\n");
            }
            );
            WriteLiteral("\r\n");
            DefineSection("PagePluginScripts", async() => {
                WriteLiteral("\r\n    <script src=\"/assets/js/jquery.gritter.min.js\"></script>\r\n    <script");
                BeginWriteAttribute("src", " src=\'", 6040, "\'", 6090, 1);
#nullable restore
#line 139 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
WriteAttributeValue("", 6046, Url.Content("~/Scripts/jquery.validate.js"), 6046, 44, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" type=\'text/javascript\'></script>\r\n    <script");
                BeginWriteAttribute("src", " src=\'", 6137, "\'", 6199, 1);
#nullable restore
#line 140 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\RolesAdmin\Operation\Index.cshtml"
WriteAttributeValue("", 6143, Url.Content("~/Scripts/jquery.validate.unobtrusive.js"), 6143, 56, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" type=\'text/javascript\'></script>\r\n\r\n");
            }
            );
            WriteLiteral("\r\n\r\n");
            DefineSection("PageInlineScripts", async() => {
                WriteLiteral(@"
    <!-- inline scripts related to this page -->
    <script type=""text/javascript"">
        function UpdateData(action, id) {
            if (action === 'create') {
                $(""#frmUpdateLabel"").html(""Create new operation"");
                $(""#frmUpdate form"").attr(""action"", ""/Operation/Create"");
                $(""#AccessId"").val("""");
                $(""#ActionName"").val("""");
            }
            else {
                $(""#frmUpdate form"").attr(""action"", ""/Operation/Update"");
                $(""#frmUpdateLabel"").html(""Edit operation"");
                $(""#AccessId"").val(id);
                $(""#ActionName"").val($(""#hdActionName_"" + id).val())
            }

            $('#frmUpdate').modal('show');
        }
        function ShowOperationLang(id) {
            $.ajax({
                url: 'Operation/ShowOperationLang',
                data: {
                    id: id
                },
                success: function (result) {
                    if (result) {");
                WriteLiteral(@"
                        $(""#myModalContent"").html(result);
                        $('body').addClass('modal-open');
                        $('.modal-backdrop').show();
                        $('#myModal').modal('show');
                        $('.selectpicker').selectpicker({ style: ""btn-default"" });
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    var responseTitle = ""Error encountered""
                    $.showErrorMessage('Error message', $(responseTitle).text() + ""\n"" + formatErrorMessage(jqXHR, errorThrown), function () { });
                }
            });
        }
    </script>
");
            }
            );
            WriteLiteral("\r\n\r\n\r\n\r\n\r\n\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<VnCompany.WebApp.Models.OperationViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
