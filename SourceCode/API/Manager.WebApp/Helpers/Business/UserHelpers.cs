using Autofac;
using Manager.DataLayer.Entities;
using Manager.DataLayer.Stores.Business;
using Manager.DataLayer.Stores.System;
using Manager.WebApp.Settings;
using Serilog;
using System;

namespace Manager.WebApp.Helpers.Business
{
    public class UserHelpers
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(UserHelpers));

        private static ICacheProvider _myCache;
        private static int _cacheExpiredTime = 10080;

        public static IdentityInformationUser GetBaseInfo(int id)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.User, id);

            IdentityInformationUser info = null;
            try
            {
                //Check the cache first (Find the product that has Id equal to id)
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();

                info = cacheProvider.Get<IdentityInformationUser>(myKey);

                if (info == null)
                {
                    var myStore = Startup.IocContainer.Resolve<IApiStoreUser>();
                    
                    info = myStore.GetById(id.ToString());

                    if (info != null)
                    {
                        //Storage to cache
                        
                        cacheProvider.Set(myKey, info, SystemSettings.DefaultCachingTimeInMinutes);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Could not GetBaseInfoProject: " + ex.ToString());
            }

            return info;
        }
    }
}
