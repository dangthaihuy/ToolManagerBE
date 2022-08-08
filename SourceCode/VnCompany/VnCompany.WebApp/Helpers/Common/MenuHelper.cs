using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using VnCompany.DataLayer.Stores;
using VnCompany.DataLayer;
using VnCompany.SharedLibs;

namespace VnCompany.WebApp.Helpers
{
    public class MenuHelper
    {
        const string CST_ALL_MENU_ITEMS_PREFIX = "MNU_ALL_MENU_ITEMS";
        const string CST_ALL_PERMISSIONS_PREFIX = "PRIVILEGES_";
        const string CST_ALL_MENU_ITEMS_PATTERN = "MNU_ALL_MENU_ITEMS.{0}";

        public static void ClearAllMenuCache()
        {
            var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();

            //Clear all menu
            cacheProvider.ClearByPrefix(CST_ALL_MENU_ITEMS_PREFIX);

            //Clear all permissions
            cacheProvider.ClearByPrefix(CST_ALL_PERMISSIONS_PREFIX);
        }

        public static void ClearUserMenuCache(string currentUser)
        {
            string menuCacheKey = string.Format(CST_ALL_MENU_ITEMS_PATTERN, currentUser);
            var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
            cacheProvider.Clear(menuCacheKey);
            //cacheProvider.ClearAll(CST_ALL_MENU_ITEMS_PREFIX);
        }

        public static IEnumerable<IdentityMenu> GetAdminNavigationMenuItems()
        {
            IEnumerable<IdentityMenu> result = null;

            var currentUser = CommonHelpers.GetCurrentUser();
            if (currentUser != null)
            {
                var cacheKeyByUser = string.Format(CST_ALL_MENU_ITEMS_PATTERN, currentUser.Id);
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();

                result = cacheProvider.Get<IEnumerable<IdentityMenu>>(cacheKeyByUser);
                if (result == null)
                {
                    var menuItems = GetMenuItemsFromDatabase(currentUser.Id);
                    if (menuItems != null && menuItems.Count > 0)
                    {
                        result = menuItems.OrderBy(m => m.SortOrder);
                        cacheProvider.Set(cacheKeyByUser, result);
                    }
                }
            }

            return result;
        }

        public static List<IdentityMenu> GetAdminMenus()
        {
            var currentUser = CommonHelpers.GetCurrentUser();
            List<IdentityMenu> result = null;

            if (currentUser != null)
            {
                var cacheKeyByUser = string.Format(CST_ALL_MENU_ITEMS_PATTERN, currentUser.Id);
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();

                result = cacheProvider.Get<List<IdentityMenu>>(cacheKeyByUser);
                if (result == null)
                {
                    if (result == null)
                    {
                        var allMenus = GetAllMenuFromDatabase();
                        if (allMenus.HasData())
                        {
                            //Take root menus only
                            allMenus = allMenus.Where(x => x.ParentId == 0).ToList();

                            result = new List<IdentityMenu>();
                            var permissions = PermissionHelper.GetAllPermission();
                            var hasPerData = permissions.HasData();
                            if (hasPerData)
                            {
                                foreach (var item in allMenus)
                                {
                                    //Has sub menus
                                    if (item.SubMenu.HasData())
                                    {
                                        var subMenus = item.SubMenu.OrderBy(x => x.SortOrder).ToList();

                                        var m = item.DeepCopy();
                                        m.SubMenu = new List<IdentityMenu>();

                                        foreach (var s in subMenus)
                                        {
                                            var sp = permissions.Where(
                                                x => x.Controller.Equals(s.Controller, StringComparison.CurrentCultureIgnoreCase)
                                                && x.Action.Equals(s.Action, StringComparison.OrdinalIgnoreCase))
                                            .FirstOrDefault();
                                            if (sp != null)
                                            {
                                                //Add sub menu which has permission
                                                m.SubMenu.Add(s);
                                            }
                                            else
                                            {
                                                if (!s.CheckPermission)
                                                {
                                                    //Add sub menu doesn't check permission
                                                    m.SubMenu.Add(s);
                                                }
                                            }
                                        }

                                        if (m.SubMenu.HasData())
                                            result.Add(m);
                                    }
                                    else //Has no sub menu
                                    {
                                        //Menu doesn't check permission
                                        if (!item.CheckPermission)
                                            result.Add(item);
                                        else
                                        {
                                            var p = permissions.Where(
                                               x => x.Controller.Equals(item.Controller, StringComparison.CurrentCultureIgnoreCase)
                                               && x.Action.Equals(item.Action, StringComparison.OrdinalIgnoreCase))
                                           .FirstOrDefault();

                                            if (p != null)
                                                result.Add(item);
                                        }
                                    }

                                }
                            }
                        }

                        if (result.HasData())
                        {
                            result = result.OrderBy(x => x.SortOrder).ToList();

                            //Save to cache
                            cacheProvider.Set(cacheKeyByUser, result);
                        }
                    }
                }
            }

            

            return result;
        }

        private static List<IdentityMenu> GetMenuItemsFromDatabase(string userId)
        {

            List<IdentityMenu> _listMenu = null;

            var userStore = Startup.IocContainer.Resolve<IStoreUser>();

            try
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    _listMenu = userStore.GetRootMenuByUserId(userId);
                    if (_listMenu != null && _listMenu.Count > 0)
                    {
                        foreach (var parentItem in _listMenu)
                        {
                            MenuItemFindAllChildren(parentItem, userId);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }

            return _listMenu;
        }

        public static void MenuItemFindAllChildren(IdentityMenu parentItem, string userId)
        {
            var userStore = Startup.IocContainer.Resolve<IStoreUser>();
            try
            {
                List<IdentityMenu> childMenus = userStore.GetChildMenuByUserId(userId, parentItem.Id).ToList();

                parentItem.SubMenu = childMenus;
                if (childMenus != null && childMenus.Count > 0)
                {
                    foreach (IdentityMenu childInfo in childMenus)
                    {
                        MenuItemFindAllChildren(childInfo, userId);
                    }
                }
            }
            catch
            {

            }
        }

        public static List<IdentityMenu> GetCurrentListMenuByLang(List<IdentityMenu> allMenus)
        {
            try
            {
                var currentLangCode = CommonHelpers.GetCurrentLanguageOrDefault();
                if (allMenus.HasData())
                {
                    foreach (var item in allMenus)
                    {
                        var langItem = item.LangList.Where(x => x.MenuId == item.Id && x.LangCode == currentLangCode).FirstOrDefault();
                        if (langItem != null)
                            item.CurrentTitleLang = langItem.Title;
                        else
                            item.CurrentTitleLang = item.Title;

                        if (item.SubMenu != null && item.SubMenu.Count() > 0)
                            item.SubMenu = GetCurrentListMenuByLang(item.SubMenu);
                    }
                }
            }
            catch
            {
            }

            return allMenus;
        }
                
        //private static MenuItem CheckCurrentMenuItemInCollections(IEnumerable<MenuItem> menuItems, string actionName, string controllerName)
        //{
        //    MenuItem result = null;
        //    foreach (var mi in menuItems)
        //    {
        //        if (CheckCurrentAction(mi, actionName, controllerName))
        //        {
        //            //Check current item
        //            result = mi;
        //            break;
        //        }

        //        if (mi.SubMenu != null && mi.SubMenu.Any())
        //        {
        //            //Find in subMenu
        //            foreach (var smi in mi.SubMenu)
        //            {
        //                var tempItem = CheckCurrentMenuItemInCollections(mi.SubMenu, actionName, controllerName);
        //                if (tempItem != null)
        //                {
        //                    result = tempItem;
        //                    break;
        //                }
        //            }

        //            //Found in subMenu
        //            if (result != null)
        //            {
        //                break;
        //            }
        //        }
        //    }
        //    return result;
        //}

        //private static bool CheckCurrentAction(MenuItem mi, string actionName, string controllerName)
        //{
        //    if (
        //        !string.IsNullOrEmpty(mi.Action)
        //        && !string.IsNullOrEmpty(mi.Controller)
        //        && !string.IsNullOrEmpty(actionName)
        //        && !string.IsNullOrEmpty(controllerName)
        //        && mi.Action.Equals(actionName, StringComparison.InvariantCultureIgnoreCase)
        //        && mi.Controller.Equals(controllerName, StringComparison.InvariantCultureIgnoreCase)
        //        )
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        //public static bool CheckCurrentAction(MenuItem mi)
        //{
        //    if (HttpContext.Current == null)
        //        return false;

        //    string controllerName = null;
        //    string actionName = null;

        //    var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;
        //    if (routeValues != null)
        //    {
        //        if (routeValues.ContainsKey("action"))
        //        {
        //            actionName = routeValues["action"].ToString();
        //        }
        //        if (routeValues.ContainsKey("controller"))
        //        {
        //            controllerName = routeValues["controller"].ToString();
        //        }
        //    }

        //    return CheckCurrentAction(mi, actionName, controllerName);
        //}

        //public static bool CheckCurrentGroup(MenuItem gmi)
        //{
        //    if (CheckCurrentAction(gmi))
        //    {
        //        return true;
        //    }

        //    if (gmi.SubMenu != null && gmi.SubMenu.Any())
        //    {
        //        foreach (var smi in gmi.SubMenu)
        //        {
        //            if (CheckCurrentGroup(smi))
        //                return true;
        //        }
        //    }

        //    return false;
        //}

        private static List<IdentityMenu> GetAllMenuFromDatabase()
        {

            List<IdentityMenu> allMenus = null;

            var userStore = Startup.IocContainer.Resolve<IStoreUser>();

            try
            {
                allMenus = userStore.GetAllDislayMenu();
                if (allMenus.HasData())
                {
                    foreach (var p in allMenus)
                    {
                        p.SubMenu = allMenus.Where(x => x.ParentId == p.Id).ToList();
                    }
                }
            }
            catch(Exception ex)
            {
                //logger.ErrorFormat("Could not GetAllMenuFromDatabase because: {0}", ex.ToString());
                throw;
            }

            return allMenus;
        }

        private static bool CheckCurrentActionCustom(IdentityMenu mi, string actionName, string controllerName, bool ignoreAction = false)
        {
            var result = false;
            if (
                !string.IsNullOrEmpty(mi.Action)
                && !string.IsNullOrEmpty(mi.Controller)
                && !string.IsNullOrEmpty(actionName)
                && !string.IsNullOrEmpty(controllerName)
                && mi.Action.Equals(actionName, StringComparison.InvariantCultureIgnoreCase)
                && mi.Controller.Equals(controllerName, StringComparison.InvariantCultureIgnoreCase)
                )
            {
                result = true;
            }
            if (ignoreAction)
            {
                if (result == false
                    && !string.IsNullOrEmpty(mi.Controller)
                    && !string.IsNullOrEmpty(controllerName)
                    && mi.Controller.Equals(controllerName, StringComparison.InvariantCultureIgnoreCase))
                {
                    result = true;
                }
            }
            return result;
        }

        public static bool CheckCurrentGroupCustom(IdentityMenu gmi, string actionName, string controllerName, bool igoreAction = false)
        {
            var result = false;
            if (CheckCurrentActionCustom(gmi, actionName, controllerName, igoreAction))
            {
                result = true;
            }

            if (gmi.SubMenu != null && gmi.SubMenu.Count > 0)
            {
                foreach (var smi in gmi.SubMenu)
                {
                    if (CheckCurrentGroupCustom(smi, actionName, controllerName))
                    {
                        result = true;
                    }
                }

                if (result == false)
                {
                    foreach (var smi in gmi.SubMenu)
                    {
                        if (CheckCurrentGroupCustom(smi, actionName, controllerName, true))
                        {
                            result = true;
                        }
                    }
                }
            }
            return result;
        }
    }
}