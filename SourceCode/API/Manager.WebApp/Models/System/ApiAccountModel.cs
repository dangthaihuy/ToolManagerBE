using Manager.DataLayer.Entities;
using Manager.WebApp.Resources;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Manager.WebApp.Models.System
{
    public class ApiRegisterModel
    {
        public string Email { get; set; }

        public string Password { get; set; }
        
        public string Fullname { get; set; }

    }

    public class ApiLoginModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class UserModel
    {
        public int Id { get; set; }

        public IFormFile Avatar { get; set; }
    }
}
