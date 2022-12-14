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
    public class RpsMessage
    {
        private readonly string _conStr;

        public RpsMessage(string connectionString)
        {
            _conStr = connectionString;
        }

        public RpsMessage()
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
        }

        public int Insert(IdentityMessage identity)
        {
            var sqlCmd = @"Message_Insert";
            int newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@ConversationId", identity.ConversationId },
                {"@Message", identity.Message },
                {"@Type", identity.Type },
                {"@ReplyMessageId", identity.ReplyMessageId },

                {"@SenderId", identity.SenderId },
                {"@ReceiverId", identity.ReceiverId },

            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, parameters);

                    newId = Convert.ToInt32(returnObj);

                    if(newId > 0)
                    {
                        if (identity.Attachments.HasData())
                        {
                            foreach (var att in identity.Attachments)
                            {
                                if (string.IsNullOrEmpty(att.Path))
                                    continue;

                                //For attachment parameters
                                var attParms = new Dictionary<string, object>
                                {
                                    {"@Name", att.Name },
                                    {"@ConversationId", identity.ConversationId },
                                    {"@MessageId", newId },
                                    {"@Path", att.Path}
                                };

                                var attachId = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, @"Message_InsertAttachment", attParms);
                                att.Id = Convert.ToInt32(attachId);
                            }
                        }

                        var conSenderParam = new Dictionary<string, object>
                        {
                            {"@ConversationId", identity.ConversationId },
                            {"@SenderId", identity.SenderId }
                        };
                        MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, @"Conversation_User_UpdateIsRead", conSenderParam);

                        
                    }


                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return newId;
        }

        public int ChangeImportant(int Id, int important)
        {
            var sqlCmd = @"Message_ChangeImportant";
            int newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", Id },
                {"@Important", important }

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

        public List<IdentityMessage> GetByPage(IdentityMessageFilter filter)
        {
            var sqlCmd = @"Message_GetByPage";
            List<IdentityMessage> listData = new List<IdentityMessage>();

            int offset = (filter.CurrentPage - 1) * filter.PageSize;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@ConversationId", filter.ConversationId},
                {"@Offset", offset},
                {"@Id", filter.Id},
                {"@Direction", filter.Direction},
                {"@IsMore", filter.IsMore},
                {"@PageSize", filter.PageSize}

            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            IdentityMessage info = new IdentityMessage();

                            info.Id = Convert.ToInt32(reader["Id"]);
                            info.PageIndex = Convert.ToInt32(reader["PageIndex"]);
                            info.TotalCount = Convert.ToInt32(reader["TotalCount"]);

                            listData.Add(info);
                        }
                        /*if(filter.Id > 0 && reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                IdentityMessage info = new IdentityMessage();

                                info.Id = Convert.ToInt32(reader["Id"]);
                                info.TotalCount = Convert.ToInt32(reader["TotalCount"]);

                                listData.Add(info);
                            }
                        }*/
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

        public List<IdentityMessage> GetBySearch(IdentityMessageFilter filter)
        {
            var sqlCmd = @"Message_GetBySearch";
            List<IdentityMessage> listData = new List<IdentityMessage>();

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@ConversationId", filter.ConversationId},
                {"@Keyword", filter.Keyword},
                {"@PageSize", filter.PageSize},

            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        listData = ParsingListMessageFromReader(reader);
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

        public IdentityMessage GetById(int id)
        {
            var info = new IdentityMessage();

            if (id <= 0)
            {
                return info;
            }

            var sqlCmd = @"Message_GetById";

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
                        if (reader.Read())
                        {
                            info = ExtractMessage(reader);
                        }

                        if(info != null && reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                var record = ExtractMessageAttachment(reader);

                                if(info.Attachments == null)
                                {
                                    info.Attachments = new List<IdentityMessageAttachment>();
                                }

                                info.Attachments.Add(record);
                            }
                        }
                        if(info != null && reader.NextResult())
                        {
                            if (reader.Read())
                            {
                                var replyMessage = new IdentityMessageReply();
                                replyMessage.Id = Utils.ConvertToInt32(reader["Id"]);
                                replyMessage.Message = reader["Message"].ToString();
                                replyMessage.CreatedDate = DateTime.Parse(reader["CreatedDate"].ToString());

                                info.ReplyMessage = replyMessage;
                            }
                        }
                        if (info != null && reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                var attachment = ExtractMessageAttachment(reader);
                                if (info.ReplyMessage.Attachments == null)
                                {
                                    info.ReplyMessage.Attachments  = new List<IdentityMessageAttachment>();
                                }
                                info.ReplyMessage.Attachments.Add(attachment);

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

        public List<IdentityMessage> GetImportant(IdentityMessageFilter filter)
        {
            var sqlCmd = @"Message_GetImportant";
            List<IdentityMessage> listData = null;

            int offset = (filter.CurrentPage - 1) * filter.PageSize;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@ConversationId", filter.ConversationId},
                {"@Keyword", filter.Keyword},
                {"@Offset", offset},
                {"@PageSize", filter.PageSize},

            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        listData = ParsingListMessageFromReader(reader);
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

        public IdentityMessage GetLastMessage(int id)
        {
            var info = new IdentityMessage();

            if (id <= 0)
            {
                return null;
            }

            var sqlCmd = @"Message_GetLastMessage";

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
                            info = ExtractMessage(reader);
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

        public IdentityMessageReply GetReplyMessageById(int id)
        {
            var info = new IdentityMessageReply();

            if (id <= 0)
            {
                return null;
            }

            var sqlCmd = @"Message_GetReplyMessageById";

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
                        if (reader.Read())
                        {
                            info.Id = Utils.ConvertToInt32(reader["Id"]);
                            info.Message = reader["Message"].ToString();
                            info.CreatedDate = DateTime.Parse(reader["CreatedDate"].ToString());
                        }

                        if (info != null && reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                var attachment = ExtractMessageAttachment(reader);
                                if (info.Attachments == null)
                                {
                                    info.Attachments = new List<IdentityMessageAttachment>();
                                }
                                info.Attachments.Add(attachment);

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

        public int DeleteByConId(int conversationId)
        {
            var sqlCmd = @"Message_DeleteByConId";
            int newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@ConversationId", conversationId },
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

        public int DeleteMessage(IdentityMessage identity)
        {
            var sqlCmd = @"Message_DeleteMessage";
            int newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id },
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

        private List<IdentityMessage> ParsingListMessageFromReader(IDataReader reader)
        {
            List<IdentityMessage> listData = new List<IdentityMessage>();
            while (reader.Read())
            {
                //Get common information
                var record = ExtractMessage(reader);

                listData.Add(record);
            }

            return listData;
        }

        public static IdentityMessage ExtractMessage(IDataReader reader)
        {
            var record = new IdentityMessage();

            //Seperate properties
            

            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.ConversationId = Utils.ConvertToInt32(reader["ConversationId"]);
            record.Message = reader["Message"].ToString();
            record.Type = Utils.ConvertToInt32(reader["Type"]);

            record.SenderId = Utils.ConvertToInt32(reader["SenderId"]);
            record.ReceiverId = Utils.ConvertToInt32(reader["ReceiverId"]);

            record.CreatedDate = DateTime.Parse(reader["CreatedDate"].ToString());
            record.Important = Utils.ConvertToInt32(reader["Important"]);

            if (reader.HasColumn("TaskId"))
            {
                record.TaskId = Utils.ConvertToInt32(reader["TaskId"]);
            }
            if (reader.HasColumn("ReplyMessageId"))
            {
                record.PageIndex = Utils.ConvertToInt32(reader["ReplyMessageId"]);
            }

            if (reader.HasColumn("PageIndex"))
            {
                record.PageIndex = Utils.ConvertToInt32(reader["PageIndex"]);
            }

            return record;
        }

        public static IdentityMessageAttachment ExtractMessageAttachment(IDataReader reader)
        {
            var record = new IdentityMessageAttachment();

            //Seperate properties


            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.Name = reader["Name"].ToString();
            record.ConversationId = Utils.ConvertToInt32(reader["ConversationId"]);
            record.MessageId = Utils.ConvertToInt32(reader["MessageId"]);
            record.Path = reader["Path"].ToString();



            return record;
        }
    }
}
