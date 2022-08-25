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
    public interface IStoreContact
    {
        int Insert(IdentityEmployeeContact identity);
        IdentityEmployeeContact GetByEmployeeId(int employeeId);
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

        public int Insert(IdentityEmployeeContact identity)
        {
            return r.Insert(identity);
        }
    }
}
