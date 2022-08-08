using Manager.DataLayer.Entities;
using Manager.Datalayer.Repositories;
using Manager.SharedLibs;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manager.DataLayer.Repositories;
using Manager.DataLayer.Repositories.Business;
using Manager.DataLayer.Entities.Business;

namespace Manager.DataLayer.Stores.Business
{
    public interface IStoreEmployee
    {
        int Insert(IdentityEmployee identity);
        List<IdentityEmployee> GetByPage(IdentityEmployee filter, int currentPage, int pageSize);
        IdentityEmployee GetById(int id);

        List<IdentityEmployee> GetList();
    }
    public class StoreEmployee : IStoreEmployee
    {
        private readonly string _conStr;
        private RpsEmployee r;
        public StoreEmployee() : this("MainDBConn")
        {
        }

        public StoreEmployee(string connectionStringName)
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
            r = new RpsEmployee(_conStr);
        }

        public IdentityEmployee GetById(int id)
        {
            return r.GetById(id);
        }

        public List<IdentityEmployee> GetByPage(IdentityEmployee filter, int currentPage, int pageSize)
        {
            return r.GetByPage(filter, currentPage, pageSize);
        }

        public List<IdentityEmployee> GetList()
        {
            return r.GetList();
        }

        public int Insert(IdentityEmployee identity)
        {
            return r.Insert(identity);
        }
    }
}
