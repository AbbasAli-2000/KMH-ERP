using KMH_ERP.Application.Interfaces.Repository;
using KMH_ERP.Infrastructure.Data;
using KMH_ERP.Infrastructure.Repositories.DapperRepository;
using KMH_ERP.Infrastructure.Repositories.EFRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace KMH_ERP.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly Dapper_DbConnection _dapperDbConnection;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(AppDbContext dbContext, Dapper_DbConnection dapperDbConnection)
        {
            _dbContext = dbContext;
            _dapperDbConnection = dapperDbConnection;
        }

        public IEFGenericRepository<T> EFRepo<T>() where T : class
        {
            return new EFGenericRepository<T>(_dbContext);

        }
        public IDapperRepository<T> DapperRepo<T>() where T : class
        {
            return new DapperRepository<T>(_dapperDbConnection);
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }
        public async Task CommitTransactionAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
                if (_transaction != null) await _transaction.CommitAsync();
            }
            finally
            {
                if (_transaction != null) await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
        public async Task RollbackTransactionAsync()
        {
            try
            {
                if (_transaction != null) await _transaction.RollbackAsync();
            }
            finally
            {
                if (_transaction != null) await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _dbContext.Dispose();
        }
    }
}
