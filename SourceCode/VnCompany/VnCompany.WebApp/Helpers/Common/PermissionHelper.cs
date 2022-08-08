using System.Web;
using Autofac;
using System.Collections.Generic;
using System;
using VnCompany.SharedLibs;
using VnCompany.DataLayer.Stores;
using VnCompany.DataLayer.Entities;

namespace VnCompany.WebApp.Helpers
{
    public static class PermissionHelper
    {
        private static readonly string ALL_PERMISSIONS_KEY = "PRIVILEGES_";

        public static bool CheckPermission(string actionName = "", string controllerName = "", string currentUserId = "")
        {
            var hasPermission = false;
            var currentUser = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(actionName))
                    actionName = HttpContext.Current.Request.RouteValues["action"]?.ToString();

                if (string.IsNullOrEmpty(controllerName))
                    controllerName = HttpContext.Current.Request.RouteValues["controller"]?.ToString();

                if (!string.IsNullOrEmpty(currentUser))
                {
                    var userInfo = CommonHelpers.GetCurrentUser();
                    if (userInfo != null)
                    {
                        if (userInfo.ParentId == 0)
                        {
                            return true;
                        }
                    }
                }

                List<IdentityPermission> allPermissions = new List<IdentityPermission>();
                if (string.IsNullOrEmpty(currentUserId))
                    allPermissions = GetAllPermission();
                else
                    allPermissions = GetAllPermissionDirectly(currentUserId);

                if (allPermissions.HasData())
                {
                    hasPermission = allPermissions.Exists(m => string.Equals(m.Action, actionName, StringComparison.CurrentCultureIgnoreCase)
                   && (string.Equals(m.Controller, controllerName, StringComparison.CurrentCultureIgnoreCase)));
                }

            }
            catch (Exception ex)
            {
                var strError = string.Format("Could not CheckPermission of user [{0}] because: {1}", currentUser, ex.ToString());

                throw;
            }

            return hasPermission;
        }

        public static List<IdentityPermission> GetAllPermission()
        {            
            List<IdentityPermission> perList = null;
            try
            {
                var currentUser = CommonHelpers.GetCurrentUser();
                if(currentUser != null)
                {
                    var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                    var myKey = ALL_PERMISSIONS_KEY + currentUser.Id;

                    //Check from cache first
                    perList = cacheProvider.Get<List<IdentityPermission>>(myKey);

                    //Has data from cache
                    if (perList == null)
                    {
                        var userStore = Startup.IocContainer.Resolve<IStoreUser>();

                        perList = userStore.GetPermissionsByUser(currentUser.Id);

                        if (perList != null && perList.Count > 0)
                        {
                            //Set to cache
                            cacheProvider.Set(myKey, perList);
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                var strError = string.Format("Could not GetAllPermission because: {0}", ex.ToString());
                throw;
            }

            return perList;
        }

        public static List<IdentityPermission> GetAllPermissionDirectly(string currentUserId)
        {
            List<IdentityPermission> perList = null;
            try
            {
                var currentAction = HttpContext.Current.Request.RouteValues["action"]?.ToString();
                var currentController = HttpContext.Current.Request.RouteValues["controller"]?.ToString();

                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                var myKey = ALL_PERMISSIONS_KEY + currentUserId;

                //Check from cache first
                perList = cacheProvider.Get<List<IdentityPermission>>(myKey);

                //Has data from cache
                if (perList == null)
                {
                    var userStore = Startup.IocContainer.Resolve<IStoreUser>();

                    perList = userStore.GetPermissionsByUser(currentUserId);
                    if (perList.HasData())
                    {
                        //Write to cache
                        cacheProvider.Set(myKey, perList);
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Could not GetAllPermission of user [{0}] because: {1}", currentUserId, ex.ToString());
                throw;
            }

            return perList;
        }

        public static void ClearPermissionsCache(string userId)
        {
            var strError = string.Empty;
            try
            {
                var cacheKey = ALL_PERMISSIONS_KEY + userId;
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                cacheProvider.Clear(cacheKey);
            }
            catch (Exception ex)
            {
                strError = string.Format("Failed ClearUserCache: {0}", ex.ToString());
                throw;
            }
        }
    }
}