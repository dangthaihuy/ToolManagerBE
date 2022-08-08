using Manager.Business;
using Manager.WebApp.Services;

namespace Manager.WebApp.Settings
{
    public interface ISettings
    {
        GeneralSettings General { get; }
        CacheSettings Cache { get; }
        MailSettings Mail { get; }

    }

    public class SiteSettings : ISettings
    {
        private GeneralSettings _generalSettings;
        public GeneralSettings General { get { return _generalSettings; } }

        private CacheSettings _cacheSettings;
        public CacheSettings Cache { get { return _cacheSettings; } }

        private MailSettings _mailSettings;
        public MailSettings Mail { get { return _mailSettings; } }

        public SiteSettings()
        {
            //Init all settings -- loading after
            _generalSettings = new GeneralSettings();
            _cacheSettings = new CacheSettings();
            _mailSettings = new MailSettings();
        }

        public void Load(ISettingsService settingsService, bool useCache = false)
        {
            _generalSettings = CreateSettings<GeneralSettings>(settingsService, useCache);
            _cacheSettings = CreateSettings<CacheSettings>(settingsService, useCache);
            _mailSettings = CreateSettings<MailSettings>(settingsService, useCache);
        }

        public void Save(ISettingsService settingsService)
        {
            if (_generalSettings != null)
                _generalSettings.Save(settingsService);

            if (_cacheSettings != null)
                _cacheSettings.Save(settingsService);

            if (_mailSettings != null)
                _mailSettings.Save(settingsService);
        }

        public void Save(string settingTypeName, ISettingsService settingsService)
        {
            if (_generalSettings != null && _generalSettings.GetType().Name == settingTypeName)
            {
                _generalSettings.Save(settingsService);
                return;
            }

            if (_cacheSettings != null && _cacheSettings.GetType().Name == settingTypeName)
            {
                _cacheSettings.Save(settingsService);
                return;
            }

            if (_mailSettings != null && _mailSettings.GetType().Name == settingTypeName)
            {
                _mailSettings.Save(settingsService);
                return;
            }
        }

        private T CreateSettings<T>(ISettingsService settingsService, bool useCache) where T : SettingsBase, new()
        {
            var settings = new T();
            settings.Load(settingsService, useCache);
            return settings;
        }
    }

    public class BusinessSettings
    {
        private FTCompanySettings _ftCompanySettings;
        public FTCompanySettings FTCompany { get { return _ftCompanySettings; } }

        private FACompanySettings _faCompanySettings;
        public FACompanySettings FACompany { get { return _faCompanySettings; } }

        public BusinessSettings()
        {
            _ftCompanySettings = new FTCompanySettings();
            _faCompanySettings = new FACompanySettings();
        }

        public void Load(ISettingsService settingsService, bool useCache = false)
        {
            _ftCompanySettings = CreateSettings<FTCompanySettings>(settingsService, useCache);
            _faCompanySettings = CreateSettings<FACompanySettings>(settingsService, useCache);
        }

        public void Save(ISettingsService settingsService)
        {
            if (_ftCompanySettings != null)
                _ftCompanySettings.Save(settingsService);

            if (_faCompanySettings != null)
                _faCompanySettings.Save(settingsService);
        }

        public void Save(string settingTypeName, ISettingsService settingsService)
        {
            if (_ftCompanySettings != null && _ftCompanySettings.GetType().Name == settingTypeName)
            {
                _ftCompanySettings.Save(settingsService);
                return;
            }

            if (_faCompanySettings != null && _faCompanySettings.GetType().Name == settingTypeName)
            {
                _faCompanySettings.Save(settingsService);
                return;
            }
        }

        private T CreateSettings<T>(ISettingsService settingsService, bool useCache) where T : SettingsBase, new()
        {
            var settings = new T();
            settings.Load(settingsService, useCache);
            return settings;
        }
    }
}
