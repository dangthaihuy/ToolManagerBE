#pragma checksum "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "15c42b56465591c95c4c103015fab61da8b7b20d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
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
#line 1 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\Home\Index.cshtml"
using VnCompany.WebApp.Resources;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\Home\Index.cshtml"
using System.Globalization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\Home\Index.cshtml"
using VnCompany.WebApp;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"15c42b56465591c95c4c103015fab61da8b7b20d", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"99313092a47a5e84e125cd027ab0be8096437f60", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 5 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\Home\Index.cshtml"
  
    ViewBag.Title = ManagerResource.LB_DASHBOARD;
    ViewBag.HideSubHeader = true;
    var currentLang = CultureInfo.CurrentCulture.ToString();
    var siteName = WebContext.GetSiteSettings().General.SiteName;
    if (string.IsNullOrEmpty(siteName))
    {
        siteName = "Admin";
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 16 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\Home\Index.cshtml"
Write(Html.Partial("../Widgets/Modals/_LargeModal"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"

<div class=""m-alert m-alert--icon m-alert--air m-alert--square alert alert-success alert-dismissible fade show"" role=""alert"" id=""DashboardWelcome"">
    <div class=""m-alert__icon"">
        <i class=""la la-dashboard""></i>
    </div>
    <div class=""m-alert__text"" style=""font-size:1.1rem;"">
        ");
#nullable restore
#line 23 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\Home\Index.cshtml"
   Write(ManagerResource.LB_DASHBOARD_WELCOME_TO);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        <strong>\r\n            ");
#nullable restore
#line 25 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\Home\Index.cshtml"
       Write(siteName);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            <small>(v1.0)</small>\r\n        </strong>\r\n        ");
#nullable restore
#line 28 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\Home\Index.cshtml"
   Write(ManagerResource.LB_DASHBOARD_WELCOME_SLOGAN);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
    </div>
    <div class=""m-alert__close"">
        <button type=""button"" class=""close"" data-dismiss=""alert"" aria-label=""Close""></button>
    </div>
</div>

<div class=""row hidden"">
    <div class=""col-md-12"">
        <div class=""m-portlet"" m-portlet=""true"" id=""m_portlet_tools_1"">
            <div class=""m-portlet__head"">
                <div class=""m-portlet__head-caption"">
                    <div class=""m-portlet__head-title"">
                        <h3 class=""m-portlet__head-text m-chart-title"">
                            <i class=""flaticon-notes fz-20""></i>
                            ");
#nullable restore
#line 43 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\Home\Index.cshtml"
                       Write(ManagerResource.LB_NEW_FEATURES);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        </h3>\r\n                    </div>\r\n                </div>\r\n            </div>\r\n            <div class=\"m-portlet__body\">\r\n");
#nullable restore
#line 49 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\Home\Index.cshtml"
                 if (currentLang == "ja-JP")
                {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                    <div class=""m-widget3"">
                        <div class=""m-widget3__item"">
                            <div class=""m-widget3__header"">

                            </div>
                            <div class=""m-widget3__body"">
                                <div");
            BeginWriteAttribute("class", " class=\"", 2095, "\"", 2103, 0);
            EndWriteAttribute();
            WriteLiteral(@">
                                    <i class=""fa fa-hand-o-right text-info""></i>
                                    07:30 18/05/2020 一括メール送信できる機能をついかしました。
                                </div>
                            </div>
                        </div>
                    </div>
");
#nullable restore
#line 64 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\Home\Index.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n\r\n<div class=\"row\">\r\n    <div class=\"col-xl-6\" id=\"WidgetNotifContainer\">\r\n");
            WriteLiteral(@"        <div class=""m-portlet m-portlet--full-height m-portlet--full-height-custom "">
            <div class=""m-portlet__head"">
                <div class=""m-portlet__head-caption"">
                    <div class=""m-portlet__head-title"">

                        <h3 class=""m-portlet__head-text"">
                            <i class=""flaticon-music-2 fz-20""></i> ");
#nullable restore
#line 80 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\Home\Index.cshtml"
                                                              Write(ManagerResource.LB_NOTIFICATION);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                        </h3>
                    </div>
                </div>
            </div>
            <div class=""m-portlet__body"">
                <div class=""m-widget3"" id=""WidgetNotif"" style=""min-height:100px; max-height:50vh;overflow: auto;"">

                </div>
            </div>
        </div>
");
            WriteLiteral(@"    </div>
</div>

<div class=""row"">
    <div class=""col-md-6"" id=""WeekStatisticsChart"" style=""display:none;"">
        <div class=""m-portlet"" m-portlet=""true"" id=""m_portlet_tools_1"">
            <div class=""m-portlet__head"">
                <div class=""m-portlet__head-caption"">
                    <div class=""m-portlet__head-title"">
                        <h3 class=""m-portlet__head-text m-chart-title"">
                            <i class=""flaticon-line-graph fz-20""></i>
                            ");
#nullable restore
#line 103 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\Home\Index.cshtml"
                       Write(ManagerResource.LB_STATISTICS_IN_WEEK);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                        </h3>
                    </div>
                </div>
            </div>
            <div class=""m-portlet__body"" style=""height:400px"">
                <canvas id=""WeekStatisticsChartCanvas""></canvas>
            </div>
        </div>
    </div>

    <div class=""col-md-6"" id=""AppWeekStatisticsChart"" style=""display:none;"">
        <div class=""m-portlet"" m-portlet=""true"" id=""m_portlet_tools_3"">
            <div class=""m-portlet__head"">
                <div class=""m-portlet__head-caption"">
                    <div class=""m-portlet__head-title"">
                        <h3 class=""m-portlet__head-text m-chart-title"">
                            <i class=""flaticon-statistics fz-20""></i>
                            ");
#nullable restore
#line 121 "E:\MyJobs\sensei\SourceCode\VnCompany\VnCompany.WebApp\Views\Home\Index.cshtml"
                       Write(ManagerResource.LB_STATISTICS_APP_IN_WEEK);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                        </h3>
                    </div>
                </div>
            </div>
            <div class=""m-portlet__body"" style=""height:400px"">
                <canvas id=""AppWeekStatisticsChartCanvas""></canvas>
            </div>
        </div>
    </div>
</div>



");
            DefineSection("PageInlineStyles", async() => {
                WriteLiteral(@"
    <style>
        canvas {
            -moz-user-select: none;
            -webkit-user-select: none;
            -ms-user-select: none;
        }

        .m-portlet__head-text {
            color: #1A3365 !important;
        }

            .m-portlet__head-text i:before {
                font-weight: bold;
            }

        .m-body .m-content {
            padding: 15px 15px;
            height: auto;
        }

        .widget-noti-item {
            padding: 5px !important;
            border: 1px solid #ccc !important;
            margin-bottom: 5px !important;
        }
    </style>
");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
