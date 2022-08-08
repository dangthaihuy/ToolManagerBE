using Manager.DataLayer.Entities;
using Manager.SharedLibs;
using Manager.SharedLibs.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.DataLayer.Repositories.System
{
    public class APIRpsUser
    {
        private readonly string _conStr;

        public APIRpsUser(string connectionString)
        {
            _conStr = connectionString;
        }

        public APIRpsUser()
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
        }

        public int Register(IdentityUser identity)
        {
            var sqlCmd = @"APIUser_Insert";
            int newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@UserName", identity.UserName },
                {"@PasswordHash", identity.PasswordHash },
                {"@FullName", identity.FullName },

                {"@PhoneNumber", identity.PhoneNumber },
                {"@Email", identity.Email }
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

        public IdentityUser Login(IdentityUser identity)
        {
            IdentityUser info = null;
            var sqlCmd = @"APIUser_Login";

            var parameters = new Dictionary<string, object>
            {
                {"@UserName", identity.UserName},
                {"@Password", identity.PasswordHash}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (reader.Read())
                        {
                            info = ExtractUserData(reader);
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

        public List<IdentityUser> GetList()
        {
            var sqlCmd = @"APIUser_GetList";
            List<IdentityUser> listData = new List<IdentityUser>();

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, null))
                    {
                        while (reader.Read())
                        {
                            var info = new IdentityUser();
                            info = ExtractUserData(reader);

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



        public static IdentityUser ExtractUserData(IDataReader reader)
        {
            var record = new IdentityUser();

            //Seperate properties
            if (reader.HasColumn("TotalCount"))
                record.TotalCount = Utils.ConvertToInt32(reader["TotalCount"]);

            record.Id = reader["Id"].ToString();
            record.UserName = reader["UserName"].ToString();
            record.Email = reader["Email"].ToString();
            record.FullName = reader["FullName"].ToString();
            record.PhoneNumber = reader["PhoneNumber"].ToString();

            
            return record;
        }



    }
}
