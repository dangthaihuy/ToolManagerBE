using Manager.DataLayer.Repositories;
using Manager.SharedLibs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Manager.DataLayer.Stores
{
    public interface IStoreRole
    {
        List<IdentityRole> GetList();
        List<IdentityRole> GetListByAgencyId(int agencyId);
        List<IdentityRole> GetRolesByUserId(string userId);
        IdentityRole GetRoleByUserId(string userId);
        IdentityRole GetById(string roleId);
        int Insert(IdentityRole info);
        int Update(IdentityRole info);
        void Delete(string roleId);
        void AddUserToRole(string userId, string roleId);
        void DeleteUserInRole(string userId, string roleId);
    }

    public class StoreRole : IStoreRole
    {
        private readonly string _conStr;
        private RpsRole m;
        public StoreRole() : this("MainDBConn")
        {

        }

        public StoreRole(string connectionStringName)
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
            m = new RpsRole(_conStr);
        }

        public List<IdentityRole> GetList()
        {
            return m.GetList();
        }

        public List<IdentityRole> GetListByAgencyId(int agencyId)
        {
            return m.GetListByAgencyId(agencyId);
        }

        public List<IdentityRole> GetRolesByUserId(string userId)
        {
            return m.GetRolesByUserId(userId);
        }

        public IdentityRole GetById(string roleId)
        {
            return m.GetById(roleId);
        }

        public int Insert(IdentityRole info)
        {
            return m.Insert(info);
        }

        public int Update(IdentityRole info)
        {
            return m.Update(info);
        }

        public void Delete(string roleId)
        {
            m.Delete(roleId);
        }

        public void AddUserToRole(string userId, string roleId)
        {
            m.AddUserToRole(userId, roleId);
        }

        public void DeleteUserInRole(string userId, string roleId)
        {
            m.DeleteUserInRole(userId, roleId);
        }

        public IdentityRole GetRoleByUserId(string userId)
        {
            return m.GetRoleByUserId(userId);
        }
    }
}
