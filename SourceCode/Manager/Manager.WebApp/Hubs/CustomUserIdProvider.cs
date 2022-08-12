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
            // your logic to fetch a user identifier goes here.
            // for example:
            
            return "1";
        }
    }
}
