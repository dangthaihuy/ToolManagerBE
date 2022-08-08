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
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

namespace VnCompany.WebApp.Controllers
{
    public class MyAccountController : BaseAuthedController
    {
        private readonly IStoreAccessRoles _mainStore;
        private readonly IStoreActivity _activityStore;
        private readonly ILogger<MyAccountController> _logger;

        public MyAccountController(ILogger<MyAccountController> logger)
        {
            _mainStore = Startup.IocContainer.Resolve<IStoreAccessRoles>();
            _activityStore = Startup.IocContainer.Resolve<IStoreActivity>();
            _logger = logger;
        }

        //[AccessRoleChecker]
        public ActionResult Profile()
        {
            AccountDetailViewModel model = new AccountDetailViewModel();
            string userId = GetCurrentUserId();

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Account", "Login");

            try
            {
                var userInfo = GetCurrentUser();

                if(userInfo != null)
                {
                    model = userInfo.MappingObject<AccountDetailViewModel>();

                    var roleStore = Startup.IocContainer.Resolve<IStoreRole>();

                    var userRoles = roleStore.GetRolesByUserId(userId);

                    if (userRoles.HasData())
                    {
                        model.RolesList = userRoles.Select(x => x.Name).ToList();
                    }
                }

                //int currentPage = 1;
                ////Limit activity on once query.
                //int pageSize = int.Parse(ConfigurationManager.AppSettings["Paging:PageSize"]);
                //int total = 0;

                //model.ActivityNews = _activityStore.GetActivityLogByUserId(currentUser, currentPage, pageSize);
                //total = _activityStore.CountAllActivityLogByUserId(currentUser);

                //model.ActivityPagingInfo = new PagingInfo
                //{
                //    CurrentPage = currentPage,
                //    //PageNo = (int)(total / pageSize),
                //    PageNo = (total + pageSize - 1) / pageSize,
                //    PageSize = pageSize,
                //    Total = total
                //};

                //if (model.ActivityNews != null && model.ActivityNews.Count > 0)
                //{
                //    foreach (var record in model.ActivityNews)
                //    {
                //        //Calculate time
                //        record.FriendlyRelativeTime = DateTimeHelper.GetFriendlyRelativeTime(record.ActivityDate);
                //    }
                //}
            }
            catch(Exception ex)
            {
                _logger.LogError("Could not display Profile page because: {0}", ex.ToString());
            }

            return View(model);
        }

        [AllowAnonymous]
        //public JsonResult GetActivityLogs(string page)
        //{
        //    List<ActivityLog> list = new List<ActivityLog>();
        //    string currentUser = System.Web.HttpContext.Current.User.Identity.GetUserId();
        //    try
        //    {
        //        list = _activityStore.GetActivityLogByUserId(currentUser, Convert.ToInt32(page), AdminSettings.PageSize);
        //        if (list != null && list.Count > 0)
        //        {
        //            foreach (var record in list)
        //            {
        //                //Calculate time
        //                record.FriendlyRelativeTime = DateTimeHelper.GetFriendlyRelativeTime(record.ActivityDate);
        //            }
        //        }
        //    }
        //    catch
        //    {
        //    }
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}

        //Show change password form
        //[AccessRoleChecker]
        public ActionResult ChangePassword()
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            AccountChangePasswordViewModel model = new AccountChangePasswordViewModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(AccountChangePasswordViewModel model)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                    return RedirectToAction("Account", "Login");

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                model.OldPassword = Utility.Md5HashingData(model.OldPassword);
                model.NewPassword = Utility.Md5HashingData(model.NewPassword);

                //Change password
                var userStore = Startup.IocContainer.Resolve<IStoreUser>();

                var result = userStore.ChangePassword(new IdentityUser { Id = userId, PasswordHash = model.NewPassword });

                this.AddNotification(ManagerResource.LB_CHANGE_PWD_SUCCESS, NotificationType.SUCCESS);

                return RedirectToAction("Profile", "MyAccount");
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not ChangePassword because: {0}", ex.ToString());
            }

            return View(model);
        }

        //public ActionResult UpdateProfile()
        //{
        //    var model = new UpdateUserProfileModel();
        //    try
        //    {
        //        if (Request.IsAuthenticated)
        //        {
        //            model.UserId = User.Identity.GetUserId();
        //            model.UserName = User.Identity.GetUserName();

        //            var claims = (ClaimsIdentity)User.Identity;
        //            model.FullName = claims.FindFirstValue(ClaimKeys.FullName);
        //            model.Avatar = claims.FindFirstValue(ClaimKeys.Avatar);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.AddNotification("System is busy now. Please try again later", NotificationType.ERROR);
        //        var strError = string.Format("Could not open page UpdateProfile because: {0}", ex.ToString());
        //        logger.Error(strError);

        //        return View(model);
        //    }

        //    return View(model);
        //}

        [HttpPost]
        public async Task<ActionResult> UploadAvatar(AccountUpdateAvatarModel model)
        {
            var isSuccess = false;
            var msg = string.Empty;
            var fileName = string.Empty;
            var contentType = "image/jpeg";
            try
            {
                var currentUser = GetCurrentUser();
                var staff_id = currentUser.StaffId;
                var oldPath = currentUser.Avatar;

                var changed = string.Empty;
                SlimObject slimObj = null;

                if (!string.IsNullOrEmpty(model.Base64ImageStr))
                {
                    slimObj = JsonConvert.DeserializeObject<SlimObject>(model.Base64ImageStr);
                }

                if (slimObj == null)
                {
                    return Json(new { success = isSuccess, message = ManagerResource.COMMON_ERROR_DATA_INVALID });
                }

                if (string.IsNullOrEmpty(model.ChangingDetected))
                {
                    isSuccess = true;
                    msg = ManagerResource.LB_UPDATE_SUCCESS;

                    return Json(new { success = true, message = msg });
                }

                if (slimObj.input != null && slimObj.input != null)
                {
                    if (!string.IsNullOrEmpty(slimObj.input.name))
                    {
                        var ext = System.IO.Path.GetExtension(slimObj.input.name);
                        fileName = Utility.Md5HashingData(slimObj.input.name) + ext;
                    }
                    else
                        fileName = string.Format("{0}_{1}", staff_id, EpochTime.GetIntDate(DateTime.Now));

                    if (!string.IsNullOrEmpty(slimObj.input.type))
                        contentType = slimObj.input.type;
                }

                if (slimObj.output != null && !string.IsNullOrEmpty(slimObj.output.image))
                {
                    Regex r = new Regex("data:image/\\w+;base64,");
                    var imageBase64Str = r.Replace(slimObj.output.image, string.Empty);
                    var imgDataBytes = Convert.FromBase64String(imageBase64Str);
                    if (imgDataBytes != null)
                    {
                        var file = FileUploadHelper.Base64ToImage(imageBase64Str, fileName);
                        if (file != null)
                        {
                            var newPath = FileUploadHelper.UploadImageAsync(file, string.Format("Avatars/{0}", currentUser.StaffId)).Result;

                            await Task.FromResult(newPath);

                            if (!string.IsNullOrEmpty(newPath))
                            {
                                currentUser.Avatar = newPath;

                                var store = Startup.IocContainer.Resolve<IStoreUser>();
                                store.UpdateAvatar(currentUser);

                                CommonHelpers.ClearUserCache(currentUser);
                                try
                                {
                                    if (!string.IsNullOrEmpty(oldPath))
                                    {                                       
                            /*            var delPath = Path.Combine(Directory.GetCurrentDirectory(), "Media" + "/" + oldPath);*/
                                        var delPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/" + oldPath);
                                        System.IO.File.Delete(delPath);
                                    }

                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError("Failed when Delete old image because: {0}", ex.ToString());

                                }

                                return Json(new { success = true, message = ManagerResource.LB_UPDATE_SUCCESS });
                            }
                        }
                        else
                        {
                            _logger.LogError("Failed to get Upload image because: The image is null");
                        }
                    }
                }

                return Json(new { success = true, message = ManagerResource.LB_UPDATE_SUCCESS });
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed when trying UploadAvatar because: {0}", ex.ToString());
                _logger.LogError(strError);

                msg = ManagerResource.COMMON_ERROR_EXTERNALSERVICE_TIMEOUT;
            }

            return Json(new { success = isSuccess, message = msg });
        }
    }

    public class SlimInput
    {
        public string name { get; set; }
        public string type { get; set; }
        public string size { get; set; }
        public string width { get; set; }
        public string height { get; set; }
    }
    public class SlimOutput
    {
        public string width { get; set; }
        public string height { get; set; }
        public string image { get; set; }
    }
    public class SlimCrop
    {
        public string x { get; set; }
        public string y { get; set; }
        public string height { get; set; }
        public string width { get; set; }
        public string type { get; set; }
    }
    public class SlimSize
    {
        public string width { get; set; }
        public string height { get; set; }
    }

    public class SlimActions
    {
        public SlimCrop crop { get; set; }
        public SlimSize size { get; set; }
    }

    public class SlimObject
    {
        public object server { get; set; }
        public object meta { get; set; }
        public SlimInput input { get; set; }
        public SlimOutput output { get; set; }
        public SlimActions actions { get; set; }
    }
}