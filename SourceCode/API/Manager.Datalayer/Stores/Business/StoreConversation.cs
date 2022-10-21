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
        int InsertGroup(IdentityConversationDefault identity);
        List<IdentityConversation> GetById(string id);
        List<IdentityConversation> GetGroupByUserId(string id);
        IdentityConversation GetDetail(int senderId, int receiverId);
        int Delete(int id);
        IdentityConversationDefault Update(IdentityConversationUpdate identity);
        int GetReceiverById(IdentityConversationUser identity);
        string GetFile(IdentityGetFile identity);
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

        public List<IdentityConversation> GetById(string id)
        {
            return r.GetById(id);
        }

        public List<IdentityConversation> GetGroupByUserId(string id)
        {
            return r.GetGroupByUserId(id);
        }

        public IdentityConversation GetDetail(int senderId, int receiverId)
        {
            return r.GetDetail(senderId, receiverId);
        }

        public int Insert(IdentityConversationDefault identity)
        {
            return r.Insert(identity);
        }
        public int InsertGroup(IdentityConversationDefault identity)
        {
            return r.InsertGroup(identity);
        }
        public int Delete(int id)
        {
            return r.Delete(id);
        }

        public IdentityConversationDefault Update(IdentityConversationUpdate identity)
        {
            return r.Update(identity);
        }
        public int GetReceiverById(IdentityConversationUser identity)
        {
            return r.GetReceiverById(identity);
        }
        public string GetFile(IdentityGetFile identity)
        {
            return r.GetFile(identity);
        }


    }
}
