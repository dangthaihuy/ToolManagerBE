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
    public class RpsWorkFlow
    {
        private readonly string _conStr;

        public RpsWorkFlow(string connectionString)
        {
            _conStr = connectionString;
        }

        #region  Common
        public List<IdentityWorkFlow> GetByPage(IdentityWorkFlow filter, int currentPage, int pageSize)
        {
            //Common syntax           
            var sqlCmd = @"WorkFlow_GetByPage";
            List<IdentityWorkFlow> listData = null;

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

        private List<IdentityWorkFlow> ParsingListFormFromReader(IDataReader reader)
        {
            List<IdentityWorkFlow> listData = _ = new List<IdentityWorkFlow>();
            while (reader.Read())
            {
                //Get common information
                var record = ExtractWorkFlowData(reader);

                //Extends information
                if (reader.HasColumn("TotalCount"))
                    record.TotalCount = Utils.ConvertToInt32(reader["TotalCount"]);

                listData.Add(record);
            }

            return listData;
        }

        private IdentityWorkFlow ExtractWorkFlowData(IDataReader reader)
        {
            var record = new IdentityWorkFlow
            {
                //Seperate properties
                Id = Utils.ConvertToInt32(reader["Id"]),
                Name = reader["Name"].ToString(),
                Code = reader["Code"].ToString(),
                Description = reader["Description"].ToString(),

                CreatedDate = reader["CreatedDate"] == DBNull.Value ? null : (DateTime?)reader["CreatedDate"],
                Status = Utils.ConvertToInt32(reader["Status"])
            };

            return record;
        }

        public int Insert(IdentityWorkFlow identity)
        {
            //Common syntax           
            var sqlCmd = @"WorkFlow_Insert";
            var newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Code", identity.Code },
                {"@Name", identity.Name},
                {"@Description", identity.Description},
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
        public bool Update(IdentityWorkFlow identity)
        {
            //Common syntax
            var sqlCmd = @"WorkFlow_Update";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id},
                {"@Code", identity.Code },
                {"@Name", identity.Name},
                {"@Description", identity.Description },
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

        public IdentityWorkFlow GetById(int Id)
        {
            var info = new IdentityWorkFlow();
            var sqlCmd = @"WorkFlow_GetById";

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
                            info = ExtractWorkFlowData(reader);
                        }

                        //All form ids
                        if (reader.NextResult())
                        {
                            info.Forms = new List<IdentityForm>();
                            while (reader.Read())
                            {
                                var frm = new IdentityForm();
                                frm.Id = Utils.ConvertToInt32(reader["Id"]);

                                info.Forms.Add(frm);
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

        public bool Delete(int Id)
        {
            //Common syntax            
            var sqlCmd = @"WorkFlow_Delete";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", Id},
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

        public int InsertWorkFlowForm(IdentityWorkFlowForm identity)
        {
            //Common syntax           
            var sqlCmd = @"WorkFlow_InsertWorkFlowForm";
            var newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@FormId", identity.FormId },
                {"@WorkFlowId", identity.WorkFlowId},
                {"@SortOrder", identity.SortOrder},
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

        public bool DeleteWorkFlowForm(int workFlowId, int formId)
        {
            //Common syntax           
            var sqlCmd = @"WorkFlow_DeleteWorkflowForm";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@WorkFlowId", workFlowId },
                {"@FormId", formId },
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, parameters);
                };
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public int UpdateWorkFlowForm(IdentityWorkFlowForm identity)
        {
            //Common syntax           
            var sqlCmd = @"WorkFlow_UpdateWorkFlowForm";
            var newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id },
                {"@FormId", identity.FormId },
                {"@WorkFlowId", identity.WorkFlowId},
                {"@SortOrder", identity.WorkFlowId},
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

        #endregion
    }
}
