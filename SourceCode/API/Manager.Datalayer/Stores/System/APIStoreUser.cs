

using Manager.DataLayer.Entities;
using Manager.DataLayer.Repositories.System;
using Manager.SharedLibs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.DataLayer.Stores.System
{
    public interface IAPIStoreUser
    {
        int Register(IdentityInformationUser identity);
        IdentityInformationUser Login(IdentityInformationUser identity);

        List<IdentityCurrentUser> GetList();

        IdentityInformationUser GetById(string id);

    }
    public class APIStoreUser : IAPIStoreUser
    {
        private readonly string _conStr;
        private APIRpsUser m;

        public APIStoreUser() : this("MainDBConn")
        {

        }

        public APIStoreUser(string connectionStringName)
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
            m = new APIRpsUser(_conStr);
        }

        public int Register(IdentityInformationUser identity)
        {
            return m.Register(identity);
        }

        public IdentityInformationUser Login(IdentityInformationUser identity)
        {
            return m.Login(identity);
        }

        public List<IdentityCurrentUser> GetList()
        {
            return m.GetList();
        }

        public IdentityInformationUser GetById(string id)
        {
            return m.GetById(id);
        }
    }
}
