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
    public interface IStoreFileManagement
    {
        int InsertFolder(IdentityFolder identity);
        
        int DeleteFolder(IdentityFolder identity);
        List<IdentityFolder> GetChild(IdentityFolder identity);

        IdentityFolder UpdateFolder(IdentityFolder identity);

        int InsertFile(IdentityFile identity);
        IdentityFile GetFileById(IdentityFile identity);
        List<IdentityFile> GetFileByFolderId(IdentityFolder identity);
        int DeleteFile(IdentityFile identity);

    }

    public class StoreFileManagement : IStoreFileManagement
    {
        private readonly string _conStr;
        private RpsFileManagement r;
        public StoreFileManagement() : this("MainDBConn")
        {
        }

        public StoreFileManagement(string connectionStringName)
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
            r = new RpsFileManagement(_conStr);
        }

        public int InsertFolder(IdentityFolder identity)
        {
            return r.InsertFolder(identity);
        }

        public int DeleteFolder(IdentityFolder identity)
        {
            return r.DeleteFolder(identity);
        }

        public IdentityFolder UpdateFolder(IdentityFolder identity)
        {
            return r.UpdateFolder(identity);
        }

        public List<IdentityFolder> GetChild(IdentityFolder identity)
        {
            return r.GetChild(identity);
        }

        public int InsertFile(IdentityFile identity)
        {
            return r.InsertFile(identity);
        }

        public int DeleteFile(IdentityFile identity)
        {
            return r.DeleteFile(identity);
        }

        public IdentityFile GetFileById(IdentityFile identity)
        {
            return r.GetFileById(identity);
        }
        public List<IdentityFile> GetFileByFolderId(IdentityFolder identity)
        {
            return r.GetFileByFolderId(identity);
        }


    }
}
