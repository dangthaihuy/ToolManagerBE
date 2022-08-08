using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VnCompany.WebApp.Helpers
{
    public static class EnumCommonCode
    {
        public static int Success = 1;
        public static int Error = -1;
        public static int Error_Info_NotFound = -2;        
    }

    public static class EnumSearchType
    {
        public static string Keyword = "keyword";
        public static string Address = "address";
    }

    public static class EnumResultType
    {
        public static string File = "file";
        public static string Text = "text";
    }

    public static class EnumSearchEngine
    {
        public static string Google = "google";
        public static string Yahoo = "yahoo";
        public static string Bing = "bing";
    }

    public enum EnumNotificationUserType
    {
        VnCompany = 0,
    }

    public static class EnumFormatInfoCacheKeys
    {
        public static string WorkFlow = "WORKFLOW_{0}";
        public static string Form = "FORM_{0}";
    }

    public static class EnumFormatListCacheKeys
    {
        public static string Forms = "FORMS";
    }
}
