﻿using System;
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

        public string Message { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }
        public DateTime CreateDate { get; set; }

    }
}