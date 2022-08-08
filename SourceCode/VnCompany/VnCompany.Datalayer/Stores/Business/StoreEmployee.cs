using VnCompany.DataLayer.Entities;
using VnCompany.Datalayer.Repositories;
using VnCompany.SharedLibs;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnCompany.DataLayer.Repositories;
using VnCompany.DataLayer.Repositories.Business;
using VnCompany.DataLayer.Entities.Business;

namespace VnCompany.DataLayer.Stores.Business
{
    public interface IStoreEmployee
    {
        int Insert(IdentityEmployee identity);
        List<IdentityEmployee> GetByPage(IdentityEmployee filter, int currentPage, int pageSize);
        IdentityEmployee GetById(int id);
        bool Update(IdentityEmployee identity);
        List<IdentityEmployee> GetList();

        int InsertAll(IdentityEmployee employee, IdentityEmployeeContact contact);
        bool UpdateAll(IdentityEmployee employee, IdentityEmployeeContact contact);
        bool Delete(int employeeId);
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

        public bool Delete(int employeeId)
        {
            return r.Delete(employeeId);
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

        public int InsertAll(IdentityEmployee employee, IdentityEmployeeContact contact)
        {
            return r.InsertAll(employee, contact);
        }

        public bool Update(IdentityEmployee identity)
        {
            return r.Update(identity);
        }

        public bool UpdateAll(IdentityEmployee employee, IdentityEmployeeContact contact)
        {
            return r.UpdateAll(employee, contact);
        }
    }
}
