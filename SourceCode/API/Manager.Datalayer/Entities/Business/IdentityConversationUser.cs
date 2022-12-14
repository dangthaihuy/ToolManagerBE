using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.DataLayer.Entities.Business
{
    public class IdentityConversationUser
    {
        public int Id { get; set; }
        public int CreatedBy { get; set; }
        public int ConversationId { get; set; }
        public int Type { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public List<IdentityCurrentUser> Member { get; set; }
    }
}
