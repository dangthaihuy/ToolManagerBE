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
    public class RpsProject
    {
        private readonly string _conStr;

        public RpsProject(string connectionString)
        {
            _conStr = connectionString;
        }

        public RpsProject()
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
        }

        public int InsertProject(IdentityProject identity)
        {
            var sqlCmd = @"Project_Insert";
            int newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Name", identity.Name },
                {"@CreatedBy", identity.CreatedBy },
                {"@Description", identity.Description }

            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, parameters);

                    newId = Convert.ToInt32(returnObj);

                    if (identity.MemberIds.HasData())
                    {
                        foreach(var userId in identity.MemberIds)
                        {
                            var param = new Dictionary<string, object>
                            {
                                {"@UserId", userId },
                                {"@ProjectId", newId },
                                {"@Role", 2 }

                            };
                            MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, @"Project_InsertUser", param);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return newId;
        }

        public List<int> DeleteProject(IdentityProject identity)
        {
            var list = new List<int>();

            var sqlCmd = @"Project_Delete";
            int newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id }
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            var res = Utils.ConvertToInt32(reader["Id"]);
                            list.Add(res);
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

        public IdentityProject UpdateProject(IdentityProject identity)
        {
            var info = new IdentityProject();

            //Common syntax
            var sqlCmd = @"Project_Update";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id},
                {"@Name", identity.Name},
                {"@Description", identity.Description},
                {"@Avatar", identity.Avatar},
                {"@Process", identity.Process},
                {"@Status", identity.Status},
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (reader.Read())
                        {
                            info = ExtractProject(reader);
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

        public List<int> GetProjectByUserId(int id)
        {
            var list = new List<int>();

            var sqlCmd = @"Project_GetByUserId";

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
                        while (reader.Read())
                        {
                            var res = Utils.ConvertToInt32(reader["ProjectId"]);
                            list.Add(res);
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

        public IdentityProject GetProjectById(int id)
        {
            var res = new IdentityProject();

            var sqlCmd = @"Project_GetById";

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
                            res = ExtractProject(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return res;
        }
        public int InsertUserToProject(IdentityUserProject identity)
        {
            var sqlCmd = @"Project_InsertUser";
            int newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@UserId", identity.UserId },
                {"@ProjectId", identity.ProjectId },
                {"@Role", identity.Role }

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

        public int DeleteUserInProject(IdentityUserProject identity)
        {
            var sqlCmd = @"Project_DeleteUser";
            int newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@UserId", identity.UserId },
                {"@ProjectId", identity.ProjectId }

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

        public int UpdateUserInProject(IdentityUserProject identity)
        {
            var sqlCmd = @"Project_UpdateUser";
            int newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Role", identity.Role },
                {"@UserId", identity.UserId },
                {"@ProjectId", identity.ProjectId }

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


        public int InsertTask(IdentityTask identity)
        {
            var sqlCmd = @"Task_Insert";
            int newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Name", identity.Name },
                {"@ProjectId", identity.ProjectId },
                {"@FeatureId", identity.FeatureId },
                {"@CreatedBy", identity.CreatedBy },
                {"@Assignee", identity.Assignee },
                {"@Process", identity.Process },
                {"@Deadline", identity.Deadline },
                {"@MessageId", identity.MessageId },
                {"@Description", identity.Description },

            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, parameters);
                    
                    newId = Convert.ToInt32(returnObj);
                    if (newId > 0 && identity.Files.HasData())
                    {
                        foreach (var file in identity.Files)
                        {
                            var param = new Dictionary<string, object>
                            {
                                {"@Name", file.Name},
                                {"@ProjectId", identity.ProjectId},
                                {"@TaskId", newId},
                                {"@FeatureId", identity.FeatureId},
                                {"@Path", file.Path},
                            };
                            MsSqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, @"Project_InsertAttachment", param);
                        }
                    }

                    
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return newId;
        }

        public List<string> DeleteTask(int id)
        {
            var list = new List<string>();

            var sqlCmd = @"Task_Delete";

            var parameters = new Dictionary<string, object>
            {
                {"@TaskId", id}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            var res = reader["Path"].ToString();
                            list.Add(res);
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

        public IdentityTask UpdateTask(IdentityTask identity)
        {
            var info = new IdentityTask();

            //Common syntax
            var sqlCmd = @"Task_Update";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id},
                {"@Name", identity.Name},
                {"@Description", identity.Description},
                {"@Status", identity.Status},
                {"@Process", identity.Process},
                {"@Assignee", identity.Assignee},
                {"@Deadline", identity.Deadline},
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (reader.Read())
                        {
                            info = ExtractTask(reader);
                        }
                    }

                    if (identity.Files.HasData())
                    {
                        foreach (var file in identity.Files)
                        {
                            var param = new Dictionary<string, object>
                        {
                            {"@Name", file.Name},
                            {"@ProjectId", identity.ProjectId},
                            {"@TaskId", identity.Id},
                            {"@FeatureId", identity.FeatureId},
                            {"@Path", file.Path},
                        };
                            MsSqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, @"Project_InsertAttachment", param);
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
        public List<int> GetTaskByUserId(int id)
        {
            var list = new List<int>();

            var sqlCmd = @"Task_GetByUserId";

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
                        while (reader.Read())
                        {
                            var res = Utils.ConvertToInt32(reader["Id"]);
                            list.Add(res);
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

        public IdentityTask GetTaskById(int id)
        {
            var res = new IdentityTask();

            var sqlCmd = @"Task_GetById";

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
                            res = ExtractTask(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return res;
        }

        public List<int> GetTaskByProjectId(int id)
        {
            var list = new List<int>();

            var sqlCmd = @"Task_GetByProjectId";

            var parameters = new Dictionary<string, object>
            {
                {"@ProjectId", id}
            };
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            var res = Utils.ConvertToInt32(reader["Id"]);
                            list.Add(res);
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

        public List<int> GetTaskIdByFeatureId(int id)
        {
            var list = new List<int>();

            var sqlCmd = @"Task_GetByFeatureId";

            var parameters = new Dictionary<string, object>
            {
                {"@FeatureId", id}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            var res = Utils.ConvertToInt32(reader["Id"]);
                            list.Add(res);
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

        public List<string> GetUserByProjectId(int id)
        {
            var list = new List<string>();

            var sqlCmd = @"ApiUser_GetByProjectId";

            var parameters = new Dictionary<string, object>
            {
                {"@ProjectId", id}
            };
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            var res = reader["UserId"].ToString();
                            list.Add(res);
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
        public List<IdentityProjectAttachment> GetAttachmentByProjectId(int id)
        {
            var list = new List<IdentityProjectAttachment>();

            var sqlCmd = @"Project_GetAttachmentByProjectId";

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
                            var res = ExtractAttachment(reader);
                            list.Add(res);
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
        public List<int> GetProjectBySearch(IdentitySearchFilter identity)
        {
            var res = new List<int>();

            var sqlCmd = @"Project_GetBySearch";

            int offset = (identity.CurrentPage - 1) * identity.PageSize;

            var parameters = new Dictionary<string, object>
            {
                {"@Keyword", identity.Keyword},
                {"@PageSize", identity.PageSize},
                {"@Offset", offset},
                {"@UserId", identity.UserId},

            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            var projectId = Utils.ConvertToInt32(reader["ProjectId"]);
                            res.Add(projectId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return res;
        }

        public int InsertUserToTask(IdentityUserProject identity)
        {
            var sqlCmd = @"Task_InsertUser";
            int newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@UserId", identity.UserId },
                {"@TaskId", identity.TaskId }

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
        public int DeleteUserInTask(IdentityUserProject identity)
        {
            var sqlCmd = @"Task_DeleteUser";
            int newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@UserId", identity.UserId },
                {"@TaskId", identity.TaskId }

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

        

        public List<IdentityProjectAttachment> GetAttachmentByTaskId(int id)
        {
            var list = new List<IdentityProjectAttachment>();

            var sqlCmd = @"Project_GetAttachmentByTaskId";

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
                            var res = ExtractAttachment(reader);
                            list.Add(res);
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

        public List<int> GetTaskBySearch(IdentitySearchFilter identity)
        {
            var res = new List<int>();

            var sqlCmd = @"Task_GetBySearch";

            int offset = (identity.CurrentPage - 1) * identity.PageSize;

            var parameters = new Dictionary<string, object>
            {
                {"@Keyword", identity.Keyword},
                {"@PageSize", identity.PageSize},
                {"@Offset", offset},
                {"@UserId", identity.UserId},

            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            var taskId = Utils.ConvertToInt32(reader["Id"]);

                            res.Add(taskId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return res;
        }

        public IdentityProjectAttachment DeleteAttachmentById(int id)
        {
            var res = new IdentityProjectAttachment();

            var sqlCmd = @"Project_DeleteAttachmentById";

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
                            res = ExtractAttachment(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return res;
        }
        public int GetRoleUser(IdentityUserProject identity)
        {
            var res = new int();

            var sqlCmd = @"Project_GetRoleUser";

            var parameters = new Dictionary<string, object>
            {
                {"@UserId", identity.UserId},
                {"@ProjectId", identity.ProjectId}
            };
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (reader.Read())
                        {
                            res = Utils.ConvertToInt32(reader["Role"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return res;
        }



        public IdentityFeature InsertFeature(IdentityFeature identity)
        {
            var res = new IdentityFeature();
            var sqlCmd = @"Feature_Insert";
            int newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Name", identity.Name },
                {"@ProjectId", identity.ProjectId },
                {"@ParentId", identity.ParentId },
                {"@CreatedBy", identity.CreatedBy },
                {"@Description", identity.Description },
                 

            }; 

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (reader.Read())
                        {
                            res = ExtractFeature(reader);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return res;
        }

        public List<int> GetChild(int parentId)
        {
            var list = new List<int>();

            //Common syntax
            var sqlCmd = @"Feature_GetChild";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
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
                            var res = Utils.ConvertToInt32(reader["Id"]);
                            list.Add(res);
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
        public List<string> DeleteFeature(int id)
        {
            var list = new List<string>();
            var sqlCmd = @"Feature_Delete";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", id }
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            var res = reader["Path"].ToString();
                            list.Add(res);
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
        public IdentityFeature UpdateFeature(IdentityFeature identity)
        {
            var info = new IdentityFeature();

            //Common syntax
            var sqlCmd = @"Feature_Update";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id},
                {"@Name", identity.Name},
                {"@Description", identity.Description}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (reader.Read())
                        {
                            info = ExtractFeature(reader);
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
        public IdentityFeature GetFeatureById(int id)
        {
            var res = new IdentityFeature();

            var sqlCmd = @"Feature_GetById";

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
                            res = ExtractFeature(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return res;
        }

        public List<int> GetFeatureByProjectId(int id)
        {
            var list = new List<int>();

            var sqlCmd = @"Feature_GetByProjectId";

            var parameters = new Dictionary<string, object>
            {
                {"@ProjectId", id}
            };
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            var res = Utils.ConvertToInt32(reader["Id"]);
                            list.Add(res);
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

        public List<int> GetAllFeatureByProjectId(int id)
        {
            var list = new List<int>();

            var sqlCmd = @"Feature_GetAllByProjectId";

            var parameters = new Dictionary<string, object>
            {
                {"@ProjectId", id}
            };
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            var res = Utils.ConvertToInt32(reader["Id"]);
                            list.Add(res);
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

        public List<int> GetSubFeature(int id)
        {
            var list = new List<int>();

            var sqlCmd = @"Feature_GetSubFeature";

            var parameters = new Dictionary<string, object>
            {
                {"@ParentId", id}
            };
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            var res = Utils.ConvertToInt32(reader["Id"]);
                            list.Add(res);
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

        public int InsertFile(IdentityProjectAttachment identity)
        {
            var sqlCmd = @"Project_InsertFile";
            int newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Name", identity.Name },
                {"@ProjectId", identity.ProjectId },
                {"@FeatureId", identity.FeatureId },
                {"@Path", identity.Path }

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

        public string DeleteFile(int id)
        {
            var res = "";
            var sqlCmd = @"Project_DeleteFile";
            
            //For parameters
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
                            res = reader["Path"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return res;
        }

        public List<IdentityProjectAttachment> GetAttachmentByFeatureId(IdentityProjectAttachment identity)
        {
            var res = new List<IdentityProjectAttachment>();
            var sqlCmd = @"Project_GetAttachmentByFeatureId";
            int newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@FeatureId", identity.FeatureId},
                {"@ProjectId", identity.ProjectId}

            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            var file = ExtractAttachment(reader);
                            res.Add(file);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return res;
        }

        public int InsertNotif(IdentityNotification identity)
        {
            var sqlCmd = @"Notif_Insert";
            int newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@UserId", identity.UserId },
                {"@Content", identity.Content },
                {"@ProjectId", identity.ProjectId },
                {"@FeatureId", identity.FeatureId },
                {"@TaskId", identity.TaskId },
                {"@CreatedBy", identity.CreatedBy },
                {"@IsRead", identity.IsRead },

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

        public List<IdentityNotification> GetNotificationByUserId(int id)
        {
            var res = new List<IdentityNotification>();
            var sqlCmd = @"Notif_GetByUserId";

            //For parameters
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
                        while (reader.Read())
                        {
                            var noti = ExtractNotif(reader);
                            res.Add(noti);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return res;
        }

        public IdentityNotification UpdateReadNotif(IdentityNotification identity)
        {
            var res = new IdentityNotification();
            var sqlCmd = @"Notif_UpdateRead";

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Id", identity.Id}

            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            res = ExtractNotif(reader);
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return res;
        }
        private IdentityProject ExtractProject(IDataReader reader)
        {
            var record = new IdentityProject();

            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.Name = reader["Name"].ToString();
            record.CreatedBy = Utils.ConvertToInt32(reader["CreatedBy"]);
            record.CreatedDate = DateTime.Parse(reader["CreatedDate"].ToString());
            record.Process = Utils.ConvertToInt32(reader["Process"]);
            record.Avatar = reader["Avatar"].ToString();
            record.Description = reader["Description"].ToString();
            record.Status = Utils.ConvertToInt32(reader["Status"]);

            return record;
        }

        private IdentityTask ExtractTask(IDataReader reader)
        {
            var record = new IdentityTask();

            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.Name = reader["Name"].ToString();
            record.ProjectId = Utils.ConvertToInt32(reader["ProjectId"]);
            record.CreatedBy = Utils.ConvertToInt32(reader["CreatedBy"]);
            record.Description = reader["Description"].ToString();
            record.CreatedDate = DateTime.Parse(reader["CreatedDate"].ToString());
            record.Deadline = DBNull.Value == reader["Deadline"] ? null : DateTime.Parse(reader["Deadline"].ToString());
            record.FeatureId = Utils.ConvertToInt32(reader["FeatureId"]);
            record.Assignee = Utils.ConvertToInt32(reader["Assignee"]);
            record.Process = Utils.ConvertToInt32(reader["Process"]);
            record.MessageId = Utils.ConvertToInt32(reader["MessageId"]);
            record.Status = Utils.ConvertToInt32(reader["Status"]);

            return record;
        }

        private IdentityProjectAttachment ExtractAttachment(IDataReader reader)
        {
            var record = new IdentityProjectAttachment();

            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.Name = reader["Name"].ToString();
            record.ProjectId = Utils.ConvertToInt32(reader["ProjectId"]);
            record.TaskId = Utils.ConvertToInt32(reader["TaskId"]);
            record.Path = reader["Path"].ToString();

            return record;
        }

        private IdentityFeature ExtractFeature(IDataReader reader)
        {
            var record = new IdentityFeature();

            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.Name = reader["Name"].ToString();
            record.ProjectId = Utils.ConvertToInt32(reader["ProjectId"]);
            record.ParentId = Utils.ConvertToInt32(reader["ParentId"]);
            record.CreatedBy = Utils.ConvertToInt32(reader["CreatedBy"]);
            record.CreatedDate = DateTime.Parse(reader["CreatedDate"].ToString());
            record.Description = reader["Description"].ToString();

            return record;
        }

        private IdentityNotification ExtractNotif(IDataReader reader)
        {
            var record = new IdentityNotification();

            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.UserId = Utils.ConvertToInt32(reader["UserId"]);
            record.ProjectId = Utils.ConvertToInt32(reader["ProjectId"]);
            record.FeatureId = Utils.ConvertToInt32(reader["FeatureId"]);
            record.Content = reader["Content"].ToString();
            record.TaskId = Utils.ConvertToInt32(reader["TaskId"]);
            record.CreatedBy = Utils.ConvertToInt32(reader["CreatedBy"]);
            record.CreatedDate = DateTime.Parse(reader["CreatedDate"].ToString());
            record.IsRead = Utils.ConvertToBoolean(reader["IsRead"]);


            return record;
        }

    }
}
