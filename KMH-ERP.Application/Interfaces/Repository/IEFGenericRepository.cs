using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KMH_ERP.Application.Interfaces.Repository
{
    public  interface IEFGenericRepository<T> where T : class
    {

        // Add or Update entity (based on primary key).
        Task AddOrUpdateAsync(T entity);

        /// update entity.
        void Update(T entity);

        /// Add entity.
        Task AddAsync(T entity);

        //Delete entity.
        void Remove(T entity);

        //Check if any record matches filter.
        Task<bool> AnyAsync(Expression<Func<T, bool>> filter);

        //Get a single record.
        Task<T?> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);

        //Get all records (with optional filter).
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool tracked = false);

        //Add multiple entities in one go.
        Task BulkInsertAsync(IEnumerable<T> entities);

        Task<T?> FindAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IQueryable<T>>? queryShaper = null, bool tracked = false);

    }
}
