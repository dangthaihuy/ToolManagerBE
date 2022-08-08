using Autofac;
using VnCompany.DataLayer.Entities.Business;
using VnCompany.DataLayer.Stores.Business;
using VnCompany.SharedLibs;
using VnCompany.WebApp.Helpers;
using VnCompany.WebApp.Models.Business;
using VnCompany.WebApp.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VnCompany.WebApp.Helpers.Business;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace VnCompany.WebApp.Controllers.Business
{
    public class EmployeeController : BaseAuthedController
    {
        private readonly IStoreEmployee _mainStore;
        private readonly IStoreContact _contactStore;
        private readonly ILogger<EmployeeController> _logger;


        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _mainStore = Startup.IocContainer.Resolve<IStoreEmployee>();
            _contactStore = Startup.IocContainer.Resolve<IStoreContact>();
            _logger = logger;

            //Clear cache
            CachingHelpers.ClearUserCache();
            CommonHelpers.ClearCookie(CommonHelpers.GetVersionToken());
        }
    
    
        public ActionResult Index(EmployeeManagerModel model)
        {
            model = GetDefaultFilterModel(model);
            try
            {
                var filter = new IdentityEmployee
                {
                    Keyword = model.Keyword,                    
                };

                GetCompanyIdOfEmployee(filter);

                var results = _mainStore.GetByPage(filter, model.CurrentPage, model.PageSize);

                model.PageNo = (int)(model.Total / model.PageSize);

                if (results.HasData())
                {
                    model.SearchResult = new List<IdentityEmployeeContact>();

                    foreach (var item in results)
                    {
                        var employeeInfo = HelperEmployee.GetBaseInfo(item.Id);
                        var employeeContact = HelperEmployee.GetContactInfo(item.Id);

                        if (employeeInfo != null && employeeContact != null)
                        {
                            employeeContact.Employee = employeeInfo;
                            model.SearchResult.Add(employeeContact);
                        }                       
                    }

                    if (model.SearchResult.HasData())
                    {
                        model.TotalCount = results[0].TotalCount;
                        model.CurrentPage = model.CurrentPage;
                        model.PageSize = model.PageSize;
                    }
                }
            }
            catch (Exception ex)
            {
                this.AddNotification(ManagerResource.LB_SYSTEM_BUSY, NotificationType.ERROR);

                _logger.LogError("Could not display Index page because: {0}", ex.ToString());
            }

            return View(model);
        }

        public ActionResult Details(EmployeeContactUpdateModel model)
        {
            try
            {
                if (model.EmployeeId <= 0)
                {
                    return new NotFoundResult();
                }

                var info = _contactStore.GetByEmployeeId(model.EmployeeId);

                if (info == null)
                    return RedirectToErrorPage();

                var detailModel = ParseUpdateDataEmployeeContact(info);

                var employee = _mainStore.GetById(model.EmployeeId);

                if (model.EmployeeId > 0)
                {
                    detailModel.Employee = employee;
                }

                return View(detailModel);
            }
            catch (Exception ex)
            {
                this.AddNotification(ManagerResource.LB_SYSTEM_BUSY, NotificationType.ERROR);

                _logger.LogError("Could not display Detail page because: {0}", ex.ToString());
            }
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            var returnModel = new EmployeeContactUpdateModel() { Employee = new IdentityEmployee()};

            try
            {
                GetCompanyIdOfEmployee(returnModel.Employee);                              
            }
            catch (Exception ex)
            {
                this.AddNotification(ManagerResource.LB_SYSTEM_BUSY, NotificationType.ERROR);

                _logger.LogError("Could not display Create page because: {0}", ex.ToString());
            }

            return View(returnModel);
        }

        [HttpPost]
        public ActionResult Create(EmployeeContactUpdateModel model)
        {
            var returnModel = new EmployeeContactUpdateModel() { Employee = new IdentityEmployee() };
            var newId = 0;
            var path = string.Empty;

            var currentUser = GetCurrentUser();
            var staffId = 0;

            if (currentUser != null)
                staffId = currentUser.StaffId;

            //Save image to wwwroot/image
            if (model.ImageFile != null)
            {
                path = FileUploadHelper.UploadImageAsync(model.ImageFile, string.Format("EmployeeAvatars/{0}", staffId)).Result;
            }
           
            try
            {                                                                                                                  
                var contact = ExtractEmployeeContactUpdateData(model);

                if (model.Employee != null)
                {
                    if (!string.IsNullOrEmpty(path))
                        model.Employee.AvatarUrl = path;

                    newId = _mainStore.InsertAll(model.Employee, contact);
                }
                    
                if (newId > 0)
                {
                    this.AddNotification(ManagerResource.LB_INSERT_SUCCESS, NotificationType.SUCCESS);
                    returnModel.EmployeeId = newId;
                }
            }
            catch (Exception ex)
            {
                this.AddNotification(ManagerResource.LB_SYSTEM_BUSY, NotificationType.ERROR);

                var delPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/" + path);
                System.IO.File.Delete(delPath);

                _logger.LogError("Failed for Create Employee request: " + ex.ToString());

                return View(model);
            }

            return RedirectToAction("Details", returnModel);
        }

        public ActionResult Edit(int employeeId)
        {
            var returnModel = new EmployeeContactUpdateModel();

            try
            {
                if (employeeId <= 0)
                {
                    return new NotFoundResult();
                }

                var info = _contactStore.GetByEmployeeId(employeeId);

                if (info == null)
                    return RedirectToErrorPage();

                var editModel = ParseUpdateDataEmployeeContact(info);

                var employee = _mainStore.GetById(employeeId);

                if (employee.Id > 0)
                {
                    editModel.Employee = employee;

                    if (!string.IsNullOrEmpty(editModel.Employee.AvatarUrl))
                    {
                        editModel.OldAvatarPath = employee.AvatarUrl;
                    }
                }

                return View(editModel);
            }
            catch (Exception ex)
            {
                this.AddNotification(ManagerResource.LB_SYSTEM_BUSY, NotificationType.ERROR);

                _logger.LogError("Could not display Edit page because: {0}", ex.ToString());
            }

            return View(returnModel);
        }

        [HttpPost]
        public ActionResult Edit(EmployeeContactUpdateModel model)
        {
            var returnModel = new EmployeeContactUpdateModel() { Employee = new IdentityEmployee() };

            var currentUser = GetCurrentUser();
            var staffId = 0;

            if (currentUser != null)
                staffId = currentUser.StaffId;

            var newPath = string.Empty;

            try
            {
                if (model.ImageFile != null)
                {
                    newPath = FileUploadHelper.UploadImageAsync(model.ImageFile, string.Format("EmployeeAvatars/{0}", staffId)).Result;
                }
                

                if (model.Employee != null)
                {
                    var info = ExtractEmployeeContactUpdateData(model);                   

                    if (!string.IsNullOrEmpty(newPath))
                    {
                        info.Employee.AvatarUrl = newPath;
                    }
                    else
                    {
                        info.Employee.AvatarUrl = model.OldAvatarPath;
                    }   

                    if (model.DeleteImage)
                    {
                        info.Employee.AvatarUrl = null;
                    }

                    var updateEmp = _mainStore.UpdateAll(info.Employee, info);

                    if (updateEmp)
                    {
                        HelperEmployee.ClearCache(model.EmployeeId);

                        var oldPath = model.OldAvatarPath;

                        if (!string.IsNullOrEmpty(oldPath))
                        {
                            var delPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/" + oldPath);

                            if (model.ImageFile != null || model.DeleteImage == true)
                            {
                                System.IO.File.Delete(delPath);
                            }
                        }                      

                        returnModel.EmployeeId = model.EmployeeId;
                       
                        this.AddNotification(ManagerResource.LB_UPDATE_SUCCESS, NotificationType.SUCCESS);                    
                    }
                }                                                       

            }

            catch (Exception ex)
            {
                this.AddNotification(ManagerResource.LB_SYSTEM_BUSY, NotificationType.ERROR);

                _logger.LogError("Failed for Update Employee request: " + ex.ToString());

                return View(model);
            }

            return RedirectToAction("Details", returnModel);
        }


        public ActionResult Delete(int employeeId)
        {
            if (employeeId <= 0)
            {
                return new NotFoundResult();
            }

            return PartialView("_PopupDelete", employeeId);
        }


        //POST: Classes/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_Confirm(int employeeId)
        {
            var strError = string.Empty;
            if (employeeId <= 0)
                return Json(new { success = false, message = ManagerResource.LB_ERROR_OCCURED });

            try
            {
                _mainStore.Delete(employeeId);

                //Clear cache
                HelperEmployee.ClearCache(employeeId);
            }
            catch (Exception ex)
            {
                strError = ManagerResource.LB_SYSTEM_BUSY;

                _logger.LogError("Failed to get Delete Employee because: " + ex.ToString());

                return Json(new { success = false, message = strError });
            }

            return Json(new { success = true, message = ManagerResource.LB_DELETE_SUCCESS, title = ManagerResource.LB_NOTIFICATION, clientcallback = "location.reload();" });
        }

        private IdentityEmployeeContact ExtractEmployeeContactUpdateData(EmployeeContactUpdateModel model)
        {
            var info = model.MappingObject<IdentityEmployeeContact>();

            return info;
        }


        private EmployeeContactUpdateModel ParseUpdateDataEmployeeContact(IdentityEmployeeContact data)
        {
            var info = data.MappingObject<EmployeeContactUpdateModel>();

            var fullAddress = string.Empty;
            String[] text = { info.PrefectureId, info.District, info.Street, info.RoomNumber };

            if (text.Length > 0)
            {
                fullAddress = string.Join(" - ", text);
            }

            info.FullAddress = fullAddress;
            
            return info;
        }



        private IdentityEmployee GetCompanyIdOfEmployee(IdentityEmployee employee)
        {
            var currentUser = CommonHelpers.GetCurrentUser();

            if (currentUser != null)
            {
                if (currentUser.UserName != "admin")
                {
                    employee.CompanyId = currentUser.Id;
                }
                else
                {
                    employee.CreatedBy = currentUser.Id;
                }
            }

            return employee;

        }
    }
}
