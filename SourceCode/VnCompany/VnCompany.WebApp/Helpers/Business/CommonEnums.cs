using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VnCompany.WebApp.Helpers.Business
{
    public static class EnumFormatInfoCacheKeys
    {
        public static string Forms = "FORM_{0}";

        public static string Employee = "EMPLOYEE_{0}";

        public static string EmployeeContact = "EMPLOYEE_CONTACT_{0}";
    }

    public enum EnumGender
    {
        Male = 1,
        Female = 2,
        Unknown = 0
    }
}
