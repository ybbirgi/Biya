namespace Domain.Entities.ObjectExtensions;

public interface IHasConcurrencyStamp
{
    string ConcurrencyStamp { get; set; }
}