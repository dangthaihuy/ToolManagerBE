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
        int Insert(int groupId, int memberId);
        int Delete(int groupId, int memberId);
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
        public int Insert(int groupId, int memberId)
        {
            return r.Insert(groupId, memberId);
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

    }
}
