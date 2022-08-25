using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using System.Data;
using System.Data.SqlClient;
using Manager.DataLayer.Repositories;
using Manager.DataLayer.Entities;
using Manager.SharedLibs;

namespace Manager.DataLayer.Stores
{
    public class StoreActivity : IStoreActivity
    {
        private readonly string _conStr;
        private RpsActivity m;
        public StoreActivity() : this("MainDBConn")
        {

        }

        public StoreActivity(string connectionStringName)
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
            m = new RpsActivity(_conStr);
        }

        #region Activity Log

        public bool WriteActivityLog(IdentityActivity log)
        {
            return m.InsertActivityLog(log);
        }

        public List<IdentityActivity> GetActivityLogByUserId(string UserId, int page, int pageSize)
        {
            return m.GetActivityLogByUserId(UserId, page, pageSize).ToList();
        }

        public int CountAllActivityLogByUserId(string UserId)
        {
            return m.CountAllActivityLogByUserId(UserId);
        }

        public List<IdentityActivity> FilterActivityLog(ActivityLogQueryParms filters)
        {
            return m.FilterActivityLog(filters).ToList();
        }

        public int CountAllFilterActivityLog(ActivityLogQueryParms filters)
        {
            return m.CountAllFilterActivityLog(filters);
        }

        public IdentityActivity GetActivityLogById(string Id)
        {
            return m.GetActivityLogById(Id);
        }

        #endregion
       
    }

    public interface IStoreActivity
    {
        bool WriteActivityLog(IdentityActivity log);
        List<IdentityActivity> GetActivityLogByUserId(string UserId, int page, int pageSize);
        int CountAllActivityLogByUserId(string UserId);
        List<IdentityActivity> FilterActivityLog(ActivityLogQueryParms filters);
        int CountAllFilterActivityLog(ActivityLogQueryParms filters);
        IdentityActivity GetActivityLogById(string Id);
    }
}
