using Manager.DataLayer.Entities;
using Manager.DataLayer.Entities.Business;
using Manager.SharedLibs;
using Manager.SharedLibs.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.DataLayer.Repositories.Business
{
    public class RpsGroup
    {
        private readonly string _conStr;

        public RpsGroup(string connectionString)
        {
            _conStr = connectionString;
        }

        public RpsGroup()
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
        }
        public IdentityGroup GetById(string id)
        {
            int Id = Utils.ConvertToInt32(id);

            var info = new IdentityGroup();

            if (Id <= 0)
            {
                return info;
            }

            var sqlCmd = @"Conversations_GetGroupById";

            var parameters = new Dictionary<string, object>
            {
                {"@Id", id}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            info = ExtractGroup(reader);
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

        public List<IdentityCurrentUser> GetUserById(int id)
        {
            var listData = new List<IdentityCurrentUser>();
            
            if (id <= 0)
            {
                return listData;
            }

            var sqlCmd = @"Group_User_GetUserById";

            var parameters = new Dictionary<string, object>
            {
                {"@Id", id}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            var info = new IdentityCurrentUser();
                            info = ExtractCurrentUser(reader);

                            listData.Add(info);
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

        public List<int> GetGroupIdByUserId(string id)
        {
            var listData = new List<int>();

            var userId = Utils.ConvertToInt32(id);

            if (userId <= 0)
            {
                return listData;
            }

            var sqlCmd = @"Group_User_GetGroupByUserId";

            var parameters = new Dictionary<string, object>
            {
                {"@Id", userId}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            var info = Utils.ConvertToInt32(reader["Id"]);
                            listData.Add(info);
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




        public int Insert(int groupId, int memberId)
        {
            var sqlCmd = @"Group_User_Insert";
            int newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@GroupId", groupId },
                {"@UserId", memberId },
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
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

        public int Delete(int groupId, int memberId)
        {
            var sqlCmd = @"Group_User_Delete";
            int newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@GroupId", groupId },
                {"@UserId", memberId },
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
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
        public int DeleteByGrpId(int groupId)
        {
            var sqlCmd = @"Group_User_DeleteByGrpId";
            int newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@GroupId", groupId },
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
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

        private static IdentityGroup ExtractGroup(IDataReader reader)
        {
            var record = new IdentityGroup();

            //Seperate properties

            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.CreatedBy = Utils.ConvertToInt32(reader["CreatedBy"]);
            record.Name = reader["Name"].ToString();

            return record;
        }

        private static IdentityCurrentUser ExtractCurrentUser(IDataReader reader)
        {
            var record = new IdentityCurrentUser();

            //Seperate properties

            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.Email = reader["Email"].ToString();
            record.Avatar = reader["Avatar"].ToString();
            record.Fullname = reader["Fullname"].ToString();

            return record;
        }
    }
}
