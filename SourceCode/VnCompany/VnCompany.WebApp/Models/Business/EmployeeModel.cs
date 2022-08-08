using VnCompany.DataLayer.Entities.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace VnCompany.WebApp.Models.Business
{
    public class EmployeeUpdateModel
    {
        public int Id { get; set; }
        public DateTime? JoinDate { get; set; }
        public DateTime? LeaveDate { get; set; }
        public string AvatarUrl { get; set; }
        public string Department { get; set; }
        public string CompanyId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
    }

    public class EmployeeManagerModel : CommonPagingModel
    {
        public List<IdentityEmployeeContact> SearchResult { get; set; }
        public int? Status { get; set; }

        public int Total { get; set; }
        public int PageNo { get; set; }
    }

    public class EmployeeContactUpdateModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public IdentityEmployee Employee { get; set; }
        public int IsEmployee { get; set; }
        public int Relationship { get; set; }
        public string FamilyName { get; set; }
        public string FamilyNameYomigana { get; set; }
        public string Name { get; set; }
        public string NameYomigana { get; set; }
        public int Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public int NationalityId { get; set; }
        public int VisaType { get; set; }
        public DateTime? VisaExpiryDate { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string PrefectureId { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public string RoomNumber { get; set; }

        public string AddressYomigana { get; set; }
        public string FullAddress { get; set; }

        public IFormFile ImageFile { get; set; }

        public string OldAvatarPath { get; set; }

        public bool DeleteImage { get; set; }

        public EmployeeContactUpdateModel()
        {
            Employee = new IdentityEmployee();
        }
    }
}
