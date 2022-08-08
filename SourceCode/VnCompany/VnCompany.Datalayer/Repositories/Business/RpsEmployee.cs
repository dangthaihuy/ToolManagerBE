using VnCompany.DataLayer.Entities.Business;
using VnCompany.SharedLibs;
using VnCompany.SharedLibs.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VnCompany.DataLayer.Repositories.Business
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

                {"@EmployeeCode", identity.EmployeeCode },
                {"@Department", identity.Department },
                {"@CompanyId", identity.CompanyId },
                {"@CreatedBy", identity.CreatedBy },

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

        public int InsertAll (IdentityEmployee employee, IdentityEmployeeContact contact)
        {
            var sqlCmdForEmployee = @"Employee_Insert";
            var sqlCmdForContact = @"Employee_Contact_Insert";

            int newEmployeeId = 0;
            int newEmployeeContactId = 0;

            var transName = @"Employee_InsertAll";

            var parametersForEmployee = new Dictionary<string, object>
            {
                {"@JoinDate", employee.JoinDate },
                {"@LeaveDate", employee.LeaveDate },
                {"@AvatarUrl", employee.AvatarUrl },

                {"@EmployeeCode", employee.EmployeeCode },
                {"@Department", employee.Department },
                {"@CompanyId", employee.CompanyId },
                {"@CreatedBy", employee.CreatedBy },

                {"@EmploymentType", employee.EmploymentType }
            };

           

            try
            {
                SqlConnection conn = new SqlConnection(_conStr);
                conn.Open();

                SqlTransaction trans = conn.BeginTransaction(transName);

                try
                {
                    var returnObj = MsSqlHelper.TransactionInitStoreProcedureCommand(conn, trans, sqlCmdForEmployee, parametersForEmployee).ExecuteScalar();
                    newEmployeeId = Convert.ToInt32(returnObj);

                    var parametersForContact = new Dictionary<string, object>
                    {
                        {"@EmployeeId", newEmployeeId },
                        {"@IsEmployee", contact.IsEmployee },
                        {"@Relationship", contact.Relationship },

                        {"@FamilyName", contact.FamilyName },
                        {"@FamilyNameYomigana", contact.FamilyNameYomigana },
                        {"@Name", contact.Name },
                        {"@NameYomigana", contact.NameYomigana },

                        {"@Gender", contact.Gender },
                        {"@DateOfBirth", contact.DateOfBirth },
                        {"@Email", contact.Email },

                        {"@NationalityId", contact.NationalityId },
                        {"@VisaType", contact.VisaType },
                        {"@VisaExpiryDate", contact.VisaExpiryDate },

                        {"@PostalCode", contact.PostalCode },
                        {"@PhoneNumber", contact.PhoneNumber },

                        {"@PrefectureId", contact.PrefectureId },
                        {"@District", contact.District },
                        {"@Street", contact.Street },
                        {"@RoomNumber", contact.RoomNumber },
                        {"@AddressYomigana", contact.AddressYomigana }
                    };

                    var returnObject = MsSqlHelper.TransactionInitStoreProcedureCommand(conn, trans, sqlCmdForContact, parametersForContact).ExecuteScalar();
                    newEmployeeContactId = Convert.ToInt32(returnObject);

                    trans.Commit();
                }

                catch (Exception tranEx)
                {
                    trans.Rollback();

                    throw tranEx;
                }

                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", transName, ex.Message);
                throw new CustomSQLException(strError);
            }

            return newEmployeeId;
        }

        public List<IdentityEmployee> GetByPage(IdentityEmployee filter, int currentPage, int pageSize)
        {
            var sqlCmd = @"Employee_GetByPage";
            List<IdentityEmployee> listData = new List<IdentityEmployee>();

            int offset = (currentPage - 1) * pageSize;

            var parameters = new Dictionary<string, object>
            {
                {"@CompanyId", filter.CompanyId },
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

        

        public bool Update(IdentityEmployee identity)
        {
            //Common syntax
            var sqlCmd = @"Employee_Update";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id },
                {"@JoinDate", identity.JoinDate },
                {"@LeaveDate", identity.LeaveDate },
                {"@AvatarUrl", identity.JoinDate },

                {"@EmployeeCode", identity.EmployeeCode },
                {"@Department", identity.Department },
                {"@CompanyId", identity.CompanyId },
                {"@CreatedBy", identity.CreatedBy },

                {"@EmploymentType", identity.EmploymentType }
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

        public bool UpdateAll(IdentityEmployee employee, IdentityEmployeeContact contact)
        {
            var sqlCmdForEmployee = @"Employee_Update";
            var sqlCmdForContact = @"Employee_Contact_Update";         

            var transName = @"Employee_UpdateAll";

            var parametersForEmployee = new Dictionary<string, object>
            {
                {"@Id", employee.Id },
                {"@JoinDate", employee.JoinDate },
                {"@LeaveDate", employee.LeaveDate },
                {"@AvatarUrl", employee.AvatarUrl },

                {"@EmployeeCode", employee.EmployeeCode },
                {"@Department", employee.Department },
                {"@CompanyId", employee.CompanyId },
                {"@CreatedBy", employee.CreatedBy },

                {"@EmploymentType", employee.EmploymentType }
            };

            var parametersForContact = new Dictionary<string, object>
            {
                {"@EmployeeId", employee.Id },
                {"@IsEmployee", contact.IsEmployee },
                {"@Relationship", contact.Relationship },

                {"@FamilyName", contact.FamilyName },
                {"@FamilyNameYomigana", contact.FamilyNameYomigana },
                {"@Name", contact.Name },
                {"@NameYomigana", contact.NameYomigana },

                {"@Gender", contact.Gender },
                {"@DateOfBirth", contact.DateOfBirth },
                {"@Email", contact.Email },

                {"@NationalityId", contact.NationalityId },
                {"@VisaType", contact.VisaType },
                {"@VisaExpiryDate", contact.VisaExpiryDate },

                {"@PostalCode", contact.PostalCode },
                {"@PhoneNumber", contact.PhoneNumber },

                {"@PrefectureId", contact.PrefectureId },
                {"@District", contact.District },
                {"@Street", contact.Street },
                {"@RoomNumber", contact.RoomNumber },
                {"@AddressYomigana", contact.AddressYomigana }
            };

            try
            {
                SqlConnection conn = new SqlConnection(_conStr);
                conn.Open();

                SqlTransaction trans = conn.BeginTransaction(transName);

                try
                {
                    MsSqlHelper.TransactionInitStoreProcedureCommand(conn, trans, sqlCmdForEmployee, parametersForEmployee).ExecuteNonQuery();                  
                    MsSqlHelper.TransactionInitStoreProcedureCommand(conn, trans, sqlCmdForContact, parametersForContact).ExecuteNonQuery();
   
                    trans.Commit();
                }

                catch (Exception tranEx)
                {
                    trans.Rollback();

                    throw tranEx;
                }

                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", transName, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public bool Delete(int employeeId)
        {
            var sqlCmdForEmployee = @"Employee_Delete";
            var sqlCmdForContact = @"Employee_Contact_Delete";

            var transName = @"Employee_DeleteAll";

            var parameters = new Dictionary<string, object>
            {
                {"@EmployeeId", employeeId},
            };

            try
            {
                SqlConnection conn = new SqlConnection(_conStr);
                conn.Open();

                SqlTransaction trans = conn.BeginTransaction(transName);

                try
                {
                    MsSqlHelper.TransactionInitStoreProcedureCommand(conn, trans, sqlCmdForEmployee, parameters).ExecuteNonQuery();
                    MsSqlHelper.TransactionInitStoreProcedureCommand(conn, trans, sqlCmdForContact, parameters).ExecuteNonQuery();                   

                    trans.Commit();
                }

                catch (Exception tranEx)
                {
                    trans.Rollback();

                    throw tranEx;
                }

                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", transName, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
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
