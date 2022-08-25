using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Manager.WebApp.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Manager.DataLayer;
using Manager.DataLayer.Entities;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Manager.WebApp.Controllers
{
    public class BaseController : Controller
    {
        //protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        //{
        //    var lang = CommonHelpers.GetCurrentLanguageOrDefault();
        //    var cultureInfo = new CultureInfo(lang);
        //    Thread.CurrentThread.CurrentUICulture = cultureInfo;
        //    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);

        //    return base.BeginExecuteCore(callback, state);
        //}

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Preparing before executing action
            if (!Request.IsAjaxRequest())
                ViewBag.AdminNavMenu = MenuHelper.GetAdminMenus();

            base.OnActionExecuting(filterContext);
        }

        protected string GetModelStateErrors(ModelStateDictionary stateDic)
        {
            var sb = new StringBuilder();
            foreach (var errorKey in stateDic.Keys)
            {
                foreach (var errorMsg in stateDic[errorKey].Errors)
                {
                    sb.AppendLine(errorMsg.ErrorMessage + "<br />");
                }
            }

            return sb.ToString();
        }


        public Dictionary<string, List<string>> GetModelStateErrorList(ModelStateDictionary stateDic)
        {
            var result = new Dictionary<string, List<string>>();

            foreach (var errorKey in stateDic.Keys)
            {
                var errors = new List<string>();
                foreach (var errorMsg in stateDic[errorKey].Errors)
                {
                    errors.Add(errorMsg.ErrorMessage);
                }

                result.Add(errorKey, errors);
            }

            return result;
        }

        public void AddNotificationModelStateErrors(ModelStateDictionary stateDic)
        {
            foreach (var errorKey in stateDic.Keys)
            {
                foreach (var errorMsg in stateDic[errorKey].Errors)
                {
                    this.AddNotification(errorMsg.ErrorMessage, NotificationType.ERROR);
                }
            }
        }

        protected IdentityUser GetCurrentUser()
        {
            if (User.Identity.IsAuthenticated)
                return CommonHelpers.GetCurrentUser();

            return null;
        }

        //protected IdentityUser GetByStaffId(int staffId)
        //{
        //    if (staffId > 0)
        //        return AccountHelper.GetByStaffId(staffId);

        //    return null;
        //}

        protected string GetCurrentUserId()
        {
            var currentUser = GetCurrentUser();
            if (currentUser != null)
                return currentUser.Id;

            return string.Empty;
        }

        protected int GetCurrentAgencyId()
        {
            var currentUser = GetCurrentUser();
            if (currentUser != null)
            {
                if (currentUser.ParentId == 0)
                    return currentUser.StaffId;
                else
                    return currentUser.ParentId;
            }

            return 0;
        }

        protected int GetCurrentStaffId()
        {
            var currentUser = GetCurrentUser();
            if (currentUser != null)
            {
                return currentUser.StaffId;
            }

            return 0;
        }        
    }
}