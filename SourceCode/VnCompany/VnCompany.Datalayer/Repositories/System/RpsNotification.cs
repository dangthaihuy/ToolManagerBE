using VnCompany.DataLayer.Entities;
using VnCompany.SharedLibs;
using VnCompany.SharedLibs.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VnCompany.DataLayer.Repositories
{
    public class RpsNotification
    {
        private readonly string _connStr;

        public RpsNotification(string connectionString)
        {
            _connStr = connectionString;
        }

        public RpsNotification()
        {
            _connStr = AppConfiguration.GetAppsetting("MainDBConn");
        }

        #region Common

        public static IdentityNotification ExtractBaseNotificationData(IDataReader reader)
        {
            var record = new IdentityNotification();

            //Seperate properties
            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.ActionType = Utils.ConvertToInt32(reader["ActionType"]);
            record.ActionId = Utils.ConvertToInt32(reader["ActionId"]);
            record.SenderId = Utils.ConvertToInt32(reader["SenderId"]);
            record.TargetType = Utils.ConvertToInt32(reader["TargetType"]);
            record.TargetId = Utils.ConvertToInt32(reader["TargetId"]);
            record.Content = reader["Content"].ToString();
            record.CreatedDate = reader["CreatedDate"] == DBNull.Value ? null : (DateTime?)reader["CreatedDate"];

            return record;
        }

        public static IdentityNotification ExtractNotificationData(IDataReader reader)
        {
            var record = new IdentityNotification();

            //Seperate properties
            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.UserType = Utils.ConvertToInt32(reader["UserType"]);
            record.UserId = Utils.ConvertToInt32(reader["UserId"]);
            record.NotificationId = Utils.ConvertToInt32(reader["NotificationId"]);
            record.IsViewed = Utils.ConvertToBoolean(reader["IsViewed"]);
            record.IsRead = Utils.ConvertToBoolean(reader["IsRead"]);
            record.ReadDate = reader["ReadDate"] == DBNull.Value ? null : (DateTime?)reader["ReadDate"];

            return record;
        }

        public int SinglePush(IdentityNotification identity)
        {
            //Common syntax           
            var sqlCmd = @"Notification_SinglePush";
            var newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@ActionType", identity.ActionType},
                {"@TargetId", identity.TargetId },
                {"@SenderId", identity.SenderId },
                {"@TargetType", identity.TargetType },
                {"@Content", identity.Content },
                {"@UserType", identity.UserType },
                {"@UserId", identity.UserId }
            };

            try
            {
                using (var conn = new SqlConnection(_connStr))
                {
                    var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, parameters);

                    newId = Convert.ToInt32(returnObj);
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return newId;
        }

        public int MultiplePush(List<int> listIds, IdentityNotification identity)
        {
            //Common syntax           
            var sqlCmd = @"Notification_MultiplePush";
            var newId = 0;

            var listIdsStr = string.Join(",", listIds);

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@ActionType", identity.ActionType},
                {"@ActionId", identity.ActionId },
                {"@TargetId", identity.TargetId },
                {"@SenderId", identity.SenderId },
                {"@TargetType", identity.TargetType },
                {"@Content", identity.Content },                              
                {"@UserType", identity.UserType },                              
                {"@ListIds", listIdsStr }
            };

            try
            {
                using (var conn = new SqlConnection(_connStr))
                {
                    var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, parameters);

                    newId = Convert.ToInt32(returnObj);
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return newId;
        }

        public IdentityNotification GetById(int id)
        {
            IdentityNotification info = null;
            var sqlCmd = @"Notification_GetById";

            var parameters = new Dictionary<string, object>
            {
                {"@Id", id}
            };

            try
            {
                using (var conn = new SqlConnection(_connStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        //Detail
                        if (reader.Read())
                        {
                            info = ExtractNotificationData(reader);
                        }

                        //Base data
                        if (reader.NextResult())
                        {
                            if (reader.Read())
                            {
                                var baseNotif = ExtractBaseNotificationData(reader);
                                info.ActionType = baseNotif.ActionType;
                                info.SenderId = baseNotif.SenderId;
                                info.TargetType = baseNotif.TargetType;
                                info.TargetId = baseNotif.TargetId;
                                info.Content = baseNotif.Content;
                                info.CreatedDate = baseNotif.CreatedDate;
                                info.NotificationId = baseNotif.Id;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }
            return info;
        }

        public bool Delete(int id)
        {
            //Common syntax            
            var sqlCmd = @"Notification_Delete";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@id", id},
            };

            try
            {
                using (var conn = new SqlConnection(_connStr))
                {
                    MsSqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, sqlCmd, parameters);
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public List<IdentityNotification> GetByUser(dynamic filter, int currentPage, int pageSize)
        {
            //Common syntax            
            var sqlCmd = @"Notification_GetByUser";

            int offset = (currentPage - 1) * pageSize;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@UserType", filter.UserType},
                {"@UserId", filter.UserId},
                {"@Offset", offset},
                {"@PageSize", pageSize},
                {"@IsUpdateView", filter.IsUpdateView},
            };

            List<IdentityNotification> listData = new List<IdentityNotification>();
            List<IdentityNotificationBase> listBase = new List<IdentityNotificationBase>();
            try
            {
                using (var conn = new SqlConnection(_connStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        //Detail
                        while (reader.Read())
                        {
                            var record = ExtractNotificationData(reader);

                            if (reader.HasColumn("TotalCount"))
                                record.TotalCount = Utils.ConvertToInt32(reader["TotalCount"]);

                            listData.Add(record);
                        }

                        if (reader.NextResult())
                        {
                            //Base data
                            while (reader.Read())
                            {
                                var baseNotif = ExtractBaseNotificationData(reader);
                                listBase.Add(baseNotif);
                            }
                        }

                        if (listData.HasData() && listBase.HasData())
                        {
                            foreach (var item in listData)
                            {
                                var matchedMaster = listBase.Where(x => x.Id == item.NotificationId).FirstOrDefault();
                                if (matchedMaster != null)
                                {
                                    item.ActionType = matchedMaster.ActionType;
                                    item.SenderId = matchedMaster.SenderId;
                                    item.TargetType = matchedMaster.TargetType;
                                    item.TargetId = matchedMaster.TargetId;
                                    item.Content = matchedMaster.Content;
                                    item.CreatedDate = matchedMaster.CreatedDate;
                                    item.NotificationId = matchedMaster.Id;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return listData;
        }

        public int CountUnread(dynamic identity)
        {
            //Common syntax           
            var sqlCmd = @"Notification_CountUnread";
            var totalCount = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@UserType", identity.UserType },
                {"@UserId", identity.UserId },
            };

            try
            {
                using (var conn = new SqlConnection(_connStr))
                {
                    var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, parameters);

                    totalCount = Convert.ToInt32(returnObj);
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return totalCount;
        }

        public bool MarkIsRead(IdentityNotification identity)
        {
            //Common syntax           
            var sqlCmd = @"Notification_MarkIsRead";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id },
                {"@UserType", identity.UserType },                
                {"@UserId", identity.UserId },
                {"@IsRead", identity.IsRead },
            };

            try
            {
                using (var conn = new SqlConnection(_connStr))
                {
                    MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, parameters);
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        #endregion
    }
}
