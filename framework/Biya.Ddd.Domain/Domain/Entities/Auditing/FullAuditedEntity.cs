using Domain.Entities.ObjectExtensions;

namespace Domain.Entities.Auditing;

[Serializable]
public abstract class FullAuditedEntity : CreationAuditedEntity, IFullAuditedObject
{
    public virtual DateTime? LastModificationTime { get; set; }

    public virtual Guid? LastModifierId { get; set; }
    
    public virtual bool IsDeleted { get; set; }

    public virtual Guid? DeleterId { get; set; }

    public virtual DateTime? DeletionTime { get; set; }
}

[Serializable]
public abstract class FullAuditedEntity<TKey> : CreationAuditedEntity<TKey>, IFullAuditedObject
{
    public virtual DateTime? LastModificationTime { get; set; }

    public virtual Guid? LastModifierId { get; set; }
    
    public virtual bool IsDeleted { get; set; }

    public virtual Guid? DeleterId { get; set; }

    public virtual DateTime? DeletionTime { get; set; }

    protected FullAuditedEntity()
    {

    }

    protected FullAuditedEntity(TKey id)
        : base(id)
    {

    }
}
