using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace VnCompany.DataLayer.Entities
{
    public class IdentityAccount
    {
        public virtual int Id { get; set; }

        [JsonIgnore]
        public virtual string PasswordHash { get; set; }
        [JsonIgnore]
        public virtual string SecurityStamp { get; set; }
        public virtual string Email { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual bool EmailConfirmed { get; set; }
        public virtual bool PhoneNumberConfirmed { get; set; }
        [JsonIgnore]
        public virtual int AccessFailedCount { get; set; }
        public virtual bool LockoutEnabled { get; set; }
        public virtual DateTime? LockoutEndDate { get; set; }
        [JsonIgnore]
        public virtual bool TwoFactorAuthEnabled { get; set; }

        //Entends
        public virtual string UserName { get; set; }
        public virtual DateTime? CreatedDateUtc { get; set; }
        [JsonIgnore]
        public virtual string PasswordHash2 { get; set; }
        public virtual string FullName { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string Avatar { get; set; }
        public virtual string OTPType { get; set; }
        public virtual DateTime? Birthday { get; set; }
        public virtual int Sex { get; set; }
        public virtual string Address { get; set; }
        public virtual string IDCard { get; set; }
        public virtual string Note { get; set; }
        public virtual string SocialProvider { get; set; }


        public virtual string TokenKey { get; set; }
        public virtual DateTime? TokenCreatedDate { get; set; }
        public virtual DateTime? TokenExpiredDate { get; set; }
        [JsonIgnore]
        public virtual int LoginDurations { get; set; }
        [JsonIgnore]
        public virtual string Domain { get; set; }
        [JsonIgnore]
        public virtual string NewPassword { get; set; }

        public virtual List<string> Roles { get; set; }

        public virtual bool IsFollow { get; set; }

        public int SocialProviderId { get; set; }


        public IdentityAccount()
        {
            this.Roles = new List<string>();
            LockoutEnabled = true;
        }

    }
}
