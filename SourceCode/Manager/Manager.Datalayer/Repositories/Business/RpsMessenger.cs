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
                {"@CreateDate", identity.CreateDate },

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

        public List<IdentityMessageFilter> GetByPage(int ConversationId, string Keyword, int CurrentPage, int PageSize)
        {
            var sqlCmd = @"Message_GetByPage";
            List<IdentityMessageFilter> listData = null;

            int offset = (CurrentPage - 1) * PageSize;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@ConversationId", ConversationId},
                {"@Keyword", Keyword},
                {"@Offset", offset},
                {"@PageSize", PageSize},

            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        listData = ParsingListUserFromReader(reader);
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

        private List<IdentityMessageFilter> ParsingListUserFromReader(IDataReader reader)
        {
            List<IdentityMessageFilter> listData = new List<IdentityMessageFilter>();
            while (reader.Read())
            {
                //Get common information
                var record = ExtractMessageFilter(reader);

                listData.Add(record);
            }

            return listData;
        }

        public static IdentityMessageFilter ExtractMessageFilter(IDataReader reader)
        {
            var record = new IdentityMessageFilter();

            //Seperate properties
            

            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.ConversationId = Utils.ConvertToInt32(reader["ConversationId"]);
            record.Message = reader["Message"].ToString();

            record.SenderId = Utils.ConvertToInt32(reader["SenderId"]);
            record.ReceiverId = Utils.ConvertToInt32(reader["ReceiverId"]);

            record.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());


            return record;
        }
    }
}
