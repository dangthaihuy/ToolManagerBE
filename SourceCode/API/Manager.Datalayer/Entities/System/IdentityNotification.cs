using System;

namespace Manager.DataLayer.Entities
{
    public class IdentityNotificationBase : IdentityCommon
    {
        public int Id { get; set; }
        public int ActionType { get; set; }
        public int ActionId { get; set; }
        public int SenderId { get; set; }
        public int TargetType { get; set; }
        public int TargetId { get; set; }
        public string Content { get; set; }        

        public DateTime? CreatedDate { get; set; }

        //Extensions
        public int CustomerId { get; set; }
        public bool IsUpdateView { get; set; }
        public int FactoryId { get; set; }
        public int WarehouseId { get; set; }
    }

    public class IdentityNotification : IdentityNotificationBase
    {
        public int UserType { get; set; }
        public int UserId { get; set; }
        public int NotificationId { get; set; }
        public bool IsViewed { get; set; }
        public bool IsRead { get; set; }
        public int Status { get; set; }
        public DateTime? ReadDate { get; set; }
    }
}
