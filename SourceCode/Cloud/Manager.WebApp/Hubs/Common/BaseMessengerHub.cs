using Autofac;
using Manager.DataLayer.Stores.Business;
using Manager.SharedLibs;
using Manager.WebApp.Helpers.Business;
using Manager.WebApp.Models.Business;
using Microsoft.AspNetCore.SignalR;

using Serilog;
using System;
using System.Threading.Tasks;

namespace Manager.WebApp.Hubs.Common
{
    public class BaseMessengerHub : Hub
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(MessengerHelpers));
        private static readonly IStoreGroup storeGroup = Startup.IocContainer.Resolve<IStoreGroup>();
        public void Connect(string SenderId)
        {
            int Id = Utils.ConvertToInt32(SenderId);
            var connectionId = Context.ConnectionId;

            try
            {
                var newConnector = new Connector();
                newConnector.ConnectionId = connectionId;
                if (SenderId != null)
                {
                    newConnector.Id = Id;
                    
                    var con = new ConnectionInfo();
                    con.ConnectionId = connectionId;
                    newConnector.Connections.Add(con);
                }
                if(newConnector.Id > 0)
                {
                    var listUser = MessengerHelpers.AddUserToCache(newConnector);
                }
            }
            catch(Exception ex)
            {
                _logger.Error("Could not connect: " + ex.ToString());

            }
        }

        
    }
}
