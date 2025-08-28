using System;
using System.Threading.Tasks;

namespace KMH_ERP.Application.Interfaces.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IEFGenericRepository<T> EFRepo<T>() where T : class;
        IDapperRepository<T> DapperRepo<T>() where T : class;

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
