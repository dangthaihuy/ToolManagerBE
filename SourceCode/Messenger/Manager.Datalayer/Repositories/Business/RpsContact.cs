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
    public class RpsContact
    {
        private readonly string _conStr;

        public RpsContact(string connectionString)
        {
            _conStr = connectionString;
        }

        public RpsContact()
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
        }

        public int Insert(IdentityEmployeeContact identity)
        {
            //Common syntax           
            var sqlCmd = @"Employee_Contact_Insert";
            int newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@EmployeeId", identity.EmployeeId },
                {"@IsEmployee", identity.IsEmployee },
                {"@Relationship", identity.Relationship },

                {"@FamilyName", identity.FamilyName },
                {"@FamilyNameYomigana", identity.FamilyNameYomigana },
                {"@Name", identity.Name },
                {"@NameYomigana", identity.NameYomigana },

                {"@Gender", identity.Gender },
                {"@DateOfBirth", identity.DateOfBirth },
                {"@Email", identity.Email },

                {"@NationalityId", identity.NationalityId },
                {"@VisaType", identity.VisaType },
                {"@VisaExpiryDate", identity.VisaExpiryDate },

                {"@PostalCode", identity.PostalCode },
                {"@PhoneNumber", identity.PhoneNumber },

                {"@PrefectureId", identity.PrefectureId },
                {"@District", identity.District },
                {"@Street", identity.Street }
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

        public IdentityEmployeeContact GetByEmployeeId(int employeeId)
        {
            var info = new IdentityEmployeeContact();

            if (employeeId <= 0)
            {
                return info;
            }

            var sqlCmd = @"Employee_Contact_GetByEmployeeId";

            var parameters = new Dictionary<string, object>
            {
                {"@EmployeeId", employeeId}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            info = ExtractEmployeeContact(reader);
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



        private IdentityEmployeeContact ExtractEmployeeContact(IDataReader reader)
        {
            var record = new IdentityEmployeeContact();

            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.IsEmployee = Utils.ConvertToInt32(reader["IsEmployee"]);
            record.Relationship = Utils.ConvertToInt32(reader["Relationship"]);

            record.FamilyName = reader["FamilyName"].ToString();
            record.FamilyNameYomigana = reader["FamilyNameYomigana"].ToString();
            record.Name = reader["Name"].ToString();
            record.NameYomigana = reader["NameYomigana"].ToString();
            record.Gender = Utils.ConvertToInt32(reader["Gender"]);
            record.DateOfBirth = reader["DateOfBirth"] == DBNull.Value ? null : (DateTime?)reader["DateOfBirth"];
            record.Email = reader["Email"].ToString();

            record.NationalityId = Utils.ConvertToInt32(reader["NationalityId"]);
            record.VisaType = Utils.ConvertToInt32(reader["VisaType"]);
            record.VisaExpiryDate = reader["VisaExpiryDate"] == DBNull.Value ? null : (DateTime?)reader["VisaExpiryDate"];

            record.PostalCode = reader["PostalCode"].ToString();
            record.PhoneNumber = reader["PhoneNumber"].ToString();
            record.PrefectureId = Utils.ConvertToInt32(reader["PrefectureId"]);
            record.District = reader["District"].ToString();
            record.Street = reader["Street"].ToString();

            record.Status = Utils.ConvertToInt32(reader["Status"]);

            return record;
        }
    }
}
