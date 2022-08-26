using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.DataLayer.Entities.Business
{
    [Serializable]
    public class IdentityEmployee : CommonIdentity
    {
        public int Id { get; set; }
        public DateTime? JoinDate { get; set; }
        public DateTime? LeaveDate { get; set; }
        public string AvatarUrl { get; set; }
        public string Department { get; set; }
        public string CompanyId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int EmploymentType { get; set; }
        public string EmployeeCode { get; set; }
        public int Status { get; set; }
    }

    [Serializable]
    public class IdentityEmployeeContact : CommonIdentity
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
        public int PrefectureId { get; set; }
        public string District { get; set; }
        public string Street { get; set; }

        public int Status { get; set; }
    }

    [Serializable]
    public class IdentityLearningHistory
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int Level { get; set; }
        public int MajorId { get; set; }
        public string SchoolName { get; set; }
        public int Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    [Serializable]
    public class IdentityCertificate
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string CertificateName { get; set; }
        public int Status { get; set; }
        public float Points { get; set; }
        public DateTime? Date { get; set; }
    }
}
