using Manager.SharedLibs;
using System;

namespace Manager.WebApp.Settings
{
    //public class AWSSettings
    //{
    //    #region

    //    public static bool AWS_SES_Enabled
    //    {
    //        get
    //        {
    //            return Convert.ToBoolean(AppConfiguration.GetAppsetting("AWS-SES-Enabled"));
    //        }
    //    }

    //    public static string AWS_SES_EmailDisplayName
    //    {
    //        get
    //        {
    //            return AppConfiguration.GetAppsetting("AWS-SES-EmailDisplayName").ToString();
    //        }
    //    }

    //    public static string AWS_SES_Email
    //    {
    //        get
    //        {
    //            return AppConfiguration.GetAppsetting("AWS-SES-Email").ToString();
    //        }
    //    }

    //    public static string AWS_SES_AccessKeyId
    //    {
    //        get
    //        {
    //            return AppConfiguration.GetAppsetting("AWS-SES-AccessKeyId").ToString();
    //        }
    //    }

    //    public static string AWS_SES_SecrectAccessKey
    //    {
    //        get
    //        {
    //            return AppConfiguration.GetAppsetting("AWS-SES-SecrectAccessKey").ToString();
    //        }
    //    }

    //    #endregion
    //}

    public class SystemSettings
    {
        public static string Environment
        {
            get
            {
                return AppConfiguration.GetAppsetting("SystemSetting:Environment");
            }
        }

        public static string MessengerCloud
        {
            get
            {
                return AppConfiguration.GetAppsetting("SystemSetting:MessengerCloud");
            }
        }


        public static string CurrentVersion
        {
            get
            {
                return AppConfiguration.GetAppsetting("SystemSetting:CurrentVersion");
            }
        }

        public static string CultureKey
        {
            get
            {
                return AppConfiguration.GetAppsetting("SystemSetting:CultureKey");
            }
        }

        public static string DefaultCacheKeyPrefix
        {
            get
            {
                return AppConfiguration.GetAppsetting("System:DefaultCacheKeyPrefix").ToString();
            }
        }

        public static string JwtSecretKey
        {
            get
            {
                return AppConfiguration.GetAppsetting("SystemSetting:JwtSecretKey");
            }
        }

        public static string SingleSignOnApi
        {
            get
            {
                return AppConfiguration.GetAppsetting("SystemSetting:SingleSignOnApi");
            }
        }

        public static int DefaultPageSize
        {
            get
            {
                return Utils.ConvertToInt32(AppConfiguration.GetAppsetting("SystemSetting:DefaultPageSize"));
            }
        }

        public static int MaximumVideoUploadLengthInMB
        {
            get
            {
                return Utils.ConvertToInt32(AppConfiguration.GetAppsetting("SystemSetting:MaximumVideoUploadLengthInMB"));
            }
        }

        public static int MaxPageSize
        {
            get
            {
                return Utils.ConvertToInt32(AppConfiguration.GetAppsetting("SystemSetting:MaxPageSize"));
            }
        }

        public static string EncryptKey
        {
            get
            {
                return AppConfiguration.GetAppsetting("SystemSetting:EncryptKey");
            }
        }

        public static string EmailSender
        {
            get
            {
                return AppConfiguration.GetAppsetting("SystemSetting:Email_Sender");
            }
        }

        public static string EmailSenderPwd
        {
            get
            {
                return AppConfiguration.GetAppsetting("SystemSetting:Email_SenderPwd");
            }
        }

        public static string EmailHost
        {
            get
            {
                return AppConfiguration.GetAppsetting("SystemSetting:Email_Host");
            }
        }

        public static string EmailServerPort
        {
            get
            {
                return AppConfiguration.GetAppsetting("SystemSetting:Email_ServerPort");
            }
        }

        public static bool EmailIsUseSSL
        {
            get
            {
                return Convert.ToBoolean(AppConfiguration.GetAppsetting("SystemSetting:Email_IsUseSSL"));
            }
        }

        public static bool EmailActiveAccountManual
        {
            get
            {
                return Convert.ToBoolean(AppConfiguration.GetAppsetting("SystemSetting:EmailActiveAccountManual"));
            }
        }

        public static string Email_ActiveLink
        {
            get
            {
                return AppConfiguration.GetAppsetting("SystemSetting:Email_ActiveLink").ToString();
            }
        }

        public static string Email_AccepFriendInvitationLink
        {
            get
            {
                return AppConfiguration.GetAppsetting("SystemSetting:Email_AccepFriendInvitationLink").ToString();
            }
        }

        public static bool IsLogParamater
        {
            get
            {
                return Convert.ToBoolean(AppConfiguration.GetAppsetting("SystemSetting:IsLogParamater"));
            }
        }

        public static string SMSServiceUrl
        {
            get
            {
                return AppConfiguration.GetAppsetting("SystemSetting:SMSUrl");
            }
        }

        public static string SendEmailServiceUrl
        {
            get
            {
                return AppConfiguration.GetAppsetting("SystemSetting:SendEmailServiceUrl");
            }
        }

        public static double ExternalServiceTimeout
        {
            get
            {
                return Convert.ToDouble(AppConfiguration.GetAppsetting("SystemSetting:ExternalServiceTimeout"));
            }
        }

        public static string CommonCacheKeyPrefix
        {
            get
            {
                return AppConfiguration.GetAppsetting("SystemSetting:CommonCacheKeyPrefix");
            }
        }

        public static string MediaFileFolder
        {
            get
            {
                return AppConfiguration.GetAppsetting("SystemSetting:MediaFileFolder");
            }
        }

        public static string FrontendURL
        {
            get
            {
                return AppConfiguration.GetAppsetting("SystemSetting:FrontendURL");
            }
        }

        public static int UserCachingTime
        {
            get
            {
                return Utils.ConvertToInt32(AppConfiguration.GetAppsetting("SystemSetting:UserCachingTime"));
            }
        }

        public static string DomainShare
        {
            get
            {
                return AppConfiguration.GetAppsetting("SystemSetting:DomainShare");
            }
        }

        public static int DefaultCachingTimeInMinutes
        {
            get
            {
                return Utils.ConvertToInt32(AppConfiguration.GetAppsetting("SystemSetting:DefaultCachingTimeInMinutes"));
            }
        }
    }

    public class CDNSettings
    {
        public static string CoreFileReadingServer
        {
            get
            {
                return AppConfiguration.GetAppsetting("CDN:CoreFileReadingServer");
            }
        }

        public static string CoreImgReadingUrl
        {
            get
            {
                return AppConfiguration.GetAppsetting("CDN:CoreImgReadingUrl");
            }
        }

        public static string SocialFileReadingServer
        {
            get
            {
                return AppConfiguration.GetAppsetting("CDN:SocialFileReadingServer");
            }
        }

        public static string SocialImgReadingUrl
        {
            get
            {
                return AppConfiguration.GetAppsetting("CDN:SocialImgReadingUrl");
            }
        }
    }

    public class MyCloudSettings
    {
        public static string MyCloudServer
        {
            get
            {
                return AppConfiguration.GetAppsetting("SystemSetting:MyCloudServer");
            }
        }

        public static string CommonHub
        {
            get
            {
                return AppConfiguration.GetAppsetting("SystemSetting:CommonHub");
            }
        }

        public static string ManagerHub
        {
            get
            {
                return AppConfiguration.GetAppsetting("SystemSetting:ManagerHub");
            }
        }
    }
}