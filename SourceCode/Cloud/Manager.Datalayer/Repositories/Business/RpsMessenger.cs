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
    public class RpsMessenger
    {
        private readonly string _conStr;

        public RpsMessenger(string connectionString)
        {
            _conStr = connectionString;
        }

        public RpsMessenger()
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

                {"@SenderId", identity.SenderId },
                {"@ReceiverId", identity.ReceiverId },
                {"@CreatedDate", identity.CreatedDate },

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

            record.SenderId = Utils.ConvertToInt32(reader["SenderId"]);
            record.ReceiverId = Utils.ConvertToInt32(reader["ReceiverId"]);

            record.CreatedDate = DateTime.Parse(reader["CreateDate"].ToString());
            record.Important = Utils.ConvertToInt32(reader["Important"]);


            return record;
        }
    }
}
