using Manager.DataLayer.Entities;
using Manager.Datalayer.Repositories;
using Manager.SharedLibs;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Manager.DataLayer;

namespace Manager.DataLayer.Stores
{
    public interface IStoreUser
    {
        List<IdentityUser> GetByPage(IdentityUser filter, int currentPage, int pageSize);
        string Insert(IdentityUser identity);
        bool Update(IdentityUser identity);
        bool ChangePassword(IdentityUser identity);
        bool UpdateAvatar(IdentityUser identity);
        bool Delete(string id);
        bool LockAccount(IdentityUser identity);
        bool UnLockAccount(IdentityUser identity);
        IdentityUser GetById(string id);
        IdentityUser GetByUserName(string userName);
        IdentityUser GetDetail(int id);
        IdentityUser Login(IdentityUser identity);

        List<IdentityPermission> GetPermissionsByUser(string userId);
        List<IdentityMenu> GetRootMenuByUserId(string userId);
        List<IdentityMenu> GetChildMenuByUserId(string userId, int parentId);
        List<IdentityMenu> GetAllDislayMenu();

        bool UpdateRoleofUser(string userId, string roleId);
    }

    public class StoreUser : IStoreUser
    {
        private readonly string _conStr;
        private RpsUser m;

        public StoreUser() : this("MainDBConn")
        {

        }

        public StoreUser(string connectionStringName)
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
            m = new RpsUser(_conStr);
        }

        #region  Common

        public List<IdentityUser> GetByPage(IdentityUser filter, int currentPage, int pageSize)
        {
            return m.GetByPage(filter, currentPage, pageSize);
        }

        public string Insert(IdentityUser identity)
        {
            return m.Insert(identity);
        }

        public bool Update(IdentityUser identity)
        {
            return m.Update(identity);
        }

        public bool ChangePassword(IdentityUser identity)
        {
            return m.ChangePassword(identity);
        }

        public bool UpdateAvatar(IdentityUser identity)
        {
            return m.UpdateAvatar(identity);
        }

        public bool LockAccount(IdentityUser identity)
        {
            return m.LockAccount(identity);
        }

        public bool UnLockAccount(IdentityUser identity)
        {
            return m.UnLockAccount(identity);
        }

        public IdentityUser GetDetail(int id)
        {
            return m.GetDetail(id);
        }

        public IdentityUser GetById(string id)
        {
            return m.GetById(id);
        }

        public IdentityUser GetByUserName(string userName)
        {
            return m.GetByUserName(userName);
        }

        public bool Delete(string id)
        {
            return m.Delete(id);
        }

        public IdentityUser Login(IdentityUser identity)
        {
            return m.Login(identity);
        }

        public List<IdentityPermission> GetPermissionsByUser(string userId)
        {
            return m.GetPermissionsByUser(userId);
        }

        public List<IdentityMenu> GetRootMenuByUserId(string userId)
        {
            return m.GetRootMenuByUserId(userId);
        }

        public List<IdentityMenu> GetChildMenuByUserId(string userId, int parentId)
        {
            return m.GetChildMenuByUserId(userId, parentId);
        }

        public List<IdentityMenu> GetAllDislayMenu()
        {
            return m.GetAllDislayMenu();
        }

        public bool UpdateRoleofUser(string userId, string roleId)
        {
            return m.UpdateRoleofUser(userId, roleId);
        }

        #endregion
    }
}
