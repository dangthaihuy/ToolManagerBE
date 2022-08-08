using Autofac;
using Manager.DataLayer.Entities.Business;
using Manager.DataLayer.Stores.Business;
using Manager.SharedLibs;
using Manager.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Manager.WebApp.Helpers;
using System.Linq;

namespace Manager.WebApp.Controllers.Business
{
    public class WorkFlowController : BaseAuthedController
    {
        private readonly IStoreWorkFlow _mainStore;
        private readonly IStoreForm _formStore;
        private readonly ILogger<WorkFlowController> _logger;
        public WorkFlowController(ILogger<WorkFlowController> logger)
        {
            _logger = logger;
            _mainStore = Startup.IocContainer.Resolve<IStoreWorkFlow>();
            _formStore = Startup.IocContainer.Resolve<IStoreForm>();
        }

        public IActionResult Index(WorkFlowViewModel model)
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
            var filter = new IdentityWorkFlow
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
            var model = new WorkFlowViewModel();

            return View(model);
        }

        /// <summary>
        /// Create WorkFlow. POST
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [AccessRoleChecker]
        public ActionResult Create(WorkFlowViewModel model)
        {
            int newId;
            try
            {
                var listWorkFlowForm = JsonConvert.DeserializeObject<List<IdentityWorkFlowForm>>(model.ModelWorkFlow.WorkFlowForm);

                // Mapping data from ViewModel to Entity
                var info = model.ModelWorkFlow.MappingObject<IdentityWorkFlow>();
                info.CreatedBy = 1;
                info.Status = 1;

                newId = _mainStore.Insert(info);
                if (listWorkFlowForm.HasData())
                {
                   foreach(var item in listWorkFlowForm)
                    {
                        item.WorkFlowId = newId;
                        _mainStore.InsertWorkFlowForm(item);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Failed for Create Menu Service request: " + ex.ToString());
                return View(model);
            }
            return RedirectToAction("Edit", "WorkFlow", new { Id = newId });
        }

        /// <summary>
        /// GET: /WorkFlow/Edit/1
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AccessRoleChecker]
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var workFlow = HelperWorkFlow.GetBaseInfo(id);

                await Task.FromResult(workFlow);
                if (workFlow == null)
                {
                    return RedirectToErrorPage();
                }

                // Mapping data from entity to viewModel
                WorkFlowViewModel model = new WorkFlowViewModel();
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

                    //model.ModelWorkFlow.WorkFlowForm = JsonConvert.SerializeObject(model.ModelWorkFlow.Forms.Select(m => m.Id).ToList());
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
        /// Update WorkFlow. POST
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AccessRoleChecker]
        public async Task<ActionResult> Edit(WorkFlowViewModel model)
        {
            try
            {
                var listWorkFlowForm = JsonConvert.DeserializeObject<List<IdentityWorkFlowForm>>(model.ModelWorkFlow.WorkFlowForm);
                var workFlow = HelperWorkFlow.GetBaseInfo(model.Id);
                
                // Mapping data from viewModel to entity
                var info = model.ModelWorkFlow.MappingObject<IdentityWorkFlow>();
                info.LastUpdatedBy = 1;
                info.Status = 1;
                
                if (listWorkFlowForm.HasData())
                {
                    var listFormId = listWorkFlowForm.Select(m => m.FormId).ToList();
                    var listFormIdInDb = workFlow.Forms.Select(m => m.Id).ToList();
                    var listFormIdNotChange = listFormIdInDb.Where(c => listFormId.Contains(c)).ToList();

                    var listFormIdDelete = listFormIdInDb.Except(listFormIdNotChange).ToArray();
                    var listFormIdAdd = listFormId.Except(listFormIdNotChange).ToList();
                    if (listFormIdDelete.HasData())
                    {
                        foreach(var formId in listFormIdDelete)
                        {
                            _ = _mainStore.DeleteWorkFlowForm(model.Id, formId);
                        }
                    }

                    var listFormAdd = listWorkFlowForm.Where(c => listFormIdAdd.Contains(c.FormId)).ToList();
                    if (listFormAdd.HasData())
                    {
                        foreach(var form in listFormAdd)
                        {
                            form.WorkFlowId = model.Id;
                            _ = _mainStore.InsertWorkFlowForm(form);
                        }
                    }

                    var listFormUpdate = listWorkFlowForm.Where(c => listFormIdNotChange.Contains(c.FormId)).ToList();
                    if (listFormUpdate.HasData())
                    {
                        foreach(var form in listFormUpdate)
                        {
                            form.WorkFlowId = model.Id;
                            form.SortOrder = form.SortOrder;
                            _ = _mainStore.UpdateWorkFlowForm(form);
                        }
                    }
                }
                // Update Menu Service
                var result = _mainStore.Update(info);
                
                await Task.FromResult(result);

                //Clear cache
                HelperWorkFlow.ClearCache(model.Id);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Failed for edit work flow request: " + ex.ToString());
                return View(model);
            }
            return RedirectToAction("Index", "WorkFlow");
        }

        /// <summary>
        /// Methos:Post. WorkFlow/Search
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AccessRoleChecker]
        [HttpPost]
        public ActionResult Search(FormViewModel model)
        {
            if (model.PageSize == 0 && model.CurrentPage == 0)
            {
                model = GetDefaultFilterModel(model);
            }

            model.PageSize = 50;
            if (string.IsNullOrEmpty(model.SearchExec))
            {
                model.SearchExec = "Y";
                if (!ModelState.IsValid)
                {
                    ModelState.Clear();
                }
            }
            var filter = new IdentityForm
            {
                Keyword = !string.IsNullOrEmpty(model.Keyword) ? model.Keyword.ToStringNormally() : null,
                Status = model.SearchStatus
            };
            try
            {
                model.SearchResult = _formStore.GetByPage(filter, model.CurrentPage, model.PageSize);

                if (model.SearchResult != null && model.SearchResult.Count > 0)
                {
                    model.TotalCount = model.SearchResult[0].TotalCount;
                    model.PageNo = (int)(model.TotalCount / model.PageSize);

                    model.CurrentPage = model.CurrentPage;
                    model.PageSize = model.PageSize;
                }
                model.SearchResult.ForEach(item =>
                {
                    var formFields = _formStore.GetFormFieldsByFormId(item.Id);
                    if (formFields.HasData())
                    {
                        item.Fields = formFields.Where(x => x.TemplateInfo != null).ToList();
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Failed to get data because: " + ex.ToString());
                return View(model);
            }

            return PartialView("Partials/_FormContent", model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> SearchFormViewDetail(int id)
        {
            var model = new FormCreateOrUpdateViewModel();
            try
            {
                var form = HelperForm.GetBaseInfo(id);

                await Task.FromResult(form);
                if (form == null)
                {
                    return RedirectToErrorPage();
                }

                // Mapping data from entity to viewModel
                 model = form.MappingObject<FormCreateOrUpdateViewModel>();

                // Get fields of form
                var formFields = _formStore.GetFormFieldsByFormId(id);
                if (formFields.HasData())
                {
                    var listTmp = formFields.Where(x => x.TemplateInfo != null).Select(x => x.TemplateInfo).ToList();

                    model.Template = JsonConvert.SerializeObject(listTmp);
                }
            }catch(Exception ex)
            {
                _logger.LogDebug("Failed to get data because: " + ex.ToString());
                return View(model);
            }
            return PartialView("Partials/_DetailForm", model);
        }

    }
}
