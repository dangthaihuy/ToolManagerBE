using Autofac;
using Manager.DataLayer.Entities.Business;
using Manager.DataLayer.Stores.Business;
using Manager.SharedLibs;
using Manager.WebApp.Helpers;
using Manager.WebApp.Models.Business;
using Manager.WebApp.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.WebApp.Controllers.Business
{
    public class EmployeeController : BaseAuthedController
    {
        private readonly IStoreEmployee _mainStore;
        private readonly IStoreContact _contactStore;
        private readonly IStoreForm _formStore;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _mainStore = Startup.IocContainer.Resolve<IStoreEmployee>();
            _contactStore = Startup.IocContainer.Resolve<IStoreContact>();
            _formStore = Startup.IocContainer.Resolve<IStoreForm>();
            _logger = logger;

            //Clear cache
            CachingHelpers.ClearUserCache();
            CommonHelpers.ClearCookie(CommonHelpers.GetVersionToken());
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


        private IdentityEmployeeContact ExtractEmployeeContactUpdateData(EmployeeContactUpdateModel model, int employeeId)
        {
            var info = model.MappingObject<IdentityEmployeeContact>();
            info.EmployeeId = employeeId;

            return info;
        }

        private EmployeeContactUpdateModel ParseUpdateDataEmployeeContact(IdentityEmployeeContact data)
        {
            var info = data.MappingObject<EmployeeContactUpdateModel>();

            return info;
        }



        public ActionResult Index(EmployeeManagerModel model)
        {
            var listData = _mainStore.GetList();
            model.SearchResult = listData;
            return View(model);
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
            var newEmployeeId = 0;
            var newEmployeeContactId = 0;

            try
            {
                if (model.Employee != null)
                {
                    newEmployeeId = _mainStore.Insert(model.Employee);
                }

                if (newEmployeeId > 0)
                {
                    var info = ExtractEmployeeContactUpdateData(model, newEmployeeId);

                    newEmployeeContactId = _contactStore.Insert(info);

                    if (newEmployeeContactId > 0)
                    {
                        this.AddNotification(ManagerResource.LB_INSERT_SUCCESS, NotificationType.SUCCESS);
                    }
                }
            }
            catch (Exception ex)
            {
                this.AddNotification(ManagerResource.LB_SYSTEM_BUSY, NotificationType.ERROR);

                _logger.LogError("Failed for Create Employee request: " + ex.ToString());

                return View(model);
            }

            return View(returnModel);
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
    }
}
