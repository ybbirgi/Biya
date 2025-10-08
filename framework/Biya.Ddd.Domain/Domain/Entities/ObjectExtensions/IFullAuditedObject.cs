namespace Domain.Entities.ObjectExtensions;

public interface IFullAuditedObject : ICreationAuditedObject, IModificationAuditedObject, IDeletionAuditedObject
{
}

public interface IFullAuditedObject<TUser> : IFullAuditedObject, ICreationAuditedObject<TUser>,
    IModificationAuditedObject<TUser>
{
}