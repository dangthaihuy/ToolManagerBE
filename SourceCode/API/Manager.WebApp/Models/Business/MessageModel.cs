using Manager.DataLayer.Entities.Business;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

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
        public int Id { get; set; }
        public int ConversationId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public int GroupId { get; set; }
        public int Type { get; set; }
        public string Message { get; set; }
        public int ReplyMessageId { get; set; }
        public List<string> UserIdsDeleted { get; set; }

        public IdentityMessageReply ReplyMessage { get; set; }
        public List<IdentityMessageAttachment> Attachments { get; set; }
        public bool IsFinal { get; set; }
        public DateTime CreatedDate { get; set; }
        
    }

    public class ReturnMessageModel
    {
        public string Type { get; set; }
        public string Code { get; set; }
    }
}