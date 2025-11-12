namespace Biya;

public interface ICurrentOrganizationUnitAccessor
{
    BasicOrganizationUnitInfo? Current { get; set; }
}