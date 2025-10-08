namespace Domain.Entities.ObjectExtensions;

public interface IHasModificationTime
{
    DateTime? LastModificationTime { get; }
}