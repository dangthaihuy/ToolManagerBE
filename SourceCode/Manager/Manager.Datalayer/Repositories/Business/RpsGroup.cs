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
    public class RpsGroup
    {
        private readonly string _conStr;

        public RpsGroup(string connectionString)
        {
            _conStr = connectionString;
        }

        public RpsGroup()
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
        }
        public IdentityGroup GetById(string id)
        {
            int Id = Utils.ConvertToInt32(id);

            var info = new IdentityGroup();

            if (Id <= 0)
            {
                return info;
            }

            var sqlCmd = @"Group_User_GetGroupById";

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
                            info = ExtractGroup(reader);
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

        

        
        public static IdentityGroup ExtractGroup(IDataReader reader)
        {
            var record = new IdentityGroup();

            //Seperate properties


            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.Name = reader["Name"].ToString();




            return record;
        }
    }
}
