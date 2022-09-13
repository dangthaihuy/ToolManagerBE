
using Manager.DataLayer.Entities;
using Manager.SharedLibs;
using Manager.SharedLibs.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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

       
        public int Register(IdentityInformationUser identity)
        {
            var sqlCmd = @"APIUser_Insert";
            int newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {   
                {"@Email", identity.Email },
                {"@PasswordHash", identity.PasswordHash },
                {"@Fullname", identity.Fullname },

                
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

        public IdentityInformationUser Login(IdentityInformationUser identity)
        {
            IdentityInformationUser info = null;
            var sqlCmd = @"APIUser_Login";

            var parameters = new Dictionary<string, object>
            {
                {"@Email", identity.Email},
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

        public List<IdentityCurrentUser> GetList()
        {
            var sqlCmd = @"APIUser_GetList";
            List<IdentityCurrentUser> listData = new List<IdentityCurrentUser>();

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, null))
                    {
                        while (reader.Read())
                        {
                            var info = new IdentityCurrentUser();
                            info = ExtractCurrentUserData(reader);

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

        public IdentityInformationUser GetById(string id)
        {
            int userid = Utils.ConvertToInt32(id);
            var info = new IdentityInformationUser();

            if (userid <= 0)
            {
                return info;
            }

            var sqlCmd = @"APIUser_GetCurrentById";

            var parameters = new Dictionary<string, object>
            {
                {"@Id", userid}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
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

        public IdentityInformationUser GetInforUser(int id)
        {
            var info = new IdentityInformationUser();

            if (id <= 0)
            {
                return info;
            }

            var sqlCmd = @"APIUser_GetInforUser";

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

        public bool Update(IdentityInformationUser identity)
        {
            //Common syntax
            var sqlCmd = @"APIUser_Update";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id},
                {"@NewPassword", identity.PasswordHash},

                {"@Fullname", identity.Fullname},
                {"@Phone", identity.Phone},

                {"@Avatar", identity.Avatar },
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

        public static IdentityInformationUser ExtractUserData(IDataReader reader)
        {
            var record = new IdentityInformationUser();

            //Seperate properties
            if (reader.HasColumn("TotalCount"))
                record.TotalCount = Utils.ConvertToInt32(reader["TotalCount"]);

            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.Email = reader["Email"].ToString();
            record.Fullname = reader["FullName"].ToString();
            record.Avatar = reader["Avatar"].ToString();
            record.Phone = reader["Phone"].ToString();

            return record;
        }

        public static IdentityCurrentUser ExtractCurrentUserData(IDataReader reader)
        {
            var record = new IdentityCurrentUser();

            //Seperate properties
            

            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.Email = reader["Email"].ToString();
            record.Fullname = reader["FullName"].ToString();
            record.Avatar = reader["Avatar"].ToString();


            return record;
        }




    }
}
