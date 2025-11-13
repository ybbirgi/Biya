using System.Linq.Expressions;
using Domain.Entities;

namespace Domain.Repositories;

public interface IReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity, Guid>
    where TEntity : class, IEntity<Guid>
{
}

public interface IReadOnlyRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    IQueryable<TEntity> GetQueryable(
        bool includeDetails = true
    );

    Task<TEntity?> GetAsync(
        TKey id,
        bool includeDetails = true,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    );

    Task<TEntity?> GetByAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool includeDetails = true,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    );

    Task<TEntity?> GetByQueryableAsync(
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryable = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    );

    Task<List<TEntity>> GetListAsync(
        bool includeDetails = false,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
    );

    Task<List<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool includeDetails = false,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
    );

    Task<List<TEntity>> GetListByQueryableAsync(
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryable = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    );
}