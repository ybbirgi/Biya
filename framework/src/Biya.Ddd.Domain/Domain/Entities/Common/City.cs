using Domain.Entities.Auditing;
using Domain.Entities.ObjectExtensions;

namespace Domain.Entities.Common;

public class City : CreationAuditedEntity<int>, ILookup
{
    public int CountryCode { get; set; }
    public string Name { get; set; }
    public int? Code { get; set; }

    public virtual Country Country { get; set; }
    public virtual IList<District> Districts { get; set; }
}