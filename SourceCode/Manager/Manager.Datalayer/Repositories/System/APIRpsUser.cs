﻿
using Manager.DataLayer.Entities;
using Manager.DataLayer.Helpers;
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
        private readonly AppSettings _appSettings;

        public APIRpsUser(string connectionString)
        {
            _conStr = connectionString;
        }

        public APIRpsUser()
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
            
        }

        public APIRpsUser(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }


        private List<IdentityUser> _users = new List<IdentityUser>
        {
            new IdentityUser { Id = "1", UserName = "test", PasswordHash = "test" }
        };

        

        public IEnumerable<IdentityUser> GetAll()
        {
            return _users;
        }



        public int Register(IdentityUser identity)
        {
            var sqlCmd = @"APIUser_Insert";
            int newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {   
                {"@Email", identity.Email },
                {"@PasswordHash", identity.PasswordHash },
                {"@FullName", identity.FullName },

                
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

        public IdentityUser GetById(int id)
        {
            return _users.FirstOrDefault(x => x.Id == Convert.ToString(id));
        }



        public static IdentityUser ExtractUserData(IDataReader reader)
        {
            var record = new IdentityUser();

            //Seperate properties
            if (reader.HasColumn("TotalCount"))
                record.TotalCount = Utils.ConvertToInt32(reader["TotalCount"]);

            record.Id = reader["Id"].ToString();
            record.Email = reader["Email"].ToString();
            record.FullName = reader["FullName"].ToString();

            
            return record;
        }


        private string generateJwtToken(IdentityUser user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


    }
}
