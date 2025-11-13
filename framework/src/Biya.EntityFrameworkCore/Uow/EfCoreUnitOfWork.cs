using Domain.Uow;
using Microsoft.EntityFrameworkCore;
using Biya.DependencyInjection;

namespace Biya.EntityFrameworkCore.Uow;

public class EfCoreUnitOfWork<TDbContext> : IUnitOfWork, IScopedDependency
    where TDbContext : DbContext
{
    private readonly TDbContext _dbContext;

    public EfCoreUnitOfWork(TDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public virtual Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.Database.CommitTransactionAsync(cancellationToken);
    }

    public virtual Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.Database.RollbackTransactionAsync(cancellationToken);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}