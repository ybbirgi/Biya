using Biya.EntityFrameworkCore.Extensions;
using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biya.EntityFrameworkCore.Configurations;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(
        EntityTypeBuilder<City> builder
    )
    {
        builder.ConfigureByConvention();
        builder.ToTable(builder.GetTableName(), CommonDatabaseConstants.CommonDatabaseSchema);
        
        builder.HasOne(c => c.Country)
            .WithMany(c => c.Cities)
            .HasForeignKey(c => c.CountryCode)
            .HasPrincipalKey(c => c.Code)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => c.Code).IsUnique();

        builder.Property(c => c.Name).HasMaxLength(128);
    }
}