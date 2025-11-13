using Biya.EntityFrameworkCore.Extensions;
using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biya.EntityFrameworkCore.Configurations;

public class NeighborhoodConfiguration : IEntityTypeConfiguration<Neighborhood>
{
    public void Configure(
        EntityTypeBuilder<Neighborhood> builder
    )
    {
        builder.ConfigureByConvention();
        builder.ToTable(builder.GetTableName(), CommonDatabaseConstants.CommonDatabaseSchema);

        builder.HasOne<District>()
            .WithMany(d=> d.Neighborhoods) 
            .HasForeignKey(n => n.DistrictCode)
            .HasPrincipalKey(d => d.Code)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(n => n.Code).IsUnique();
        
        builder.Property(n => n.Name).HasMaxLength(128); 
    }
}