using Autofac;
using Manager.DataLayer.Entities.Business;
using Manager.DataLayer.Stores.Business;
using Manager.SharedLibs;
using Manager.WebApp.Helpers;
using Manager.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Manager.WebApp.Controllers.Business
{
    public class MenuServiceController : BaseAuthedController
    {
        private readonly IStoreMenuService _mainStore;
        private readonly IStoreWorkFlow _workflowStore;

        private readonly ILogger<MenuServiceController> _logger;
        public MenuServiceController(ILogger<MenuServiceController> logger)
        {
            _logger = logger;
            _mainStore = Startup.IocContainer.Resolve<IStoreMenuService>();
            _workflowStore = Startup.IocContainer.Resolve<IStoreWorkFlow>();
        }

        /// <summary>
        /// Search MenuService
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult Index(MenuServiceViewModel model)
        {
            model = GetDefaultFilterModel(model);
            if (string.IsNullOrEmpty(model.SearchExec))
            {
                model.SearchExec = "Y";
                if (!ModelState.IsValid)
                {
                    ModelState.Clear();
                }
            }
            var filter = new IdentityMenuService
            {
                Keyword = !string.IsNullOrEmpty(model.Keyword) ? model.Keyword.ToStringNormally() : null,
                Status = model.SearchStatus
            };
            try
            {
                model.SearchResult = _mainStore.GetByPage(filter, model.CurrentPage, model.PageSize);
                model.PageNo = (int)(model.Total / model.PageSize);

                if (model.SearchResult != null && model.SearchResult.Count > 0)
                {
                    model.TotalCount = model.SearchResult[0].TotalCount;
                    model.CurrentPage = model.CurrentPage;
                    model.PageSize = model.PageSize;
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Failed to get data because: " + ex.ToString());

                return View(model);
            }

            return View(model);
        }

        public IActionResult Create()
        {
            var model = new MenuServiceCreateOrUpdateViewModel();
            return View(model);
        }

        /// <summary>
        /// Create MenuService. POST
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [AccessRoleChecker]
        public ActionResult Create(MenuServiceCreateOrUpdateViewModel model)
        {
            int newId;
            try
            {
                // Convert json to List<MenuServiceWorkflow>
                var listMenuServiceWorkflow = JsonConvert.DeserializeObject<List<IdentityMenuWorkFlow>>(model.JsonWorkflow);
                
                // Mapping data from ViewModel to Entity
                var identityMenuService = model.MappingObject<IdentityMenuService>();
                identityMenuService.CreatedBy = 1;
                identityMenuService.Status = 1;

                newId = _mainStore.Insert(identityMenuService);

                // Insert data to relation table
                foreach(var item in listMenuServiceWorkflow)
                {
                    item.MenuId = newId;
                    _ = _mainStore.InsertMenuServiceWorkflow(item);
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Failed for Create Menu Service request: " + ex.ToString());
                return View(model);
            }
            return RedirectToAction("Edit", "MenuService", new { Id = newId });
        }

        /// <summary>
        /// GET: /MenuService/Edit/1
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AccessRoleChecker]
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var menuService = HelperMenuService.GetBaseInfo(id);

                await Task.FromResult(menuService);
                if (menuService == null)
                {
                    return RedirectToErrorPage();
                }

                // Mapping data from entity to viewModel
                var model = menuService.MappingObject<MenuServiceCreateOrUpdateViewModel>();

                if (model.WorkFlows.HasData())
                {
                    for(int i =0; i < model.WorkFlows.Count; i++)
                    {
                        model.WorkFlows[i] = HelperWorkFlow.GetBaseInfo(model.WorkFlows[i].Id);
                        if (model.WorkFlows[i].Forms.HasData())
                        {
                            for(var j = 0; j < model.WorkFlows[i].Forms.Count; j++)
                            {
                                model.WorkFlows[i].Forms[j] = HelperForm.GetBaseInfo(model.WorkFlows[i].Forms[j].Id);
                            }
                        }
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not display Edit Menu Service page because: {0}", ex.ToString());

                return RedirectToErrorPage();
            }
        }

        /// <summary>
        /// Update Menu Service. POST
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AccessRoleChecker]
        public async Task<ActionResult> Edit(MenuServiceCreateOrUpdateViewModel model)
        {
            try
            {
                // Mapping data from viewModel to entity
                var identityMenuService = model.MappingObject<IdentityMenuService>();
                identityMenuService.LastUpdatedBy = 1;
                identityMenuService.Status = 1;

                // Update Menu Service
                var result = _mainStore.Update(identityMenuService);
                await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Failed for edit menu service request: " + ex.ToString());
                return View(model);
            }
            return RedirectToAction("Index", "MenuService");
        }

        [AccessRoleChecker]
        [HttpPost]
        public ActionResult Search(WorkFlowViewModel model)
        {
            model = GetDefaultFilterModel(model);
            model.PageSize = 50;
            if (string.IsNullOrEmpty(model.SearchExec))
            {
                model.SearchExec = "Y";
                if (!ModelState.IsValid)
                {
                    ModelState.Clear();
                }
            }
            var filter = new IdentityWorkFlow
            {
                Keyword = !string.IsNullOrEmpty(model.Keyword) ? model.Keyword.ToStringNormally() : null,
                Status = model.SearchStatus
            };
            try
            {
                model.SearchResult = _workflowStore.GetByPage(filter, model.CurrentPage, model.PageSize);

                if (model.SearchResult != null && model.SearchResult.Count > 0)
                {
                    model.TotalCount = model.SearchResult[0].TotalCount;
                    model.PageNo = (int)(model.TotalCount / model.PageSize);

                    model.CurrentPage = model.CurrentPage;
                    model.PageSize = model.PageSize;
                }
                model.SearchResult.ForEach(item =>
                {
                    var workFlow = HelperWorkFlow.GetBaseInfo(item.Id);
                    
                    if (workFlow.Forms.HasData())
                    {
                        item.Forms = new List<IdentityForm>();
                        foreach (var form in workFlow.Forms)
                        {
                            var frmInfo = HelperForm.GetBaseInfo(form.Id);

                            if (frmInfo != null)
                                item.Forms.Add(frmInfo);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Failed to get data because: " + ex.ToString());
                return View(model);
            }

            return PartialView("Partials/_SearchWorkflow", model);
        }

        [AccessRoleChecker]
        public async Task<ActionResult> SearchWorkFlowViewDetail(int id)
        {
            WorkFlowViewModel model = new WorkFlowViewModel();
            try
            {
                var workFlow = HelperWorkFlow.GetBaseInfo(id);

                await Task.FromResult(workFlow);
                if (workFlow == null)
                {
                    return RedirectToErrorPage();
                }

                // Mapping data from entity to viewModel
                model.ModelWorkFlow = workFlow.MappingObject<WorkFlowCreateOrUpdateViewModel>();

                if (workFlow.Forms.HasData())
                {
                    model.ModelWorkFlow.Forms = new List<IdentityForm>();
                    foreach (var item in workFlow.Forms)
                    {
                        var frmInfo = HelperForm.GetBaseInfo(item.Id);

                        if (frmInfo != null)
                            model.ModelWorkFlow.Forms.Add(frmInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Failed to get data because: " + ex.ToString());
                //return View(model);
            }
            return PartialView("Partials/_DetailWorkflow", model);
        }
    }
}
