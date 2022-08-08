using System;
using System.Globalization;
using System.IO;
using VnCompany.WebApp.Settings;
using VnCompany.SharedLibs;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VnCompany.WebApp.Helpers;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.Tasks;
using Rotativa;

namespace VnCompany.WebApp.Controllers
{
    [Authorize]
    public class BaseAuthedController : BaseController
    {
        //protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        //{
        //    var lang = UserCookieManager.GetCurrentLanguageOrDefault();
        //    var cultureInfo = new CultureInfo(lang);
        //    Thread.CurrentThread.CurrentUICulture = cultureInfo;
        //    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);

        //    //if (!User.Identity.IsAuthenticated && !Request.IsAjaxRequest())
        //    //    Response.Redirect("~/Account/Login");

        //    return base.BeginExecuteCore(callback, state);
        //}

        [AllowAnonymous]
        public ActionResult ChangeLanguage(string lang)
        {
            var currentLang = CommonHelpers.GetCurrentLanguageOrDefault();
            if (Request != null)
            {
                var currentUrl = Request.Headers["Referer"].ToString();
                if (!string.IsNullOrEmpty(currentUrl))
                {
                    if (currentLang != lang)
                        new LanguageMessageHandler().SetLanguage(lang);

                    return Redirect(currentUrl);
                }
            }

            new LanguageMessageHandler().SetLanguage(lang);

            return RedirectToAction("Index", "Home");
        }

        public dynamic GetDefaultFilterModel(dynamic model,int searchStatus = 1)
        {
            model.Action = ControllerContext.RouteData.Values["action"].ToString();
            model.Controller = ControllerContext.RouteData.Values["controller"].ToString();
            model.SearchStatus = searchStatus;
            int currentPage = 1;

            foreach (PropertyInfo prop in model.GetType().GetProperties())
            {
                if (prop.PropertyType.ToString().Contains("DateTime"))
                {
                    var value = Request.Headers[prop.Name];
                    if (!string.IsNullOrEmpty(Request.Headers[prop.Name]))
                    {
                        DateTime myDate = DateTime.ParseExact(Request.Headers[prop.Name], "yyyy/MM/dd",
                                       CultureInfo.InvariantCulture);
                        prop.SetValue(model, myDate);
                    }
                }
            }

            currentPage = Utils.ConvertToInt32(Request.Headers["Page"], 1);
            model.PageSize = SystemSettings.DefaultPageSize;
            model.CurrentPage = currentPage;

            return model;
        }

        public string PartialViewAsString<TModel>(string viewName, TModel model, bool partial = true)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = this.ControllerContext.ActionDescriptor.ActionName;
            }

            this.ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                IViewEngine viewEngine = this.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                ViewEngineResult viewResult = viewEngine.FindView(this.ControllerContext, viewName, !partial);

                if (viewResult.Success == false)
                {
                    return $"A view with the name {viewName} could not be found";
                }

                ViewContext viewContext = new ViewContext(
                    this.ControllerContext,
                    viewResult.View,
                    this.ViewData,
                    this.TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }

        protected ActionResult RedirectToErrorPage(string pageName = "NotFound")
        {
            return View(string.Format("../Error/{0}", pageName));
        }

        protected dynamic GetFilterConfig()
        {
            var currentPage = 1;
            var status = -1;
            var keyword = string.Empty;
            var pageSize = SystemSettings.DefaultPageSize;

            currentPage = Utils.ConvertToInt32(Request.Headers["Page"], 1);
            keyword = Request.Headers["Keyword"].ToString();
            status = Utils.ConvertToInt32(Request.Headers["Status"], -1);

            if (!string.IsNullOrEmpty(keyword))
                keyword = keyword.Trim();

            dynamic filter = new System.Dynamic.ExpandoObject();

            filter.keyword = keyword;
            filter.status = status;
            filter.page_index = currentPage;
            filter.page_size = pageSize;
            filter.language_code = CommonHelpers.GetCurrentLanguageOrDefault();

            return filter;
        }
    }

    public class ViewAsPdfToFile : PartialViewAsPdf
    {
        public ViewAsPdfToFile(string viewName, object model) : base(viewName, model)
        {

        }
    }
}