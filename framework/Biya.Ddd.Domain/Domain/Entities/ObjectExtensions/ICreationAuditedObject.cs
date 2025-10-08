namespace Domain.Entities.ObjectExtensions;

public interface ICreationAuditedObject : IHasCreationTime, IMayHaveCreator
{

}

public interface ICreationAuditedObject<TCreator> : ICreationAuditedObject, IMayHaveCreator<TCreator>
{

}
