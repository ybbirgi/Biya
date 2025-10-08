namespace Domain.Entities.ObjectExtensions;

public interface IHasDeletionTime
{
    DateTime? DeletionTime { get; }
}