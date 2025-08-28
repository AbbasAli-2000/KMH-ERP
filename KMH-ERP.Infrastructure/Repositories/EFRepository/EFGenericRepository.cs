using KMH_ERP.Application.Exceptions;
using KMH_ERP.Application.Interfaces.Repository;
using KMH_ERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KMH_ERP.Infrastructure.Repositories.EFRepository
{
    public class EFGenericRepository<T> : IEFGenericRepository<T> where T : class
    {

        private readonly AppDbContext _appDbContext;
        private DbSet<T> _dbSet;

        public EFGenericRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _dbSet = _appDbContext.Set<T>();
        }


        #region EF Core CRUD  



        //Get a single record.
        public async Task<T?> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            try
            {
                IQueryable<T> query = tracked ? _dbSet : _dbSet.AsNoTracking();
                query = query.Where(filter);
                if (!string.IsNullOrWhiteSpace(includeProperties))
                {
                    foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp);
                    }
                }
                return await query.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Error while fetching {typeof(T).Name}", ex);
            }
        }


        public async Task<T?> FindAsync( Expression<Func<T, bool>> filter,Func<IQueryable<T>, IQueryable<T>>? queryShaper = null, bool tracked = false)
        {
            try
            {
                IQueryable<T> query = tracked ? _dbSet : _dbSet.AsNoTracking();

                query = query.Where(filter);
                if (queryShaper != null)
                {
                    query = queryShaper(query);
                }
                return await query.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Error while finding {typeof(T).Name}", ex);
            }
        }


        //Get all records (with optional filter).
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool tracked = false)
        {
            try
            {
                IQueryable<T> query = tracked ? _dbSet : _dbSet.AsNoTracking();

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (!string.IsNullOrEmpty(includeProperties))
                {
                    foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp);
                    }
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Error while fetching all {typeof(T).Name}", ex);
            }



        }



        // Add or Update entity (based on primary key).
        public async Task AddOrUpdateAsync(T entity)
        {
            try
            {
                var entry = _appDbContext.Entry(entity);
                var key = entry.Metadata.FindPrimaryKey()?.Properties.FirstOrDefault();
                var keyValue = key != null ? entry.Property(key.Name).CurrentValue : null;

                bool isNew = keyValue == null ||
                             (keyValue is int intKey && intKey == 0) ||
                             (keyValue is Guid guidKey && guidKey == Guid.Empty);

                if (isNew)
                {
                    await _dbSet.AddAsync(entity);
                }
                else
                {
                    var existingEntity = await _dbSet.FindAsync(keyValue);
                    if (existingEntity != null)
                    {
                        _appDbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
                    }
                    else
                    {
                        await _dbSet.AddAsync(entity);
                    }
                }
            }
            catch (Exception ex)
            {

                throw new RepositoryException($"Error while saving {typeof(T).Name}", ex);
            }
        }

        /// Add entity.
        public async Task AddAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Error while adding {typeof(T).Name}", ex);
            }
        }


        /// Update entity
        public void Update(T entity)
        {
            try
            {
                _dbSet.Update(entity);
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Error while updating {typeof(T).Name}", ex);
            }
        }

        //Check if any record matches filter.
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            try
            {
                return await _dbSet.AnyAsync(filter);

            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Error while checking existence of {typeof(T).Name}", ex);
            }
        }

        //Delete entity.
        public void Remove(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Error while deleting {typeof(T).Name}", ex);
            }
        }

        //Add multiple entities in one go.
        public async Task BulkInsertAsync(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null || !entities.Any())
                {
                    return;
                }
                await _dbSet.AddRangeAsync(entities);
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Error while bulk inserting {typeof(T).Name}", ex);
            }
        }
        #endregion
    }
}
