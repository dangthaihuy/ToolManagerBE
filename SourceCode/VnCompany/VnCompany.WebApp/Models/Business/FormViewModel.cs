using VnCompany.DataLayer.Entities.Business;
using VnCompany.WebApp.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VnCompany.WebApp.Models
{
    public class FormViewModel : CommonPagingModel
    {
        public FormViewModel()
        {
            FormModel = new IdentityForm();
            SearchStatus = -1;
        }
        public List<IdentityForm> SearchResult { get; set; }
        public IdentityForm FormModel { get; set; }
        public int Total { get; set; }
        public int PageNo { get; set; }
        public string Template { get; set; }
    }

    public class FormCreateOrUpdateViewModel
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.ManagerResource), Name = nameof(ManagerResource.LB_CODE))]
        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.ERROR_NOT_NULL_REQUIRED))]
        public string Code { get; set; }

        [Display(ResourceType = typeof(Resources.ManagerResource), Name = nameof(ManagerResource.LB_NAME))]
        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.ERROR_NOT_NULL_REQUIRED))]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Resources.ManagerResource), Name = nameof(ManagerResource.LB_DESCRIPTION))]
        public string ShortDescription { get; set; }
        public string Template { get; set; }
    }
}
