using Biya.EntityFrameworkCore.Extensions;
using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biya.EntityFrameworkCore.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(
        EntityTypeBuilder<Tenant> builder
    )
    {
        builder.ConfigureByConvention(); 
        
        builder.ToTable(builder.GetTableName(), CommonDatabaseConstants.CommonDatabaseSchema);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(Domain.Entities.PropertyConfigurationConsts.LookupNameLenght);

        builder.HasOne(t => t.Country)
            .WithMany()
            .HasForeignKey(t => t.CountryCode)
            .HasPrincipalKey(c => c.Code)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.City)
            .WithMany()
            .HasForeignKey(t => t.CityCode)
            .HasPrincipalKey(c => c.Code)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.District)
            .WithMany()
            .HasForeignKey(t => t.DistrictCode)
            .HasPrincipalKey(d => d.Code)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(t => t.Neighborhood)
            .WithMany()
            .HasForeignKey(t => t.NeighborhoodCode)
            .HasPrincipalKey(n => n.Code)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(t => t.OrganizationUnits)
            .WithOne(ou => ou.Tenant)
            .HasForeignKey(ou => ou.TenantId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}