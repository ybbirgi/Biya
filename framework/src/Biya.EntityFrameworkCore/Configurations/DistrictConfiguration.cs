using Biya.EntityFrameworkCore.Extensions;
using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biya.EntityFrameworkCore.Configurations;

public class DistrictConfiguration : IEntityTypeConfiguration<District>
{
    public void Configure(
        EntityTypeBuilder<District> builder
    )
    {
        builder.ConfigureByConvention();
        builder.HasOne(c => c.City)
            .WithMany(c => c.Districts)
            .HasForeignKey(c => c.CityCode)
            .HasPrincipalKey(c => c.Code)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => c.Code).IsUnique();
        builder.Property(c => c.Name).HasMaxLength(128);
    }
}