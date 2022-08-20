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
    public interface IStoreGroup
    {
        IdentityGroup GetById(string id);
    }
    public class StoreGroup : IStoreGroup
    {
        private readonly string _conStr;
        private RpsGroup r;
        public StoreGroup() : this("MainDBConn")
        {
        }

        public StoreGroup(string connectionStringName)
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
            r = new RpsGroup(_conStr);
        }
        public IdentityGroup GetById(string id)
        {
            return r.GetById(id);
        }
    }
}
