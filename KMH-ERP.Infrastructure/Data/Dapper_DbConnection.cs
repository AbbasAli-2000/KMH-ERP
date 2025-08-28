using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace KMH_ERP.Infrastructure.Data
{
    public class Dapper_DbConnection
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionName;

        public Dapper_DbConnection(IConfiguration configuration, string connectionName = "KMH_ERPDbConnDefault")
        {
            _configuration = configuration;
            _connectionName = connectionName;
        }

        public IDbConnection GetConnection()
        {
            var connectionString = _configuration.GetConnectionString(_connectionName);
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Database connection string is not configured.");

            var connection = new SqlConnection(connectionString);
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            return connection;
        }
    }
}
