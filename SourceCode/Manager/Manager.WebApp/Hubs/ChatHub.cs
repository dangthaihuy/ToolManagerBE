using Manager.WebApp.Connection;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Manager.WebApp.Helpers.Business;
using Manager.WebApp.Hubs.Common;
using Manager.DataLayer.Entities.Business;
using Manager.SharedLibs;

namespace Manager.WebApp.Hubs
{

    public class ChatHub : BaseMessengerHub
    {
        

      /*  public override Task OnDisconnectedAsync(Exception exception)
        {
            if(_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {
                _connections.Remove(Context.ConnectionId);
                Clients.Group(userConnection.Room)
                    .SendAsync("ReceiveMessage", _botUser, $"{userConnection.User} has left");

            }
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string message)
        {
            if(_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {
                await Clients.Groups(userConnection.Room)
                    .SendAsync("ReceiveMessage", userConnection.User, message);
            }
        }*/

        



        public async Task SendToAll(string user, string message)
        {

            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        

        [HubMethodName("SendToUser")]
        public void SendRedirect(string SenderId, string ReceiverId, string Message)
        {
            
            var connectedUsers = MessengerHelpers.GetAllUsersFromCache();
            //Lấy người gửi trong cache
            var fromUser = connectedUsers.FirstOrDefault(x => x.Id == Utils.ConvertToInt32(SenderId));
            //Lấy người nhận trong cache
            var toUser = connectedUsers.FirstOrDefault(x => x.Id == Utils.ConvertToInt32(ReceiverId));
            if (connectedUsers != null && connectedUsers.Count > 0)
            {
                if (fromUser != null && fromUser.Connections.HasData())
                {
                    foreach (var senderConn in fromUser.Connections)
                    {
                        Clients.Client(senderConn.ConnectionId).SendAsync(Message);
                    }
                }

                if (toUser != null && toUser.Connections.HasData())
                {
                    foreach (var receiverConn in toUser.Connections)
                    {
                        // Broad cast message
                        Clients.Client(receiverConn.ConnectionId).SendAsync("ReceiveMessage",Message);
                    }
                }
            }

        }


    }
}
