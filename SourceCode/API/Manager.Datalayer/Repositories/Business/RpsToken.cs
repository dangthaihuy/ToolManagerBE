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
    public class RpsToken
    {
        private readonly string _conStr;

        public RpsToken(string connectionString)
        {
            _conStr = connectionString;
        }

        public RpsToken()
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
        }
        public int Insert(IdentityRefreshToken identity)
        {
            var sqlCmd = @"Token_Insert";
            int newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@JwtId", identity.JwtId },
                {"@IsUsed", identity.IsUsed },
                {"@IsRevorked", identity.IsRevorked },
                {"@UserId", identity.UserId },
                {"@AddedDate", identity.AddedDate },
                {"@ExpiryDate", identity.ExpiryDate },
                {"@Token", identity.Token },


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

        public List<IdentityRefreshToken> GetList()
        {
            var sqlCmd = @"Token_GetList";
            List<IdentityRefreshToken> listData = new List<IdentityRefreshToken>();

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, null))
                    {
                        while (reader.Read())
                        {
                            var info = new IdentityRefreshToken();
                            info = ExtractToken(reader);

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

        public bool UpdateRefToken(IdentityRefreshToken token)
        {
            //Common syntax
            var sqlCmd = @"Token_Update";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", token.Id},
                {"@UserId", token.UserId },
                {"@Token", token.Token},
                {"@JwtId", token.JwtId },
                {"@IsUsed", token.IsUsed },
                {"@IsRevorked", token.IsRevorked },
                {"@ExpiryDate", DateTime.Now}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    MsSqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, sqlCmd, parameters);
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }


        private IdentityRefreshToken ExtractToken(IDataReader reader)
        {
            var record = new IdentityRefreshToken();

            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.UserId = Utils.ConvertToInt32(reader["UserId"]);
            record.Token = reader["Token"].ToString();

            record.JwtId = reader["JwtId"].ToString();
            record.IsUsed = Utils.ConvertToBoolean(reader["IsUsed"]);
            record.IsRevorked = Utils.ConvertToBoolean(reader["IsRevorked"]);
            record.AddedDate = Utils.ConvertToNullableDateTime(reader["AddedDate"], null);
            record.ExpiryDate = Utils.ConvertToNullableDateTime(reader["ExpiryDate"], null);



            return record;
        }
    }
}
