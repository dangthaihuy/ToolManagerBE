using VnCompany.WebApp.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Serilog;
using System;

namespace VnCompany.WebApp
{
    public class AccessRoleChecker : ActionFilterAttribute
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(CommonHelpers));

        public bool AdminRequired = false;
        public bool AgencyRequired { get; set; }
        //Your Properties in AccessRoleChecker Filter
        public string ControllerName { get; set; }
        public string ActionName { get; set; }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var hasPermission = false;

            var ctrl = context.RouteData.Values["action"].ToString();
            string currentAction = context.RouteData.Values["action"].ToString();
            string currentController = context.RouteData.Values["controller"].ToString();           
            var currentUser = CommonHelpers.GetCurrentUser();
            
            if (AdminRequired)
            {
                var isAdmin = CommonHelpers.CurrentUserIsAdmin();
                if (!isAdmin)
                    hasPermission = false;                
            }

            if (AgencyRequired)
            {
                var isAgency = CommonHelpers.CurrentUserIsAgency();
                if (isAgency)
                    hasPermission = false;
                else
                    hasPermission = true;
            }

            if (string.IsNullOrEmpty(this.ControllerName))
            {
                ControllerName += currentController;
            }

            if (string.IsNullOrEmpty(this.ActionName))
            {
                ActionName += currentAction;
            }

            try
            {
                var perList = PermissionHelper.GetAllPermission();
                var userInfo = CommonHelpers.GetCurrentUser();

                if (userInfo.ParentId == 0)
                {
                    hasPermission = true;
                }
                else
                {
                    //Has data from cache
                    if (perList != null && perList.Count > 0)
                    {
                        hasPermission = perList.Exists(m => string.Equals(m.Action, currentAction, StringComparison.CurrentCultureIgnoreCase)
                        && (string.Equals(m.Controller, currentController, StringComparison.CurrentCultureIgnoreCase)));
                    }
                    else
                    {
                        hasPermission = false;
                    }
                }

                if (!hasPermission)
                {
                    if (!context.HttpContext.Request.IsAjaxRequest())
                    {                        
                        context.Result = new RedirectToRouteResult(
                            "Default",
                            new RouteValueDictionary
                            {
                                { "controller", "Error" },
                                { "action", "PermissionDennied" }
                            });
                    }
                    else
                    {
                        context.Result = new JsonResult(
                            new
                            {
                                success = false,
                                message = "Permission dennied !!!"
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Could not check permission of user [{0}] because: {1}", currentUser, ex.ToString());
                _logger.Error(strError);
            }

            base.OnResultExecuting(context);
        }
    }
}
