using Manager.DataLayer.Entities;
using Manager.WebApp.Resources;
using System.ComponentModel.DataAnnotations;

namespace Manager.WebApp.Models.System
{
    public class ApiAccountModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        
        public string FullName { get; set; }

        
        public string PhoneNumber { get; set; }

        
        public string Email { get; set; }        

    }

    public class ApiLoginModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
