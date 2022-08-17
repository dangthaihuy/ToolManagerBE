﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.DataLayer.Entities.Business
{
    public class IdentityConversation 
    {
        public int Id { get; set; }
        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        public IdentityCurrentUser Receiver { get; set; }


    }

    public class IdentityConversationDefault
    {
        public int Id { get; set; }
        public int UserId1 { get; set; }
        public int UserId2 { get; set; }
        public int CreateBy { get; set; }
        public int DeleteByUser1 { get; set; }
        public int DeleteByUser2 { get; set; }
    }

   
    
}
