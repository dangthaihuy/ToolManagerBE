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
    public class HelperMenuService
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(HelperMenuService));

        public static IdentityMenuService GetBaseInfo(int id)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.Form, id);
            IdentityMenuService baseInfo = null;
            try
            {
                //Check from cache first
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
                baseInfo = cacheProvider.Get<IdentityMenuService>(myKey);

                if (baseInfo == null)
                {
                    var myStore = Startup.IocContainer.Resolve<IStoreMenuService>();
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

        //public static List<IdentityMenuService> GetList()
        //{
        //    var myKey = string.Format(EnumFormatListCacheKeys.Forms);
        //    List<IdentityMenuService> returnList = new List<IdentityMenuService>();

        //    try
        //    {
        //        //Check from cache first
        //        var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
        //        returnList = cacheProvider.Get<List<IdentityMenuService>>(myKey);

                
        //        if (!returnList.HasData())
        //        {
        //            returnList = new List<IdentityMenuService>();

        //            var myStore = Startup.IocContainer.Resolve<IStoreMenuService>();
        //            var list = myStore.GetList();

        //            if (list.HasData())
        //            {
        //                foreach (var item in list)
        //                {
        //                    if(item.Id > 0)
        //                    {
        //                        var itemInfo = GetBaseInfo(item.Id);
        //                        if (itemInfo != null) {

        //                            returnList.Add(itemInfo);
        //                        }
        //                    }
        //                }

        //                if (returnList.HasData())
        //                {
        //                    //Storage to cache
        //                    cacheProvider.Set(myKey, returnList, SystemSettings.DefaultCachingTimeInMinutes);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error("Could not GetList: " + ex.ToString());
        //    }

        //    return returnList;
        //}

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