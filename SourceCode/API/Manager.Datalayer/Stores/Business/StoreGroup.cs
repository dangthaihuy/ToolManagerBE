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
    public interface IStoreGroup
    {
        List<int> GetGroupIdByUserId(string id);
        IdentityGroup GetById(string id);
        List<IdentityCurrentUser> GetUserById(int id);
        int Insert(int groupId, int memberId);
        int Delete(int groupId, int memberId);
        
    }
    public class StoreGroup : IStoreGroup
    {
        private readonly string _conStr;
        private RpsGroup r;
        public StoreGroup() : this("MainDBConn")
        {
        }

        public StoreGroup(string connectionStringName)
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
            r = new RpsGroup(_conStr);
        }
        public IdentityGroup GetById(string id)
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
