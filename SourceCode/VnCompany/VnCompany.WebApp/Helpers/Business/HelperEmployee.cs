using Autofac;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VnCompany.DataLayer.Entities.Business;
using VnCompany.DataLayer.Stores.Business;
using VnCompany.WebApp.Settings;

namespace VnCompany.WebApp.Helpers.Business
{
    public class HelperEmployee
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(HelperEmployee));

        public static IdentityEmployee GetBaseInfo(int id)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.Employee, id);

            IdentityEmployee info = null;
            try
            {
                //Check the cache first
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();

                info = cacheProvider.Get<IdentityEmployee>(myKey);

                if (info == null)
                {
                    var myStore = Startup.IocContainer.Resolve<IStoreEmployee>();
                    info = myStore.GetById(id);

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

        public static IdentityEmployeeContact GetContactInfo(int id)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.EmployeeContact, id);

            IdentityEmployeeContact info = null;
            try
            {
                //Check the cache first
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();

                info = cacheProvider.Get<IdentityEmployeeContact>(myKey);

                if (info == null)
                {
                    var myStore = Startup.IocContainer.Resolve<IStoreContact>();
                    info = myStore.GetByEmployeeId(id);

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
                var myEmployeeKey = string.Empty;
                var myContactKey = string.Empty;

                if (id > 0)
                    myEmployeeKey = string.Format(EnumFormatInfoCacheKeys.Employee, id);
                    myContactKey = string.Format(EnumFormatInfoCacheKeys.EmployeeContact, id);

                cacheProvider.Clear(myEmployeeKey);
                cacheProvider.Clear(myContactKey);
            }
            catch (Exception ex)
            {              
                _logger.Error("Failed to ClearCache: {0}", ex.ToString());
            }
        }
    }
}
