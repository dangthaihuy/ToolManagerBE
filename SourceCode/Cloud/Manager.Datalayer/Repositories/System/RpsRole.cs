using Manager.DataLayer.Entities;
using Manager.SharedLibs;
using Manager.SharedLibs.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Manager.DataLayer.Repositories
{
    public class RpsRole
    {
        private readonly string _conStr;

        public RpsRole(string connectionString)
        {
            _conStr = connectionString;
        }

        public RpsRole()
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
        }

        private IdentityRole ExtractRoleData(IDataReader reader)
        {
            var record = new IdentityRole();

            record.Id = reader["Id"].ToString();
            record.Name = reader["Name"].ToString();
            record.UserId = reader["UserId"].ToString();
            record.AgencyId = Utils.ConvertToInt32(reader["Agency_id"]);
            
            return record;
        }

        public List<IdentityRole> GetList()
        {
            //Common syntax            
            var sqlCmd = @"Role_GetList";

            List<IdentityRole> listData = new List<IdentityRole>();
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, null))
                    {
                        while (reader.Read())
                        {
                            var record = ExtractRoleData(reader);

                            listData.Add(record);
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

        public List<IdentityRole> GetListByAgencyId(int agencyId)
        {
            //Common syntax            
            var sqlCmd = @"Role_GetListByAgencyId";

            List<IdentityRole> listData = new List<IdentityRole>();
            try
            {
                var parms = new Dictionary<string, object>
                {
                    {"@AgencyId", agencyId},
                };

                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parms))
                    {
                        while (reader.Read())
                        {
                            var record = ExtractRoleData(reader);

                            listData.Add(record);
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

        public List<IdentityRole> GetRolesByUserId(string userId)
        {
            //Common syntax            
            var sqlCmd = @"Role_GetByUserId";

            List<IdentityRole> listData = new List<IdentityRole>();
            try
            {
                var parms = new Dictionary<string, object>
                {
                    {"@UserId", userId},
                };

                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parms))
                    {
                        while (reader.Read())
                        {
                            var record = ExtractRoleData(reader);

                            listData.Add(record);
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

        public IdentityRole GetRoleByUserId(string userId)
        {
            var sqlCmd = @"Role_GetByUserId";
            var info = new IdentityRole();

            try
            {
                var parms = new Dictionary<string, object>
                {
                    {"@UserId", userId},
                };

                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parms))
                    {
                        if (reader.Read())
                        {
                            info = ExtractRoleData(reader);
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

        public IdentityRole GetById(string roleId)
        {
            //Common syntax            
            var sqlCmd = @"Role_GetById";

            IdentityRole info = null;
            try
            {
                var parms = new Dictionary<string, object>
                {
                    {"@Id", roleId},
                };

                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.Text, @"SELECT TOP 1 * FROM aspnetroles WHERE 1=1 AND Id = @Id", parms))
                    {
                        if (reader.Read())
                        {
                            info = ExtractRoleData(reader);
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

        public int Insert(IdentityRole info)
        {
            var sqlCmd = @"Role_Insert";
            var result = 1;
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var parms = new Dictionary<string, object>
                    {
                        {"@Name", info.Name},
                        {"@UserId", info.UserId},
                        {"@AgencyId", info.AgencyId},
                    };

                    var objReturn = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, parms);
                    result = Utils.ConvertToInt32(objReturn);
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return result;
        }

        public int Update(IdentityRole info)
        {
            var sqlCmd = @"Role_Update";
            var result = 1;
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var parms = new Dictionary<string, object>
                    {
                        {"@Id", info.Id},
                        {"@Name", info.Name},
                        {"@AgencyId", info.AgencyId},
                    };

                    var objReturn = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, parms);
                    result = Utils.ConvertToInt32(objReturn);
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return result;
        }

        public void AddUserToRole(string userId, string roleId)
        {
            var sqlCmd = @"User_AddToRole";
            try
            {
                if (!string.IsNullOrEmpty(roleId))
                {
                    using (var conn = new SqlConnection(_conStr))
                    {
                        var parms = new Dictionary<string, object>
                        {
                            {"@UserId", userId},
                            {"@RoleId", roleId}
                        };

                        MsSqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, sqlCmd, parms);
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }
        }

        public void DeleteUserInRole(string userId, string roleId)
        {
            var sqlCmd = @"Role_DeleteUserInRole";
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var parms = new Dictionary<string, object>
                    {
                        {"@Id", userId},
                        {"@RoleId", roleId}
                    };

                    MsSqlHelper.ExecuteNonQuery(conn, @"Delete FROM AspNetUserRoles WHERE UserId=@Id AND RoleId=@RoleId", parms);
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }
        }

        public void Delete(string roleId)
        {
            var sqlCmd = @"Role_Delete";
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var parms = new Dictionary<string, object>
                    {
                        {"@Id", roleId}
                    };

                    //Delete role
                    MsSqlHelper.ExecuteNonQuery(conn, @"Delete FROM aspnetroles WHERE Id = @Id", parms);

                    //Delete relationship User-Role
                    MsSqlHelper.ExecuteNonQuery(conn, @"Delete FROM AspNetUserRoles WHERE RoleId = @Id", parms);
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }
        }
    }
}
