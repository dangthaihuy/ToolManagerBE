using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.DataLayer.Entities.Business
{

    public class IdentityMessageFilter : IdentityMessage
    {
        public int Direction { get; set; }
        public int CurrentId { get; set; }
        public bool IsMore { get; set; }

    }
    public class IdentityMessage : CommonIdentity
    {
        public int Id { get; set; }
        public int ConversationId { get; set; }
        public int Type { get; set; }
        public string Message { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Important { get; set; }
        public int ReplyMessageId { get; set; }
        public List<string> UserIdsDeleted { get; set; }
        public IdentityMessageReply ReplyMessage { get; set; }
        public int TotalCount { get; set; }
        public int TaskId { get; set; }
        public int PageIndex { get; set; }
        public List<IdentityMessageAttachment> Attachments { get; set; }
        public List<IdentityInformationUser> Users { get; set; }
    }

    public class IdentityMessageReply
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public List<IdentityMessageAttachment> Attachments { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class IdentityMessageAttachment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ConversationId { get; set; }
        public int MessageId { get; set; }
        public string Path { get; set; }
    }
}
