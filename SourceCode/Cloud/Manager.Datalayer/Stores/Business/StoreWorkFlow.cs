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
    public interface IStoreWorkFlow
    {
        List<IdentityWorkFlow> GetByPage(IdentityWorkFlow filter, int currentPage, int pageSize);
        int Insert(IdentityWorkFlow identity);
        bool Update(IdentityWorkFlow identity);
        IdentityWorkFlow GetById(int id);
        bool Delete(int id);
        int InsertWorkFlowForm(IdentityWorkFlowForm identity);
        int UpdateWorkFlowForm(IdentityWorkFlowForm identity);
        bool DeleteWorkFlowForm(int workFlowId, int formId);
    }
    public class StoreWorkFlow : IStoreWorkFlow
    {
        private readonly string _conStr;
        private RpsWorkFlow r;
        public StoreWorkFlow() : this("MainDBConn")
        {
        }

        public StoreWorkFlow(string connectionStringName)
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
            r = new RpsWorkFlow(_conStr);
        }
        public bool Delete(int id)
        {
            return r.Delete(id);
        }

        public bool DeleteWorkFlowForm(int workFlowId, int formId)
        {
            return r.DeleteWorkFlowForm(workFlowId, formId);
        }

        public IdentityWorkFlow GetById(int id)
        {
            return r.GetById(id);
        }

        public List<IdentityWorkFlow> GetByPage(IdentityWorkFlow filter, int currentPage, int pageSize)
        {
            return r.GetByPage(filter, currentPage, pageSize);
        }

        public int Insert(IdentityWorkFlow identity)
        {
            return r.Insert(identity);
        }

        public int InsertWorkFlowForm(IdentityWorkFlowForm identity)
        {
            return r.InsertWorkFlowForm(identity);
        }

        public bool Update(IdentityWorkFlow identity)
        {
            return r.Update(identity);
        }

        public int UpdateWorkFlowForm(IdentityWorkFlowForm identity)
        {
            return r.UpdateWorkFlowForm(identity);
        }
    }
}
