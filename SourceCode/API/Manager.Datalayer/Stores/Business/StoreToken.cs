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
    public interface IStoreToken
    {
        int Insert(IdentityRefreshToken identity);
        List<IdentityRefreshToken> GetList();
        void UpdateRefToken(IdentityRefreshToken token);
    }
    public class StoreToken : IStoreToken
    {
        private readonly string _conStr;
        private RpsToken r;
        public StoreToken() : this("MainDBConn")
        { }

        public StoreToken(string connectionStringName)
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
            r = new RpsToken(_conStr);
        }

        public int Insert(IdentityRefreshToken identity)
        {
            return r.Insert(identity);
        }

        public List<IdentityRefreshToken> GetList()
        {
            return r.GetList();
        }

        public void UpdateRefToken(IdentityRefreshToken token)
        {
            r.UpdateRefToken(token);
        }
    }
}
