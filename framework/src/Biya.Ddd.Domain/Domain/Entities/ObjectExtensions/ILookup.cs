namespace Domain.Entities.ObjectExtensions;

public interface ILookup
{
    public string Name { get; set; }
    public int? Code { get; set; }
}