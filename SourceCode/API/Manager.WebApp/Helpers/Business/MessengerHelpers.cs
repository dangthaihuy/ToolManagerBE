using Autofac;
using Manager.DataLayer.Entities;
using Manager.DataLayer.Entities.Business;
using Manager.DataLayer.Stores.Business;
using Manager.SharedLibs;
using Manager.WebApp.Models.Business;
using Manager.WebApp.Settings;
using Microsoft.AspNetCore.SignalR.Client;
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

        public static IdentityMessage GetBaseInfo(int id)
        {
            var myKey = string.Format(EnumFormatInfoCacheKeys.Message, id);

            IdentityMessage info = null;
            try
            {
                //Check the cache first (Find the product that has Id equal to id)
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();

                info = cacheProvider.Get<IdentityMessage>(myKey);

                if (info == null)
                {
                    var myStore = Startup.IocContainer.Resolve<IStoreMessage>();
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

        public static void ClearCacheBaseInfo(int id)
        {
            try
            {
                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();

                var myKey = string.Format(EnumFormatInfoCacheKeys.Message, id);


                cacheProvider.Clear(myKey);

            }
            catch (Exception ex)
            {
                _logger.Error("Failed to ClearCache: {0}", ex.ToString());
            }
        }


        public static void NotifNewGroupMessage(IdentityMessage msg)
        {
            try
            {
                var apiGroupMsg = new SendMessageModel();
                apiGroupMsg = msg.MappingObject<SendMessageModel>();
                apiGroupMsg.CreatedDate = DateTime.Now;

                var connBuilder = new HubConnectionBuilder();
                connBuilder.WithUrl(string.Format("{0}/chat", SystemSettings.MessengerCloud));
                connBuilder.WithAutomaticReconnect(); //I don't think this is totally required, but can't hurt either

                var conn = connBuilder.Build();

                //Start the connection
                var t = conn.StartAsync();

                //Wait for the connection to complete
                t.Wait();

                //Make your call - but in this case don't wait for a response 
                conn.InvokeAsync("SendToGroup", apiGroupMsg);

            }
            catch (Exception ex)
            {
                _logger.Error("Could not NotifNewGroupMessage: " + ex.ToString());

            }
        }

        public static void NotifNewPrivateMessage(IdentityMessage msg)
        {
            try
            {
                var apiPrivateMsg = new SendMessageModel();
                apiPrivateMsg = msg.MappingObject<SendMessageModel>();
                apiPrivateMsg.CreatedDate = DateTime.Now;

                var connBuilder = new HubConnectionBuilder();
                connBuilder.WithUrl(string.Format("{0}/chat", SystemSettings.MessengerCloud));
                connBuilder.WithAutomaticReconnect(); //I don't think this is totally required, but can't hurt either

                var conn = connBuilder.Build();

                //Start the connection
                var t = conn.StartAsync();

                //Wait for the connection to complete
                t.Wait();

                _logger.Error("Begin Invoke SendToUser");

                //Make your call - but in this case don't wait for a response 
                conn.InvokeAsync("SendToUser", apiPrivateMsg);

            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to NotifNewPrivateMessage because: {0}", ex.ToString());
                _logger.Error(strError);
            }
        }
    }
}
