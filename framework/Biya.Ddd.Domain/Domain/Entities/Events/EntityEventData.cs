using Biya;

namespace Domain.Entities.Events;

[Serializable]
public class EntityEventData<TEntity>
{
    /// <summary>
    /// Related entity with this event.
    /// </summary>
    public TEntity Entity { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entity">Related entity with this event</param>
    public EntityEventData(TEntity entity)
    {
        Entity = entity;
    }

    public virtual object[] GetConstructorArgs()
    {
        return new object[] { Entity! };
    }

    public virtual bool IsMultiTenant(out Guid? tenantId)
    {
        if (Entity is IMultiTenant multiTenantEntity)
        {
            tenantId = multiTenantEntity.TenantId;
            return true;
        }

        tenantId = null;
        return false;
    }
}
