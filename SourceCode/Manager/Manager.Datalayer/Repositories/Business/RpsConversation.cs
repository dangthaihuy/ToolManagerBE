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
        public IdentityConversation GetDetail(string senderId, string receiverId)
        {
            var SenderId = Utils.ConvertToInt32(senderId);
            var ReceiverId = Utils.ConvertToInt32(receiverId);

            var listData = new IdentityConversation();

            if (SenderId <= 0 || ReceiverId<=0)
            {
                return listData;
            }

            var sqlCmd = @"Conversations_GetDetail";

            var parameters = new Dictionary<string, object>
            {
                {"@SenderId", SenderId},
                {"@ReceiverId", ReceiverId}

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
