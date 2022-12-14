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
                {"@CreatedBy", identity.CreatedBy },
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
                {"@CreatedBy", identity.CreatedBy },
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
            int conId = Utils.ConvertToInt32(id);
            var listData = new List<IdentityConversation>();

            if (conId <= 0)
            {
                return listData;
            }

            var sqlCmd = @"Conversations_GetById";

            var parameters = new Dictionary<string, object>
            {
                {"@Id", conId}
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
            int conId = Utils.ConvertToInt32(id);
            var listData = new List<IdentityConversation>();

            if (conId <= 0)
            {
                return listData;
            }

            var sqlCmd = @"Conversation_User_GetGroupByUserId";

            var parameters = new Dictionary<string, object>
            {
                {"@Id", conId}
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
                {"@CreatedBy", senderId},
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

        public int Delete(int id)
        {
            var sqlCmd = @"Conversations_Delete";
            int newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", id }
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

        public IdentityConversationDefault Update(IdentityConversationUpdate identity)
        {
            var info = new IdentityConversationDefault();

            //Common syntax
            var sqlCmd = @"Conversations_Update";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id},
                {"@Name", identity.Name},
                {"@Avatar", identity.Avatar },
                {"@AvatarPath", identity.AvatarPath },

            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            info = ExtractConversationDefault(reader);
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

        public int GetReceiverById(IdentityConversationUser identity)
        {
            var res = new int();

            //Common syntax
            var sqlCmd = @"Conversations_GetReceiverById";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.ConversationId},
                {"@SenderId", identity.UserId},

            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (reader.Read())
                        {
                            res = Utils.ConvertToInt32(reader["ReceiverId"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return res;
        }

        public IdentityMessageAttachment GetFile(IdentityGetFile identity)
        {
            var res = new IdentityMessageAttachment();

            //Common syntax
            var sqlCmd = @"Conversations_GetFile";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id},
                {"@ConversationId", identity.ConversationId},
                {"@Direction", identity.Direction},

            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (reader.Read())
                        {
                            res.Id = Utils.ConvertToInt32(reader["Id"]);
                            res.Path = reader["Path"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return res;
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

        private IdentityConversationDefault ExtractConversationDefault(IDataReader reader)
        {
            var record = new IdentityConversationDefault();

            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.CreatedBy = Utils.ConvertToInt32(reader["CreatedBy"]);
            record.ReceiverId = Utils.ConvertToInt32(reader["ReceiverId"]);
            record.Type = Utils.ConvertToInt32(reader["Type"]);
            record.Name = reader["Name"].ToString();
            record.Avatar = reader["AvatarPath"].ToString();

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
        public static IdentityMessageAttachment ExtractMessageAttachment(IDataReader reader)
        {
            var record = new IdentityMessageAttachment();

            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.Name = reader["Name"].ToString();
            record.ConversationId = Utils.ConvertToInt32(reader["ConversationId"]);
            record.MessageId = Utils.ConvertToInt32(reader["MessageId"]);
            record.Path = reader["Path"].ToString();

            return record;
        }
    }
}
