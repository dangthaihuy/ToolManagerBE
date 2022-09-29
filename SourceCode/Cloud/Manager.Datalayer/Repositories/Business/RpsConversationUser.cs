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
    public class RpsConversationUser
    {
        private readonly string _conStr;

        public RpsConversationUser(string connectionString)
        {
            _conStr = connectionString;
        }

        public RpsConversationUser()
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
        }
        public IdentityConversationUser GetById(string id)
        {
            int Id = Utils.ConvertToInt32(id);

            var info = new IdentityConversationUser();

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

        public List<IdentityCurrentUser> GetUserById(int Id)
        {
            var listData = new List<IdentityCurrentUser>();
            
            if (Id <= 0)
            {
                return listData;
            }

            var sqlCmd = @"Conversation_User_GetUserById";

            var parameters = new Dictionary<string, object>
            {
                {"@Id", Id}
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

            var Id = Utils.ConvertToInt32(id);

            if (Id <= 0)
            {
                return listData;
            }

            var sqlCmd = @"Conversation_User_GetGroupByUserId";

            var parameters = new Dictionary<string, object>
            {
                {"@Id", Id}
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
            var sqlCmd = @"Conversation_User_Insert";
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
            var sqlCmd = @"Conversation_User_Delete";
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


        private static IdentityConversationUser ExtractGroup(IDataReader reader)
        {
            var record = new IdentityConversationUser();

            //Seperate properties

            record.Id = Utils.ConvertToInt32(reader["Id"]);
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
