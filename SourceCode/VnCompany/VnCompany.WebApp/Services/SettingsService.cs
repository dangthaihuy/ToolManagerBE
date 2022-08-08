using System.Collections.Generic;
using System.Threading.Tasks;
using VnCompany.DataLayer.Stores;
using VnCompany.DataLayer.Entities;
using VnCompany.WebApp.Helpers;

namespace VnCompany.WebApp.Services
{
    public class SettingsService : ServiceBase, ISettingsService
    {
        const string KEY_PREFIX = "SETTINGS.";
        const string KEY_SETTINGS_BY_TYPES = KEY_PREFIX + ".TYPES.{0}";

        //private readonly IOracleEpgStore _epgStore;
        private readonly IStoreSetting _myStore;
        private readonly ICacheProvider _cache;

        public SettingsService(IStoreSetting myStore, ICacheProvider cacheProvider)
        {
            _myStore = myStore;
            _cache = cacheProvider;
        }

        public virtual Task<List<IdentitySetting>> LoadSettings(string settingType, bool useCache = true)
        {
            List<IdentitySetting> result;
            if (useCache)
            {
                string cacheKey = string.Format(KEY_SETTINGS_BY_TYPES, settingType);
                result = _cache.Get<List<IdentitySetting>>(cacheKey);

                if (result == null)
                {
                    result = _myStore.LoadSettings(settingType);
                    _cache.Set(cacheKey, result);
                }
            }
            else
            {
                result = _myStore.LoadSettings(settingType);
            }

            return Task.FromResult<List<IdentitySetting>>(result);
        }


        public virtual Task<bool> SaveSettings(List<IdentitySetting> settings)
        {
            this.ClearAllCacheData();
            var savedResult = _myStore.SaveSettings(settings);

            return Task.FromResult<bool>(savedResult);
        }

        public virtual void ClearAllCacheData()
        {
            _cache.ClearByPrefix(KEY_PREFIX);
        }
    }

    public interface ISettingsService
    {
        Task<List<IdentitySetting>> LoadSettings(string settingType, bool useCache = true);

        Task<bool> SaveSettings(List<IdentitySetting> settings);

        void ClearAllCacheData();
    }
}
