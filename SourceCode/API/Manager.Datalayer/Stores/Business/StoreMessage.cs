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
        IdentityMessage GetById(int id);
        List<IdentityMessage> GetBySearch(IdentityMessageFilter filter);
        List<IdentityMessage> GetImportant(IdentityMessageFilter filter);
        IdentityMessage GetLastMessage(int id);
        
        int DeleteMessage(IdentityMessage identity);

        int ChangeImportant(int Id, int important);

    }
    public class StoreMessage : IStoreMessage
    {
        private readonly string _conStr;
        private RpsMessage r;
        public StoreMessage() : this("MainDBConn")
        {
        }

        public StoreMessage(string connectionStringName)
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
            r = new RpsMessage(_conStr);
        }

        public int Insert(IdentityMessage identity)
        {
            return r.Insert(identity);
        }

        public List<IdentityMessage> GetByPage(IdentityMessageFilter filter)
        {
            return r.GetByPage(filter);
        }


        public IdentityMessage GetById(int id)
        {
            return r.GetById(id);
        }

        public List<IdentityMessage> GetBySearch(IdentityMessageFilter filter)
        {
            return r.GetBySearch(filter);
        }

        public List<IdentityMessage> GetImportant(IdentityMessageFilter filter)
        {
            return r.GetImportant(filter);
        }

        public int ChangeImportant(int Id, int important)
        {
            return r.ChangeImportant(Id, important);
        }


        public IdentityMessage GetLastMessage(int id)
        {
            return r.GetLastMessage(id);
        }

        
        public int DeleteMessage(IdentityMessage identity)
        {
            return r.DeleteMessage(identity);
        }

    }
}
