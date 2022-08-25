//using Manager.WebApp.Helpers;
//using Manager.WebApp.Resources;
//using Manager.WebApp.Settings;
//using System;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.ComponentModel;
//using System.Dynamic;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;
//using System.Web;
//using System.Web.Mvc;
//using System.Web.Routing;

//namespace Manager.WebApp
//{
//    public static class ObjectToDictionaryHelper
//    {
//        public static IDictionary<string, object> ToDictionary(this object source)
//        {
//            return source.ToDictionary<object>();
//        }

//        public static IDictionary<string, T> ToDictionary<T>(this object source)
//        {
//            if (source == null)
//                ThrowExceptionWhenSourceArgumentIsNull();

//            var dictionary = new Dictionary<string, T>();
//            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
//                AddPropertyToDictionary<T>(property, source, dictionary);
//            return dictionary;
//        }

//        private static void AddPropertyToDictionary<T>(PropertyDescriptor property, object source, Dictionary<string, T> dictionary)
//        {
//            object value = property.GetValue(source);
//            if (IsOfType<T>(value))
//                dictionary.Add(property.Name, (T)value);
//        }

//        private static bool IsOfType<T>(object value)
//        {
//            return value is T;
//        }

//        private static void ThrowExceptionWhenSourceArgumentIsNull()
//        {
//            throw new ArgumentNullException("source", "Unable to convert object to a dictionary. The source object is null.");
//        }
//    }

//    public static class SecurityHelper
//    {
//        private static readonly char[] padding = { '=' };

//        public static MvcHtmlString EncodedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
//        {
//            string queryString = string.Empty;
//            string htmlAttributesString = string.Empty;
//            if (routeValues != null)
//            {
//                RouteValueDictionary d = new RouteValueDictionary(routeValues);
//                for (int i = 0; i < d.Keys.Count; i++)
//                {
//                    if (i > 0)
//                    {
//                        queryString += "&";
//                    }
//                    queryString += d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
//                }
//            }

//            if (htmlAttributes != null)
//            {
//                RouteValueDictionary d = new RouteValueDictionary(htmlAttributes);
//                for (int i = 0; i < d.Keys.Count; i++)
//                {
//                    htmlAttributesString += " " + d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
//                }
//            }

//            //What is Entity Framework??
//            StringBuilder ancor = new StringBuilder();
//            ancor.Append("<a ");
//            if (htmlAttributesString != string.Empty)
//            {
//                ancor.Append(htmlAttributesString);
//            }
//            ancor.Append(" href='");
//            if (controllerName != string.Empty)
//            {
//                ancor.Append("/" + controllerName);
//            }

//            if (actionName != "Index")
//            {
//                ancor.Append("/" + actionName);
//            }
//            if (queryString != string.Empty)
//            {
//                var token = GenerateUrlToken(controllerName, actionName, routeValues);
//                ancor.Append(string.Format("?{0}&tk={1}", queryString, token));
//            }
            
//            ancor.Append("'");
//            ancor.Append(">");
//            ancor.Append(linkText);
//            ancor.Append("</a>");
//            return new MvcHtmlString(ancor.ToString());
//        }

//        public static string GenerateSecureLink(string controllerName, string actionName, object argumentParams)
//        {
//            StringBuilder builderStr = new StringBuilder();
//            string queryString = string.Empty;
//            try
//            {
//                string htmlAttributesString = string.Empty;
//                if (argumentParams != null)
//                {
//                    RouteValueDictionary d = new RouteValueDictionary(argumentParams);
//                    for (int i = 0; i < d.Keys.Count; i++)
//                    {
//                        if (i > 0)
//                        {
//                            queryString += "&";
//                        }
//                        queryString += d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
//                    }
//                }

//                //What is Entity Framework??
//                if (controllerName != string.Empty)
//                {
//                    builderStr.Append("/" + controllerName);
//                }

//                if (actionName != "Index")
//                {
//                    builderStr.Append("/" + actionName);
//                }
//                if (queryString != string.Empty)
//                {
//                    var token = GenerateUrlToken(controllerName, actionName, argumentParams);
//                    builderStr.Append(string.Format("?{0}&tk={1}", queryString, token));
//                }
//            }
//            catch
//            {

//            }

//            return builderStr.ToString();
//        }

//        public static string GenerateSecureLinkDynamic(string controllerName, string actionName, dynamic argumentParams)
//        {
//            StringBuilder builderStr = new StringBuilder();
//            string queryString = string.Empty;
//            try
//            {
//                string htmlAttributesString = string.Empty;
//                if (argumentParams != null)
//                {
//                    RouteValueDictionary d = new RouteValueDictionary(argumentParams);
//                    for (int i = 0; i < d.Keys.Count; i++)
//                    {
//                        if (i > 0)
//                        {
//                            queryString += "&";
//                        }
//                        queryString += d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
//                    }
//                }

//                //What is Entity Framework??
//                if (controllerName != string.Empty)
//                {
//                    builderStr.Append("/" + controllerName);
//                }

//                if (actionName != "Index")
//                {
//                    builderStr.Append("/" + actionName);
//                }
//                if (queryString != string.Empty)
//                {
//                    var token = GenerateUrlToken(controllerName, actionName, argumentParams);
//                    builderStr.Append(string.Format("?{0}&tk={1}", queryString, token));
//                }
//            }
//            catch
//            {

//            }

//            return builderStr.ToString();
//        }

//        public static string GenerateUrlToken(string controllerName, string actionName, object argumentParams)
//        {
//            string token = "";
//            //The salt can be defined global
//            string salt = "#testsalt";
//            //generating the partial url
//            if (!string.IsNullOrEmpty(controllerName))
//                controllerName = controllerName.ToLower();

//            if (!string.IsNullOrEmpty(actionName))
//                actionName = actionName.ToLower();

//            string stringToToken = controllerName + "/" + actionName + "/";
//            if (argumentParams.GetType() == typeof(RouteValueDictionary))
//            {
//                var myParams = (RouteValueDictionary)argumentParams;
//                foreach (var item in myParams)
//                {
//                    if(item.Value.GetType() == typeof(string))
//                    {
//                        if (string.IsNullOrEmpty(item.Value.ToString()))
//                        {
//                            continue;
//                        }
//                    }

//                    if(item.Value != null)
//                        stringToToken += "/" + item.Value;
//                }
//            }
//            else
//            {
//                var paramsDictionary = ObjectToDictionaryHelper.ToDictionary(argumentParams);
//                foreach (var item in paramsDictionary)
//                {
//                    if (item.Value.GetType() == typeof(string))
//                    {
//                        if (string.IsNullOrEmpty(item.Value.ToString()))
//                        {
//                            continue;
//                        }
//                    }

//                    if (item.Value != null)
//                        stringToToken += "/" + item.Value;
//                }
//            }

//            ////Converting the salt in to a byte array
//            //byte[] saltValueBytes = System.Text.Encoding.ASCII.GetBytes(salt);
//            ////Encrypt the salt bytes with the password
//            //Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(SystemSettings.GenerateTokenSecretKey, saltValueBytes);
//            ////get the key bytes from the above process
//            //byte[] secretKey = key.GetBytes(16);
//            ////generate the hash
//            //HMACSHA1 tokenHash = new HMACSHA1(secretKey);
//            //tokenHash.ComputeHash(System.Text.Encoding.ASCII.GetBytes(stringToToken));
//            ////convert the hash to a base64string
//            //token = Convert.ToBase64String(tokenHash.Hash);

//            //token = EncryptDecryptUtils.Encrypt(stringToToken, SystemSettings.GenerateTokenSecretKey, salt, "SHA1");

//            token = Utility.EncryptText(stringToToken, SystemSettings.GenerateTokenSecretKey, salt, "SHA1");
//            if (!string.IsNullOrEmpty(token))
//            {
//                token = token.TrimEnd(padding).Replace('+', '-').Replace('/', '_');
//            }

//            //token = HttpUtility.UrlEncode(token);
//            return token;
//        }
//    }

//    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
//    public class EncryptedActionParameterAttribute : ActionFilterAttribute
//    {
//        public override void OnActionExecuting(ActionExecutingContext filterContext)
//        {
//            Dictionary<string, object> decryptedParameters = new Dictionary<string, object>();
//            if (HttpContext.Current.Request.QueryString.Get("tk") != null)
//            {
//                string encryptedQueryString = HttpContext.Current.Request.QueryString.Get("tk");
//                //string decrptedString = HtmlHelpers.Decrypt(encryptedQueryString.ToString());
//                string decrptedString = string.Empty;
//                string[] paramsArrs = decrptedString.Split('?');

//                for (int i = 0; i < paramsArrs.Length; i++)
//                {
//                    string[] paramArr = paramsArrs[i].Split('=');
//                    decryptedParameters.Add(paramArr[0], Convert.ToInt32(paramArr[1]));
//                }
//            }
//            for (int i = 0; i < decryptedParameters.Count; i++)
//            {
//                filterContext.ActionParameters[decryptedParameters.Keys.ElementAt(i)] = decryptedParameters.Values.ElementAt(i);
//            }
//            base.OnActionExecuting(filterContext);
//        }
//    }

//    //This is a static helper class which creates the URL Hash
//    public static class TokenUtility
//    {
//        private static readonly char[] padding = { '=' };

//        #region Not use 
//        public static string generateUrlToken(string controllerName, string actionName, RouteValueDictionary argumentParams, string password)
//        {
//            //The URL hash is dynamic by assign a dynamic key in each session. So
//            //eventhough your URL is stolen, it will not work in other session
//            if (HttpContext.Current.Session["url_dynamickey"] == null)
//            {
//                HttpContext.Current.Session["url_dynamickey"] = RandomString();
//            }
//            string token = "";
//            //The salt include the dynamic session key and valid for an hour.
//            string salt = HttpContext.Current.Session["url_dynamickey"].ToString() + DateTime.Now.ToShortDateString() + " " + DateTime.Now.Hour; ;
//            //generating the partial url
//            string stringToToken = controllerName + "/" + actionName + "/";
//            foreach (KeyValuePair<string, object> item in argumentParams)
//            {
//                if (item.Key != "controller" && item.Key != "action" && item.Key != "tk")
//                {
//                    stringToToken += "/" + item.Value;
//                }
//            }
//            //Converting the salt in to a byte array
//            byte[] saltValueBytes = System.Text.Encoding.ASCII.GetBytes(salt);
//            //Encrypt the salt bytes with the password
//            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, saltValueBytes);
//            //get the key bytes from the above process
//            byte[] secretKey = key.GetBytes(16);
//            //generate the hash
//            HMACSHA1 tokenHash = new HMACSHA1(secretKey);
//            tokenHash.ComputeHash(System.Text.Encoding.ASCII.GetBytes(stringToToken));
//            //convert the hash to a base64string
//            token = Convert.ToBase64String(tokenHash.Hash).Replace("/", "_");
//            return token;
//        }

//        //This validates the token
//        public static bool validateToken(string token, string controllerName, string actionName, RouteValueDictionary argumentParams, string password)
//        {
//            //compute the token for the currrent parameter
//            string computedToken = generateUrlToken(controllerName, actionName, argumentParams, password);
//            //compare with the submitted token
//            if (computedToken != token)
//            {
//                computedToken = generateUrlToken("", actionName, argumentParams, password);
//            }
//            else { return true; }

//            if (computedToken != token)
//            {
//                return false;
//            }
//            else { return true; }
//        }

//        public static RouteValueDictionary QueryStringAsRouteValueDictionary(RequestContext requestContext)
//        {
//            // shorthand
//            var qs = requestContext.HttpContext.Request.QueryString;

//            NameValueCollection fixedQueryString = new NameValueCollection();
//            foreach (string key in qs.Keys)
//            {
//                string value = qs[key];
//                if (value.Contains(","))
//                {
//                    value = value.Split(',')[0];
//                }
//                fixedQueryString.Add(key, value);
//            }

//            //RouteValueDictionary tRVD = new RouteValueDictionary(requestContext.RouteData.Values);
//            //foreach (string key in requestContext.HttpContext.Request.QueryString.Keys)
//            //{
//            //    tRVD[key] = requestContext.HttpContext.Request.QueryString[key].ToString();
//            //}

//            //return tRVD;

//            // because LINQ is the (old) new black
//            return fixedQueryString.AllKeys.Aggregate(new RouteValueDictionary(requestContext.RouteData.Values),
//                (rvd, k) =>
//                {
//                    // can't separately add multiple values `?foo=1&foo=2` to dictionary, they'll be combined as `foo=1,2`
//                    //qs.GetValues(k).ForEach(v => rvd.Add(k, v));
//                    rvd.Add(k, fixedQueryString[k]);
//                    return rvd;
//                });
//        }

//        //public static object ParseObjectFromQueryString(string valueToConvert)
//        //{
//        //    TypeConverter obj = TypeDescriptor.GetConverter(dataType);
//        //    object value = obj.ConvertFromString(null, CultureInfo.InvariantCulture, valueToConvert);
//        //    return value;
//        //}

//        //It validates the token, where all the parameters passed as a RouteValueDictionary
//        public static bool validateToken(RouteValueDictionary requestUrlParts, string password)
//        {
//            //get the parameters
//            string controllerName;
//            try
//            {
//                controllerName = Convert.ToString(requestUrlParts["controller"]);
//            }
//            catch
//            {
//                controllerName = "";
//            }

//            string actionName = Convert.ToString(requestUrlParts["action"]);
//            string token = Convert.ToString(requestUrlParts["tk"]);
//            //Compute a new hash
//            string computedToken = generateUrlToken(controllerName, actionName, requestUrlParts, password);
//            //compare with the submited hash
//            if (computedToken != token)
//            {
//                computedToken = generateUrlToken("", actionName, requestUrlParts, password);
//            }
//            else { return true; }

//            if (computedToken != token)
//            {
//                return false;
//            }
//            else { return true; }
//        }

//        //This method create the random dynamic key for the session
//        private static string RandomString()
//        {
//            StringBuilder builder = new StringBuilder();
//            Random random = new Random();
//            char ch;
//            for (int i = 0; i < 8; i++)
//            {
//                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
//                builder.Append(ch);
//            }
//            return builder.ToString();
//        }

//        #endregion

//        public static object ToAnonymousObject(this IDictionary<string, object> @this)
//        {
//            var expandoObject = new ExpandoObject();
//            var expandoDictionary = (IDictionary<string, object>)expandoObject;

//            foreach (var keyValuePair in @this)
//            {
//                expandoDictionary.Add(keyValuePair);
//            }
//            return expandoObject;
//        }

//        //It validates the token, where all the parameters passed as a RouteValueDictionary
//        public static bool IsValidToken(RouteValueDictionary requestUrlParts, string password, AuthorizationContext filterContext)
//        {
//            //get the parameters
//            string controllerName;
//            try
//            {
//                controllerName = Convert.ToString(requestUrlParts["controller"]);
//            }
//            catch
//            {
//                controllerName = "";
//            }

//            string actionName = Convert.ToString(requestUrlParts["action"]);
            
//            string token = Convert.ToString(requestUrlParts["tk"]);

//            if (string.IsNullOrEmpty(token))
//            {
//                if(filterContext.HttpContext.Request.HttpMethod == "POST")
//                {
//                    token = filterContext.HttpContext.Request["tk"].ToString();
//                }
//            }

//            //Compute a new hash
//            var myParams = requestUrlParts;
//            myParams.Remove("controller");
//            myParams.Remove("action");
//            myParams.Remove("tk");
//            myParams.Remove("_");

//            string computedToken = SecurityHelper.GenerateUrlToken(controllerName, actionName, myParams);
//            //computedToken = HttpUtility.UrlDecode(computedToken);

//            if (!string.IsNullOrEmpty(computedToken))
//            {
//                computedToken = computedToken.TrimEnd(padding).Replace('+', '-').Replace('/', '_');
//            }

//            if (computedToken != token)
//            {
//                return false;
//            }
//            else { return true; }
//        }

//        public static RouteValueDictionary ToRouteValues(this NameValueCollection queryString)
//        {
//            if (queryString == null || queryString.HasKeys() == false) return new RouteValueDictionary();

//            var routeValues = new RouteValueDictionary();
//            foreach (string key in queryString.AllKeys)
//                routeValues.Add(key, queryString[key]);

//            return routeValues;
//        }
//    }


//    public class RestrictCopyRequestAttribute : AuthorizeAttribute
//    {
//        public override void OnAuthorization(AuthorizationContext filterContext)
//        {
//            // basic athorization check 
//            //base.OnAuthorization(filterContext);
//            if (filterContext.HttpContext != null)
//            {
//                //Http referrer check and do the redirection if error occurs
//                //It uses a controller named ErrorViewController and action named DisplayHttpReferrerError
//                //These controller and action need to be present in your project in the project name space

//                if (IsCopiedRequest(filterContext) && SystemSettings.RestrictedCopyURL)
//                {
//                    filterContext.Result = new RedirectToRouteResult(
//                    new System.Web.Routing.RouteValueDictionary
//                    {
//                        //{ "langCode", filterContext.RouteData.Values[ "langCode" ] },
//                        { "controller", "Error" },
//                        { "action", "Restricted" },
//                        { "Restricted", filterContext.HttpContext.Request.RawUrl }
//                    });
//                }
//            }            
//        }

//        public static bool IsCopiedRequest(AuthorizationContext filterContext)
//        {
//            var isCopiedRequest = false;
//            if (filterContext.HttpContext.Request.UrlReferrer == null)
//            {
//                isCopiedRequest = true;
//            }
//            else
//            {
//                if (filterContext.HttpContext.Request.Url.Host != filterContext.HttpContext.Request.UrlReferrer.Host)
//                {
//                    isCopiedRequest = true;
//                }
//            }

//            return isCopiedRequest;
//        }
//    }

//    public class IsValidURLRequestAttribute : AuthorizeAttribute
//    {
//        public override void OnAuthorization(AuthorizationContext filterContext)
//        {            
//            // basic athorization check 
//            //base.OnAuthorization(filterContext);
//            //if (filterContext.HttpContext != null)
//            //{
//            //    //Http referrer check and do the redirection if error occurs
//            //    //It uses a controller named ErrorViewController and action named DisplayHttpReferrerError
//            //    //These controller and action need to be present in your project in the project name space
//            //    if (filterContext.HttpContext.Request.UrlReferrer == null)
//            //    {
//            //        filterContext.Result = new RedirectToRouteResult(
//            //        new System.Web.Routing.RouteValueDictionary
//            //        {
//            //                { "langCode", filterContext.RouteData.Values[ "langCode" ] },
//            //                { "controller", "ErrorView" },
//            //                { "action", "DisplayHttpReferrerError" },
//            //                { "ReturnUrl", filterContext.HttpContext.Request.RawUrl },
//            //        });
//            //    }
//            //}

//            /*Add code here to check the domain name the request come from*/

//            // The call for validation of URL hash and do the redirection if error occurs
//            //It uses a controller named ErrorViewController and action named DisplayURLError
//            //These controller and action need to be present in your project in the project name space
//            //if (TokenUtility.validateToken(filterContext.RequestContext.RouteData.Values, ActionLinkExtensions.tokenPassword) == false)

//            var paramDic = TokenUtility.QueryStringAsRouteValueDictionary(filterContext.RequestContext);
//            //var paramValueCollections = HttpUtility.ParseQueryString(filterContext.RequestContext.HttpContext.Request.QueryString.ToString());
//            if (TokenUtility.IsValidToken(paramDic, SystemSettings.GenerateTokenSecretKey, filterContext) == false)
//            {
//                if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
//                {                                
//                    var isJsonResponse = true;
//                    if(filterContext.RequestContext.HttpContext.Request.Headers != null)
//                    {
//                        if (!string.IsNullOrEmpty(filterContext.RequestContext.HttpContext.Request.Headers["Accept"])){
//                            if (filterContext.RequestContext.HttpContext.Request.Headers["Accept"].Contains("html"))
//                            {
//                                isJsonResponse = false;
//                            }
//                        }                        
//                    }

//                    if (!isJsonResponse)
//                    {
//                        filterContext.HttpContext.Response.StatusCode = 200;
//                        filterContext.Result = new ContentResult {
//                            Content = @"<div style='color:#f4516c !important; padding:30px;'><i class='fa fa-warning'></i> "+ ManagerResource.COMMON_ERROR_DATA_INVALID +"</div>",
//                            ContentEncoding = Encoding.UTF8
//                        };
//                    }
//                    else
//                    {
//                        filterContext.HttpContext.Response.StatusCode = 400;
//                        filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;

//                        filterContext.Result = new JsonResult
//                        {
//                            Data = new { Error = ManagerResource.COMMON_ERROR_DATA_INVALID, StatusCode = 400 },
//                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
//                        };
//                    }
//                }
//                else
//                {
//                    filterContext.Result = new RedirectToRouteResult(
//                    new System.Web.Routing.RouteValueDictionary
//                    {
//                            //{ "langCode", filterContext.RouteData.Values[ "langCode" ] },
//                            { "controller", "Error" },
//                            { "action", "RobotDetected" },
//                            { "ReturnUrl", filterContext.HttpContext.Request.RawUrl }
//                    });
//                }
//            }
//        }
//    }
//}