//using Autofac;
//using VnCompany.SharedLibs.Caching.Providers;
//using VnCompany.SharedLibs.Logging;
//using VnCompany.WebApp.Helpers;
//using VnCompany.WebApp.Resources;
//using System;
//using System.Linq;
//using System.Net;
//using System.Security.Cryptography;
//using System.Text;
//using System.Web;
//using System.Web.Mvc;

//namespace VnCompany.WebApp
//{
//    public class PreventSpamAttribute : ActionFilterAttribute
//    {
//        // This stores the time between Requests (in seconds)
//        public int DelayRequest = 10;
//        // The Error Message that will be displayed in case of 
//        // excessive Requests
//        public string ErrorMessage = ManagerResource.LB_WARNING_SPAM_REQUEST;
//        // This will store the URL to Redirect errors to
//        public string RedirectURL;

//        public override void OnActionExecuting(ActionExecutingContext filterContext)
//        {
//            // Store our HttpContext (for easier reference and code brevity)
//            var request = filterContext.HttpContext.Request;
//            // Store our HttpContext.Cache (for easier reference and code brevity)
//            //var cache = filterContext.HttpContext.Cache;
//            var cacheProvider = GlobalContainer.IocContainer.Resolve<ICacheProvider>();

//            // Grab the IP Address from the originating Request (example)
//            var originationInfo = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress;

//            // Append the User Agent
//            originationInfo += request.UserAgent;

//            // Now we just need the target URL Information
//            var targetInfo = request.RawUrl + request.QueryString;

//            // Generate a hash for your strings (appends each of the bytes of
//            // the value into a single hashed string
//            var hashValue = string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(originationInfo + targetInfo)).Select(s => s.ToString("x2")));

//            string cachedValue = null;

//            cacheProvider.Get(hashValue, out cachedValue);

//            // Checks if the hashed value is contained in the Cache (indicating a repeat request)
//            if (cachedValue != null)
//            {
//                // Adds the Error Message to the Model and Redirect
//                filterContext.Controller.ViewData.ModelState.AddModelError("ExcessiveRequests", ErrorMessage);
//            }
//            else
//            {
//                // Adds an empty object to the cache using the hashValue
//                // to a key (This sets the expiration that will determine
//                // if the Request is valid or not)
//                //cache.Add(hashValue, null, null, DateTime.Now.AddSeconds(DelayRequest), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
//                //cache.Add(hashValue, DateTime.Now.AddSeconds(DelayRequest), null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
//                cacheProvider.SetBySeconds(hashValue, hashValue, DelayRequest);
//            }

//            base.OnActionExecuting(filterContext);
//        }
//    }

//    public class PreventCrossOriginAttribute : ActionFilterAttribute
//    {
//        private readonly ILog logger = LogProvider.For<PreventCrossOriginAttribute>();
//        //public override void OnActionExecuting(HttpActionContext actionContext)
//        //{
//        //    if (HttpContext.Current.Request.UrlReferrer != null)
//        //    {
//        //        if (HttpContext.Current.Request.Url.Host != HttpContext.Current.Request.UrlReferrer.Host)
//        //        {
//        //            throw new HttpException("Invalid method.");
//        //        }
//        //    }
//        //    else
//        //    {
//        //        throw new HttpException("Invalid method.");
//        //    }

//        //    base.OnActionExecuting(actionContext);
//        //}

//        public override void OnActionExecuting(ActionExecutingContext filterContext)
//        {
//            if (HttpContext.Current.Request.UrlReferrer != null)
//            {
//                if (HttpContext.Current.Request.Url.Host != HttpContext.Current.Request.UrlReferrer.Host)
//                {
//                    var strError = string.Format("Anonymous request from : {0}, Url: '{1}'", HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.ToString());
//                    logger.Error(strError);

//                    //throw new HttpException("Invalid method.");
//                    filterContext.Result = new RedirectResult("/Error/Restricted");
//                }
//            }
//            else
//            {
//                var strError = string.Format("Anonymous request from : {0}, Url: '{1}'", HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.ToString());
//                logger.Error(strError);

//                filterContext.Result = new RedirectResult("/Error/Restricted");
//            }

//            base.OnActionExecuting(filterContext);
//        }
//    }

//    public class VerifyLoggedInUserAttribute : ActionFilterAttribute
//    {
//        public override void OnActionExecuting(ActionExecutingContext filterContext)
//        {
//            var statusCode = (int)HttpStatusCode.OK;
            
//            if (!filterContext.RequestContext.HttpContext.Request.IsAuthenticated)
//            {
//                if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
//                {                   
//                    filterContext.HttpContext.Response.StatusCode = statusCode;
//                    filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
//                    filterContext.Result = new JsonResult
//                    {
//                        Data = new { Error = ManagerResource.ERROR_LOGIN_SESSION_ENDED, clientcallback = string.Format("ConfirmFirst(NeedToLogin, '{0}')", ManagerResource.ERROR_LOGIN_SESSION_ENDED), StatusCode = statusCode },
//                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
//                    };
//                }
//                else
//                {
//                    filterContext.Result = new RedirectResult(string.Format("/WebAuth/Login?ReturnUrl={0}", filterContext.HttpContext.Request.Url.AbsolutePath));
//                }
//            }
//        }
//    }

//    public static class HttpRequestExtensions
//    {
//        private const string RequestedWithHeader = "X-Requested-With";
//        private const string XmlHttpRequest = "XMLHttpRequest";

//        public static bool IsAjaxRequest(this HttpRequest request)
//        {
//            if (request == null)
//            {
//                throw new ArgumentNullException("request");
//            }

//            if (request.Headers != null)
//            {
//                return request.Headers[RequestedWithHeader] == XmlHttpRequest;
//            }

//            return false;
//        }
//    }
//}