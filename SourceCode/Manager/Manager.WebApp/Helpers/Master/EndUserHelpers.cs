//using System;
//using Autofac;
//using Microsoft.AspNetCore.Http;
//using Manager.DataLayer.Entities;
//using Manager.DataLayer.Stores;
//using Manager.WebApp.Settings;
//using Serilog;

//namespace Manager.WebApp.Helpers
//{
//    public class EndUserHelpers
//    {
//        public static IdentityEndUser GetBaseInfo(int id)
//        {
//            var myKey = string.Format(EnumFormatInfoCacheKeys.EndUserBase, id);
//            IdentityEndUser baseInfo = null;
//            try
//            {
//                //Check from cache first
//                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
//                baseInfo = cacheProvider.Get<IdentityEndUser>(myKey);

//                if (baseInfo == null)
//                {
//                    var myStore = Startup.IocContainer.Resolve<IStoreEndUser>();
//                    baseInfo = myStore.GetById(id);

//                    if (baseInfo != null)
//                    {
//                        //Storage to cache
//                        cacheProvider.Set(myKey, baseInfo, SystemSettings.DefaultCachingTimeInMinutes);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Log.ForContext<EndUserHelpers>().Error("Could not GetBaseInfo: " + ex.ToString());
//            }

//            return baseInfo;
//        }

//        public static IdentityEndUser GetByUId(string _id)
//        {
//            IdentityEndUser baseInfo = null;
//            try
//            {
//                var myStore = Startup.IocContainer.Resolve<IStoreEndUser>();
//                baseInfo = myStore.GetByUid(_id);
//            }
//            catch (Exception ex)
//            {
//                Log.ForContext<EndUserHelpers>().Error("Could not GetByUId: " + ex.ToString());
//            }

//            return baseInfo;
//        }

//        //public IdentityEndUser GetCurrentUser()
//        //{
//        //    IdentityEndUser info = null;
//        //    try
//        //    {
//        //        var req = Request;
//        //        if (req.Headers["x-auth-token"] != null)
//        //        {
//        //            var token = req.Headers["x-auth-token"];
//        //            if (!string.IsNullOrEmpty(token))
//        //            {
//        //                info = Utility.DecodeJwtAuthTokenData(token, SystemSettings.JwtSecretKey);
//        //            }
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        Log.ForContext<EndUserHelpers>().Error("Could not GetCurrentUser because: {0}", ex.ToString());
//        //    }

//        //    return info;
//        //}

//        //public static IdentitySession GetCurrentSession()
//        //{
//        //    IdentitySession mySession = null;
//        //    try
//        //    {
//        //        var req = HttpContext.Current.Request;

//        //        var sFilter = new IdentitySession();
//        //        sFilter.ipAddress = req.UserHostAddress;
//        //        sFilter.userAgent = req.UserAgent;
//        //        if (!string.IsNullOrEmpty(sFilter.ipAddress) && !string.IsNullOrEmpty(sFilter.userAgent))
//        //        {
//        //            if (req.Headers["x-device-uid"] != null && req.Headers["x-device-platform"] != null)
//        //            {
//        //                sFilter.deviceUid = req.Headers["x-device-uid"];
//        //                sFilter.devicePlatform = req.Headers["x-device-platform"];
//        //            }
//        //            else
//        //            {
//        //                sFilter.devicePlatform = "browser";
//        //            }
//        //        }
//        //        else
//        //        {
//        //            if (req.Headers["x-device-uid"] != null && req.Headers["x-device-platform"] != null)
//        //            {
//        //                sFilter.deviceUid = req.Headers["x-device-uid"];
//        //                sFilter.devicePlatform = req.Headers["x-device-platform"];
//        //            }
//        //        }

//        //        //Key format: ip,agent,devicePlatform,deviceUid
//        //        var keyFormat = "SESSION_{0}_{1}_{2}_{3}";
//        //        var myKey = Utility.Md5HashingData(string.Format(keyFormat, sFilter.ipAddress, sFilter.userAgent, sFilter.devicePlatform, sFilter.deviceUid));

//        //        //Check from cache first
//        //        var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
//        //        cacheProvider.Get<IdentitySession>(myKey, out mySession);

//        //        if (mySession == null)
//        //        {
//        //            var sessionStore = Startup.IocContainer.Resolve<IStoreSession>();
//        //            mySession = sessionStore.GetByMetaData(sFilter);

//        //            if (mySession == null || string.IsNullOrEmpty(mySession._id))
//        //            {
//        //                mySession = sFilter;

//        //                //Create new session
//        //                mySession.tryExamLeft = 2;
//        //                mySession.deviceUid = sFilter.deviceUid;
//        //                mySession.devicePlatform = sFilter.devicePlatform;
//        //                mySession._id = myKey;
//        //                mySession.ipAddress = sFilter.ipAddress;
//        //                mySession.userAgent = sFilter.userAgent;

//        //                var returnSession = sessionStore.Insert(mySession);
//        //            }

//        //            if (mySession != null)
//        //            {
//        //                //Storage to cache
//        //                cacheProvider.Set(myKey, mySession, SystemSettings.DefaultCachingTimeInMinutes);
//        //            }
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        Log.ForContext<EndUserHelpers>().Error("Could not GetCurrentSession because: {0}", ex.ToString());
//        //    }

//        //    return mySession;
//        //}

//        //public IdentitySession ClearCurrentSession()
//        //{
//        //    IdentitySession mySession = null;
//        //    try
//        //    {
//        //        var req = HttpContext.Current.Request;

//        //        var sFilter = new IdentitySession();
//        //        sFilter.ipAddress = req.UserHostAddress;
//        //        sFilter.userAgent = req.UserAgent;
//        //        if (!string.IsNullOrEmpty(sFilter.ipAddress) && !string.IsNullOrEmpty(sFilter.userAgent))
//        //        {
//        //            if (req.Headers["x-device-uid"] != null && req.Headers["x-device-platform"] != null)
//        //            {
//        //                sFilter.deviceUid = req.Headers["x-device-uid"];
//        //                sFilter.devicePlatform = req.Headers["x-device-platform"];
//        //            }
//        //            else
//        //            {
//        //                sFilter.devicePlatform = "browser";
//        //            }
//        //        }
//        //        else
//        //        {
//        //            if (req.Headers["x-device-uid"] != null && req.Headers["x-device-platform"] != null)
//        //            {
//        //                sFilter.deviceUid = req.Headers["x-device-uid"];
//        //                sFilter.devicePlatform = req.Headers["x-device-platform"];
//        //            }
//        //        }

//        //        //Key format: ip,agent,devicePlatform,deviceUid
//        //        var keyFormat = "SESSION_{0}";
//        //        var idFormat = "{0}_{1}_{2}_{3}";
//        //        var uId = Utility.Md5HashingData(string.Format(idFormat, sFilter.ipAddress, sFilter.userAgent, sFilter.devicePlatform, sFilter.deviceUid));
//        //        var myKey = string.Format(keyFormat, uId);

//        //        var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
//        //        //Storage to cache
//        //        cacheProvider.Clear(myKey);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        Log.ForContext<EndUserHelpers>().Error("Could not ClearCurrentSession because: {0}", ex.ToString());
//        //    }

//        //    return mySession;
//        //}
//    }
//}