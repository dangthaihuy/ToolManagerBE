using Autofac;
using Manager.DataLayer.Entities;
using Manager.DataLayer.Entities.Business;
using Manager.DataLayer.Stores.Business;
using Manager.WebApp.Models.Business;
using Manager.WebApp.Settings;
using Serilog;
using System;
using System.Collections.Generic;

namespace Manager.WebApp.Helpers.Business
{
    public class MessengerHelpers
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(MessengerHelpers));
        private static ICacheProvider _myCache;
        private static string _allUsersCacheKey = string.Format("MESSENGER_USERS");
        private static int _cacheExpiredTime = 10080;


        // TIN NHẮN 1-1
        public static List<Connector> GetAllUsersFromCache()
        {
            var strError = string.Empty;
            List<Connector> listUser = null;

            try
            {
                _myCache = Startup.IocContainer.Resolve<ICacheProvider>();
                listUser = _myCache.Get<List<Connector>>(_allUsersCacheKey);

            }
            catch (Exception ex)
            {
                _logger.Error("Could not get list type: " + ex.ToString());
            }
            if (listUser == null)
                listUser = new List<Connector>();
            return listUser;
        }

        public static List<Connector> AddUserToCache(Connector newUser)
        {
            var existed = false;
            var listUser = GetAllUsersFromCache();
            try
            {
                if (listUser != null && listUser.Count > 0)
                {
                    //Remove if any
                    //listUser.RemoveAll(x => x.UserId == newUser.UserId);
                    foreach (var item in listUser)
                    {
                        if (newUser.Id == item.Id)
                        {
                            //set lại connectionId
                            item.ConnectionId = newUser.ConnectionId;
                            
                            // thêm connection
                            item.Connections.AddRange(newUser.Connections);
                            existed = true;
                            break;
                        }
                    }
                }

                if (!existed)
                    //Add new user
                    listUser.Add(newUser);

                //Save users
                _myCache.Set(_allUsersCacheKey, listUser, _cacheExpiredTime);
            }
            catch (Exception ex)
            {
                _logger.Error("Could not get list type: " + ex.ToString());
            }

            return listUser;
        }

        public static void StorageListUsersToCache(List<Connector> listUsers)
        {
            var strError = string.Empty;
            try
            {
                _myCache = Startup.IocContainer.Resolve<ICacheProvider>();

                //Save to cache
                _myCache.Set(_allUsersCacheKey, listUsers, _cacheExpiredTime);
            }
            catch (Exception ex)
            {
                _logger.Error("Could not get list type: " + ex.ToString());
            }
        }

        
        public static void ClearCache()
        {
            try
            {
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();

                var myKey = string.Format(_allUsersCacheKey);


                cacheProvider.Clear(myKey);

            }
            catch (Exception ex)
            {
                _logger.Error("Failed to ClearCache: {0}", ex.ToString());
            }
        }



    }
}
