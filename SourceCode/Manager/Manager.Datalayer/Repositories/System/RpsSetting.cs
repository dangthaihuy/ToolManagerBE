using Manager.DataLayer;
using Manager.DataLayer.Entities;
using Manager.SharedLibs;
using Manager.SharedLibs.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Manager.Datalayer.Repositories
{
    public class RpsSetting
    {
        private readonly string _conStr;

        public RpsSetting(string connectionString)
        {
            _conStr = connectionString;
        }

        public RpsSetting()
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
        }

        #region Common

        public List<IdentitySetting> LoadSettings(string settingType)
        {
            List<IdentitySetting> listReturn = null;
            var sqlCmd = @"Settings_LoadSettings";

            var parameters = new Dictionary<string, object>
            {
                {"@pType", settingType}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var reader = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (reader.Read())
                        {
                            if (listReturn == null)
                                listReturn = new List<IdentitySetting>();

                            var entity = FillDataSeting(reader);

                            listReturn.Add(entity);
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return listReturn;
        }

        private IdentitySetting FillDataSeting(IDataReader reader)
        {
            return new IdentitySetting()
            {
                Name = reader["SettingName"].ToString(),
                Type = reader["SettingType"].ToString(),
                Value = reader["SettingValue"].ToString(),
            };
        }

        public bool SaveSettings(List<IdentitySetting> settings)
        {
            //Common syntax            
            var sqlCmd = new StringBuilder();

            try
            {
                if (settings != null && settings.Count > 0)
                {
                    sqlCmd.Append(string.Format("DELETE FROM cmn_settings WHERE 1=1 AND SettingType = '{0}'; ", settings[0].Type));
                    foreach (var st in settings)
                    {
                        sqlCmd.Append(string.Format("INSERT INTO cmn_settings(SettingName, SettingType, SettingValue) VALUES (N'{0}',N'{1}',N'{2}'); ", st.Name, st.Type, st.Value));
                    }
                }

                using (var conn = new SqlConnection(_conStr))
                {
                    MsSqlHelper.ExecuteNonQuery(conn, CommandType.Text, sqlCmd.ToString(), null);
                }
            }
            catch (Exception ex)
            {
                var strError = "Failed to execute SaveSettings. Error: " + ex.Message;
                throw new CustomSQLException(strError);
            }

            return true;
        }
        
        #endregion        
    }
}
