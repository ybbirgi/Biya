using Domain.Entities.Auditing;
using Domain.Entities.ObjectExtensions;

namespace Domain.Entities.Common;

public class Country : CreationAuditedEntity<Guid>, ILookup
{
    public string Name { get; set; }
    public int? Code { get; set; }
    
    public virtual IList<City> Cities { get; set; }
}