namespace Biya;

public class BasicOrganizationUnitInfo
{
    public Guid? OrganizationUnitId { get; }

    public string? Name { get; }

    public BasicOrganizationUnitInfo(Guid? organizationUnitId, string? name = null)
    {
        OrganizationUnitId = organizationUnitId;
        Name = name;
    }
}