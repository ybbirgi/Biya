namespace Domain.Entities.ObjectExtensions;

public interface IDeletionAuditedObject : IHasDeletionTime
{
    Guid? DeleterId { get; }
}

public interface IDeletionAuditedObject<TUser> : IDeletionAuditedObject
{
    TUser? Deleter { get; }
}