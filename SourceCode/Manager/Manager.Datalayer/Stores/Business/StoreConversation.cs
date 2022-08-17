using Manager.DataLayer.Entities.Business;
using Manager.DataLayer.Repositories.Business;
using Manager.SharedLibs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.DataLayer.Stores.Business
{
    public interface IStoreConversation
    {
        int Insert(IdentityConversationDefault identity);
        List<IdentityConversation> GetById(string Id);
        IdentityConversation GetDetail(string SenderId, string ReceiverId);
    }

    public class StoreConversation : IStoreConversation
    {
        private readonly string _conStr;
        private RpsConversation r;
        public StoreConversation() : this("MainDBConn")
        {
        }

        public StoreConversation(string connectionStringName)
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
            r = new RpsConversation(_conStr);
        }

        public List<IdentityConversation> GetById(string Id)
        {
            return r.GetById(Id);
        }

        public IdentityConversation GetDetail(string SenderId, string ReceiverId)
        {
            return r.GetDetail(SenderId, ReceiverId);
        }

        public int Insert(IdentityConversationDefault identity)
        {
            return r.Insert(identity);
        }

    }
}
