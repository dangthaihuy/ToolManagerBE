using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.DataLayer.Entities.Business
{
    public class IdentityConversationNavbar : IdentityConversation
    {
        
        public string LastMessage { get; set; }
        public DateTime LastTime { get; set; }


    }
    public class IdentityConversation 
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public int Type { get; set; }

        public int LastMessageId { get; set; }
        public string LastMessage { get; set; }
        public DateTime LastTime { get; set; }
        public bool IsRead { get; set; }
        public IdentityInformationUser Receiver { get; set; }
        public IdentityConversationUser Group { get; set; }

    }

    public class IdentityConversationDefault
    {
        public int Id { get; set; }
        public int CreatedBy { get; set; }
        public int ReceiverId { get; set; }
        public int DeleteByUser1 { get; set; }
        public int DeleteByUser2 { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
    }

   public class IdentityConversationUpdate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string AvatarPath { get; set; }

    }

}
