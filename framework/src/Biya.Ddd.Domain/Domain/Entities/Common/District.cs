using Domain.Entities.Auditing;
using Domain.Entities.ObjectExtensions;

namespace Domain.Entities.Common;

public class District : CreationAuditedEntity<Guid> , ILookup
{
    public int CityCode { get; set; }
    public string Name { get; set; }
    public int? Code { get; set; }
    
    public virtual City City { get; set; }
}