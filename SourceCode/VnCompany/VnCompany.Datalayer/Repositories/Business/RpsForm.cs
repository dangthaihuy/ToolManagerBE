using VnCompany.DataLayer.Entities.Business;
using VnCompany.SharedLibs;
using VnCompany.SharedLibs.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace VnCompany.DataLayer.Repositories.Business
{
    public class RpsForm
    {
        private readonly string _conStr;

        public RpsForm(string connectionString)
        {
            _conStr = connectionString;
        }

        #region  Common
        public List<IdentityForm> GetByPage(IdentityForm filter, int currentPage, int pageSize)
        {
            //Common syntax           
            var sqlCmd = @"Form_GetByPage";
            List<IdentityForm> listData = null;

            //For paging 
            int offset = (currentPage - 1) * pageSize;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Keyword", filter.Keyword},
                {"@Status", filter.Status},
                {"@Offset", offset},
                {"@PageSize", pageSize},
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        listData = ParsingListFormFromReader(reader);
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

        private List<IdentityForm> ParsingListFormFromReader(IDataReader reader)
        {
            List<IdentityForm> listData = _ = new List<IdentityForm>();
            while (reader.Read())
            {
                //Get common information
                var record = ExtractFormData(reader);

                //Extends information
                if (reader.HasColumn("TotalCount"))
                    record.TotalCount = Utils.ConvertToInt32(reader["TotalCount"]);

                listData.Add(record);
            }

            return listData;
        }

        private IdentityForm ExtractFormData(IDataReader reader)
        {
            var record = new IdentityForm
            {
                //Seperate properties
                Id = Utils.ConvertToInt32(reader["Id"]),
                Name = reader["Name"].ToString(),
                Code = reader["Code"].ToString(),
                ShortDescription = reader["ShortDescription"].ToString(),

                CreatedDate = reader["CreatedDate"] == DBNull.Value ? null : (DateTime?)reader["CreatedDate"],
                Status = Utils.ConvertToInt32(reader["Status"])
            };

            return record;
        }

        private IdentityFormField ExtractFormFieldData(IDataReader reader)
        {
            var record = new IdentityFormField();

            //Seperate properties
            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.FormId = Utils.ConvertToInt32(reader["FormId"]);
            record.Template = reader["Template"].ToString();

            if (!string.IsNullOrEmpty(record.Template))
            {
                record.TemplateInfo = JsonConvert.DeserializeObject<IdentityFormFieldTemplate>(record.Template);

                if (record.TemplateInfo != null)
                {
                    record.TemplateInfo.FieldId = record.Id;
                    record.TemplateInfo.FormId = record.FormId;
                }
            }

            return record;
        }

        public int Insert(IdentityForm identity)
        {
            //Common syntax           
            var sqlCmd = @"Form_Insert";
            var newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Code", identity.Code },
                {"@Name", identity.Name},
                {"@ShortDescription", identity.ShortDescription},
                {"@CreatedBy", identity.CreatedBy},
                {"@Status", identity.Status},
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, parameters);
                    newId = Convert.ToInt32(returnObj);
                };
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return newId;
        }

        public int InsertFormField(IdentityFormField identity)
        {
            //Common syntax           
            var sqlCmd = @"Form_InsertFormField";
            var newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@FormId", identity.FormId },
                {"@Template", identity.Template},
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, parameters);
                    newId = Convert.ToInt32(returnObj);
                };
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return newId;
        }

        public bool Update(IdentityForm identity)
        {
            //Common syntax
            var sqlCmd = @"Form_Update";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id},
                {"@Code", identity.Code },
                {"@Name", identity.Name},
                {"@ShortDescription", identity.ShortDescription },
                {"@LastUpdatedBy", identity.LastUpdatedBy },
                {"@Status", identity.Status}
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

        public IdentityForm GetById(int Id)
        {
            var info = new IdentityForm();
            var sqlCmd = @"Form_GetById";

            var parameters = new Dictionary<string, object>
            {
                {"@Id", Id}
            };
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        //Base info
                        if (reader.Read())
                        {
                            info = ExtractFormData(reader);
                        }

                        //All fields
                        if (reader.NextResult())
                        {
                            info.Fields = new List<IdentityFormField>();

                            while (reader.Read())
                            {
                                var field = ExtractFormFieldData(reader);

                                info.Fields.Add(field);
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
            return info;
        }
        public List<IdentityFormField> GetFormFieldsByFormId(int Id)
        {
            var listData = new List<IdentityFormField>();
            var sqlCmd = @"Form_GetFormFieldsByFormId";

            var parameters = new Dictionary<string, object>
            {
                {"@Id", Id}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using(var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            var temp = ExtractFormFieldData(reader);
                            listData.Add(temp);
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

        public bool Delete(int Id)
        {
            //Common syntax            
            var sqlCmd = @"Form_Delete";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", Id},
            };

            try
            {
                using var conn = new SqlConnection(_conStr);
                MsSqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, sqlCmd, parameters);
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public bool UpdateFormField(IdentityFormField identity)
        {
            //Common syntax
            var sqlCmd = @"FormField_Update";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id},
                {"@Template", identity.Template}
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

        public bool DeleteFormField(int id)
        {
            //Common syntax            
            var sqlCmd = @"FormField_Delete";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", id},
            };

            try
            {
                using var conn = new SqlConnection(_conStr);
                MsSqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, sqlCmd, parameters);
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public List<IdentityForm> GetList()
        {
            //Common syntax            
            var sqlCmd = @"Form_GetList";

            List<IdentityForm> listData = new List<IdentityForm>();
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, null))
                    {
                        while (reader.Read())
                        {
                            var record = ExtractFormData(reader);

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

    }
}
