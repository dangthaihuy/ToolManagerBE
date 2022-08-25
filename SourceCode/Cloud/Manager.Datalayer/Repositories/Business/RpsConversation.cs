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
    public class RpsConversation
    {
        private readonly string _conStr;

        public RpsConversation(string connectionString)
        {
            _conStr = connectionString;
        }

        public RpsConversation()
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
        }

        public int Insert(IdentityConversationDefault identity)
        {
            var sqlCmd = @"Conversations_Insert";
            int newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@CreatorId", identity.CreatorId },
                {"@ReceiverId", identity.ReceiverId },
                {"@Type", identity.Type },
                {"@Name", identity.Name }


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

        public int InsertGroup(IdentityConversationDefault identity)
        {
            var sqlCmd = @"Conversations_InsertGroup";
            int newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@CreatorId", identity.CreatorId },
                {"@Type", identity.Type },
                {"@Name", identity.Name }


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

        public List<IdentityConversation> GetById(string id)
        {
            int Id = Utils.ConvertToInt32(id);
            var listData = new List<IdentityConversation>();

            if (Id <= 0)
            {
                return listData;
            }

            var sqlCmd = @"Conversations_GetById";

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
                            var info = new IdentityConversation();
                            info = ExtractConversation(reader);

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

        public List<IdentityConversation> GetGroupByUserId(string id)
        {
            int Id = Utils.ConvertToInt32(id);
            var listData = new List<IdentityConversation>();

            if (Id <= 0)
            {
                return listData;
            }

            var sqlCmd = @"Group_User_GetGroupByUserId";

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
                            var info = new IdentityConversation();
                            info = ExtractGroup(reader);

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

        public IdentityConversation GetDetail(int senderId, int receiverId)
        {
            var listData = new IdentityConversation();

            if (senderId <= 0 || receiverId <= 0)
            {
                return listData;
            }

            var sqlCmd = @"Conversations_GetDetail";

            var parameters = new Dictionary<string, object>
            {
                {"@CreatorId", senderId},
                {"@ReceiverId", receiverId}

            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            listData = ExtractConversationDetail(reader);
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


        private IdentityConversation ExtractConversation(IDataReader reader)
        {
            var record = new IdentityConversation();

            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.SenderId = Utils.ConvertToInt32(reader["SenderId"]);
            record.ReceiverId = Utils.ConvertToInt32(reader["ReceiverId"]);
            record.Type = Utils.ConvertToInt32(reader["Type"]);



            return record;
        }

        private IdentityConversation ExtractGroup(IDataReader reader)
        {
            var record = new IdentityConversation();

            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.SenderId = Utils.ConvertToInt32(reader["SenderId"]);


            return record;
        }

        private IdentityConversation ExtractConversationDetail(IDataReader reader)
        {
            var record = new IdentityConversation();

            record.Id = Utils.ConvertToInt32(reader["Id"]);



            return record;
        }
    }
}
