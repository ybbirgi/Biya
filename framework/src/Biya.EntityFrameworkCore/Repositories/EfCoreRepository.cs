using System.Linq.Dynamic.Core;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Biya.DependencyInjection;
using System.Linq.Expressions;
using Domain.Models;

namespace Biya.EntityFrameworkCore.Repositories;

public abstract class EfCoreRepository<TDbContext, TEntity, TKey> : IRepository<TEntity, TKey>, IScopedDependency
    where TDbContext : DbContext
    where TEntity : class, IEntity<TKey>
{
    protected readonly IDbContextProvider<TDbContext> DbContextProvider;

    protected EfCoreRepository(
        IDbContextProvider<TDbContext> dbContextProvider
    )
    {
        DbContextProvider = dbContextProvider;
    }

    protected virtual TDbContext DbContext => DbContextProvider.GetDbContext();
    protected virtual DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

    protected virtual IQueryable<TEntity> ApplyNoTracking(
        IQueryable<TEntity> query,
        bool asNoTracking
    )
    {
        return asNoTracking ? query.AsNoTracking() : query;
    }

    public virtual IQueryable<TEntity> GetQueryable(
        bool includeDetails = true
    )
    {
        return DbSet.AsQueryable();
    }

    public virtual async Task<TEntity?> GetAsync(
        TKey id,
        bool includeDetails = true,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        var entity = await GetByAsync(
            p => p.Id!.Equals(id),
            includeDetails: includeDetails,
            asNoTracking: asNoTracking,
            cancellationToken: cancellationToken
        );

        return entity;
    }

    public virtual async Task<TEntity?> GetByAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool includeDetails = true,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        var query = GetQueryable(includeDetails).Where(predicate);
        query = ApplyNoTracking(query, asNoTracking);

        var entity = await query.FirstOrDefaultAsync(cancellationToken: cancellationToken);
        return entity;
    }

    public virtual async Task<TEntity?> GetByQueryableAsync(
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryable = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        IQueryable<TEntity>? queryableData = GetQueryable();

        if (queryable != null)
        {
            queryableData = queryable(queryableData);
        }

        queryableData = ApplyNoTracking(queryableData, asNoTracking);

        return await queryableData.FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetListAsync(
        bool includeDetails = false,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        var query = GetQueryable(includeDetails);
        query = ApplyNoTracking(query, asNoTracking);

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool includeDetails = false,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        var query = GetQueryable(includeDetails).Where(predicate);
        query = ApplyNoTracking(query, asNoTracking);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<TEntity>> GetListByQueryableAsync(
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryable = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IQueryable<TEntity>? queryableData = GetQueryable();

        if (queryable != null)
        {
            queryableData = queryable(queryableData);
        }

        queryableData = ApplyNoTracking(queryableData, asNoTracking);

        return await queryableData.ToListAsync(cancellationToken: cancellationToken);
    }

    public virtual Task<long> GetCountAsync(
        CancellationToken cancellationToken = default
    )
    {
        return DbSet.LongCountAsync(cancellationToken);
    }

    public virtual Task<long> GetCountAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    )
    {
        return DbSet.LongCountAsync(predicate, cancellationToken);
    }

    public virtual async Task<TEntity> InsertAsync(
        TEntity entity,
        bool autoSave = false,
        CancellationToken cancellationToken = default
    )
    {
        var savedEntity = (await DbSet.AddAsync(entity, cancellationToken)).Entity;

        if (autoSave)
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        return savedEntity;
    }

    public virtual async Task InsertManyAsync(
        IEnumerable<TEntity> entities,
        bool autoSave = false,
        CancellationToken cancellationToken = default
    )
    {
        await DbSet.AddRangeAsync(entities, cancellationToken);

        if (autoSave)
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public virtual async Task<TEntity> UpdateAsync(
        TEntity entity,
        bool autoSave = false,
        CancellationToken cancellationToken = default
    )
    {
        DbSet.Update(entity);

        if (autoSave)
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        return entity;
    }

    public virtual async Task UpdateManyAsync(
        IEnumerable<TEntity> entities,
        bool autoSave = false,
        CancellationToken cancellationToken = default
    )
    {
        DbSet.UpdateRange(entities);

        if (autoSave)
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public virtual async Task DeleteAsync(
        TEntity entity,
        bool autoSave = false,
        CancellationToken cancellationToken = default
    )
    {
        DbSet.Remove(entity);

        if (autoSave)
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public virtual async Task DeleteAsync(
        TKey id,
        bool autoSave = false,
        CancellationToken cancellationToken = default
    )
    {
        var entity = await DbSet.FindAsync(new object?[] { id }, cancellationToken: cancellationToken);

        if (entity == null)
        {
            return;
        }

        await DeleteAsync(entity, autoSave, cancellationToken);
    }

    public virtual async Task DeleteManyAsync(
        IEnumerable<TEntity> entities,
        bool autoSave = false,
        CancellationToken cancellationToken = default
    )
    {
        DbSet.RemoveRange(entities);

        if (autoSave)
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public virtual async Task DeleteManyAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool autoSave = false,
        CancellationToken cancellationToken = default
    )
    {
        var entities = await GetListAsync(predicate, asNoTracking: true, cancellationToken: cancellationToken);
        await DeleteManyAsync(entities, autoSave, cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetDynamicListAsync(
        DynamicQueryModel input,
        bool includeDetails = false,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        var query = GetQueryable(includeDetails);

        if (!string.IsNullOrWhiteSpace(input.Filter))
        {
            query = query.Where(input.Filter, input.FilterArgs ?? Array.Empty<object>());
        }

        if (!string.IsNullOrWhiteSpace(input.OrderBy))
        {
            query = query.OrderBy(input.OrderBy);
        }

        query = query
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        query = ApplyNoTracking(query, asNoTracking);

        return await query.ToListAsync(cancellationToken);
    }

    public virtual Task<long> GetDynamicCountAsync(
        DynamicQueryModel input,
        CancellationToken cancellationToken = default
    )
    {
        var query = GetQueryable(false);

        if (!string.IsNullOrWhiteSpace(input.Filter))
        {
            query = query.Where(input.Filter, input.FilterArgs ?? Array.Empty<object>());
        }

        return query.LongCountAsync(cancellationToken);
    }
}