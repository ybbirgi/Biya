using Biya.DependencyInjection;
using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace Biya.EntityFrameworkCore;

public class BiyaDbContext<TDbContext> : DbContext where TDbContext : DbContext
{
    protected virtual Guid? CurrentTenantId => CurrentTenant?.Id;
    public IBiyaLazyServiceProvider LazyServiceProvider { get; set; } = default!;
    
    public ICurrentTenant CurrentTenant => LazyServiceProvider.LazyGetRequiredService<ICurrentTenant>();
    
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<Neighborhood> Neighborhoods { get; set; }
    
    public BiyaDbContext(DbContextOptions<TDbContext> options) 
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BiyaDbContext<TDbContext>).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TDbContext).Assembly);
    }
}