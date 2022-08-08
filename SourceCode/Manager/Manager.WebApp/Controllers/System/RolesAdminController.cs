using System;
using System.Linq;
using Manager.WebApp.Helpers;
using Manager.WebApp.Resources;
using Manager.SharedLibs;
using Microsoft.Extensions.Logging;
using Manager.WebApp.Models;
using Manager.DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Manager.DataLayer.Stores;
using Manager.DataLayer;
using Autofac;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.WebApp.Controllers
{
    public class RolesAdminController : BaseAuthedController
    {
        private readonly IStoreRole _mainStore;
        private readonly IStoreActivity _activityStore;
        private readonly ILogger<RolesAdminController> _logger;

        public RolesAdminController(ILogger<RolesAdminController> logger)
        {
            _mainStore = Startup.IocContainer.Resolve<IStoreRole>();
            _activityStore = Startup.IocContainer.Resolve<IStoreActivity>();
            _logger = logger;
        }

        [AccessRoleChecker]
        public ActionResult Index()
        {
            var model = new IndexRoleViewModel();
            List<IdentityRole> roles = new List<IdentityRole>();
            model.UserId = GetCurrentUserId();
            try
            {
                roles = _mainStore.GetListByAgencyId(GetCurrentAgencyId());
            }
            catch (Exception ex)
            {
                this.AddNotification("Failed to get data because: " + ex.ToString(), NotificationType.ERROR);
                return View(model);
            }

            model.RoleList = roles;
            return View(model);
        }

        //
        // POST: /RolesAdmin/Create
        [HttpPost]
        [AccessRoleChecker]
        public ActionResult Create(IndexRoleViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var role = new IdentityRole()
                    {
                        Name = model.CurrentRole.Name,
                        UserId = GetCurrentUserId(),
                        AgencyId = GetCurrentAgencyId()
                    };
                    var result = _mainStore.Insert(role);
                    if (result > 0)
                    {
                        this.AddNotification(ManagerResource.LB_INSERT_SUCCESS, NotificationType.SUCCESS);
                        MenuHelper.ClearAllMenuCache();
                    }
                    else
                    {
                        this.AddNotification(ManagerResource.ERROR_EXISTS_ROLE, NotificationType.ERROR);
                    }

                    //Write log
                    var activityText = "Create new role device [Name: {0}]";
                    activityText = string.Format(activityText, model.CurrentRole.Name);
                    WriteActivityLog(activityText, ActivityLogType.CreateRole, model.CurrentRole.Id, TargetObjectType.RolesAdmin);

                    return RedirectToAction("Index", "RolesAdmin");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Could not Create Role because: {0}", ex.ToString()));
            }

            this.AddNotificationModelStateErrors(ModelState);
            return RedirectToAction("Index", "RolesAdmin");
        }

        //
        // POST: /RolesAdmin/Update
        [HttpPost]
        [AccessRoleChecker]
        public ActionResult Update(IndexRoleViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var role = new IdentityRole()
                    {
                        Name = model.CurrentRole.Name,
                        Id = model.CurrentRole.Id,
                        AgencyId = GetCurrentAgencyId()
                    };
                    var result = _mainStore.Update(role);

                    if (result > 0)
                    {
                        this.AddNotification(ManagerResource.LB_UPDATE_SUCCESS, NotificationType.SUCCESS);
                        MenuHelper.ClearAllMenuCache();
                    }
                    else
                    {
                        this.AddNotification(ManagerResource.ERROR_EXISTS_ROLE, NotificationType.ERROR);
                    }
                    //var roleresult =  await RoleManager.UpdateAsync(role);
                    //if (!roleresult.Succeeded)
                    //{
                    //    ModelState.AddModelError("", roleresult.Errors.First());
                    //}
                    //else
                    //{
                    //this.AddNotification("The role [" + role.Name + "] is updated succesffully", NotificationType.SUCCESS);

                    //Write log
                    var activityText = "Updated the role [Name: {0}]";
                    activityText = string.Format(activityText, model.CurrentRole.Name);
                    WriteActivityLog(activityText, ActivityLogType.UpdateRole, model.CurrentRole.Id, TargetObjectType.RolesAdmin);

                    return RedirectToAction("Index", "RolesAdmin");
                    //}
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Could not Create Role because: {0}", ex.ToString()));
            }
            this.AddNotificationModelStateErrors(ModelState);
            return RedirectToAction("Index", "RolesAdmin");
        }

        //Show popup confirm delete
        public ActionResult DeleteRole(string id)
        {
            if (id == null)
            {
                return new NotFoundResult();
            }

            RoleViewModel record = new RoleViewModel();
            record.Id = id;

            return PartialView("_DeleteroleInfo", record);
        }

        [HttpPost, ActionName("DeleteRole")]
        [ValidateAntiForgeryToken]
        [AccessRoleChecker]
        public ActionResult AcceptDeleteRole(string id)
        {
            try
            {
                if (id == null)
                {
                    return Json(new { success = false, message = ManagerResource.LB_ERROR_OCCURED });
                }

                //DB execute
                _mainStore.Delete(id);

                MenuHelper.ClearAllMenuCache();

                //Write log
                var activityText = "Delete the role [Id: {0}]";
                activityText = string.Format(activityText, id);
                WriteActivityLog(activityText, ActivityLogType.DeleteRole, id, TargetObjectType.RolesAdmin);

                return Json(new { success = true, message = ManagerResource.LB_DELETE_SUCCESS, clientcallback = "location.reload();" });
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Failed when Delete Role because: {0}", ex.ToString()));
            }

            return Json(new { success = false, message = ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT });

        }

        #region Helpers

        private void WriteActivityLog(string activityText, ActivityLogType activityType, string targetId, TargetObjectType targetType)
        {
            var logData = new IdentityActivity
            {
                UserId = GetCurrentUserId(),
                ActivityText = activityText,
                ActivityType = activityType.ToString(),
                TargetId = targetId,
                TargetType = targetType.ToString(),
                IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString()
            };

            _activityStore.WriteActivityLog(logData);
        }

        #endregion
    }
}