namespace Domain.Models;

public class DynamicQueryModel
{
    public string? Filter { get; set; }
    public object[]? FilterArgs { get; set; } = Array.Empty<object>();
    public string? OrderBy { get; set; }
    public int SkipCount { get; set; }
    public int MaxResultCount { get; set; } = 10;
}