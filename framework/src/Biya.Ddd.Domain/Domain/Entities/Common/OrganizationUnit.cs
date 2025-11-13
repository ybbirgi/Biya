using Biya;
using Domain.Entities.Auditing;
using Domain.Entities.ObjectExtensions;

namespace Domain.Entities.Common;

public class OrganizationUnit : FullAuditedAggregateRoot<Guid>, IHasIsActive, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid? ParentId { get; set; }
    public int? CountryCode { get; set; }
    public int? CityCode { get; set; }
    public int? DistrictCode { get; set; }
    public int? NeighborhoodCode { get; set; }
    public string Name { get; set; }
    public string? PrimaryCode { get; set; }
    public string? SecondaryCode { get; set; }
    public bool IsActive { get; set; }

    public virtual Tenant Tenant { get; set; }
    public virtual OrganizationUnit? Parent { get; set; }
    public virtual IList<OrganizationUnit> Childs { get; set; }
    public virtual Country Country { get; set; }
    public virtual City City { get; set; }
    public virtual District District { get; set; }
    public virtual Neighborhood Neighborhood { get; set; }
}