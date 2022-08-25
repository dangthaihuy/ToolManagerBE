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
    public interface IStoreMenuService
    {
        List<IdentityMenuService> GetByPage(IdentityMenuService filter, int currentPage, int pageSize);
        int Insert(IdentityMenuService identity);
        bool Update(IdentityMenuService identity);
        IdentityMenuService GetById(int id);
        bool Delete(int id);
        int InsertMenuServiceWorkflow(IdentityMenuWorkFlow identity);
        bool UpdateMenuServiceWorkflow(IdentityMenuWorkFlow identity);
        bool DeleteMenuServiceWorkflow(IdentityMenuWorkFlow identity);
        
    }

    public class StoreMenuService : IStoreMenuService
    {
        private readonly string _conStr;
        private RpsMenuService r;
        public StoreMenuService() : this("MainDBConn")
        {
        }

        public StoreMenuService(string connectionStringName)
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
            r = new RpsMenuService(_conStr);
        }

        public bool Delete(int id)
        {
            return r.Delete(id);
        }

        public bool DeleteMenuServiceWorkflow(IdentityMenuWorkFlow identity)
        {
            return r.DeleteMenuServiceWorkflow(identity);
        }

        public IdentityMenuService GetById(int id)
        {
            return r.GetById(id);
        }

        public List<IdentityMenuService> GetByPage(IdentityMenuService filter, int currentPage, int pageSize)
        {
            return r.GetByPage(filter, currentPage, pageSize);
        }

        public int Insert(IdentityMenuService identity)
        {
            return r.Insert(identity);
        }

        public int InsertMenuServiceWorkflow(IdentityMenuWorkFlow identity)
        {
            return r.InsertMenuServiceWorkflow(identity);
        }

        public bool Update(IdentityMenuService identity)
        {
            return r.Update(identity);
        }

        public bool UpdateMenuServiceWorkflow(IdentityMenuWorkFlow identity)
        {
            return r.UpdateMenuServiceWorkflow(identity);
        }
    }
}
