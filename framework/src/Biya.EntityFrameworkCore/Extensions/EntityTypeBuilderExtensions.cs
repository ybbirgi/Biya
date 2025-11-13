using Domain.Entities;
using Domain.Entities.Common;
using Domain.Entities.ObjectExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biya.EntityFrameworkCore.Extensions;

public static class EntityTypeBuilderExtensions
{
   public static void ConfigureByConvention(this EntityTypeBuilder b)
    {
        b.TryConfigureConcurrencyStamp();
        b.TryConfigureMayHaveCreator();
        b.TryConfigureSoftDelete();
        b.TryConfigureDeletionTime();
        b.TryConfigureDeletionAudited();
        b.TryConfigureCreationTime();
        b.TryConfigureLastModificationTime();
        b.TryConfigureModificationAudited();
        b.TryConfigureMultiTenant();
        b.TryConfigureLookupEntity();
        b.TryConfigureIsActive();
    }
    public static void TryConfigureConcurrencyStamp(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IHasConcurrencyStamp>())
        {
            b.Property(nameof(IHasConcurrencyStamp.ConcurrencyStamp))
                .IsConcurrencyToken()
                .HasMaxLength(PropertyConfigurationConsts.ConcurrencyStampMaxLength)
                .HasColumnName(nameof(IHasConcurrencyStamp.ConcurrencyStamp));
        }
    }
    public static void TryConfigureIsActive(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IHasIsActive>())
        {
            b.Property(nameof(IHasIsActive.IsActive))
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName(nameof(IHasIsActive.IsActive));
        }
    }
    
    
    public static void TryConfigureLookupEntity(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<ILookup>())
        {
            b.Property(nameof(ILookup.Name))
                .IsRequired()
                .HasMaxLength(PropertyConfigurationConsts.LookupNameLenght)
                .HasColumnName(nameof(ILookup.Name));
            
            b.Property(nameof(ILookup.Code))
                .IsRequired()
                .ValueGeneratedOnAdd()
                .HasColumnName(nameof(ILookup.Code));
        }
    }

    public static void TryConfigureSoftDelete(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<ISoftDelete>())
        {
            b.Property(nameof(ISoftDelete.IsDeleted))
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName(nameof(ISoftDelete.IsDeleted));
        }
    }

    public static void TryConfigureDeletionTime(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IHasDeletionTime>())
        {
            b.TryConfigureSoftDelete();

            b.Property(nameof(IHasDeletionTime.DeletionTime))
                .IsRequired(false)
                .HasColumnName(nameof(IHasDeletionTime.DeletionTime));
        }
    }

    public static void TryConfigureMayHaveCreator(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IMayHaveCreator>())
        {
            b.Property(nameof(IMayHaveCreator.CreatorId))
                .IsRequired(false)
                .HasColumnName(nameof(IMayHaveCreator.CreatorId));
        }
    }
    public static void TryConfigureDeletionAudited(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IDeletionAuditedObject>())
        {
            b.TryConfigureDeletionTime();

            b.Property(nameof(IDeletionAuditedObject.DeleterId))
                .IsRequired(false)
                .HasColumnName(nameof(IDeletionAuditedObject.DeleterId));
        }
    }

    public static void TryConfigureCreationTime(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IHasCreationTime>())
        {
            b.Property(nameof(IHasCreationTime.CreationTime))
                .IsRequired()
                .HasColumnName(nameof(IHasCreationTime.CreationTime));
        }
    }

    public static void TryConfigureLastModificationTime(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IHasModificationTime>())
        {
            b.Property(nameof(IHasModificationTime.LastModificationTime))
                .IsRequired(false)
                .HasColumnName(nameof(IHasModificationTime.LastModificationTime));
        }
    }

    public static void TryConfigureModificationAudited(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IModificationAuditedObject>())
        {
            b.TryConfigureLastModificationTime();

            b.Property(nameof(IModificationAuditedObject.LastModifierId))
                .IsRequired(false)
                .HasColumnName(nameof(IModificationAuditedObject.LastModifierId));
        }
    }

    public static void TryConfigureMultiTenant(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IMultiTenant>())
        {
            b.Property(nameof(IMultiTenant.TenantId))
                .IsRequired(false)
                .HasColumnName(nameof(IMultiTenant.TenantId));
        }
    }
    
    public static void IgnoreCommonEntities(this ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<Tenant>();
        modelBuilder.Ignore<OrganizationUnit>();
        modelBuilder.Ignore<Country>();
        modelBuilder.Ignore<City>();
        modelBuilder.Ignore<District>();
        modelBuilder.Ignore<Neighborhood>();
    }
}