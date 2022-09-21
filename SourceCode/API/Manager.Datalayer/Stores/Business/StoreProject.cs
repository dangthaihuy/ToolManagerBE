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
    public interface IStoreProject
    {
        int InsertProject(IdentityProject identity);
        int DeleteProject(IdentityProject identity);
        IdentityProject UpdateProject(IdentityProject identity);

        int InsertTask(IdentityTask identity);
        int DeleteTask(IdentityTask identity);


    }

    public class StoreProject : IStoreProject
    {
        private readonly string _conStr;
        private RpsProject r;
        public StoreProject() : this("MainDBConn")
        {
        }

        public StoreProject(string connectionStringName)
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
            r = new RpsProject(_conStr);
        }
        public int InsertProject(IdentityProject identity)
        {
            return r.InsertProject(identity);
        }
        public int DeleteProject(IdentityProject identity)
        {
            return r.DeleteProject(identity);
        }
        public IdentityProject UpdateProject(IdentityProject identity)
        {
            return r.UpdateProject(identity);
        }
        public int InsertTask(IdentityTask identity)
        {
            return r.InsertTask(identity);
        }
        public int DeleteTask(IdentityTask identity)
        {
            return r.DeleteTask(identity);
        }


    }
}
