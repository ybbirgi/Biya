using Biya.EntityFrameworkCore.Extensions;
using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biya.EntityFrameworkCore.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(
        EntityTypeBuilder<Country> builder
    )
    {
        builder.ConfigureByConvention();
        builder.HasIndex(c => c.Code).IsUnique();
        builder.Property(c => c.Name).HasMaxLength(64);
    }
}