using System.Collections.Generic;

namespace Manager.WebApp.Models.Business
{
    public class Connector : Member
    {
        //public int agency_id { get; set; }

        public List<ConnectionInfo> Connections { get; set; }

        public bool has_logout { get { return false; } set { } }

        public Connector()
        {
            Connections = new List<ConnectionInfo>();
        }
    }
    public class Member
    {
        public int Id { get; set; }
        public string Fullname { get; set; }

        //public string Avatar { get; set; }

        public string ConnectionId { get; set; }
        
    }
    public class ConnectionInfo
    {
        public string ConnectionId { get; set; }
        
    }
}
