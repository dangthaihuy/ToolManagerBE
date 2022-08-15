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
    public interface IStoreMessage
    {
        int Insert(IdentityMessage identity);
        List<IdentityMessage> GetByPage(IdentityMessageFilter filter);
    }
    public class StoreMessage : IStoreMessage
    {
        private readonly string _conStr;
        private RpsMessenger r;
        public StoreMessage() : this("MainDBConn")
        {
        }

        public StoreMessage(string connectionStringName)
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
            r = new RpsMessenger(_conStr);
        }

        public int Insert(IdentityMessage identity)
        {
            return r.Insert(identity);
        }

        public List<IdentityMessage> GetByPage(IdentityMessageFilter filter)
        {
            return r.GetByPage(filter);
        }
    }
}
