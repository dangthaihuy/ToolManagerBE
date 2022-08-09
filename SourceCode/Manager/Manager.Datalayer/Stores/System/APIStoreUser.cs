﻿

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
        int Register(IdentityUser identity);
        IdentityUser Login(IdentityUser identity);

        List<IdentityUser> GetList();

        IdentityUser GetById(int id);

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

        public int Register(IdentityUser identity)
        {
            return m.Register(identity);
        }

        public IdentityUser Login(IdentityUser identity)
        {
            return m.Login(identity);
        }

        public List<IdentityUser> GetList()
        {
            return m.GetList();
        }

        public IdentityUser GetById(int id)
        {
            return m.GetById(id);
        }
    }
}
