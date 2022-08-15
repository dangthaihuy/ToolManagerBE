using Autofac;
using Manager.DataLayer.Entities;
using Manager.DataLayer.Entities.Business;
using Manager.DataLayer.Stores.Business;
using Manager.DataLayer.Stores.System;
using Manager.WebApp.Settings;
using Serilog;
using System;
using System.Collections.Generic;

namespace Manager.WebApp.Helpers.Business
{
    public class ConversationHelpers
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(ConversationHelpers));

        public static IdentityCurrentUser GetBaseInfo(string id)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.Conversation, id);

            IdentityCurrentUser info = null;
            try
            {
                //Check the cache first (Find the product that has Id equal to id)
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();

                info = cacheProvider.Get<IdentityCurrentUser>(myKey);

                if (info == null)
                {
                    var myStore = Startup.IocContainer.Resolve<IAPIStoreUser>();
                    info = myStore.GetById(id)
;

                    if (info != null)
                    {
                        //Storage to cache
                        cacheProvider.Set(myKey, info, SystemSettings.DefaultCachingTimeInMinutes);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Could not GetBaseInfo: " + ex.ToString());
            }

            return info;
        }

        public static void ClearCache(int id)
        {
            try
            {
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();

                var productKey = string.Format(EnumFormatInfoCacheKeys.Conversation);


                cacheProvider.Clear(productKey);

            }
            catch (Exception ex)
            {
                _logger.Error("Failed to ClearCache: {0}", ex.ToString());
            }
        }
    }
}
