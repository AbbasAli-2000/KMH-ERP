using Dapper;
using KMH_ERP.Application.Exceptions;
using KMH_ERP.Application.Interfaces.Repository;
using KMH_ERP.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace KMH_ERP.Infrastructure.Repositories.DapperRepository
{
    public class DapperRepository<T> : IDapperRepository<T> where T : class
    {
        private readonly Dapper_DbConnection _dbConnection;

        public DapperRepository(Dapper_DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public IDbConnection Connection()
        {
            return _dbConnection.GetConnection();
        }

        public async Task<int> CreateAsync(string storedProcedure, object parameters, string outParameter)
        {
            try
            {
                var dynamicParameters = new DynamicParameters(parameters);
                if (!dynamicParameters.ParameterNames.Contains(outParameter))
                {
                    dynamicParameters.Add(outParameter, dbType: DbType.Int32, direction: ParameterDirection.Output);
                }
                using (var Conn = Connection())
                {
                    await Conn.ExecuteAsync(storedProcedure, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return dynamicParameters.Get<int>(outParameter) > 0 ? dynamicParameters.Get<int>(outParameter) : 0;
                }
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Error executing DML for {typeof(T).Name}", ex);
            }
        }
        public async Task<int> CreateAsync(string storedProcedure, object? parameters = null)
        {
            try
            {
                using var conn = Connection();
                return await conn.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Error executing DML for {typeof(T).Name}", ex);
            }
        }
        public async Task<IEnumerable<T>> GetAllQueryAsync(string storedProcedure)
        {
            try
            {
                using var conn = Connection();
                var result = await conn.QueryAsync<T>(storedProcedure, commandType: CommandType.StoredProcedure);
                return result ?? Enumerable.Empty<T>();
            }
            catch (Exception ex)
            {

                throw new RepositoryException($"Error fetching all records for {typeof(T).Name}", ex);
            }
        }
        public async Task<IEnumerable<T>> GetQueryAsync(string storedProcedure, object? parameters = null)
        {
            try
            {
                using var conn = Connection();
                var result = await conn.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return result ?? Enumerable.Empty<T>();
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Error fetching records for {typeof(T).Name}", ex);
            }
        }
        public async Task<T?> GetSingleAsync(string storedProcedure, object? parameters = null)
        {
            try
            {
                using var conn = Connection();
                return await conn.QuerySingleOrDefaultAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Error fetching single record for {typeof(T).Name}", ex);
            }
        }
        public async Task BulkInsertAsync(IEnumerable<T> entities, string tableName)
        {
            if (entities == null || !entities.Any()) return;

            try
            {
                using var bulkCopy = new SqlBulkCopy((SqlConnection)_dbConnection.GetConnection())
                {
                    DestinationTableName = tableName
                };

                // Convert entities to DataTable
                var dt = new DataTable();
                var props = typeof(T).GetProperties();
                foreach (var prop in props)
                    dt.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                foreach (var item in entities)
                {
                    var values = props.Select(p => p.GetValue(item) ?? DBNull.Value).ToArray();
                    dt.Rows.Add(values);
                }

                await bulkCopy.WriteToServerAsync(dt);
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Error bulk inserting {typeof(T).Name}", ex);
            }
        }
    }
}
