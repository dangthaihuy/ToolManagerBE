using System.Collections.Generic;

namespace Manager.WebApp.Models.Business
{
    public class ConversationUserModel
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public List<string> UsersId { get; set; }
    }
}
