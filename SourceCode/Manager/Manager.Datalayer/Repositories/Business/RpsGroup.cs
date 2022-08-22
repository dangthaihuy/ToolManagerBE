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


        public static IdentityGroup ExtractGroup(IDataReader reader)
        {
            var record = new IdentityGroup();

            //Seperate properties


            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.Name = reader["Name"].ToString();




            return record;
        }
    }
}
