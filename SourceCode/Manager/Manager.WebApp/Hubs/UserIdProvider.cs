using Manager.DataLayer.Entities;
using Microsoft.AspNet.SignalR.Hubs;

namespace Manager.WebApp.Hubs
{
    public interface IUserIdProvider
    {
        string GetUserId(Microsoft.AspNet.SignalR.IRequest request);
    }

    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(Microsoft.AspNet.SignalR.IRequest request)
        {
            

            return "1"; 
        }
    }
}

