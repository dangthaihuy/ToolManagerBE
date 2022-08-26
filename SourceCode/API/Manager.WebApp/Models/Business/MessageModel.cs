using System;

namespace Manager.WebApp.Models.Business
{
    public class MessageModel
    {
        public int Id { get; set; }
        public string Page { get; set; }
        public string Keyword { get; set; }
    }

    public class MessageCheckImportantModel
    {
        public int Id { get; set; }
        public int Important { get; set; }
    }

    public class SendMessageModel
    {
        public int ConversationId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public int MessageId { get; set; }
        public string Message { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
