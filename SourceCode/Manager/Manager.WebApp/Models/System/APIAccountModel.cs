using Manager.DataLayer.Entities;
using Manager.WebApp.Resources;
using System.ComponentModel.DataAnnotations;

namespace Manager.WebApp.Models.System
{
    public class ApiRegisterModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        
        public string FullName { get; set; }

    }

    public class ApiLoginModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
