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
                {"@CreateBy", identity.CreatedBy }


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
    }
}
