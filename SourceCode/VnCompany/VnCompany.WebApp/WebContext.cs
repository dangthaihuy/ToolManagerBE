using System;
using System.Web;
using Autofac;
using VnCompany.WebApp.Settings;
using VnCompany.WebApp.Services;
using VnCompany.DataLayer.Entities;
using System.Collections.Generic;

namespace VnCompany.WebApp
{
    /// <summary>
    /// Working with data in HttpContext.Current
    /// </summary>
    public static class WebContext
    {
        #region System Settings

        public static SiteSettings GetSiteSettings()
        {
            return GetSiteSettings(true);
        }

        public static SiteSettings GetSiteSettings(bool useHttpContext)
        {
            if (useHttpContext)
            {
                return GetSiteSettingsFromContext();
            }
            else
            {
                return GetSiteSettingsFromCache();
            }
        }


        private static SiteSettings GetSiteSettingsFromContext()
        {
            if (HttpContext.Current == null) return GetSiteSettingsFromCache();

            string contextKey = "SiteSettings";

            SiteSettings siteSettings = HttpContext.Current.Items[contextKey] as SiteSettings;
            if (siteSettings == null)
            {
                siteSettings = GetSiteSettingsFromCache();
                if (siteSettings != null)
                    HttpContext.Current.Items[contextKey] = siteSettings;
            }
            return siteSettings;
        }

        private static Settings.SiteSettings GetSiteSettingsFromCache()
        {
            try
            {
                ISettingsService settingService = Startup.IocContainer.Resolve<ISettingsService>();
                SiteSettings siteSettings = new SiteSettings();
                siteSettings.Load(settingService, true);
                return siteSettings;
            }
            catch (Exception ex)
            {
                return new SiteSettings();
            }

        }

        public static BusinessSettings GetBusinessSettings()
        {
            return GetBusinessSettings(true);
        }

        public static BusinessSettings GetBusinessSettings(bool useHttpContext)
        {
            if (useHttpContext)
            {
                return GetBusinessSettingsFromContext();
            }
            else
            {
                return GetBusinessSettingsFromCache();
            }
        }

        private static BusinessSettings GetBusinessSettingsFromContext()
        {
            if (HttpContext.Current == null) return GetBusinessSettingsFromCache();

            string contextKey = "BusinessSettings";

            BusinessSettings siteSettings = HttpContext.Current.Items[contextKey] as BusinessSettings;
            if (siteSettings == null)
            {
                siteSettings = GetBusinessSettingsFromCache();
                if (siteSettings != null)
                    HttpContext.Current.Items[contextKey] = siteSettings;
            }
            return siteSettings;
        }

        private static Settings.BusinessSettings GetBusinessSettingsFromCache()
        {
            try
            {
                ISettingsService settingService = Startup.IocContainer.Resolve<ISettingsService>();
                BusinessSettings siteSettings = new BusinessSettings();
                siteSettings.Load(settingService, true);
                return siteSettings;
            }
            catch (Exception ex)
            {
                return new BusinessSettings();
            }

        }

        #endregion
    }
}
