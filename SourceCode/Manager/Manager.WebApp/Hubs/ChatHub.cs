using Manager.WebApp.Connection;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.WebApp.Hubs
{

    /*    public interface IUserIdProvider
        {
            string GetUserId(IRequest request);
        }*/
    


    public class ChatHub : Hub
    {
        /*private readonly string _botUser;
        private readonly IDictionary<string, UserConnection> _connections;
        public ChatHub(IDictionary<string, UserConnection> connections)
        {
            _botUser = "Mychat Bot";
            _connections = connections;
        }
*/
        


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

        



            // Chat one to one
        [HubMethodName("SendMessageToUser")]
        public async Task DirectMessage(string user, string message)
        {

            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public void Send(string userId, string message)
        {
            Clients.User(userId).SendAsync(message);
        }


    }
}
