using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.DataLayer.Entities.Business
{

    public class IdentityMessageFilter : IdentityMessage
    {
        public string Keyword { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }
    }
    public class IdentityMessage
    {
        public int Id { get; set; }

        public int ConversationId { get; set; }
        public int Type { get; set; }

        public string Message { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }
        public int ReplyMessageId { get; set; }
        public IdentityMessageReply ReplyMessage { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Important { get; set; }
        public List<IdentityMessageAttachment> Attachments { get; set; }

    }
    public class IdentityMessageAttachment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MessageId { get; set; }
        public string Path { get; set; }
    }

    public class IdentityMessageReply
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public List<IdentityMessageAttachment> Attachments { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
