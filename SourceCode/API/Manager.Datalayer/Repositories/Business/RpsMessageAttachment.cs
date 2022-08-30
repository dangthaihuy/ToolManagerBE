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
    public class RpsMessageAttachment
    {
        private readonly string _conStr;

        public RpsMessageAttachment(string connectionString)
        {
            _conStr = connectionString;
        }

        public RpsMessageAttachment()
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
        }

        public List<IdentityMessageAttachment> GetByMessageId(int messageId)
        {
            var sqlCmd = @"MessageAttachment_GetByMessageId";
            List<IdentityMessageAttachment> listData = null;

            

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@MessageId", messageId}

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

        public int DeleteByConId(int conversationId)
        {
            var sqlCmd = @"MessageAttachment_DeleteByConId";
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

        private List<IdentityMessageAttachment> ParsingListMessageFromReader(IDataReader reader)
        {
            List<IdentityMessageAttachment> listData = new List<IdentityMessageAttachment>();
            while (reader.Read())
            {
                //Get common information
                var record = ExtractMessageAttachment(reader);

                listData.Add(record);
            }

            return listData;
        }

        public static IdentityMessageAttachment ExtractMessageAttachment(IDataReader reader)
        {
            var record = new IdentityMessageAttachment();

            //Seperate properties


            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.ConversationId = Utils.ConvertToInt32(reader["ConversationId"]);
            record.MessageId = Utils.ConvertToInt32(reader["MessageId"]);
            record.Path = reader["Path"].ToString();



            return record;
        }

    }
}
