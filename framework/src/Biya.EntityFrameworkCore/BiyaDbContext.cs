using Biya.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Biya.EntityFrameworkCore;

public class BiyaDbContext<TDbContext> : DbContext , ITransientDependency where TDbContext : DbContext
{
    protected virtual Guid? CurrentTenantId => CurrentTenant?.Id;
    public IBiyaLazyServiceProvider LazyServiceProvider { get; set; } = default!;
    
    public ICurrentTenant CurrentTenant => LazyServiceProvider.LazyGetRequiredService<ICurrentTenant>();
}