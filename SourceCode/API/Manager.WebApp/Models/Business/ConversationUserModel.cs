using System.Collections.Generic;

namespace Manager.WebApp.Models.Business
{
    public class ConversationUserModel
    {
        public int Id { get; set; }
        public int ConversationId { get; set; }
        public List<string> UsersId { get; set; }
        public int UserId { get; set; }
    }
}
