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
        List<IdentityMessageFilter> GetByPage(int ConversationId, string Keyword, int CurrentPage, int PageSize);
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

        public List<IdentityMessageFilter> GetByPage(int ConversationId, string Keyword, int CurrentPage, int PageSize)
        {
            return r.GetByPage(ConversationId, Keyword, CurrentPage, PageSize);
        }
    }
}
