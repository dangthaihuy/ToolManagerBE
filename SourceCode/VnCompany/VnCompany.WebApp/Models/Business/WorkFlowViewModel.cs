using VnCompany.DataLayer.Entities.Business;
using VnCompany.WebApp.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VnCompany.WebApp.Models
{
    public class WorkFlowViewModel : CommonPagingModel
    {
        public WorkFlowViewModel()
        {
            ModelWorkFlow = new WorkFlowCreateOrUpdateViewModel();
        }
        public WorkFlowCreateOrUpdateViewModel ModelWorkFlow { get; set; }
        public List<IdentityWorkFlow> SearchResult { get; set; }
        public int Total { get; set; }
        public int PageNo { get; set; }
    }

    public class WorkFlowCreateOrUpdateViewModel
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.ManagerResource), Name = nameof(ManagerResource.LB_CODE))]
        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.ERROR_NOT_NULL_REQUIRED))]
        public string Code { get; set; }

        [Display(ResourceType = typeof(Resources.ManagerResource), Name = nameof(ManagerResource.LB_NAME))]
        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.ERROR_NOT_NULL_REQUIRED))]

        public string Name { get; set; }
        [Display(ResourceType = typeof(Resources.ManagerResource), Name = nameof(ManagerResource.LB_DESCRIPTION))]

        public string Description { get; set; }

        public int Status { get; set; }
        public List<IdentityForm> Forms { get; set; }
        public string WorkFlowForm { get; set; }
        public List<IdentityWorkFlowForm> WorkFlowForms { get; set; }
    }
}
