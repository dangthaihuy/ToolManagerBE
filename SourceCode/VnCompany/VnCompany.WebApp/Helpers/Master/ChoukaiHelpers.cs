//using System;
//using Autofac;
//using VnCompany.DataLayer.Entities;
//using VnCompany.DataLayer.Stores;
//using System.Collections.Generic;
//using VnCompany.SharedLibs.Extensions;
//using Serilog;
//using VnCompany.WebApp.Settings;

//namespace VnCompany.WebApp.Helpers
//{
//    public class ChoukaiHelpers
//    {
//        public static List<IdentityChoukaiMondai> GetListChoukaiMondai()
//        {
//            var myKey = string.Format(EnumListCacheKeys.ChoukaiMondais);
//            List<IdentityChoukaiMondai> list = null;
//            try
//            {
//                //Check from cache first
//                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
//                list = cacheProvider.Get<List<IdentityChoukaiMondai>>(myKey);

//                if (list == null)
//                {
//                    var myStore = Startup.IocContainer.Resolve<IStoreChoukai>();
//                    list = myStore.GetListChoukaiMondai();

//                    if (list.HasData())
//                    {
//                        //Storage to cache
//                        cacheProvider.Set(myKey, list, SystemSettings.DefaultCachingTimeInMinutes);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Log.ForContext<ChoukaiHelpers>().Error("Could not GetList: " + ex.ToString());
//            }

//            return list;
//        }        
//    }
//}