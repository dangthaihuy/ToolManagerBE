using System;
using System.Collections.Generic;
using Autofac;
using VnCompany.DataLayer.Entities.Business;
using VnCompany.DataLayer.Stores.Business;
using VnCompany.SharedLibs;
using VnCompany.WebApp.Settings;
using Serilog;

namespace VnCompany.WebApp.Helpers
{
    public class HelperForm
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(HelperForm));

        public static IdentityForm GetBaseInfo(int id)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.Form, id);
            IdentityForm baseInfo = null;
            try
            {
                //Check from cache first
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                baseInfo = cacheProvider.Get<IdentityForm>(myKey);

                if (baseInfo == null)
                {
                    var myStore = Startup.IocContainer.Resolve<IStoreForm>();
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

        public static List<IdentityForm> GetList()
        {
            var myKey = string.Format(EnumFormatListCacheKeys.Forms);
            List<IdentityForm> returnList = new List<IdentityForm>();

            try
            {
                //Check from cache first
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                returnList = cacheProvider.Get<List<IdentityForm>>(myKey);

                
                if (!returnList.HasData())
                {
                    returnList = new List<IdentityForm>();

                    var myStore = Startup.IocContainer.Resolve<IStoreForm>();
                    var list = myStore.GetList();

                    if (list.HasData())
                    {
                        foreach (var item in list)
                        {
                            if(item.Id > 0)
                            {
                                var itemInfo = GetBaseInfo(item.Id);
                                if (itemInfo != null) {

                                    returnList.Add(itemInfo);
                                }
                            }
                        }

                        if (returnList.HasData())
                        {
                            //Storage to cache
                            cacheProvider.Set(myKey, returnList, SystemSettings.DefaultCachingTimeInMinutes);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Could not GetList: " + ex.ToString());
            }

            return returnList;
        }

        public static void ClearCache(int id)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.Form, id);
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