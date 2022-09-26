﻿using Manager.DataLayer.Entities.Business;
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
                {"@CreatedBy", identity.CreatedBy }

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

        public int DeleteProject(IdentityProject identity)
        {
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
                {"Avatar", identity.Avatar},
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


        public int InsertTask(IdentityTask identity)
        {
            var sqlCmd = @"Task_Insert";
            int newId = 0;

            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Name", identity.Name },
                {"@ProjectId", identity.ProjectId },
                {"@CreatedBy", identity.CreatedBy },

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

        public int DeleteTask(IdentityTask identity)
        {
            var sqlCmd = @"Task_Delete";
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
                {"Description", identity.Description},
                {"Status", identity.Status},
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
                            var res = Utils.ConvertToInt32(reader["TaskId"]);
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
                        if (res != null && reader.NextResult())
                        {
                            res.File = new List<IdentityProjectAttachment>();
                            while (reader.Read())
                            {
                                res.File.Add(ExtractAttachment(reader));
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


        public List<int> GetUserByProjectId(int id)
        {
            var list = new List<int>();

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
                            var res = Utils.ConvertToInt32(reader["UserId"]);
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

        private IdentityProject ExtractProject(IDataReader reader)
        {
            var record = new IdentityProject();

            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.Name = reader["Name"].ToString();
            record.CreatedBy = Utils.ConvertToInt32(reader["CreatedBy"]);
            record.CreatedDate = DateTime.Parse(reader["CreatedDate"].ToString());
            record.Avatar = reader["Avatar"].ToString();

            return record;
        }

        private IdentityTask ExtractTask(IDataReader reader)
        {
            var record = new IdentityTask();

            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.Name = reader["Name"].ToString();
            record.ProjectId = Utils.ConvertToInt32(reader["ProjectId"]);
            record.CreatedBy = Utils.ConvertToInt32(reader["CreatedBy"]);
            record.Description = reader["CreatedBy"].ToString();
            record.CreatedDate = DateTime.Parse(reader["CreatedDate"].ToString());
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
    }
}
