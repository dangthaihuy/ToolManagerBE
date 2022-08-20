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
        public string LastMessage { get; set; }
        public DateTime LastTime { get; set; }

        public IdentityCurrentUser Receiver { get; set; }
        public IdentityGroup Group { get; set; }


    }

    public class IdentityConversationDefault
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public int ReceiverId { get; set; }
        public int CreateBy { get; set; }
        public int DeleteByUser1 { get; set; }
        public int DeleteByUser2 { get; set; }
        public int Type { get; set; }
    }

   
    
}
