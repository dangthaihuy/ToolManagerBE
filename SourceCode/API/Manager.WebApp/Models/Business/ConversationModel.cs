using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.WebApp.Models.Business
{
    public class ConversationModel
    {
        public int Id { get; set; }
        public int CreatedBy { get; set; }
        public int ReceiverId { get; set; }
        public int DeleteByUser1 { get; set; }

        public int DeleteByUser2 { get; set; }
        public bool IsRead { get; set; }
        public List<string> MemberGroup { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }

    }

    public class ConversationUpdateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // public IFormFile File { get; set; }
    }
}
