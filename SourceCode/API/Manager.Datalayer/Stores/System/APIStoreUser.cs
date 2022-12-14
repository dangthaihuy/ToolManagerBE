

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
    public interface IApiStoreUser
    {
        int Register(IdentityInformationUser identity);
        IdentityInformationUser Login(IdentityInformationUser identity);

        List<IdentityCurrentUser> GetList();

        IdentityInformationUser GetById(string id);
        IdentityInformationUser GetByPassword(IdentityInformationUser identity);
        IdentityInformationUser GetByEmail(string email);
        IdentityInformationUser GetByRefreshToken(string refreshToken);
        IdentityInformationUser GetInforUser(int id);

        IdentityInformationUser Update(IdentityInformationUser identity);

    }
    public class ApiStoreUser : IApiStoreUser
    {
        private readonly string _conStr;
        private APIRpsUser m;

        public ApiStoreUser() : this("MainDBConn")
        {

        }

        public ApiStoreUser(string connectionStringName)
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

        public IdentityInformationUser GetByPassword(IdentityInformationUser identity)
        {
            return m.GetByPassword(identity);
        }

        public IdentityInformationUser GetByEmail(string email)
        {
            return m.GetByEmail(email);
        }

        public IdentityInformationUser GetByRefreshToken(string refreshToken)
        {
            return m.GetByRefreshToken(refreshToken);

        }


        public IdentityInformationUser GetInforUser(int id)
        {
            return m.GetInforUser(id);
        }

        public IdentityInformationUser Update(IdentityInformationUser identity)
        {
            return m.Update(identity);
        }
    }
}
