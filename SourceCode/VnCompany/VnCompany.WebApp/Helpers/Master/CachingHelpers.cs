using System;
using Autofac;
using VnCompany.SharedLibs;
using Serilog;

namespace VnCompany.WebApp.Helpers
{
    public interface ICacheProvider
    {
        T Get<T>(string key);
        void Set(string key, object data, int minutes = 30);
        void Clear(string key);
        void ClearByPrefix(string prefix);
    }

    public class CacheProvider : ICacheProvider
    {
        public void Clear(string key)
        {
            try
            {
                Startup.RedisCacheClient.GetDbFromConfiguration().RemoveAsync(key);
            }
            catch (Exception ex)
            {
                Log.ForContext<CacheProvider>().Error(ex, "Could not Clear because: {0}");
            }
        }

        public void ClearByPrefix(string prefix)
        {
            try
            {
                var rdb = Startup.RedisCacheClient.GetDbFromConfiguration();
                rdb.RemoveAsync(prefix + "*");

                var keys = rdb.SearchKeysAsync(prefix + "*").Result;
                if(keys != null)
                {
                    rdb.RemoveAllAsync(keys);
                }                
            }
            catch (Exception ex)
            {
                Log.ForContext<CacheProvider>().Error(ex, "Could not ClearByPrefix because: {0}");
            }
        }

        public T Get<T>(string key)
        {
            try
            {
               return Startup.RedisCacheClient.GetDbFromConfiguration().GetAsync<T>(key).Result;
            }
            catch (Exception ex)
            {
                Log.ForContext<CacheProvider>().Error(ex, "Could not Get because: {0}");
            }

            return default(T);
        }

        public void Set(string key, object data, int minutes)
        {
            try
            {
                if (minutes == 0)
                    minutes = Utils.ConvertToInt32(AppConfiguration.GetAppsetting("DefaultCacheTimeInMinutes"), 30);

                var offset = TimeSpan.FromMinutes(minutes);

                Startup.RedisCacheClient.GetDbFromConfiguration().AddAsync(key, data, offset);
            }
            catch (Exception ex)
            {
                Log.ForContext<CacheProvider>().Error(ex, "Could not Set because: {0}");
            }
        }
    }

    public class CachingHelpers
    {
        public static void ClearCacheByKey(string key)
        {
            try
            {
                //var cacheProvider = GlobalContainer.IocContainer.Resolve<ICacheProvider>();
                //cacheProvider.Clear(key);               
            }
            catch (Exception ex)
            {
                Log.ForContext<CacheProvider>().Error("Could not ClearCacheByKey: " + ex.ToString());
            }
        }

        public static void ClearCacheByPrefix(string prefix)
        {
            try
            {
                //var cacheProvider = GlobalContainer.IocContainer.Resolve<ICacheProvider>();
                //cacheProvider.ClearAll(prefix);
            }
            catch (Exception ex)
            {
                Log.ForContext<CacheProvider>().Error("Could not ClearCacheByPrefix: " + ex.ToString());
            }
        }

        public static void ClearUserCache(string cacheKey = "")
        {
            var strError = string.Empty;
            try
            {
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                if (string.IsNullOrEmpty(cacheKey))
                    cacheProvider.ClearByPrefix("USERS");
                else
                    cacheProvider.Clear(string.Format("USER_{0}", cacheKey));
            }
            catch (Exception ex)
            {
                Log.ForContext<CacheProvider>().Error(ex, "Could not ClearUserCache because: {0}");
            }
        }
    }
}