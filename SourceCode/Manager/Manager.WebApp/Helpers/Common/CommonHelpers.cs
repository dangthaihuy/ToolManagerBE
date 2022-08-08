using Manager.WebApp.Models;
using System;
using System.IO;
using System.Security.Claims;
using Manager.WebApp.Settings;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Manager.DataLayer.Entities;
using Autofac;
using Manager.DataLayer.Stores;
using Serilog;
using Manager.DataLayer;
using System.Collections.Generic;
using Manager.SharedLibs;

namespace Manager.WebApp.Helpers
{
    public static class CommonHelpers
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(CommonHelpers));

        public static string GetVersionToken()
        {
            return "JZ-VSTK";
        }

        public static string GetCurrentLanguageOrDefault()
        {
            try
            {
                var myObjStr = GetCookie(SystemSettings.CultureKey);
                if (myObjStr != null && !string.IsNullOrEmpty(myObjStr))
                {
                    if (LanguageMessageHandler.IsSupported(myObjStr))
                    {
                        return myObjStr;
                    }
                    else
                    {
                        return LanguageMessageHandler.GetDefaultLanguage();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("GetCurrentLanguageOrDefault error because: {0}", ex.ToString()));
            }

            return LanguageMessageHandler.GetDefaultLanguage();
        }

        public static string GetCookie(string key)
        {
            return System.Web.HttpContext.Current.Request.Cookies[key];
        }

        public static void ClearCookie(string cookieName)
        {
            System.Web.HttpContext.Current.Response.Cookies.Delete(cookieName);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static void SetCookie(string key, object data, int expireInMinutes = 30)
        {
            string myObjectJson = JsonConvert.SerializeObject(data);
            var encryptedStr = Base64Encode(myObjectJson);

            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddMinutes(expireInMinutes);

            System.Web.HttpContext.Current.Response.Cookies.Delete(key);

            System.Web.HttpContext.Current.Response.Cookies.Append(key, encryptedStr, option);
        }

        public static ClaimsPrincipal GetLoggedInUserClaimPricipal()
        {
            return System.Web.HttpContext.Current.User;
        }

        public static IdentityUser GetCurrentUser()
        {
            IdentityUser user = null;

            var principal = GetLoggedInUserClaimPricipal();
            if (principal != null && principal.Identity.IsAuthenticated)
            {
                var userId = principal.GetLoggedInUserId<string>();
                //user = new IdentityUser();

                //user.Id = principal.GetLoggedInUserId<string>();
                //user.UserName = principal.GetLoggedInUserName();
                //user.FullName = principal.GetLoggedInUserDisplayName();

                user = GetUserById(userId);
            }

            return user;
        }

/*        public static List<IdentityRole> GetListRoleByAgencyId(int agencyId)
        {
            var myKey = string.Format("{0}_{1}", "LIST_ROLE_BY_AGENCY", agencyId);
            List<IdentityRole> list = null;

            try
            {
                //Check from cache first
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                list = cacheProvider.Get<List<IdentityRole>>(myKey);

                if (!list.HasData())
                {
                    var myStore = Startup.IocContainer.Resolve<IStoreRole>();
                    list = myStore.GetListByAgencyId(agencyId);

                    if (list.HasData())
                    {
                        //Storage to cache
                        cacheProvider.Set(myKey, list, SystemSettings.DefaultCachingTimeInMinutes);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("GetListRoleByAgencyId error because: {0}", ex.ToString()));
            }

            return list;
        }*/

        public static T GetLoggedInUserId<T>()
        {
            var principal = GetLoggedInUserClaimPricipal();
            if (principal == null)
                return default(T);

            var loggedInUserId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(loggedInUserId, typeof(T));
            }
            else if (typeof(T) == typeof(int) || typeof(T) == typeof(long))
            {
                return loggedInUserId != null ? (T)Convert.ChangeType(loggedInUserId, typeof(T)) : (T)Convert.ChangeType(0, typeof(T));
            }
            else
            {
                return default(T);
            }
        }

        public static string GetLoggedInUserName()
        {
            var principal = GetLoggedInUserClaimPricipal();
            if (principal == null)
                return string.Empty;

            return principal.FindFirstValue(ClaimTypes.Name);
        }

        public static string GetLoggedInUserDisplayName()
        {
            var principal = GetLoggedInUserClaimPricipal();
            if (principal == null)
                return string.Empty;

            return principal.FindFirstValue(ClaimTypes.GivenName);
        }

        public static string GetLoggedInUserEmail()
        {
            var principal = GetLoggedInUserClaimPricipal();
            if (principal == null)
                return string.Empty;

            return principal.FindFirstValue(ClaimTypes.Email);
        }

        public static string MapPath(string path)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), path);
        }

        public static bool CurrentUserIsAdmin()
        {
            try
            {
                var user = GetCurrentUser();
                if (user != null)
                {
                    var currentUserName = user.UserName.ToLower();
                    if (user.UserName == "admin" || user.UserName == "bangvl")
                        return true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Check CurrentUserIsAdmin error because: {0}", ex.ToString()));
            }

            return false;
        }

        public static bool CurrentUserIsAgency()
        {
            try
            {                
                var user = GetCurrentUser();
                if (user != null)
                {
                    if (user.ParentId == 0)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Check CurrentUserIsAgency error because: {0}", ex.ToString()));
            }

            return false;
        }

        public static IdentityUser GetUserById(string userId)
        {
            var myKey = string.Format("{0}_{1}", "USER", userId);
            IdentityUser info = null;

            try
            {
                //Check from cache first
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();

                info = cacheProvider.Get<IdentityUser>(myKey);

                if (info == null)
                {
                    var myStore = Startup.IocContainer.Resolve<IStoreUser>();
                    info = myStore.GetById(userId);

                    //Storage to cache
                    if (info != null)
                        cacheProvider.Set(myKey, info, SystemSettings.DefaultCachingTimeInMinutes);
                }

            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("GetUserById error because: {0}", ex.ToString()));
            }

            return info;
        }

        public static void ClearUserCache(IdentityUser currentUser)
        {
            try
            {
                var staffKey = string.Format("{0}_{1}", "USER", currentUser.StaffId);
                var idKey = string.Format("{0}_{1}", "USER", currentUser.Id);

                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();

                cacheProvider.Clear(staffKey);
                cacheProvider.Clear(idKey);
            }
            catch (Exception ex)
            {
                _logger.Error("Could not ClearUserCache: " + ex.ToString());
            }
        }

/*        public static void ClearListRoleCache(int agencyId)
        {
            try
            {
                var agencyKey = string.Format("{0}_{1}", "LIST_ROLE_BY_AGENCY", agencyId);

                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();

                cacheProvider.Clear(agencyKey);
            }
            catch (Exception ex)
            {
                _logger.Error("Could not ClearListRoleCache: " + ex.ToString());
            }
        }*/
    }
}
