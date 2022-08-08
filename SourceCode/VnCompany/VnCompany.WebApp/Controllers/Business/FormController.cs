using VnCompany.DataLayer.Entities.Business;
using VnCompany.DataLayer.Stores.Business;
using VnCompany.SharedLibs;
using VnCompany.WebApp.Models;
using VnCompany.WebApp.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Autofac;
using System.Threading.Tasks;
using VnCompany.WebApp.Helpers;

namespace VnCompany.WebApp.Controllers.Business
{
    public class FormController : BaseAuthedController
    {
        private readonly IStoreForm _mainStore;
        private readonly ILogger<AccountController> _logger;
        public FormController(ILogger<AccountController> logger)
        {
            _logger = logger;
            _mainStore = Startup.IocContainer.Resolve<IStoreForm>();
            
        }

        /// <summary>
        /// Search Form
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult Index(FormViewModel model)
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
            var filter = new IdentityForm
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

        /// <summary>
        /// Get data create form
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            var formModel = new FormCreateOrUpdateViewModel();
            return View(formModel);
        }
    
        /// <summary>
        /// Create Post
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [AccessRoleChecker]
        public ActionResult Create(FormCreateOrUpdateViewModel form)
        {
            dynamic json = JsonConvert.DeserializeObject(form.Template);
            int newId;
            try
            {
                // Mapping data from ViewModel to Entity
                var identityForm = form.MappingObject<IdentityForm>();
                identityForm.CreatedBy = 1;
                identityForm.Status = 1;

                newId = _mainStore.Insert(identityForm);
                foreach (dynamic item in json)
                {
                    var formField = new IdentityFormField
                    {
                        FormId = newId,
                        Template = item.ToString()
                    };
                    var temp = _mainStore.InsertFormField(formField);
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Failed for Create Form request: " + ex.ToString());
                return View(form);
            }
            return RedirectToAction("Edit", "Form", new { Id = newId });
        }

        /// <summary>
        /// GET: /Form/Edit/1
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AccessRoleChecker]
        public async Task<ActionResult> Edit(int id)
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
                var formFields = _mainStore.GetFormFieldsByFormId(id);
                if (formFields.HasData())
                {
                    var listTmp = formFields.Where(x => x.TemplateInfo != null).Select(x => x.TemplateInfo).ToList();

                    model.Template = JsonConvert.SerializeObject(listTmp);
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not display Edit page because: {0}", ex.ToString());

                return RedirectToErrorPage();
            }
        }

        /// <summary>
        /// Update form
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [AccessRoleChecker]
        public async Task<ActionResult> Edit(FormCreateOrUpdateViewModel form)
        {
            dynamic json = JsonConvert.DeserializeObject(form.Template);
            var listFormFieldTemplate = new List<IdentityFormFieldTemplate>();
            try
            {
                // Mapping data from viewModel to entity
                var identityForm = form.MappingObject<IdentityForm>();
                identityForm.LastUpdatedBy = 1;
                identityForm.Status = 1;

                foreach (dynamic item in json)
                {
                    var formFieldTemplate = JsonConvert.DeserializeObject<IdentityFormFieldTemplate>(JsonConvert.SerializeObject(item));
                    listFormFieldTemplate.Add(formFieldTemplate);
       
                }
                var formFields = _mainStore.GetFormFieldsByFormId(form.Id);
                if (formFields != null)
                {
                    // Form field don't exist in DB
                    var listNewFormFieldTemplate = listFormFieldTemplate.Where(c => c.FieldId == 0 && c.FormId == 0).ToList();
                    // Form field from request (Exist in DB: Update)
                    var listOldFormFieldTemplate = listFormFieldTemplate.Where(c => c.FieldId != 0 || c.FormId != 0).ToList();
                    var listOldFormFieldTempalteId = listOldFormFieldTemplate.Select(m => m.FieldId).ToList();
                    
                    var listFormFieldIdInDB = formFields.Select(m => m.Id);
                    // Form field update 
                    var listFormFieldUpdate = formFields.Where(c => listOldFormFieldTempalteId.Contains(c.Id));
                    // Form field Delete
                    var listFormFieldDeleteId = formFields.Except(listFormFieldUpdate).Select(m => m.Id).ToList();

                    // Action: Insert
                    foreach(var item in listNewFormFieldTemplate)
                    {
                        var formField = new IdentityFormField
                        {
                            FormId = form.Id,
                            Template = JsonConvert.SerializeObject(item)
                        };
                        var t = _mainStore.InsertFormField(formField);
                    }

                    // Action: Update Form field
                    foreach(var item in listOldFormFieldTemplate)
                    {
                        var identityformField = new IdentityFormField();
                        identityformField.Id = item.FieldId;
                        identityformField.FormId = item.FormId;
                        identityformField.Template = JsonConvert.SerializeObject(item);
                        var t = _mainStore.UpdateFormField(identityformField);
                    }

                    // Action: Delete Form filed
                    foreach(var item in listFormFieldDeleteId)
                    {
                        var t = _mainStore.DeleteFormField(item);
                    }
                }

                // Update form
                var result = _mainStore.Update(identityForm);
                await Task.FromResult(result);

                //Clear cache
                HelperForm.ClearCache(form.Id);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Failed for Edit Form request: " + ex.ToString());
                return View(form);
            }
            return RedirectToAction("Index", "Form");
        }
    }
}
