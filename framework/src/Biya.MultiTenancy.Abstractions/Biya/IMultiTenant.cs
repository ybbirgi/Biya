namespace Biya;

public interface IMultiTenant
{
    Guid? TenantId { get; set; }
}