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
    public interface IStoreForm
    {
        List<IdentityForm> GetByPage(IdentityForm filter, int currentPage, int pageSize);
        int Insert(IdentityForm identity);
        int InsertFormField(IdentityFormField identity);
        bool Update(IdentityForm identity);
        IdentityForm GetById(int id);
        bool Delete(int id);
        List<IdentityForm> GetList();
        List<IdentityFormField> GetFormFieldsByFormId(int formId);
        bool UpdateFormField(IdentityFormField identity);
        bool DeleteFormField(int id);
    }

    public class StoreForm : IStoreForm
    {
        private readonly string _conStr;
        private RpsForm r;
        public StoreForm() : this("MainDBConn")
        {
        }

        public StoreForm(string connectionStringName)
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
            r = new RpsForm(_conStr);
        }
        public bool Delete(int id)
        {
            return r.Delete(id);
        }

        public bool DeleteFormField(int id)
        {
            return r.DeleteFormField(id);
        }

        public IdentityForm GetById(int id)
        {
            return r.GetById(id);
        }

        public List<IdentityForm> GetByPage(IdentityForm filter, int currentPage, int pageSize)
        {
            return r.GetByPage(filter, currentPage, pageSize);
        }

        public List<IdentityFormField> GetFormFieldsByFormId(int formId)
        {
            return r.GetFormFieldsByFormId(formId);
        }

        public List<IdentityForm> GetList()
        {
            return r.GetList();
        }

        public int Insert(IdentityForm identity)
        {
            return r.Insert(identity);
        }

        public int InsertFormField(IdentityFormField identity)
        {
            return r.InsertFormField(identity);
        }

        public bool Update(IdentityForm identity)
        {
            return r.Update(identity);
        }

        public bool UpdateFormField(IdentityFormField identity)
        {
            return r.UpdateFormField(identity);
        }
    }
}
