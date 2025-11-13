namespace Domain.Entities.ObjectExtensions;

public interface ISoftDelete : IHasDeletionTime , IDeletionAuditedObject
{
    public bool IsDeleted { get; set; }
}