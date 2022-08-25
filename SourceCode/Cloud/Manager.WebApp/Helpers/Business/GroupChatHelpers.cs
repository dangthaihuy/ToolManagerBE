using Autofac;
using Manager.DataLayer.Entities.Business;
using Manager.DataLayer.Stores.Business;
using Manager.WebApp.Settings;
using Serilog;
using System;
using System.Collections.Generic;

namespace Manager.WebApp.Helpers.Business
{
    public class GroupChatHelpers
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(ConversationHelpers));

        public static IdentityGroup GetGroupInfo(IdentityConversation item)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.ConversationGroup, item.Id);

            IdentityGroup info = null;
            try
            {
                //Check the cache first (Find the product that has Id equal to id)
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();

                info = cacheProvider.Get<IdentityGroup>(myKey);

                if (info == null)
                {
                    var myStore = Startup.IocContainer.Resolve<IStoreGroup>();
                    info = myStore.GetById(Convert.ToString(item.Id))
;                   info.Member = myStore.GetUserById(item.Id);

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

                var myKey = string.Format(EnumFormatInfoCacheKeys.ConversationGroup, id);


                cacheProvider.Clear(myKey);

            }
            catch (Exception ex)
            {
                _logger.Error("Failed to ClearCache: {0}", ex.ToString());
            }
        }
    }
}
