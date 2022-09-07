using Autofac;
using Manager.DataLayer.Entities;
using Manager.DataLayer.Entities.Business;
using Manager.DataLayer.Stores.Business;
using Manager.DataLayer.Stores.System;
using Manager.WebApp.Helpers.Business;
using Manager.WebApp.Settings;
using Serilog;
using System;
using System.Collections.Generic;

namespace Manager.WebApp.Helpers
{
    public class ConversationHelpers
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(ConversationHelpers));

        public static IdentityInformationUser GetReceiverInfo(IdentityConversation item)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.Conversation, item.SenderId, item.ReceiverId);

            IdentityInformationUser info = null;
            try
            {
                //Check the cache first (Find the product that has Id equal to id)
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();

                info = cacheProvider.Get<IdentityInformationUser>(myKey);

                if (info == null)
                {
                    var myStore = Startup.IocContainer.Resolve<IAPIStoreUser>();
                    info = myStore.GetById(Convert.ToString(item.ReceiverId))
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

        

        public static IdentityMessage GetLastMessage(IdentityConversation item)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.ConversationLastMessage, item.Id);
            IdentityMessage info = null;

            try
            {
                //Check the cache first (Find the product that has Id equal to id)
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();

                info = cacheProvider.Get<IdentityMessage>(myKey);

                if (info == null)
                {
                    var myStore = Startup.IocContainer.Resolve<IStoreMessage>();
                    info = myStore.GetLastMessage(item.Id)
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

            var myKey = string.Format(EnumFormatInfoCacheKeys.ConversationLastMessage, id);


            cacheProvider.Clear(myKey);

        }
        catch (Exception ex)
        {
            _logger.Error("Failed to ClearCache: {0}", ex.ToString());
        }
        }
    }
}
