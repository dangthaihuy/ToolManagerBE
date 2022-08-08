using VnCompany.DataLayer;
using VnCompany.DataLayer.Entities;
using VnCompany.SharedLibs;
using VnCompany.SharedLibs.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace VnCompany.Datalayer.Repositories
{
    public class RpsUser
    {
        private readonly string _conStr;

        public RpsUser(string connectionString)
        {
            _conStr = connectionString;
        }

        public RpsUser()
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
        }

        #region Common

        public List<IdentityUser> GetByPage(IdentityUser filter, int currentPage, int pageSize)
        {
            //Common syntax           
            var sqlCmd = @"User_GetByPage";
            List<IdentityUser> listData = null;

            //For paging 
            int offset = (currentPage - 1) * pageSize;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Keyword", filter.Keyword},
                {"@RoleId", filter.RoleId},
                {"@LockedEnable", filter.LockoutEnabled },
                {"@Offset", offset},
                {"@PageSize", pageSize},
                {"@ParentId", filter.ParentId}
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

        private List<IdentityUser> ParsingListUserFromReader(IDataReader reader)
        {
            List<IdentityUser> listData =  new List<IdentityUser>();
            while (reader.Read())
            {
                //Get common information
                var record = ExtractUserData(reader);

                listData.Add(record);
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
            record.Avatar = reader["Avatar"].ToString();
            record.Email = reader["Email"].ToString();
            record.EmailConfirmed = Utils.ConvertToBoolean(reader["EmailConfirmed"]);
            record.FullName = reader["FullName"].ToString();
            record.PhoneNumber = reader["PhoneNumber"].ToString();

            record.CompanyName = reader["CompanyName"].ToString();
            record.CompanyTel = reader["CompanyTel"].ToString();
            record.CompanyAddress = reader["CompanyAddress"].ToString();
            record.CompanyCode = reader["CompanyCode"].ToString();


            record.StaffId = Utils.ConvertToInt32(reader["StaffId"]);
            record.ParentId = Utils.ConvertToInt32(reader["ParentId"]);
            record.LockoutEnabled = Utils.ConvertToBoolean(reader["LockoutEnabled"]);
            record.LockoutEndDateUtc = reader["LockoutEndDateUtc"] != DBNull.Value ? (DateTime?)reader["LockoutEndDateUtc"] : null;
            record.CreatedDateUtc = Convert.ToDateTime(reader["CreatedDateUtc"]);

            return record;
        }

        public static IdentityPermission ExtractPermissionData(IDataReader reader)
        {
            var record = new IdentityPermission();

            //Seperate properties
            record.Action = reader["ActionName"].ToString();
            record.Controller = reader["AccessName"].ToString();

            return record;
        }

        public static IdentityMenu ExtractMenuData(IDataReader reader)
        {
            var item = new IdentityMenu();

            item.Id = Utils.ConvertToInt32(reader["Id"]);
            item.ParentId = reader["ParentId"] != null ? Utils.ConvertToInt32(reader["ParentId"]) : 0;
            item.Area = reader["Area"].ToString();
            item.Name = reader["Name"].ToString();
            item.Title = reader["Title"].ToString();
            item.Desc = reader["Desc"].ToString();
            item.Action = reader["Action"].ToString();
            item.Controller = reader["Controller"].ToString();
            item.Visible = (Utils.ConvertToInt32(reader["Visible"]) == 1) ? true : false;
            item.Authenticate = (Utils.ConvertToInt32(reader["Authenticate"]) == 1) ? true : false;
            item.CssClass = reader["CssClass"].ToString();
            item.SortOrder = reader["SortOrder"] != null ? Utils.ConvertToInt32(reader["SortOrder"]) : 0;
            item.AbsoluteUri = reader["AbsoluteUri"].ToString();
            item.Active = (Utils.ConvertToInt32(reader["Active"]) == 1) ? true : false;
            item.IconCss = reader["IconCss"].ToString();
            item.CheckPermission = Utils.ConvertToBoolean(reader["CheckPermission"]);

            return item;
        }

        private IdentityMenuLang ExtractMenuLangData(IDataReader reader)
        {
            var item = new IdentityMenuLang();
            item.Id = Utils.ConvertToInt32(reader["Id"]);
            item.MenuId = Utils.ConvertToInt32(reader["MenuId"]);
            item.Title = reader["Title"].ToString();
            item.LangCode = reader["LangCode"].ToString();

            return item;
        }

        public string Insert(IdentityUser identity)
        {
            //Common syntax           
            var sqlCmd = @"User_Insert";
            var newId = string.Empty;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@UserName", identity.UserName },
                {"@CompanyName", identity.CompanyName },
                {"@CompanyCode", identity.CompanyCode },
                {"@CompanyAddress", identity.CompanyAddress },
                {"@CompanyTel", identity.CompanyTel },

                {"@PasswordHash", identity.PasswordHash },
                {"@ParentId", identity.ParentId },
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, parameters);

                    newId = returnObj.ToString();
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return newId;
        }

        public bool Update(IdentityUser identity)
        {
            //Common syntax           
            var sqlCmd = @"User_Update";
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id },
                {"@UserName", identity.UserName },
                {"@CompanyName", identity.CompanyName },
                {"@CompanyCode", identity.CompanyCode },
                {"@CompanyAddress", identity.CompanyAddress },
                {"@CompanyTel", identity.CompanyTel },
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, parameters);
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public bool ChangePassword(IdentityUser identity)
        {
            //Common syntax           
            var sqlCmd = @"User_ChangePassword";
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id },
                {"@PasswordHash", identity.PasswordHash }
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, parameters);
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public bool UpdateAvatar(IdentityUser identity)
        {
            //Common syntax           
            var sqlCmd = @"User_UpdateAvatar";
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id },
                {"@Avatar", identity.Avatar }
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, parameters);
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public bool LockAccount(IdentityUser identity)
        {
            //Common syntax           
            var sqlCmd = @"User_LockAccount";
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id },
                {"@LockoutEndDateUtc", identity.LockoutEndDateUtc }
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, parameters);
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public bool UnLockAccount(IdentityUser identity)
        {
            //Common syntax           
            var sqlCmd = @"User_UnLockAccount";
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id }
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, parameters);
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public IdentityUser Login(IdentityUser identity)
        {
            IdentityUser info = null;
            var sqlCmd = @"User_Login";

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

        public IdentityUser GetById(string id)
        {
            IdentityUser info = null;
            var sqlCmd = @"User_GetById";

            var parameters = new Dictionary<string, object>
            {
                {"@UserId", id}
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

        public IdentityUser GetByUserName(string userName)
        {
            IdentityUser info = null;
            var sqlCmd = @"User_GetByUserName";

            var parameters = new Dictionary<string, object>
            {
                {"@UserName", userName}
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

        public IdentityUser GetDetail(int id)
        {
            IdentityUser info = null;
            var sqlCmd = @"User_GetDetail";

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

        public bool Delete(string id)
        {
            //Common syntax            
            var sqlCmd = @"User_Delete";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", id}
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

        public List<IdentityUser> GetList()
        {
            //Common syntax            
            var sqlCmd = @"User_GetList";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
            };

            List<IdentityUser> listData = new List<IdentityUser>();
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, null))
                    {
                        while (reader.Read())
                        {
                            var record = ExtractUserData(reader);

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

        #endregion

        public List<IdentityPermission> GetPermissionsByUser(string userId)
        {
            List<IdentityPermission> list = new List<IdentityPermission>();
            var sqlCmd = @"User_GetAllPermissionById";

            var parameters = new Dictionary<string, object>
            {
                {"@UserId", userId}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            var r = ExtractPermissionData(reader);

                            list.Add(r);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return list;
        }

        public List<IdentityMenu> GetRootMenuByUserId(string userId)
        {
            List<IdentityMenu> list = new List<IdentityMenu>();
            var sqlCmd = @"Menu_GetRootMenuByUserId";

            var parameters = new Dictionary<string, object>
            {
                {"@UserId", userId}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            var r = ExtractMenuData(reader);

                            list.Add(r);
                        }

                        if (list.HasData())
                        {
                            //All languages
                            if (reader.NextResult())
                            {
                                while (reader.Read())
                                {
                                    var langItem = ExtractMenuLangData(reader);
                                    foreach (var item in list)
                                    {
                                        if (item.Id == langItem.MenuId)
                                        {
                                            item.LangList.Add(langItem);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return list;
        }

        public List<IdentityMenu> GetChildMenuByUserId(string userId, int parentId)
        {
            List<IdentityMenu> list = new List<IdentityMenu>();
            var sqlCmd = @"Menu_GetChildMenuByUserId";

            var parameters = new Dictionary<string, object>
            {
                {"@UserId", userId},
                {"@ParentId", parentId}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            var r = ExtractMenuData(reader);

                            list.Add(r);
                        }

                        if (list.HasData())
                        {
                            //All languages
                            if (reader.NextResult())
                            {
                                while (reader.Read())
                                {
                                    var langItem = ExtractMenuLangData(reader);
                                    foreach (var item in list)
                                    {
                                        if (item.Id == langItem.MenuId)
                                        {
                                            item.LangList.Add(langItem);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return list;
        }

        public List<IdentityMenu> GetAllDislayMenu()
        {
            List<IdentityMenu> list = new List<IdentityMenu>();
            var sqlCmd = @"Menu_GetAllDisplayMenu";

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, null))
                    {
                        while (reader.Read())
                        {
                            var r = ExtractMenuData(reader);

                            list.Add(r);
                        }

                        if (list.HasData())
                        {
                            //All languages
                            if (reader.NextResult())
                            {
                                while (reader.Read())
                                {
                                    var langItem = ExtractMenuLangData(reader);
                                    foreach (var item in list)
                                    {
                                        if (item.Id == langItem.MenuId)
                                        {
                                            item.LangList.Add(langItem);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return list;
        }

        public bool UpdateRoleofUser(string userId, string roleId)
        {
            var sqlCmd = @"Role_UpdateRoleOfUser";
            try
            {
                if (!string.IsNullOrEmpty(roleId) && !string.IsNullOrEmpty(userId))
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
            return true;
        }
    }
}
