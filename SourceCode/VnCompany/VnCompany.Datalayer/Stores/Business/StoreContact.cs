using VnCompany.DataLayer.Entities.Business;
using VnCompany.DataLayer.Repositories.Business;
using VnCompany.SharedLibs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VnCompany.DataLayer.Stores.Business
{
    public interface IStoreContact
    {
        int Insert(IdentityEmployeeContact identity);
        IdentityEmployeeContact GetByEmployeeId(int employeeId);
        List<IdentityEmployeeContact> GetList();
        bool Update(IdentityEmployeeContact identity);
    }
    public class StoreContact : IStoreContact
    {
        private readonly string _conStr;
        private RpsContact r;
        public StoreContact() : this("MainDBConn")
        {
        }

        public StoreContact(string connectionStringName)
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
            r = new RpsContact(_conStr);
        }

        public IdentityEmployeeContact GetByEmployeeId(int employeeId)
        {
            return r.GetByEmployeeId(employeeId);
        }

        public List<IdentityEmployeeContact> GetList()
        {
            return r.GetList();
        }

        public int Insert(IdentityEmployeeContact identity)
        {
            return r.Insert(identity);
        }

        public bool Update(IdentityEmployeeContact identity)
        {
            return r.Update(identity);
        }
    }
}
