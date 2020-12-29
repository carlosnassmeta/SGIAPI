using IMS.Infrastructure.Context;
using System;
using System.Threading.Tasks;

namespace IMS.Infrastructure.Repository.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationContext Context { get; }
        Task<int> SaveChangesAsync();
        int SaveChanges();
    }
}
