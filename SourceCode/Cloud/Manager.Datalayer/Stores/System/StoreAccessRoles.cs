using System.Collections.Generic;
using System.Linq;
using Manager.DataLayer.Entities;
using Manager.DataLayer.Repositories;
using Manager.SharedLibs;
using Manager.SharedLibs.Extensions;

namespace Manager.DataLayer.Stores
{
    public class StoreAccessRoles : IStoreAccessRoles
    {
        private readonly string _conStr;
        private readonly RpsAccessRoles m;

        public StoreAccessRoles() : this("MainDBConn")
        {

        }

        public StoreAccessRoles(string connName)
        {
            _conStr = AppConfiguration.GetAppsetting(connName);
            m = new RpsAccessRoles(_conStr);
        }

        public List<IdentityAccess> GetAllAccess()
        {
            var list = m.GetAllAccess();
            return list.ToList();
        }

        public List<IdentityOperation> GetAllOperations()
        {
            var list = m.GetAllOperations();
            return list.ToList();
        }

        public List<IdentityAccessRoles> GetAccessRolesByAccessId(string AccessId)
        {
            var list = m.GetAccessRolesByAccessId(AccessId);
            return list.ToList();
        }

        public List<IdentityAccessRoles> GetPermissionByRoleId(string RoleId)
        {
            var list = m.GetPermissionByRoleId(RoleId);
            return list.ToList();
        }

        public List<IdentityAccessRoles> GetAccessByRoleId(string RoleId)
        {
            var list = m.GetAccessByRoleId(RoleId);
            return list.ToList();
        }

        public List<IdentityOperation> GetListOperationNotUse()
        {
            var list = m.GetListOperationNotUse();
            return list.ToList();
        }
        public List<IdentityOperation> GetOperationsByAccessId(string AccessId)
        {
            var list = m.GetOperationsByAccessId(AccessId);
            return list.ToList();
        }
        public void DeleteAllAccessOfRole(string RoleId)
        {
            m.DeleteAllAccessOfRole(RoleId);
        }

        public bool UpdateAccessOfRole(string[] operations, string RoleId)
        {
            return m.UpdateAccessOfRole(operations, RoleId);
        }

        public bool CheckPermission(string UserId, string AccessName, string ActionName)
        {
            return m.CheckPermission(UserId, AccessName, ActionName);
        }

        public List<IdentityPermission> GetPermissionsByUser(string UserId)
        {
            return m.GetPermissionsByUser(UserId);
        }

        public List<IdentityMenu> GetRootMenuByUserId(string UserId)
        {
            var list = m.GetRootMenuByUserId(UserId);
            return list.ToList();
        }

        public List<IdentityMenu> GetChildMenuByUserId(string UserId, int ParentId)
        {
            var list = m.GetChildMenuByUserId(UserId, ParentId);
            return list.ToList();
        }

        public List<IdentityMenu> GetAllMenus()
        {
            var list = m.GetAllMenus();
            return list.ToList();
        }

        public List<IdentityMenu> GetAllDislayMenu()
        {
            return m.GetAllDislayMenu();
        }

        #region Menu
        public string InsertMenu(IdentityMenu identity)
        {
            return m.InsertMenu(identity);
        }

        public string UpdateMenu(IdentityMenu identity)
        {
            return m.UpdateMenu(identity);
        }

        public IdentityMenu GetMenuById(int id)
        {
            return m.GetMenuById(id);
        }

        public void DeleteMenu(int id)
        {
            m.DeleteMenu(id);
        }

        public IdentityMenu GetMenuDetail(int id)
        {
            return m.GetMenuDetail(id);
        }

        public bool UpdateSorting(List<SortingElement> elements)
        {
            return m.UpdateSorting(elements);
        }

        public int AddNewLang(IdentityMenuLang identity)
        {
            return m.AddNewLang(identity);
        }

        public int UpdateLang(IdentityMenuLang identity)
        {
            return m.UpdateLang(identity);
        }

        public bool DeleteLang(int Id)
        {
            return m.DeleteLang(Id);
        }

        public IdentityMenuLang GetLangDetail(int id)
        {
            return m.GetLangDetail(id);
        }

        #endregion

        #region Access

        public bool DeleteAccess(string AccessId)
        {
            return m.DeleteAccess(AccessId);
        }

        public bool CheckAccessDuplicate(IdentityAccess identity)
        {
            return m.CheckAccessDuplicate(identity);
        }

        public bool CreateAccess(IdentityAccess identity)
        {
            return m.CreateAccess(identity);
        }

        public bool UpdateAccess(IdentityAccess identity)
        {
            return m.UpdateAccess(identity);
        }

        #endregion

        #region AccessLang
        public IdentityAccessLang GetAccessLangDetail(int id)
        {
            return m.GetAccessLangDetail(id);
        }
        public int AddAccessLang(IdentityAccessLang identity)
        {
            return m.AddAccessLang(identity);
        }

        public int UpdateAccessLang(IdentityAccessLang identity)
        {
            return m.UpdateAccessLang(identity);
        }

        public bool DeleteAccessLang(int id)
        {
            return m.DeleteAccessLang(id);
        }

        public IdentityAccess GetAccessDetail(string id)
        {
            return m.GetAccessDetail(id);
        }
        #endregion

        #region  Operations
        public bool DeleteOperation(int Id)
        {
            return m.DeleteOperation(Id);
        }

        public bool CheckOperationDuplicate(IdentityOperation identity)
        {
            return m.CheckOperationDuplicate(identity);
        }

        public bool CreateOperation(IdentityOperation identity)
        {
            return m.CreateOperation(identity);
        }

        public bool UpdateOperation(IdentityOperation identity)
        {
            return m.UpdateOperation(identity);
        }

        public IdentityOperation GetOperationDetail(int id)
        {
            return m.GetOperationDetail(id);
        }
        #endregion

        #region OperationsLang
        public IdentityOperationLang GetOperationLangDetail(int id)
        {
            return m.GetOperationLangDetail(id);
        }
        public int AddOperationLang(IdentityOperationLang identity)
        {
            return m.AddOperationLang(identity);
        }

        public int UpdateOperationLang(IdentityOperationLang identity)
        {
            return m.UpdateOperationLang(identity);
        }

        public bool DeleteOperationLang(int id)
        {
            return m.DeleteOperationLang(id);
        }

        #endregion
    }

    public interface IStoreAccessRoles
    {
        List<IdentityAccess> GetAllAccess();

        List<IdentityOperation> GetAllOperations();

        List<IdentityAccessRoles> GetAccessRolesByAccessId(string AccessId);

        List<IdentityAccessRoles> GetPermissionByRoleId(string RoleId);

        List<IdentityAccessRoles> GetAccessByRoleId(string RoleId);

        List<IdentityOperation> GetOperationsByAccessId(string AccessId);
        List<IdentityOperation> GetListOperationNotUse();

        void DeleteAllAccessOfRole(string RoleId);

        bool UpdateAccessOfRole(string[] operations, string RoleId);

        bool CheckPermission(string userId, string AccessName, string ActionName);

        List<IdentityPermission> GetPermissionsByUser(string UserId);

        List<IdentityMenu> GetRootMenuByUserId(string UserId);

        List<IdentityMenu> GetChildMenuByUserId(string UserId, int ParentId);

        List<IdentityMenu> GetAllMenus();

        List<IdentityMenu> GetAllDislayMenu();

        #region Menu
        string InsertMenu(IdentityMenu identity);
        string UpdateMenu(IdentityMenu identity);
        IdentityMenu GetMenuById(int Id);
        void DeleteMenu(int Id);
        bool UpdateSorting(List<SortingElement> elements);

        int AddNewLang(IdentityMenuLang identity);
        int UpdateLang(IdentityMenuLang identity);
        bool DeleteLang(int Id);
        IdentityMenu GetMenuDetail(int id);
        IdentityMenuLang GetLangDetail(int id);

        #endregion

        #region Access
        bool DeleteAccess(string AccessId);

        bool UpdateAccess(IdentityAccess identity);

        bool CheckAccessDuplicate(IdentityAccess identity);

        bool CreateAccess(IdentityAccess identity);
        IdentityAccess GetAccessDetail(string id);

        #endregion

        #region AccessLang
        IdentityAccessLang GetAccessLangDetail(int id);
        int AddAccessLang(IdentityAccessLang identity);
        int UpdateAccessLang(IdentityAccessLang identity);
        bool DeleteAccessLang(int id);

        #endregion

        #region Operation
        bool DeleteOperation(int Id);

        bool UpdateOperation(IdentityOperation identity);

        bool CheckOperationDuplicate(IdentityOperation identity);

        bool CreateOperation(IdentityOperation identity);

        IdentityOperation GetOperationDetail(int id);
        #endregion

        #region OperationsLang
        IdentityOperationLang GetOperationLangDetail(int id);
        
        int AddOperationLang(IdentityOperationLang identity);
       
        int UpdateOperationLang(IdentityOperationLang identity);
        bool DeleteOperationLang(int id);

        #endregion
    }
}
