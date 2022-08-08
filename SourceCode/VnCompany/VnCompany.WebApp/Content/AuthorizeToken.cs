//using System;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using System.Web.Routing;
//using Microsoft.AspNet.Identity;
//using VnCompany.DataLayer;

//using Autofac;
//using VnCompany.SharedLibs.Caching.Providers;
//using System.Collections.Generic;
//using VnCompany.SharedLibs.Logging;
//using VnCompany.WebApp.Helpers;
//using VnCompany.WebApp.Resources;
//using System.Text;
//using Newtonsoft.Json;

//namespace VnCompany.WebApp
//{
//    public class AuthorizeToken : AuthorizeAttribute
//    {
//        private readonly ILog logger = LogProvider.For<AuthorizeToken>();
//        public bool AdminRequired = false;
//        public string ControllerName { get; set; }
//        public string ActionName { get; set; }

//		protected override bool AuthorizeCore(HttpContextBase httpContext)
//		{
//			bool Authorized = this.CheckPermission(httpContext);
//			if (Authorized)
//			{
//				Authorized = this.CheckToken(httpContext);
//			}
//			return Authorized;
//		}

//		protected bool CheckPermission(HttpContextBase httpContext)
//		{
//			RouteData rd = httpContext.Request.RequestContext.RouteData;
//			string currentAction = rd.GetRequiredString("action");
//			string currentController = rd.GetRequiredString("controller");
//			string currentUser = HttpContext.Current.User.Identity.GetUserId();
//			string currentUserName = HttpContext.Current.User.Identity.GetUserName();
//			if (this.AdminRequired && !string.IsNullOrEmpty(currentUserName) && !AccountHelper.CurrentUserIsAdmin())
//			{
//				return false;
//			}
//			if (string.IsNullOrEmpty(this.ControllerName))
//			{
//				this.ControllerName += currentController;
//			}
//			if (string.IsNullOrEmpty(this.ActionName))
//			{
//				this.ActionName += currentAction;
//			}
//			if (!base.AuthorizeCore(httpContext))
//			{
//				return false;
//			}
//			bool hasPermission = false;
//			try
//			{
//				List<IdentityPermission> perList = PermissionHelper.GetAllPermission();
//				hasPermission = (AccountHelper.GetUserById(currentUser).ParentId == 0 || (perList != null && perList.Count > 0 && perList.Exists((IdentityPermission m) => string.Equals(m.Action, currentAction, StringComparison.CurrentCultureIgnoreCase) && string.Equals(m.Controller, currentController, StringComparison.CurrentCultureIgnoreCase))));
//			}
//			catch (Exception ex)
//			{
//				string strError = string.Format("Could not check permission of user [{0}] because: {1}", currentUser, ex.ToString());
//				this.logger.Error(strError);
//			}
//			return hasPermission;
//		}

//		protected bool CheckToken(HttpContextBase httpContext)
//		{
//			RouteData rd = httpContext.Request.RequestContext.RouteData;
//			string currentAction = rd.GetRequiredString("action");
//			string currentController = rd.GetRequiredString("controller");
//			string objStr = string.Empty;
//			string tkUrl = string.Empty;
//			string[] allKeys = httpContext.Request.QueryString.AllKeys;
//			for (int i = 0; i < allKeys.Length; i++)
//			{
//				string key = allKeys[i];
//				if (key != "tk")
//				{
//					if (string.IsNullOrEmpty(objStr))
//					{
//						int result = 0;
//						if (int.TryParse(httpContext.Request[key], out result))
//						{
//							objStr = key + ":" + result.ToString();
//						}
//						else
//						{
//							objStr = key + ":'" + HttpUtility.UrlEncode(httpContext.Request[key]) + "'";
//						}
//					}
//					else
//					{
//						int result2 = 0;
//						if (int.TryParse(httpContext.Request[key], out result2))
//						{
//							objStr = string.Concat(new string[]
//							{
//								objStr,
//								" , ",
//								key,
//								":",
//								result2.ToString()
//							});
//						}
//						else
//						{
//							objStr = string.Concat(new string[]
//							{
//								objStr,
//								" , ",
//								key,
//								":'",
//								HttpUtility.UrlEncode(httpContext.Request[key]),
//								"'"
//							});
//						}
//					}
//				}
//				else
//				{
//					tkUrl = httpContext.Request[key];
//				}
//			}
//			objStr = "{ " + objStr + " }";
//			object objParams = JsonConvert.DeserializeObject<object>(objStr);
			

//			var tk = SecurityHelper.GenerateUrlToken(currentController, currentAction, objParams);
//			if (tk == tkUrl) return true;
//			return false;
//		}

//		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
//		{
//			if (filterContext.HttpContext.Request.IsAjaxRequest())
//			{
//				filterContext.Result = new JsonResult
//				{
//					Data = new
//					{
//						success = false,
//						message = ManagerResource.COMMON_ERROR_DATA_INVALID,
//						title = ManagerResource.LB_ERROR_OCCURED
//					},
//					ContentEncoding = Encoding.UTF8,
//					ContentType = "application/json",
//					JsonRequestBehavior = JsonRequestBehavior.AllowGet
//				};
//				return;
//			}
//			filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
//			{
//				controller = "Error",
//				action = "Unauthorised"
//			}));
//		}

//	}
//}
