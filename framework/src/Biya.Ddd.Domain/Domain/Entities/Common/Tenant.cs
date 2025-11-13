using Domain.Entities.Auditing;
using Domain.Entities.ObjectExtensions;

namespace Domain.Entities.Common;

public class Tenant : FullAuditedAggregateRoot<Guid>, IHasIsActive
{
    public int? CountryCode { get; set; }
    public int? CityCode { get; set; }
    public int? DistrictCode { get; set; }
    public int? NeighborhoodCode { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }

    public virtual Country Country { get; set; }
    public virtual City City { get; set; }
    public virtual District District { get; set; }
    public virtual Neighborhood Neighborhood { get; set; }
    public virtual IList<OrganizationUnit> OrganizationUnits { get; set; }
}