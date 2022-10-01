using Manager.DataLayer.Entities;
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
    public interface IStoreConversationUser
    {
        List<int> GetGroupIdByUserId(string id);
        IdentityConversationUser GetById(string id);
        List<IdentityCurrentUser> GetUserById(int id);
        int Insert(int groupId, int memberId, int type);
        int Delete(int groupId, int memberId);
        bool GetIsRead(IdentityConversation identity);
        int UpdateRead(IdentityConversationUser identity);

        List<int> GetUsersReadConversation(int conversationId);


    }
    public class StoreConversationUser : IStoreConversationUser
    {
        private readonly string _conStr;
        private RpsConversationUser r;
        public StoreConversationUser() : this("MainDBConn")
        {
        }

        public StoreConversationUser(string connectionStringName)
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
            r = new RpsConversationUser(_conStr);
        }
        public IdentityConversationUser GetById(string id)
        {
            return r.GetById(id);
        }
        public int Insert(int groupId, int memberId, int type)
        {
            return r.Insert(groupId, memberId, type);
        }
        public int Delete(int groupId, int memberId)
        {
            return r.Delete(groupId, memberId);
        }
        
        public List<IdentityCurrentUser> GetUserById(int id)
        {
            return r.GetUserById(id);
        }

        public List<int> GetGroupIdByUserId(string id)
        {
            return r.GetGroupIdByUserId(id);
        }

        public bool GetIsRead(IdentityConversation identity)
        {
            return r.GetIsRead(identity);
        }
        public int UpdateRead(IdentityConversationUser identity)
        {
            return r.UpdateRead(identity);
        }
        public List<int> GetUsersReadConversation(int conversationId)
        {
            return r.GetUsersReadConversation(conversationId);
        }

    }
}
