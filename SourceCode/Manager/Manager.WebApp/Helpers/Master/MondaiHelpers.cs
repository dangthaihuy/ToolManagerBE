//using System;
//using Autofac;
//using Manager.DataLayer.Entities;
//using Manager.DataLayer.Stores;
//using System.Collections.Generic;
//using Manager.SharedLibs.Extensions;
//using Serilog;
//using Manager.WebApp.Settings;

//namespace Manager.WebApp.Helpers
//{
//    public class MondaiHelpers
//    {
//        public static List<IdentityMondai> GetList()
//        {
//            var myKey = string.Format(EnumListCacheKeys.Mondais);
//            List<IdentityMondai> list = null;
//            try
//            {
//                //Check from cache first
//                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
//                list = cacheProvider.Get<List<IdentityMondai>>(myKey);

//                if (list == null)
//                {
//                    var myStore = Startup.IocContainer.Resolve<IStoreMondai>();
//                    list = myStore.GetList();

//                    if (list.HasData())
//                    {
//                        //Storage to cache
//                        cacheProvider.Set(myKey, list, SystemSettings.DefaultCachingTimeInMinutes);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Log.ForContext<MondaiHelpers>().Error("Could not GetList: " + ex.ToString());
//            }

//            return list;
//        }        
//    }
//}