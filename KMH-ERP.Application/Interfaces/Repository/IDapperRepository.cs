using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMH_ERP.Application.Interfaces.Repository
{
    public interface IDapperRepository<T> where T : class
    {

        Task<int> CreateAsync(string storedProcedure, object parameters, string outParameter);
        Task<int> CreateAsync(string storedProcedure, object parameters);
        Task<IEnumerable<T>> GetAllQueryAsync(string storedProcedure);
        Task<IEnumerable<T>> GetQueryAsync(string storedProcedure, object parameters);
        Task<T?> GetSingleAsync(string storedProcedure, object parameters);
        Task BulkInsertAsync(IEnumerable<T> entities, string tableName);

    }
}
