using VnCompany.DataLayer.Entities;

namespace VnCompany.WebApp.Models
{
    public class NotificationItemModel
    {
        public IdentityNotification NotifInfo { get; set; }
    }

    public class NotificationMarkIsReadModel
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public bool IsRead { get; set; }
    }
}