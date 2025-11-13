using Biya.EntityFrameworkCore.Extensions;
using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biya.EntityFrameworkCore.Configurations;

public class OrganizationUnitConfiguration : IEntityTypeConfiguration<OrganizationUnit>
{
    public void Configure(
        EntityTypeBuilder<OrganizationUnit> builder
    )
    {
        builder.ConfigureByConvention();
        builder.ToTable(builder.GetTableName(), CommonDatabaseConstants.CommonDatabaseSchema);
        
        builder.Property(ou => ou.Name)
            .IsRequired()
            .HasMaxLength(Domain.Entities.PropertyConfigurationConsts.LookupNameLenght);

        builder.Property(ou => ou.PrimaryCode).HasMaxLength(64).IsRequired(false);
        builder.Property(ou => ou.SecondaryCode).HasMaxLength(64).IsRequired(false);

        builder.HasOne(ou => ou.Parent)
            .WithMany(ou => ou.Childs)
            .HasForeignKey(ou => ou.ParentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ou => ou.Tenant)
            .WithMany(t => t.OrganizationUnits)
            .HasForeignKey(ou => ou.TenantId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(ou => ou.Country)
            .WithMany()
            .HasForeignKey(ou => ou.CountryCode)
            .HasPrincipalKey(c => c.Code)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(ou => ou.City)
            .WithMany()
            .HasForeignKey(ou => ou.CityCode)
            .HasPrincipalKey(c => c.Code)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ou => ou.District)
            .WithMany()
            .HasForeignKey(ou => ou.DistrictCode)
            .HasPrincipalKey(d => d.Code)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(ou => ou.Neighborhood)
            .WithMany()
            .HasForeignKey(ou => ou.NeighborhoodCode)
            .HasPrincipalKey(n => n.Code)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}