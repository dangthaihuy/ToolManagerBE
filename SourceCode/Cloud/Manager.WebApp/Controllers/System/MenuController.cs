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
using System.Collections.Generic;
using Manager.SharedLibs.Extensions;
using Newtonsoft.Json;

namespace Manager.WebApp.Controllers
{
    public class MenuController : BaseAuthedController
    {
        private readonly IStoreAccessRoles _mainStore;
        private readonly IStoreActivity _activityStore;
        private readonly ILogger<MenuController> _logger;

        public MenuController(ILogger<MenuController> logger)
        {
            _mainStore = Startup.IocContainer.Resolve<IStoreAccessRoles>();
            _activityStore = Startup.IocContainer.Resolve<IStoreActivity>();
            _logger = logger;
        }

        [AccessRoleChecker(AdminRequired = true)]
        public ActionResult Index(MenuViewModels model)
        {
            try
            {
                model.AllMenus = _mainStore.GetAllMenus();
            }
            catch (Exception ex)
            {
                this.AddNotification(string.Format("Could not get all menus because: {0}", ex.ToString()), NotificationType.ERROR);
            }

            return View(model);
        }

        public ActionResult Create(int parentid)
        {
            MenuViewModels model = new MenuViewModels();
            model.ParentId = parentid;
            model.Active = true;
            model.Visible = true;
            try
            {
                model.AllOperation = _mainStore.GetListOperationNotUse();
            }
            catch (Exception ex)
            {
                this.AddNotification("Failed to get data because: " + ex.ToString(), NotificationType.ERROR);
                PartialView("_Create", model);
            }
            return PartialView("_Create", model);
        }
        public ActionResult SaveCreate(MenuViewModels model)
        {
            string Controller = null;
            string Action = null;
            if (model.OperationName != null)
            {
                Controller = model.OperationName.Split('/')[0];
                Action = model.OperationName.Split('/')[1];
            }
            int? ParentId = null;
           if (model.ParentId != 0)
            {
                ParentId = model.ParentId;
            }
            try
            {
                _mainStore.InsertMenu(new IdentityMenu
                {
                    ParentId = model.ParentId,
                    Active = model.Active,
                    Controller = Controller,
                    Action = Action,
                    CssClass = model.CssClass,
                    IconCss = model.IconCss,
                    SortOrder = model.SortOrder,
                    Name = model.Name,
                    Visible = model.Visible,
                    Title = model.Title
                });

                MenuHelper.ClearAllMenuCache();

                return Json(new { Success = true, Message = "Thêm mới thành công" });
            }
            catch (Exception ex)
            {
                this.AddNotification("Failed to get data because: " + ex.ToString(), NotificationType.ERROR);
                return Json(new { Success = false, Message = "Thêm mới thất bại" });
            }
        }
        public ActionResult Update(int id)
        {
            MenuViewModels model = new MenuViewModels();
            try
            {
               var menu = _mainStore.GetMenuById(id);
                model.Name = menu.Name;
                model.Active = menu.Active;
                model.CssClass = menu.CssClass;
                model.IconCss = menu.IconCss;
                model.SortOrder = menu.SortOrder??0;
                model.Name = menu.Name;
                model.Visible = menu.Visible;
                model.Title = menu.Title;
                model.ParentId = menu.ParentId??0;
                if (menu.Controller != null&& menu.Controller != "")
                {
                    model.OperationName = menu.Controller + "/" + menu.Action;
                }
                model.AllOperation = _mainStore.GetListOperationNotUse();
            }
            catch (Exception ex)
            {
                this.AddNotification("Failed to get data because: " + ex.ToString(), NotificationType.ERROR);
                PartialView("_Update", model);
            }
            return PartialView("_Update", model);
        }

        public ActionResult SaveUpdate(MenuViewModels model)
        {
            string Controller = null;
            string Action = null;
            if (model.OperationName != null)
            {
                Controller = model.OperationName.Split('/')[0];
                Action = model.OperationName.Split('/')[1];
            }
            int? ParentId = null;
            if (model.ParentId != 0)
            {
                ParentId = model.ParentId;
            }
            try
            {
                _mainStore.UpdateMenu(new IdentityMenu
                {
                    Id=model.Id,
                    ParentId = model.ParentId,
                    Active = model.Active,
                    Controller = Controller,
                    Action = Action,
                    CssClass = model.CssClass,
                    IconCss = model.IconCss,
                    SortOrder = model.SortOrder,
                    Name = model.Name,
                    Visible = model.Visible,
                    Title = model.Title
                });

                MenuHelper.ClearAllMenuCache();

                return Json(new { Success = true, Message = "Cập nhật thành công" });
            }
            catch (Exception ex)
            {
                this.AddNotification("Failed to get data because: " + ex.ToString(), NotificationType.ERROR);
                return Json(new { Success = false, Message = "Cập nhật thất bại" });
            }
        }

        private void ApplyNewSorting(List<SortingElement> sortList, int parentId = 0)
        {
            if (sortList.HasData())
            {
                var beginOrder = 0;
                foreach (var item in sortList)
                {
                    beginOrder++;
                    item.SortOrder = beginOrder;
                    item.ParentId = parentId;

                    if (item.children.HasData())
                    {
                        ApplyNewSorting(item.children, item.id);
                    }
                }
            }
        }

        public ActionResult UpdateSorting(string data)
        {            
            try
            {
                var sortList = new List<SortingElement>();

                if(!string.IsNullOrEmpty(data))
                    sortList = JsonConvert.DeserializeObject<List<SortingElement>>(data);

                if (sortList != null)
                    ApplyNewSorting(sortList);

                //Update in DB
                if (sortList != null)
                    _mainStore.UpdateSorting(sortList);

                //Clear Cache
                MenuHelper.ClearAllMenuCache();
                
                return Json(new { Success = true, Message = "Cập nhật thành công" });
            }
            catch (Exception ex)
            {
                this.AddNotification("Failed to get data because: " + ex.ToString(), NotificationType.ERROR);
                return Json(new { Success = false, Message = "Cập nhật thất bại" });
            }
        }

        //Show popup confirm delete        
        public ActionResult PopupDelete(string id)
        {
            if (id == null)
            {
                return new NotFoundResult();
            }

            MenuViewModels record = new MenuViewModels();
            record.Id = Convert.ToInt32(id);

            return PartialView("_DeleteInfo", record);
        }

        [HttpPost, ActionName("PopupDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundResult();
            }
            try
            {
                int Id = Convert.ToInt32(id);
                _mainStore.DeleteMenu(Id);

                //Clear Cache
                MenuHelper.ClearAllMenuCache();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to Delete menu because: " + ex.ToString());
            }
           
            return Json(new { success = true, clientcallback = "location.reload();" });
        }

        public ActionResult ShowMenuLang(int id)
        {
            ManageMenuLangModel model = new ManageMenuLangModel();
            try
            {
                model.MenuId = id;
                model.Languages = LanguagesProvider.GetListLanguages();
                model.MenuInfo = _mainStore.GetMenuDetail(id);
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
            ManageMenuLangModel model = new ManageMenuLangModel();
            var id = Utils.ConvertToInt32(HttpContext.Request.Query["Id"]);
            var groupId = Utils.ConvertToInt32(HttpContext.Request.Query["MenuId"]);

            if (groupId == 0)
            {
                return new NotFoundResult();
            }

            if (id > 0)
            {
                model.IsUpdate = true;
            }

            try
            {
                model.Languages = LanguagesProvider.GetListLanguages();
                model.MenuId = groupId;

                //Begin db transaction
                var info = _mainStore.GetLangDetail(id);

                if (info != null)
                {
                    model.MenuId = groupId;
                    model.Id = info.Id;
                    model.LangCode = info.LangCode;
                    model.Title = info.Title;
                }
            }
            catch (Exception ex)
            {
                this.AddNotification(ManagerResource.LB_SYSTEM_BUSY, NotificationType.ERROR);

                _logger.LogError("Failed for Show UpdateLang form request: " + ex.ToString());
            }

            return PartialView("~/Views/Menu/_UpdateLang.cshtml", model);
        }

        [HttpPost]
        public ActionResult UpdateLang(ManageMenuLangModel model)
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
                var data = new IdentityMenuLang();
                data.Id = model.Id;
                data.MenuId = model.MenuId;
                data.Title = model.Title;                
                data.LangCode = model.LangCode;

                if (model.Id > 0)
                {
                    //Update
                    code = _mainStore.UpdateLang(data);                   

                    if (code == EnumCommonCode.Error)
                    {
                        return Json(new { success = isSuccess, title = ManagerResource.LB_NOTIFICATION, message = ManagerResource.LB_DUPLICATE_DATA, clientcallback = string.Format(" ShowMenuLang({0})", model.MenuId) });
                    }

                    //Clear cache
                    MenuHelper.ClearAllMenuCache();
                }
                else
                {
                    //Add new
                    code = _mainStore.AddNewLang(data);

                    if (code == EnumCommonCode.Error)
                    {
                        return Json(new { success = isSuccess, title = ManagerResource.LB_NOTIFICATION, message = ManagerResource.LB_DUPLICATE_DATA, clientcallback = string.Format(" ShowMenuLang({0})", model.MenuId) });
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

            return Json(new { success = isSuccess, title = ManagerResource.LB_NOTIFICATION, message = msg, clientcallback = string.Format(" ShowMenuLang({0})", model.MenuId) });
        }

        public ActionResult DeleteLang()
        {
            ManageMenuLangModel model = new ManageMenuLangModel();
            var id = Utils.ConvertToInt32(HttpContext.Request.Query["Id"]);
            var groupId = Utils.ConvertToInt32(HttpContext.Request.Query["MenuId"]);
            if (id <= 0)
            {
                return new NotFoundResult();
            }

            model.MenuId = groupId;
            model.Id = id;

            return PartialView("~/Views/Menu/_DeleteLangInfo.cshtml", model);
        }

        [HttpPost, ActionName("DeleteLang")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteLang_Confirm(ManageMenuLangModel model)
        {
            var strError = string.Empty;
            if (model.Id <= 0)
            {
                return new NotFoundResult();
            }

            try
            {
                _mainStore.DeleteLang(model.Id);

                //Clear cache
                MenuHelper.ClearAllMenuCache();
            }
            catch (Exception ex)
            {
                strError = ManagerResource.LB_SYSTEM_BUSY;

                _logger.LogError("Failed to get Delete Menu Lang because: " + ex.ToString());

                return Json(new { success = false, message = strError });
            }

            return Json(new { success = true, message = ManagerResource.LB_DELETE_SUCCESS, title = ManagerResource.LB_NOTIFICATION, clientcallback = string.Format(" ShowMenuLang({0})", model.MenuId) });
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