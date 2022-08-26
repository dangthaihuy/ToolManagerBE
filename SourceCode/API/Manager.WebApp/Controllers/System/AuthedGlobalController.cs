//using System;
//using System.Linq;
//using Manager.WebApp.Helpers;
//using Manager.WebApp.Resources;
//using Manager.SharedLibs;
//using Microsoft.Extensions.Logging;
//using Manager.WebApp.Models;
//using Manager.DataLayer.Entities;
//using Microsoft.AspNetCore.Mvc;
//using Manager.DataLayer.Stores;
//using Manager.DataLayer;
//using Autofac;
//using System.Threading.Tasks;
//using System.Collections.Generic;

//namespace Manager.WebApp.Controllers
//{
//    public class AuthedGlobalController : BaseAuthedController
//    {
//        private readonly ILogger<AuthedGlobalController> _logger;

//        public AuthedGlobalController(ILogger<AuthedGlobalController> logger)
//        {
//            _logger = logger;
//        }

//        [HttpPost]
//        public async Task<ActionResult> GetNotifCount()
//        {
//            var isSuccess = false;
//            var msg = string.Empty;
//            var counter = 0;
//            try
//            {
//                var store = Startup.IocContainer.Resolve<IStoreNotification>();
//                var filter = new IdentityNotification();
//                filter.UserType = (int)EnumNotificationUserType.Manager;
//                filter.UserId = GetCurrentStaffId();

//                counter = store.CountUnread(filter);

//                await Task.FromResult(counter);

//                isSuccess = true;
//            }
//            catch (Exception ex)
//            {
//                var strError = string.Format("Failed when trying GetNotifCount because: {0}", ex.ToString());
//                _logger.LogError(strError);
//            }

//            return Json(new { success = isSuccess, message = msg, data = counter });
//        }

//        [HttpPost]
//        public async Task<ActionResult> GetNotification()
//        {
//            var isSuccess = false;
//            var htmlReturn = string.Empty;
//            List<NotificationItemModel> returnList = new List<NotificationItemModel>();

//            var msg = string.Empty;
//            try
//            {
//                var filter = new IdentityNotification();

//                filter.UserId = GetCurrentStaffId();
//                filter.UserType = (int)EnumNotificationUserType.Manager;
//                filter.IsUpdateView = true;

//                var pageIndex = Utils.ConvertToInt32(HttpContext.Request.Query["Page"]);
//                if (pageIndex == 0)
//                    pageIndex = 1;

//                var pageSize = 20;

//                var store = Startup.IocContainer.Resolve<IStoreNotification>();
//                var listNotif = store.GetByUser(filter, pageIndex, pageSize);

//                await Task.FromResult(listNotif);

//                if (listNotif.HasData())
//                {
//                    foreach (var n in listNotif)
//                    {
//                        var it = new NotificationItemModel { NotifInfo = n };

//                        returnList.Add(it);
//                    }
//                }

//                isSuccess = true;
//                htmlReturn = PartialViewAsString("../Widgets/Items/Notification/_NotificationItems", returnList);
//            }
//            catch (Exception ex)
//            {
//                var strError = string.Format("Failed when trying GetNotification because: {0}", ex.ToString());
//                _logger.LogError(strError);
//            }

//            return Json(new { success = isSuccess, message = msg, html = htmlReturn });
//        }

//        [HttpPost]
//        public async Task<ActionResult> GetDashboardNotification()
//        {
//            var isSuccess = false;
//            var htmlReturn = string.Empty;
//            List<NotificationItemModel> returnList = new List<NotificationItemModel>();
//            var msg = string.Empty;
//            try
//            {
//                var filter = new IdentityNotification();

//                filter.UserId = GetCurrentStaffId();
//                filter.UserType = (int)EnumNotificationUserType.Manager;
//                filter.IsUpdateView = true;

//                var pageIndex = Utils.ConvertToInt32(HttpContext.Request.Query["Page"]);
//                if (pageIndex == 0)
//                    pageIndex = 1;

//                var pageSize = 20;

//                var store = Startup.IocContainer.Resolve<IStoreNotification>();
//                var listNotif = store.GetByUser(filter, pageIndex, pageSize);

//                await Task.FromResult(listNotif);

//                if (listNotif.HasData())
//                {
//                    foreach (var n in listNotif)
//                    {
//                        var it = new NotificationItemModel { NotifInfo = n };

//                        returnList.Add(it);
//                    }
//                }

//                isSuccess = true;
//                if (returnList.HasData())
//                    htmlReturn = PartialViewAsString("../Widgets/Area/Notification/_NotificationList", returnList);
//                else
//                    htmlReturn = "";
//            }
//            catch (Exception ex)
//            {
//                var strError = string.Format("Failed when trying GetDashboardNotification because: {0}", ex.ToString());
//                _logger.LogError(strError);
//            }

//            return Json(new { success = isSuccess, message = msg, html = htmlReturn });
//        }

//        public async Task<ActionResult> ReadNotif(int? id = 0, int? targetId = 0)
//        {
//            var model = new NotificationItemModel();
//            try
//            {
//                var notifId = Utils.ConvertToIntFromNullable(id);
//                if (notifId == 0)
//                    return RedirectToErrorPage();

//                var identity = new IdentityNotification();
//                identity.Id = notifId;
//                identity.UserId = GetCurrentStaffId();
//                identity.UserType = (int)EnumNotificationUserType.Manager;
//                identity.IsRead = true;

//                IdentityNotification info = null;

//                var store = Startup.IocContainer.Resolve<IStoreNotification>();
//                info = store.GetById(notifId);

//                await Task.FromResult(info);

//                if (info != null)
//                {
//                    store.MarkIsRead(identity);

//                    //if (info.TargetType == (int)EnumNotificationTargetType.FactoryOrder)
//                    //{
//                    //    var orderStatus = -1;
//                    //    if (targetId != null && targetId > 0)
//                    //    {
//                    //        var order = HelperFactoryOrder.GetBaseInfo((int)targetId);
//                    //        orderStatus = order.Status;
//                    //    }
//                    //    var link = SecurityHelper.GenerateSecureLink("Factory", "Order", new { id = targetId, orderStatus = orderStatus, SearchExec = "Y" });
//                    //    return Redirect(link);
//                    //}

//                    //if (info.TargetType == (int)EnumNotificationTargetType.GoodsReceipt)
//                    //{
//                    //    var link = SecurityHelper.GenerateSecureLink("Warehouse", "GoodsReceiptDetail", new { id = targetId, SearchExec = "Y" });
//                    //    return Redirect(link);
//                    //}

//                    //if (info.TargetType == (int)EnumNotificationTargetType.MaterialOrder)
//                    //{
//                    //    var link = SecurityHelper.GenerateSecureLink("MaterialOrder", "Index", new { id = targetId, SearchExec = "Y" });
//                    //    return Redirect(link);
//                    //}

//                    //if (info.TargetType == (int)EnumNotificationTargetType.SemiProductOrder)
//                    //{
//                    //    var link = SecurityHelper.GenerateSecureLink("SemiProductOrder", "Index", new { id = targetId, SearchExec = "Y" });
//                    //    return Redirect(link);
//                    //}

//                    //if (info.TargetType == (int)EnumNotificationTargetType.GoodsIssueRequest)
//                    //{
//                    //    var link = SecurityHelper.GenerateSecureLink("TransferRequest", "Index", new { id = targetId, SearchExec = "Y" });
//                    //    return Redirect(link);
//                    //}

//                    //if (info.TargetType == (int)EnumNotificationTargetType.TransferRequest)
//                    //{
//                    //    var link = SecurityHelper.GenerateSecureLink("TransferRequest", "Index", new { id = targetId });
//                    //    return Redirect(link);
//                    //}
//                }
//                else
//                {
//                    return RedirectToErrorPage();
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError("Failed to ReadNotification because: " + ex.ToString());
//            }

//             return RedirectToErrorPage();
//        }
//    }
//}
