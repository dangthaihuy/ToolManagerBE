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
        public string Fullname { get; set; }
        public string Phone { get; set; }

        public IFormFile Avatar { get; set; }
    }

    public class ChangePasswordModel
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
    }

    public class EmailModel
    {
        public string Sender { get; set; }
        public string SenderName { get; set; }
        public string SenderPwd { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Receiver { get; set; }
    }
}
