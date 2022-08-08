using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using VnCompany.DataLayer;
using VnCompany.WebApp.Resources;

namespace VnCompany.WebApp.Models
{
    public class RoleViewModel
    {
        [Display(ResourceType = typeof(Resources.ManagerResource), Name = nameof(ManagerResource.LB_NAME))]
        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.ERROR_NOT_NULL_REQUIRED))]
        public string Name { get; set; }

        [Required]
        public string Id {get; set;}
    }  


    public class IndexRoleViewModel
    {
        public IndexRoleViewModel()
        {
            CurrentRole = new RoleViewModel();
            RoleList = new List<IdentityRole>();
        }

        public IndexRoleViewModel(IQueryable<IdentityRole> roles)
        {
            CurrentRole = new RoleViewModel() { Id = Guid.NewGuid().ToString()};
            RoleList = new List<IdentityRole>();
            
        }

        public RoleViewModel CurrentRole { get; set; }
        public List<IdentityRole> RoleList { get; set; }
        public string UserId { get; set; }
    }
}