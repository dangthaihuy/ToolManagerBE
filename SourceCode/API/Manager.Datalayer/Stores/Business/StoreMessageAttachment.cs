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
    public interface IStoreMessageAttachment
    {
        List<IdentityMessageAttachment> GetByMessageId(int id);
        
        List<IdentityMessageAttachment> GetByConId(IdentityMessageFilter filter);
        int Insert(IdentityMessageAttachment identity);
    }
    public class StoreMessageAttachment : IStoreMessageAttachment
    {
        private readonly string _conStr;
        private RpsMessageAttachment r;
        public StoreMessageAttachment() : this("MainDBConn")
        { }

        public StoreMessageAttachment(string connectionStringName)
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
            r = new RpsMessageAttachment(_conStr);
        }

        public List<IdentityMessageAttachment> GetByMessageId(int id)
        {
            return r.GetByMessageId(id);
        }
        public List<IdentityMessageAttachment> GetByConId(IdentityMessageFilter filter)
        {
            return r.GetByConId(filter);
        }
        
        public int Insert(IdentityMessageAttachment identity)
        {
            return r.Insert(identity);
        }
    }
}
