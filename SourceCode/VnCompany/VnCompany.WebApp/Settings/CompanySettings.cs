using System.ComponentModel.DataAnnotations;
using VnCompany.WebApp.Settings;

namespace VnCompany.Business
{
    public class CompanySettings : SettingsBase
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
    }

    public class FTCompanySettings : CompanySettings
    {
        
    }

    public class FACompanySettings : CompanySettings
    {

    }
}
