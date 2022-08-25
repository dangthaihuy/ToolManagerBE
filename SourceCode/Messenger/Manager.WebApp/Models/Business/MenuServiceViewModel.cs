using Manager.DataLayer.Entities.Business;
using Manager.WebApp.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Manager.WebApp.Models
{
    public class MenuServiceViewModel : CommonPagingModel
    {
        public MenuServiceViewModel()
        {
            MenuServiceModel = new IdentityMenuService();
            SearchStatus = -1;
        }
        public List<IdentityMenuService> SearchResult { get; set; }
        public IdentityMenuService MenuServiceModel { get; set; }
        public int Total { get; set; }
        public int PageNo { get; set; }
        public string Template { get; set; }
    }

    public class MenuServiceCreateOrUpdateViewModel
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
        public string JsonWorkflow { get; set; }

        public List<IdentityMenuWorkFlow> MenuWorkFlows { get; set; }
        
        public List<IdentityWorkFlow> WorkFlows { get; set; }
        public MenuServiceCreateOrUpdateViewModel()
        {
            WorkFlows = new List<IdentityWorkFlow>();
        }
        
    }
}
