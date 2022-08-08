using System;
using System.Linq;
using VnCompany.WebApp.Helpers;
using VnCompany.WebApp.Resources;
using VnCompany.SharedLibs;
using Microsoft.Extensions.Logging;
using VnCompany.WebApp.Models;
using VnCompany.DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using VnCompany.DataLayer.Stores;
using VnCompany.DataLayer;
using Autofac;
using System.Threading.Tasks;

namespace VnCompany.WebApp.Controllers
{
    public class AccessRolesController : BaseAuthedController
    {
        private readonly IStoreRole _roleStore;
        private readonly IStoreAccessRoles _accessStore;
        private readonly IStoreActivity _activityStore;
        private readonly ILogger<AccessRolesController> _logger;

        public AccessRolesController(ILogger<AccessRolesController> logger)
        {
            _roleStore = Startup.IocContainer.Resolve<IStoreRole>();
            _accessStore = Startup.IocContainer.Resolve<IStoreAccessRoles>();
            _activityStore = Startup.IocContainer.Resolve<IStoreActivity>();
            _logger = logger;
        }

        // GET: AccessRoles
        //[AccessRoleChecker(ControllerName = "AccessRoles", ActionName = "Index")]
        [AccessRoleChecker]
        public ActionResult Index()
        {
            AccessRolesViewModel model = new AccessRolesViewModel();
            string roleId = null;
            if (!string.IsNullOrEmpty(HttpContext.Request.Query["RoleId"]))
            {
                roleId = HttpContext.Request.Query["RoleId"];
            }

            try
            {
                var agencyId = GetCurrentAgencyId();

                model.AllRoles = _roleStore.GetListByAgencyId(agencyId);

                if (model.AllRoles.HasData())
                {
                    model.AllRoles.OrderBy(m => m.Name).ToList();
                }

                model.AllOperations = _accessStore.GetAllOperations();

                if (!string.IsNullOrEmpty(roleId))
                {
                    //List access by role
                    //model.AccessList = _accessStore.GetAccessByRoleId(roleId);
                    //model.AccessList = _accessStore.GetAccessByRoleId(roleId);
                    //if (model.AccessList != null && model.AccessList.Count > 0)
                    //{
                    //    foreach (var acc in model.AccessList)
                    //    {
                    //        acc.OperationsList = _accessStore.GetOperationsByAccessId(acc.AccessId).ToList();
                    //    }
                    //}
                    model.Menus = MenuHelper.GetAdminNavigationMenuItems().ToList();

                    model.RoleId = roleId;
                    model.PermissionsList = _accessStore.GetPermissionByRoleId(roleId);                   

                    //All access
                    model.AllAccess = _accessStore.GetAllAccess();
                    //if (model.AllAccess != null && model.AllAccess.Count > 0)
                    //{
                    //    foreach (var acc in model.AllAccess)
                    //    {
                    //        acc.OperationsList = _accessStore.GetOperationsByAccessId(acc.Id).ToList();
                    //    }
                    //}
                }
            }
            catch(Exception ex) 
            {
                this.AddNotification("Failed to get data: " + ex.ToString(), NotificationType.ERROR);
                return View(model);
            }
            
            return View(model);
        }

        //
        // POST: /UpdateAccessRoles
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessRoleChecker]
        public async Task<ActionResult> UpdateAccessRoles(string roleId, params string[] selectedOperations)
        {
            AccessRolesViewModel model = new AccessRolesViewModel();
            try
            {
                var roleInfo = _roleStore.GetById(roleId);

                await Task.FromResult(roleInfo);

                if (roleInfo == null)
                {
                    this.AddNotification("This role not found", NotificationType.ERROR);
                    return RedirectToAction("Index", model);
                }

                selectedOperations = selectedOperations ?? new string[] { };

                model.RoleId = roleId;
                //Remove all access roles
                _accessStore.DeleteAllAccessOfRole(roleId);

                //Update new access roles
                bool isUpdated = _accessStore.UpdateAccessOfRole(selectedOperations, roleId);

                if (isUpdated)
                {

                    MenuHelper.ClearAllMenuCache();

                    this.AddNotification(ManagerResource.LB_UPDATE_SUCCESS, NotificationType.SUCCESS);
                }
                else
                    this.AddNotification("Failed to update aceess roles: Please recheck the query and you connection", NotificationType.ERROR);
            }
            catch (Exception ex)
            {
                this.AddNotification("Failed to update aceess roles: " + ex.InnerException.Message.ToString(), NotificationType.ERROR);
            }

            model.RoleId = roleId;
            this.AddNotificationModelStateErrors(ModelState);
            return RedirectToAction("Index",model);
        }
        
        [AccessRoleChecker]
        public ActionResult ManageAccess(AccessViewModel model)
        {
            model.AllAccess = _accessStore.GetAllAccess();
            return View(model);
        }
    }
}