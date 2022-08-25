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

namespace Manager.WebApp.Controllers
{
    public class AccessController : BaseAuthedController
    {
        private readonly IStoreAccessRoles _mainStore;
        private readonly IStoreActivity _activityStore;
        private readonly ILogger<AccessController> _logger;

        public AccessController(ILogger<AccessController> logger)
        {
            _mainStore = Startup.IocContainer.Resolve<IStoreAccessRoles>();
            _activityStore = Startup.IocContainer.Resolve<IStoreActivity>();
            _logger = logger;
        }

        [AccessRoleChecker(AdminRequired = true)]
        public ActionResult Index(AccessViewModel model)
        {
            var strError = string.Empty;
            try
            {
                model.AllAccess = _mainStore.GetAllAccess();

                model.AllControllers = Constant.GetAllControllers(Path.Combine(Directory.GetCurrentDirectory(), "~/Controllers"));                ;
            }
            catch (Exception ex)
            {
                strError = string.Format("Could not GetAllAccess because: {0}", ex.ToString());
                _logger.LogError(strError);
                this.AddNotification(strError, NotificationType.ERROR);
            }

           
            return View(model);
        }

        [HttpPost]
        [AccessRoleChecker(AdminRequired = true)]
        public ActionResult Create(AccessViewModel model)
        {
            var result = false;
            if (ModelState.IsValid)
            {
                var strError = string.Empty;
                var accessIdentity = new IdentityAccess { Id = model.AccessId, AccessName = model.AccessName, Description = model.AccessDesc };
                try
                {
                    var isDuplicated = _mainStore.CheckAccessDuplicate(accessIdentity);
                    if (isDuplicated)
                    {
                        this.AddNotification(string.Format("Could not create access due to the access [{0}] is existed", model.AccessName), NotificationType.ERROR);
                        return RedirectToAction("Index", "Access");
                    }

                    result = _mainStore.CreateAccess(accessIdentity);
                    if (result)
                    {
                        this.AddNotification("The access [" + model.AccessName + "] is created succesfully", NotificationType.SUCCESS);

                        //Write log
                        var activityText = "Create new access [Name: {0}]";
                        activityText = string.Format(activityText, model.AccessName);
                        WriteActivityLog(activityText, ActivityLogType.CreateAccess, model.AccessId, TargetObjectType.Access);

                        return RedirectToAction("Index", "Access");
                    }
                    else
                    {
                        this.AddNotification("Could not create access due to database exception occurred", NotificationType.ERROR);
                    }
                }
                catch (Exception ex)
                {
                    strError = string.Format("Could not CreateAccess because: {0}", ex.ToString());
                    _logger.LogError(strError);
                    this.AddNotification(strError, NotificationType.ERROR);
                }
            }

            return RedirectToAction("Index", "Access");
        }

        [HttpPost]
        [AccessRoleChecker(AdminRequired = true)]
        public ActionResult Update(AccessViewModel model)
        {
            var result = false;
            if (ModelState.IsValid)
            {
                var strError = string.Empty;
                var accessIdentity = new IdentityAccess { Id = model.AccessId, AccessName = model.AccessName, Description = model.AccessDesc };
                try
                {
                    var isDuplicated = _mainStore.CheckAccessDuplicate(accessIdentity);
                    if (isDuplicated)
                    {
                        this.AddNotification(string.Format("Could not update due to the access [{0}] is existed", model.AccessName), NotificationType.ERROR);
                        return RedirectToAction("Index", "Access");
                    }

                    result = _mainStore.UpdateAccess(accessIdentity);
                    if (result)
                    {
                        this.AddNotification("The access [" + model.AccessName + "] is updated succesfully", NotificationType.SUCCESS);

                        //Write log
                        var activityText = "Updated the access [Name: {0}]";
                        activityText = string.Format(activityText, model.AccessName);
                        WriteActivityLog(activityText, ActivityLogType.UpdateAccess, model.AccessId, TargetObjectType.Access);

                        return RedirectToAction("Index", "Access");
                    }
                    else
                    {
                        this.AddNotification("Could not update acces due to database exception occurred", NotificationType.ERROR);
                    }
                }
                catch (Exception ex)
                {
                    strError = string.Format("Could not UpdateAccess because: {0}", ex.ToString());
                    _logger.LogError(strError);
                    this.AddNotification(strError, NotificationType.ERROR);
                }
            }

            return RedirectToAction("Index", "Access");
        }

        //Show popup confirm delete
        public ActionResult DeleteAccess(string id)
        {
            if (id == null)
            {
                return new NotFoundResult();
            }

            AccessViewModel record = new AccessViewModel();
            record.AccessId = id;

            return PartialView("_DeleteAccessInfo", record);
        }

        [HttpPost, ActionName("DeleteAccess")]
        [ValidateAntiForgeryToken]
        [AccessRoleChecker(AdminRequired = true)]
        public ActionResult AcceptDeleteAccess(string id)
        {
            var strError = string.Empty;
            var result = false;
            if (id == null)
            {
                return new NotFoundResult();
            }

            result = _mainStore.DeleteAccess(id);
            if (result)
            {
                MenuHelper.ClearAllMenuCache();

                //Write log
                var activityText = "Delete the Access [Id: {0}]";
                activityText = string.Format(activityText, id);
                WriteActivityLog(activityText, ActivityLogType.DeleteAccess, id, TargetObjectType.Access);

                return Json(new { success = true, message = ManagerResource.LB_OPERATION_SUCCESS, clientcallback = "location.reload();" });
            }
            else
            {
                throw new Exception("Failed to delete this access");
            }
        }

        public ActionResult ShowAccessLang(string id)
        {
            ManageAccessLangModel model = new ManageAccessLangModel();
            try
            {
                model.AccessId = id;
                model.Languages = LanguagesProvider.GetListLanguages();
                model.AccessInfo = _mainStore.GetAccessDetail(id);
            }
            catch (Exception ex)
            {
                this.AddNotification("Failed to get data because: " + ex.ToString(), NotificationType.ERROR);
                PartialView("_Detail", model);
            }
            return PartialView("_Detail", model);
        }

        public ActionResult UpdateLang()
        {
            ManageAccessLangModel model = new ManageAccessLangModel();
            var id = Utils.ConvertToInt32(HttpContext.Request.Query["Id"]);
            var accessId = HttpContext.Request.Query["AccessId"].ToString();

            if (string.IsNullOrEmpty(accessId))
            {
                return new NotFoundResult();
            }

            try
            {
                model.Languages = LanguagesProvider.GetListLanguages();
                model.AccessId = accessId;

                //Begin db transaction
                var info = _mainStore.GetAccessLangDetail(id);

                if (info != null)
                {
                    model.AccessId = accessId;
                    model.Id = info.Id;
                    model.LangCode = info.LangCode;
                    model.Description = info.Description;
                }
            }
            catch (Exception ex)
            {
                this.AddNotification(ManagerResource.LB_SYSTEM_BUSY, NotificationType.ERROR);

                _logger.LogError("Failed for Show UpdateLang form request: " + ex.ToString());
            }

            return PartialView("~/Views/Access/_UpdateLang.cshtml", model);
        }

        [HttpPost]
        public ActionResult UpdateLang(ManageAccessLangModel model)
        {
            var msg = ManagerResource.LB_OPERATION_SUCCESS;
            var isSuccess = false;

            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                       .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage + x.Exception));
                this.AddNotification(messages, NotificationType.ERROR);

                return Json(new { success = isSuccess, title = ManagerResource.LB_NOTIFICATION, message = messages });
            }

            try
            {
                var code = 0;

                //Begin db transaction
                var data = new IdentityAccessLang();
                data.Id = model.Id;
                data.AccessId = model.AccessId;
                data.Description = model.Description;
                data.LangCode = model.LangCode;

                if (model.Id > 0)
                {
                    //Update
                    code = _mainStore.UpdateAccessLang(data);

                    if (code == EnumCommonCode.Error)
                    {
                        return Json(new { success = isSuccess, title = ManagerResource.LB_NOTIFICATION, message = ManagerResource.LB_DUPLICATE_DATA, clientcallback = string.Format("ShowAccessLang('{0}')", model.AccessId) });
                    }

                    //Clear cache
                    MenuHelper.ClearAllMenuCache();
                }
                else
                {
                    //Add new
                    code = _mainStore.AddAccessLang(data);

                    if (code == EnumCommonCode.Error)
                    {
                        return Json(new { success = isSuccess, title = ManagerResource.LB_NOTIFICATION, message = ManagerResource.LB_DUPLICATE_DATA, clientcallback = string.Format(" ShowAccessLang('{0}')", model.AccessId) });
                    }

                    //Clear cache
                    MenuHelper.ClearAllMenuCache();
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                this.AddNotification(ManagerResource.LB_SYSTEM_BUSY, NotificationType.ERROR);

                _logger.LogError("Failed for UpdateLang request: " + ex.ToString());

                return Json(new { success = isSuccess, title = ManagerResource.LB_NOTIFICATION, message = ManagerResource.LB_SYSTEM_BUSY });
            }

            return Json(new { success = isSuccess, title = ManagerResource.LB_NOTIFICATION, message = msg, clientcallback = string.Format(" ShowAccessLang('{0}')", model.AccessId) });
        }

        public ActionResult DeleteLang()
        {
            ManageAccessLangModel model = new ManageAccessLangModel();
            var id = Utils.ConvertToInt32(HttpContext.Request.Query["Id"]);
            var accessId = HttpContext.Request.Query["AccessId"].ToString();
            if (string.IsNullOrEmpty(accessId))
            {
                return new NotFoundResult();
            }

            model.AccessId = accessId;
            model.Id = id;

            return PartialView("~/Views/Access/_DeleteLangInfo.cshtml", model);
        }

        [HttpPost, ActionName("DeleteLang")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteLang_Confirm(ManageAccessLangModel model)
        {
            var strError = string.Empty;
            if (model.Id <= 0)
            {
                return new NotFoundResult();
            }

            try
            {
                _mainStore.DeleteAccessLang(model.Id);

                //Clear cache
                MenuHelper.ClearAllMenuCache();
            }
            catch (Exception ex)
            {
                strError = ManagerResource.LB_SYSTEM_BUSY;

                _logger.LogError("Failed to get Delete Access Lang because: " + ex.ToString());

                return Json(new { success = false, message = strError });
            }

            return Json(new { success = true, message = ManagerResource.LB_DELETE_SUCCESS, title = ManagerResource.LB_NOTIFICATION, clientcallback = string.Format(" ShowAccessLang('{0}')", model.AccessId) });
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