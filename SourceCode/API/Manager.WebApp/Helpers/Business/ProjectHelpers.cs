using Autofac;
using Manager.DataLayer.Entities.Business;
using Manager.DataLayer.Stores.Business;
using Manager.WebApp.Settings;
using Serilog;
using System;

namespace Manager.WebApp.Helpers.Business
{
    public class ProjectHelpers
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(MessengerHelpers));
        private static ICacheProvider _myCache;
        private static int _cacheExpiredTime = 10080;

        public static IdentityProject GetBaseInfoProject(int id)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.Project, id);

            IdentityProject info = null;
            try
            {
                //Check the cache first (Find the product that has Id equal to id)
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();

                info = cacheProvider.Get<IdentityProject>(myKey);

                if (info == null)
                {
                    var myStore = Startup.IocContainer.Resolve<IStoreProject>();
                    info = myStore.GetProjectById(id);

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

        public static IdentityTask GetBaseInfoTask(int id)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.Task, id);

            IdentityTask info = null;
            try
            {
                //Check the cache first (Find the product that has Id equal to id)
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();

                info = cacheProvider.Get<IdentityTask>(myKey);

                if (info == null)
                {
                    var myStore = Startup.IocContainer.Resolve<IStoreProject>();
                    info = myStore.GetTaskById(id);

                    if (info != null)
                    {
                        //Storage to cache
                        cacheProvider.Set(myKey, info, SystemSettings.DefaultCachingTimeInMinutes);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Could not GetBaseInfoTask: " + ex.ToString());
            }

            return info;
        }







        //Clear Cache
        public static void ClearCacheBaseInfoProject(int id)
        {
            try
            {
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();

                var myKey = string.Format(EnumFormatInfoCacheKeys.Project, id);


                cacheProvider.Clear(myKey);

            }
            catch (Exception ex)
            {
                _logger.Error("Failed to ClearCache: {0}", ex.ToString());
            }
        }

        public static void ClearCacheBaseInfoTask(int id)
        {
            try
            {
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();

                var myKey = string.Format(EnumFormatInfoCacheKeys.Task, id);


                cacheProvider.Clear(myKey);

            }
            catch (Exception ex)
            {
                _logger.Error("Failed to ClearCache: {0}", ex.ToString());
            }
        }
    }
}
