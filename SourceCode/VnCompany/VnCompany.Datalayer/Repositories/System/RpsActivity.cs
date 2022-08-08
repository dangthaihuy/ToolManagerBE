using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using VnCompany.SharedLibs;
using VnCompany.SharedLibs.Extensions;

namespace VnCompany.DataLayer.Repositories
{
    public class RpsActivity
    {
        private readonly string _conStr;

        public RpsActivity(string connectionString)
        {
            _conStr = connectionString;
        }

        public RpsActivity()
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
        }

        #region Activity Log

        //Create new log
        public bool InsertActivityLog(IdentityActivity sv)
        {
            bool success = false;
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var dateUTCNow = DateTime.UtcNow;
                    var parameters = new Dictionary<string, object>
                    {
                        {"@UserId", sv.UserId},
                        {"@ActivityText", sv.ActivityText },
                        {"@ActivityType", sv.ActivityType },
                        {"@TargetType", sv.TargetType },
                        {"@TargetId", sv.TargetId },
                        {"@IPAddress", sv.IPAddress },
                        {"@ActivityDate", dateUTCNow}
                    };

                    string query = @"INSERT INTO aspnetactivitylog (UserId,ActivityText,ActivityType,TargetType,TargetId,IPAddress,ActivityDate) 
                                    VALUES(@UserId,@ActivityText,@ActivityType,@TargetType,@TargetId,@IPAddress,@ActivityDate)";

                    MsSqlHelper.ExecuteNonQuery(conn, query, parameters);
                    success = true;
                }
            }
            catch(Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", "InsertActivityLog", ex.Message);
                throw new CustomSQLException(strError);
            }
            return success;
        }

        //Get Activity Log by UserId
        public IQueryable<IdentityActivity> GetActivityLogByUserId(string UserId, int page, int pageSize)
        {
            List<IdentityActivity> list = new List<IdentityActivity>();

            using (var conn = new SqlConnection(_conStr))
            {
                int offset = (page - 1) * pageSize;
                var parameters = new Dictionary<string, object>
                {
                    {"@UserId", UserId},
                    {"@offset", offset},
                    {"@pageSize", pageSize},
                };
                var query = @"SELECT a.* FROM aspnetactivitylog as a                   
                    Where a.UserId = @UserId
                    ORDER BY a.ActivityDate DESC
                    OFFSET @offset ROWS		
                    FETCH NEXT @pageSize ROWS ONLY"
                    ;
                var reader = MsSqlHelper.ExecuteReader(conn, CommandType.Text,
                   query, parameters);
                while (reader.Read())
                {
                    var item = (IdentityActivity)Activator.CreateInstance(typeof(IdentityActivity));

                    item.ActivityLogId = Convert.ToInt32(reader[0]);
                    item.UserId = reader[1].ToString();
                    item.ActivityText = reader[2].ToString();
                    item.TargetType = reader[3].ToString();
                    item.TargetId = reader[4].ToString();
                    item.IPAddress = reader[5].ToString();
                    item.ActivityDate = (DateTime)reader[6];
                    item.ActivityType = reader[7].ToString();

                    list.Add(item);
                }
            }
            return list.AsQueryable<IdentityActivity>();
        }

        public int CountAllActivityLogByUserId(string UserId)
        {
            var total = 0;
            var parameters = new Dictionary<string, object>
                {
                    {"@UserId", UserId}
                };
            using (var conn = new SqlConnection(_conStr))
            {
                var reader = MsSqlHelper.ExecuteReader(conn, CommandType.Text,
                    @"SELECT count(*) as CountNum FROM aspnetactivitylog                    
                    Where UserId = @UserId"
                    , parameters);
                if (reader.Read())
                {
                    total = Convert.ToInt32(reader["CountNum"]);
                }
            }
            return total;
        }

        public IQueryable<IdentityActivity> FilterActivityLog(ActivityLogQueryParms parms)
        {
            List<IdentityActivity> list = new List<IdentityActivity>();

            using (var conn = new SqlConnection(_conStr))
            {
                int offset = (parms.CurrentPage - 1) * parms.PageSize;
                var parameters = new Dictionary<string, object>
                {
                    {"@Email", parms.Email},
                    {"@ActivityText", parms.ActivityText},
                    {"@ActivityType", parms.ActivityType},
                    {"@FromDate", parms.FromDate},
                    {"@ToDate", parms.ToDate},
                    {"@pageSize", parms.PageSize},
                    {"@offset", offset}
                };

                var reader = MsSqlHelper.ExecuteReader(conn, CommandType.Text,
                    @"SELECT a.*,b.Email FROM aspnetactivitylog as a    
                    LEFT JOIN aspnetusers as b on a.UserId = b.Id               
                    WHERE 1 = 1
                    AND b.Email LIKE CONCAT('%', @Email , '%') AND a.ActivityText LIKE CONCAT('%', @ActivityText , '%') 
                    AND STR_TO_DATE(a.ActivityDate,'%Y-%m-%d') BETWEEN STR_TO_DATE(@FromDate,'%Y-%m-%d') AND STR_TO_DATE(@ToDate,'%Y-%m-%d')
                    AND a.ActivityType LIKE CONCAT('%', @ActivityType , '%') 
                    ORDER BY a.ActivityDate DESC, b.Email ASC
                    LIMIT @offset,@pageSize", parameters);

                while (reader.Read())
                {

                    var item = (IdentityActivity)Activator.CreateInstance(typeof(IdentityActivity));

                    item.ActivityLogId = (int)reader[0];
                    item.UserId = reader[1].ToString();
                    item.ActivityText = reader[2].ToString();
                    item.TargetType = reader[3].ToString();
                    item.TargetId = reader[4].ToString();
                    item.IPAddress = reader[5].ToString();
                    item.ActivityDate = (DateTime)reader[6];
                    item.ActivityType = reader[7].ToString();
                    item.UserName = reader[8].ToString();

                    list.Add(item);
                }
            }
            return list.AsQueryable<IdentityActivity>();
        }

        public int CountAllFilterActivityLog(ActivityLogQueryParms parms)
        {
            var total = 0;
            var parameters = new Dictionary<string, object>
                {
                    {"@Email", parms.Email},
                    {"@ActivityText", parms.ActivityText},
                    {"@ActivityType", parms.ActivityType},
                    {"@FromDate", parms.FromDate},
                    {"@ToDate", parms.ToDate}
                };

            using (var conn = new SqlConnection(_conStr))
            {
                var reader = MsSqlHelper.ExecuteReader(conn, CommandType.Text,
                    @"SELECT count(*) as CountNum FROM aspnetactivitylog as a    
                LEFT JOIN aspnetusers as b on a.UserId = b.Id               
                WHERE 1 = 1
                AND b.Email LIKE CONCAT('%', @Email , '%') AND a.ActivityText LIKE CONCAT('%', @ActivityText , '%') 
                AND STR_TO_DATE(a.ActivityDate,'%Y-%m-%d') BETWEEN STR_TO_DATE(@FromDate,'%Y-%m-%d') AND STR_TO_DATE(@ToDate,'%Y-%m-%d')
                AND a.ActivityType LIKE CONCAT('%', @ActivityType , '%') 
                ", parameters);
                if (reader.Read())
                {
                    total = Utils.ConvertToInt32(reader["CountNum"], 0);
                }
            }
            return total;
        }

        public IdentityActivity GetActivityLogById(string Id)
        {
            var info = (IdentityActivity)Activator.CreateInstance(typeof(IdentityActivity));
            using (var conn = new SqlConnection(_conStr))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@Id", Id}
                };

                var reader = MsSqlHelper.ExecuteReader(conn, CommandType.Text,
                    @"SELECT a.*,b.Email FROM aspnetactivitylog as a    
                    LEFT JOIN aspnetusers as b on a.UserId = b.Id               
                    WHERE 1 = 1 AND ActivityLogId = @Id
                    ", parameters);
                while (reader.Read())
                {
                    info.ActivityLogId = (int)reader[0];
                    info.UserId = reader[1].ToString();
                    info.ActivityText = reader[2].ToString();
                    info.TargetType = reader[3].ToString();
                    info.TargetId = reader[4].ToString();
                    info.IPAddress = reader[5].ToString();
                    info.ActivityDate = (DateTime)reader[6];
                    info.ActivityType = reader[7].ToString();
                    info.UserName = reader[8].ToString();
                }
            }

            return info;
        }
        #endregion
    }
}
