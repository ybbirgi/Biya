using Domain.Entities.ObjectExtensions;

namespace Domain.Entities.Auditing;

[Serializable]
public abstract class FullAuditedAggregateRoot : CreationAuditedAggregateRoot, IFullAuditedObject
{
    public virtual DateTime? LastModificationTime { get; set; }

    public virtual Guid? LastModifierId { get; set; }
    
    public virtual bool IsDeleted { get; set; }

    public virtual Guid? DeleterId { get; set; }

    public virtual DateTime? DeletionTime { get; set; }
}

[Serializable]
public abstract class FullAuditedAggregateRoot<TKey> : CreationAuditedAggregateRoot<TKey>, IFullAuditedObject
{
    public virtual DateTime? LastModificationTime { get; set; }

    public virtual Guid? LastModifierId { get; set; }
    
    public virtual bool IsDeleted { get; set; }

    public virtual Guid? DeleterId { get; set; }

    public virtual DateTime? DeletionTime { get; set; }

    protected FullAuditedAggregateRoot()
    {

    }

    protected FullAuditedAggregateRoot(TKey id)
    : base(id)
    {

    }
}
