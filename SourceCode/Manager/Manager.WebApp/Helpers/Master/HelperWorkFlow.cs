using System;
using Autofac;
using Manager.DataLayer.Entities.Business;
using Manager.DataLayer.Stores.Business;
using Manager.WebApp.Settings;
using Serilog;

namespace Manager.WebApp.Helpers
{
    public class HelperWorkFlow
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(HelperWorkFlow));

        public static IdentityWorkFlow GetBaseInfo(int id)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.WorkFlow, id);
            IdentityWorkFlow baseInfo = null;
            try
            {
                //Check from cache first
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                baseInfo = cacheProvider.Get<IdentityWorkFlow>(myKey);

                if (baseInfo == null)
                {
                    var myStore = Startup.IocContainer.Resolve<IStoreWorkFlow>();
                    baseInfo = myStore.GetById(id);

                    if (baseInfo != null)
                    {
                        //Storage to cache
                        cacheProvider.Set(myKey, baseInfo, SystemSettings.DefaultCachingTimeInMinutes);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Could not GetBaseInfo: " + ex.ToString());
            }

            return baseInfo;
        }

        public static void ClearCache(int id)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.WorkFlow, id);
            try
            {
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                cacheProvider.Clear(myKey);
            }
            catch (Exception ex)
            {
                _logger.Error("Could not ClearCache: " + ex.ToString());
            }
        }        
    }
}