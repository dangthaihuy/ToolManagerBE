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
    public class RpsMenuService
    {
        private readonly string _conStr;

        public RpsMenuService(string connectionString)
        {
            _conStr = connectionString;
        }

        public List<IdentityMenuService> GetByPage(IdentityMenuService filter, int currentPage, int pageSize)
        {
            //Common syntax           
            var sqlCmd = @"Menu_Service_GetByPage";
            List<IdentityMenuService> listData = null;

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
                        listData = ParsingListMenuFromReader(reader);
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

        private List<IdentityMenuService> ParsingListMenuFromReader(IDataReader reader)
        {
            List<IdentityMenuService> listData = _ = new List<IdentityMenuService>();
            while (reader.Read())
            {
                //Get common inMenuation
                var record = ExtractMenuData(reader);

                //Extends inMenuation
                if (reader.HasColumn("TotalCount"))
                    record.TotalCount = Utils.ConvertToInt32(reader["TotalCount"]);

                listData.Add(record);
            }

            return listData;
        }

        private IdentityMenuService ExtractMenuData(IDataReader reader)
        {
            var record = new IdentityMenuService
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

        public int Insert(IdentityMenuService identity)
        {
            //Common syntax           
            var sqlCmd = @"Menu_Service_Insert";
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
        public bool Update(IdentityMenuService identity)
        {
            //Common syntax
            var sqlCmd = @"Menu_Service_Update";

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

        public IdentityMenuService GetById(int Id)
        {
            var info = new IdentityMenuService();
            var sqlCmd = @"Menu_Service_GetById";

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
                        while (reader.Read())
                        {
                            info = ExtractMenuData(reader);
                        }

                        //All form ids
                        if (reader.NextResult())
                        {
                            info.WorkFlows = new List<IdentityWorkFlow>();
                            while (reader.Read())
                            {
                                var workFlow = new IdentityWorkFlow();
                                workFlow.Id = Utils.ConvertToInt32(reader["Id"]);

                                info.WorkFlows.Add(workFlow);
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
            var sqlCmd = @"Menu_Service_Delete";

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

        public int InsertMenuServiceWorkflow(IdentityMenuWorkFlow identity)
        {
            //Common syntax           
            var sqlCmd = @"Menu_Service_InsertMenuWorkflow";
            var newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@MenuId", identity.MenuId },
                {"@WorkflowId", identity.WorkFlowId},
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

        public bool UpdateMenuServiceWorkflow(IdentityMenuWorkFlow identity)
        {
            //Common syntax
            var sqlCmd = @"Menu_Service_UpdateMenuWorkflow";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id},
                {"@MenuId", identity.MenuId },
                {"@WorkflowId", identity.WorkFlowId},
                {"@SortOrder", identity.SortOrder },
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

        public bool DeleteMenuServiceWorkflow(IdentityMenuWorkFlow identity)
        {
            //Common syntax            
            var sqlCmd = @"Menu_Service_DeleteMenuWorkflow";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id},
                {"@MenuId", identity.MenuId },
                {"@WorkflowId", identity.WorkFlowId }
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
    }
}
