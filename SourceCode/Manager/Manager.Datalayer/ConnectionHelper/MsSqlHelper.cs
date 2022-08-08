using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Manager.DataLayer
{
    public static class MsSqlHelper
    {

        public static int ExecuteNonQuery(SqlConnection conn, string cmdText, Dictionary<string, object> cmdParms)
        {
            SqlCommand cmd = conn.CreateCommand();
            try
            {
                PrepareCommand(cmd, conn, null, CommandType.Text, cmdText, cmdParms);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        public static int ExecuteNonQueryTransaction(SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, Dictionary<string, object> cmdParms)
        {
            SqlCommand cmd = conn.CreateCommand();
            try
            {
                PrepareCommand(cmd, conn, trans, cmdType, cmdText, cmdParms);

                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        public static int ExecuteNonQuery(SqlConnection conn, CommandType cmdType, string cmdText, Dictionary<string, object> cmdParms)
        {
            SqlCommand cmd = conn.CreateCommand();
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        public static IDataReader ExecuteReader(SqlConnection conn, CommandType cmdType, string cmdText, Dictionary<string, object> cmdParms)
        {
            SqlCommand cmd = conn.CreateCommand();
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return rdr;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static DataSet ExecuteReaderToDataSet(SqlConnection conn, CommandType cmdType, string cmdText, Dictionary<string, object> cmdParms)
        {
            SqlCommand cmd = conn.CreateCommand();
            try
            {
                DataSet dtSet = new DataSet();

                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);

                SqlDataAdapter adapter = new SqlDataAdapter();

                adapter.SelectCommand = cmd;
                adapter.Fill(dtSet);

                return dtSet;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static object ExecuteScalar(SqlConnection conn, CommandType cmdType, string cmdText, Dictionary<string, object> cmdParms)
        {
            SqlCommand cmd = conn.CreateCommand();
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }

        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, Dictionary<string, object> cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                foreach (var param in cmdParms)
                {
                    var parameter = cmd.CreateParameter();
                    parameter.ParameterName = param.Key;
                    if (param.Value == null)
                    {
                        parameter.Value = DBNull.Value;
                    }
                    else
                    {
                        parameter.Value = param.Value;
                    }

                    cmd.Parameters.Add(parameter);
                }

            }
        }

        public static void PrepareCommand(this SqlCommand cmd, Dictionary<string, object> cmdParms, SqlTransaction trans = null)
        {
            if (cmdParms != null)
            {
                if (trans != null)
                {
                    cmd.Transaction = trans;
                }

                foreach (var param in cmdParms)
                {
                    var parameter = cmd.CreateParameter();
                    parameter.ParameterName = param.Key;
                    if (param.Value == null)
                    {
                        parameter.Value = DBNull.Value;
                    }
                    else
                    {
                        parameter.Value = param.Value;
                    }

                    cmd.Parameters.Add(parameter);
                }
            }
        }

        public static SqlCommand TransactionInitStoreProcedureCommand(SqlConnection conn, SqlTransaction trans, string sqlCmd, Dictionary<string, object> parms)
        {
            // Command Objects for the transaction
            SqlCommand cmdCreateRoot = new SqlCommand(sqlCmd, conn);
            cmdCreateRoot.CommandType = CommandType.StoredProcedure;
            cmdCreateRoot.PrepareCommand(parms, trans);

            return cmdCreateRoot;
        }
    }
}
