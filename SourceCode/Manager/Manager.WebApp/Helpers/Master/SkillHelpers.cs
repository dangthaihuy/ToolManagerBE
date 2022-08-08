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
//    public class SkillHelpers
//    {
//        public static List<IdentitySkill> GetList()
//        {
//            var myKey = string.Format(EnumListCacheKeys.Skills);
//            List<IdentitySkill> list = null;
//            try
//            {
//                //Check from cache first
//                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
//                list = cacheProvider.Get<List<IdentitySkill>>(myKey);

//                if (list == null)
//                {
//                    var myStore = Startup.IocContainer.Resolve<IStoreSkill>();
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
//                Log.ForContext<SkillHelpers>().Error("Could not GetList: " + ex.ToString());
//            }

//            return list;
//        }        
//    }
//}