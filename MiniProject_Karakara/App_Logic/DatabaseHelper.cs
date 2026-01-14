using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MiniProject_Karakara
{
    // Design Pattern 1: Singleton
    // Ensures only one instance of the helper exists (though Connections are pooled)
    // and provides a global access point to DB configuration.
    public class DatabaseHelper
    {
        private static DatabaseHelper _instance;
        private readonly string _connectionString;

        // Private constructor prevents external instantiation
        private DatabaseHelper()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ConnKarakara"].ConnectionString;
        }

        // Global access point
        public static DatabaseHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DatabaseHelper();
                }
                return _instance;
            }
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public void ExecuteNonQuery(string query, Action<SqlCommand> parameterizer)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    parameterizer?.Invoke(cmd);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public T ExecuteScalar<T>(string query, Action<SqlCommand> parameterizer)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    parameterizer?.Invoke(cmd);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return result == null || result == DBNull.Value ? default(T) : (T)Convert.ChangeType(result, typeof(T));
                }
            }
        }

        public DataTable ExecuteQuery(string query, Action<SqlCommand> parameterizer)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    parameterizer?.Invoke(cmd);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }
    }
}
