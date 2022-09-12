using Manager.DataLayer.Entities;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Manager.DataLayer.Entities
{
    public class IdentityUser : IdentityCommon
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public int AccessFailedCount { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public int ProviderId { get; set; }
       
        public string Code { get; set; }
        public string Avatar { get; set; }
        public List<string> Roles { get; set; }
        public List<IdentityUserClaim> Claims { get; set; }

        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyTel { get; set; }

        public string FullName { get; set; }
        public int StaffId { get; set; }

        public int ParentId { get; set; }
        public string HashingData { get; set; }
        public int TotalCount { get; set; }

        public int Status { get; set; }
        public IdentityUser()
        {
            this.Claims = new List<IdentityUserClaim>();
            this.Roles = new List<string>();
            this.Id = Guid.NewGuid().ToString();
            LockoutEnabled = true;
        }

        public IdentityUser(string userName)
            : this()
        {
            this.UserName = userName;
        }

        //Filtering
        public string RoleId { get; set; }
        public IdentityRole Role { get; set; }
    }
    public sealed class IdentityUserLogin
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Provider { get; set; }
        public string ProviderKey { get; set; }
    }
    public class IdentityUserClaim
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
    public class IdentityStatisticsUserByYear
    {
        public int id { get; set; }
        public int AgencyId { get; set; }
        public int year { get; set; }
        public int month_1 { get; set; }
        public int month_2 { get; set; }
        public int month_3 { get; set; }
        public int month_4 { get; set; }
        public int month_5 { get; set; }
        public int month_6 { get; set; }
        public int month_7 { get; set; }
        public int month_8 { get; set; }
        public int month_9 { get; set; }
        public int month_10 { get; set; }
        public int month_11 { get; set; }
        public int month_12 { get; set; }

        public DateTime? AgencyRegisteredDate { get; set; }
    }




    // Code Api Huy Đặng
    public class IdentityCurrentUser
    {
        public int Id { get; set; }
        
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Fullname { get; set; }
        


    }

    public class IdentityInformationUser
    {
        public int Id { get; set; }

        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Fullname { get; set; }
        public string PasswordHash { get; set; }
        public int TotalCount { get; set; }
    }

   
}
