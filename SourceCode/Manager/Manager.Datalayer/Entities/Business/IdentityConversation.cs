using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.DataLayer.Entities.Business
{
    public class IdentityConversation
    {
    }

    public class IdentityConversationReply : IdentityCommon
    {
        public int Id { get; set; }

        public string ConversationId { get; set; }

        public string Message { get; set; }

        public int SenderId { get; set; }
        

        public int ReceiverId { get; set; }
        

        public DateTime? CreatedDate { get; set; }

        public int Status { get; set; }

        

        

    }
}
