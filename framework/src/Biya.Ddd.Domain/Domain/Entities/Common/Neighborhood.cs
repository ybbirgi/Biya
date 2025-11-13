using Domain.Entities.Auditing;
using Domain.Entities.ObjectExtensions;

namespace Domain.Entities.Common;

public class Neighborhood : CreationAuditedEntity<Guid> , ILookup
{
    public int DistrictCode { get; set; }
    public string Name { get; set; }
    public int? Code { get; set; }
}