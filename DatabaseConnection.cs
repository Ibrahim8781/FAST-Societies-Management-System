using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SocietiesManagementSystem
{
    public class DatabaseConnection
    {
        private static string connectionString = @"Server=.\SQLEXPRESS;Database=SocietiesManagementDB;Integrated Security=true;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static void SetConnectionString(string server, string database, string userId = null, string password = null)
        {
            if (string.IsNullOrEmpty(userId))
            {
                connectionString = $"Server={server};Database={database};Integrated Security=true;";
            }
            else
            {
                connectionString = $"Server={server};Database={database};User Id={userId};Password={password};";
            }
        }

        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Database query error: " + ex.Message);
            }
            return dt;
        }

        public static int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Database execution error: " + ex.Message);
            }
        }

        public static object ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        return cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Database scalar error: " + ex.Message);
            }
        }
    }
}
