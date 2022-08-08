using System;
using VnCompany.WebApp.Helpers;
using VnCompany.DataLayer;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VnCompany.DataLayer.Entities;
using VnCompany.WebApp.Resources;
using static VnCompany.WebApp.Helpers.LanguagesProvider;

namespace VnCompany.WebApp.Models
{
    public class FunctionViewModel
    {
        public int Id { get; set; }

        public string OperationName { get; set; }

        [Required]
        [Display(Name = "Controller")]
        public string AccessId { get; set; }
        public string AccessName { get; set; }

        [Required]
        [Display(Name = "Action")]
        public string ActionName { get; set; }

        public List<ControllerOperations> AllControllerOperations { get; set; }
        public List<IdentityAccess> AllAccesses { get; set; }

        public List<IdentityOperation> AllOperations { get; set; }

        public int IndexOrder { get; set; }

        public FunctionViewModel()
        {
            AllControllerOperations = Constant.GetAllControllerOperations();
            AllOperations = new List<IdentityOperation>();
            AllAccesses = new List<IdentityAccess>();
        }
    }
    public class ManageOperationLangModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.ERROR_NOT_NULL_REQUIRED))]
        public string OperationName { get; set; }

        public string LangCode { get; set; }

        public int OperationId { get; set; }

        public bool IsUpdate { get; set; }

        public List<Languages> Languages { get; set; }

        public IdentityOperation OperationInfo { get; set; }

        public ManageOperationLangModel()
        {
            Languages = new List<Languages>();
        }
    }
}