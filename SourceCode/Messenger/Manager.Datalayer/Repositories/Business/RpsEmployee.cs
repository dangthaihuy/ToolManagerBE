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
    public class RpsEmployee
    {
        private readonly string _conStr;

        public RpsEmployee(string connectionString)
        {
            _conStr = connectionString;
        }

        public RpsEmployee()
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
        }

        public int Insert(IdentityEmployee identity)
        {
            //Common syntax           
            var sqlCmd = @"Employee_Insert";
            int newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@JoinDate", identity.JoinDate },
                {"@LeaveDate", identity.LeaveDate },
                {"@AvatarUrl", identity.JoinDate },

                {"@Department", identity.Department },
                {"@CompanyId", identity.CompanyId },
                {"@CreatedBy", identity.CreatedBy },

                {"@Status", identity.Status },
                {"@EmploymentType", identity.EmploymentType }
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

        public List<IdentityEmployee> GetByPage(IdentityEmployee filter, int currentPage, int pageSize)
        {
            var sqlCmd = @"Employee_GetByPage";
            List<IdentityEmployee> listData = new List<IdentityEmployee>();

            int offset = (currentPage - 1) * pageSize;

            var parameters = new Dictionary<string, object>
            {
                {"@CreatedBy", filter.CreatedBy },
                {"@Keyword", filter.Keyword },
                {"@Offset", offset},
                {"@PageSize", pageSize},
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            var info = new IdentityEmployee();
                            info.Id = Utils.ConvertToInt32(reader["Id"]);

                            if (reader.HasColumn("TotalCount"))
                                info.TotalCount = Utils.ConvertToInt32(reader["TotalCount"]);

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

        public List<IdentityEmployee> GetList()
        {
            var sqlCmd = @"Employee_GetList";
            List<IdentityEmployee> listData = new List<IdentityEmployee>();

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, null))
                    {
                        while (reader.Read())
                        {
                            var info = new IdentityEmployee();
                            info = ExtractEmployee(reader);

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

        public IdentityEmployee GetById(int id)
        {
            var info = new IdentityEmployee();

            if (id <= 0)
            {
                return info;
            }

            var sqlCmd = @"Employee_GetById";

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
                            info = ExtractEmployee(reader);
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

        private IdentityEmployee ExtractEmployee(IDataReader reader)
        {
            var record = new IdentityEmployee();

            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.CompanyId = reader["CompanyId"].ToString();
            record.CreatedBy = reader["CreatedBy"].ToString();

            record.AvatarUrl = reader["AvatarUrl"].ToString();
            record.Department = reader["Department"].ToString();
            record.EmploymentType = Utils.ConvertToInt32(reader["EmploymentType"]);
            record.EmployeeCode = reader["EmployeeCode"].ToString();

            record.JoinDate = reader["JoinDate"] == DBNull.Value ? null : (DateTime?)reader["JoinDate"];
            record.LeaveDate = reader["LeaveDate"] == DBNull.Value ? null : (DateTime?)reader["LeaveDate"];         

            record.Status = Utils.ConvertToInt32(reader["Status"]);

            return record;
        }
    }
}
